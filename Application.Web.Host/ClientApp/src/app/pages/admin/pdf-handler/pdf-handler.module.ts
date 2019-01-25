import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { PdfHandlerRoutingModule } from "./pdf-handler-routing.module";
import { PdfHandlerComponent } from "./pdf-handler.component";
import { MaterialModule } from "app/shared/material-components.module";
import { ListModule } from "app/shared/list/list.module";
import { BreadcrumbsModule } from "app/core/breadcrumbs/breadcrumbs.module";
import { PdfHandlerService } from "shared/_services/pdf-handler.service";
import { ScrollbarModule } from "app/shared/scrollbar/scrollbar.module";
import { SharedPipesModule } from "shared/_pipe/safe.pipe";
import { NgxExtendedPdfViewerModule } from "ngx-extended-pdf-viewer";

@NgModule({
  declarations: [PdfHandlerComponent],
  imports: [
    CommonModule,
    PdfHandlerRoutingModule,
    FormsModule,
    MaterialModule,
    // Core
    ListModule,
    BreadcrumbsModule,
    ScrollbarModule,
    SharedPipesModule,
    NgxExtendedPdfViewerModule
  ],
  providers: [PdfHandlerService],
  bootstrap: [PdfHandlerComponent]
})
export class PdfHandlerrModule {}
