import { Component, inject } from '@angular/core';
import { ReportsService } from './reports.service';
import { WeightUnit } from '../model/enums';
import { TotalVolumeReportRequest, TotalVolumeReportResponse } from '../model/interfaces';

@Component({
  selector: 'app-reports',
  imports: [],
  templateUrl: './reports.html',
  styleUrl: './reports.css',
})
export class Reports {
  reportsService = inject(ReportsService);

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
        },
        error: err => console.error(err)
      });
  }
}
