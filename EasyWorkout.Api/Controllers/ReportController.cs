using EasyWorkout.Api.Auth;
using EasyWorkout.Api.Mapping;
using EasyWorkout.Application.Services;
using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EasyWorkout.Api.Controllers
{
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger _logger;

        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Reports.GetTotalVolume)]
        public async Task<IActionResult> GetTotalVolume([FromBody] TotalVolumeReportRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var cwsToReportForUser = await _reportService.GetFilteredCompletedWorkoutsForUserAsync(userId!.Value, 
                            request.FromDate, request.ToDate, request.WorkoutId, token);
            var totalVolumeReportResponse = cwsToReportForUser.MapToResponse(request.WeightUnit);

            return Ok(totalVolumeReportResponse);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Reports.GetTotalTime)]
        public async Task<IActionResult> GetTotalTime([FromBody] TotalTimeReportRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var cwsToReportForUser = await _reportService.GetFilteredCompletedWorkoutsForUserAsync(userId!.Value,
                            request.FromDate, request.ToDate, request.WorkoutId, token);
            var totalTimeReportResponse = cwsToReportForUser.MapToResponse(request.DurationUnit);

            return Ok(totalTimeReportResponse);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Reports.GetTotalDistance)]
        public async Task<IActionResult> GetTotalDistance([FromBody] TotalDistanceReportRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var cwsToReportForUser = await _reportService.GetFilteredCompletedWorkoutsForUserAsync(userId!.Value,
                            request.FromDate, request.ToDate, request.WorkoutId, token);
            var totalDistanceReportResponse = cwsToReportForUser.MapToResponse(request.DistanceUnit);

            return Ok(totalDistanceReportResponse);
        }

        [Authorize(AuthConstants.FreeMemberUserPolicyName)]
        [HttpPost(Endpoints.Reports.GetAveragePercentCompleted)]
        public async Task<IActionResult> GetAveragePercentCompleted([FromBody] AveragePercentCompletedReportRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var cwsToReportForUser = await _reportService.GetFilteredCompletedWorkoutsForUserAsync(userId!.Value,
                            request.FromDate, request.ToDate, request.WorkoutId, token);
            var avgPercentCompletedReportResponse = cwsToReportForUser.MapToResponse();

            return Ok(avgPercentCompletedReportResponse);
        }
    }
}
