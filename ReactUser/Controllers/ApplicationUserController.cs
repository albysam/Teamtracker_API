using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactUser.Data;
using ReactUser.Models.Dto;
using ReactUser.Models;
using ReactUser.Services;
using ReactUser.Utility;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Talento.Models;
using AutoMapper;
using System.Linq;

namespace ReactUser.Controllers
{
    [Route("api/ApplicationUser")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
       
            private readonly ApplicationDbContext _db;
            private readonly IBlobService _blobService;
            private ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper; 
        public ApplicationUserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IBlobService blobService, IMapper mapper) // Inject IMapper
        {
                _db = db;
                _blobService = blobService;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper; 
            _response = new ApiResponse();
            }

       
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetApplicationUsers(string searchString, int department, int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                IEnumerable<ApplicationUser> applicationUsers = _db.ApplicationUsers;

                
                if (User.IsInRole("admin") && department > 0)
                {
                    applicationUsers = applicationUsers.Where(u => u.Department == department);
                }

               

                if (!string.IsNullOrEmpty(searchString))
                {
                    applicationUsers = applicationUsers
                        .Where(u => u.UserName.ToLower().Contains(searchString.ToLower()) ||
                                    u.Email.ToLower().Contains(searchString.ToLower()) ||
                                    u.Roles.ToLower().Contains(searchString.ToLower()) ||
                                     u.Name.ToLower().Contains(searchString.ToLower()) ||
                                    u.Id.ToLower().Contains(searchString.ToLower()));
                }

               

                Pagination pagination = new()
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = applicationUsers.Count(),
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

                _response.Result = applicationUsers.Skip((pageNumber - 1) * pageSize).Take(pageSize);
               
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("Userschat")]
        public async Task<ActionResult<ApiResponse>> GetApplicationUserschat()
        {
            try
            {
                IEnumerable<ApplicationUser> applicationUsers = _db.ApplicationUsers;



                _response.Result = applicationUsers;

                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id}", Name = "GetApplicationUser")]
        public async Task<IActionResult> GetApplicationUser(string id)
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

                _response.Result = applicationUser;
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



        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateApplicationUser(string id, [FromForm] ApplicationUserUpdateDTO applicationUserUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (applicationUserUpdateDTO == null || id != applicationUserUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }

                    ApplicationUser applicationUserFromDb = await _db.Users.FindAsync(id);
                    if (applicationUserFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }

                
                    bool employeeIdExists = await _db.Users.AnyAsync(u => u.EmployeeId == applicationUserUpdateDTO.EmployeeId && u.Id != id);
                    if (employeeIdExists)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string> { "This Employee ID already exists." };
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_response);
                    }

                 
                    _mapper.Map(applicationUserUpdateDTO, applicationUserFromDb);

               
                    var existingRolesList = await _userManager.GetRolesAsync(applicationUserFromDb);
                    var applicationUserRolesList = applicationUserUpdateDTO.Roles.Split(',').ToList();
                    var rolesToAdd = applicationUserRolesList.Except(existingRolesList);
                    var rolesToRemove = existingRolesList.Except(applicationUserRolesList);

                    await _userManager.AddToRolesAsync(applicationUserFromDb, rolesToAdd);
                    await _userManager.RemoveFromRolesAsync(applicationUserFromDb, rolesToRemove);

                  
                    if (applicationUserUpdateDTO.File != null && applicationUserUpdateDTO.File.Length > 0)
                    {
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(applicationUserUpdateDTO.File.FileName)}";
                        if (applicationUserFromDb.Image != null)
                        {
                            await _blobService.DeleteBlob(applicationUserFromDb.Image.Split('/').Last(), SD.SD_Storage_Container);
                        }
                        applicationUserFromDb.Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, applicationUserUpdateDTO.File);
                    }

                
                    await _userManager.UpdateAsync(applicationUserFromDb);
                    _db.Users.Update(applicationUserFromDb);
                    await _db.SaveChangesAsync();

                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }





        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteApplicationUser(string id)
        {
            try
            {
                if (id == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                ApplicationUser applicationUserFromDb = await _db.Users.FindAsync(id);
                if (applicationUserFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

             
                if (!string.IsNullOrEmpty(applicationUserFromDb.Image))
                {
                    await _blobService.DeleteBlob(applicationUserFromDb.Image.Split('/').Last(), SD.SD_Storage_Container);
                    int milliseconds = 2000;
                    Thread.Sleep(milliseconds);
                }

                _db.Users.Remove(applicationUserFromDb);
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
        [HttpPut("{id}/Password")]
        public async Task<ActionResult<ApiResponse>> UpdateApplicationUserPassword(string id, [FromForm] ApplicationUserUpdateDTO applicationUserUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (applicationUserUpdateDTO == null || id != applicationUserUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    ApplicationUser applicationUserFromDb = await _db.Users.FindAsync(id);
                    if (applicationUserFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                   
                    _mapper.Map(applicationUserUpdateDTO, applicationUserFromDb);

                    // Check if a new password is provided and hash it
                    if (!string.IsNullOrEmpty(applicationUserUpdateDTO.PasswordHash))
                    {
                        var passwordHasher = new PasswordHasher<ApplicationUser>();
                        applicationUserFromDb.PasswordHash = passwordHasher.HashPassword(applicationUserFromDb, applicationUserUpdateDTO.PasswordHash);
                    }


                   
                    if (applicationUserUpdateDTO.File != null && applicationUserUpdateDTO.File.Length > 0)
                    {
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(applicationUserUpdateDTO.File.FileName)}";
                        if (applicationUserFromDb.Image != null)
                        {
                            await _blobService.DeleteBlob(applicationUserFromDb.Image.Split('/').Last(), SD.SD_Storage_Container);
                        }

                        applicationUserFromDb.Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, applicationUserUpdateDTO.File);
                    }

                    _db.Users.Update(applicationUserFromDb);
                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }


        [HttpPut("{id}/Block")]
        public async Task<ActionResult<ApiResponse>> BlockUser(string id)
        {
            try
            {
                if (id == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                ApplicationUser applicationUserFromDb = await _db.Users.FindAsync(id);
                if (applicationUserFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                applicationUserFromDb.Status = 1; // Blocked
                _db.Users.Update(applicationUserFromDb);
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("{id}/Unblock")]
        public async Task<ActionResult<ApiResponse>> UnblockUser(string id)
        {
            try
            {
                if (id == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                ApplicationUser applicationUserFromDb = await _db.Users.FindAsync(id);
                if (applicationUserFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                applicationUserFromDb.Status = 0; // Unblocked
                _db.Users.Update(applicationUserFromDb);
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }





        [HttpGet("SalaryDetails")]
        public async Task<IActionResult> GetSalaryDetails(string search, int? year, int? month)
        {
          
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

         
            year ??= currentYear;
            month ??= currentMonth;

           
            var query = _db.ApplicationUsers
                .Where(u => u.Email != "superadmin@nomail.com");

           
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Name.Contains(search) || u.Email.Contains(search) || (u.EmployeeId != null && u.EmployeeId.Contains(search)));
            }

           
            var userSalaryDetails = await query
                .Select(u => new
                {
                    Id = u.Id,
                    EmployeeId = u.EmployeeId,
                    Name = u.Name,
                    Email = u.Email,
                    MonthlySalary = u.MonthlySalary,
                    OvertimeHourlySalary = u.Overtime_hourly_Salary,
                    TotalUserOvertimes = _db.UserOvertimes
                        .Where(ot => ot.UserId == u.Id && ot.Status == 1 && ot.OvertimeDate.Year == year && ot.OvertimeDate.Month == month)
                        .Sum(ot => EF.Functions.DateDiffMinute(ot.OvertimeFrom, ot.OvertimeTo) / 60.0)
                })
                .ToListAsync();

           
            if (!userSalaryDetails.Any())
            {
                return NotFound("No users found.");
            }

            return Ok(userSalaryDetails);
        }


    }
}

