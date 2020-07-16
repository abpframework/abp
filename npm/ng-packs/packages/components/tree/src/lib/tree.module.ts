import { NgModule } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NzTreeModule } from 'ng-zorro-antd/tree';
import { TreeComponent } from './components/tree.component';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [CommonModule, NzTreeModule, NgbDropdownModule],
  exports: [TreeComponent],
  declarations: [TreeComponent],
})
export class TreeModule {}
