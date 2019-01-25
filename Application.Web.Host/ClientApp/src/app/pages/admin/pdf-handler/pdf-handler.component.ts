import {
  Component,
  Injector,
  OnDestroy,
  Input,
  AfterViewInit,
  ViewChild
} from "@angular/core";

import { MatPaginator, MatSort } from "@angular/material";
import {
  PagedListingComponentBase,
  PagedRequestDto
} from "shared/paged-listing-component-base";
import { ListColumn } from "app/shared/list/list-column.model";
import { Router } from "@angular/router";
import Swal from "sweetalert2";
import { PdfHandlerService } from "shared/_services/pdf-handler.service";
import { FileInfoDto } from "shared/_models/file-info-model";

@Component({
  selector: "fury-pdf-handler",
  templateUrl: "./pdf-handler.component.html",
  styleUrls: ["./pdf-handler.component.scss"]
})
export class PdfHandlerComponent extends PagedListingComponentBase<FileInfoDto>
  implements AfterViewInit, OnDestroy {
  menuWidth = "30%";
  contentWidth = `calc(100% - ${this.menuWidth})`;
  currentPdfSource: string = `/pdf/loading.pdf`;
  currentSelectedFileName = "";

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  pdfData: FileInfoDto[];

  @Input()
  columns: ListColumn[] = [
    {
      name: "Files",
      property: "FileName",
      visible: true,
      isModelProperty: true
    },
    { name: "Actions", property: "actions", visible: false }
  ] as ListColumn[];

  constructor(
    private injector: Injector,
    private _pdfHandlerService: PdfHandlerService,
    private _router: Router
  ) {
    super(injector);
  }

  get visibleColumns() {
    return this.columns
      .filter(column => column.visible)
      .map(column => column.property);
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  protected list(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    this._pdfHandlerService
      .getAllPaged(this.searchText, request.skipCount, request.maxResultCount)
      .subscribe(result => {
        this.pdfData = result.Data;
        if (this.pdfData.length) {
          this.currentPdfSource = this.pdfData[0].FilePath;
          this.currentSelectedFileName = this.pdfData[0].FileName;
        }
        this.dataSource.data = this.pdfData;
        if (this.totalItems == 0 || this.totalItems != result.TotalCount)
          this.totalItems = result.TotalCount;
        finishedCallback();
      });
  }

  selectCurrentFile = (data: FileInfoDto) => {
    this.currentPdfSource = `${data.FilePath}`;
    this.currentSelectedFileName = data.FileName;
  };

  create = () => {
    // this._router.navigate(["/app/user-management", "Create"], {});
  };

  update = (entity: FileInfoDto) => {
    // this._router.navigate(["/app/user-management", "Edit"], {
    //   queryParams: { q: entity.Id }
    // });
  };

  protected delete(entity: FileInfoDto): void {
    // Swal({
    //   title: "Are you sure?",
    //   text: `Delete User ${entity.FirstName}?`,
    //   type: "warning",
    //   showCancelButton: true,
    //   confirmButtonText: "Yes",
    //   cancelButtonText: "No"
    // }).then(result => {
    //   if (result.value) {
    //     this._appUserService.delete(entity.Id).subscribe(() => {
    //       this.refresh();
    //     });
    //   }
    // });
  }

  ngOnDestroy() {}
}
