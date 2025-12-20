import { Injectable } from '@angular/core';

const TOKEN_KEY = 'jwt';

@Injectable({ providedIn: 'root' })
export class TokenService {

  set(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
  }

  get(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  clear(): void {
    localStorage.removeItem(TOKEN_KEY);
  }

  hasToken(): boolean {
    return !!this.get();
  }
}
