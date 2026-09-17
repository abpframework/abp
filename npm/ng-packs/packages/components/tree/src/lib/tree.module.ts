import { NgModule } from '@angular/core';
import { TreeComponent } from './components/tree.component';
import { ExpandedIconTemplateDirective } from './templates/expanded-icon-template.directive';
import { TreeNodeTemplateDirective } from './templates/tree-node-template.directive';

@NgModule({
  imports: [TreeComponent, TreeNodeTemplateDirective, ExpandedIconTemplateDirective],
  exports: [TreeComponent, TreeNodeTemplateDirective, ExpandedIconTemplateDirective],
  declarations: [],
})
export class TreeModule {}
