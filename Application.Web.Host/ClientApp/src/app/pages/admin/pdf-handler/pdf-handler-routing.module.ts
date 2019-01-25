import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AuthGuard } from "shared/_guards";
import { PdfHandlerComponent } from "./pdf-handler.component";

@NgModule({
  imports: [
    RouterModule.forChild([
      // {
      //   path: ":mode",
      //   loadChildren:
      //     "app/pages/admin/app-user/app-user-actions/app-user-action.module#AppUserActionModule",
      //   canActivate: [AuthGuard]
      // },
      {
        path: "",
        component: PdfHandlerComponent,
        canActivate: [AuthGuard]
      }
    ])
  ],
  exports: [RouterModule]
})
export class PdfHandlerRoutingModule {}
