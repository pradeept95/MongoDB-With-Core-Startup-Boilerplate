import { Injectable } from "@angular/core";
import { Router, NavigationStart } from "@angular/router";
import { Observable, Subject } from "rxjs";
import { AppUserDto } from "shared/_models";

@Injectable()
export class UserInfoService {
  private subject = new Subject<any>();
  private keepAfterNavigationChange = true;

  constructor(private router: Router) {
    // clear user on route change
    router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        if (this.keepAfterNavigationChange) {
          // only keep for a single location change
          // this.keepAfterNavigationChange = true;
        } else {
          // clear userinfo
          // this.subject.next();
        }
      }
    });
  }

  saveUserInfo(userInfo: AppUserDto, keepAfterNavigationChange = true) {
    this.keepAfterNavigationChange = keepAfterNavigationChange;
    this.subject.next(userInfo);
  }

  getUserInfo(): Observable<any> {
    return this.subject.asObservable();
  }
}
