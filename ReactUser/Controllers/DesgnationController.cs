using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactUser.Data;
using ReactUser.Migrations;
using ReactUser.Models;
using ReactUser.Models.Dto;
using ReactUser.Services;
using ReactUser.Utility;
using System.Net;
using static Azure.Core.HttpHeader;

namespace ReactUser.Controllers
{
    [Route("api/Desgnation")]
    [ApiController]
    public class DesgnationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        private ApiResponse _response;
        public DesgnationController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetDesgnations(string searchString)
        {
            try
            {
                IEnumerable<Desgnation> desgnations = _db.DesgnationsType
                    .Include(d => d.Department)  
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    desgnations = desgnations
                        .Where(d => d.DesgnationName.ToLower().Contains(searchString.ToLower()));
                }

               
                var result = desgnations.Select(d => new
                {
                    Id = d.Id,
                    DesgnationName = d.DesgnationName,
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.Department.DepartmentName  
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

        [HttpGet("{id:int}", Name = "GetDesgnations")]
        public async Task<IActionResult> GetDesgnations(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                // Include the Department
                Desgnation desgnation = _db.DesgnationsType
                    .Include(d => d.Department)
                    .FirstOrDefault(u => u.Id == id);

                if (desgnation == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                var result = new
                {
                    Id = desgnation.Id,
                    DesgnationName = desgnation.DesgnationName,
                    DepartmentId = desgnation.DepartmentId,
                    DepartmentName = desgnation.Department.DepartmentName
                };

                _response.Result = result;
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
        public async Task<ActionResult<ApiResponse>> CreateDesgnation([FromForm] DesgnationCreateDTO desgnationCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    Desgnation desgnationToCreate = new()
                    {
                        DepartmentId = desgnationCreateDTO.DepartmentId,
                        DesgnationName = desgnationCreateDTO.DesgnationName
                        
                    };

                   
                    _db.DesgnationsType.Add(desgnationToCreate);
                    _db.SaveChanges();

                   
                    desgnationToCreate = _db.DesgnationsType
                        .Include(d => d.Department)
                        .FirstOrDefault(d => d.Id == desgnationToCreate.Id);

                   
                    var result = new
                    {
                        Id = desgnationToCreate.Id,
                        DesgnationName = desgnationToCreate.DesgnationName,
                        DepartmentId = desgnationToCreate.DepartmentId,
                      //DepartmentName = desgnationToCreate.Department?.DepartmentName 
                    };

                    _response.Result = result;
                    _response.StatusCode = HttpStatusCode.Created;

                   
                    return CreatedAtRoute("GetDesgnations", new { id = desgnationToCreate.Id }, _response);
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
        public async Task<ActionResult<ApiResponse>> UpdateDesgnation(int id, [FromForm] DesgnationUpdateDTO desgnationUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (desgnationUpdateDTO == null || id != desgnationUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    Desgnation desgnationFromDb = await _db.DesgnationsType.FindAsync(id);
                    if (desgnationFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }
                    desgnationFromDb.DepartmentId = desgnationUpdateDTO.DepartmentId;
                    desgnationFromDb.DesgnationName = desgnationUpdateDTO.DesgnationName;
                  




                    _db.DesgnationsType.Update(desgnationFromDb);
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
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }

            return _response;
        }

      

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteDesgnation(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                Desgnation desgnationFromDb = await _db.DesgnationsType.FindAsync(id);
                if (desgnationFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.DesgnationsType.Remove(desgnationFromDb); 
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

        

            [HttpGet("Departments")]
            public async Task<ActionResult<ApiResponse>> GetDepartments()
            {
                try
                {
                    var departments = await _db.Departments
                        .Select(d => new
                        {
                            DepartmentId = d.Id,
                            DepartmentName = d.DepartmentName
                        })
                        .ToListAsync();

                    _response.Result = departments;
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

        [HttpGet("Desgnations")]
        public async Task<ActionResult<ApiResponse>> GetDesgnationsType([FromQuery] int departmentId)
        {
            try
            {
                var desgnations = await _db.DesgnationsType
                    .Where(d => d.DepartmentId == departmentId) 
                    .Select(d => new
                    {
                        DesgnationId = d.Id,
                        DepartmentId = d.DepartmentId,
                        DesgnationName = d.DesgnationName,
                    })
                    .ToListAsync();

                _response.Result = desgnations;
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
        [HttpGet("Shifts")]
        public async Task<ActionResult<ApiResponse>> GetShifts()
        {
            try
            {
                var shifts = await _db.Shifts
                    .Select(d => new
                    {
                        ShiftId = d.Id,
                        ShiftName = d.ShiftName,

                        StartTime = d.StartTime,
                        EndTime = d.EndTime,
                        ShiftDuration = d.ShiftDuration,
                        BreakDuration = d.BreakDuration,
                        Notes = d.Notes,
                        TotalShiftDuration = d.TotalShiftDuration,
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
