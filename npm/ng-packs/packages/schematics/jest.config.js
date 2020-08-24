const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'schematics',
  testMatch: ['<rootDir>/packages/schematics/**/+(*.)+(spec).+(ts)'],
};
