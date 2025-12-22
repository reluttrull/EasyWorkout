import { Routes, provideRouter, withRouterConfig } from '@angular/router';

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
  { path: '', redirectTo: 'workouts', pathMatch: 'full' }
];

provideRouter(routes, withRouterConfig({ onSameUrlNavigation: 'reload' }));