import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Workout {
  id: number;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class WorkoutsService {
  private baseUrl = 'https://localhost:7011/api/workouts';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Workout[]>(this.baseUrl);
  }
}