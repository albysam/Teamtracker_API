using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactUser.Data;
using ReactUser.Models;
using ReactUser.Models.Dto;
using ReactUser.Services;
using ReactUser.Utility;
using System.Net;

namespace ReactUser.Controllers
{
    [Route("api/Department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
       
        private ApiResponse _response;
        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
         _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetDepartments(string searchString)
        {
            try
            {
                IEnumerable<Department> departments = _db.Departments;

                if (!string.IsNullOrEmpty(searchString))
                {
                    departments = departments
                        .Where(u => u.DepartmentName.ToLower().Contains(searchString.ToLower()) 
                                    
                                    );
                }

                _response.Result = departments.ToList(); 
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

        [HttpGet("{id:int}", Name = "GetDepartment")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                Department department = _db.Departments.FirstOrDefault(u => u.Id == id);
                if (department == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = department;
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
        public async Task<ActionResult<ApiResponse>> CreateDepartment([FromForm] DepartmentCreateDTO departmentCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    Department departmentToCreate = new()
                    {
                        DepartmentName = departmentCreateDTO.DepartmentName
                    };
                    _db.Departments.Add(departmentToCreate);
                    _db.SaveChanges();
                    _response.Result = departmentToCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetDepartment", new { id = departmentToCreate.Id }, _response);


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
        public async Task<ActionResult<ApiResponse>> UpdateDepartment(int id, [FromForm] DepartmentUpdateDTO departmentUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (departmentUpdateDTO == null || id != departmentUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    Department departmentFromDb = await _db.Departments.FindAsync(id);
                    if (departmentFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    departmentFromDb.DepartmentName = departmentUpdateDTO.DepartmentName;
                   

                  

                    _db.Departments.Update(departmentFromDb);
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
        public async Task<ActionResult<ApiResponse>> DeleteDepartment(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                Department departmentFromDb = await _db.Departments.FindAsync(id);
                if (departmentFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                _db.Departments.Remove(departmentFromDb); 
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
