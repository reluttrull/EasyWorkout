import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Exercise, CreateExerciseRequest, UpdateExerciseRequest, ExerciseResponse } from '../model/interfaces';


@Injectable({ providedIn: 'root' })
export class ExercisesService {
  private baseUrl = 'https://localhost:7011/api/exercises';

  constructor(private http: HttpClient, private router: Router) {}

  getAll() {
    return this.http.get<Exercise[]>(`${this.baseUrl}/me`);
  }
  
  create(request: CreateExerciseRequest) {
    return this.http.post<{ id: string }>(this.baseUrl, request);
  }

  update(id:string, request: UpdateExerciseRequest) {
    return this.http.put<ExerciseResponse>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: string) {
    return this.http.delete<any>(`${this.baseUrl}/${id}`);
  }
}
