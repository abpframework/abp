import { createTreeWithEmptyWorkspace } from '@nx/devkit/testing';
import { Tree, readProjectConfiguration } from '@nx/devkit';

import { changeThemeGenerator } from './generator';
import { ChangeThemeGeneratorSchema } from './schema';

jest.mock('@nx/devkit/ngcli-adapter', () => ({
  wrapAngularDevkitSchematic: jest.fn(() => jest.fn()),
}));

describe('change-theme generator', () => {
  let tree: Tree;
  const options: ChangeThemeGeneratorSchema = { name: 1, targetProject: 'test' };

  beforeEach(() => {
    tree = createTreeWithEmptyWorkspace();
  });

  it('should run successfully', async () => {
    const result = await changeThemeGenerator(tree, options);
    expect(result).toBeDefined();
    expect(typeof result).toBe('function');
  });
});
