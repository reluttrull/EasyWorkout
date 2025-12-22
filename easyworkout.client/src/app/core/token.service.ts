import { Injectable, signal } from '@angular/core';

const ACCESS_TOKEN_KEY = 'jwt';
const REFRESH_TOKEN_KEY = 'refresh_token';

@Injectable({ providedIn: 'root' })
export class TokenService {
  public hasTokenSignal = signal<boolean>(localStorage.getItem(ACCESS_TOKEN_KEY) != null);

  setTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
    this.hasTokenSignal.set(true);
  }

  setAccessToken(token: string): void {
    localStorage.setItem(ACCESS_TOKEN_KEY, token);
    this.hasTokenSignal.set(true);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  get(): string | null {
    return this.getAccessToken();
  }

  clear(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    this.hasTokenSignal.set(false);
  }

  hasToken(): boolean {
    return !!this.get();
  }
}
