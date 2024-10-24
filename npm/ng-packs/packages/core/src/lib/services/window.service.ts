import { Injectable, inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class AbpWindowService {
  public readonly document = inject(DOCUMENT);
  public readonly window = this.document.defaultView;
  public readonly navigator = this.window.navigator;

  copyToClipboard(text: string): Promise<void> {
    return this.navigator.clipboard.writeText(text);
  }

  open(url?: string | URL, target?: string, features?: string): Window {
    return this.window.open(url, target, features);
  }

  reloadPage(): void {
    this.window.location.reload();
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
