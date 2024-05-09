using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactUser.Data;
using ReactUser.Models.Dto;
using ReactUser.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ReactUser.Utility;
using AutoMapper;

namespace ReactUser.Controllers
{
    [Route("api/UserLeave")]
    [ApiController]
    public class UserLeaveController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;
        public UserLeaveController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }

       
         [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetLeaves(string searchString)
        {
            try
            {
                IQueryable<UserLeave> userLeaves = _db.UserLeaves
                    .Include(d => d.User)
                    .Include(d => d.Leave);

                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    userLeaves = userLeaves.Where(u =>
                        u.UserId.ToLower().Contains(searchString) ||
                        u.User.Name.ToLower().Contains(searchString) ||
                        u.User.UserName.ToLower().Contains(searchString) ||
                        u.Leave.LeaveName.ToLower().Contains(searchString));
                }

                var result = await userLeaves.Select(d => _mapper.Map<UserLeaveDTO>(d)).ToListAsync();

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
        [HttpGet("{id:int}", Name = "GetuserLeave")]
        public async Task<IActionResult> GetuserLeave(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                UserLeave userLeave = _db.UserLeaves.FirstOrDefault(u => u.Id == id);
                if (userLeave == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = userLeave;
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
        public async Task<ActionResult<ApiResponse>> CreateuserLeave([FromForm] UserLeaveCreateDTO userLeaveCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Fetch Leave
                    var leave = await _db.Leaves.FindAsync(userLeaveCreateDTO.LeaveId);

                    if (leave == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string>() { "Invalid Leave Id" };
                        return BadRequest(_response);
                    }

                  
                    if (!int.TryParse(leave.PaidLeaveDays, out int paidLeaveDays))
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string>() { "Invalid Paid Leave Days" };
                        return BadRequest(_response);
                    }

                    
                    if (!int.TryParse(userLeaveCreateDTO.LeaveDays, out int leaveDays))
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string>() { "Invalid Leave Days" };
                        return BadRequest(_response);
                    }

                   
                    if (leaveDays > paidLeaveDays)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string>() { "You cannot apply for more leave than the allowed paid leave days" };
                        return BadRequest(_response);
                    }

                    UserLeave userLeaveToCreate = _mapper.Map<UserLeave>(userLeaveCreateDTO);
                    userLeaveToCreate.Status = SD.Leave_Pending;

                    _db.UserLeaves.Add(userLeaveToCreate);
                    await _db.SaveChangesAsync();

                    _response.Result = _mapper.Map<UserLeaveUpdateDTO>(userLeaveToCreate);
                    _response.StatusCode = HttpStatusCode.Created;

                    return CreatedAtRoute("GetUserLeave", new { id = userLeaveToCreate.Id }, _response);
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
        public async Task<ActionResult<ApiResponse>> UpdateUserLeave(int id, [FromForm] UserLeaveUpdateDTO userLeaveUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userLeaveUpdateDTO == null || id != userLeaveUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    UserLeave userLeaveFromDb = await _db.UserLeaves.FindAsync(id);
                    if (userLeaveFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    _mapper.Map(userLeaveUpdateDTO, userLeaveFromDb);

                    _db.UserLeaves.Update(userLeaveFromDb);
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
        public async Task<ActionResult<ApiResponse>> DeleteUserLeave(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                UserLeave userLeaveFromDb = await _db.UserLeaves.FindAsync(id);
                if (userLeaveFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.UserLeaves.Remove(userLeaveFromDb); 
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

        [HttpGet("Leaves")]
        public async Task<ActionResult<ApiResponse>> GetLeaves()
        {
            try
            {
                var leaves = await _db.Leaves
                    .Select(d => new
                    {
                        LeaveId = d.Id,
                        leaveName = d.LeaveName,
                        paidLeaveDays = d.PaidLeaveDays,
                    })
                    .ToListAsync();

                _response.Result = leaves;
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
