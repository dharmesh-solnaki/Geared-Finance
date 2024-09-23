import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { alertResponses, validationRegexes } from '../Shared/constants';
import { AuthService } from '../Service/auth.service';
import { ToastrService } from 'ngx-toastr';
import { TokenService } from '../Service/token.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './app-login.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class AppLoginComponent {
  loginForm: FormGroup;
  isFormSubmitted: boolean = false;
  isEnableLoader: boolean = false;
  constructor(
    private _fb: FormBuilder,
    private _authService: AuthService,
    private _toaster: ToastrService,
    private _tokenService: TokenService,
    private _router: Router
  ) {
    this.loginForm = this._fb.group({
      email: [
        String.Empty,
        [
          Validators.required,
          Validators.pattern(validationRegexes.EMAIL_REGEX),
        ],
      ],
      password: [
        String.Empty,
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(validationRegexes.PASSWORD_REGEX),
        ],
      ],
      isRemember: [false],
    });
  }
  onLoginFomSubmit() {
    this.isFormSubmitted = true;
    if (this.loginForm.invalid) {
      return;
    }
    this.isEnableLoader = true;
    this._authService
      .authenticateUser(this.loginForm.value)
      .subscribe((res) => {
        if (res.accessToken) {
          this._tokenService.setToken(res.accessToken);
          this._router.navigate(['settings/role']);
        } else {
          this._toaster.error(alertResponses.ON_LOGIN_ERROR);
        }
        this.isEnableLoader = false;
      });
  }
}
