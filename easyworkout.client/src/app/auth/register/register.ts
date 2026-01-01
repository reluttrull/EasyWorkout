import { Component, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, MatIconModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent {
  validationErrors = signal<string[]>([]);
  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    this.form = this.fb.nonNullable.group({
      userName: [''],
      email: [''],
      firstName: [''],
      lastName: [''],
      password: [''],
      confirmPassword: [''],
    });
  }

  submit() {
    this.validationErrors.set([]);
    if (this.form.value.password != this.form.value.confirmPassword) {
      this.validationErrors.update(errs => [...errs, 'Typed passwords do not match.']);
      return;
    }
    this.auth.register(this.form.value).subscribe({
      next: () => {
        this.router.navigate(['login']);
      },
      error: (err) => {
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