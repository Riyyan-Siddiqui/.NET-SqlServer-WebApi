using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_tutorial.Dto;
using WebAPI_tutorial.GenericResponse;
using WebAPI_tutorial.IServices;

namespace WebAPI_tutorial.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authservice)
        {
            _authService = authservice;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserDto dto)
        {
            var result = await _authService.LoginUser(dto);

            try
            {
                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Message));

                }
                else if (result.Item1 == 1)
                {
                    return BadRequest(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Message));
                }
                else if (result.Item1 == 2)
                {
                    return Ok(ResponseResult<TokenDto>.Success(result.Item2, result.Item2.Message));
                }
                else
                {
                    return StatusCode(500, ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Message));
                }

            } catch(Exception e)
            {
                return StatusCode(500, "An error occurred during login: " + e.Message);
            }

        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto dto)
        {
            var result = await _authService.RegisterUser(dto);

            if (result.Item1 == 0)
            {
                return Ok(ResponseResult<string>.Failure(null, result.Item2));

            }
            return Ok(ResponseResult<string>.Success(null, result.Item2));
        }
    }
}
