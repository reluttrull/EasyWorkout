import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Workout, CreateWorkoutRequest } from '../model/interfaces';


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

  delete(id: string) {
    return this.http.delete<any>(`${this.baseUrl}/${id}`);
  }
}
