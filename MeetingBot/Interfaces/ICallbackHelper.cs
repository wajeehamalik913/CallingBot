using Microsoft.Graph.Communications.Core.Notifications;
using Microsoft.Graph;
using System.Threading.Tasks;

namespace MeetingBot.Interfaces
{
    public interface ICallbackHelper
    {
        Task CallHandler(NotificationEventArgs args, Call call);
    }
}
