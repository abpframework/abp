import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import {
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbTimepickerModule,
  NgbTypeaheadModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { DateTimePickerComponent } from './components/date-time-picker/date-time-picker.component';
import { ExtensibleFormPropComponent } from './components/extensible-form/extensible-form-prop.component';
import { ExtensibleFormComponent } from './components/extensible-form/extensible-form.component';
import { ExtensibleTableComponent } from './components/extensible-table/extensible-table.component';
import { GridActionsComponent } from './components/grid-actions/grid-actions.component';
import { PageToolbarComponent } from './components/page-toolbar/page-toolbar.component';
import { DisabledDirective } from './directives/disabled.directive';
import { PropDataDirective } from './directives/prop-data.directive';
import { CreateInjectorPipe } from './pipes/create-injector.pipe';

@NgModule({
  exports: [
    DateTimePickerComponent,
    PageToolbarComponent,
    GridActionsComponent,
    ExtensibleFormComponent,
    ExtensibleTableComponent,
    PropDataDirective,
    DisabledDirective,
    CreateInjectorPipe,
  ],
  declarations: [
    DateTimePickerComponent,
    PageToolbarComponent,
    GridActionsComponent,
    ExtensibleFormPropComponent,
    ExtensibleFormComponent,
    ExtensibleTableComponent,
    PropDataDirective,
    DisabledDirective,
    CreateInjectorPipe,
  ],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgxValidateCoreModule,
    NgbDatepickerModule,
    NgbDropdownModule,
    NgbTimepickerModule,
    NgbTypeaheadModule,
  ],
})
export class BaseUiExtensionsModule {}

@NgModule({
  exports: [BaseUiExtensionsModule],
  imports: [BaseUiExtensionsModule],
})
export class UiExtensionsModule {}
