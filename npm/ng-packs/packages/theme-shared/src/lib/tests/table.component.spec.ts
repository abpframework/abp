import { Pipe, PipeTransform } from '@angular/core';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { TableComponent } from '../components';

@Pipe({
  name: 'abpLocalization',
})
export class DummyLocalizationPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    return value;
  }
}

describe('TableComponent', () => {
  let spectator: SpectatorHost<TableComponent>;
  const createHost = createHostFactory({
    component: TableComponent,
    declarations: [DummyLocalizationPipe],
    imports: [NgbPaginationModule],
  });

  describe('without value', () => {
    beforeEach(() => {
      spectator = createHost(
        `<abp-table
          [headerTemplate]="header"
          [colgroupTemplate]="colgroup"
          [value]="value">
        </abp-table>
        <ng-template #colgroup><colgroup><col /></colgroup></ng-template>
        <ng-template #header><th>name</th></ng-template>`,
        {
          hostProps: {
            value: [],
          },
        },
      );
    });

    it('should display the empty message', () => {
      expect(spectator.query('caption.ui-table-empty')).toHaveText(
        'AbpAccount::NoDataAvailableInDatatable',
      );
    });

    it('should display the header', () => {
      expect(spectator.query('thead')).toBeTruthy();
      expect(spectator.query('th')).toHaveText('name');
    });

    it('should place the colgroup template', () => {
      expect(spectator.query('colgroup')).toBeTruthy();
      expect(spectator.query('col')).toBeTruthy();
    });
  });

  describe('with value', () => {
    // TODO
    beforeEach(() => {
      spectator = createHost(
        `<abp-table
        [headerTemplate]="header"
        [value]="value"></abp-table>
        <ng-template #header><th>name</th></ng-template>
        `,
        {
          hostProps: {
            value: [],
          },
        },
      );
    });
  });
});
