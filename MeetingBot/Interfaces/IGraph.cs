
using Microsoft.Graph;
using System;
using System.Threading.Tasks;
namespace MeetingBot.Interfaces
{
  
    public interface IGraph
    {
        Task<Call> AddBotToCallAsync(string threadId, string organizerId);
    }
}
