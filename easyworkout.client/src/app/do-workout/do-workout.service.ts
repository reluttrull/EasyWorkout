import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FinishWorkoutRequest, CompletedWorkoutResponse } from '../model/interfaces';


@Injectable({ providedIn: 'root' })
export class DoWorkoutService {
  private baseUrl = 'https://localhost:7011/api/completed-workouts';

  constructor(private http: HttpClient, private router: Router) {}

  get(id:string) {
    return this.http.get<CompletedWorkoutResponse>(`${this.baseUrl}/${id}`);
  }

  create(request: FinishWorkoutRequest) {
    console.log(`sending request to ${this.baseUrl}`, request);
    return this.http.post<{ id: string }>(this.baseUrl, request);
  }
}
