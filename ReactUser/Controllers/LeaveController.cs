using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactUser.Data;
using ReactUser.Models;
using ReactUser.Models.Dto;
using ReactUser.Services;
using ReactUser.Utility;
using System.Net;
using System.Net.NetworkInformation;

namespace ReactUser.Controllers
{
    [Route("api/Leave")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
       
        private ApiResponse _response;
        public LeaveController(ApplicationDbContext db)
        {
            _db = db;
         _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetLeaves(string searchString)
        {
            try
            {
                IEnumerable<Leave> leaves = _db.Leaves;

                if (!string.IsNullOrEmpty(searchString))
                {
                    leaves = leaves
                        .Where(u => u.LeaveName.ToLower().Contains(searchString.ToLower()) 
                                    
                                    );
                }

                _response.Result = leaves.ToList(); 
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

        [HttpGet("{id:int}", Name = "Getleave")]
        public async Task<IActionResult> Getleave(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                Leave leave = _db.Leaves.FirstOrDefault(u => u.Id == id);
                if (leave == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = leave;
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
        public async Task<ActionResult<ApiResponse>> CreateLeave([FromForm] LeaveCreateDTO leaveCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    Leave leaveToCreate = new()
                    {
                        LeaveName = leaveCreateDTO.LeaveName,
                        PaidLeaveDays = leaveCreateDTO.PaidLeaveDays,
                        Status = 0,
                    };
                    _db.Leaves.Add(leaveToCreate);
                    _db.SaveChanges();
                    _response.Result = leaveToCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetLeave", new { id = leaveToCreate.Id }, _response);


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


        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateLeave(int id, [FromForm] LeaveUpdateDTO leaveUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (leaveUpdateDTO == null || id != leaveUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    Leave leaveFromDb = await _db.Leaves.FindAsync(id);
                    if (leaveFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    leaveFromDb.LeaveName = leaveUpdateDTO.LeaveName;
                    leaveFromDb.PaidLeaveDays = leaveUpdateDTO.PaidLeaveDays;

                   

                    _db.Leaves.Update(leaveFromDb);
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
        public async Task<ActionResult<ApiResponse>> DeleteLeave(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                Leave leaveFromDb = await _db.Leaves.FindAsync(id);
                if (leaveFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.Leaves.Remove(leaveFromDb); 
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
