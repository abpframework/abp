# Custom layout usage with Lepton X components


First, The custom layout component should be created and implemented for the Angular application.
Related content can be found in the [Component Replacement Document](../../framework/ui/angular/component-replacement.md#how-to-replace-a-layout)


 
After creating a custom layout, these imports should be imported in the `app.module.ts` file because the modules contain definitions of the Lepton X components.


```javascript
// app.module.ts
import { LpxSideMenuLayoutModule } from '@volosoft/ngx-lepton-x/layouts';
import { LpxResponsiveModule } from '@volo/ngx-lepton-x.core';// optional. Only, if you are using lpxResponsive directive

 @NgModule({
 //... removed for clearity
  imports: [
  	//... removed for clearity
  	LpxSideMenuLayoutModule,
  	LpxResponsiveModule // <-- Optional
  	]
})
export class AppModule {}

```

Here is the simplified version of the `side-menu-layout.ts` file. Only the ABP Component Replacement code has been removed.


```html
<ng-container *lpxResponsive="'all md-none'">
    <ng-container *ngTemplateOutlet="content"></ng-container>
  </ng-container>
  <ng-container *lpxResponsive="'md'">
    <div class="lpx-scroll-container ps" [perfectScrollbar]>
      <ng-container *ngTemplateOutlet="content"></ng-container>
    </div>
  </ng-container>
  <ng-template #content>
    <div
      id="lpx-wrapper">
      <div class="lpx-sidebar-container" *lpxResponsive="'md'">
        <div class="lpx-sidebar ps" [perfectScrollbar]>
            <lpx-navbar></lpx-navbar>
        </div>
      </div>
  
      <div class="lpx-content-container">
        <div class="lpx-topbar-container">
          <div class="lpx-topbar">
            <div class="lpx-breadcrumb-container">
                <lpx-breadcrumb></lpx-breadcrumb>
            </div>
            <div class="lpx-topbar-content">
              <lpx-topbar-content></lpx-topbar-content>
            </div>
          </div>
        </div>
        <div class="lpx-content-wrapper">
          <div class="lpx-content">
            <router-outlet></router-outlet>
          </div>
        </div>
        <div class="lpx-footbar-container">
            <lpx-footer></lpx-footer>
        </div>
      </div>
  
      <lpx-mobile-navbar *lpxResponsive="'all md-none'"></lpx-mobile-navbar>

      <div class="lpx-toolbar-container" *lpxResponsive="'md'">
        <lpx-toolbar-container></lpx-toolbar-container>
        <lpx-avatar></lpx-avatar>
        <lpx-settings></lpx-settings>
      </div>
    </div>
  </ng-template>

```

Add this code to your application template and customize it as desired.
