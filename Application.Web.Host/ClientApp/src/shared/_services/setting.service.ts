import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { map } from "rxjs/operators";
import { Observable } from "rxjs";
import {
  ListResultDto,
  PagedResultDto,
  ResultDto
} from "shared/paged-listing-component-base";
import {
  FileInfoDto,
  FileDirectorySetting
} from "shared/_models/file-info-model";
import { setting_GetEdit, setting_Update } from "shared/_helpers/url-helpers";

@Injectable()
export class SettingService {
  pdfData: FileInfoDto = new FileInfoDto();
  listResult: ListResultDto = new ListResultDto();
  pagedResult: PagedResultDto = new PagedResultDto();
  result: ResultDto = new ResultDto();

  constructor(private http: HttpClient) {}

  get(): Observable<ResultDto> {
    let url = `${setting_GetEdit}/`;
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

  update(input: FileDirectorySetting): Observable<ResultDto> {
    let url = `${setting_Update}`;
    url = url.replace(/[?&]$/, "");

    const content_ = JSON.stringify(input);

    return this.http.put<any>(url, input).pipe(
      map(result => {
        if (result) {
          var data = result["Result"];

          const res: ResultDto = Object.assign({}, this.result, data);
          return res;
        }
      })
    );
  }
}
