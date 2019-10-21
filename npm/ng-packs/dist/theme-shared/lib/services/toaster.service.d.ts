import { AbstractToaster } from '../abstracts/toaster';
import { Message } from 'primeng/components/common/message';
export declare class ToasterService extends AbstractToaster {
    addAll(messages: Message[]): void;
}
