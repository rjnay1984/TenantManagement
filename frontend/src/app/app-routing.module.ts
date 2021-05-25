import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/auth.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { HomeComponent } from './home/home.component';
import { UnauthorizedComponent } from './core/unauthorized/unauthorized.component';
import { UserinfoComponent } from './userinfo/userinfo.component';
import { WeatherComponent } from './weather/weather.component';
import { RoleGuard } from './core/role.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'weather', component: WeatherComponent, canActivate: [RoleGuard] },
  { path: 'userinfo', component: UserinfoComponent, canActivate: [AuthGuard] },
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: '**', pathMatch: 'full', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
