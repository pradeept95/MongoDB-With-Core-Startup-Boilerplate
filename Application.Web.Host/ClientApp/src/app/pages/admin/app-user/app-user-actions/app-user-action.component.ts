import { Component, OnInit, Injector } from "@angular/core";

import { Router, ActivatedRoute } from "@angular/router";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
// import { NgxSpinnerService } from "ngx-spinner";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "shared/app-component-base";
import { AppUserDto } from "shared/_models/app-user-model";
import { AppUserService } from "shared/_services/appuser.service";
import { AlertService, AuthenticationService } from "shared/_services";
import { UserInfoService } from "shared/_services/user-info.service";

@Component({
  selector: "fury-app-user-actions",
  templateUrl: "./app-user-action.component.html",
  styleUrls: ["./app-user-action.component.scss"]
})
export class AppUserActionComponent extends AppComponentBase implements OnInit {
  mode: string; //"Create" | "Edit" | "Detail";
  userModel: AppUserDto = new AppUserDto();
  saveModel: AppUserDto = new AppUserDto();

  entityId: string = "";
  form: FormGroup;

  values: any[];

  constructor(
    private injector: Injector,
    private _router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private alertService: AlertService,
    private _appUserService: AppUserService, // private spinner: NgxSpinnerService
    private authService: AuthenticationService,
    private userInfoService: UserInfoService
  ) {
    super(injector);
    this.route.queryParams.subscribe(params => {
      if (params["q"]) this.entityId = decodeURIComponent(params["q"]);
    });

    this.route.paramMap.subscribe(params => {
      this.mode = params.get("mode").toLowerCase();
    });
  }

  ngOnInit() {
    if (
      this.mode !== "create" &&
      this.mode !== "edit" &&
      this.mode !== "detail" &&
      this.mode !== "change-password"
    ) {
      this.goBack();
      return;
    }

    this.prepareForm();
  }

  prepareForm = () => {
    this.form = this.fb.group({
      Id: [""],
      FirstName: ["", Validators.required],
      MiddleName: [""],
      LastName: ["", Validators.required],
      UserName: ["", Validators.required],
      Email: [""],
      Password: ["", Validators.required],
      ConfirmPassword: ["", Validators.required],
      IsActive: [true]
    });
    if (this.mode == "edit" || this.mode == "change-password") {
      this.getValueForEdit();
    }
  };

  getValueForEdit = () => {
    if (
      this.entityId == "" ||
      this.entityId == null ||
      this.entityId == undefined
    ) {
      this.entityId = "0";
    }
    this.isLoading = true;
    this._appUserService
      .get(this.entityId)
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe(result => {
        this.userModel = Object.assign({}, this.saveModel, result.Data);
        if (this.mode == "edit") {
          this.form.patchValue({
            Id: this.userModel.Id,
            FirstName: this.userModel.FirstName,
            MiddleName: this.userModel.MiddleName,
            LastName: this.userModel.LastName,
            UserName: this.userModel.UserName,
            Email: this.userModel.Email,
            Password: this.userModel.Password,
            ConfirmPassword: this.userModel.Password,
            isActive: this.userModel.IsActive
          });
        } else if (this.mode == "change-password") {
          this.form.patchValue({
            Id: this.userModel.Id,
            FirstName: this.userModel.FirstName,
            MiddleName: this.userModel.MiddleName,
            LastName: this.userModel.LastName,
            UserName: this.userModel.UserName,
            Email: this.userModel.Email,
            Password: "",
            ConfirmPassword: "",
            isActive: this.userModel.IsActive
          });
        }
      });
  };

  reset = () => {
    if (this.mode == "change-password") {
      this.form.patchValue({
        Password: "",
        ConfirmPassword: ""
      });
    } else {
      this.form.patchValue({
        FirstName: "",
        MiddleName: "",
        LastName: "",
        UserName: "",
        Email: "",
        Password: "",
        ConfirmPassword: "",
        isActive: this.userModel.IsActive
      });
    }
  };

  submit() {
    if (this.form.invalid) {
      this.alertService.error("Invalid Request");
      this.snackbar.open("Invalid Request.", null, {
        duration: 2000
      });
      return;
    }

    if (this.mode == "create") {
      this.create();
    } else if (this.mode == "edit") {
      this.update();
    } else if (this.mode == "change-password") {
      this.changePassword();
    } else {
      this.alertService.error("Your Request is not valid");
    }
  }

  goBack = () => {
    this._router.navigate(["/app/user-management"]);
  };

  create = () => {
    const templateDto: AppUserDto = Object.assign(
      {},
      this.saveModel,
      this.form.value
    );
    this.isLoading = true;
    this._appUserService
      .create(templateDto)
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe(
        res => {
          if (res) {
            this.alertService.success("User successfully created");
            this.goBack();
          }
        },
        err => {
          this.alertService.error(err);
        }
      );
  };

  update = () => {
    const templateDto: AppUserDto = Object.assign(
      {},
      this.saveModel,
      this.form.value
    );

    this.isLoading = true;
    this._appUserService
      .update(templateDto)
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.updateUserInfo();
        })
      )
      .subscribe(
        res => {
          if (res) {
            this.alertService.success("User successfully updated");
            this.goBack();
          }
        },
        err => {
          this.alertService.error(err);
        }
      );
  };

  changePassword = () => {
    const templateDto: AppUserDto = Object.assign(
      {},
      this.saveModel,
      this.form.value
    );
    this.isLoading = true;
    this._appUserService
      .changePassword(templateDto)
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe(
        res => {
          if (res) {
            this.alertService.success("Password Successfully changed");
            this.goBack();
          }
        },
        err => {
          this.alertService.error(err);
        }
      );
  };

  updateUserInfo = () => {
    this.authService.getCurrentUser().subscribe(res => {
      const userData: AppUserDto = Object.assign(
        {},
        new AppUserDto(),
        res.Data
      );
      debugger;
      this.userInfoService.saveUserInfo(userData);
    });
  };
}
