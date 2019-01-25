import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { DashboardRoutingModule } from "./dashboard-routing.module";
import { DashboardComponent } from "./dashboard.component";
import { MaterialModule } from "app/shared/material-components.module";
import { AspectRatioModule } from "app/shared/aspect-ratio/aspect-ratio.module";

@NgModule({
  imports: [
    CommonModule,
    DashboardRoutingModule,
    MaterialModule,

    // Core
    AspectRatioModule
  ],
  declarations: [DashboardComponent],
  providers: []
})
export class DashboardModule {}
