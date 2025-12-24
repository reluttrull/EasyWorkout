import { Routes, provideRouter, withRouterConfig } from '@angular/router';
import { DoWorkout } from './do-workout/do-workout';
import { Account } from './account/account';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'workouts',
    loadChildren: () =>
      import('./workouts/workouts.routes').then(m => m.WORKOUTS_ROUTES)
  },
  {
    path: 'exercises',
    loadChildren: () =>
      import('./exercises/exercises.routes').then(m => m.EXERCISES_ROUTES)
  },
  {
    path: 'do-workout/:id',
    component: DoWorkout
  },
  {
    path: 'account',
    component: Account
  },
  { path: '', redirectTo: 'workouts', pathMatch: 'full' }
];

provideRouter(routes, withRouterConfig({ onSameUrlNavigation: 'reload' }));