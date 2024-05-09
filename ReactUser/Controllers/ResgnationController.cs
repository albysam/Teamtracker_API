using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactUser.Data;
using ReactUser.Models;
using ReactUser.Models.Dto;
using ReactUser.Services;
using ReactUser.Utility;
using System.Net;
using System.Net.NetworkInformation;

namespace ReactUser.Controllers
{
    [Route("api/Resgnation")]
    [ApiController]
    public class ResgnationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
       
        private ApiResponse _response;
        public ResgnationController(ApplicationDbContext db)
        {
            _db = db;
         _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetResgnations(string searchString)
        {
            try
            {
                IEnumerable<Resgnation> resgnations = _db.Resgnations
                    .Include(d => d.User)
                     
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    resgnations = resgnations
                        .Where(u => u.EmployeeId.ToLower().Contains(searchString.ToLower()) ||
                            u.User.Name.ToLower().Contains(searchString.ToLower()) ||
                            u.User.UserName.ToLower().Contains(searchString.ToLower()));
                                    
                                   
                }

                var result = resgnations.Select(d => new
                {
                    Id = d.Id,
                    EmployeeId = d.EmployeeId,
                    Name = d.User.Name,
                    UserName = d.User.UserName,
                   
                    ResgnationDate = d.ResgnationDate,
                    Reason = d.Reason,
                    Comment = d.Comment,
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

        [HttpGet("{id:int}", Name = "Getresgnation")]
        public async Task<IActionResult> Getresgnation(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                Resgnation resgnation = _db.Resgnations.FirstOrDefault(u => u.Id == id);
                if (resgnation == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = resgnation;
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
        public async Task<ActionResult<ApiResponse>> CreateResgnation([FromForm] ResgnationCreateDTO resgnationCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Resgnation resgnationToCreate = new()
                    {
                        EmployeeId = resgnationCreateDTO.EmployeeId,
                        ResgnationDate = resgnationCreateDTO.ResgnationDate,
                        Reason = resgnationCreateDTO.Reason,
                        Comment = resgnationCreateDTO.Comment,
                        Status = SD.Resgnation_Pending,
                    };
                    _db.Resgnations.Add(resgnationToCreate);
                    _db.SaveChanges();
                    _response.Result = resgnationToCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetResgnation", new { id = resgnationToCreate.Id }, _response);


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
        public async Task<ActionResult<ApiResponse>> UpdateResgnation(int id, [FromForm] ResgnationUpdateDTO resgnationUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (resgnationUpdateDTO == null || id != resgnationUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    Resgnation resgnationFromDb = await _db.Resgnations.FindAsync(id);
                    if (resgnationFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    resgnationFromDb.EmployeeId = resgnationUpdateDTO.EmployeeId;
                    resgnationFromDb.ResgnationDate = resgnationUpdateDTO.ResgnationDate;
                    resgnationFromDb.Reason = resgnationUpdateDTO.Reason;
                    resgnationFromDb.Comment = resgnationUpdateDTO.Comment;
                    resgnationFromDb.Status = resgnationUpdateDTO.Status;

                    _db.Resgnations.Update(resgnationFromDb);
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
        public async Task<ActionResult<ApiResponse>> DeleteResgnation(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                Resgnation resgnationFromDb = await _db.Resgnations.FindAsync(id);
                if (resgnationFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.Resgnations.Remove(resgnationFromDb); 
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
