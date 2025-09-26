import { setupZoneTestEnv } from 'jest-preset-angular/setup-env/zone';
setupZoneTestEnv();

const originalError = console.error;
console.error = (...args: any[]) => {
  if (args[0]?.includes?.('ExpressionChangedAfterItHasBeenCheckedError')) {
    return;
  }
  originalError.apply(console, args);
};
