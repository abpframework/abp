import { SpectatorDirective, createDirectiveFactory } from '@ngneat/spectator/jest';
import { TableModule, Table } from 'primeng/table';
import { TableSortDirective } from '../directives/table-sort.directive';

describe('TableSortDirective', () => {
  let spectator: SpectatorDirective<TableSortDirective>;
  let directive: TableSortDirective;
  const createDirective = createDirectiveFactory({
    directive: TableSortDirective,
    imports: [TableModule],
  });

  beforeEach(() => {
    spectator = createDirective(`<p-table [value]="[1,4,2]" [abpTableSort]="{ order: 'asc' }"></p-table>`);
    directive = spectator.directive;
  });

  test('should be created', () => {
    expect(directive).toBeTruthy();
  });

  test('should change table value', () => {
    expect(directive.value).toEqual([1, 4, 2]);
    const table = spectator.query(Table);
    expect(table.value).toEqual([1, 2, 4]);
  });
});
