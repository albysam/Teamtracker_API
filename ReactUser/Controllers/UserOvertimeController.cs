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
    [Route("api/UserOvertime")]
    [ApiController]
    public class UserOvertimeController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;
        public UserOvertimeController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetUserOvertime(string searchString)
        {
            try
            {
                IQueryable<UserOvertime> userOvertimes = _db.UserOvertimes
                    .Include(d => d.User);


                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    userOvertimes = userOvertimes.Where(u =>
                        u.UserId.ToLower().Contains(searchString) ||
                        u.User.Name.ToLower().Contains(searchString) ||
                        u.User.UserName.ToLower().Contains(searchString));
                }

                var result = await userOvertimes.Select(d => _mapper.Map<UserOvertimeDTO>(d)).ToListAsync();

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
        [HttpGet("{id:int}", Name = "GetuserOvertime")]
        public async Task<IActionResult> GetuserOvertime(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                UserOvertime userOvertime = _db.UserOvertimes.FirstOrDefault(u => u.Id == id);
                if (userOvertime == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = userOvertime;
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
        public async Task<ActionResult<ApiResponse>> CreateuserOvertime([FromForm] UserOvertimeCreateDTO userOvertimeCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   

                    UserOvertime userOvertimeToCreate = _mapper.Map<UserOvertime>(userOvertimeCreateDTO);
                    userOvertimeToCreate.Status = SD.Overtime_Pending;

                    _db.UserOvertimes.Add(userOvertimeToCreate);
                    await _db.SaveChangesAsync();

                    _response.Result = _mapper.Map<UserOvertimeUpdateDTO>(userOvertimeToCreate);
                    _response.StatusCode = HttpStatusCode.Created;

                    return CreatedAtRoute("GetUserOvertime", new { id = userOvertimeToCreate.Id }, _response);
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
        public async Task<ActionResult<ApiResponse>> UpdateUserOvertime(int id, [FromForm] UserOvertimeUpdateDTO userOvertimeUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userOvertimeUpdateDTO == null || id != userOvertimeUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    UserOvertime userOvertimeFromDb = await _db.UserOvertimes.FindAsync(id);
                    if (userOvertimeFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    _mapper.Map(userOvertimeUpdateDTO, userOvertimeFromDb);

                    _db.UserOvertimes.Update(userOvertimeFromDb);
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
        public async Task<ActionResult<ApiResponse>> DeleteUserOvertime(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                UserOvertime userOvertimeFromDb = await _db.UserOvertimes.FindAsync(id);
                if (userOvertimeFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.UserOvertimes.Remove(userOvertimeFromDb); 
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
