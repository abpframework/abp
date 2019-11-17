import { Injectable } from '@angular/core';
import { AbstractToaster } from '../abstracts/toaster';
import { Message } from 'primeng/components/common/message';
import { MessageService } from 'primeng/components/common/messageservice';

@Injectable({ providedIn: 'root' })
export class ToasterService extends AbstractToaster {
  constructor(protected messageService: MessageService) {
    super(messageService);
  }

  addAll(messages: Message[]): void {
    this.messageService.addAll(messages.map(message => ({ key: this.key, ...message })));
  }
}
