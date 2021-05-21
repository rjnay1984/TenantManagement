import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class WeatherService {
  constructor(private http: HttpClient) {}

  getWeather() {
    return this.http.get('https://localhost:5004/weatherforecast').pipe(map((response: any) => response));
  }
}
