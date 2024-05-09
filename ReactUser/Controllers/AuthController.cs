using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReactUser.Data;
using ReactUser.Models.Dto;
using ReactUser.Models;
using Microsoft.IdentityModel.Tokens;
using ReactUser.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ReactUser.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        public AuthController(ApplicationDbContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _response = new ApiResponse();
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            try
            {
                ApplicationUser userFromDb = _db.ApplicationUsers
                    .FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

                if (userFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Username or password is incorrect");
                    return BadRequest(_response);
                }
                // Check if the user is blocked (status == 1)
                if (userFromDb.Status == 1)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("You are blocked. Please contact HR.");
                    return BadRequest(_response);
                }
                bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);

                if (!isValid)
                {
                    _response.Result = new LoginResponseDTO();
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Username is incorrect");
                    return BadRequest(_response);
                }

                //Generate JWT Token
                var roles = await _userManager.GetRolesAsync(userFromDb);

                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.ASCII.GetBytes(secretKey);

                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim("fullName", userFromDb.Name),
                new Claim("id", userFromDb.Id.ToString()),
                new Claim(ClaimTypes.Email, userFromDb.UserName.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                new Claim("department", userFromDb.Department.ToString()),

                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                LoginResponseDTO loginResponse = new()
                {
                    Email = userFromDb.Email,
                    Token = tokenHandler.WriteToken(token)
                };

                if (string.IsNullOrEmpty(loginResponse.Email) || string.IsNullOrEmpty(loginResponse.Token))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Username or password is incorrect");
                    return BadRequest(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = loginResponse;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("An error occurred while processing the request.");
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            ApplicationUser userFromDb = _db.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

            if (userFromDb != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            ApplicationUser newUser = new()
            {
                UserName = model.UserName,
                Email = model.UserName,
                NormalizedEmail = model.UserName.ToUpper(),
                Name = model.Name,
                Roles = model.Role
            };

            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        //create roles in database
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_SuperAdmin));
                    }
                    if (model.Role.ToLower() == SD.Role_Admin)
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                    }
                    else if (model.Role.ToLower() == SD.Role_SuperAdmin)
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_SuperAdmin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Employee);
                    }

                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
            }
            catch (Exception)
            {

            }
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Error while registering");
            return BadRequest(_response);

        }

        [HttpGet("list-users")]
        [Authorize(Roles = SD.Role_SuperAdmin + "," + SD.Role_Admin)]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _db.ApplicationUsers.Select(u => new UserListDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    UserName = u.UserName,
                    Email = u.Email,
                    Department = u.Department,
                    Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault()
                }).ToList();

                _response.Result = users;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add($"Error while listing users: {ex.Message}");
                return BadRequest(_response);
            }
        }

        [HttpGet("{id}", Name = "GetUsers")]
        public IActionResult GetUser(string id)
        {
            try
            {
                if (id == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                ApplicationUser applicationUser = _db.Users.FirstOrDefault(u => u.Id == id);

                if (applicationUser == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                var userDTO = new UserListDTO
                {
                    Id = applicationUser.Id,
                    Name = applicationUser.Name,
                    UserName = applicationUser.UserName,
                    Email = applicationUser.Email,
                    Role = _userManager.GetRolesAsync(applicationUser).Result.FirstOrDefault()
                };

                _response.Result = userDTO;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }






        

        [HttpPut("update/{id}")]
        [Authorize(Roles = SD.Role_Admin)] 
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequestDTO model)
        {
            try
            {
                if (id == null || model == null || id != model.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                ApplicationUser userFromDb = await _userManager.FindByIdAsync(id);

                if (userFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User not found");
                    return BadRequest(_response);
                }

                // Update user properties
                userFromDb.UserName = model.UserName;
                userFromDb.Name = model.Name;

              

                var result = await _userManager.UpdateAsync(userFromDb);

                if (result.Succeeded)
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
                else
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Error while updating user");
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add($"Error while updating user: {ex.Message}");
                return BadRequest(_response);
            }
        }


    }
}

