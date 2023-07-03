import { AbstractControl, ValidationErrors } from "@angular/forms";
import { Observable } from "rxjs";

export interface AsyncValidatorFn {
    (control: AbstractControl):
    | Promise<ValidationErrors | null>
    | Observable<ValidationErrors | null>;
}
   