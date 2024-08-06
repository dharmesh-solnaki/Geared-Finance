import { Component } from '@angular/core';
import { TokenService } from './Service/token.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  shouldApplyWrapper: boolean = false;

   constructor(private _tokenService:TokenService,private _router:Router) {}
  viewHandler(isNavClosed: boolean) {
    this.shouldApplyWrapper = isNavClosed;
  }
  get getLoginStatus(){
    const token = this._tokenService.getAccessToken()
    return !!token
  }
  get isForgotPassStatus(){
  return this._router.url==="/forgot-password";
  }
  

}
