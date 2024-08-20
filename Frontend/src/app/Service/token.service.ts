import { Injectable } from '@angular/core';
import { parseJwt } from '../Shared/common-functions';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private ACCESS_TOKEN: string = 'authToken';

  constructor(private _router: Router) {}

  setToken(accToken: string) {
    localStorage.setItem(this.ACCESS_TOKEN, accToken);
  }
  getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN);
  }

  isAccessTokenExpired() {
    const decToken = this.getDecryptedToken();
    return Math.floor(Date.now() / 1000) > decToken.exp;
  }

  clearToken() {
    localStorage.removeItem(this.ACCESS_TOKEN);
    this._router.navigate(['/login']);
  }
  getUserNameFromToken(): string {
    const accToken = this.getAccessToken();
    const decToken = accToken && parseJwt(accToken);
    return decToken.userName;
  }
  getDecryptedToken() {
    const accToken = this.getAccessToken();
    return accToken && parseJwt(accToken);
  }
}
