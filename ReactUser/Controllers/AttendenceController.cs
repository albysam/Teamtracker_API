using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactUser.Data;
using ReactUser.Models.Dto;
using ReactUser.Models;
using ReactUser.Utility;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ReactUser.Controllers
{
    [Route("api/Attendence")]
    [ApiController]
    public class AttendenceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper; 
        private ApiResponse _response;
      

        public AttendenceController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAttendences(string searchString)
        {
            try
            {
                IEnumerable<AttendenceUser> attendences = _db.AttendenceUsers
                .Include(d => d.User)

                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    attendences = attendences
                        .Where(u => u.EmployeeId.ToLower().Contains(searchString.ToLower()) ||
                            u.User.Name.ToLower().Contains(searchString.ToLower()) ||
                            u.User.UserName.ToLower().Contains(searchString.ToLower()));


                }

                var result = attendences
            .Where(d => d.ClockOutTime == DateTime.MinValue) 
            .Select(d => new
            {
                    Id = d.Id,
                    EmployeeId = d.EmployeeId,
                    Name = d.User.Name,
                    UserName = d.User.UserName,

                    ClockInTime = d.ClockInTime,
                    ClockOutTime = d.ClockOutTime,
                    BreakStartTime = d.BreakStartTime,
                    BreakEndTime = d.BreakEndTime,
                    WorkingDate = d.WorkingDate,
                    WorkedHours = d.WorkedHours,
                    BreakHours = d.BreakHours,
                    Status = d.Status,

                }).ToList();

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
        [HttpGet("{id:int}", Name = "Getattendence")]
        public async Task<IActionResult> Getattendence(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                AttendenceUser attendence = _db.AttendenceUsers.FirstOrDefault(u => u.Id == id);
                if (attendence == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = attendence;
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
        public async Task<ActionResult<ApiResponse>> CreateAttendence([FromForm] AttendenceCreateDTO attendenceCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AttendenceUser attendenceToCreate = new()
                    {
                        EmployeeId = attendenceCreateDTO.EmployeeId,
                        ClockInTime = attendenceCreateDTO.ClockInTime,
                        WorkingDate = attendenceCreateDTO.WorkingDate,
                        WorkedHours = "0",
                        BreakHours = "0",
                        Status = 0,
                    };
                    _db.AttendenceUsers.Add(attendenceToCreate);
                    await _db.SaveChangesAsync(); 
                    _response.Result = attendenceToCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetAttendence", new { id = attendenceToCreate.Id }, _response);
                }
                else
                {
                    _response.IsSuccess = false;
                  
                    _response.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                                .Select(e => e.ErrorMessage)
                                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message }; 
            }
            return _response;
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateAttendence(int id, [FromForm] AttendenceUpdateDTO attendenceUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (attendenceUpdateDTO == null || id != attendenceUpdateDTO.Id)
                    {
                        return BadRequest();
                    }

                    AttendenceUser attendenceFromDb = await _db.AttendenceUsers.FindAsync(id);
                    if (attendenceFromDb == null)
                    {
                        return BadRequest();
                    }

                  
                    _mapper.Map(attendenceUpdateDTO, attendenceFromDb);

                  
                    _db.AttendenceUsers.Update(attendenceFromDb);
                    await _db.SaveChangesAsync();

                    return NoContent();
                }
                else
                {
                    return BadRequest(ModelState.Values.SelectMany(v => v.Errors)
                                                      .Select(e => e.ErrorMessage)
                                                      .ToList());
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

       
    }
}

       