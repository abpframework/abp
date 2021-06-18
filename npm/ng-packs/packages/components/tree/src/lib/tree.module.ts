import { CoreModule } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NzNoAnimationModule } from 'ng-zorro-antd/core/no-animation';
import { NzTreeModule } from 'ng-zorro-antd/tree';
import { TreeComponent } from './components/tree.component';
import { ExpandedIconTemplateDirective } from './templates/expanded-icon-template.directive';
import { TreeNodeTemplateDirective } from './templates/tree-node-template.directive';

const templates = [TreeNodeTemplateDirective, ExpandedIconTemplateDirective];

const exported = [...templates, TreeComponent];

@NgModule({
  imports: [CoreModule, NzTreeModule, NgbDropdownModule, NzNoAnimationModule],
  exports: [...exported],
  declarations: [...exported],
})
export class TreeModule {}
