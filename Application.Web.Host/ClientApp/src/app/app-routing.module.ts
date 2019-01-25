import { NgModule } from "@angular/core";
import { PreloadAllModules, RouterModule, Routes } from "@angular/router";
import { LayoutComponent } from "./core/layout/layout.component";
import { AuthGuard } from "shared/_guards";

const routes: Routes = [
  {
    path: "login",
    loadChildren: "app/pages/account/login/login.module#LoginModule"
  },
  // {
  //   path: "register",
  //   loadChildren:
  //     "app/pages/custom-pages/register/register.module#RegisterModule"
  // },
  // {
  //   path: "forgot-password",
  //   loadChildren:
  //     "app/pages/custom-pages/forgot-password/forgot-password.module#ForgotPasswordModule"
  // },
  {
    path: "",
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: "",
        loadChildren:
          "app/pages/admin/dashboard/dashboard.module#DashboardModule",
        pathMatch: "full"
      },
      {
        path: "app/user-management",
        loadChildren: "app/pages/admin/app-user/app-user.module#AppUserModule"
      },
      {
        path: "app/file-management",
        loadChildren:
          "app/pages/admin/pdf-handler/pdf-handler.module#PdfHandlerrModule"
      },

      {
        path: "app/profile",
        loadChildren:
          "app/pages/admin/user-profile/user-profile.module#UserProfileModule"
      },
      {
        path: "app/setting",
        loadChildren: "app/pages/admin/settings/settings.module#SettingsModule"
      },
      // otherwise redirect to home
      { path: "**", redirectTo: "" }
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      preloadingStrategy: PreloadAllModules
    })
  ],
  exports: [RouterModule],
  providers: []
})
export class AppRoutingModule {}
