import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { UserResponse } from '../model/interfaces';

@Injectable({ providedIn: 'root' })
export class AccountService {
  private baseUrl = 'https://localhost:7011/api/users';

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

    get() {
      return this.http.get<UserResponse>(`${this.baseUrl}/me`);
    }
}
