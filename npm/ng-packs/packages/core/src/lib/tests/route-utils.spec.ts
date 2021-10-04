import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator/jest';
import { RouterOutletComponent } from '../components';
import { RoutesService } from '../services/routes.service';
import { findRoute, getRoutePath } from '../utils/route-utils';

@Component({ template: '' })
class DummyComponent {}

describe('Route Utils', () => {
  describe('#findRoute', () => {
    const node = { path: '/foo' };

    test.each`
      path              | expected | count
      ${'/foo/bar/baz'} | ${node}  | ${3}
      ${'/foo/bar'}     | ${node}  | ${2}
      ${'/foo'}         | ${node}  | ${1}
      ${'/'}            | ${null}  | ${1}
    `(
      'should find $expected in $count turns when path is $path',
      async ({ path, expected, count }) => {
        const find = jest.fn(cb => (cb(node) ? node : null));
        const routes = { find } as any as RoutesService;
        const route = findRoute(routes, path);
        expect(route).toBe(expected);
        expect(find).toHaveBeenCalledTimes(count);
      },
    );
  });

  describe('#getRoutePath', () => {
    let spectator: SpectatorRouting<RouterOutletComponent>;
    const createRouting = createRoutingFactory({
      component: RouterOutletComponent,
      stubsEnabled: false,
      declarations: [DummyComponent],
      imports: [RouterModule],
      routes: [
        {
          path: '',
          children: [
            {
              path: 'foo',
              children: [
                {
                  path: 'bar',
                  children: [
                    {
                      path: 'baz',
                      component: DummyComponent,
                    },
                  ],
                },
              ],
            },
          ],
        },
      ],
    });

    beforeEach(async () => {
      spectator = createRouting();
    });

    test.each`
      url               | expected
      ${''}             | ${'/'}
      ${'/'}            | ${'/'}
      ${'/foo'}         | ${'/foo'}
      ${'/foo/bar'}     | ${'/foo/bar'}
      ${'/foo/bar/baz'} | ${'/foo/bar/baz'}
      ${'/foo?bar=baz'} | ${'/foo'}
      ${'/foo#bar'}     | ${'/foo'}
    `('should return $expected when url is $url', async ({ url, expected }) => {
      await spectator.router.navigateByUrl(url);
      expect(getRoutePath(spectator.router)).toBe(expected);
    });
  });
});
