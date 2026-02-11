import 'zone.js';
import 'zone.js/testing';
import { getTestBed } from '@angular/core/testing';
import { BrowserTestingModule, platformBrowserTesting } from '@angular/platform-browser/testing';

// Initialize Angular testing environment
getTestBed().initTestEnvironment(BrowserTestingModule, platformBrowserTesting());

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
