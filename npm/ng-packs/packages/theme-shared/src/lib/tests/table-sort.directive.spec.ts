import { SpectatorDirective, createDirectiveFactory } from '@ngneat/spectator/jest';
import { TableSortDirective } from '../directives/table-sort.directive';
import { TableComponent } from '../components/table/table.component';
import { DummyLocalizationPipe } from './table.component.spec';
import { PaginationComponent } from '../components';

describe('TableSortDirective', () => {
  let spectator: SpectatorDirective<TableSortDirective>;
  let directive: TableSortDirective;
  const createDirective = createDirectiveFactory({
    directive: TableSortDirective,
    declarations: [TableComponent, DummyLocalizationPipe, PaginationComponent],
  });

  beforeEach(() => {
    spectator = createDirective(
      `<abp-table [value]="[1,4,2]" [abpTableSort]="{ order: 'asc' }"></abp-table>`,
    );
    directive = spectator.directive;
  });

  test('should be created', () => {
    expect(directive).toBeTruthy();
  });

  test('should change table value', () => {
    expect(directive.value).toEqual([1, 4, 2]);
    const table = spectator.query(TableComponent);
    expect(table.value).toEqual([1, 2, 4]);
  });
});
