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
    [Route("api/Holiday")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;

        public HolidayController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetHolidays(string searchString)
        {
            try
            {
                IEnumerable<Holiday> Holidays = _db.Holidays;

                if (!string.IsNullOrEmpty(searchString))
                {
                    Holidays = Holidays
                        .Where(u => u.HolidayName.ToLower().Contains(searchString.ToLower()) 
                                    
                                    );
                }

                _response.Result = Holidays.ToList(); 
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

        [HttpGet("{id:int}", Name = "GetHoliday")]
        public async Task<IActionResult> GetHoliday(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                Holiday Holiday = _db.Holidays.FirstOrDefault(u => u.Id == id);
                if (Holiday == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = Holiday;
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
        public async Task<ActionResult<ApiResponse>> CreateHoliday([FromForm] HolidayCreateDTO HolidayCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Holiday HolidayToCreate = _mapper.Map<Holiday>(HolidayCreateDTO);

                    _db.Holidays.Add(HolidayToCreate);
                    _db.SaveChanges();
                    _response.Result = HolidayToCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetHoliday", new { id = HolidayToCreate.Id }, _response);
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
        public async Task<ActionResult<ApiResponse>> UpdateHoliday(int id, [FromForm] HolidayUpdateDTO HolidayUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (HolidayUpdateDTO == null || id != HolidayUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    Holiday HolidayFromDb = await _db.Holidays.FindAsync(id);
                    if (HolidayFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    _mapper.Map(HolidayUpdateDTO, HolidayFromDb);

                    _db.Holidays.Update(HolidayFromDb);
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
        public async Task<ActionResult<ApiResponse>> DeleteHoliday(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                Holiday HolidayFromDb = await _db.Holidays.FindAsync(id);
                if (HolidayFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.Holidays.Remove(HolidayFromDb); 
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
