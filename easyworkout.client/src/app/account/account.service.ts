import { Injectable, signal, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserResponse, UpdateUserRequest } from '../model/interfaces';
import { TokenService } from '../core/token.service';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AccountService {
  private baseUrl = 'https://localhost:7011/api/users';
  private readonly _user = signal<UserResponse | null>(null);
  readonly user = this._user.asReadonly();

  constructor(
    private http: HttpClient,
    private tokenService: TokenService
  ) {
    effect(() => {
      if (this.tokenService.hasTokenSignal()) {
        this.loadUser();
      } else {
        this._user.set(null);
      }
    });
  }

  private loadUser() {
    this.http.get<UserResponse>(`${this.baseUrl}/me`)
      .subscribe({
        next: user => this._user.set(user),
        error: () => this._user.set(null)
      });
  }
  
  update(request: UpdateUserRequest): Observable<UserResponse> {
    return this.http
      .put<UserResponse>(`${this.baseUrl}/me`, request)
      .pipe(
        tap(user => this._user.set(user)) // side-effect belongs here
      );
  }

  clear() {
    this._user.set(null);
  }
}