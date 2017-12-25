using System.Net;

public static HttpResponseMessage Run(HttpRequestMessage req, string name, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // Fetching the name from the path parameter in the request URL
    return req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
}


---------------------------------------------

https://xamfunctionssample.azurewebsites.net/api/HttpTriggerCSharp/name/{name}?code=9r9jauRrThIFjGcP1nz3xRJLiriF9IAo5dmPlsLHBfS4hq0gv06E7A==

https://xamfunctionssample.azurewebsites.net/api/add/num1/{num1}/num2/{num2}?code=9r9jauRrThIFjGcP1nz3xRJLiriF9IAo5dmPlsLHBfS4hq0gv06E7A==