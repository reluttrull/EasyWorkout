import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Workout, CreateWorkoutRequest, UpdateWorkoutRequest, WorkoutResponse } from '../model/interfaces';


@Injectable({ providedIn: 'root' })
export class WorkoutsService {
  private baseUrl = 'https://localhost:7011/api/workouts';

  constructor(private http: HttpClient, private router: Router) {}

  getAll() {
    return this.http.get<Workout[]>(`${this.baseUrl}/me`);
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
