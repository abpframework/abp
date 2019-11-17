module.exports = {
  globals: {
    'ts-jest': {
      allowSyntheticDefaultImports: true,
    },
  },
  transform: {
    '^.+\\.(ts|js|html)$': 'ts-jest',
  },
  moduleFileExtensions: ['ts', 'js', 'html'],
  coverageDirectory: '<rootDir>/coverage',
  coverageReporters: ['html'],
  preset: 'jest-preset-angular',
  setupFilesAfterEnv: ['<rootDir>/test-setup.ts', 'jest-canvas-mock'],
  snapshotSerializers: [
    'jest-preset-angular/AngularSnapshotSerializer.js',
    'jest-preset-angular/HTMLCommentSerializer.js',
  ],
};
