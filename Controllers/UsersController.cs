using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixerAPI.Dtos.Commons;
using PixerAPI.Dtos.Responses.Users;
using PixerAPI.Models;
using PixerAPI.UnitOfWorks.Interfaces;
using System.Security.Claims;

namespace PixerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IServiceUnitOfWork serviceUnitOfWork;

        public UsersController(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
        }

        [HttpGet("inventories")]
        public async Task<IActionResult> GetInventoriesByUserId()
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                bool userIdLock = await this.serviceUnitOfWork.UserService.IsLockAsync(Convert.ToInt32(userId));
                if (userIdLock) return Unauthorized(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "User is locked",
                    Data = null
                });

                List<ShortInventoryDto> shortInventories = await this.serviceUnitOfWork.UserService.GetShortInventoriesByUserIdAsync(Convert.ToInt32(userId));

                return Ok(new ResponseDto<List<ShortInventoryDto>>()
                {
                    IsSuccess = true,
                    Message = "Successful",
                    Data = shortInventories
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(500, new ResponseDto<object>()
                {
                    IsSuccess = true,
                    Message = "Failed",
                    Data = null
                });
            }
        }

        [HttpGet("solds")]
        public async Task<IActionResult> GetSoldOutByUserId()
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                bool userIdLock = await this.serviceUnitOfWork.UserService.IsLockAsync(Convert.ToInt32(userId));
                if (userIdLock) return Unauthorized(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "User is locked",
                    Data = null
                });

                return Ok(new ResponseDto<List<object>>()
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

        [AllowAnonymous]
        [HttpGet("{username}/profile")]
        public async Task<IActionResult> GetProfileByUsername(string username)
        {
            try
            {
                User? user = await this.serviceUnitOfWork.UserService.FindByUsernameAsync(username);

                if (user == null) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Profile not found",
                    Data = null
                });

                UserProfileDto profile = new UserProfileDto();
                profile.ProfileImage = user.ProfileImage;
                profile.Username = user.Username;
                profile.Description = user.Description;

                return Ok(new ResponseDto<UserProfileDto>()
                {
                    IsSuccess = true,
                    Message = "Successful",
                    Data = profile
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
    }
}
