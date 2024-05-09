using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactUser.Data;
using ReactUser.Models.Dto;
using ReactUser.Models;
using ReactUser.Utility;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace ReactUser.Controllers
{
    [Route("api/AttendenceUser")]
    [ApiController]
    public class AttendenceUserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        private ApiResponse _response;
        public AttendenceUserController(ApplicationDbContext db)
        {
            _db = db;
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
            .Where(d => d.ClockOutTime > DateTime.MinValue)
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
      
        private int ConvertToTotalMinutes(int hours, int minutes)
        {
            return hours * 60 + minutes;
        }

        [HttpGet("Aggregated")]
        public async Task<ActionResult<ApiResponse>> GetAggregatedAttendences(string searchString)
        {
            try
            {
                var attendences = _db.AttendenceUsers
                    .Include(d => d.User)
                    .Where(d => d.ClockOutTime > DateTime.MinValue)
                    .Where(u => string.IsNullOrEmpty(searchString) ||
                                u.EmployeeId.ToLower().Contains(searchString.ToLower()) ||
                                u.User.Name.ToLower().Contains(searchString.ToLower()) ||
                                u.User.UserName.ToLower().Contains(searchString.ToLower()))
                    .ToList();

                var aggregatedResult = attendences
                    .GroupBy(d => new { d.User.UserName, d.WorkingDate, d.User.Name, d.EmployeeId })
                    .Select(g => new
                    {
                        Name = g.Key.Name, 
                        UserName = g.Key.UserName,
                        WorkingDate = g.Key.WorkingDate,
                        EmployeeId = g.Key.EmployeeId,
                        TotalMinutes = g.Sum(a => ConvertToTotalMinutes(a.WorkedHours))
                    })
                    .ToList()
                    .Select(x => new
                    {
                        x.UserName,
                        x.Name,
                        x.WorkingDate,
                        x.EmployeeId,
                        TotalWorkedHours = x.TotalMinutes / 60,
                        TotalWorkedMinutes = x.TotalMinutes % 60,
                        ShiftName = GetShiftNameForEmployee(x.EmployeeId, x.WorkingDate)
                    })
                    .ToList();

                _response.Result = aggregatedResult;
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

        private string GetShiftNameForEmployee(string employeeId, DateTime workingDate)
        {
            var userShift = _db.UserShifts
                .Include(us => us.Shift)
                .FirstOrDefault(us => us.UserId == employeeId &&
                                      us.ShiftDateFrom <= workingDate &&
                                      us.ShiftDateTo >= workingDate);

            return userShift?.Shift?.ShiftName ?? "Unknown";
        }

        private int ConvertToTotalMinutes(string workedHours)
        {
            if (workedHours.Contains("hours") && workedHours.Contains("minutes"))
            {
                var hours = int.Parse(workedHours.Split(" hours ")[0]);
                var minutes = int.Parse(workedHours.Split(" hours ")[1].Split(" minutes")[0]);
                return hours * 60 + minutes;
            }
            else
            {
                return int.Parse(workedHours);
            }
        }




    }
}
