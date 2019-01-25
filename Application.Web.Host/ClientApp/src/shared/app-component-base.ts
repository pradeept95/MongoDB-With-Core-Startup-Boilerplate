import { Injector, ElementRef } from "@angular/core";
import { MatSnackBar } from "@angular/material";

export abstract class AppComponentBase {
  isLoading: boolean = false;
  elementRef: ElementRef;
  // dialog: MatDialog;
  snackbar: MatSnackBar;

  constructor(injector: Injector) {
    this.elementRef = injector.get(ElementRef);
    // this.dialog = injector.get(MatDialog);
    this.snackbar = injector.get(MatSnackBar);
  }
}
