import { TemplateRef, ɵSIGNAL as SIGNAL } from '@angular/core';
import { SpectatorDirective, createDirectiveFactory } from '@ngneat/spectator/vitest';
import { ForDirective } from '../directives/for.directive';

const setInputSignal = <T>(inputSignal: () => T, value: T) => {
  const node = inputSignal[SIGNAL];
  node.applyValueToInputSignal(node, value);
};

describe('ForDirective', () => {
  let spectator: SpectatorDirective<ForDirective>;
  let directive: ForDirective;
  const items = [0, 1, 2, 3, 4, 5];
  const createDirective = createDirectiveFactory({
    directive: ForDirective,
  });

  const renderChanges = () => {
    directive.ngOnChanges();
    spectator.fixture.detectChanges(false);
    spectator.fixture.detectChanges(false);
  };

  const resetProjection = () => {
    directive['vcRef'].clear();
    directive['lastItemsRef'] = null;
    directive['differ'] = null;
  };

  const setDirectiveInputs = (inputs: Partial<Record<keyof ForDirective, unknown>>) => {
    Object.entries(inputs).forEach(([key, value]) => {
      setInputSignal(directive[key], value);
    });
    resetProjection();
    renderChanges();
  };

  const getTexts = () => spectator.queryAll('li').map(el => el.textContent.trim());

  const getTemplateRef = node => {
    try {
      return node.injector.get(TemplateRef);
    } catch {
      return null;
    }
  };

  const getTemplateRefs = () =>
    spectator.fixture.debugElement.queryAllNodes(getTemplateRef).map(getTemplateRef);

  describe('basic', () => {
    beforeEach(() => {
      spectator = createDirective('<ul><ng-template abpFor let-item><li>{{ item }}</li></ng-template></ul>');
      directive = spectator.directive;
      setDirectiveInputs({ items });
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
      setDirectiveInputs({ items: [10, 11, 12] });

      const elements = spectator.queryAll('li');
      expect(elements[1]).toHaveText('11');
      expect(elements).toHaveLength(3);
    });

    test('should sync the DOM when add an item', () => {
      setDirectiveInputs({ items: [...items, 6] });

      const elements = spectator.queryAll('li');
      expect(elements[6]).toHaveText('6');
      expect(elements).toHaveLength(7);
    });
  });

  describe('trackBy', () => {
    const trackByFn = (_: number, item: number) => item;

    beforeEach(() => {
      spectator = createDirective('<ul><ng-template abpFor let-item><li>{{ item }}</li></ng-template></ul>');
      directive = spectator.directive;
      setDirectiveInputs({ items, trackBy: trackByFn });
    });

    test('should be setted the trackBy', () => {
      expect(directive.trackBy()).toEqual(trackByFn);
    });
  });

  describe('with basic order', () => {
    beforeEach(() => {
      spectator = createDirective('<ul><ng-template abpFor let-item><li>{{ item }}</li></ng-template></ul>');
      directive = spectator.directive;
      setDirectiveInputs({ items: [3, 6, 2], orderDir: 'ASC' });
    });

    test('should order by asc', () => {
      expect(getTexts()).toEqual(['2', '3', '6']);
    });
  });

  describe('with order', () => {
    beforeEach(() => {
      spectator = createDirective(
        '<ul><ng-template abpFor let-item><li>{{ item.value }}</li></ng-template></ul>',
      );
      directive = spectator.directive;
      setDirectiveInputs({
        items: [{ value: 3 }, { value: 6 }, { value: 2 }],
        orderBy: 'value',
        orderDir: 'ASC',
      });
    });

    test('should order by asc', () => {
      expect(getTexts()).toEqual(['2', '3', '6']);
    });

    test('should order by desc', () => {
      setDirectiveInputs({ orderDir: 'DESC' });

      expect(getTexts()).toEqual(['6', '3', '2']);
    });
  });

  describe('with filter', () => {
    beforeEach(() => {
      spectator = createDirective(
        '<ul><ng-template abpFor let-item><li>{{ item.value }}</li></ng-template></ul>',
      );
      directive = spectator.directive;
      setDirectiveInputs({
        items: [{ value: 'test' }, { value: 'abp' }, { value: 'volo' }],
        filterBy: 'value',
        filterVal: '',
      });
    });

    test('should not filter when filterVal is empty,', () => {
      expect(getTexts()).toEqual(['test', 'abp', 'volo']);
    });

    test('should be filtered', () => {
      setDirectiveInputs({ filterVal: 'volo' });

      expect(spectator.query('li')).toHaveText('volo');
    });

    test('should not show an element when filter value not match to any text', () => {
      setDirectiveInputs({ filterVal: 'volos' });

      expect(spectator.queryAll('li')).toHaveLength(0);
    });
  });

  describe('with empty ref', () => {
    beforeEach(() => {
      spectator = createDirective(`
        <ul>
          <ng-template abpFor let-item>
            <li>{{ item.value }}</li>
          </ng-template>

          <ng-template #empty>No records found</ng-template>
        </ul>
      `);
      directive = spectator.directive;
      const [, emptyRef] = getTemplateRefs();
      setDirectiveInputs({ items: [], emptyRef });
    });

    test('should display the empty ref', () => {
      expect(spectator.query('ul')).toHaveText('No records found');
      expect(spectator.queryAll('li')).toHaveLength(0);
    });

    test('should not display the empty ref', () => {
      expect(spectator.query('ul')).toHaveText('No records found');
      expect(spectator.queryAll('li')).toHaveLength(0);

      setDirectiveInputs({ items: [{ value: 0 }] });

      expect(spectator.query('ul')).not.toHaveText('No records found');
      expect(spectator.queryAll('li')).toHaveLength(1);
    });
  });
});
