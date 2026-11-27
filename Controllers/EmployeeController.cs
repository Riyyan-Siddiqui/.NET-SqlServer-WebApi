using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_tutorial.Dto;
using WebAPI_tutorial.GenericResponse;
using WebAPI_tutorial.IServices;

namespace WebAPI_tutorial.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService) : ControllerBase
    {
        [HttpGet("employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var result = await employeeService.GetAllEmployeeAsync();

                if (!result.Item2.Any())
                {
                    return Ok(ResponseResult<List<EmployeeDto>>.Failure(null, "No Employee Found"));
                }
                return Ok(ResponseResult<List<EmployeeDto>>.Success(result.Item2, "Employees Found"));

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("employees")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employee)
        {
            try
            {
                var result = await employeeService.CreateEmployee(employee);

                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("employees")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employee)
        {
            try
            {
                var result = await employeeService.UpdateEmployee(employee);

                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));

            } catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("employees/{Id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid Id)
        {
            try
            {
                var result = await employeeService.DeleteEmployee(Id);
                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("employees/{Id}")]
        public async Task<IActionResult> GetEmployeeByID([FromRoute] Guid Id)
        {
            try
            {
                var result = await employeeService.GetEmployeeByID(Id);
                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<EmployeeDto>.Failure(null, "Employee with this ID does not exist"));
                }
                return Ok(ResponseResult<EmployeeDto>.Success(result.Item2, "Employee Found"));

            } catch(Exception)
            {
                throw;
            }
        }
    }
}
