* Document Notes
  * Quick Start: Creating a Layered Web Application
    * **Disable the DEMO PLUGIN and re-take all screenshots**. Also, wait for the followings done:
      * Re-take welcome page screenshot when the large logo is removed
      * Re-take *Create new solution* wizard screenshot when we change it to Application (Layered)
      * Re-take *Solution Properties* wizard page screenshot to hide *Use local ABP* option
      * Re-take *UI Theme* wizard page screenshot to remove the old *Lepton theme* selection
      * Re-take *Database Configurations* wizard page screenshot to change connection string to LocalDb.
      * Re-take the abp-studio-created-new-solution.png (after ABP Studio logo is removed from the welcome page).
    * **Make that tutorial with options or extra sections**
      * Options
        * UI options (Angular, Blazor... etc)
        * database providers (MongoDB, EF Core)
        * Tiered / Non-Tiered selection
      * Extra sections
        * Mobile selection (none, MAUI, React Native)
        * Public website selection
* Other Notes
  * We should support MAUI Blazor Hybrid UI option as well.
  * App-pro template: Root `aspnet-core` folder should not be exists. Event if the Angular UI is selected, we can create `angular` folder inside the root folder of the .NET solution.
  * Docker-compose file is wrong for the app-pro template. There are bookstore-web and sql server for example. - Also, even when I delete these, it didn't properly started by ABP Studio. Network name is wrong. Run-docker.ps1 is also wrong...