import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { OidcSecurityService } from 'angular-auth-oidc-client';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { MaterialModule } from './material.module';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { WeatherComponent } from './weather/weather.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, MaterialModule],
      declarations: [AppComponent, HomeComponent, WeatherComponent, UnauthorizedComponent],
      providers: [{ provide: OidcSecurityService }],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
