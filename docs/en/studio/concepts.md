# ABP Studio Concepts

````json
//[doc-nav]
{
  "Next": {
    "Name": "Overview",
    "Path": "studio/overview"
  }
}
````

We use some concepts and terms in ABP Studio and the documentation that may not be clear when you first see. Some of them may seem new to you, or might be used in other meaning in different domains.

In this document, we are trying to clearly define these terms and concepts clearly, so you don't confuse.

## All Concepts and Terms

### Solution

*Typically referred as "ABP Studio Solution", or just "Solution".*

An ABP Studio solution is the most-top container that contains all the applications, modules and packages of your product or solution.

An ABP Studio solution may contain zero, one or many .NET solutions. For example, in a microservice solution, it may contain a separate .NET solution for each microservice, so you have multiple .NET solutions in your microservice solution. In such a scenario, each microservice (typically each separate .NET solution) will be a separate ABP Studio module.

### Module

*Typically referred as "ABP Studio Module", or just "Module".*

An ABP Studio module is a sub-solution that contains zero, one or multiple packages. A module typically have a corresponding .NET solution with one or multiple .NET projects (e.g. `csproj`). A .NET project is called as *Package* in ABP Studio.

### Package

*Typically referred as "ABP Studio Package", or just "Package".*

An ABP Studio Package typically matches to a .NET project (`csproj`).

## ABP Studio vs .NET Terms

Some ABP Studio terms may seem conflict with .NET and Visual Studio. To make them even more clear, you can use the following table.

| ABP Studio | .NET / Visual Studio   |
| ---------- | ---------------------- |
| Solution   | *- no matching term -* |
| Module     | Solution               |
| Package    | Project                |

In essence, ABP Studio uses the solution term to cover all the .NET solutions and other components of your product. ABP Studio is used to build and manage the relations of these multiple .NET solutions and provides a high-level view of the whole system.
