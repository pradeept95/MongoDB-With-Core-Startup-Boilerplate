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
import { AppUserDto } from "shared/_models/app-user-model";
import { ListColumn } from "app/shared/list/list-column.model";
import { AppUserService } from "shared/_services/appuser.service";
import { Router } from "@angular/router";
import Swal from "sweetalert2";
import { AlertService } from "shared/_services";

@Component({
  selector: "fury-app-user",
  templateUrl: "./app-user.component.html",
  styleUrls: ["./app-user.component.scss"]
})
export class AppUserComponent extends PagedListingComponentBase<AppUserDto>
  implements AfterViewInit, OnDestroy {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  appUsers: AppUserDto[];

  @Input()
  columns: ListColumn[] = [
    {
      name: "Username",
      property: "UserName",
      visible: true,
      isModelProperty: true
    },
    {
      name: "First Name",
      property: "FirstName",
      visible: true,
      isModelProperty: true
    },
    {
      name: "Middle Name",
      property: "MiddleName",
      visible: true,
      isModelProperty: true
    },
    {
      name: "Last Name",
      property: "LastName",
      visible: true,
      isModelProperty: true
    },
    {
      name: "Email",
      property: "Email",
      visible: false,
      isModelProperty: true
    },
    { name: "Actions", property: "actions", visible: true }
  ] as ListColumn[];

  constructor(
    private injector: Injector,
    private _appUserService: AppUserService,
    private alertService: AlertService,
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
    this._appUserService
      .getAllPaged(this.searchText, request.skipCount, request.maxResultCount)
      .subscribe(result => {
        this.appUsers = result.Data;
        this.dataSource.data = this.appUsers;
        if (this.totalItems == 0 || this.totalItems != result.TotalCount)
          this.totalItems = result.TotalCount;
        finishedCallback();
      });
  }

  create = () => {
    this._router.navigate(["/app/user-management", "Create"], {});
  };

  update = (entity: AppUserDto) => {
    this._router.navigate(["/app/user-management", "Edit"], {
      queryParams: { q: entity.Id }
    });
  };

  changePassword = (entity: AppUserDto) => {
    this._router.navigate(["/app/user-management", "Change-Password"], {
      queryParams: { q: entity.Id }
    });
  };

  protected delete(entity: AppUserDto): void {
    Swal({
      title: "Are you sure?",
      text: `Delete User ${entity.FirstName}?`,
      type: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes",
      cancelButtonText: "No"
    }).then(
      result => {
        if (result.value) {
          this._appUserService.delete(entity.Id).subscribe(() => {
            this.alertService.success(
              `User ${entity.FirstName} ${entity.MiddleName} ${
                entity.lastName
              } is successfully deleted`
            );
            this.refresh();
          });
        }
      },
      err => {
        this.alertService.error(err);
      }
    );
  }

  ngOnDestroy() {}
}
