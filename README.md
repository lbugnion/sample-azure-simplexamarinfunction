# Implementing a simple Azure Function with a Xamarin.Forms client

Azure Functions are a great way to implement an API in an easy manner, without any deployment or maintenance headaches. You can implement the code in the Azure Web Portal directly (or in Visual Studio if you prefer, of course). Functions are also running at a very interesting cost, since they are only billed when they are actually executed. If your business is just starting and you have only a few users, the costs will be very low. Later when the business expands, you have the possibility to switch to a different plan making it easier to budget your costs.

Because they are very easy to implement and maintain, and accessible through HTTP, Azure Functions are a great way to implement an API for a mobile application. Microsoft offers great cross-platform tools for iOS, Android and Windows with Xamarin. As such, Xamarin and Azure Functions are working great together. This article will show how to implement a very simple Azure Function in the Azure Web Portal at first, and build a cross-platform client with Xamarin.Forms, running on Android, iOS and Windows. 

Later we will refine this application by using JSON as a communication medium between the server and the mobile clients. Finally we will show how to implement the Azure Functions application in Visual Studio, where we can take advantage of additional features such as unit tests, automation, etc.

## Table of content

- [Creating the basic function](./Doc/creating.md)
- Creating and running the Xamarin.Forms client
- Modifying the application to use JSON instead
- Creating the function in Visual Studio
