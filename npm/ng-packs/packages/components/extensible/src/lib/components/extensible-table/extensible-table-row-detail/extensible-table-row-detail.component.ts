import {Component, contentChild, input, TemplateRef, ChangeDetectionStrategy,} from '@angular/core';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'abp-extensible-table-row-detail',
    template: '',
})
export class ExtensibleTableRowDetailComponent<R = any> {
    readonly rowHeight = input<string | number>('100%');
    readonly template = contentChild(TemplateRef<{ row: R; expanded: boolean }>);
}
