import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private oidcSecurityService: OidcSecurityService, private router: Router) {}

  canActivate(): Observable<boolean> {
    return this.oidcSecurityService.isAuthenticated$.pipe(
      map((isAuthorized: boolean) => {
        if (!isAuthorized) {
          this.router.navigateByUrl('/unauthorized');
          return false;
        }

        return true;
      })
    );
  }
}
