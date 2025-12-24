import { Component, signal, computed, effect } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './auth/auth.service';
import { TokenService } from './core/token.service';
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
  user = signal<UserResponse | null>(null);
  constructor(
    private authService: AuthService, 
    private tokenService: TokenService, 
    private accountService: AccountService) {
    effect(() => {
      if (this.isLoggedIn()) {
        this.accountService.get().subscribe(u => this.user.set(u));
      } else {
        this.user.set(null);
      }
    });
  }
  
  logout() {
    this.authService.logout();
  }  
  
  isLoggedIn = computed(() => {
    return this.tokenService.hasTokenSignal() ?? false;
  });
}