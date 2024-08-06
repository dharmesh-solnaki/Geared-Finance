import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { alertResponses, validationRegexes } from '../Shared/constants';
import { AuthService } from '../Service/auth.service';
import { parseJwt } from '../Shared/common-functions';
import { ToastrService } from 'ngx-toastr';
import { TokenService } from '../Service/token.service';


@Component({
  selector: 'app-login',
  templateUrl: './app-login.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class AppLoginComponent {

loginForm:FormGroup
constructor(
  private _fb:FormBuilder,
   private _authService:AuthService,
   private _toaster:ToastrService,
   private _tokenService:TokenService
   ){

this.loginForm = this._fb.group({
  email: ['', [Validators.required, Validators.pattern(validationRegexes.EMAIL_REGEX)]],
  password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(validationRegexes.PASSWORD_REGEX)]],
  isRemember: [false]
})
}
onLoginFomSubmit(){
  if(this.loginForm.invalid){
      this.loginForm.markAllAsTouched()
      return
  }

  this._authService.authenticateUser(this.loginForm.value).subscribe((res)=>{
   if(res.accessToken){
     this._toaster.success(alertResponses.ON_LOGIN_SUCCESS)
     this._tokenService.setToken(res.accessToken)
   }else{
    this._toaster.error(alertResponses.ON_LOGIN_ERROR)
   }
  })

}
}
