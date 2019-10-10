const { pathsToModuleNameMapper } = require('ts-jest/utils');
const { compilerOptions } = require('./tsconfig.spec');
const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'core',
  testMatch: ['<rootDir>/packages/core/**/+(*.)+(spec|test).+(ts|js)?(x)'],
  moduleNameMapper: pathsToModuleNameMapper(compilerOptions.paths /*, { prefix: '<rootDir>/' } */),
};
