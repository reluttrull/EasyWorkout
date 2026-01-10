import { Component, inject, computed, signal } from '@angular/core';
import { ReactiveFormsModule, Validators, FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { BaseChartDirective } from 'ng2-charts';
import { ChartOptions, ChartData, ChartType } from 'chart.js';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { ReportsService } from './reports.service';
import { WeightUnit } from '../model/enums';
import { TotalVolumeReportRequest, DataPointResponse } from '../model/interfaces';

@Component({
  selector: 'app-reports',
  imports: [BaseChartDirective, ReactiveFormsModule, MatButtonModule, MatFormFieldModule, MatSelectModule, MatDatepickerModule],
  templateUrl: './reports.html',
  styleUrl: './reports.css',
})
export class Reports {
  fb = inject(FormBuilder);
  form:FormGroup = this.fb.nonNullable.group({
    chartType: new FormControl(null, Validators.required),
    weightUnit: new FormControl(WeightUnit.Pounds)
  });
  reportsService = inject(ReportsService);
  isChartLoaded = signal(false);
  weightUnitLabel = signal(WeightUnit.Pounds.toString());
  
  public readonly WeightUnit = WeightUnit;
  public weightUnitOptions = Object.values(this.WeightUnit);
  
  public lineChartOptions: ChartOptions = {
    responsive: true,
    scales: {
      y: {
        title: {
          display: true,
          text: this.weightUnitLabel()
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

  submit() {
  this.isChartLoaded.set(false);

  this.weightUnitLabel.set(this.form.value.weightUnit?.toString());

  switch (this.form.value.chartType) {
    case 'totalVolume':
      this.fetchTotalVolume();
      break;
  }
}


  updateLineChartOptions() {
    this.lineChartOptions = {
      ...this.lineChartOptions,
      scales: {
        ...this.lineChartOptions.scales,
        y: {
          ...this.lineChartOptions.scales?.['y'],
          title: {
            display: true,
            text: this.weightUnitLabel()
          }
        }
      }
    };
  }

  fetchTotalVolume() {
    const request: TotalVolumeReportRequest = {
      fromDate: null,
      toDate: null,
      workoutId: null,
      weightUnit: this.form.value.weightUnit
    };
    this.reportsService.getTotalVolumeReport(request).subscribe({
        next: (response) => {
          console.log(response);
          this.isChartLoaded.set(true);
          this.displayData(response);
        },
        error: err => console.error(err)
      });
  }

displayData(response: { dataPoints: DataPointResponse[] }) {
    this.lineChartData = {
      labels: response.dataPoints.map(d =>
        new Date(d.completedDate).toISOString().split('T')[0]
      ),
      datasets: [{ data: response.dataPoints.map(d => d.totalVolume) }]
    };

    this.updateLineChartOptions();

    this.isChartLoaded.set(true);
  }
}
