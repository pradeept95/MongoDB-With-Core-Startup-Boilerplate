import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { AppUserService } from "shared/_services/appuser.service";
import { MaterialModule } from "app/shared/material-components.module";
import { BreadcrumbsModule } from "app/core/breadcrumbs/breadcrumbs.module";
import { UserInfoService } from "shared/_services/user-info.service";
import { UserProfileComponent } from "./user-profile.component";
import { UserProfileRoutingModule } from "./user-profile-routing.module";

@NgModule({
  declarations: [UserProfileComponent],
  imports: [
    CommonModule,
    UserProfileRoutingModule,
    FormsModule,
    MaterialModule,
    // Core
    BreadcrumbsModule
  ],
  providers: [UserInfoService, AppUserService],
  bootstrap: [UserProfileComponent]
})
export class UserProfileModule {}
