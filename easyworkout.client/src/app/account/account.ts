import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from './account.service';
import { UserResponse, UpdateUserRequest } from '../model/interfaces';
import { ChangePassword } from '../components/change-password/change-password';
import { ChangeEmail } from '../components/change-email/change-email';

@Component({
  selector: 'app-account',
  imports: [ReactiveFormsModule, ChangePassword, ChangeEmail],
  templateUrl: './account.html',
  styleUrl: './account.css',
})
export class Account {
  private accountService = inject(AccountService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  form:FormGroup = this.fb.nonNullable.group({
    firstName: [''],
    lastName: ['']
  });
  validationErrors = signal<string[]>([]);
  user = this.accountService.user;
  isEdit = signal(false);
  changeEmail = signal(false);
  changePassword = signal(false);

  toggleIsEdit() {
    this.isEdit.set(!this.isEdit());
  }

  toggleChangeEmail() {
    this.changeEmail.set(!this.changeEmail());
  }
  
  toggleChangePassword() {
    this.changePassword.set(!this.changePassword());
  }
  
  submit() {
    this.validationErrors.set([]);
    
    let request: UpdateUserRequest = {
      firstName: this.form.value.firstName,
      lastName: this.form.value.lastName
    };
    this.accountService.update(request).subscribe({
      next: (res:UserResponse) => {
        this.isEdit.set(false);
        this.router.navigate(['/account']);
      },
      error: (err:any) => {
        if (err.status == 400 && err.error?.errors) {
          for (const key of Object.keys(err.error.errors)) {
            this.validationErrors.update(errs => [...errs, ...err.error.errors[key]]);
          }
        } else {
          this.validationErrors.update(errs => [...errs, 'An unexpected error occurred.']);
        };
        return;
      }
    });
  }
}
