using CMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CMS.Filters
{
    public class LoggingFilter : IAsyncActionFilter
    {
        private readonly ILogger<LoggingFilter> _logger;
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _stringBuilder.Append("Request");

            try
            {

                if (context.HttpContext.Request.Method != "GET")
                {
                    var request = context.HttpContext.Request;

                    if (request.Body.CanSeek)
                    {
                        request.Body.Seek(0, SeekOrigin.Begin);

                        using (var reader = new StreamReader(request.Body))
                        {
                            var requestBody = await reader.ReadToEndAsync();
                            var deserializedCard = JsonSerializer.Deserialize<Card>(requestBody);

                            AppendLines(_stringBuilder, deserializedCard);                            
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                LogErrors(_logger, exception);
            }

            _logger.LogInformation(_stringBuilder.ToString());

            var actionExecutionDelegate = await next();

            try
            {
                _stringBuilder.Clear();
                _stringBuilder.AppendLine("Response");

                var objectResult = actionExecutionDelegate.Result as ObjectResult;
                _stringBuilder.AppendLine($"StatusCode: {objectResult.StatusCode}");

                if (objectResult.Value is IList<Card> cards)
                {
                    foreach (var card in cards)
                    {
                        AppendLines(_stringBuilder, card);
                    }
                }

                if (objectResult.Value is Card c)
                {
                    AppendLines(_stringBuilder, c);
                }
            }
            catch (Exception exception)
            {
                LogErrors(_logger, exception);
            }

            _logger.LogInformation(_stringBuilder.ToString());
        }

        private void AppendLines(StringBuilder stringBuilder, Card card)
        {
            stringBuilder.AppendLine($"Id: {card.Id}");
            stringBuilder.AppendLine($"Cvc: {Regex.Replace(card.Cvc, @"[\d]", "*")}");

            stringBuilder.AppendLine($"Pan: " +
                $"{Regex.Replace(card.Pan.Substring(0, card.Pan.Length - 4), @"\d", "*")}" +
                $"{card.Pan.Substring(card.Pan.Length - 4, 4)}");

            stringBuilder.AppendLine($"Expire: " +
                $"Month {card.Expire.Month}, " +
                $"Year {card.Expire.Year}");

            stringBuilder.AppendLine($"IsDefault: {card.IsDefault}");
            stringBuilder.AppendLine($"UserId: {card.UserId}");

            stringBuilder.AppendLine();
        }

        private void LogErrors(ILogger logger, Exception exception)
        {
            logger.LogError(exception.Message);
            logger.LogError(exception.StackTrace);
            logger.LogError(exception.InnerException.Message);
        }
    }
}