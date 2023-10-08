import { Component } from '@angular/core';
// ilgili bileşenlere (Highcharts) erişim
import * as Highcharts from 'highcharts'
// SignalR için import
import * as signalR from '@microsoft/signalr';

interface SeriesData {
  name: string;
  data: number[];
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  url = "https://localhost:7218/satishub";
  connection: signalR.HubConnection;
  chart!: Highcharts.Chart;
  updateFromInput = false;
  chartCallback: (chart: Highcharts.Chart) => void;

  constructor() {
    // SignalR bağlantısı
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this.url)
      .build();

    // SignalR bağlantısını başlat
    this.connection.start();

    // hubdaki reciveMessage fonksiyonunu çağır
    this.connection.on("reciveMessage", data => {
      // Mevcut serileri temizle
      while (this.chart.series.length > 0) {
        this.chart.series[0].remove(true);
      }

      // Gelen verilere göre yeni serileri ekle
      data.forEach((seriesData: SeriesData) => {
        this.chart.addSeries({
          type: 'line',
          name: seriesData.name,
          data: seriesData.data
        }, true, true);
      });
    });


    const self = this;
    this.chartCallback = chart => {
      self.chart = chart;
      
    }

  }

  //*****  Highcharts değişkeni ve config ayarları *****//


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
    /*
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
    */
    series: [], // Yeni verilerle dinamik olarak doldurulacak boş seri dizisi
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
