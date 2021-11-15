import { Injectable } from '@angular/core';

export type ModalDismissMode = 'hard' | 'soft';

export interface DismissableModal {
  dismiss(mode: ModalDismissMode): void;
}

@Injectable({ providedIn: 'root' })
export class ModalRefService {
  modalRefs: DismissableModal[] = [];

  register(modal: DismissableModal) {
    this.modalRefs.push(modal);
  }
  unregister(modal: DismissableModal) {
    const index = this.modalRefs.indexOf(modal);
    if (index > -1) {
      this.modalRefs.splice(index, 1);
    }
  }

  dismissAll(mode: ModalDismissMode) {
    this.modalRefs.forEach(modal => modal.dismiss(mode));
  }
}
