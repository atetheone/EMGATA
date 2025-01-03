import { inject } from '@angular/core';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '#services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  let authService = inject(AuthService);
  
  const token = authService.getToken();

  if (token) {
    const authRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });

    return next(authRequest)
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        // Token might be expired
        authService.logout();
      }
      return throwError(() => error);
    })
  );
};
