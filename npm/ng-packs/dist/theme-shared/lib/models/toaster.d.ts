export declare namespace Toaster {
    interface Options {
        id?: any;
        closable?: boolean;
        life?: number;
        sticky?: boolean;
        data?: any;
        messageLocalizationParams?: string[];
        titleLocalizationParams?: string[];
    }
    type Severity = 'success' | 'info' | 'warn' | 'error';
    const enum Status {
        confirm = "confirm",
        reject = "reject",
        dismiss = "dismiss"
    }
}
