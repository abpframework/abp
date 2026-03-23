/* eslint-disable */
/**
 * @deprecated use vitest instead of jest
 * @see https://vitest.dev/guide/migration.html#jest
 */
export default {
  displayName: 'schematics',
  preset: '../../jest.preset.js',
  testEnvironment: 'node',
  coverageDirectory: '../../coverage/packages/schematics',
  transform: {
    '^.+\\.[tj]s$': ['ts-jest', { tsconfig: '<rootDir>/tsconfig.spec.json' }],
  },
  moduleFileExtensions: ['ts', 'js', 'html'],
};
