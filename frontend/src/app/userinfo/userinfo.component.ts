import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-userinfo',
  templateUrl: './userinfo.component.html',
  styleUrls: ['./userinfo.component.scss'],
})
export class UserinfoComponent implements OnInit {
  userData$!: Observable<any>;

  token = '';

  idToken = '';

  constructor(public oidcSecurityService: OidcSecurityService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.userData$ = this.oidcSecurityService.userData$;
    this.token = this.oidcSecurityService.getToken();
    this.idToken = this.oidcSecurityService.getIdToken();
  }

  copyToken(token: string, type: string) {
    navigator.clipboard.writeText(token).then(
      () => {
        this.snackBar.open(`${type} token copied to clipboard!`, 'Dismiss', {
          duration: 1000,
        });
      },
      (err) => {
        this.snackBar.open(`Error: ${err}`, 'Dismiss', {
          duration: 1000,
        });
      }
    );
  }
}
