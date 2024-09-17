using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Dtos.AccountDtos;
using PhoneDirectory.Models;
using PhoneDirectory.Services;

namespace PhoneDirectory.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController:ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,TokenService tokenService,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            try { 
                
               AppUser existsUser= await _userManager.Users.FirstAsync(x => x.Email == registerDto.Email);
                   return Conflict();
            }
            catch {  try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser=new AppUser {
                UserName =registerDto.FullName,
                Email =registerDto.Email
                };
              
                var createdUser =await _userManager.CreateAsync(appUser,registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        var newUserDto = new NewUserDto
                        {
                            FullName = appUser.UserName,
                            Email = appUser.Email,
                            Token = _tokenService.CreateToken(appUser)
                        };

                        return Ok(newUserDto);
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception ex) {
                return StatusCode(500, ex);
            }  
            }
           
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto) {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var appUser=await _userManager.Users.FirstOrDefaultAsync(x=>x.Email==loginDto.Email);
                if (appUser == null) return Unauthorized("Invalid Email");
                var result=await _signInManager.CheckPasswordSignInAsync(appUser,loginDto.Password,false);
                if(!result.Succeeded) return Unauthorized("Email not found and/or password incorrec");

                return Ok(new NewUserDto { Email = appUser.Email, FullName = appUser.UserName, Token = _tokenService.CreateToken(appUser) });
            }
            catch (Exception ex) {
                return StatusCode(500);
            }
           

        }
    }
}
