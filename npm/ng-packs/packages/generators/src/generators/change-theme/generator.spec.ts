import { createTreeWithEmptyWorkspace } from '@nx/devkit/testing';
import { Tree } from '@nx/devkit';

import { changeThemeGenerator } from './generator';
import { ChangeThemeGeneratorSchema } from './schema';

vi.mock('@nx/devkit/ngcli-adapter', () => ({
  wrapAngularDevkitSchematic: vi.fn(() => vi.fn()),
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
