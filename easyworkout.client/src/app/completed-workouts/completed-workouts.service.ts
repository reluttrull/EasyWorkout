import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FinishWorkoutRequest, CompletedWorkoutResponse } from '../model/interfaces';


@Injectable({ providedIn: 'root' })
export class CompletedWorkoutsService {
  private baseUrl = 'https://localhost:7011/api/completed-workouts';

  constructor(private http: HttpClient, private router: Router) {}

  get(id:string) {
    return this.http.get<CompletedWorkoutResponse>(`${this.baseUrl}/${id}`);
  }

  getAll() {
    return this.http.get<CompletedWorkoutResponse[]>(`${this.baseUrl}/me`);
  }

  create(request: FinishWorkoutRequest) {
    return this.http.post<{ id: string }>(this.baseUrl, request);
  }
}
