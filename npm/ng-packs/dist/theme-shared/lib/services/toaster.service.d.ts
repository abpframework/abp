import { AbstractToaster } from '../abstracts/toaster';
import { Message } from 'primeng/components/common/message';
import { MessageService } from 'primeng/components/common/messageservice';
export declare class ToasterService extends AbstractToaster {
    protected messageService: MessageService;
    constructor(messageService: MessageService);
    addAll(messages: Message[]): void;
}
