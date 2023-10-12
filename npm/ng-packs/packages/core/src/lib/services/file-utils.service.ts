import { Injectable, inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class FileUtilsService {
  protected readonly document = inject(DOCUMENT);
  get window() {
    return this.document.defaultView;
  }
  downloadBlob(blob: Blob, fileName: string) {
    const blobUrl = this.window.URL.createObjectURL(blob);
    const a = this.document.createElement('a');
    a.style.display = 'none';
    a.href = blobUrl;
    a.download = fileName;
    this.document.body.appendChild(a);
    a.dispatchEvent(
      new MouseEvent('click', {
        bubbles: true,
        cancelable: true,
        view: this.window,
      }),
    );
    this.window.URL.revokeObjectURL(blobUrl);
    this.document.body.removeChild(a);
  }
}
