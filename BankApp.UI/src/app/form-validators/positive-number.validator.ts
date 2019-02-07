import { Directive, forwardRef, Attribute } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[positiveNumber][formControlName],[positiveNumber][formControl],[positiveNumber][ngModel]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => PositiveNumberValidator), multi: true }
  ]
})

export class PositiveNumberValidator implements Validator {
  constructor(@Attribute('min') public min: number) { }

  validate(control: AbstractControl): { [key: string]: boolean } | null {
    const value = control.value;

    if (value != null && this.min != null && value < this.min) {
      return { positiveNumber: true };
    }
    return null;
  }
}
