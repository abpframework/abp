const { pathsToModuleNameMapper } = require('ts-jest/utils');
const { compilerOptions } = require('./tsconfig.spec');
const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'core',
  moduleNameMapper: pathsToModuleNameMapper(compilerOptions.paths /*, { prefix: '<rootDir>/' } */),
  'ts-jest': { allowSyntheticDefaultImports: true },
};
