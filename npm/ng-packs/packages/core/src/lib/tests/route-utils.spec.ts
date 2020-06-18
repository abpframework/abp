import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator/jest';
import { RouterOutletComponent } from '../components';
import { getRoutePath } from '../utils/route-utils';

// tslint:disable-next-line
@Component({ template: '' })
class DummyComponent {}

describe('Route Utils', () => {
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
