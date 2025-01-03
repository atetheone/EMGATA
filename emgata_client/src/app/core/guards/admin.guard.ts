import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '#services/auth.service';
import { map } from 'rxjs/operators';

export const adminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);

  return authService.isAdmin$.pipe(
    map(isAdmin => {
      if (!isAdmin) {
        router.navigate(['/auth/login']);
        return false;
      }
      return true;
    })
  );
};