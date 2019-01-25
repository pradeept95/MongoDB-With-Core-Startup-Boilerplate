import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AuthGuard } from "shared/_guards";
import { SettingsComponent } from "./settings.component";

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: "",
        component: SettingsComponent,
        canActivate: [AuthGuard]
      }
    ])
  ],
  exports: [RouterModule]
})
export class SettingsRoutingModule {}
