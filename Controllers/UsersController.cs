using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixerAPI.Dtos.Commons;
using PixerAPI.Dtos.Requests.Users;
using PixerAPI.Dtos.Responses.Products;
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

        [AllowAnonymous]
        [HttpGet("{username}/products/sales")]
        public async Task<IActionResult> GetUserProductSales(string username)
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

                List<ProfileProductDto> profileProductDtos = await this.serviceUnitOfWork.ProductService.GetShortProductByUserIdAsync(user.Id);

                return Ok(new ResponseDto<List<ProfileProductDto>>()
                {
                    IsSuccess = true,
                    Message = "Successful",
                    Data = profileProductDtos
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

        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto dto)
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

                User? user = await this.serviceUnitOfWork.UserService.FindByIdAsync(Convert.ToInt32(userId));
                if (user == null) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "User not found",
                    Data = null
                });

                // update user profile
                if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
                {
                    // file type should be JPEG, PNG, GIF
                    bool isImageFile = this.serviceUnitOfWork.FileService.IsImageFile(dto.ProfileImage);
                    if (!isImageFile) return BadRequest(new ResponseDto<object>()
                    {
                        IsSuccess = false,
                        Message = "File should be image type",
                        Data = null
                    });

                    user.ProfileImage = await this.serviceUnitOfWork.FileService.ToBytesAsync(dto.ProfileImage);
                }

                // update username
                if (dto.Username != null && dto.Username.Trim().Length > 0 && dto.Username != user.Username)
                {
                    bool isExistUsername = await this.serviceUnitOfWork.UserService.IsExistUsername(dto.Username);
                    if (isExistUsername) return BadRequest(new ResponseDto<object>()
                    {
                        IsSuccess = false,
                        Message = "Username is aleady to use",
                        Data = null
                    });

                    user.Username = dto.Username;
                }

                // update description
                if (dto.Description != null && dto.Description != user.Description)
                {
                    dto.Description = user.Description;
                }

                await this.serviceUnitOfWork.UserService.UpdateAsync(user);

                return Ok(new ResponseDto<object>()
                {
                    IsSuccess = true,
                    Message = "Successful",
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return this.StatusCode(500, new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Failed",
                    Data = null
                });
            }
        }
    }
}
