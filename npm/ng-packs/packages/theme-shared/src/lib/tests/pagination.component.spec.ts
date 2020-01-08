import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { PaginationComponent } from '../components';

describe('PaginationComponent', () => {
  let spectator: SpectatorHost<PaginationComponent>;
  const createHost = createHostFactory({
    component: PaginationComponent,
  });

  beforeEach(() => {
    spectator = createHost(
      '<abp-pagination [totalPages]="totalPages" [value]="value"></abp-pagination>',
      {
        hostProps: {
          value: 5,
          totalPages: 12,
        },
      },
    );
  });

  it('should add ui-state-active class to current page', () => {
    expect(spectator.query('.ui-state-active').textContent).toBe('5');
  });

  it('should display the correct pages', () => {
    expect(spectator.queryAll('.ui-paginator-page').map(node => node.textContent)).toEqual([
      '3',
      '4',
      '5',
      '6',
      '7',
    ]);

    spectator.click('.ui-paginator-first');

    expect(spectator.queryAll('.ui-paginator-page').map(node => node.textContent)).toEqual([
      '1',
      '2',
      '3',
      '4',
      '5',
    ]);

    spectator.setHostInput({ value: 12 });

    expect(spectator.queryAll('.ui-paginator-page').map(node => node.textContent)).toEqual([
      '8',
      '9',
      '10',
      '11',
      '12',
    ]);

    spectator.setHostInput({ value: 1, totalPages: 3 });

    expect(spectator.queryAll('.ui-paginator-page').map(node => node.textContent)).toEqual([
      '1',
      '2',
      '3',
    ]);
  });
});
