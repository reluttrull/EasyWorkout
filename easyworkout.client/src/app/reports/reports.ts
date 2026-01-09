import { Component, inject, signal } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartOptions, ChartData, ChartType } from 'chart.js';
import { ReportsService } from './reports.service';
import { WeightUnit } from '../model/enums';
import { TotalVolumeReportRequest, TotalVolumeReportResponse, DataPointResponse } from '../model/interfaces';

@Component({
  selector: 'app-reports',
  imports: [BaseChartDirective],
  templateUrl: './reports.html',
  styleUrl: './reports.css',
})
export class Reports {
  weightUnit = signal(WeightUnit.Pounds.toString());
  reportsService = inject(ReportsService);
  isChartLoaded = signal(false);
  
  public lineChartOptions: ChartOptions = {
    responsive: true,
    scales: {
      y: {
        title: {
          display: true,
          text: this.weightUnit()
        }
      }
    }
  };
  public lineChartType: ChartType = 'line';
  public lineChartData: ChartData<'line'> = {
    labels: [] as string[],
    datasets: [
      { data: [] }
    ]
  };
  public lineChartLegend = false;

  fetchTotalVolume() {
    // default to pounds for now
    const request: TotalVolumeReportRequest = {
      fromDate: null,
      toDate: null,
      workoutId: null,
      weightUnit: WeightUnit.Pounds
    };
    this.reportsService.getTotalVolumeReport(request).subscribe({
        next: (response) => {
          console.log(response);
          this.isChartLoaded.set(true);
          this.weightUnit.set(response.weightUnit);
          this.displayData(response);
        },
        error: err => console.error(err)
      });
  }

  displayData(response:{dataPoints: DataPointResponse[]}) {
    let newDataArr:number[] = [];
    let newLabelsArr:string[] = [];
    response.dataPoints.forEach(d => {
    // 1. Add the new label
    newLabelsArr = [...newLabelsArr, new Date(d.completedDate).toISOString().split('T')[0]];

    // 2. Add the new data point to the first dataset
    newDataArr = [...newDataArr, d.totalVolume];
      });

    // 3. Clone the entire datasets array and reassign to trigger change detection
    this.lineChartData = {
      labels: newLabelsArr,
      datasets: [
      { data: newDataArr }
      ]
    };
  }
}
