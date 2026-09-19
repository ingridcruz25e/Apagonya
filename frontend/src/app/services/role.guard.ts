import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from './auth.service';
export const roleGuard: CanActivateFn = (route) => {
  const auth=inject(AuthService); const router=inject(Router);
  if(!auth.isLoggedIn()) { router.navigate(['/login']); return false; }
  const allowed=route.data?.['roles'] as string[] | undefined;
  if(!allowed) return true;
  const role=localStorage.getItem('role');
  if(role && allowed.includes(role)) return true;
  router.navigate(['/dashboard']); return false;
};
