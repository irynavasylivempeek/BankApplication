import { Directive, forwardRef, Attribute } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[validateEqual][formControlName],[validateEqual][formControl],[validateEqual][ngModel]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => EqualValidator), multi: true }
  ]
})

export class EqualValidator implements Validator {
  constructor(@Attribute('validateEqual') public validateEqual: string, @Attribute('reverse') public reverse: string) { }

  private get isReverse() {
    if (!this.reverse) {
      return false;
    }
    return this.reverse === 'true';
  }

  validate(control: AbstractControl): { [key: string]: boolean } | null {
    const value = control.value;
    const element = control.root.get(this.validateEqual);

    if (element && value === element.value && this.isReverse) {
      delete element.errors['validateEqual'];
      if (!Object.keys(element.errors).length) {
        element.setErrors(null);
      }
    }

    if (element && value !== element.value && this.isReverse) {
      element.setErrors({ validateEqual: true });
    }

    if (element && value !== element.value && !this.isReverse) {
      return { validateEqual: true };
    }

    return null;
  }
}
