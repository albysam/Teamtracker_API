using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactUser.Data;
using ReactUser.Models.Dto;
using ReactUser.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ReactUser.Utility;
using AutoMapper;
using Talento_API.Models;
using Talento_API.Models.Dto;

namespace ReTalento_APIactUser.Controllers
{
    [Route("api/UserOnduty")]
    [ApiController]
    public class UserOndutyController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;
        public UserOndutyController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetUserOnduty(string searchString)
        {
            try
            {
                IQueryable<UserOnduty> userOndutys = _db.UserOndutys
                    .Include(d => d.User);


                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    userOndutys = userOndutys.Where(u =>
                        u.UserId.ToLower().Contains(searchString) ||
                        u.User.Name.ToLower().Contains(searchString) ||
                        u.User.UserName.ToLower().Contains(searchString));
                }

                var result = await userOndutys.Select(d => _mapper.Map<UserOndutyDTO>(d)).ToListAsync();

                _response.Result = result;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        [HttpGet("{id:int}", Name = "GetuserOnduty")]
        public async Task<IActionResult> GetuserOnduty(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                UserOnduty userOnduty = _db.UserOndutys.FirstOrDefault(u => u.Id == id);
                if (userOnduty == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = userOnduty;
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateuserOnduty([FromForm] UserOndutyCreateDTO userOndutyCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   

                    UserOnduty userOndutyToCreate = _mapper.Map<UserOnduty>(userOndutyCreateDTO);
                    userOndutyToCreate.Status = SD.Onduty_Pending;

                    _db.UserOndutys.Add(userOndutyToCreate);
                    await _db.SaveChangesAsync();

                    _response.Result = _mapper.Map<UserOndutyUpdateDTO>(userOndutyToCreate);
                    _response.StatusCode = HttpStatusCode.Created;

                    return CreatedAtRoute("GetUserOnduty", new { id = userOndutyToCreate.Id }, _response);
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


        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateUserOnduty(int id, [FromForm] UserOndutyUpdateDTO userOndutyUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userOndutyUpdateDTO == null || id != userOndutyUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    UserOnduty userOndutyFromDb = await _db.UserOndutys.FindAsync(id);
                    if (userOndutyFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    _mapper.Map(userOndutyUpdateDTO, userOndutyFromDb);

                    _db.UserOndutys.Update(userOndutyFromDb);
                    await _db.SaveChangesAsync();

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
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteUserOnduty(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                UserOnduty userOndutyFromDb = await _db.UserOndutys.FindAsync(id);
                if (userOndutyFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.UserOndutys.Remove(userOndutyFromDb); 
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
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


    }
    
}
