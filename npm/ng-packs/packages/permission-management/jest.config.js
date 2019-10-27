const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'permission-management',
  'ts-jest': { allowSyntheticDefaultImports: true },
};
