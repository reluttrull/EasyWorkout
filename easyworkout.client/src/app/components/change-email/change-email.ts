import { Component, inject, output, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { AccountService } from '../../account/account.service';
import { ChangeEmailRequest, UserResponse } from '../../model/interfaces';

@Component({
  selector: 'app-change-email',
  imports: [ReactiveFormsModule, MatIconModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './change-email.html',
  styleUrl: './change-email.css',
})
export class ChangeEmail {
  private accountService = inject(AccountService);
  private fb = inject(FormBuilder);

  onCancel = output();

  form:FormGroup = this.fb.nonNullable.group({
    email: ['']
  });
  validationErrors = signal<string[]>([]);

  cancel() {
    this.onCancel.emit();
  }
  
  submit() {
    this.validationErrors.set([]);
    
    let request: ChangeEmailRequest = {
      email: this.form.value.email
    };
    this.accountService.changeEmail(request).subscribe({
      next: (res:UserResponse) => {
        alert('Email changed!');
        this.onCancel.emit();
      },
      error: (err:any) => {
        if (err.status == 400 && err.error?.errors) {
          for (const key of Object.keys(err.error.errors)) {
            this.validationErrors.update(errs => [...errs, ...err.error.errors[key]]);
          }
        } else if (err.error) {
          this.validationErrors.update(errs => [...errs, err.error]);
        } else {
          this.validationErrors.update(errs => [...errs, 'An unexpected error occurred.']);
        };
        return;
      }
    });
  }
}
