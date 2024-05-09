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
    [Route("api/Shift")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;

        public ShiftController(ApplicationDbContext db, IMapper mapper)
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
                IEnumerable<Shift> shifts = _db.Shifts;

                if (!string.IsNullOrEmpty(searchString))
                {
                    shifts = shifts
                        .Where(u => u.ShiftName.ToLower().Contains(searchString.ToLower()) 
                                    
                                    );
                }

                _response.Result = shifts.ToList(); 
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

        [HttpGet("{id:int}", Name = "Getshift")]
        public async Task<IActionResult> Getshift(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                Shift shift = _db.Shifts.FirstOrDefault(u => u.Id == id);
                if (shift == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = shift;
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
        public async Task<ActionResult<ApiResponse>> CreateShift([FromForm] ShiftCreateDTO shiftCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Shift shiftToCreate = _mapper.Map<Shift>(shiftCreateDTO);

                    _db.Shifts.Add(shiftToCreate);
                    _db.SaveChanges();
                    _response.Result = shiftToCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetShift", new { id = shiftToCreate.Id }, _response);
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
        public async Task<ActionResult<ApiResponse>> UpdateShift(int id, [FromForm] ShiftUpdateDTO shiftUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (shiftUpdateDTO == null || id != shiftUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    Shift shiftFromDb = await _db.Shifts.FindAsync(id);
                    if (shiftFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    _mapper.Map(shiftUpdateDTO, shiftFromDb);

                    _db.Shifts.Update(shiftFromDb);
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
        public async Task<ActionResult<ApiResponse>> DeleteShift(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                Shift shiftFromDb = await _db.Shifts.FindAsync(id);
                if (shiftFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.Shifts.Remove(shiftFromDb); 
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
