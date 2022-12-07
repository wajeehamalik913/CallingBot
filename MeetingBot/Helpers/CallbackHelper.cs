using MeetingBot.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Communications.Core.Notifications;
using Microsoft.Graph;
using System.Threading.Tasks;
using System;
using MeetingBot.Utility;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MeetingBot.Helpers
{
    public class CallbackHelper
    {
        private readonly ILogger<CallbackHelper> logger;
        private readonly IConfiguration configuration;
        private readonly IGraph graph;

        public CallbackHelper(ILogger<CallbackHelper> logger, IConfiguration configuration, IGraph graph)
        {

            this.logger = logger;
            this.configuration = configuration;
            this.graph = graph;
        }
        public async Task CallHandler(NotificationEventArgs args, Call call)
        {
            //if ((args.ChangeType == ChangeType.Created || args.ChangeType == ChangeType.Updated)
            //    && call.State == CallState.Establishing)
            //{
            //    var callId = args.Notification.ExtractCallIdFromNotification();
            //    if (args.ResourceData is ICollection<object> participants)
            //    {
            //        var currentParticipants = participants.Select(x => x as Participant).Where(p => p.Info.Identity.Application == null).ToList();
            //        //await graph.AnswerCall(call.Id);
            //        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //        using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt"), true))
            //        {
            //            outputFile.WriteLine();
            //        }
            //    }
            //    var session = await sessionRepo.GetByThreadId(call?.ChatInfo?.ThreadId);
            //    if (session != null)
            //    {
            //        if (session.CallId != Guid.Parse(callId))
            //        {
            //            session.CallId = Guid.Parse(callId);
            //            await sessionRepo.Update(session);
            //            await notificationService.PublishNotification(NotificationDomain.Session, UDC.Pretz.Publisher.Contracts.Event.Updated, session.Id.ToString(), new NotificationPayload { data = session, changes = new string[] { nameof(session.CallId), nameof(session.LastModified) } });
            //        }
            //    }
            //    else
            //    {
            //        session = new SessionEntity
            //        {
            //            Id = Guid.NewGuid(),
            //            ThreadId = call?.ChatInfo?.ThreadId,
            //            CallId = Guid.Parse(callId)
            //        };
            //        await sessionRepo.Add(session);
            //        await notificationService.PublishNotification(NotificationDomain.Session, UDC.Pretz.Publisher.Contracts.Event.Created, session.Id.ToString(), new NotificationPayload { data = session });
            //    }
            //}
        }

    }
}
