import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { TotalVolumeReportRequest, TotalVolumeReportResponse, TotalTimeReportRequest, TotalTimeReportResponse } from '../model/interfaces';


@Injectable({ providedIn: 'root' })
export class ReportsService {
  private baseUrl = `${environment.workoutsApi}/api/reports`;
  http = inject(HttpClient);

  getTotalVolumeReport(request:TotalVolumeReportRequest) {
    return this.http.post<TotalVolumeReportResponse>(`${this.baseUrl}/totalvolume`, request);
  }

  getTotalTimeReport(request:TotalTimeReportRequest) {
    return this.http.post<TotalTimeReportResponse>(`${this.baseUrl}/totaltime`, request);
  }
}
