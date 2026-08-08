using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.Entirty.Mail;
using Store.CoreLayer.IServices;
using Store.DTO;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Store.Controllers
{
  
    public class SurveyController : BaseApiController
    {
        private readonly IEmailSetting _email;

        public SurveyController(IEmailSetting email)
        {
            _email = email;
        }
        [HttpPost("MakeSurvey")]
        public async Task<ActionResult<string>> MakeSurvey(SurveyDTO survey)
        {
            //var user=await _userManager.FindByEmailAsync(survey.SenderEmail);
            var emailMessage = new Email()
            {
                To = "abdosalah44456@gmail.com",
                Subject = $"Survey from {survey.SenderName}",
                Body = $@"
Name: {survey.SenderName}

Email: {survey.SenderEmail}

Survey:

{survey.SureveyMessages}
"
            };
            await _email.SendAsyncByUsingMailKite(emailMessage);
            return Ok("Your Survey send SuccessFully thenk for using Our Product");
        }
    }
}
