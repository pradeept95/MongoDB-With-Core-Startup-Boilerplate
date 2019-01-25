export class LoginRequestModel {
  usernameOrEmail: string;
  password: string;
  isRemember: boolean;
}

export class LoginResponseModel {
  IsLoginSuccess: boolean;
  Token: string;
  Expires: number;
  UserId: string;
  FullName: string;
  Email: string;
}
