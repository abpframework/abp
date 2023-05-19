import { inject, Injectable, Pipe, PipeTransform, SecurityContext } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Injectable()
@Pipe({ name: 'abpSafeHtml' })
export class SafeHtmlPipe implements PipeTransform {
  private readonly sanitizer = inject(DomSanitizer);

  transform(value: string): string {
    if (typeof value !== 'string') return '';
    return this.sanitizer.sanitize(SecurityContext.HTML, value);
  }
}
