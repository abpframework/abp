import { TemplateRef } from '@angular/core';
export declare namespace Layout {
    interface State {
        navigationElements: NavigationElement[];
    }
    interface NavigationElement {
        name: string;
        element: TemplateRef<any>;
        order?: number;
    }
}
