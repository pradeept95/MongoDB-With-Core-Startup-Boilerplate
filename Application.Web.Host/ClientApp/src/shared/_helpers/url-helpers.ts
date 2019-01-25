export const loginUrl = `/api/users/authenticate`;
export const currentUserInfoUrl = `/api/users/GetLogedInUser`;

//for setting
export const setting_GetEdit = `/api/Setting/getEdit`;
export const setting_Update = `/api/Setting/Update`;

//for user management
export const appUser_GetAllUrl = `/api/AppUser/GetAll`;
export const appUser_GetAllPagedUrl = `/api/AppUser/GetAllPaged`;
export const appUser_GetUrl = `/api/AppUser/Get`;
export const appUser_SaveUrl = `/api/AppUser/Save`;
export const appUser_UpdateUrl = `/api/AppUser/Update`;
export const appUser_DeleteUrl = `/api/AppUser/Delete`;
export const appUser_ChangePasswordUrl = `/api/AppUser/ChangePassword`;
export const appUser_ChangeMyPasswordUrl = `/api/AppUser/ChangeMyPassword`;

//for pdf handler
export const pdfHandler_GetAllUrl = `/api/PdfHandler/GetAll`;
export const pdfHandler_GetAllPagedUrl = `/api/PdfHandler/GetAllPaged`;

export class UrlHelper {
  /**
   * The URL requested, before initial routing.
   */
  static readonly initialUrl = location.href;

  static getQueryParameters(): any {
    return document.location.search
      .replace(/(^\?)/, "")
      .split("&")
      .map(
        function(n) {
          return (n = n.split("=")), (this[n[0]] = n[1]), this;
        }.bind({})
      )[0];
  }
}
