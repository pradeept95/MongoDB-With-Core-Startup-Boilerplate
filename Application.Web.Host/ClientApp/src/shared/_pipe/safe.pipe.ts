import { Pipe, PipeTransform } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";

@Pipe({ name: "safe" })
export class SafePipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) {}
  transform(url) {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
}

import { NgModule } from "@angular/core";
@NgModule({
  declarations: [SafePipe],
  exports: [SafePipe]
})
export class SharedPipesModule {}
