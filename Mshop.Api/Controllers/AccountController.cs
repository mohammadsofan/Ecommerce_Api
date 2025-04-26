using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mshop.Api.Data.models;
using Mshop.Api.DTOs.Requests;

namespace Mshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser>  signInManager,
            RoleManager<IdentityRole> roleManager
            ,IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var applicationUser = registerRequest.Adapt<ApplicationUser>();
                var result = await userManager.CreateAsync(applicationUser, registerRequest.Password);
                if (result.Succeeded)
                {
                    await emailSender.SendEmailAsync(applicationUser.Email!, "Welcome to MShop!", $@"
                        <h2>Welcome to MShop, {applicationUser.FirstName} {applicationUser.LastName}!</h2>
                        <p>We're excited to have you on board.</p>
                        <p>Your account has been successfully created. You can now log in and start shopping!</p>
                        <p>If you have any questions, feel free to reach out to us at <a href='mailto:support@mshop.com'>support@mshop.com</a>.</p>
                        <br/>
                        <p>Happy Shopping!<br/>The MShop Team</p>
                        "
                        );
                    await signInManager.SignInAsync(applicationUser,false);
                    return NoContent();
                }

                return BadRequest(result.Errors);
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var applicationUser = await userManager.FindByEmailAsync(loginRequest.Email);
                if(applicationUser is not null)
                {
                    var result = await userManager.CheckPasswordAsync(applicationUser, loginRequest.Password);
                    if (result)
                    {
                        await signInManager.SignInAsync(applicationUser, loginRequest.RememberMe);
                        return NoContent();
                    }
                }
                return BadRequest(new { Message = "Invalid email or password" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await signInManager.SignOutAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var applicationUser = await userManager.GetUserAsync(User);
            if(applicationUser is not null)
            {

                  var res= await userManager.ChangePasswordAsync(applicationUser, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
                    if (res.Succeeded)
                    {
                        return NoContent();
                    }
                    return BadRequest(res.Errors);

            }
            return Unauthorized(new { message = "User no longer exists." });

        }
    }
}
