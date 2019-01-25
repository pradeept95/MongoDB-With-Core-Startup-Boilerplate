import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AuthenticationService } from "shared/_services";
import { LoginResponseModel } from "shared/_models/auth-model";

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"]
})
export class DashboardComponent implements OnInit {
  private static isInitialLoad = true;
  userInfo: LoginResponseModel = new LoginResponseModel();

  constructor(
    private authService: AuthenticationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.userInfo = this.authService.getUserInfo();
  }
}
