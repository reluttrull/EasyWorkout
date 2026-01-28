import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { AccountService } from '../account/account.service';
import { TokenService } from '../core/token.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = `${environment.authApi}/api/auth`;
  private accountService = inject(AccountService);
  private tokenService = inject(TokenService);
  private http = inject(HttpClient);
  private router = inject(Router);

  login(userName: string, password: string) {
    return this.http
      .post<{ accessToken: string, refreshToken: string  }>(`${this.baseUrl}/login`, {
        userName,
        password
      })
      .pipe(
        tap(res => {
          this.tokenService.setTokens(res.accessToken, res.refreshToken);
          this.accountService.loadUser();
          }));
  }

  refreshToken() {
    const refreshToken = this.tokenService.getRefreshToken();
    return this.http
      .post<{ accessToken: string; refreshToken: string }>(`${this.baseUrl}/refresh`, {
        refreshToken
      })
      .pipe(
        tap(res => {
          this.tokenService.setTokens(res.accessToken, res.refreshToken);
          this.accountService.loadUser();
      }));
  }

  register(data: any) {
    return this.http.post(`${this.baseUrl}/register`, data);
  }

  logout() {
    this.tokenService.clear();
    this.accountService.clear();
    this.router.navigate(['login']);
  }
}