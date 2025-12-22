import { Component, signal, computed } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './auth/auth.service';
import { TokenService } from './core/token.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.html'
})
export class AppComponent {
  constructor(private authService: AuthService, private tokenService: TokenService) {
  }
  
  logout() {
    this.authService.logout();
  }  
  
  isLoggedIn = computed(() => {
    return this.tokenService.hasTokenSignal() ?? false;
  });
}