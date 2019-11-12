const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'theme-basic',
  'ts-jest': { allowSyntheticDefaultImports: true },
};
