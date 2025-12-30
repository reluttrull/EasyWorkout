import { Component, signal, computed, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import { AuthService } from './auth/auth.service';
import { AccountService } from './account/account.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MatIconModule, MatButtonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent {
  private accountService = inject(AccountService);
  private authService = inject(AuthService);
  user = this.accountService.user;
  currentYear = new Date().getFullYear();
  
  logout() {
    this.authService.logout();
  }  
  
  isLoggedIn = computed(() => !!this.user());
}