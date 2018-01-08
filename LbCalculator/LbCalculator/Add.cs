using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Calculator.Data;
using System;

namespace LbCalculator
{
    public static class Add
    {
        [FunctionName("Add")]
        public static HttpResponseMessage Run(
            [HttpTrigger(
                AuthorizationLevel.Function, 
                "get",
                Route = "add/num1/{num1}/num2/{num2}")]
            HttpRequestMessage req, 
            int num1,
            int num2,
            TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request with {num1} and {num2}");

            var addition = num1 + num2;

            var result = new AdditionResult
            {
                Result = addition,
                TimeOnServer = DateTime.Now
            };

            // Fetching the name from the path parameter in the request URL
            return req.CreateResponse(HttpStatusCode.OK, result, "application/json");
        }
    }
}
