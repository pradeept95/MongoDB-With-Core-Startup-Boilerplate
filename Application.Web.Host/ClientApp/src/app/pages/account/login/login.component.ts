import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { Router, ActivatedRoute } from "@angular/router";
import { AuthenticationService, AlertService } from "shared/_services";
import { LoginRequestModel } from "shared/_models/auth-model";
import { finalize } from "rxjs/operators";
import { UserInfoService } from "shared/_services/user-info.service";
import { AppUserDto } from "shared/_models";

@Component({
  selector: "fury-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"]
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  inputType = "password";
  visible = false;
  loading: boolean = false;
  returnUrl: string;

  authenticateModel: LoginRequestModel = new LoginRequestModel();

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private cd: ChangeDetectorRef,
    private snackbar: MatSnackBar,
    private authService: AuthenticationService,
    private alertService: AlertService,
    private userInfoService: UserInfoService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      usernameOrEmail: ["", Validators.required],
      password: ["", Validators.required],
      isRemember: [false]
    });

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/";

    // reset login status
    if (this.authService.isLogedIn()) {
      this.router.navigate([this.returnUrl]);
    } else {
      this.authService.logout();
    }
  }

  login() {
    if (!this.form.valid) {
      this.snackbar.open("Please provide all required information", "GOT IT", {
        duration: 3000
      });
      this.alertService.error("Please provide all required information");
      return;
    }
    const loginModel: LoginRequestModel = Object.assign(
      {},
      this.authenticateModel,
      this.form.value
    );

    this.loading = true;
    this.authService
      .login(loginModel)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe(
        result => {
          if (result.IsLoginSuccess) {
            this.router.navigate([this.returnUrl]);
          } else {
            this.authService.logout();
            this.alertService.error("Login failed, Please try again.");
          }
        },
        error => {
          this.alertService.error(error);
        }
      );
  }

  toggleVisibility() {
    if (this.visible) {
      this.inputType = "password";
      this.visible = false;
      this.cd.markForCheck();
    } else {
      this.inputType = "text";
      this.visible = true;
      this.cd.markForCheck();
    }
  }
}
