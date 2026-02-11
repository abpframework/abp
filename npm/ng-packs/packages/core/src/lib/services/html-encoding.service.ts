import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HtmlEncodingService {
  encode(value: string): string {
    if (!value) {
      return value;
    }

    return value
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;');
  }

  decode(value: string): string {
    if (!value) {
      return value;
    }

    return value
      .replace(/&amp;/g, '&')
      .replace(/&lt;/g, '<')
      .replace(/&gt;/g, '>')
      .replace(/&quot;/g, '"')
      .replace(/&#39;/g, "'");
  }
}
