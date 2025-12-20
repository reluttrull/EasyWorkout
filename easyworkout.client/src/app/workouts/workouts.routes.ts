import { Routes } from '@angular/router';
import { WorkoutsComponent } from './workouts';
import { authGuard } from '../auth/auth.guard';

export const WORKOUTS_ROUTES: Routes = [
  {
    path: '',
    component: WorkoutsComponent,
    canActivate: [authGuard]
  }
];