using MediatR;
using MeetMind.Application.Features.Meetings.Commands;
using MeetMind.Application.Features.Meetings.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetMind.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MeetingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeetingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyMeetings()
    {
        var meetings = await _mediator.Send(new GetMyMeetingsQuery());
        return Ok(meetings);
    }

    [HttpPost("upload")]
    [RequestSizeLimit(524_288_000)]
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = 524_288_000)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new { errors = new[] { "No file was uploaded." } });
        }

        await using var stream = file.OpenReadStream();
        var command = new UploadMeetingCommand(stream, file.FileName, file.ContentType, file.Length);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Ok(result.Value);
    }
}
