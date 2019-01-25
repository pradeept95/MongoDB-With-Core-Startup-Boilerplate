import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AppUserService } from "shared/_services/appuser.service";
import { MaterialModule } from "app/shared/material-components.module";
import { BreadcrumbsModule } from "app/core/breadcrumbs/breadcrumbs.module";
import { UserInfoService } from "shared/_services/user-info.service";
import { SettingsComponent } from "./settings.component";
import { SettingsRoutingModule } from "./settings-routing.module";
import { SettingService } from "shared/_services/setting.service";
import { HighlightModule } from "app/shared/highlightjs/highlight.module";
import { FuryCardModule } from "app/shared/card/card.module";
import { QuillModule } from "ngx-quill";

@NgModule({
  declarations: [SettingsComponent],
  imports: [
    CommonModule,
    SettingsRoutingModule,
    CommonModule,
    FormsModule,
    MaterialModule,
    ReactiveFormsModule,
    // Core
    HighlightModule,
    FuryCardModule,
    BreadcrumbsModule,
    QuillModule
  ],
  providers: [UserInfoService, AppUserService, SettingService],
  bootstrap: [SettingsComponent]
})
export class SettingsModule {}
