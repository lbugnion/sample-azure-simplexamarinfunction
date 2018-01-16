
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace LbCalculator
{
    public static class Add
    {
        [FunctionName("Add")]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function, 
                "get",
                Route = "add/num1/{num1}/num2/{num2}")]
            HttpRequest req, 
            int num1,
            int num2,
            TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request with {num1} and {num2}");

            var addition = num1 + num2;

            return new OkObjectResult(addition);
        }
    }
}
