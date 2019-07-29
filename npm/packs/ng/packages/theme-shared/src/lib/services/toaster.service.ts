import { Injectable } from '@angular/core';
import { AbstractToasterClass } from '../abstracts/toaster';
import { Message } from 'primeng/components/common/message';

@Injectable({ providedIn: 'root' })
export class ToasterService extends AbstractToasterClass {
  addAll(messages: Message[]): void {
    this.messageService.addAll(messages.map(message => ({ key: this.key, ...message })));
  }
}
