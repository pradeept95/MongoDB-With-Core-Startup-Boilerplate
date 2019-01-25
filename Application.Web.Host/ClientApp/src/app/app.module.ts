import { CommonModule } from "@angular/common";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ServiceWorkerModule } from "@angular/service-worker";

import "hammerjs"; // Needed for Touch functionality of Material Components
import { environment } from "../environments/environment";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { CoreModule } from "./core/core.module";
import { AuthGuard } from "shared/_guards";
import { AlertService, AuthenticationService } from "shared/_services";
import {
  JwtInterceptor,
  ErrorInterceptor
  // fakeBackendProvider
} from "shared/_helpers";
import { AlertComponent } from "shared/_directives";
import { UserInfoService } from "shared/_services/user-info.service";

@NgModule({
  imports: [
    // Angular Core Module // Don't remove!
    CommonModule,
    BrowserAnimationsModule,
    HttpClientModule,

    // Fury Core Modules
    CoreModule,
    AppRoutingModule,

    // Register a Service Worker (optional)
    ServiceWorkerModule.register("/ngsw-worker.js", {
      enabled: environment.production
    }),

    ServiceWorkerModule.register("ngsw-worker.js", {
      enabled: environment.production
    })
  ],
  declarations: [AppComponent],
  providers: [
    AuthGuard,
    AlertService,
    UserInfoService,
    AuthenticationService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }

    // provider used to create fake backend
    // fakeBackendProvider
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
