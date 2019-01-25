import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { AppUserService } from "shared/_services/appuser.service";
import { AppUserRoutingModule } from "./app-user-routing.module";
import { AppUserComponent } from "./app-user.component";
import { MaterialModule } from "app/shared/material-components.module";
import { ListModule } from "app/shared/list/list.module";
import { BreadcrumbsModule } from "app/core/breadcrumbs/breadcrumbs.module";

@NgModule({
  declarations: [AppUserComponent],
  imports: [
    CommonModule,
    AppUserRoutingModule,
    FormsModule,
    MaterialModule,
    // Core
    ListModule,
    BreadcrumbsModule
  ],
  providers: [AppUserService],
  bootstrap: [AppUserComponent]
})
export class AppUserModule {}
