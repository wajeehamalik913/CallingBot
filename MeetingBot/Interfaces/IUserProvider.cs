using MeetingBot.Models;
using Microsoft.Graph;
using System.Collections.Generic;

namespace MeetingBot.Interfaces
{
    public interface IUserProvider
    {
        public participantDetails getUser(Participant participant);
    }
}
