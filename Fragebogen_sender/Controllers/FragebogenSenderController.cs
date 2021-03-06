﻿using System;
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
        private readonly ILogger<FragebogenSenderController> _logger;
        Uri uri;
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
                GetURIFromConsul();


                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(uri.ToString());
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await Polly.Policy
               .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
               .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
               {
                   _logger.LogWarning($"Request failed with {result.Result.StatusCode}. " +
                       $"Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
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

        private async void GetURIFromConsul()
        {
            List<Uri> _serverUrls = new List<Uri>();

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Polly.Policy
                   .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                   .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
                   {
                       _logger.LogWarning($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                   })
                   .ExecuteAsync(() =>
                   {
                       return httpClient.GetAsync("https://localhost:44322/api/config");
                   });

            if (response.IsSuccessStatusCode)
            {
                string t = await response.Content.ReadAsAsync<string>();
          
                _logger.LogInformation("Response was successful.");

                var consuleClient = new ConsulClient(async c => c.Address = new Uri(t));
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
            }
            else
            {
                _logger.LogError($"Response failed. Status code {response.StatusCode}");
            }



            uri = new Uri(_serverUrls.FirstOrDefault().ToString());
        }
    }
}