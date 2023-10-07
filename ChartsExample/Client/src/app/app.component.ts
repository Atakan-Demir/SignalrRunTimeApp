import { Component } from '@angular/core';
// ilgili bileşenlere erişim
import * as Highcharts from 'highcharts'
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  Highcharts: typeof Highcharts = Highcharts;
  chartsOptions: Highcharts.Options = {
    //başlık
    title: {
      text: 'Başlık'
    },
    // alt başlık
    subtitle: {
      text: 'Alt Başlık'
    },
    // y ekseni
    yAxis: {
      title: {
        text: 'Y Ekseni'
      }
    },
    // x ekseni
    xAxis: {
      accessibility: {
        rangeDescription: '2021 - 2023'
      }
    },
    legend: {
      layout: 'vertical',
      align: 'right',
      verticalAlign: 'middle'
    },
    // Veriler
    series: [
      {
      type: 'line',
      name: 'x',
      data: [1000, 2000, 3000]
    },
    {
      type: 'line',
      name: 'y',
      data: [4000, 3000, 2000]
    },
    {
      type: 'line',
      name: 'z',
      data: [5000, 1000, 3000]
    },
  ],
  //plotOptions
  plotOptions: {
    series: {
      label: {
        connectorAllowed: true
      },
      pointStart: 2021,
      
    }
  },

  }

}
