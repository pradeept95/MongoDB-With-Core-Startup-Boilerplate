import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { QuillModule } from "ngx-quill";
import { AppUserActionComponent } from "./app-user-action.component";
import { AppUserActionRoutingModule } from "./app-user-action-routing.module";
import { MaterialModule } from "app/shared/material-components.module";
import { HighlightModule } from "app/shared/highlightjs/highlight.module";
import { FuryCardModule } from "app/shared/card/card.module";
import { BreadcrumbsModule } from "app/core/breadcrumbs/breadcrumbs.module";
import { AppUserService } from "shared/_services/appuser.service";

@NgModule({
  declarations: [AppUserActionComponent],
  imports: [
    CommonModule,
    AppUserActionRoutingModule,
    FormsModule,
    MaterialModule,
    ReactiveFormsModule,
    // Core
    HighlightModule,
    FuryCardModule,
    BreadcrumbsModule,
    // NgxSpinnerModule,
    QuillModule
  ],
  providers: [AppUserService],
  bootstrap: [AppUserActionComponent]
})
export class AppUserActionModule {}
