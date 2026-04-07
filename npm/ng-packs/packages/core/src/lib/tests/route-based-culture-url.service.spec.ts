import { Location } from '@angular/common';
import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { ConfigStateService } from '../services/config-state.service';
import { RouteBasedCultureUrlService } from '../services/route-based-culture-url.service';
import { SessionStateService } from '../services/session-state.service';

const languages = [
  { cultureName: 'en', displayName: 'English' },
  { cultureName: 'tr', displayName: 'Turkish' },
];

describe('RouteBasedCultureUrlService', () => {
  let service: RouteBasedCultureUrlService;
  let configState: { getOne: ReturnType<typeof vi.fn> };
  let sessionState: {
    getLanguage: ReturnType<typeof vi.fn>;
    setLanguage: ReturnType<typeof vi.fn>;
  };

  function setup(useRouteBasedCulture: boolean) {
    configState = {
      getOne: vi.fn((key: string) => {
        if (key === 'localization') {
          return { useRouteBasedCulture, languages };
        }
        return undefined;
      }),
      getAll$: () => of({}),
    };
    sessionState = {
      getLanguage: vi.fn(() => 'tr'),
      setLanguage: vi.fn(),
    };

    TestBed.configureTestingModule({
      providers: [
        RouteBasedCultureUrlService,
        { provide: ConfigStateService, useValue: configState },
        { provide: SessionStateService, useValue: sessionState },
        {
          provide: Router,
          useValue: { navigateByUrl: vi.fn(() => Promise.resolve(true)), url: '/tr' },
        },
        { provide: Location, useValue: { path: () => '/tr/home' } },
      ],
    });

    service = TestBed.inject(RouteBasedCultureUrlService);
  }

  afterEach(() => {
    TestBed.resetTestingModule();
  });

  test('prefixPathWithCulture leaves path unchanged when useRouteBasedCulture is false', () => {
    setup(false);
    expect(service.prefixPathWithCulture('/identity/users')).toBe('/identity/users');
  });

  test('prefixPathWithCulture prepends session culture when enabled', () => {
    setup(true);
    expect(service.prefixPathWithCulture('/identity/users')).toBe('/tr/identity/users');
  });

  test('stripCulturePrefixIfEnabled removes leading culture segment', () => {
    setup(true);
    expect(service.stripCulturePrefixIfEnabled('/en/identity/users')).toBe('/identity/users');
  });

  test('rewritePathToCulture replaces existing culture segment', () => {
    setup(true);
    expect(service.rewritePathToCulture('/en/home?x=1', 'tr')).toBe('/tr/home?x=1');
  });

  test('rewritePathToCulture prepends culture when missing', () => {
    setup(true);
    expect(service.rewritePathToCulture('/identity/users', 'tr')).toBe('/tr/identity/users');
  });
});
