using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.Entirty.Mail;
using Store.CoreLayer.IService;
using Store.CoreLayer.IServices;
using Store.DTO;
using Store.Errors;
using Store.Helpers;
using System.Net;
using System.Security.Claims;

namespace Store.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSetting _email;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signIn;

        public AccountController(UserManager<AppUser> userManager
            ,ITokenService tokenService,
            SignInManager<AppUser> signInManager,IEmailSetting email,
            IConfiguration configuration,SignInManager<AppUser> signIn)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _email = email;
            this._configuration = configuration;
            this._signIn = signIn;
        }
        [HttpGet("CheckEmailExsist")]
        public async Task<ActionResult<bool>> CheckEmailExsist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null ? true : false;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            if (CheckEmailExsist(register.Email).Result.Value)
                return BadRequest(new ErrorApiResponse(400, "This Emali alredy exsist"));

            var user = new AppUser()
            {
                Email = register.Email,
                DisplayName = register.DisplayName,
                UserName = register.Email.Split("@")[0],
                PhoneNumber = register.PhoneNumber,
            };
            var result=await _userManager.CreateAsync(user,register.Password);
            if (result.Succeeded)
            {
                var userDto = new UserDTO()
                {
                    DisplayName = register.DisplayName,
                    Email = register.Email,
                    Token = await _tokenService.GenerateTokenAsync(user,_userManager)
                };

                return Ok(userDto);
            }
            else
                return BadRequest(new ErrorApiResponse(400));
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
        {
            var user=await _userManager.FindByEmailAsync(login.Email);
            if (user == null) return NotFound(new ErrorApiResponse(404, "this User Not Found"));
            var result=await _signInManager.CheckPasswordSignInAsync(user, login.Password,false);
            if (result.Succeeded)
            {
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshTokens = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                Response.Cookies.Append(
    "refreshToken",
    refreshToken,
    new CookieOptions
    {
        HttpOnly = true,
        Secure = true, 
        SameSite = SameSiteMode.None,
        Expires = DateTimeOffset.UtcNow.AddDays(7)
    });
                await _userManager.UpdateAsync(user);
                var loginUser = new UserDTO
                {
                    DisplayName=user.DisplayName,
                    Email=login.Email,
                    Token=await _tokenService.GenerateTokenAsync(user,_userManager),
                    RefreshToken=refreshToken
                };
                return Ok(loginUser);
            }
            return BadRequest(new ErrorApiResponse(400,"The Password or Email is incorrect"));
        }
        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var ReturnedObject = new UserDTO()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await _tokenService.GenerateTokenAsync(user, _userManager)
                };
                return Ok(ReturnedObject);
            }
            return NotFound(new ErrorApiResponse(404));
        }
        [HttpDelete("DeleteUser")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return NotFound(new ErrorApiResponse(404));
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            else
                return Ok("The User Delete Successed");
        }
        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> ChangePassword(ChangePasswordDTO changePassword)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return NotFound(new ErrorApiResponse(404));
            var result = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
            if (result.Succeeded)
            {
                var changePasswordUser = new UserDTO
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await _tokenService.GenerateTokenAsync(user, _userManager)
                };
                return Ok(changePasswordUser);
            }

            else
                return BadRequest(result.Errors);
        }
        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);

        //    if (user is null)
        //        return NotFound(new ErrorApiResponse(404, "User Not Found"));

        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var encodedToken = WebUtility.UrlEncode(token);

        //    var resetPasswordLink = $"https://yourfrontend.com/reset-password?email={email}&token={encodedToken}";

        //    var emailMessage = new Email()
        //    {
        //        To = email,
        //        Subject = "Reset Your Password",
        //        Body = $"Click the link to reset your password: {resetPasswordLink}"
        //    };

        //    await _email.SendAsyncByUsingMailKite(emailMessage);

        //    return Ok("Password reset email sent successfully");
        //}
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return NotFound(new ErrorApiResponse(404, "User Not Found"));

            //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var encodedToken = WebUtility.UrlEncode(token);
            var otp= GenerateOTP.GenerateSecureOtp(6);
            user.ResetOtp = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
            user.IsOtpVerified = false;
            await _userManager.UpdateAsync(user);
            var emailMessage = new Email()
            {
                To = email,
                Subject = "Reset Your Password",
                Body = $@"
Hello,

Use the otp below to reset your password:

{otp}

This OTP will expire soon, do not share it with anyone.

Regards,
Support Team
    "
            };
            await _email.SendAsyncByUsingMailKite(emailMessage);

            return Ok("Password reset email sent successfully");
        }
        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);

        //    if (user is null)
        //        return NotFound(new ErrorApiResponse(404, "User Not Found"));
        //    var decodedToken = WebUtility.UrlDecode(model.Token);

        //    var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

        //    if (!result.Succeeded)
        //        return BadRequest(new ErrorApiResponse(400, result.Errors.FirstOrDefault()?.Description));

        //    return Ok("Password reset successfully");
        //}

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return NotFound(new ErrorApiResponse(404, "User Not Found"));

            if (user.ResetOtp != model.OTP)
                return BadRequest("Invalid OTP");

            if (user.OtpExpiry < DateTime.UtcNow)
                return BadRequest("OTP expired");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(new ErrorApiResponse(400, result.Errors.FirstOrDefault()?.Description));

            // cleanup
            user.ResetOtp = null;
            user.OtpExpiry = null;
            user.IsOtpVerified = false;

            await _userManager.UpdateAsync(user);

            return Ok("Password reset successfully");
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserDTO>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.RefreshTokens == refreshToken);

            if (user == null)
                return Unauthorized();

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Unauthorized();

            var newAccessToken =await _tokenService.GenerateTokenAsync(user, _userManager);
                
            var newRefreshToken =_tokenService.GenerateRefreshToken();
                
            user.RefreshTokens = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            Response.Cookies.Append(
                "refreshToken",
                newRefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

            return Ok(new
            {
                Token = newAccessToken
            });
        }

[HttpPost("GoogleLogin")]
    public async Task<ActionResult<UserDTO>> GoogleLogin(GoogleLoginDTO model)
    {
        GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(
                model.IdToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[]
                    {
                    _configuration["LoginWithGoogle:ClientId"]
                    }
                });
        }
        catch
        {
            return Unauthorized(new ErrorApiResponse(401, "Invalid Google Token"));
        }

        var user = await _userManager.FindByEmailAsync(payload.Email);

        // إنشاء مستخدم إذا لم يكن موجود
        if (user == null)
        {
            user = new AppUser
            {
                UserName = payload.Email,
                Email = payload.Email,
                DisplayName = payload.Name,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);
        }
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshTokens = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);

        Response.Cookies.Append(
            "refreshToken",
            refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

        var result = new UserDTO
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = await _tokenService.GenerateTokenAsync(user, _userManager),
            RefreshToken = refreshToken
        };

        return Ok(result);
    }
    [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(x => x.RefreshTokens == refreshToken);

                if (user != null)
                {
                    user.RefreshTokens = null;
                    await _userManager.UpdateAsync(user);
                }
            }

            Response.Cookies.Delete("refreshToken");

            return Ok();
        }
    }
}
