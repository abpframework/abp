import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NzTreeModule } from 'ng-zorro-antd/tree';
import { TreeComponent } from './components/tree.component';
import { TreeNodeTemplateDirective } from './templates/tree-node-template.directive';

const exported = [TreeNodeTemplateDirective, TreeComponent];

@NgModule({
  imports: [CommonModule, NzTreeModule, NgbDropdownModule],
  exports: [...exported],
  declarations: [...exported],
})
export class TreeModule {}
