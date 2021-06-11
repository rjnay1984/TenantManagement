import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthConfigModule } from './auth/auth-config.module';
import { CoreModule } from './core/core.module';
import { HomeComponent } from './home/home.component';
import { MaterialModule } from './material.module';
import { WeatherComponent } from './weather/weather.component';
import { UserinfoComponent } from './userinfo/userinfo.component';

@NgModule({
  declarations: [AppComponent, HomeComponent, WeatherComponent, UserinfoComponent],
  imports: [BrowserModule, AppRoutingModule, MaterialModule, AuthConfigModule, CoreModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
