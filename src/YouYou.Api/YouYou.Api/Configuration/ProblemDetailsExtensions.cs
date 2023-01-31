using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadHttpRequestException = Microsoft.AspNetCore.Http.BadHttpRequestException;

namespace YouYou.Api.Configuration
{
    public static class ProblemDetailsExtensions
    {
        public static void UseProblemDetailsExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature != null)
                    {
                        var exception = exceptionHandlerFeature.Error;

                        try
                        {

                            if (exception.InnerException.Message.Contains("REFERENCE"))
                            {
                                var problemDetails = new ProblemDetails
                                {
                                    Instance = context.Request.HttpContext.Request.Path
                                };

                                problemDetails.Title = exception.Message;
                                problemDetails.Status = StatusCodes.Status500InternalServerError;
                                problemDetails.Detail = exception.InnerException.Message;


                                context.Response.StatusCode = problemDetails.Status.Value;
                                context.Response.ContentType = "application/problem+json";

                                var json = JsonConvert.SerializeObject(problemDetails);
                                await context.Response.WriteAsync(json);
                            }
                            else
                            {

                                var problemDetails = new ProblemDetails
                                {
                                    Instance = context.Request.HttpContext.Request.Path
                                };

                                if (exception is BadHttpRequestException badHttpRequestException)
                                {
                                    problemDetails.Title = "The request is invalid";
                                    problemDetails.Status = StatusCodes.Status400BadRequest;
                                    problemDetails.Detail = badHttpRequestException.Message;
                                }
                                else
                                {
                                    var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");
                                    logger.LogError($"Unexpected error: {exceptionHandlerFeature.Error}");

                                    problemDetails.Title = exception.Message;
                                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                                    problemDetails.Detail = exception.Message.ToString();
                                }

                                context.Response.StatusCode = problemDetails.Status.Value;
                                context.Response.ContentType = "application/problem+json";

                                var json = JsonConvert.SerializeObject(problemDetails);
                                await context.Response.WriteAsync(json);
                            }
                        }
                        catch
                        {
                            var problemDetails = new ProblemDetails
                            {
                                Instance = context.Request.HttpContext.Request.Path
                            };

                            if (exception is BadHttpRequestException badHttpRequestException)
                            {
                                problemDetails.Title = "The request is invalid";
                                problemDetails.Status = StatusCodes.Status400BadRequest;
                                problemDetails.Detail = badHttpRequestException.Message;
                            }
                            else
                            {
                                var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");
                                logger.LogError($"Unexpected error: {exceptionHandlerFeature.Error}");

                                problemDetails.Title = exception.Message;
                                problemDetails.Status = StatusCodes.Status500InternalServerError;
                                problemDetails.Detail = exception.Message.ToString();
                            }

                            context.Response.StatusCode = problemDetails.Status.Value;
                            context.Response.ContentType = "application/problem+json";

                            var json = JsonConvert.SerializeObject(problemDetails);
                            await context.Response.WriteAsync(json);
                        }
                    }
                });
            });
        }

    }

}
