# UI Tests

In general, there are two types of UI Tests:

* Non Visual Tests
* Visual Tests

## Non Visual Tests

Such tests completely depends on your UI Framework choice;

* For an MVC / Razor Pages UI, you typically make request to the server, get some HTML and test if some expected DOM elements exist in the returned result.
* Angular has its own infrastructure and practices to test the components, views and services.

See the following documents to learn Non Visual UI Testing;

* [Testing in ASP.NET Core MVC / Razor Pages](UI/AspNetCore/Testing.md)
* [Testing in Angular](UI/Angular/Testing.md)
* [Testing in Blazor](UI/Blazor/Testing.md)

## Visual Tests

Visual Tests are used to interact with the application UI just like a real user does. It fully tests the application, including the visual appearance of the pages and components.

Visual UI Testing is out of the scope for the ABP Framework. There are a lot of tooling in the industry (like [Selenium](https://www.selenium.dev/)) that you can use to test your application's UI.
