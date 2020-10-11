# ASP.NET Core MVC / Razor Pages UI: JavaScript Modal Manager

`abp.ModalManager` is used to easily manage a modal dialog in your application.

**Example: A modal dialog to create a new role entity**

![modal-manager-example-modal](../../../images/modal-manager-example-modal.png)

While you can always use the standard Bootstrap API (or another library) to create modal dialogs, `abp.ModalManager` provides the following benefits;

* **Lazy loads** the modal HTML into the page and **removes** it from the DOM once its closed. This makes easy to consume a reusable modal dialog. Also, every time you open the modal, it will be a fresh new modal, so you don't have to deal with resetting the modal content.
* **Auto-focuses** the first input of the form once the modal has been opened.
* Automatically determines the **form** inside a modal and posts the form via **AJAX** instead of normal page post.
* Automatically checks if the form inside the modal **has changed, but not saved**. It warns the user in this case.
* Automatically **disables the modal buttons** (save & cancel) until the AJAX operation completes.
* Makes it easy to register a **JavaScript object that is initialized** once the modal has loaded.

So, the `ModalManager` makes you write less code when you deal with the modals, especially the modals with a form inside.