import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { FinishWorkoutRequest, UpdateCompletedWorkoutRequest, CompletedWorkoutResponse } from '../model/interfaces';


@Injectable({ providedIn: 'root' })
export class CompletedWorkoutsService {
  private baseUrl = `${environment.workoutsApi}/api/completed-workouts`;
  http = inject(HttpClient);
  router = inject(Router);

  get(id:string) {
    return this.http.get<CompletedWorkoutResponse>(`${this.baseUrl}/${id}`);
  }

  getAll() {
    return this.http.get<CompletedWorkoutResponse[]>(`${this.baseUrl}/me`);
  }
  
  getLastCompletedDate() {
    return this.http.get<Date|null>(`${this.baseUrl}/me/latest`);
  }

  create(request: FinishWorkoutRequest) {
    return this.http.post<{ id: string }>(this.baseUrl, request);
  }
  
  update(id:string, request: UpdateCompletedWorkoutRequest) {
    return this.http.put<CompletedWorkoutResponse>(`${this.baseUrl}/${id}`, request);
  }
  
  delete(id: string) {
    return this.http.delete<any>(`${this.baseUrl}/${id}`);
  }

  deleteAll() {
    return this.http.delete(`${this.baseUrl}`);
  }
}
