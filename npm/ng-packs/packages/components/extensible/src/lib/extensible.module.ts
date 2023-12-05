import { NgModule } from '@angular/core';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import {
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbTimepickerModule,
  NgbTooltipModule,
  NgbTypeaheadModule,
} from '@ng-bootstrap/ng-bootstrap';
import {
  ExtensibleFormComponent,
  ExtensibleFormPropComponent,
  ExtensibleTableComponent,
  GridActionsComponent,
  PageToolbarComponent,
  ExtensibleDateTimePickerComponent,
} from './components';
import { PropDataDirective } from './directives/prop-data.directive';
import { CreateInjectorPipe } from './pipes/create-injector.pipe';
import { DisabledDirective } from '@abp/ng.theme.shared';

const importWithExport = [
  DisabledDirective,
  ExtensibleDateTimePickerComponent,
  ExtensibleFormPropComponent,
  GridActionsComponent,
  PropDataDirective,
  PageToolbarComponent,
  CreateInjectorPipe,
  ExtensibleFormComponent,
  ExtensibleTableComponent,
];
@NgModule({
  declarations: [],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgxValidateCoreModule,
    NgbDatepickerModule,
    NgbDropdownModule,
    NgbTimepickerModule,
    NgbTypeaheadModule,
    NgbTooltipModule,
    ...importWithExport,
  ],
  exports: [...importWithExport],
})
export class ExtensibleModule {}
