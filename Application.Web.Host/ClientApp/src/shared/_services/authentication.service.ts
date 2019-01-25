import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";

import { environment } from "../../environments/environment";
import {
  LoginRequestModel,
  LoginResponseModel
} from "shared/_models/auth-model";
import { loginUrl, currentUserInfoUrl } from "shared/_helpers/url-helpers";
import { Observable } from "rxjs";
import { ResultDto } from "shared/paged-listing-component-base";

@Injectable()
export class AuthenticationService {
  userData: LoginResponseModel = new LoginResponseModel();
  constructor(private http: HttpClient) {}
  result: ResultDto = new ResultDto();

  login(loginData: LoginRequestModel) {
    var url = loginUrl;
    return this.http.post<any>(url, loginData).pipe(
      map(userLoginResponse => {
        if (userLoginResponse.IsSuccess) {
          var user = userLoginResponse["Result"]["Data"];

          const loginUserModel: LoginResponseModel = Object.assign(
            {},
            this.userData,
            user
          );

          if (
            loginUserModel &&
            loginUserModel.IsLoginSuccess &&
            loginUserModel.Token
          ) {
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem("currentUser", JSON.stringify(user));
          }
          return loginUserModel;
        }
        // login successful if there's a jwt token in the response
      })
    );
  }

  getCurrentUser(): Observable<ResultDto> {
    let url = `${currentUserInfoUrl}/`;
    url = url.replace(/[?&]$/, "");

    return this.http.get<any>(url).pipe(
      map(result => {
        if (result.IsSuccess) {
          var data = result["Result"];

          const res: ResultDto = Object.assign({}, this.result, data);
          return res;
        }
      })
    );
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem("currentUser");
  }

  isLogedIn() {
    if (localStorage.getItem("currentUser")) {
      const userInfo: LoginResponseModel = Object.assign(
        {},
        this.userData,
        JSON.parse(localStorage.getItem("currentUser"))
      );

      if (userInfo.Token) {
        return true;
      }
      return false;
    }
    return false;
  }

  getUserInfo() {
    if (localStorage.getItem("currentUser")) {
      const userInfo: LoginResponseModel = Object.assign(
        {},
        this.userData,
        JSON.parse(localStorage.getItem("currentUser"))
      );
      return userInfo;
    }
    return null;
  }
}
