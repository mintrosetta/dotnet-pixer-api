using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixerAPI.UnitOfWorks.Interfaces;

namespace PixerAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TestersController : ControllerBase
    {
        private readonly IServiceUnitOfWork serviceUnitOfWork;

        public TestersController(IServiceUnitOfWork serviceUnitOfWork)
        {
            this.serviceUnitOfWork = serviceUnitOfWork;
        }

        [HttpGet("sendmail")]
        public async Task<IActionResult> TestSendingMail()
        {
            try
            {
                await this.serviceUnitOfWork.EmailService.SendAsync("mint.rosetta2001@gmail.com", "Testing send email from api", "This is a description for testing send email");

                return Ok("Successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
