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

  constructor(public oidcSecurityService: OidcSecurityService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.userData$ = this.oidcSecurityService.userData$;
    this.token = this.oidcSecurityService.getToken();
  }

  copyToken() {
    navigator.clipboard.writeText(this.token).then(
      () => {
        this.snackBar.open('Token copied to clipboard!', 'Dismiss', {
          duration: 3000,
        });
      },
      (err) => {
        this.snackBar.open(`Error: ${err}`, 'Dismiss', {
          duration: 3000,
        });
      }
    );
  }
}
