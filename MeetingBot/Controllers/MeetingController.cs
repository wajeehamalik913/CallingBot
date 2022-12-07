using MeetingBot.Interfaces;
using MeetingBot.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Threading.Tasks;

namespace MeetingBot.Controllers
{
    [Route("api/Call")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMeeting meetingHelper;
        public MeetingController(IMeeting _communicationHelper)
        {
            meetingHelper = _communicationHelper;
        }

        [HttpPost("bot")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Call))]
        public async Task<IActionResult> AddBotAsync([FromBody] AddBotRequest request)
        {
            var response = await meetingHelper.AddBotToCallAsync(request);
            return this.Ok(response);
        }
    }
}
