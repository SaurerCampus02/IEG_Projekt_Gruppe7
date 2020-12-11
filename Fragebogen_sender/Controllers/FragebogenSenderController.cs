using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Polly;

namespace Fragebogen_sender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FragebogenSenderController : ControllerBase
    {
        //https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        private readonly ILogger<FragebogenSenderController> _logger;
        private static readonly string[] creditcardServiceBaseAddresses = new string[]
            { "https://fragebogencreatornr1.azurewebsites.net" ,
                "https://fragebogencreatornr1.azurewebsites.net",
            "https://fragebogencreatornr1.azurewebsites.net"};


        public FragebogenSenderController(ILogger<FragebogenSenderController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            List<string> acceptedPaymentMethods = null;
            _logger.LogError("Accepted Paymentmethods");

            foreach (string creditcardServiceBaseAddresses in creditcardServiceBaseAddresses)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(creditcardServiceBaseAddresses);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await Policy
               .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
               .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
               {
                   _logger.LogWarning($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                   _logger.LogWarning($"URL: {creditcardServiceBaseAddresses}/api/fragebogencreator");
               })
               .ExecuteAsync(() =>
               {
                   return httpClient.GetAsync(creditcardServiceBaseAddresses + "/api/fragebogencreator");
               });

                if (response.IsSuccessStatusCode)
                {
                    acceptedPaymentMethods = await response.Content.ReadAsAsync<List<string>>();
                    _logger.LogInformation("Response was successful.");
                    break;
                }
                else
                {
                    _logger.LogError($"Response failed. Status code {response.StatusCode}");
                }
            }

            return acceptedPaymentMethods;
        }
    }
}