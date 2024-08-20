import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function matchPasswords(
  password: string,
  confPassword: string
): ValidatorFn {
  return (form: AbstractControl): ValidationErrors | null => {
    const passControl = form.get(password);
    const confPassControl = form.get(confPassword);
    if (!passControl || !confPassControl) {
      return null;
    }
    const error =
      passControl.value === confPassControl.value
        ? null
        : { isMatching: false };
    if (error) {
      confPassControl.setErrors(error);
    } else {
      confPassControl.setErrors(null);
    }
    return error;
  };
}
