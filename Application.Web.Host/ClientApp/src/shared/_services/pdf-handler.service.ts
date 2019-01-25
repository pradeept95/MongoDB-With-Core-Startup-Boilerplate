import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { map } from "rxjs/operators";
import { Observable } from "rxjs";
import {
  ListResultDto,
  PagedResultDto,
  ResultDto
} from "shared/paged-listing-component-base";
import { FileInfoDto } from "shared/_models/file-info-model";
import {
  pdfHandler_GetAllUrl,
  pdfHandler_GetAllPagedUrl
} from "shared/_helpers/url-helpers";

@Injectable()
export class PdfHandlerService {
  pdfData: FileInfoDto = new FileInfoDto();
  listResult: ListResultDto = new ListResultDto();
  pagedResult: PagedResultDto = new PagedResultDto();
  result: ResultDto = new ResultDto();

  constructor(private http: HttpClient) {}

  getAll(): Observable<ListResultDto> {
    let url = pdfHandler_GetAllUrl;
    url = url.replace(/[?&]$/, "");

    return this.http.get<any>(url).pipe(
      map(result => {
        if (result.IsSuccess) {
          var data = result["Result"];

          const res: ListResultDto = Object.assign({}, this.listResult, data);
          return res;
        }
      })
    );
  }

  getAllPaged(
    searchText,
    skipCount = 0,
    maxResultCount = 10
  ): Observable<PagedResultDto> {
    let url_ = `${pdfHandler_GetAllPagedUrl}?`;
    if (skipCount !== undefined)
      url_ += "skip=" + encodeURIComponent("" + skipCount) + "&";
    if (maxResultCount !== undefined)
      url_ += "take=" + encodeURIComponent("" + maxResultCount) + "&";
    if (searchText !== undefined && searchText !== "")
      url_ += "searchText=" + encodeURIComponent("" + searchText) + "&";
    url_ = url_.replace(/[?&]$/, "");

    return this.http.get<any>(url_).pipe(
      map(result => {
        if (result.IsSuccess) {
          var data = result["Result"];
          const res: PagedResultDto = Object.assign({}, this.pagedResult, data);
          return res;
        }
      })
    );
  }

  // get(id: string): Observable<ResultDto> {
  //   let url = `${appUser_GetUrl}/`;
  //   if (id !== undefined) url += encodeURIComponent("" + id);
  //   url = url.replace(/[?&]$/, "");

  //   return this.http.get<any>(url).pipe(
  //     map(result => {
  //       if (result.IsSuccess) {
  //         var data = result["Result"];

  //         const res: ResultDto = Object.assign({}, this.result, data);
  //         return res;
  //       }
  //     })
  //   );
  // }

  // create(input: AppUserDto): Observable<ResultDto> {
  //   let url = `${appUser_SaveUrl}`;
  //   url = url.replace(/[?&]$/, "");

  //   const content_ = JSON.stringify(input);

  //   let options_: any = {
  //     body: content_,
  //     observe: "response",
  //     responseType: "blob",
  //     headers: new HttpHeaders({
  //       "Content-Type": "application/json",
  //       Accept: "application/json"
  //     })
  //   };

  //   return this.http.post<any>(url, content_, options_).pipe(
  //     map(result => {
  //       if (result) {
  //         var data = result["Result"];

  //         const res: ResultDto = Object.assign({}, this.result, data);
  //         return res;
  //       }
  //     })
  //   );
  // }

  // update(input: AppUserDto): Observable<ResultDto> {
  //   let url = `${appUser_UpdateUrl}`;
  //   url = url.replace(/[?&]$/, "");

  //   const content_ = JSON.stringify(input);

  //   let options_: any = {
  //     body: content_,
  //     observe: "response",
  //     responseType: "blob",
  //     headers: new HttpHeaders({
  //       "Content-Type": "application/json",
  //       Accept: "application/json"
  //     })
  //   };

  //   return this.http.put<any>(url, content_, options_).pipe(
  //     map(result => {
  //       if (result) {
  //         var data = result["Result"];

  //         const res: ResultDto = Object.assign({}, this.result, data);
  //         return res;
  //       }
  //     })
  //   );
  // }

  // delete(id: string): Observable<boolean> {
  //   let url = `${appUser_DeleteUrl}/`;
  //   if (id !== undefined) url += encodeURIComponent("" + id);
  //   url = url.replace(/[?&]$/, "");

  //   return this.http.delete<any>(url).pipe(
  //     map(result => {
  //       if (result.IsSuccess) {
  //         var data = result["Result"];

  //         const res: ResultDto = Object.assign({}, this.result, data);
  //         return res.Data ? true : false;
  //       } else {
  //         return false;
  //       }
  //     })
  //   );
  // }
}
