using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IService;
using Store.DTO;
using Store.Helpers;
using System.Security.Claims;

namespace Store.Controllers
{

    public class NotificationController : BaseApiController
    {
        private readonly INotificationService _notification;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public NotificationController(INotificationService notification,
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _notification = notification;
            _userManager = userManager;
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost("SendNotification")]
        public async Task<ActionResult<string>> SendNotification(SendNotificationDTO notifi)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            await _notification.SendNotificationAsync(user.Id, notifi.Title, notifi.Massage);
            return Ok("Notification Send SuccessFull");
        }
        [Authorize]
        [HttpPost("EditNotification")]
        public async Task<ActionResult<string>> EditNotification(SendNotificationDTO notifi)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            await _notification.SendNotificationAsync(user.Id, notifi.Title, notifi.Massage);
            return Ok("Notification Send SuccessFull");
        }
        [Authorize]
        [HttpGet("GetNotificationsForSpecificUser")]
        public async Task<ActionResult<Pagination<IReadOnlyList<NotificationToReturnDTO>>>> GetNotificationsForSpecificUser([FromQuery] NotificationParamter paramter)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            var notifications = await _notification.GetNotificationsAsync(user.Id, paramter);
            var mappedNotification = _mapper.Map<IReadOnlyList<Notification>, IReadOnlyList<NotificationToReturnDTO>>(notifications);
            return Ok(mappedNotification);
        }
        [Authorize]
        [HttpGet("GetNotificationByIdForSpecificUser/{NotificationId}")]
        public async Task<ActionResult<NotificationToReturnDTO>> GetNotificationByIdForSpecificUser(int NotificationId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            var notifications = await _notification.GetNotificationByIdAsync(NotificationId,user.Id);
            var mappedNotification = _mapper.Map<Notification, NotificationToReturnDTO>(notifications);
            return Ok(mappedNotification);
        }
    }
}
