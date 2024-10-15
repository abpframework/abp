# What is Angular Schematics?

![Cover Image](cover.png)

**Angular Schematics** is a powerful tool which is part of the Angular CLI that allows developers to automate various development tasks by **generating and modifying code**. Schematics provides a way to create **templates and boilerplate code** for Angular applications or libraries, enabling consistency and reducing the amount of repetitive work.

### Key Concepts of Angular Schematics:

1. **Code Generation:**
   - Schematics can generate boilerplate code for various Angular artifacts, such as components, services, modules, pipes... For example, when you run a command like `ng generate component my-component`, Angular uses a schematic to create the component files and update the necessary modules.

2. **Code Modification:**
   - Schematics can also modify existing code in your project. This includes tasks like updating configuration files, adding new routes, or modifying Angular modules. They can be very useful when upgrading projects to a new version of Angular or adding new features that require changes to the existing codebase.

3. **Custom Schematics:**
   - Developers can create their own custom schematics to automate repetitive tasks specific to their projects. For example, if your team frequently needs to set up a certain type of service or configure specific libraries, you can create a schematic that automates these steps. In [ABP Suite](https://abp.io/suite), we use the custom schematics templates to generate CRUD pages for Angular UI.

4. **Collection of Schematics:**
   - Schematics are often grouped into collections. The Angular CLI itself is a collection of schematics. You can create your own collection or use third-party schematic collections created by the Angular community.

5. **Integration with Angular CLI:**
   - Schematics is integrated with the Angular CLI. The `ng generate` and `ng add` commands are examples of how schematics are used in everyday Angular development. When you use these commands, the Angular CLI runs the corresponding schematic to perform the desired operation.

6. **Upgrade Tasks:**
   - Schematics can be used to automate the process of upgrading Angular applications. For example, the Angular Update command (`ng update`) uses schematics to automatically apply necessary changes to your project when upgrading to a new version of Angular.

### Common Use Cases:

- **Generating Components, Services, Modules.:** Easily create Angular building blocks with commands like `ng generate component`, `ng generate service`, or `ng generate module`.
  
- **Adding Libraries:** Automatically set up and configure third-party libraries with `ng add`. For example, `ng add @angular/material` uses a schematic to install and configure Angular Material in your project.

- **Automating Project Upgrades:** Simplify the process of upgrading Angular versions by running `ng update`, which uses schematics to make necessary code changes.

- **Custom Project Scaffolding:** Create custom schematics to enforce your team's development standards and best practices by automating the creation of specific project structures.

### How to Create a Custom Schematic:

Creating a custom schematic involves several steps:
1. **Install the Schematics CLI:**
   ```bash
   npm install -g @angular-devkit/schematics-cli
   ```

2. **Create a New Schematic Project:**
   ```bash
   schematics blank --name=my-schematic
   cd my-schematic
   ```

3. **Define Your Schematic:** Modify the files in the schematic project to define what your schematic will generate or modify.

4. **Test Your Schematic:** You can run your schematic locally using the `schematics` command.

5. **Publish Your Schematic (Optional):** Once your schematic is ready, you can publish it to npm or include it in your Angular projects.

#### Example:
If you want to create a custom component with specific settings or styles, you can create a schematic to automate this. Every time a developer on your team needs to create this type of component, they can run the schematic, ensuring consistency across the project.



### Helpful Links

Here are the direct links for the Angular Schematics resources:

- [Angular Official Documentation: Schematics Overview — angular.io](https://angular.io/guide/schematics)

- [Creating Custom Schematics — angular.io](https://angular.io/guide/schematics-for-libraries)

- [CLI Schematic Command — angular.io](https://angular.io/cli/schematic)

- [Devkit Schematics— github.com](https://github.com/angular/angular-cli/tree/main/packages/angular_devkit/schematics)

- [Schematics Examples — github.com](https://github.com/angular/angular-cli/tree/main/packages/schematics/angular)

- [Debugging Angular Schematics — dev.to](https://dev.to/nikolasleblanc/debugging-angular-schematics-1ne0)

- [Using Schematics with Nx — nx.dev](https://nx.dev/guides/using-angular-schematics)

- [Schematics API — angular.io](https://angular.io/api/schematics)



### Conclusion:
Angular Schematics is a powerful tool for automating repetitive tasks, generating consistent code, and managing project upgrades. By leveraging schematics, Angular developers can save time, reduce errors, and enforce best practices across their projects.