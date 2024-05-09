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

namespace Talento_API.Controllers
{
    [Route("api/UserShift")]
    [ApiController]
    public class UserShiftController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;
        public UserShiftController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }

       
         [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetShifts(string searchString)
        {
            try
            {
                IQueryable<UserShift> userShifts = _db.UserShifts
                    .Include(d => d.User)
                    .Include(d => d.Shift);

                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    userShifts = userShifts.Where(u =>
                        u.UserId.ToLower().Contains(searchString) ||
                        u.User.Name.ToLower().Contains(searchString) ||
                        u.User.UserName.ToLower().Contains(searchString) ||
                        u.Shift.ShiftName.ToLower().Contains(searchString));
                }

                var result = await userShifts.Select(d => _mapper.Map<UserShiftDTO>(d)).ToListAsync();

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
        [HttpGet("{id:int}", Name = "GetuserShift")]
        public async Task<IActionResult> GetuserShift(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                UserShift userShift = _db.UserShifts.FirstOrDefault(u => u.Id == id);
                if (userShift == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = userShift;
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


        [HttpGet("ByUserId/{userId}")]
        public async Task<ActionResult<ApiResponse>> GetUserShiftsByUserId(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                IQueryable<UserShift> userShifts = _db.UserShifts
                    .Include(d => d.User)
                    .Include(d => d.Shift)
                    .Where(u => u.UserId == userId);

                var result = await userShifts.Select(d => _mapper.Map<UserShiftDTO>(d)).ToListAsync();

                _response.Result = result;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateuserShift([FromForm] UserShiftCreateDTO userShiftCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserShift userShiftToCreate = _mapper.Map<UserShift>(userShiftCreateDTO);
                  

                    _db.UserShifts.Add(userShiftToCreate);
                    await _db.SaveChangesAsync();

                    _response.Result = _mapper.Map<UserShiftUpdateDTO>(userShiftToCreate);
                    _response.StatusCode = HttpStatusCode.Created;

                    return CreatedAtRoute("GetUserShift", new { id = userShiftToCreate.Id }, _response);
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
        public async Task<ActionResult<ApiResponse>> UpdateUserShift(int id, [FromForm] UserShiftUpdateDTO userShiftUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userShiftUpdateDTO == null || id != userShiftUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    UserShift userShiftFromDb = await _db.UserShifts.FindAsync(id);
                    if (userShiftFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    _mapper.Map(userShiftUpdateDTO, userShiftFromDb);

                    _db.UserShifts.Update(userShiftFromDb);
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
        public async Task<ActionResult<ApiResponse>> DeleteUserShift(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                UserShift userShiftFromDb = await _db.UserShifts.FindAsync(id);
                if (userShiftFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.UserShifts.Remove(userShiftFromDb); 
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

        [HttpGet("Shift")]
        public async Task<ActionResult<ApiResponse>> GetShifts()
        {
            try
            {
                var shifts = await _db.Shifts
                    .Select(d => new
                    {
                        ShiftId = d.Id,
                        shiftName = d.ShiftName,
                       
                    })
                    .ToListAsync();

                _response.Result = shifts;
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


      


    }
}
