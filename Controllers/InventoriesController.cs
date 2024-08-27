using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixerAPI.Dtos.Commons;
using PixerAPI.Dtos.Responses.Inventories;
using PixerAPI.Dtos.Responses.Products;
using PixerAPI.Models;
using PixerAPI.UnitOfWorks.Interfaces;
using System.Security.Claims;

namespace PixerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly IServiceUnitOfWork serviceUnitOfWork;

        public InventoriesController(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
        }

        [HttpGet("{inventoryId}")]
        public async Task<IActionResult> GetInvetoryById(int inventoryId)
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                bool isLock = await this.serviceUnitOfWork.UserService.IsLockAsync(Convert.ToInt32(userId));
                if (isLock) return Unauthorized(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "User is locked"
                });

                UserInventory? userInventory = await this.serviceUnitOfWork.InventoryService.GetInventoryByIdAsync(inventoryId);
                
                // user inventory should be found
                if (userInventory == null) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "User inventory not found",
                    Data = null
                });

                // user request should be buyer of product
                if (Convert.ToInt32(userId) != userInventory.UserId) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Unauthorize to get inventory information",
                    Data = null
                });

                Product? product = await this.serviceUnitOfWork.ProductService.GetProductByIdAsync(userInventory.ProductId);
                
                // product should not be null
                if (product == null) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Product not found",
                    Data = null
                });

                User? owner = await this.serviceUnitOfWork.UserService.FindByIdAsync(product.Id);
                
                // owner of product should not be null
                if (owner == null) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Product owner not found",
                    Data = null
                });

                ProductOwner productOwner = new ProductOwner()
                {
                    Username = owner.Username,
                    ProfileImage = owner.ProfileImage
                };

                // get all master data of agreements
                List<Agreement> agreements = await this.serviceUnitOfWork.AgreementService.GetAgreementsAsync();

                // create instance result agreement of this product
                List<ProductAgreementDto> agreementDtos = new List<ProductAgreementDto>();

                // get agreement of product by productId
                List<ProductAgreement> productAgreements = await this.serviceUnitOfWork.AgreementService.GetAgreementByProductIdAsync(product.Id);

                // loop for process agreement of product
                foreach (Agreement agreement in agreements)
                {
                    bool isAccept = false;

                    // loop for check agreement of product is accept
                    /*
                        if agreement of product is in master data of agreement, should be accept
                        if agreement of product is not in master data of agreement, should be not accept
                     */
                    foreach (ProductAgreement productAgreement in productAgreements)
                    {
                        if (productAgreement.ArgrementId == agreement.Id)
                        {
                            isAccept = true;
                            break;
                        }
                    }

                    // add result of product agreement to collection of agreement result 
                    agreementDtos.Add(new ProductAgreementDto()
                    {
                        Name = agreement.Name,
                        IsAccept = isAccept
                    });
                }

                // create instance if InventoryDto for result
                InventoryDto inventoryDto = new InventoryDto()
                {
                    Id = inventoryId,
                    Image = product.Image,
                    Price = Math.Round(product.Price, 2),
                    Agreements = agreementDtos,
                    BuyDate = userInventory.CreatedAt,
                    Owner = productOwner
                };

                // return to client
                return Ok(new ResponseDto<InventoryDto>()
                {
                    IsSuccess = true,
                    Message = "Successful",
                    Data = inventoryDto
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
