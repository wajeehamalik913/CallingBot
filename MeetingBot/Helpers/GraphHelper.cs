

namespace MeetingBot.Helpers
{
    using MeetingBot.Bots;
    using MeetingBot.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Graph;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class GraphHelper : IGraph
    {
        private readonly ILogger<CallingBot> logger;
        private readonly IConfiguration configuration;
        private readonly GraphServiceClient graphServiceClient;


        public GraphHelper(ILogger<CallingBot> logger, IConfiguration configuration, GraphServiceClient graphServiceClient)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.graphServiceClient = graphServiceClient;
        }

        public async Task<Call> AddBotToCallAsync(string threadId, string organizerId)
        {
            

            var meetingInfo = new OrganizerMeetingInfo
            {
                Organizer = new IdentitySet
                {
                    User = new Identity { Id = organizerId },
                },
            };
            meetingInfo.Organizer.User.SetTenantId("687a05f8-d1ea-49c7-9b9a-ac2e7e624a5b");

            var call = new Call
            {
                CallbackUri = "https://692a-5-194-16-136.in.ngrok.io/api/callback",
                ChatInfo = new ChatInfo
                {
                    ThreadId = threadId,
                    MessageId = "0"
                },
                TenantId = "687a05f8-d1ea-49c7-9b9a-ac2e7e624a5b",
                RequestedModalities = new List<Modality>() { Modality.Audio, Modality.Video, Modality.VideoBasedScreenSharing },

                MediaConfig = new ServiceHostedMediaConfig(),
                MeetingInfo = meetingInfo
            };


            var callResponse = await graphServiceClient.Communications.Calls
            .Request()
            .AddAsync(call);

            return callResponse;
        }
    }
}
