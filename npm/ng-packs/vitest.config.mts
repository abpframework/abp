import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    projects: [
      './packages/core/vitest.config.mts',
      './packages/theme-basic/vitest.config.mts',
      './packages/theme-shared/vitest.config.mts',
      './packages/oauth/vitest.config.mts',
      './packages/generators/vitest.config.mts',
      './packages/schematics/vitest.config.mts',
    ],
  },
});