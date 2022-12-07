
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Common.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlerMiddleware> logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                string messageBody = string.Empty;
                switch (error)
                {
                    case EntityCheckFailedException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        messageBody = JsonConvert.SerializeObject(new FailedResponseContent
                        {
                            StatusMessage = ResponseContentStatusMessages.ValidationFailed,
                            ErrorCode = e.Code,
                            ErrorMessage = e.Message,
                            TransactionId = context.TraceIdentifier,
                        });
                        break;
                    case EntityMissingException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        messageBody = JsonConvert.SerializeObject(new FailedResponseContent
                        {
                            StatusMessage = ResponseContentStatusMessages.ResourceNotFound,
                            ErrorCode = e.Code,
                            ErrorMessage = e.Message,
                            TransactionId = context.TraceIdentifier,
                        });
                        break;
                    case EntityConflictException e:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        messageBody = JsonConvert.SerializeObject(new FailedResponseContent
                        {
                            StatusMessage = ResponseContentStatusMessages.ResourceConflict,
                            ErrorCode = e.Code,
                            ErrorMessage = e.Message,
                            TransactionId = context.TraceIdentifier,
                        });
                        break;
                    case EntityForbiddenException e:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        messageBody = JsonConvert.SerializeObject(new FailedResponseContent
                        {
                            StatusMessage = ResponseContentStatusMessages.ValidationFailed,
                            ErrorCode = e.Code,
                            ErrorMessage = e.Message,
                            TransactionId = context.TraceIdentifier,
                        });
                        break;
                    case TransactionUpstreamFailedException e:
                        response.StatusCode = e.ResponseCode;
                        messageBody = JsonConvert.SerializeObject(new FailedResponseContent
                        {
                            StatusMessage = ResponseContentStatusMessages.Exception,
                            ErrorCode = e.Code,
                            ErrorMessage = e.Message,
                            TransactionId = context.TraceIdentifier,
                        });
                        break;
                    case EntityUnauthorizedException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        messageBody = JsonConvert.SerializeObject(new FailedDetailedResponseContent
                        {
                            StatusMessage = ResponseContentStatusMessages.Exception,
                            ErrorCode = (int)CustomErrorCodes.UNHANDLED_ERROR,
                            TransactionId = context.TraceIdentifier,
                            ErrorMessage = CustomErrorCodes.UNHANDLED_ERROR.ToString(),
                            ErrorDetails = error
                        });
                        break;
                }
                this.logger.LogError(error, messageBody);
                var result = messageBody;
                await response.WriteAsync(result);
            }
        }
    }
}
