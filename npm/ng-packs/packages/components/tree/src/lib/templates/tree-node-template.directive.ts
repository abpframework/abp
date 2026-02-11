import { Directive, TemplateRef, inject } from '@angular/core';

@Directive({
  selector: '[abpTreeNodeTemplate],[abp-tree-node-template]',
})
export class TreeNodeTemplateDirective {  template = inject<TemplateRef<any>>(TemplateRef);

}
