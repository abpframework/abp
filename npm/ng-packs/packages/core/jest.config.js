const { pathsToModuleNameMapper } = require('ts-jest/utils');
const { compilerOptions } = require('./tsconfig.spec');

module.exports = {
  name: 'core',
  testMatch: ['<rootDir>/packages/core/**/+(*.)+(spec|test).+(ts|js)?(x)'],
  coverageDirectory: '../../coverage/libs/core',
  moduleNameMapper: pathsToModuleNameMapper(compilerOptions.paths /*, { prefix: '<rootDir>/' } */),
};
