import { Routes } from '@angular/router';
import { CarDetailComponent } from './pages/car-detail/car-detail.component'

export const CARS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/car-list/car-list.component').then(m => m.CarListComponent)
  },
  {
    path: ':id',
    component: CarDetailComponent
  }
];