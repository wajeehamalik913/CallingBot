using MeetingBot.Bots;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeetingBot.Controllers
{
    [Route("api/callback")]
    public class CallbackController : ControllerBase
    {
        private readonly CallingBot bot;

        public CallbackController(CallingBot bot)
        {
            this.bot = bot;
        }

        [HttpPost, HttpGet]
        public async Task HandleCallbackRequestAsync()
        {
            await this.bot.ProcessNotificationAsync(this.Request, this.Response).ConfigureAwait(false);
        }
    }
}
