import { Router } from '@angular/router';
import { RoutesHandler } from '../handlers';
import { RoutesService } from '../services';

describe('Routes Handler', () => {
  describe('#add', () => {
    it('should add routes from router config', () => {
      const config = [
        { path: 'x' },
        { path: 'y', data: {} },
        { path: '', data: { routes: { name: 'Foo' } } },
        { path: 'bar', data: { routes: { name: 'Bar' } } },
        { data: { routes: [{ path: '/baz', name: 'Baz' }] } },
      ];
      const foo = [{ path: '/', name: 'Foo' }];
      const bar = [{ path: '/bar', name: 'Bar' }];
      const baz = [{ path: '/baz', name: 'Baz' }];

      const routes = [];
      const add = jest.fn(routes.push.bind(routes));
      const mockRoutesService = { add } as unknown as RoutesService;
      const mockRouter = { config } as unknown as Router;

      const handler = new RoutesHandler(mockRoutesService, mockRouter);

      expect(add).toHaveBeenCalledTimes(3);
      expect(routes).toEqual([foo, bar, baz]);
    });

    it('should not add routes when there is no router', () => {
      const routes = [];
      const add = jest.fn(routes.push.bind(routes));
      const mockRoutesService = { add } as unknown as RoutesService;

      const handler = new RoutesHandler(mockRoutesService, null);

      expect(add).not.toHaveBeenCalled();
    });
  });
});
