import {
  AbstractControl,
  ControlContainer,
  ValidationErrors,
  ValidatorFn,
} from '@angular/forms';

export function arrayNotEmptyValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    return Array.isArray(value) && value.length !== 0
      ? null
      : { arrayIsEmpty: true };
  };
}
