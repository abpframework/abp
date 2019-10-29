const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'feature-management',
  'ts-jest': { allowSyntheticDefaultImports: true },
};
