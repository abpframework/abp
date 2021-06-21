import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: '[abpTreeExpandedIconTemplate],[abp-tree-expanded-icon-template]',
})
export class ExpandedIconTemplateDirective {
  constructor(public template: TemplateRef<any>) {}
}
