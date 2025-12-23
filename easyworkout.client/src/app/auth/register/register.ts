import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.html'
})
export class RegisterComponent {
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
    console.log('sdjflksdj');
    if (this.form.value.password != this.form.value.confirmPassword) {
      // todo: show validation error
      return;
    }
    this.auth.register(this.form.value).subscribe(() => {
      this.router.navigate(['/login']);
    });
  }
}