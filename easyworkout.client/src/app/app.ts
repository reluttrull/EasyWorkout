import { Component, signal, computed, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import { AuthService } from './auth/auth.service';
import { AccountService } from './account/account.service';
import { GreetingPipe } from './pipes/greeting-pipe';
import { UserResponse } from './model/interfaces';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MatIconModule, MatButtonModule, GreetingPipe],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent {
  private accountService = inject(AccountService);
  private authService = inject(AuthService);
  user = this.accountService.user;
  
  logout() {
    this.authService.logout();
  }  
  
  isLoggedIn = computed(() => !!this.user());
}