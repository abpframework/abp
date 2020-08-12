export const enum Exception {
  InvalidWorkspace = 'Invalid Workspace: The angular.json should be a valid JSON file.',
  NoProject = 'Unknown Project: Either define a default project in your workspace or specify the project name in schematics options.',
  NoWorkspace = 'Workspace Not Found: Make sure you are running schematics at the root directory of your workspace and it has an angular.json file.',
}
