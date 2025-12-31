import { Injectable, signal, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { UserResponse, UpdateUserRequest, ChangeEmailRequest, ChangePasswordRequest } from '../model/interfaces';
import { TokenService } from '../core/token.service';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AccountService {
  private baseUrl = `${environment.authApi}/api/auth`;
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
        tap(user => this._user.set(user)) 
      );
  }
  
  changeEmail(request: ChangeEmailRequest): Observable<UserResponse> {
    return this.http
      .put<UserResponse>(`${this.baseUrl}/email`, request)
      .pipe(
        tap(user => this._user.set(user)) 
      );
  }
  
  changePassword(request: ChangePasswordRequest): Observable<UserResponse> {
    return this.http
      .put<UserResponse>(`${this.baseUrl}/password`, request)
      .pipe(
        tap(user => this._user.set(user)) 
      );
  }

  clear() {
    this._user.set(null);
  }
}