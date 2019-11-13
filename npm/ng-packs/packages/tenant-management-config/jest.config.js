const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'tenant-management-config',
  'ts-jest': { allowSyntheticDefaultImports: true },
};
