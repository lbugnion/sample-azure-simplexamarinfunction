# Implementing the first version of the Xamarin.Forms client

We know that [our function works well now](./implementing.md), and we tested it in a web browser. Now we will build a Xamarin.Forms client that runs on iOS, Android and Windows to use this new API.

[In the previous step](./implementing.md), we copied the function's URL for later usage. Make sure to keep this URL handy, we will need it later in the client's code.

1. In Visual Studio 2017, select File, New, Project.

> Note: We use Visual Studio 2017 on Windows for this sample, but you can also create Xamarin.Forms applications in earlier versions of Visual Studio if you prefer. Xamarin is available for free in all versions of Visual Studio, including the free Community editions.
> - [Visual Studio Community Edition for Windows](https://www.visualstudio.com/vs/community/)
> - [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/)

2. In the New Project dialog, select the Cross-Platform category, and then Cross-Platform App (Xamarin.Forms) with Visual C#. Name the new application ```XamCalculator```, select a location for the project and press OK.

![New Project](./Img/2017-12-25_12-51-53.png)

> Note: The Cross-platform category is available if you selected the "Mobile development with .NET" workload in the Visual Studio 2017 installer.

3. In the New Cross Platform App dialog, select the following settings, then press OK.
    - Android, iOS and Windows
    - Xamarin.Forms
    - .NET Standard

![New Cross Platform App](./Img/2017-12-25_12-55-16.png)

The new application consists of 4 projects:

- XamCalculator: This is the shared user interface project, where we will implement the UI and the code calling the function. This project is referenced by each of the 3 other projects.
- XamCalculator.Android: The Android version of the application.
- XamCalculator.iOS: The iOS version of the application.
- XamCalculator.UWP: The Universal Windows Platform (UWP) of the application.

Later we will see how we can select each application to test it and run it.

4. In the XamCalculator project, select the MainPage.xaml and open it in the editor.

5. Replace the XAML code with the following:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:XamCalculator"
             x:Class="XamCalculator.MainPage">

    <StackLayout VerticalOptions="Center"
                 HorizontalOptions="Fill">

        <Entry x:Name="Number1"
               Placeholder="Enter the first integer"
               PlaceholderColor="Gray" />

        <Entry x:Name="Number2"
               Placeholder="Enter the second integer"
               PlaceholderColor="Gray" />

        <Button Text="Add"
                x:Name="AddButton" />

        <Label x:Name="Result"
               FontSize="Large"
               HorizontalOptions="Center"
               Text="Ready for operation" />

    </StackLayout>
</ContentPage>
```

The code above creates a new user interface with 4 UI elements placed under each other. The layout is performed by the ```StackLayout``` panel. By default, the StackLayout uses a vertical layout, but it could also be changed to horizontal if needed. [There are many other layout types](https://developer.xamarin.com/guides/xamarin-forms/user-interface/controls/layouts/) that can be used to create more complex layouts.

- The first and second UI elements are [Entry controls](https://developer.xamarin.com/guides/xamarin-forms/user-interface/text/#Entry) where the user will be able to enter some text. We will access this text from the code behind. Note how we use the [Placeholder propert](https://developer.xamarin.com/guides/xamarin-forms/user-interface/text/entry/#Placeholders) to show a readonly text when the field is empty. The controls are named ```Number1``` and ```Number2```.

- The third element is a [Button control](https://developer.xamarin.com/api/type/Xamarin.Forms.Button/). This control can be clicked by the user, which will create an event that we will respond to. The button is named ```AddButton```.

- The last element is a [Label control](https://developer.xamarin.com/guides/xamarin-forms/user-interface/text/#Label), used to show some simple text output to the user. The Label is named ```Result```.

6. Open the MainPage.xaml.cs now. This C# code file is what we call "code behind". This is the view's controller, where we will handle events and modify the UI accordingly.

7. 

7. Replace the ```MainPage``` constructor with the following code:

```CS
public partial class MainPage : ContentPage
{
    private const string Url = "YOUR URL HERE";

    private HttpClient _client;

    private HttpClient Client
    {
        get
        {
            if (_client == null)
            {
                _client = new HttpClient();
            }

            return _client;
        }
    }

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
                Result.Text = result + $" {result.GetType()}";
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
}
```

What the code above does is the following:

- We define a constant for the URL template for the service. You should replace the words ```YOUR URL HERE``` with the URL that [you copied in the previous step](./implementing.md).

- We define an ```HttpClient``` as a property so that we can easily reuse it. Like the name shows, the ```HttpClient``` is a class designed for interaction with servers over HTTP. It is the most convenient and simple way to access an HTTP service, such as our HTTP-Triggered function. 

- In the ```MainPage``` constructor, we will handle the Clicked event of the Button control. When this event is called, the event handler will be executed.

- We parse the text that the user entered. We want to make sure that we send integers to the server, to avoid error there. Parsing the text with the ```TryParse``` method ensures that the user input is suitable.

- If the user enters incorrect inputs, we show a warning message and we stop the execution.

- We show a message to the user saying ```Please wait```. This is because we will perform an asynchronous operation that could take a moment, and it is nice to let the user know what's happening.

The next execution block is placed in a ```try/catch``` so that we catch any potential error and inform the user accordingly. The ```HttpClient``` may throw an exception if the server is down, or if there is a server error for example. If such an exception occurs, we need to make sure that the application doesn't crash and that the user knows what happened.

- We create the URL out of the URL template declared as a constant higher up. In this URL, the first and second numbers are defined as ```{num1}``` and ```{num2}```. 

- The next line is the call to the ```GetStringAsync``` method of the ```HttpClient```. This method is asynchronous, like the name shows. This is why we use the ```await``` keyword when we call it. 

> Note: Calling the method with the ```await``` keyword