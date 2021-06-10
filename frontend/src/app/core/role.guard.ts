import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';

export interface UserInfo {
  role: string;
}

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  token = this.oidcSecurityService.getToken();

  constructor(private oidcSecurityService: OidcSecurityService, private router: Router) {}

  canActivate(): boolean {
    const userInfo: UserInfo = JSON.parse(window.atob(this.token.split('.')[1]));
    if (!userInfo || !userInfo.role || (userInfo.role !== 'Admin' && userInfo.role !== 'Landlord')) {
      this.router.navigateByUrl('/unauthorized');
      return false;
    }

    return true;
  }
}
