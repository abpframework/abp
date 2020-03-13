import { ABP } from "@abp/ng.core";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ApplicationLayoutComponent } from "@abp/ng.theme.basic";

const routes: Routes = [
  {
    path: "",
    loadChildren: () => import("./home/home.module").then(m => m.HomeModule),
    data: {
      routes: {
        name: "::Menu:Home"
      } as ABP.Route
    }
  },
  {
    path: "account",
    loadChildren: () =>
      import("./lazy-libs/account-wrapper.module").then(
        m => m.AccountWrapperModule
      )
  },
  {
    path: "identity",
    loadChildren: () =>
      import("./lazy-libs/identity-wrapper.module").then(
        m => m.IdentityWrapperModule
      )
  },
  {
    path: "tenant-management",
    loadChildren: () =>
      import("./lazy-libs/tenant-management-wrapper.module").then(
        m => m.TenantManagementWrapperModule
      )
  },
  {
    path: "setting-management",
    loadChildren: () =>
      import("./lazy-libs/setting-management-wrapper.module").then(
        m => m.SettingManagementWrapperModule
      )
  },
  {
    path: "books",
    component: ApplicationLayoutComponent,
    loadChildren: () => import("./books/books.module").then(m => m.BooksModule),
    data: {
      routes: {
        name: "::Menu:Books",
        iconClass: "fas fa-book"
      } as ABP.Route
    }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
