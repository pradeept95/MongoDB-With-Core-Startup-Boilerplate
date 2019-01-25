import {
  Component,
  Injector,
  OnDestroy,
  Input,
  AfterViewInit,
  ViewChild,
  OnInit
} from "@angular/core";

import { MatPaginator, MatSort } from "@angular/material";
import {
  PagedListingComponentBase,
  PagedRequestDto
} from "shared/paged-listing-component-base";
import { AppUserDto } from "shared/_models/app-user-model";
import { AppUserService } from "shared/_services/appuser.service";
import { Router } from "@angular/router";
import Swal from "sweetalert2";
import { AlertService, AuthenticationService } from "shared/_services";
import { AppComponentBase } from "shared/app-component-base";
import { Subscription } from "rxjs";
import { UserInfoService } from "shared/_services/user-info.service";

@Component({
  selector: "fury-user-profile",
  templateUrl: "./user-profile.component.html",
  styleUrls: ["./user-profile.component.scss"]
})
export class UserProfileComponent extends AppComponentBase
  implements OnInit, OnDestroy {
  userInfo: AppUserDto = new AppUserDto();
  private subscription: Subscription;

  constructor(
    private injector: Injector,
    private authService: AuthenticationService,
    private userInfoService: UserInfoService,
    private _appUserService: AppUserService,
    private alertService: AlertService,
    private _router: Router
  ) {
    super(injector);
    this.updateUserInfo();
  }

  ngOnInit() {
    this.subscription = this.userInfoService.getUserInfo().subscribe(user => {
      if (user) {
        this.userInfo = user;
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  updateUserInfo = () => {
    this.authService.getCurrentUser().subscribe(res => {
      const userData: AppUserDto = Object.assign(
        {},
        new AppUserDto(),
        res.Data
      );
      this.userInfoService.saveUserInfo(userData);
    });
  };
}
