import { AbstractControl, ValidationErrors } from "@angular/forms";

export interface ValidatorFn {
    (control: AbstractControl): ValidationErrors | null;
}