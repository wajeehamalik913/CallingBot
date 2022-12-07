using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Common.Middlewears
{
    public class TraceLoggerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public TraceLoggerMiddleware(
            RequestDelegate next,
            ILogger<TraceLoggerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("TransactionId", out var value))
            {
                context.TraceIdentifier = value;
            }
            else if (context.Request.Path.Value == "/api/callback")
            {
                context.TraceIdentifier = Guid.NewGuid().ToString();
            }
            using (this.logger.BeginScope("{@TransactionId}", context.TraceIdentifier))
            {
                this.logger.LogInformation("Incoming request {User} {Scheme} {Method} {Path}{Query}", context.User?.Identity?.Name, context.Request.Scheme, context.Request.Method, context.Request.Path, context.Request.QueryString);
                await next(context);
                this.logger.LogInformation("Request completed {StatusCode}", context.Response.StatusCode);
            }
        }
    }
}
