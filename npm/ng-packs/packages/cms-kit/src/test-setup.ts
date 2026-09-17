import { setupZoneTestEnv } from 'jest-preset-angular/setup-env/zone';

setupZoneTestEnv();

// Optional: align with core package behavior and provide a stable window.location
Object.defineProperty(window, 'location', {
  value: {
    href: 'http://localhost:4200',
    origin: 'http://localhost:4200',
    pathname: '/',
    search: '',
    hash: '',
  },
  writable: true,
});
