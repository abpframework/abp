import { Toaster } from './toaster';
export declare namespace Confirmation {
    interface Options extends Toaster.Options {
        hideCancelBtn?: boolean;
        hideYesBtn?: boolean;
        cancelCopy?: string;
        yesCopy?: string;
    }
}
