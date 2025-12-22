import { Routes } from '@angular/router';
import { ExercisesComponent } from './exercises';
import { authGuard } from '../auth/auth.guard';

export const EXERCISES_ROUTES: Routes = [
  {
    path: '',
    component: ExercisesComponent,
    canActivate: [authGuard]
  }
];