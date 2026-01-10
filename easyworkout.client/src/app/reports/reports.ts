import { Component, OnInit, inject, signal } from '@angular/core';
import { ReactiveFormsModule, Validators, FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { BaseChartDirective } from 'ng2-charts';
import { ChartOptions, ChartData, ChartType } from 'chart.js';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { ReportsService } from './reports.service';
import { WorkoutsService } from '../workouts/workouts.service';
import { WeightUnit, DurationUnit, DistanceUnit } from '../model/enums';
import { WorkoutResponse, TotalVolumeReportRequest, TotalTimeReportRequest, DataPointResponse } from '../model/interfaces';

@Component({
  selector: 'app-reports',
  providers: [provideNativeDateAdapter()],
  imports: [BaseChartDirective, ReactiveFormsModule, MatInputModule, MatButtonModule, 
    MatFormFieldModule, MatSelectModule, MatDatepickerModule],
  templateUrl: './reports.html',
  styleUrl: './reports.css',
})
export class Reports implements OnInit {
  fb = inject(FormBuilder);
  form:FormGroup = this.fb.nonNullable.group({
    chartType: new FormControl(null, Validators.required),
    fromDate: new FormControl(null),
    toDate: new FormControl(null),
    workoutId: new FormControl(null),
    weightUnit: new FormControl(WeightUnit.Pounds),
    durationUnit: new FormControl(DurationUnit.Minutes)
  });
  reportsService = inject(ReportsService);
  workoutsService = inject(WorkoutsService);
  isChartLoaded = signal(false);
  unitLabel = signal(WeightUnit.Pounds.toString());
  workoutTemplates:WorkoutResponse[] = [];

  ngOnInit() {
    this.workoutsService.getAll().subscribe({
        next: (response) => {
          this.workoutTemplates = response;
        },
        error: err => console.error('error loading workouts', err)
      });
  }
  
  public readonly WeightUnit = WeightUnit;
  public readonly DurationUnit = DurationUnit;
  public readonly DistanceUnit = DistanceUnit;
  public weightUnitOptions = Object.values(this.WeightUnit);
  public durationUnitOptions = Object.values(this.DurationUnit);
  public distanceUnitOptions = Object.values(this.DistanceUnit);
  
  public lineChartOptions: ChartOptions = {
    responsive: true,
    scales: {
      y: {
        title: {
          display: true,
          text: this.unitLabel()
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

    switch (this.form.value.chartType) {
      case 'totalVolume':
        this.unitLabel.set(this.form.value.weightUnit?.toString());
        this.fetchTotalVolume();
        break;
      case 'totalTime':
        this.unitLabel.set(this.form.value.durationUnit?.toString());
        this.fetchTotalTime();
        break;
    }
  }

  fetchTotalVolume() {
    const request: TotalVolumeReportRequest = {
      fromDate: this.form.value.fromDate,
      toDate: this.form.value.toDate,
      workoutId: this.form.value.workoutId,
      weightUnit: this.form.value.weightUnit
    };
    this.reportsService.getTotalVolumeReport(request).subscribe({
        next: (response) => {
          this.isChartLoaded.set(true);
          this.displayData(response);
        },
        error: err => console.error('error fetching chart data', err)
      });
  }

  fetchTotalTime() {
    const request: TotalTimeReportRequest = {
      fromDate: this.form.value.fromDate,
      toDate: this.form.value.toDate,
      workoutId: this.form.value.workoutId,
      durationUnit: this.form.value.durationUnit
    };
    this.reportsService.getTotalTimeReport(request).subscribe({
      next: (response) => {
        this.isChartLoaded.set(true);
        this.displayData(response);
      },
      error: err => console.error('error fetching chart data', err)
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
  
  updateLineChartOptions() {
    this.lineChartOptions = {
      ...this.lineChartOptions,
      scales: {
        ...this.lineChartOptions.scales,
        y: {
          ...this.lineChartOptions.scales?.['y'],
          title: {
            display: true,
            text: this.unitLabel()
          }
        }
      }
    };
  }
}
