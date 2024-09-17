using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Extensions;
using PhoneDirectory.Models;
using PhoneDirectory.Services;
using System;

namespace PhoneDirectory.Controllers
{
    [Route("cdn/[controller]")]
    [ApiController]
    public class CdnController: ControllerBase
    {
        private readonly CdnService _service;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        public CdnController(UserManager<AppUser> userManager, CdnService cdnService, IWebHostEnvironment environment)
        {
            _service = cdnService;
            _userManager = userManager;
            _environment = environment;
        }

 




        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] CdnModel file )
        {
            try
            {
                var username = User.GetEmail();

                var user = await _userManager.FindByEmailAsync(username);
                if (user == null)
                {
                    return Unauthorized();
                }


            string fileName= _service.uploadFile(file.PhotoFile);
                return Ok(fileName);

            }
            catch (Exception ex) {
                    return StatusCode(500, new { ErrorCode = "SERVER_ERROR", Message = ex.Message! });
            }
          


        }
    }
}
