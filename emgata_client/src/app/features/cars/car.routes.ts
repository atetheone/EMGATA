import { Routes } from '@angular/router';

export const CARS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/car-list/car-list.component').then(m => m.CarListComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./pages/car-detail/car-detail.component').then(m => m.CarDetailComponent)
  }
];