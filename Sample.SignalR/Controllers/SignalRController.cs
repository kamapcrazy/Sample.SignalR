using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.SignalR.Services.Interfaces;

namespace Sample.SignalR.Controllers
{
    [Produces("application/json")]
    [Route("api/SignalR")]
    public class SignalRController : Controller
    {
        private readonly INotificationService _notificationService;

        public SignalRController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("sendPublicMessage")]
        public async Task<IActionResult> SendPublicMessage([FromBody]string message)
        {
            await _notificationService.SendPublicMessage(message);

            return Ok();
        }

        [HttpPost]
        [Route("sendPrivateMessage/{groupName}")]
        public async Task<IActionResult> SendPrivateMessage(string groupName, [FromBody]string message)
        {
            var groupToSend = string.IsNullOrEmpty(groupName) ? "Server" : groupName;

            await _notificationService.SendMessageToGroup(message, groupToSend);

            return Ok();
        }
    }
}