# Refactoring the Function to use JSON

One thing that we notice [when we use the Xamarin client](./firstclient.md) is that the type of the result is ```System.String```. This is because we use HTTP to communicate between the client and the server, and HTTP (HyperText Transfer Protocol) is of course text-based. As such, it means that we need an additional agreement between the client and the server, to specify the type of the result, so that the client can parse it and convert the result to the desired type. If the server developer decides to change the implementation and the type of the result, or to add more information in the result, the client developer needs to be notified and update the application accordingly. This is not very efficient.

To avoid this kind of issues, many APIs these days use the JavaScript Object Notation JSON to encode the result of the function. JSON has various advantages: It is easy to serialize/deserialize and it can be transmitted as text over HTTP. We will now update our sample implementation to use JSON instead of a simple result.

## Adding a class library

In order to share code between the client and the server, we will use a class library that we will consume on the server and on the client. The class library can be seen as a contract between client and server. Follow the steps to add the class library to the server project:

1. Open the Functions application project in Visual Studio.

2. In the Solution Explorer, right click on the Solution name and select Add, New Project from the context menu.

3. In the Add New Project dialog, enter ```portable``` in the search box and select "Class Library (Legacy Portable), Visual C#" from the dialog.

> Note: We use a Portable Class Library here (PCL) because our Function is running on .NET 4.6.1. If you selected .NET Core (V2) when you created the Azure Function, you can use a .NET Core class library here. At the moment, .NET Core doesn't work with .NET 4.6.1 projects.

4. Enter a name (for example Calculator.Data) and a location for the new project.

![Creating the class library](./Img/2018-01-08_09-46-15.png)

5. In the Add Portable Class Library dialog, select the frameworks that you want to support. Here we will need Xamarin.Android, Xamarin.iOS and Windows Universal 10.0. A few additional frameworks will be automatically selected.

![Add Portable Class Library](./Img/2018-01-08_09-48-09.png)

6. In the Calculator.Data project, rename the file Class1.cs to AdditionResult.cs

7. Replace the class with the following code. For the sake of the sample, we will transmit the result of the addition as well as the current date/time on the server. We could also easily decide to transmit more information if necessary.

```CS
public class AdditionResult
{
    public int Result
    {
        get;
        set;
    }

    public DateTime TimeOnServer
    {
        get;
        set;
    }
}
```

## Consuming the class library on the server

Now that we have our new Data class library, we will use it on the server and modify the Function accordingly.

1. Right click on the LbCalculator Function project and select Add, Reference from the context menu.

2. In the Reference Manager dialog, select the Calculator.Data class library and press OK.

![Reference Manager dialog](./Img/2018-01-08_09-53-48.png)

3. Reopen the Add.cs file in the Azure Function application.

4. Replace the ```Add``` class with the following code:

```CS
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
```
There are a few small changes to the previous version of the function:

- The result is now an instance of the ```AdditionResult``` class that we added earlier in the ```Calculator.Data``` class library. Note how we also store the date/time on the server.

- Instead of the ```addition```, we pass the ```result``` instance to the ```CreateResponse``` method now.

- We added the media type ```"application/json"``` to the ```CreateResponse``` method.

Because of the new media type, the ```result``` object will automatically be serialized to the JavaScript Object Notation JSON and passed back to the caller.

## Testing the new interface

Now we can test the new function result in the web browser.

1. In Visual Studio, select Debug, Start without Debugging from the menu bar.

2. In the Command window with the Azure Runtime that opens up, copy the debug URL from the ```Http Function``` section (in green).

3. Like before, paste the debug URL in a web browser and replace the ```{num1}``` and ```{num2}``` occurrences in the URL with operands, for example ```12``` and ```34```.

4. In the web browser, you should now see a result looking like

```json
{"Result":46,"TimeOnServer":"2018-01-08T10:11:54.5719894-08:00"}
```

Here we see that the new interface works fine, and the function now returns a JSON-formatted result. [We can now modify the Xamarin.Forms client](./refactoring-client.md) to take advantage of the new result.