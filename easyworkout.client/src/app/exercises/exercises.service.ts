import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Exercise, CreateExerciseRequest, UpdateExerciseRequest, CreateSetRequest, ExerciseResponse } from '../model/interfaces';


@Injectable({ providedIn: 'root' })
export class ExercisesService {
  private baseUrl = 'https://localhost:7011/api/exercises';
  http = inject(HttpClient);
  router = inject(Router);

  getAll() {
    return this.http.get<Exercise[]>(`${this.baseUrl}/me`);
  }
  
  create(request: CreateExerciseRequest) {
    return this.http.post<{ id: string }>(this.baseUrl, request);
  }
  
  createSet(id:string, request: CreateSetRequest) {
    return this.http.post<any>(`${this.baseUrl}/${id}/sets`, request);
  }
  
  deleteSet(id:string, setId:string) {
    return this.http.delete<any>(`${this.baseUrl}/${id}/sets/${setId}`);
  }

  update(id:string, request: UpdateExerciseRequest) {
    return this.http.put<ExerciseResponse>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: string) {
    return this.http.delete<any>(`${this.baseUrl}/${id}`);
  }
}
