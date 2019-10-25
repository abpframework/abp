const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'identity',
  'ts-jest': { allowSyntheticDefaultImports: true },
};
