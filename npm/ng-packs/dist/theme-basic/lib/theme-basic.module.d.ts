import { AccountLayoutComponent } from './components/account-layout/account-layout.component';
import { ApplicationLayoutComponent } from './components/application-layout/application-layout.component';
import { InitialService } from './services/initial.service';
export declare const LAYOUTS: (typeof ApplicationLayoutComponent | typeof AccountLayoutComponent)[];
export declare class ThemeBasicModule {
    private initialService;
    constructor(initialService: InitialService);
}
