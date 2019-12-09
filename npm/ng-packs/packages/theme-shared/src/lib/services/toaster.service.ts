import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import { MessageService, Message } from 'primeng';

@Injectable({ providedIn: 'root' })
export class ToasterService extends AbstractToaster {
  constructor(protected messageService: MessageService) {
    super(messageService);
  }

  addAll(messages: Message[]): void {
    this.messageService.addAll(messages.map(message => ({ key: this.key, ...message })));
  }
}
