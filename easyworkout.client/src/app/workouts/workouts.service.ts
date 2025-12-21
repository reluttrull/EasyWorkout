import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Workout } from '../model/interfaces';


@Injectable({ providedIn: 'root' })
export class WorkoutsService {
  private baseUrl = 'https://localhost:7011/api/workouts';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Workout[]>(`${this.baseUrl}/me`);
  }
}
