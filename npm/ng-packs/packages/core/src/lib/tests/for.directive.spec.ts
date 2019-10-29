import { SpectatorDirective, createDirectiveFactory } from '@ngneat/spectator/jest';
import { ForDirective } from '../directives/for.directive';
import { uuid } from '../utils';

describe('ForDirective', () => {
  let spectator: SpectatorDirective<ForDirective>;
  let directive: ForDirective;
  const items = [0, 1, 2, 3, 4, 5];
  const createDirective = createDirectiveFactory({
    directive: ForDirective,
  });

  describe('basic', () => {
    beforeEach(() => {
      spectator = createDirective('<ul><li  *abpFor="let item of items">{{ item }}</li></ul>', {
        hostProps: { items },
      });
      directive = spectator.directive;
    });

    test('should be created', () => {
      expect(directive).toBeTruthy();
    });

    test('should be iterated', () => {
      const elements = spectator.queryAll('li');

      expect(elements[3]).toHaveText('3');
      expect(elements).toHaveLength(6);
    });

    test('should sync the DOM when change items', () => {
      (spectator.hostComponent as any).items = [10, 11, 12];
      spectator.detectChanges();
      const elements = spectator.queryAll('li');

      expect(elements[1]).toHaveText('11');
      expect(elements).toHaveLength(3);
    });

    test('should sync the DOM when add an item', () => {
      (spectator.hostComponent as any).items = [...items, 6];
      spectator.detectChanges();
      const elements = spectator.queryAll('li');

      expect(elements[6]).toHaveText('6');
      expect(elements).toHaveLength(7);
    });
  });

  describe('trackBy', () => {
    const trackByFn = (_, item) => item;
    beforeEach(() => {
      spectator = createDirective('<ul><li  *abpFor="let item of items; trackBy: trackByFn">{{ item }}</li></ul>', {
        hostProps: { items, trackByFn },
      });
      directive = spectator.directive;
    });

    test('should be setted the trackBy', () => {
      expect(directive.trackBy).toEqual(trackByFn);
    });
  });

  describe('with basic order', () => {
    beforeEach(() => {
      spectator = createDirective(
        `<ul>
          <li
            *abpFor="let item of [3,6,2];
            orderDir: 'ASC'">
            {{ item }}
          </li>
        </ul>`,
      );
      directive = spectator.directive;
    });

    test('should order by asc', () => {
      const elements = spectator.queryAll('li');
      expect(elements.map(el => el.textContent.trim())).toEqual(['2', '3', '6']);
    });
  });

  describe('with order', () => {
    beforeEach(() => {
      spectator = createDirective(
        `<ul>
          <li
            *abpFor="let item of [{value: 3}, {value: 6}, {value: 2}];
            orderBy: 'value';
            orderDir: orderDir">
            {{ item.value }}
          </li>
        </ul>`,
        {
          hostProps: { orderDir: 'ASC' },
        },
      );
      directive = spectator.directive;
    });

    test('should order by asc', () => {
      const elements = spectator.queryAll('li');
      expect(elements.map(el => el.textContent.trim())).toEqual(['2', '3', '6']);
    });

    test('should order by desc', () => {
      (spectator.hostComponent as any).orderDir = 'DESC';
      spectator.detectChanges();

      const elements = spectator.queryAll('li');
      expect(elements.map(el => el.textContent.trim())).toEqual(['6', '3', '2']);
    });
  });

  describe('with filter', () => {
    beforeEach(() => {
      spectator = createDirective(
        `<ul>
          <li
            *abpFor="let item of [{value: 'test'}, {value: 'abp'}, {value: 'volo'}];
            filterBy: 'value';
            filterVal: filterVal">
            {{ item.value }}
          </li>
        </ul>`,
        {
          hostProps: { filterVal: '' },
        },
      );
      directive = spectator.directive;
    });

    test('should not filter when filterVal is empty,', () => {
      const elements = spectator.queryAll('li');
      expect(elements.map(el => el.textContent.trim())).toEqual(['test', 'abp', 'volo']);
    });

    test('should be filtered', () => {
      (spectator.hostComponent as any).filterVal = 'volo';
      spectator.detectChanges();

      expect(spectator.query('li')).toHaveText('volo');
    });

    test('should not show an element when filter value not match to any text', () => {
      (spectator.hostComponent as any).filterVal = 'volos';
      spectator.detectChanges();

      const elements = spectator.queryAll('li');
      expect(elements).toHaveLength(0);
    });
  });

  describe('with empty ref', () => {
    beforeEach(() => {
      spectator = createDirective(
        `<ul>
          <li
            *abpFor="let item of items;
            emptyRef: empty">
            {{ item.value }}
          </li>

          <ng-template #empty>No records found</ng-template>
        </ul>`,
        {
          hostProps: { items: [] },
        },
      );
      directive = spectator.directive;
    });

    test('should display the empty ref', () => {
      expect(spectator.query('ul')).toHaveText('No records found');
      expect(spectator.queryAll('li')).toHaveLength(0);
    });

    test('should not display the empty ref', () => {
      expect(spectator.query('ul')).toHaveText('No records found');
      expect(spectator.queryAll('li')).toHaveLength(0);

      (spectator.hostComponent as any).items = [0];
      spectator.detectChanges();

      expect(spectator.query('ul')).not.toHaveText('No records found');
      expect(spectator.queryAll('li')).toHaveLength(1);
    });
  });
});
