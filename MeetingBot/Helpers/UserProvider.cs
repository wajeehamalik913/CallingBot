using MeetingBot.Interfaces;
using MeetingBot.Models;
using MeetingBot.Utility;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetingBot.Helpers
{
    public class UserProvider : IUserProvider
    {
        public enum UserType
        {
            user,
            acsUser,
            guest,
            phone,
            application
        }
        public participantDetails getUser(Participant participant)
        {
            participantDetails participantEntity = new participantDetails();
            
                var userType = participant.GetUserType();
                if (userType == "user")
                {

                    participantEntity.ParticipantId = participant.Id;
                    participantEntity.Name = participant.Info.Identity.User.DisplayName;
                }
                else if (userType == "acsUser")
                {
                    var targetIdentity = participant.Info.Identity.AdditionalData[userType.ToString()] as Identity;
                    participantEntity.ParticipantId = participant.Id;
                    participantEntity.Name = targetIdentity.DisplayName;
                }
                else if (userType == "phone")
                {
                    var targetIdentity = participant.Info.Identity.AdditionalData[userType.ToString()] as Identity;
                    participantEntity.ParticipantId = participant.Id;
                    participantEntity.Name = targetIdentity.DisplayName;
                }
                else
                {
                    var targetIdentity = participant.Info.Identity.AdditionalData[userType.ToString()] as Identity;
                    participantEntity.ParticipantId = participant.Id;
                    participantEntity.Name = userType == "guest" && targetIdentity.DisplayName.EndsWith("(Guest)") ?
                        targetIdentity.DisplayName.Remove(targetIdentity.DisplayName.Length - 7) :
                        targetIdentity.DisplayName;
                }
            
            return participantEntity;
        }
    }
}
