import { getJestProjectsAsync } from '@nx/jest';
/**
 * @deprecated use vitest instead of jest
 * @see https://vitest.dev/guide/migration.html#jest
 */
export default async () => ({
  projects: await getJestProjectsAsync(),
});
