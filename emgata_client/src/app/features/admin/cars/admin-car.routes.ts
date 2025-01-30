import { Routes } from '@angular/router';
import { CarFormComponent } from './car-form.component'

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
    component: CarFormComponent
  }
];