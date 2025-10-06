import { of, throwError } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { LazyLoadService } from '../services/lazy-load.service';
import { ScriptLoadingStrategy } from '../strategies/loading.strategy';
import { ResourceWaitService } from '../services/resource-wait.service';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';

describe('LazyLoadService', () => {
  let spectator: SpectatorService<LazyLoadService>;
  let service: LazyLoadService;
  let resourceWaitService: ResourceWaitService;

  const createService = createServiceFactory({
    service: LazyLoadService,
    providers: [
      {
        provide: ResourceWaitService,
        useValue: {
          wait: jest.fn(),
          addResource: jest.fn(),
        },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    resourceWaitService = spectator.inject(ResourceWaitService);
  });

  describe('#load', () => {
    const strategy = new ScriptLoadingStrategy('http://example.com/');

    afterEach(() => {
      jest.clearAllMocks();
    });

    it('should create service successfully', () => {
      expect(service).toBeTruthy();
    });

    it('should have loaded property', () => {
      expect(service.loaded).toBeDefined();
    });
  });

  describe('#remove', () => {
    it('should remove an already lazy loaded element and return true', () => {
      const script = document.createElement('script');
      document.body.appendChild(script);
      service.loaded.set('x', script);

      expect(document.body.lastElementChild).toBe(script);

      const result = service.remove('x');

      expect(document.body.lastElementChild).toBeNull();
      expect(service.loaded.has('x')).toBe(false);
      expect(result).toBe(true);
    });

    it('should return false when path not found', () => {
      service.loaded.set('foo', null);

      const result = service.remove('bar');

      expect(result).toBe(false);
    });
  });
});
