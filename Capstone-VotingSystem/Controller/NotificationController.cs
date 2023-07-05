using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.NotificationRequest;
using Capstone_VotingSystem.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationController : BaseApiController
    {
        private readonly INotificationService _notification;

        public NotificationController(INotificationService notificationService)
        {
            this._notification = notificationService;
        }
        [HttpGet("{username}")]
        [SwaggerOperation(summary: "Get Notification by username")]
        public async Task<IActionResult> getNotificationById(string? username)
        {
            try
            {
                var result = await _notification.GetNotificationId(username);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }

        [HttpPost]
        [SwaggerOperation(summary: "Create new Notification")]
        public async Task<IActionResult> CreateNotification(NotificationRequest request)
        {
            try
            {
                var result = await _notification.CreateNotification(request);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
        [HttpPut("{id}")]
        [SwaggerOperation(summary: "Update Notification")]
        public async Task<IActionResult> CreateNotification(Guid? id, NotificationRequest request)
        {
            try
            {
                var result = await _notification.UpdateNotification(id, request);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
    }
}
