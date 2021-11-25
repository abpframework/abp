import { workspaces } from '@angular-devkit/core';

export interface Project {
  name: string;
  definition: workspaces.ProjectDefinition;
}
