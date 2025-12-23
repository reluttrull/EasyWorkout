import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, take, switchMap } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';
import { TokenService } from './token.service';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenService = inject(TokenService);
  const authService = inject(AuthService);

  const token = tokenService.getAccessToken();
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

return next(req).pipe(
  catchError((error: HttpErrorResponse) => {
    if (error.status === 401) {
      const refreshToken = tokenService.getRefreshToken();

      if (!refreshToken) {
        return throwError(() => error);
      }

      return handle401Error(req, next, authService, tokenService, error);
    }

    return throwError(() => error);
  })
);


  function handle401Error(
    request: any,
    next: any,
    authService: AuthService,
    tokenService: TokenService,
    originalError: HttpErrorResponse
  ): Observable<any> {

    if (!isRefreshing) {
      isRefreshing = true;
      refreshTokenSubject.next(null);

      return authService.refreshToken().pipe(
        switchMap(res => {
          isRefreshing = false;
          refreshTokenSubject.next(res.accessToken);

          return next(request.clone({
            setHeaders: {
              Authorization: `Bearer ${res.accessToken}`
            }
          }));
        }),
        catchError(() => {
          isRefreshing = false;
          authService.logout();
          return throwError(() => originalError);
        })
      );
    }

    return refreshTokenSubject.pipe(
      filter(token => token != null),
      take(1),
      switchMap(token =>
        next(request.clone({
          setHeaders: { Authorization: `Bearer ${token}` }
        }))
      )
    );
  }
}