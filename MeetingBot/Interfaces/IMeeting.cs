using MeetingBot.Models.Requests;
using Microsoft.Graph;
using System.Threading.Tasks;

namespace MeetingBot.Interfaces
{
    public interface IMeeting
    {
        Task<Call> AddBotToCallAsync(AddBotRequest request);
    }
}
