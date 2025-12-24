import { Component, signal, computed, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './auth/auth.service';
import { AccountService } from './account/account.service';
import { GreetingPipe } from './pipes/greeting-pipe';
import { UserResponse } from './model/interfaces';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, GreetingPipe],
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