const { pathsToModuleNameMapper } = require('ts-jest/utils');
const { compilerOptions } = require('./tsconfig.spec');
const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'theme-shared',
  moduleNameMapper: pathsToModuleNameMapper(compilerOptions.paths /*, { prefix: '<rootDir>/' } */),
  'ts-jest': { allowSyntheticDefaultImports: true },
};
