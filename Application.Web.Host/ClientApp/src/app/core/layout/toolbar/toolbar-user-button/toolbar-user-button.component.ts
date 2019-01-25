import { Component, OnInit, OnDestroy } from "@angular/core";
import { AuthenticationService } from "shared/_services";
import { ActivatedRoute, Router } from "@angular/router";
import { MatDialog } from "@angular/material";
import Swal from "sweetalert2";
import { UserInfoService } from "shared/_services/user-info.service";
import { AppUserDto } from "shared/_models";
import { Subscription } from "rxjs";

@Component({
  selector: "fury-toolbar-user-button",
  templateUrl: "./toolbar-user-button.component.html",
  styleUrls: ["./toolbar-user-button.component.scss"]
})
export class ToolbarUserButtonComponent implements OnInit, OnDestroy {
  isOpen: boolean;
  userInfo: AppUserDto = new AppUserDto();
  result: AppUserDto = new AppUserDto();
  private subscription: Subscription;

  constructor(
    private userInfoService: UserInfoService,
    private authService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.updateUserInfo();
    this.subscription = this.userInfoService.getUserInfo().subscribe(user => {
      if (user) {
        this.userInfo = user;
      }
    });
  }

  updateUserInfo = () => {
    this.authService.getCurrentUser().subscribe(res => {
      const userData: AppUserDto = Object.assign({}, this.result, res.Data);
      this.userInfoService.saveUserInfo(userData);
    });
  };

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  toggleDropdown() {
    this.isOpen = !this.isOpen;
  }

  onClickOutside() {
    this.isOpen = false;
  }

  logout = () => {
    Swal({
      title: "Are you sure?",
      text: "Are you sure to logout.",
      type: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes",
      cancelButtonText: "No"
    }).then(result => {
      if (result.value) {
        this.authService.logout();
        location.reload(true);
      } else if (result.dismiss === Swal.DismissReason.cancel) {
      }
    });
  };
}
