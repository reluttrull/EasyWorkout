import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { FinishWorkoutRequest, UpdateCompletedWorkoutRequest, GetAllCompletedWorkoutsRequest, CompletedWorkoutResponse } from '../model/interfaces';


@Injectable({ providedIn: 'root' })
export class CompletedWorkoutsService {
  private baseUrl = `${environment.workoutsApi}/api/completed-workouts`;
  http = inject(HttpClient);
  router = inject(Router);

  get(id:string) {
    return this.http.get<CompletedWorkoutResponse>(`${this.baseUrl}/${id}`);
  }

  getAll(getAllFilters:GetAllCompletedWorkoutsRequest) {
    let queries = [];
    if (getAllFilters.minDate) queries.push(`minDate=${getAllFilters.minDate.toISOString()}`);
    if (getAllFilters.maxDate) queries.push(`maxDate=${getAllFilters.maxDate.toISOString()}`);
    if (getAllFilters.basedOnWorkoutId) queries.push(`basedOnWorkoutId=${getAllFilters.basedOnWorkoutId}`);
    if (getAllFilters.containsExerciseId) queries.push(`containsExerciseId=${getAllFilters.containsExerciseId}`);
    if (getAllFilters.containsText) queries.push(`containsText=${getAllFilters.containsText}`);

    let queryString = queries.join('&');
    return this.http.get<CompletedWorkoutResponse[]>(`${this.baseUrl}/me${queryString.length > 0 ? '?' + queryString : ''}`);
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
