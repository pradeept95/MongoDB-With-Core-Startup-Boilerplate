import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AppUserActionComponent } from "./app-user-action.component";
import { AuthGuard } from "shared/_guards";

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: "",
        component: AppUserActionComponent,
        canActivate: [AuthGuard]
      }
    ])
  ],
  exports: [RouterModule]
})
export class AppUserActionRoutingModule {}
