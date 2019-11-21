import { Renderer2, ElementRef } from '@angular/core';
import { Config } from '@abp/ng.core';
export declare class ErrorComponent {
    title: string | Config.LocalizationWithDefault;
    details: string | Config.LocalizationWithDefault;
    renderer: Renderer2;
    elementRef: ElementRef;
    host: any;
    destroy(): void;
}
