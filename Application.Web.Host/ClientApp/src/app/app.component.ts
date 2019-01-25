import { Component } from "@angular/core";
import { SidenavItem } from "./core/layout/sidenav/sidenav-item/sidenav-item.interface";
import { SidenavService } from "./core/layout/sidenav/sidenav.service";

@Component({
  selector: "fury-root",
  templateUrl: "./app.component.html"
})
export class AppComponent {
  constructor(sidenavService: SidenavService) {
    const menu: SidenavItem[] = [];

    menu.push({
      name: "APPS",
      position: 5,
      type: "subheading",
      customClass: "first-subheading"
    });

    menu.push({
      name: "Dashboard",
      routeOrFunction: "/",
      icon: "dashboard",
      position: 10,
      pathMatchExact: true
    });
    menu.push({
      name: "User Management",
      routeOrFunction: "/app/user-management",
      icon: "verified_user",
      position: 11,
      pathMatchExact: false
    });
    menu.push({
      name: "Pdf Management",
      routeOrFunction: "/app/file-management",
      icon: "picture_as_pdf",
      position: 11,
      pathMatchExact: false
    }); 

    // Send all created Items to SidenavService
    menu.forEach(item => sidenavService.addItem(item));
  }
}
