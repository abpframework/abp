export const enum Exception {
  FileNotFound = '[File Not Found] There is no file at "{0}" path.',
  InvalidWorkspace = '[Invalid Workspace] The angular.json should be a valid JSON file.',
  NoApi = '[API Not Available] Please double-check the source url and make sure your application is up and running.',
  NoProject = '[Project Not Found] Either define a default project in your workspace or specify the project name in schematics options.',
  NoWorkspace = '[Workspace Not Found] Make sure you are running schematics at the root directory of your workspace and it has an angular.json file.',
  RequiredApiUrl = '[API URL Required] API URL cannot be resolved. Please re-run the schematics and enter the URL to get API definitions from.',
}
