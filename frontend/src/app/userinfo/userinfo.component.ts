import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-userinfo',
  templateUrl: './userinfo.component.html',
  styleUrls: ['./userinfo.component.scss'],
})
export class UserinfoComponent implements OnInit {
  userData$!: Observable<any>;

  constructor(public oidcSecurityService: OidcSecurityService) {}

  ngOnInit(): void {
    this.userData$ = this.oidcSecurityService.userData$;
  }
}
