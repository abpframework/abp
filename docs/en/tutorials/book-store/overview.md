# Web Application Development Tutorial
````json
//[doc-params]
{
    "UI": ["MVC","Blazor","BlazorServer","NG"],
    "DB": ["EF","Mongo"]
}
````
````json
//[doc-nav]
{
  "Next": {
    "Name": "Creating the server side",
    "Path": "tutorials/book-store/part-01"
  }
}
````

## About This Tutorial

In this tutorial series, you will build an ABP based web application named `Acme.BookStore`. This application is used to manage a list of books and their authors. It is developed using the following technologies:

* **{{DB_Value}}** as the database provider.
* **{{UI_Value}}** as the UI Framework.

This tutorial is organized as the following parts:

- [Part 1: Creating the server side](part-01.md)
- [Part 2: The book list page](part-02.md)
- [Part 3: Creating, updating and deleting books](part-03.md)
- [Part 4: Integration tests](part-04.md)
- [Part 5: Authorization](part-05.md)
- [Part 6: Authors: Domain layer](part-06.md)
- [Part 7: Authors: Database Integration](part-07.md)
- [Part 8: Authors: Application Layer](part-08.md)
- [Part 9: Authors: User Interface](part-09.md)
- [Part 10: Book to Author Relation](part-10.md)

### Download the Source Code

This tutorial has multiple versions based on your **UI** and **Database** preferences. We've prepared a few combinations of the source code to be downloaded:

* [MVC (Razor Pages) UI with EF Core](https://github.com/abpframework/abp-samples/tree/master/BookStore-Mvc-EfCore)
* [Blazor UI with EF Core](https://github.com/abpframework/abp-samples/tree/master/BookStore-Blazor-EfCore)
* [Angular UI with MongoDB](https://github.com/abpframework/abp-samples/tree/master/BookStore-Angular-MongoDb)

> If you encounter the "filename too long" or "unzip" error on Windows, please see [this guide](../../kb/windows-path-too-long-fix.md).

{{if UI == "MVC" && DB == "EF"}}

### Video Tutorial

This part is also recorded as a video tutorial and **<a href="https://www.youtube.com/watch?v=cJzyIFfAlp8&list=PLsNclT2aHJcPNaCf7Io3DbMN6yAk_DgWJ&index=1" target="_blank">published on YouTube</a>**.

{{end}}
