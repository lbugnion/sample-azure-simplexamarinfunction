1. Creating the basic function 
2. Creating and running the Xamarin.Forms client
    1. Designing the UI with Xamarin Live Player
    2. Implementing the code
3. Modifying to use JSON instead
4. Creating the function in Visual Studio instead
    1. Implementing
    2. Publishing
    3. Calling


    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // Set name to query string or body data
            name = name ?? data?.name;

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
    }


--------------------------------------------------

using System.Net;

public static HttpResponseMessage Run(
    HttpRequestMessage req, 
    int num1,
    int num2,
    TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request with {num1} and {num2}");

    var addition = num1 + num2;

    var result = new Result
    {
        Addition = addition,
        Message = $"Time on the server is {DateTime.Now}"
    };

    // Fetching the name from the path parameter in the request URL
    return req.CreateResponse(HttpStatusCode.OK, result, "application/json");
}

private class Result
{
    public string Message
    {
        get;
        set;
    }

    public int Addition
    {
        get;
        set;
    }
}

----------------------------------------------------

public MainPage()
{
    InitializeComponent();

    AddButton.Clicked += async (s, e) =>
    {
        int number1 = 0, number2 = 0;

        var success = int.TryParse(Number1.Text, out number1)
            && int.TryParse(Number2.Text, out number2);

        if (!success)
        {
            await DisplayAlert("Error in inputs", "You must enter two integers", "OK");
            return;
        }

        Result.Text = "Please wait...";
        Exception error = null;

        try
        {
            var url = Url.Replace("{num1}", number1.ToString())
                .Replace("{num2}", number2.ToString());
            var result = await Client.GetStringAsync(url);

            var deserialized = JsonConvert.DeserializeObject<Result>(result);

            Result.Text = deserialized.Addition + $" {deserialized.Addition.GetType()}";
        }
        catch (Exception ex)
        {
            error = ex;
        }

        if (error != null)
        {
            Result.Text = "Error!!";
            await DisplayAlert("There was an error", error.Message, "OK");
        }
    };
}
