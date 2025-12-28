import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CreateWorkoutRequest, UpdateWorkoutRequest, WorkoutResponse } from '../model/interfaces';


@Injectable({ providedIn: 'root' })
export class WorkoutsService {
  private baseUrl = 'https://localhost:7011/api/workouts';
  http = inject(HttpClient);
  router = inject(Router);

  getAll() {
    return this.http.get<WorkoutResponse[]>(`${this.baseUrl}/me`);
  }

  get(id:string) {
    return this.http.get<WorkoutResponse>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateWorkoutRequest) {
    return this.http.post<{ id: string }>(this.baseUrl, request);
  }

  update(id:string, request: UpdateWorkoutRequest) {
    return this.http.put<WorkoutResponse>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: string) {
    return this.http.delete<any>(`${this.baseUrl}/${id}`);
  }

  addExercise(id:string, exerciseId:string) {
    return this.http.post<any>(`${this.baseUrl}/${id}/exercises/${exerciseId}`, {});
  }

  removeExercise(id:string, exerciseId:string) {
    return this.http.delete<any>(`${this.baseUrl}/${id}/exercises/${exerciseId}`);
  }
}
