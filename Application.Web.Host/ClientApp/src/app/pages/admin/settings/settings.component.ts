import { Component, Injector, OnInit } from "@angular/core";

import { Router, ActivatedRoute } from "@angular/router";
import { AlertService } from "shared/_services";
import { AppComponentBase } from "shared/app-component-base";
import { FileDirectorySetting } from "shared/_models/file-info-model";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { SettingService } from "shared/_services/setting.service";
import { finalize } from "rxjs/operators";

@Component({
  selector: "fury-settings",
  templateUrl: "./settings.component.html",
  styleUrls: ["./settings.component.scss"]
})
export class SettingsComponent extends AppComponentBase implements OnInit {
  settingData: FileDirectorySetting = new FileDirectorySetting();
  saveModel: FileDirectorySetting = new FileDirectorySetting();

  form: FormGroup;

  constructor(
    private injector: Injector,
    private _router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private alertService: AlertService,
    private settingService: SettingService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.prepareForm();
  }

  prepareForm = () => {
    this.form = this.fb.group({
      Id: [""],
      DefaultDirectoryPath: ["", Validators.required],
      DirectoryPath: ["", Validators.required],
      ArchiveDirectoryPath: ["", Validators.required]
    });
    this.getValueForEdit();
  };

  getValueForEdit = () => {
    this.isLoading = true;
    this.settingService
      .get()
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe(result => {
        this.settingData = Object.assign({}, this.saveModel, result.Data);
        this.form.patchValue({
          Id: this.settingData.Id,
          DefaultDirectoryPath: this.settingData.DefaultDirectoryPath,
          DirectoryPath: this.settingData.DirectoryPath,
          ArchiveDirectoryPath: this.settingData.ArchiveDirectoryPath
        });
      });
  };

  reset = () => {
    this.form.patchValue({
      DefaultDirectoryPath: "",
      DirectoryPath: "",
      ArchiveDirectoryPath: ""
    });
  };

  update = () => {
    const settingDto: FileDirectorySetting = Object.assign(
      {},
      this.saveModel,
      this.form.value
    );

    this.isLoading = true;
    this.settingService
      .update(settingDto)
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe(
        res => {
          if (res) {
            this.alertService.success("Setting successfully updated.");
               this.getValueForEdit();
          }
        },
        err => {
          this.alertService.error(err);
        }
      );
  };
}
