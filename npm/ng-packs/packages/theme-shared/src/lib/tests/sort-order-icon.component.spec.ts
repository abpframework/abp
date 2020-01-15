import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { SortOrderIconComponent } from '../components/sort-order-icon/sort-order-icon.component';

describe('SortOrderIconComponent', () => {
  let spectator: SpectatorHost<SortOrderIconComponent>;
  let component: SortOrderIconComponent;
  const createHost = createHostFactory(SortOrderIconComponent);

  beforeEach(() => {
    spectator = createHost(
      '<abp-sort-order-icon sortKey="testKey" [(selectedSortKey)]="selectedSortKey" [(order)]="order"></abp-sort-order-icon>',
      {
        hostProps: {
          selectedSortKey: '',
          order: '',
        },
      },
    );
    component = spectator.component;
  });

  test('should have correct icon class when selectedSortKey and sortKey are the same', () => {
    const newKey = 'testKey';
    component.sort(newKey);
    expect(component.selectedSortKey).toBe(newKey);
    expect(component.order).toBe('asc');
    expect(component.icon).toBe('sorting_asc');
  });

  test("shouldn't have any icon class when sortKey and selectedSortKey are different", () => {
    const newKey = 'otherKey';
    component.sort(newKey);
    expect(component.selectedSortKey).toBe(newKey);
    expect(component.order).toBe('asc');
    expect(component.icon).toBe('sorting');
  });

  test('should change order correctly when sort function called', () => {
    component.sort('testKey');
    expect(component.order).toBe('asc');
    component.sort('testKey');
    expect(component.order).toBe('desc');
    component.sort('testKey');
    expect(component.order).toBe('');
  });
});
