import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { alertResponses, validationRegexes } from '../Shared/constants';
import { AuthService } from '../Service/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { matchPasswords } from '../Shared/validators/password-match.validator';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './app-forgot-password.component.html',
})
export class AppForgotPasswwordComponent {
  isShowOtp: boolean = false;
  isShowPass: boolean = false;
  userMail: string = '';
  forgotPassForm: FormGroup;
  isFormSubmitted: boolean = false;
  constructor(
    private fb: FormBuilder,
    private _authService: AuthService,
    private _toaster: ToastrService,
    private _router: Router
  ) {
    this.forgotPassForm = this.fb.group({});
    this.addEmailControl();
  }

  addEmailControl() {
    this.forgotPassForm.addControl(
      'email',
      this.fb.control('', [
        Validators.required,
        Validators.pattern(validationRegexes.EMAIL_REGEX),
      ])
    );
  }

  addOtpControl() {
    this.forgotPassForm.addControl(
      'otp',
      this.fb.control('', Validators.required)
    );
  }

  addPasswordControls() {
    this.forgotPassForm.addControl(
      'newPass',
      this.fb.control('', [
        Validators.required,
        Validators.pattern(validationRegexes.PASSWORD_REGEX),
      ])
    );
    this.forgotPassForm.addControl(
      'confirmPass',
      this.fb.control('', [Validators.required])
    );
    this.forgotPassForm.setValidators(matchPasswords('newPass', 'confirmPass'));
  }

  removeControls(controls: string[]) {
    controls.forEach((control) => this.forgotPassForm.removeControl(control));
  }

  onFormSubmit() {
    this.isFormSubmitted = true;
    if (this.forgotPassForm.invalid) {
      return;
    }
    if (this.isShowOtp) {
      this.validateOtp();
    } else if (this.isShowPass) {
      this.updateNewPassword();
    } else {
      this.validateEmail();
    }
  }

  validateEmail() {
    const email = this.forgotPassForm.get('email')?.value;
    this._authService.validateMail(email).subscribe(
      (res) => {
        if (res.isMailExist) {
          this._toaster.success(alertResponses.ON_OTP_SUCCESS);
          this.isShowOtp = true;
          this.addOtpControl();
          this.removeControls(['email']);
          this.userMail = email;
        } else {
          this._toaster.error(alertResponses.ON_EMAIL_NOT_EXIST);
        }
      },
      () => this._toaster.error(alertResponses.ERROR)
    );
  }

  validateOtp() {
    const otp = this.forgotPassForm.get('otp')?.value;
    this._authService.validateOtp(this.userMail, otp).subscribe(
      (res) => {
        if (res.isValidOtp) {
          this.isShowOtp = false;
          this.isShowPass = true;
          this.addPasswordControls();
          this.removeControls(['otp']);
        } else {
          this._toaster.error(alertResponses.ON_OTP_INVALID);
        }
      },
      () => this._toaster.error(alertResponses.ERROR)
    );
  }

  updateNewPassword() {
    const pass = this.forgotPassForm.get('newPass')?.value;
    this._authService.updateCredential(this.userMail, pass).subscribe(
      (res) => {
        if (res.isPassUpdated) {
          this._toaster.success(alertResponses.ON_PASSWORD_CHANGE_SUCCESS);
          this._router.navigate(['/login']);
        } else {
          this._toaster.error(alertResponses.ERROR);
        }
      },
      () => this._toaster.error(alertResponses.ERROR)
    );
    this.forgotPassForm.reset();
  }

  OtpBackBtnClick() {
    this.userMail = '';
    this.isShowOtp = false;
    this.addEmailControl();
    this.removeControls(['otp']);
  }
}
