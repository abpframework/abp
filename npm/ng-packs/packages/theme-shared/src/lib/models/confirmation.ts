import { Toaster } from './toaster';

export namespace Confirmation {
  export interface Options extends Toaster.Options {
    hideCancelBtn?: boolean;
    hideYesBtn?: boolean;
    cancelCopy?: string;
    yesCopy?: string;
  }
}
