import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
// modülü import ediyoruz
import { HighchartsChartModule } from "highcharts-angular";
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HighchartsChartModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
