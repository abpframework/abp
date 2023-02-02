import { Rule, SchematicsException } from '@angular-devkit/schematics';
import { isLibrary, updateWorkspace, WorkspaceDefinition } from '../../utils';
import { allStyles, styleMap } from './style-map';
import { ProjectDefinition } from '@angular-devkit/core/src/workspace';
import { JsonArray, JsonValue } from '@angular-devkit/core';
import { ChangeThemeOptions } from './model';
import { ThemeOptionsEnum } from './theme-options.enum';

export default function (_options: ChangeThemeOptions): Rule {
  return async () => {
    const targetThemeName = _options.name;
    const selectedProject = _options.targetProject;
    if (!targetThemeName) {
      throw new SchematicsException('The theme name does not selected');
    }

    return updateWorkspace(storedWorkspace => {
      updateProjectStyle(selectedProject, storedWorkspace, targetThemeName);
    });
  };
}

function updateProjectStyle(
  projectName: string,
  workspace: WorkspaceDefinition,
  targetThemeName: ThemeOptionsEnum,
) {
  const project = workspace.projects.get(projectName);

  if (!project) {
    throw new SchematicsException('The target project does not selected');
  }

  const isProjectLibrary = isLibrary(project);
  if (isProjectLibrary) {
    throw new SchematicsException('The library project does not supported');
  }

  const targetOption = getProjectTargetOptions(project, 'build');
  const styles = targetOption.styles as (string | { input: string })[];

  const sanitizedStyles = removeThemeBasedStyles(styles);

  const newStyles = styleMap.get(targetThemeName);
  if (!newStyles) {
    throw new SchematicsException('The theme does not found');
  }
  targetOption.styles = [...newStyles, ...sanitizedStyles] as JsonArray;
}

export function getProjectTargetOptions(
  project: ProjectDefinition,
  buildTarget: string,
): Record<string, JsonValue | undefined> {
  const options = project.targets?.get(buildTarget)?.options;

  if (!options) {
    throw new SchematicsException(
      `Cannot determine project target configuration for: ${buildTarget}.`,
    );
  }

  return options;
}

export function removeThemeBasedStyles(styles: (string | object)[]) {
  return styles.filter(s => !allStyles.some(x => styleCompareFn(s, x)));
}

export const styleCompareFn = (item1: string | object, item2: string | object) => {
  const type1 = typeof item1;
  const type2 = typeof item1;

  if (type1 !== type2) {
    return false;
  }

  if (type1 === 'string') {
    return item1 === item2;
  }
  const o1 = item1 as { bundleName?: string };
  const o2 = item2 as { bundleName?: string };

  return o1.bundleName && o2.bundleName && o1.bundleName == o2.bundleName;
};
