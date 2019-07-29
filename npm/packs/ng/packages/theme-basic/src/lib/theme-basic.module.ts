import { CoreModule } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { LayoutAccountComponent } from './components/layout-account/layout-account.component';
import { LayoutApplicationComponent } from './components/layout-application/layout-application.component';
import { LayoutEmptyComponent } from './components/layout-empty/layout-empty.component';
import { LayoutComponent } from './components/layout/layout.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ToastModule } from 'primeng/toast';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxsModule } from '@ngxs/store';
import { LayoutState } from './states/layout.state';

export const LAYOUTS = [LayoutApplicationComponent, LayoutAccountComponent, LayoutEmptyComponent];

@NgModule({
  declarations: [...LAYOUTS, LayoutComponent, ChangePasswordComponent, ProfileComponent],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbCollapseModule,
    NgbDropdownModule,
    ToastModule,
    NgxValidateCoreModule,
    NgxsModule.forFeature([LayoutState]),
  ],
  exports: [...LAYOUTS],
  entryComponents: [...LAYOUTS],
})
export class ThemeBasicModule {}
