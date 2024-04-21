import { AbstractControl, AsyncValidatorFn } from "@angular/forms"
import { debounceTime, finalize, map, switchMap, take } from "rxjs"
import { AccountService } from "../services/account.service"

export function validateEmailNotTaken(accountService: AccountService):AsyncValidatorFn {
    return (control : AbstractControl) => {
      return control.valueChanges.pipe(
        debounceTime(1000),
        take(1),
        switchMap(() => {
          return accountService.checkEmailExists(control.value).pipe(
            map(result => result ? {emailExists: true} : null),
            finalize(() => control.markAsTouched())
          )
        })
      )
    }
  }