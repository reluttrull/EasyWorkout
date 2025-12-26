import { Routes, provideRouter, withRouterConfig } from '@angular/router';
import { DoWorkout } from './do-workout/do-workout';
import { CompletedWorkoutsComponent } from './completed-workouts/completed-workouts';
import { Account } from './account/account';
import { Home } from './components/home/home';
import { LoginComponent } from './auth/login/login';
import { RegisterComponent } from './auth/register/register';
import { authGuard } from './auth/auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: Home,
    pathMatch: 'full',
    canActivate: [authGuard]
  },
  { path: 'login', component: LoginComponent, pathMatch: 'full' },
  { path: 'register', component: RegisterComponent, pathMatch: 'full' },
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
    component: DoWorkout,
    canActivate: [authGuard]
  },
  {
    path: 'completed-workouts',
    component: CompletedWorkoutsComponent,
    canActivate: [authGuard]
  },
  {
    path: 'account',
    component: Account,
    canActivate: [authGuard]
  },
  { path: '', redirectTo: 'workouts', pathMatch: 'full' }
];

provideRouter(routes, withRouterConfig({ onSameUrlNavigation: 'reload' }));