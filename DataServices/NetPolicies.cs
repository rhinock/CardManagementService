using Polly;
using Polly.Retry;

using System;
using System.Net;
using System.Linq;

using Domain.Objects;

namespace DataServices
{
    public static class NetPolicies
    {
        private static IAsyncPolicy _currentPolicy;

        public static IAsyncPolicy GetPolicy(ResourceConnection connection)
        {
            if(_currentPolicy != null)
            {
                return _currentPolicy;
            }

            int retryCount = 0;
            int retryStart = 0;
            int exceptionCount = 0;
            int breakDuration = 0;
            var connectionParts = connection.Value.Split(';').Select(x => x.Trim());

            foreach (var part in connectionParts)
            {
                if (part.StartsWith("retryCount="))
                {
                    retryCount = int.Parse(part.Split('=').LastOrDefault());
                }
                if (part.StartsWith("retryStart="))
                {
                    retryStart = int.Parse(part.Split('=').LastOrDefault());
                }
                if (part.StartsWith("exceptionCount="))
                {
                    exceptionCount = int.Parse(part.Split('=').LastOrDefault());
                }
                if (part.StartsWith("breakDuration="))
                {
                    breakDuration = int.Parse(part.Split('=').LastOrDefault());
                }
            }

            IAsyncPolicy retryPolicy = GetRetryPolicy(retryCount, retryStart);
            IAsyncPolicy circuitBreakerPolicy = GetCircuitBreakerPolicy(exceptionCount, breakDuration);

            _currentPolicy = retryPolicy.WrapAsync(circuitBreakerPolicy);
            return _currentPolicy;
        }

        private static AsyncRetryPolicy GetRetryPolicy(int retryCount, int retryStart)
        {
            return Policy
                .Handle<WebException>(x => x.Response != null && 
                                           x.Response is HttpWebResponse &&
                                           (int)((HttpWebResponse)x.Response).StatusCode >= 500)
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryStart, retryAttempt)));
        }

        private static IAsyncPolicy GetCircuitBreakerPolicy(int exceptionCount, int breakDuration)
        {
            return Policy
                .Handle<WebException>(x => x.Response != null &&
                                           x.Response is HttpWebResponse &&
                                           (int)((HttpWebResponse)x.Response).StatusCode == 503)
                .CircuitBreakerAsync(exceptionCount, TimeSpan.FromSeconds(breakDuration));
        }
    }
}
