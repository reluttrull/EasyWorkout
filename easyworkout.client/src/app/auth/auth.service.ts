import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { TokenService } from '../core/token.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = 'https://localhost:7033/api/auth';

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private router: Router
  ) {}

  login(userName: string, password: string) {
    return this.http
      .post<{ accessToken: string, refreshToken: string  }>(`${this.baseUrl}/login`, {
        userName,
        password
      })
      .pipe(
        tap(res => this.tokenService.setTokens(res.accessToken, res.refreshToken))
      );
  }

  refreshToken() {
    const refreshToken = this.tokenService.getRefreshToken();
    return this.http
      .post<{ accessToken: string; refreshToken: string }>(`${this.baseUrl}/refresh`, {
        refreshToken
      })
      .pipe(
        tap(res => this.tokenService.setTokens(res.accessToken, res.refreshToken))
      );
  }

  register(data: any) {
    return this.http.post(`${this.baseUrl}/register`, data);
  }

  logout() {
    this.tokenService.clear();
    this.router.navigate(['/login']);
  }
}
