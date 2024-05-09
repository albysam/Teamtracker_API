using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactUser.Data;
using ReactUser.Models;
using ReactUser.Models.Dto;
using ReactUser.Services;
using ReactUser.Utility;
using System.Net;
using System.Net.NetworkInformation;
using Talento_API.Models;
using Talento_API.Models.Dto;

namespace Talento_API.Controllers
{
    [Route("api/WorkingDay")]
    [ApiController]
    public class WorkingDayController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;

        public WorkingDayController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetWorkingDays(string searchString)
        {
            try
            {
                IEnumerable<WorkingDay> WorkingDays = _db.WorkingDays;

                

                _response.Result = WorkingDays.ToList(); 
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

        [HttpGet("{id:int}", Name = "GetWorkingDay")]
        public async Task<IActionResult> GetWorkingDay(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                WorkingDay WorkingDay = _db.WorkingDays.FirstOrDefault(u => u.Id == id);
                if (WorkingDay == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = WorkingDay;
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
        public async Task<ActionResult<ApiResponse>> CreateWorkingDay([FromForm] WorkingDayCreateDTO WorkingDayCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    WorkingDay WorkingDayToCreate = _mapper.Map<WorkingDay>(WorkingDayCreateDTO);

                    _db.WorkingDays.Add(WorkingDayToCreate);
                    _db.SaveChanges();
                    _response.Result = WorkingDayToCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetWorkingDay", new { id = WorkingDayToCreate.Id }, _response);
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
        public async Task<ActionResult<ApiResponse>> UpdateWorkingDay(int id, [FromForm] WorkingDayUpdateDTO WorkingDayUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (WorkingDayUpdateDTO == null || id != WorkingDayUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    WorkingDay WorkingDayFromDb = await _db.WorkingDays.FindAsync(id);
                    if (WorkingDayFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    _mapper.Map(WorkingDayUpdateDTO, WorkingDayFromDb);

                    _db.WorkingDays.Update(WorkingDayFromDb);
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
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteWorkingDay(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                WorkingDay WorkingDayFromDb = await _db.WorkingDays.FindAsync(id);
                if (WorkingDayFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.WorkingDays.Remove(WorkingDayFromDb); 
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
