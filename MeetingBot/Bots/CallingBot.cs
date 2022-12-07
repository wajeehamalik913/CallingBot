
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using MeetingBot.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Graph;
using Microsoft.Graph.Communications.Client.Authentication;
using Microsoft.Graph.Communications.Core.Notifications;
using Microsoft.Graph.Communications.Core.Serialization;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Configuration;

using MeetingBot.Utility;
using MeetingBot.Models.Options;
using System.IO;
using MeetingBot.Models;
using MeetingBot.Helpers;
using Microsoft.Graph.ExternalConnectors;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using MeetingBot.Interfaces;
using System.Runtime.ConstrainedExecution;
using Azure.Messaging.ServiceBus;

namespace MeetingBot.Bots
{
    public class CallingBot : ActivityHandler
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<CallingBot> logger;
        private readonly BotOptions options;
        private readonly IUserProvider userProvider;
        private IRequestAuthenticationProvider AuthenticationProvider { get; }

        private INotificationProcessor NotificationProcessor { get; }
        private CommsSerializer Serializer { get; }


        public CallingBot(BotOptions options,IConfiguration configuration, ILogger<CallingBot> logger, IUserProvider userProvider)
        {
            this.options = options;
            this.configuration = configuration;
            this.logger = logger;
            this.userProvider = userProvider;

            var name = this.GetType().Assembly.GetName().Name;
            this.AuthenticationProvider = new AuthenticationProvider(name, options.AppId, options.AppSecret);
            this.Serializer = new CommsSerializer();
            this.NotificationProcessor = new NotificationProcessor(Serializer);
            this.NotificationProcessor.OnNotificationReceived += this.NotificationProcessor_OnNotificationReceived;
        }

        public async Task ProcessNotificationAsync(
            HttpRequest request,
            HttpResponse response)
        {
            try
            {
                request.Headers.Add("X-Request-Time", DateTime.UtcNow.ToString("o"));
                var httpRequest = request.CreateRequestMessage();
                var results = await this.AuthenticationProvider.ValidateInboundRequestAsync(httpRequest).ConfigureAwait(false);
                if (results.IsValid)
                {
                    var httpResponse = await this.NotificationProcessor.ProcessNotificationAsync(httpRequest).ConfigureAwait(false);
                    await httpResponse.CreateHttpResponseAsync(response).ConfigureAwait(false);
                }
                else
                {
                    var httpResponse = httpRequest.CreateResponse(HttpStatusCode.Forbidden);
                    await httpResponse.CreateHttpResponseAsync(response).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(e.ToString()).ConfigureAwait(false);
            }
        }
        private void NotificationProcessor_OnNotificationReceived(NotificationEventArgs args)
        {
            _ = NotificationProcessor_OnNotificationReceivedAsync(args).ForgetAndLogExceptionAsync(
               logger,
               $"Processing notification {args.Notification.ResourceUrl} with scenario {args.ScenarioId}");
        }

        private async Task NotificationProcessor_OnNotificationReceivedAsync(NotificationEventArgs args)
        {
            this.logger.LogInformation($" Notification ID {args.NotificationId}");
            this.logger.LogInformation($" Tenant ID {args.TenantId}");
            this.logger.LogInformation($" Callback URI {args.CallbackUri}");
            this.logger.LogInformation($" Change Type{args.ChangeType}");
            List<string> names = new List<string>();
            var callId = args.Notification.ExtractCallIdFromNotification();
            if (args.ResourceData is Call call)
            {
                //await callbackHelper.CallHandler(args, call);
            }
            else if (args.ResourceData is ICollection<object> participants)
            {

                var currentParticipants = participants.Select(x => x as Participant).Where(p => p.Info.Identity.Application == null).ToList();
                List<ParticipantData> people = new List<ParticipantData>();

                JSONReadWrite readWrite = new JSONReadWrite();
                var PartData = readWrite.Read("Participants.json", "Models");
                people = JsonConvert.DeserializeObject<List<ParticipantData>>(PartData);
                ParticipantData person = people.FirstOrDefault(x => x.CallId == callId);
                int index = people.FindIndex(x => x.CallId == callId);

                if (person == null)
                {
                    List<participantDetails> data = new List<participantDetails>() { };

                    foreach (var participant in currentParticipants)
                    {

                        var identity = participant.Info.Identity.User.DisplayName;
                        var id = participant.Id;
                        names.Add(identity);

                        participantDetails user = userProvider.getUser(participant);
                        data.Add(user);
                        ParticipantData part = new ParticipantData()
                        {
                            CallId = callId,
                            Participants = data,
                        };
                        people.Add(part);
                    }
                }
                else
                {
                    if (person.CallId == callId)
                    {
                        List<participantDetails> data = new List<participantDetails>() { };
                        List<string> Idlist = new List<string>();
                        List<string> participantlist = new List<string>();
                        //var v1 = person.Participants.Except(currentParticipants);

                        //integerList=currentParticipants.Except(person.Participants);
                        foreach (var name in person.Participants)
                        {
                            Idlist.Add(name.ParticipantId);
                        }
                        foreach (var participant in currentParticipants)
                        {
                            participantDetails user = userProvider.getUser(participant);
                            participantlist.Add(user.ParticipantId);
                            //names.Add(Id);
                        }

                        var v1 = Idlist.Except(participantlist);
                        var resultList = new List<string>();
                        var v2 = participantlist.Except(Idlist);
                        if (participantlist.Count > Idlist.Count)
                        {
                            resultList = v1.Concat(v2).ToList();
                            foreach (var n in currentParticipants)
                            {

                                if (resultList.Contains(n.Id))
                                {
                                    participantDetails user = userProvider.getUser(n);
                                    person.Participants.Add(user);
                                }
                            }
                        }
                        else
                        {
                            resultList = v1.Concat(v2).ToList();
                            var l = resultList.Count();
                            foreach (var result in resultList)
                            {
                                int i = person.Participants.FindIndex(x => x.ParticipantId == result);
                                person.Participants.RemoveAt(i);

                            }
                        }
                        people[index] = person;
                    }
                }

                string jSONString = await SendAsync(people);
                jSONString = JValue.Parse(jSONString).ToString(Formatting.Indented);
                readWrite.Write("Participants.json", "Models", jSONString);
            }

            static async Task<string> SendAsync(List<ParticipantData> people)
            {
                var connectionString = "Endpoint=sb://carsaleservicebus-1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=vYJL1rb2Z4vs8u0yORz9iB4MC5DuKZZNYfCGruzwkks=";
                var client = new ServiceBusClient(connectionString);
                var sender = client.CreateSender("mainqueue");
                string jSONString = JsonConvert.SerializeObject(people);
                var message = new ServiceBusMessage(jSONString);
                await sender.SendMessageAsync(message);
                return jSONString;
            }
            //else
            //{
            //    int index = people.FindIndex(x => x.Id == personModel.Id);
            //    people[index] = personModel;
            //}
            //string jSONString = JsonConvert.SerializeObject(people);
            //readWrite.Write("people.json", "data", jSONString);
            //return View(people);
            //}
            //if (args.ResourceData is ICollection<object> participants)
            //{
            //    var currentParticipants = participants.Select(x => x as Participant).Where(p => p.Info.Identity.Application == null).ToList();
            //    //await graph.AnswerCall(call.Id);
            //    string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //    using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt"), true))
            //    {
            //        var par = JsonConvert.SerializeObject(currentParticipants);
            //        outputFile.WriteLine(callId);
            //        foreach (var participant in currentParticipants)
            //        {

            //            var identity = participant.Info.Identity.User.DisplayName;

            //            outputFile.WriteLine(identity);
            //        }
            //    }
            //}


        }

       
    }
}
