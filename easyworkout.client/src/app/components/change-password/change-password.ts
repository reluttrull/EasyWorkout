import { Component, inject, output, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../../account/account.service';
import { ChangePasswordRequest, UserResponse } from '../../model/interfaces';

@Component({
  selector: 'app-change-password',
  imports: [ReactiveFormsModule],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css',
})
export class ChangePassword {
  private accountService = inject(AccountService);
  private fb = inject(FormBuilder);

  onCancel = output();

  form:FormGroup = this.fb.nonNullable.group({
    currentPassword: [''],
    newPassword: [''],
    confirmNewPassword: ['']
  });
  validationErrors = signal<string[]>([]);

  cancel() {
    this.onCancel.emit();
  }
  
  submit() {
    this.validationErrors.set([]);
    if (this.form.value.newPassword != this.form.value.confirmNewPassword) {
      this.validationErrors.update(errs => [...errs, 'Typed passwords do not match.']);
      return;
    }
    
    let request: ChangePasswordRequest = {
      currentPassword: this.form.value.currentPassword,
      newPassword: this.form.value.newPassword
    };
    this.accountService.changePassword(request).subscribe({
      next: (res:UserResponse) => {
        alert('Password changed!');
        this.onCancel.emit();
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
