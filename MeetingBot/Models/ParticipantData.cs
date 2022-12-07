using System.Collections.Generic;

namespace MeetingBot.Models
{
    public class ParticipantData
    {
        public string CallId { get; set; }
        public List<participantDetails> Participants { get; set; }
    }
}
