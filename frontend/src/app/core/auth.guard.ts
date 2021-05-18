/* eslint-disable import/no-extraneous-dependencies */
/* eslint-disable @typescript-eslint/no-unused-vars */
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private oidcSecurityService: OidcSecurityService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.oidcSecurityService.isAuthenticated$.pipe(
      map((isAuthorized: boolean) => {
        console.log(`AuthorizationGuard, canActivate isAuthorized: ${isAuthorized}`);

        if (!isAuthorized) {
          this.router.navigateByUrl('/unauthorized');
          return false;
        }

        return true;
      })
    );
  }
}
