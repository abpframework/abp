import { CommonModule } from '@angular/common';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { ChartComponent } from './chart.component';

declare const Chart: any

@NgModule({
    imports: [CommonModule],
    exports: [ChartComponent],
    declarations: [ChartComponent],
    providers: [],
})
export class ChartModule {
    static forRoot(): ModuleWithProviders<ChartModule> {
        return {
            ngModule: ChartModule,
            providers: [{
                provide: APP_INITIALIZER,
                multi: true,
                useFactory: (injector: Injector) => () => {
                    import('chart.js/auto').then((module) => {
                        console.dir(module)
                    })
                    return Promise.resolve()
                },
                deps: [Injector]
            }]
        }
    }
}
