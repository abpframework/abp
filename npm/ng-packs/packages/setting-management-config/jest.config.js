const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'identity-config',
  'ts-jest': { allowSyntheticDefaultImports: true },
};
