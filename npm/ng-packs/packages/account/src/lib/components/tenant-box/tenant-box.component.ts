import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ABP } from '@abp/ng.core';

@Component({
  selector: 'abp-tenant-box',
  templateUrl: './tenant-box.component.html',
})
export class TenantBoxComponent {
  constructor(private modalService: NgbModal, private fb: FormBuilder) {}

  form: FormGroup;

  selected: ABP.BasicItem;

  @ViewChild('modalContent', { static: false })
  modalContent: TemplateRef<any>;

  createForm() {
    this.form = this.fb.group({
      name: [this.selected.name],
    });
  }

  openModal() {
    this.createForm();
    this.modalService.open(this.modalContent);
  }

  onSwitch() {
    this.selected = {} as ABP.BasicItem;
    this.openModal();
  }

  save() {
    this.selected = this.form.value;
    this.modalService.dismissAll();
  }
}
