using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.Graph;

namespace MeetingBot.Utility
{
    public static class CommonUtils
    {
        public enum UserType
        {
            user,
            acsUser,
            guest,
            phone,
            application
        }
        public static string GetUserType(this Participant participant)
        {
            if (participant.Info.Identity.User != null) return "user";
            if (participant.Info.Identity.AdditionalData != null)
            {
                if (participant.Info.Identity.AdditionalData.ContainsKey(UserType.acsUser.ToString())) return "acsUser";
                else if (participant.Info.Identity.AdditionalData.ContainsKey(UserType.guest.ToString())) return "guest";
                else if (participant.Info.Identity.AdditionalData.ContainsKey(UserType.phone.ToString())) return "phone";
            }

            return "application";
        }
        public static string ExtractCallIdFromNotification(this CommsNotification notification)
        {
            int startIndex = 22;
            int length = 36;

            var callId = notification.ResourceUrl.Substring(startIndex, length);

            return callId;
        }

        public static async Task ForgetAndLogExceptionAsync(
           this Task task,
           ILogger logger,
           string description = null)
        {
            try
            {
                await task.ConfigureAwait(false);
                logger.LogInformation(
                    $"Completed running task successfully: {description ?? string.Empty}");
            }
            catch (Exception e)
            {
                // Log and absorb all exceptions here.
                logger.LogError(
                    e,
                    $"Caught an exception running the task: {description ?? string.Empty}");
            }
        }


    }
}
