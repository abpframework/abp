import { Component, contentChild, input, TemplateRef } from '@angular/core';

@Component({
    selector: 'abp-extensible-table-row-detail',
    template: '',
})
export class ExtensibleTableRowDetailComponent<R = any> {
    readonly rowHeight = input<string | number>('100%');
    readonly template = contentChild(TemplateRef<{ row: R; expanded: boolean }>);
}
