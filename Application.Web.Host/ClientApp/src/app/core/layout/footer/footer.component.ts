import { Component, OnInit } from "@angular/core";
import { Subscription } from "rxjs";
import { AlertService } from "shared/_services";
import { MatSnackBar } from "@angular/material";

@Component({
  selector: "fury-footer",
  templateUrl: "./footer.component.html",
  styleUrls: ["./footer.component.scss"]
})
export class FooterComponent implements OnInit {
  visible: boolean = false;

  private subscription: Subscription;

  message: any;
  myMessage: string = "";

  constructor(
    private alertService: AlertService,
    private snackbar: MatSnackBar
  ) {}

  ngOnInit() {
    this.subscription = this.alertService.getMessage().subscribe(message => {
      if (message) {
        this.message = message;
        this.visible = true;
        this.myMessage = this.message.text;
        // this.snackbar.open(this.message.text, "GOT IT", { duration: 5000 });

        setTimeout(() => {
          this.visible = false;
          this.myMessage = "";
        }, 5000);
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
