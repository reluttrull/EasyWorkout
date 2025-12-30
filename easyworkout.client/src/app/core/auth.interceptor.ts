import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { TokenService } from './token.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenService = inject(TokenService);
  const authService = inject(AuthService);

  // never intercept refresh itself
  if (req.url.includes('/auth/refresh')) {
    return next(req);
  }

  // attach access token if present
  const accessToken = tokenService.getAccessToken();
  const authReq = accessToken
    ? req.clone({
        setHeaders: { Authorization: `Bearer ${accessToken}` }
      })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // if not an auth problem, pass it through untouched
      if (error.status != 401) {
        return throwError(() => error);
      }

      // if no refresh token, hard fail
      if (!tokenService.getRefreshToken()) {
        authService.logout();
        return throwError(() => error);
      }

      // attempt refresh once, then retry original request
      return authService.refreshToken().pipe(
        switchMap(res => {
          const retryReq = authReq.clone({
            setHeaders: {
              Authorization: `Bearer ${res.accessToken}`
            }
          });
          return next(retryReq);
        }),
        catchError(refreshErr => {
          authService.logout();
          return throwError(() => refreshErr);
        })
      );
    })
  );
};
