export class BaseResponse<T> {
  constructor(public responseData: T[], public totalRecords: number) {}
}

export class Vendor {
  constructor(public id: number, public name: string) {}
}

export class IsExistData {
  constructor(public isEmailExist: boolean, public isExistMobile: boolean) {}
}

export class LoginDataModel {
  constructor(
    public email: string,
    public password: string,
    public isRemember: boolean
  ) {}
}

export interface ApiAddress {
  address: string;
  postcode: string;
  state: string;
  suburb: string;
  streetNumber: string;
  route: string;
}
