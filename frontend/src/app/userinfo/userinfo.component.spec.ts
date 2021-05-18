import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { of } from 'rxjs';

import { UserinfoComponent } from './userinfo.component';

describe('UserinfoComponent', () => {
  let component: UserinfoComponent;
  let fixture: ComponentFixture<UserinfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [UserinfoComponent],
      providers: [{ provide: OidcSecurityService, useValue: of('test') }],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserinfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
