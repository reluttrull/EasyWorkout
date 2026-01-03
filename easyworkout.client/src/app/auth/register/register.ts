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
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, MatProgressSpinnerModule, MatIconModule, MatButtonModule, 
    MatFormFieldModule, MatInputModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent {
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
      email: [''],
      firstName: [''],
      lastName: [''],
      password: [''],
      confirmPassword: [''],
    });
  }

  submit() {
    this.isWaiting.set(true);
    this.validationErrors.set([]);
    if (this.form.value.password != this.form.value.confirmPassword) {
      this.validationErrors.update(errs => [...errs, 'Typed passwords do not match.']);
      return;
    }
    this.auth.register(this.form.value).subscribe({
      next: () => {
        this.isWaiting.set(false);
        this.router.navigate(['login']);
      },
      error: (err) => {
        console.log(err);
        if (err.status == 400 && err.error?.errors) {
          for (var currentError in err.error.errors) {
            this.validationErrors.update(errs => [...errs, `${currentError}: ${err.error.errors[currentError]}`]);
          }
        } else {
          this.validationErrors.update(errs => [...errs, 'An unexpected error occurred.']);
        };
        this.isWaiting.set(false);
        return;
      }
    });
  }
}
