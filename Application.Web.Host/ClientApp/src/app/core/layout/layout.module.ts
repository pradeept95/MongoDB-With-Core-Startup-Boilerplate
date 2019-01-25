import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BackdropModule } from '../../shared/backdrop/backdrop.module';
import { LoadingIndicatorModule } from '../../shared/loading-indicator/loading-indicator.module';
import { MaterialModule } from '../../shared/material-components.module';
import { MediaQueryService } from '../../shared/mediareplay/media-replay.service';
import { FooterModule } from './footer/footer.module';
import { LayoutComponent } from './layout.component';
import { QuickpanelModule } from './quickpanel/quickpanel.module';
import { SidenavModule } from './sidenav/sidenav.module';
import { ToolbarModule } from './toolbar/toolbar.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule,
    LoadingIndicatorModule,

    // Core
    ToolbarModule,
    QuickpanelModule,
    SidenavModule,
    FooterModule,
    BackdropModule
  ],
  declarations: [LayoutComponent],
  providers: [MediaQueryService]
})
export class LayoutModule {
}
