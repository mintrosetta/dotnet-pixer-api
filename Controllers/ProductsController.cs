using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixerAPI.Dtos.Commons;
using PixerAPI.Dtos.Requests.Products;
using PixerAPI.Models;
using PixerAPI.UnitOfWorks.Interfaces;
using System.Security.Claims;

namespace PixerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IServiceUnitOfWork serviceUnitOfWork;

        public ProductsController(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto dto)
        {
            try
            {
                // validate user require create product
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                bool isLock = await this.serviceUnitOfWork.UserService.IsLockAsync(Convert.ToInt32(userId));
                if (isLock) return Unauthorized(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "User is locked",
                    Data = false
                });

                // validate product 
                bool isImageFile = this.serviceUnitOfWork.FileService.IsImageFile(dto.Image);
                if (!isImageFile) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "File should be image type",
                    Data = null
                });

                bool isCorrectAgreement = await this.serviceUnitOfWork.AgreementService.IsCorrectAgreement(dto.Agreement);
                if (!isCorrectAgreement) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Agreement is invalid",
                    Data = null
                });

                // create product
                Product productCreated = await this.serviceUnitOfWork.ProductService.CreateProductAsync(new Product()
                { 
                    UserId = Convert.ToInt32(userId),
                    Image = await this.serviceUnitOfWork.FileService.ToBytesAsync(dto.Image),
                    Price = dto.Price,
                    Description = dto.Description,
                    IsSoldOut = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                });

                // create product agreement
                await this.serviceUnitOfWork.AgreementService.AppendAgreementsAsync(productCreated.Id, dto.Agreement);
                

                return StatusCode(201, new ResponseDto<object>()
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
    }
}
