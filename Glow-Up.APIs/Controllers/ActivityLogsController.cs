using Glow_Up.Core.DTOs.Logs;
using Glow_Up.Core.Services.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Glow_Up.APIs.Controllers;

public class ActivityLogsController(IActivityLogService activityLogService) : ApiBaseController
{
    private readonly IActivityLogService _activityLogService = activityLogService;

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IReadOnlyList<ActivityLogDto>>> GetUserActivities(int userId)
    {
        try
        {
            var activities = await _activityLogService.GetUserActivityLogsAsync(userId);
            return Ok(activities);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

}
