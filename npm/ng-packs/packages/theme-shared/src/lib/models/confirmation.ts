import { Toaster } from './toaster';

export namespace Confirmation {
  export interface Options extends Toaster.Options {
    hideCancelBtn?: boolean;
    hideYesBtn?: boolean;
    cancelText?: string;
    yesText?: string;
    /**
     * @deprecated to be deleted in v2
     */
    cancelCopy?: string;
    /**
     * @deprecated to be deleted in v2
     */
    yesCopy?: string;
  }
}
