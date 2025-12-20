import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { TokenService } from '../core/token.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = 'https://localhost:7033/api/auth';

  constructor(
    private http: HttpClient,
    private tokenService: TokenService
  ) {}

  login(userName: string, password: string) {
    return this.http
      .post<{ token: string }>(`${this.baseUrl}/login`, {
        userName,
        password
      })
      .pipe(
        tap(res => this.tokenService.set(res.token))
      );
  }

  register(data: any) {
    return this.http.post(`${this.baseUrl}/register`, data);
  }

  logout() {
    this.tokenService.clear();
  }
}