const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'account-config',
  'ts-jest': { allowSyntheticDefaultImports: true },
};
