import { Routes } from '@angular/router';

export const ADMIN_CARS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./cars-management.component').then(m => m.CarsManagementComponent)
  },
  {
    path: 'add',
    loadComponent: () => import('./car-form.component').then(m => m.CarFormComponent)
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./car-form.component').then(m => m.CarFormComponent)
  }
];