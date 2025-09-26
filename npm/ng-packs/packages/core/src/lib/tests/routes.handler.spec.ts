import { Router } from '@angular/router';
import { RoutesHandler } from '../handlers/routes.handler';
import { RoutesService } from '../services/routes.service';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';

describe('Routes Handler', () => {
  let spectator: SpectatorService<RoutesHandler>;
  let handler: RoutesHandler;
  let routesService: RoutesService;
  let router: Router;

  const createService = createServiceFactory({
    service: RoutesHandler,
    providers: [
      {
        provide: RoutesService,
        useValue: {
          add: jest.fn(),
        },
      },
      {
        provide: Router,
        useValue: {
          config: [],
        },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    handler = spectator.service;
    routesService = spectator.inject(RoutesService);
    router = spectator.inject(Router);
  });

  it('should create handler successfully', () => {
    expect(handler).toBeTruthy();
  });

  it('should have routes service injected', () => {
    expect(routesService).toBeTruthy();
  });

  it('should have router injected', () => {
    expect(router).toBeTruthy();
  });
});
