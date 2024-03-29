using CHS.BLL.Interfaces;
using CHS.DAL.Entites.Identity;
using Credit_Hours_System.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Credit_Hours_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }
        #region register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult("Email already Exists");
            }
            var user = new AppUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                DisplayName = registerDto.DisplayName,
            };

            var result = await userManager.CreateAsync(user,registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Error occurred  while register");
            }
            await userManager.AddToRoleAsync(user, "Instructor");
            var userDto = new UserDto()
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Token = await tokenService.CreateToken(user, userManager)
            };
            return Ok(userDto);
        }
        #endregion
        #region check if email exists or not
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await userManager.FindByEmailAsync(email) != null; ;
        }
        #endregion
        #region Login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(loginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.Username);
            if (user is null)
            {
                return Unauthorized("This Account Is Not Registered");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var userDto = new UserDto()
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                InstructorType = user.InstructorType,
                Id = user.Id,
                Token = await tokenService.CreateToken(user, userManager)
            };
            return Ok(userDto);
        }
        #endregion
        #region reset password
        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(resetPasswordDto.UserName);
                if (user is null)
                {
                    return Unauthorized("This Account Is Not Registered");
                }
                var result = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest("Error occurred  while reset password");
                }
            }
            return Ok();        
        }
        #endregion
        #region foreget password
        [HttpPost("forgetPassword")]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(forgetPasswordDto.UserName);
                if (user is null)
                {
                    return Unauthorized("This Account Is Not Registered");
                }
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResetLink = Url.Action("ResetPassword", "Account", new { token, username = user.UserName }, Request.Scheme);
                return Ok(passwordResetLink);
            }
            return BadRequest("Error occurred  while forget password");
        }
        #endregion
        #region signout
        [HttpPost("signout")]
        public async Task<ActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
        #endregion
        #region Get the Id of the current user
        [HttpGet("currentUserId")]
        public async Task<ActionResult<string>> GetCurrentUserId()
        {
            if (User.Identity != null)
            {
                string name = User.Identity.Name;
                Claim IdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (IdClaim != null)
                {
                    return ("Id is " + IdClaim.Value);
                }
                else
                {
                    return "NameIdentifier claim not found";
                }
            }
            else
            {
                // Handle the case where the user is not authenticated
                return "User not authenticated";
            }
        }
        #endregion
    }
}
