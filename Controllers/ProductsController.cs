using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixerAPI.Dtos.Commons;
using PixerAPI.Dtos.Requests.Products;
using PixerAPI.Dtos.Responses.Products;
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

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                // create instance for result of product
                List<ShortProductDto> shortProductDtos = new List<ShortProductDto>();

                // get all products in database
                List<Product> products = await this.serviceUnitOfWork.ProductService.GetProductsAsync();

                // get all master data of agreements
                List<Agreement> agreements = await this.serviceUnitOfWork.AgreementService.GetAgreementsAsync();

                // loop for process products
                foreach (Product product in products)
                {
                    // get agreement of product by productId
                    List<ProductAgreement> productAgreements = await this.serviceUnitOfWork.AgreementService.GetAgreementByProductIdAsync(product.Id);

                    // create instance result agreement of this product
                    List<ProductAgreementDto> agreementDtos = new List<ProductAgreementDto>();

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

                    // after processed, add current product to collection result of product
                    ShortProductDto productDto = new ShortProductDto();
                    productDto.Image = product.Image;
                    productDto.Price = Math.Round(product.Price, 2);
                    productDto.Agreements = agreementDtos;

                    shortProductDtos.Add(productDto);
                }

                // return to client
                return Ok(new ResponseDto<List<ShortProductDto>>()
                {
                    IsSuccess = true,
                    Message = "Successful",
                    Data = shortProductDtos
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

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            try
            {
                Product? product = await this.serviceUnitOfWork.ProductService.GetProductByIdAsync(productId);

                // validate product
                if (product == null) return StatusCode(204, new ResponseDto<object>()
                {
                    IsSuccess = true,
                    Message = "No Content",
                    Data = null
                });

                // get all master data of agreements
                List<Agreement> agreements = await this.serviceUnitOfWork.AgreementService.GetAgreementsAsync();

                // get agreement of product by productId
                List<ProductAgreement> productAgreements = await this.serviceUnitOfWork.AgreementService.GetAgreementByProductIdAsync(product.Id);

                // create instance result agreement of this product
                List<ProductAgreementDto> agreementDtos = new List<ProductAgreementDto>();

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

                // get owner of product
                User? user = await this.serviceUnitOfWork.UserService.FindByIdAsync(product.Id);
                if (user == null) return BadRequest(new ResponseDto<object>()
                {
                    IsSuccess = false,
                    Message = "Owner of this product has deleted",
                    Data = null
                });

                ProductOwner owner = new ProductOwner()
                {
                    ProfileImage = user.ProfileImage,
                    Username = user.Username
                };

                // after processed, add current product to collection result of product
                ProductDto productDto = new ProductDto();
                productDto.Image = product.Image;
                productDto.Price = Math.Round(product.Price, 2);
                productDto.Agreements = agreementDtos;
                productDto.Description = product.Description;
                productDto.Owner = owner;

                // return to client
                return Ok(new ResponseDto<ProductDto>()
                {
                    IsSuccess = true,
                    Message = "Successful",
                    Data = productDto
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
