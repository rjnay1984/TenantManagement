import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';

import { WeatherService } from '../core/weather.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  title = 'Tenant Management';

  weather: any = [];

  constructor(public oidcSecurityService: OidcSecurityService, private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.oidcSecurityService.checkAuth();
    this.getWeather();
  }

  login() {
    this.oidcSecurityService.authorize();
  }

  logout() {
    this.oidcSecurityService.logoff();
  }

  getWeather() {
    this.weatherService.getWeather().subscribe((data) => {
      this.weather = data;
    });
  }
}
