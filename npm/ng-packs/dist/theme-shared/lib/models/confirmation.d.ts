import { Toaster } from './toaster';
import { Config } from '@abp/ng.core';
export declare namespace Confirmation {
    interface Options extends Toaster.Options {
        hideCancelBtn?: boolean;
        hideYesBtn?: boolean;
        cancelText?: Config.LocalizationParam;
        yesText?: Config.LocalizationParam;
        /**
         * @deprecated to be deleted in v2
         */
        cancelCopy?: Config.LocalizationParam;
        /**
         * @deprecated to be deleted in v2
         */
        yesCopy?: Config.LocalizationParam;
    }
}
