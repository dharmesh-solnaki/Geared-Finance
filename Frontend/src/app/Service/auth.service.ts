import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { LoginDataModel } from '../Models/common-models';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private API_URL: string = `${environment.BASE_URL}/Auth`;
  constructor(private _http: HttpClient) {}

  authenticateUser(
    userData: LoginDataModel
  ): Observable<{ accessToken: string }> {
    return this._http.post<{ accessToken: string }>(this.API_URL, userData);
  }
  validateToken(): Observable<{ accessToken: string }> {
    return this._http.get<{ accessToken: string }>(`${this.API_URL}/Token`);
  }
  validateMail(email: string): Observable<{ isMailExist: boolean }> {
    return this._http.get<{ isMailExist: boolean }>(
      `${this.API_URL}?email=${email}`
    );
  }
  validateOtp(email: string, otp: string): Observable<{ isValidOtp: boolean }> {
    const methodBody = { Email: email, Otp: otp };
    return this._http.post<{ isValidOtp: boolean }>(
      `${this.API_URL}/Otp`,
      methodBody
    );
  }
  updateCredential(
    email: string,
    password: string
  ): Observable<{ isPassUpdated: boolean }> {
    const methodBody = { email, password };
    return this._http.post<{ isPassUpdated: boolean }>(
      `${this.API_URL}/Credential`,
      methodBody
    );
  }
}
