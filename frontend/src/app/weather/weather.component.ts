import { Component, OnInit } from '@angular/core';
import { WeatherService } from '../core/weather.service';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.scss'],
})
export class WeatherComponent implements OnInit {
  weather: any[] = [];

  constructor(private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.getWeather();
  }

  getWeather() {
    this.weatherService.getWeather().subscribe((data) => {
      this.weather = data;
    });
  }
}
