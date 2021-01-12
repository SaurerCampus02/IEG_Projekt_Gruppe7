using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Polly;
using Fragebogen_creator.Models;
using System.Linq;
using Consul;

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
                "https://fragebogencreatornr2.azurewebsites.net",
            "https://fragebogencreatornr3.azurewebsites.net"};


        public FragebogenSenderController(ILogger<FragebogenSenderController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IEnumerable<Fragebogen>> GetAsync()
        {
            List<Fragebogen> fragebogen = null;
            _logger.LogError("Accepted Paymentmethods");

            for (int i = 0; i < 10; i++)
            {
                string uri = GetURIFromConsul().ToString();


                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(uri);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await Polly.Policy
               .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
               .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
               {
                   _logger.LogWarning($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                   _logger.LogWarning($"URL: {uri}/api/fragebogencreator");
               })
               .ExecuteAsync(() =>
               {
                   return httpClient.GetAsync(uri + "/api/fragebogencreator");
               });

                if (response.IsSuccessStatusCode)
                {
                    fragebogen = await response.Content.ReadAsAsync<List<Fragebogen>>();
                    _logger.LogInformation("Response was successful.");
                    break;
                }
                else
                {
                    _logger.LogError($"Response failed. Status code {response.StatusCode}");
                }
            }
       
            return fragebogen;
        }

        private Uri GetURIFromConsul()
        {

            List<Uri> _serverUrls = new List<Uri>();
            var consuleClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500"));
            var services = consuleClient.Agent.Services().Result.Response;
            foreach (var service in services)
            {
                var isCreditCardApi = service.Value.Tags.Any(t => t == "Fragebogen");
                if (isCreditCardApi)
                {
                    try
                    {
                        var serviceUri = new Uri($"{service.Value.Address}");
                        _serverUrls.Add(serviceUri);
                    }
                    catch (Exception)
                    {

                        ;
                    }

                }
            }
            return _serverUrls.FirstOrDefault();
        }
    }
}