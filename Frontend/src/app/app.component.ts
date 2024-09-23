import { Component, TemplateRef } from '@angular/core';
import { TokenService } from './Service/token.service';
import { Router } from '@angular/router';
import { SharedTemplateService } from './Service/shared-template.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  shouldApplyWrapper: boolean = false;

  constructor(
    private _tokenService: TokenService,
    private _router: Router,
    private _templateService: SharedTemplateService
  ) {}
  viewHandler(isNavClosed: boolean) {
    this.shouldApplyWrapper = isNavClosed;
  }
  get getLoginStatus() {
    const token = this._tokenService.getAccessToken();
    return !!token;
  }
  get isForgotPassStatus() {
    return this._router.url === '/forgot-password';
  }
  get getTemplateRef(): TemplateRef<HTMLElement> | null {
    return this._templateService.getTemplate();
  }
}
