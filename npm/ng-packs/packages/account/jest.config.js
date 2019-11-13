const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'account',
  'ts-jest': { allowSyntheticDefaultImports: true },
};
