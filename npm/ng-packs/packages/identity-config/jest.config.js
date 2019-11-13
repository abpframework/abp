const jestConfig = require('../../jest.config');

module.exports = {
  ...jestConfig,
  name: 'setting-management-config',
  'ts-jest': { allowSyntheticDefaultImports: true },
};
