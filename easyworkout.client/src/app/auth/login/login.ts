import { Component, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, MatProgressSpinnerModule, MatIconModule, MatButtonModule, 
    MatFormFieldModule, MatInputModule],
  templateUrl: './login.html'
})
export class LoginComponent {
  isWaiting = signal(false);
  validationErrors = signal<string[]>([]);
  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    
    this.form = this.fb.nonNullable.group({
      userName: [''],
      password: ['']
    });
  }

  submit() {
    this.isWaiting.set(true);
    this.validationErrors.set([]);
    this.auth.login(
      this.form.value.userName!,
      this.form.value.password!
    ).subscribe({
      next: () => {
        this.router.navigate(['workouts']);
      },
      error: (err) => {
        this.validationErrors.update(errs => [...errs, err.error?.message ?? 'Login failed.']);
      },
      complete: () => {
        this.isWaiting.set(false);
      }
    });
  }
}
