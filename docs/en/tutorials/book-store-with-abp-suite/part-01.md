```json
//[doc-seo]
{
    "Description": "Learn to create a new web application solution using ABP Suite, with step-by-step guidance tailored for MVC, Blazor, and various databases."
}
```

# Web Application Development Tutorial (with ABP Suite) - Part 1: Creating the Solution

````json
//[doc-params]
{
    "UI": ["MVC","Blazor","BlazorServer", "BlazorWebApp","NG","MAUIBlazor"],
    "DB": ["EF", "Mongo"]
}
````

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Overview",
    "Path": "tutorials/book-store-with-abp-suite/index"
  },
  "Next": {
    "Name": "Creating the Books",
    "Path": "tutorials/book-store-with-abp-suite/part-02"
  }
}
````

Before starting the development, create a new solution named `Acme.BookStore` and run it by following the [getting started tutorial](../../get-started/layered-web-application.md).

You can use the following configurations:

* **Solution Template:** Application (Layered)
* **Solution Name:** `Acme.BookStore`
* **UI Framework:** {{UI_Value}}
* **UI Theme:** LeptonX
* **Mobile Framework:** None
* **Database Provider:** {{DB_Value}}
* **Public Website:** No
* **Tiered:** No
* **Sample Crud Page:** No

You can select the other options based on your preference.

> **Please complete the [Get Started](../../get-started/layered-web-application.md) guide and run the web application before going further.**

> **Please do not check the `Sample Crud Page` option while creating the solution, since it conflicts with this tutorial.**

## Summary

We've created the initial layered monolith solution. In the next part, we will learn how to create entities, and generate CRUD pages based on the specified options (including tests, UI, customizable code support etc.) with [ABP Suite](../../suite/index.md).
