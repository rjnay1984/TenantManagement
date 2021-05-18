import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthConfigModule } from './auth/auth-config.module';
import { CoreModule } from './core/core.module';
import { HomeComponent } from './home/home.component';
import { MaterialModule } from './material.module';

@NgModule({
  declarations: [AppComponent, HomeComponent],
  imports: [BrowserModule, AppRoutingModule, MaterialModule, AuthConfigModule, CoreModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
