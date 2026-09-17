import '@angular/compiler';
import 'zone.js';
import 'zone.js/testing';
import { BrowserTestingModule, platformBrowserTesting } from '@angular/platform-browser/testing';
import {
  ɵgetCleanupHook as getCleanupHook,
  getTestBed
} from '@angular/core/testing';


beforeEach(getCleanupHook(false));
afterEach(getCleanupHook(true));

// Initialize Angular testing environment
getTestBed().initTestEnvironment(BrowserTestingModule, platformBrowserTesting());


// Mock window.location for test environment
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
