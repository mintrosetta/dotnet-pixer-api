using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixerAPI.Dtos.Commons;
using PixerAPI.Dtos.Requests.Auths;
using PixerAPI.Models;
using PixerAPI.UnitOfWorks.Interfaces;
using System.Security.Claims;

namespace PixerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthsController : ControllerBase
    {
        private readonly IServiceUnitOfWork serviceUnitOfWork;

        public AuthsController(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Acthenticate([FromBody] AuthenticateDto dto)
        {
            try 
            {
                // validate user by email
                User? user = await this.serviceUnitOfWork.UserService.FindByEmailAsync(dto.Email);

                if (user == null) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Email or Password is invalid",
                    Data = null
                });

                // validate password
                bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
                if (!isPasswordMatch) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Email or Password is invalid",
                    Data = null
                });

                // generate token
                string token = this.serviceUnitOfWork.JwtService.GenerateToken(user);

                return Ok(new ResponseDto<string>()
                {
                    IsSuccess = true,
                    Message = "Successful",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(500, new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Failed",
                    Data = null
                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                // validate email  
                if (await this.serviceUnitOfWork.UserService.IsExistEmail(dto.Email))
                {
                    return BadRequest(new ResponseDto<object>()
                    {
                        IsSuccess = false,
                        Message = "Email is already to use",
                        Data = null
                    });
                }

                // validate username
                if (await this.serviceUnitOfWork.UserService.IsExistUsername(dto.Username))
                {
                    return BadRequest(new ResponseDto<object>()
                    {
                        IsSuccess = false,
                        Message = "Username is already to use",
                        Data = null
                    });
                }

                // get role
                int roleIdUser = await this.serviceUnitOfWork.RoleService.GetRoleIdUserAsync();

                // hash password 
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                // save user
                await this.serviceUnitOfWork.UserService.CreateAsync(new User()
                { 
                    RoleId = roleIdUser,
                    Email = dto.Email,
                    Username = dto.Username,
                    Password = passwordHash
                });

                return Ok(new ResponseDto<object>()
                {
                    IsSuccess = true,
                    Message = "Successful",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(500, new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Failed",
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpGet("healthcheck")]
        public IActionResult GetHealthCheck()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine(userId);
            
            return Ok(new ResponseDto<object>()
            {
                IsSuccess = true,
                Message = "Successful",
                Data = null
            });
        }
    }
}
