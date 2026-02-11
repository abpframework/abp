import {
  Rule,
  SchematicsException,
  Tree,
  apply,
  url,
  mergeWith,
  MergeStrategy,
  filter,
  chain,
} from '@angular-devkit/schematics';
import { join, normalize } from '@angular-devkit/core';
import { AiConfigSchema, AiTool } from './model';
import { getWorkspace } from '../../utils';

export default function (options: AiConfigSchema): Rule {
  return async (tree: Tree) => {
    if (!options.tool || options.tool.trim() === '') {
      console.log('ℹ️  No AI tools selected. Skipping configuration generation.');
      console.log('');
      console.log('💡 Usage examples:');
      console.log('   ng g @abp/ng.schematics:ai-config --tool=claude,cursor');
      console.log('   ng g @abp/ng.schematics:ai-config --tool="claude, cursor"');
      console.log('   ng g @abp/ng.schematics:ai-config --tool=gemini --tool=cursor');
      console.log('   ng g @abp/ng.schematics:ai-config --tool=gemini --target-project=my-app');
      console.log('');
      console.log('Available tools: claude, copilot, cursor, gemini, junie, windsurf');
      return tree;
    }

    const tools = options.tool.split(/[\s,]+/).filter(t => t) as AiTool[];

    const validTools: AiTool[] = ['claude', 'copilot', 'cursor', 'gemini', 'junie', 'windsurf'];
    const invalidTools = tools.filter(tool => !validTools.includes(tool));
    if (invalidTools.length > 0) {
      throw new SchematicsException(
        `Invalid AI tool(s): ${invalidTools.join(', ')}. Valid options are: ${validTools.join(', ')}`,
      );
    }

    if (tools.length === 0) {
      console.log('ℹ️  No AI tools selected. Skipping configuration generation.');
      return tree;
    }

    const workspace = await getWorkspace(tree);
    let targetPath = '/';

    if (options.targetProject) {
      const trimmedTargetProject = options.targetProject.trim();
      const project = workspace.projects.get(trimmedTargetProject);
      if (!project) {
        throw new SchematicsException(`Project "${trimmedTargetProject}" not found in workspace.`);
      }
      targetPath = normalize(project.root);
    }

    console.log('🚀 Generating AI configuration files...');
    console.log(`📁 Target path: ${targetPath}`);
    console.log(`🤖 Selected tools: ${tools.join(', ')}`);

    const rules: Rule[] = tools.map(tool =>
      generateConfigForTool(tool, targetPath, options.overwrite || false),
    );

    return chain([
      ...rules,
      (tree: Tree) => {
        console.log('✅ AI configuration files generated successfully!');
        console.log('\n📝 Generated files:');

        tools.forEach(tool => {
          const configPath = getConfigPath(tool, targetPath);
          console.log(`   - ${configPath}`);
        });

        console.log('\n💡 Tip: Restart your IDE or AI tool to apply the new configurations.');

        return tree;
      },
    ]);
  };
}

function generateConfigForTool(tool: AiTool, targetPath: string, overwrite: boolean): Rule {
  return (tree: Tree) => {
    const configPath = getConfigPath(tool, targetPath);

    if (tree.exists(configPath) && !overwrite) {
      console.log(`⚠️  Configuration file already exists: ${configPath}`);
      console.log(`   Use --overwrite flag to replace existing files.`);
      return tree;
    }

    const sourceDir = `./files/${tool}`;
    const source = apply(url(sourceDir), [
      filter(path => {
        return !path.endsWith('.DS_Store');
      }),
    ]);

    return mergeWith(source, overwrite ? MergeStrategy.Overwrite : MergeStrategy.Default);
  };
}

function getConfigPath(tool: AiTool, basePath: string): string {
  const configFiles: Record<AiTool, string> = {
    claude: '.claude/CLAUDE.md',
    copilot: '.github/copilot-instructions.md',
    cursor: '.cursor/rules/cursor.mdc',
    gemini: '.gemini/GEMINI.md',
    junie: '.junie/guidelines.md',
    windsurf: '.windsurf/rules/guidelines.md',
  };

  const configFile = configFiles[tool];
  return join(normalize(basePath), configFile);
}
