const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'tenant-management',
  'ts-jest': { allowSyntheticDefaultImports: true },
};
