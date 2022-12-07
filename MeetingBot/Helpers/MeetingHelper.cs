using MeetingBot.Interfaces;
using MeetingBot.Models.Requests;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

namespace MeetingBot.Helpers
{
    public class MeetingHelper :IMeeting
    {
        private readonly IConfiguration configuration;
        private readonly IGraph graph;
       

        public MeetingHelper( IConfiguration configuration, IGraph graph
           )
        {

            this.configuration = configuration;
            this.graph = graph;
           
        }

        public async Task<Call> AddBotToCallAsync(AddBotRequest request)
        {

            var call = await graph.AddBotToCallAsync(request.ThreadId, request.OrganizerId);
            return call;
        }
    }
}
