import { createTreeWithEmptyWorkspace } from '@nx/devkit/testing';
import { Tree, readProjectConfiguration } from '@nx/devkit';

import { generateProxyGenerator } from './generator';
import { GenerateProxyGeneratorSchema } from './schema';

describe('generate-proxy generator', () => {
  let tree: Tree;
  const options: GenerateProxyGeneratorSchema = { name: 'test' };

  beforeEach(() => {
    tree = createTreeWithEmptyWorkspace();
  });

  it('should run successfully', async () => {
    await generateProxyGenerator(tree, options);
    const config = readProjectConfiguration(tree, 'test');
    expect(config).toBeDefined();
  });
});
