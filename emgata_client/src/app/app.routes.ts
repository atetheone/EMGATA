import { Routes } from '@angular/router';
import { PublicLayoutComponent } from './layouts/public/public-layout.component';
import { AdminLayoutComponent } from './layouts/admin/admin-layou.component';
import { adminGuard } from '#guards/admin.guard'
import { authGuard } from '#guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      {
        path: '',
        loadComponent: () => 
          import('./features/home/home.component').then(m => m.HomeComponent)
      },
      {
        path: 'cars',
        loadChildren: () => 
          import('./features/cars/car.routes').then(m => m.CARS_ROUTES)
      },
      {
        path: 'contact',
        loadComponent: () => 
          import('./features/contact/contact.component').then(m => m.ContactComponent)
      },
      {
        path: 'about',
        loadComponent: () => 
          import('./features/about/about.component').then(m => m.AboutComponent)
      },
      {
        path: 'auth',
        loadChildren: () => 
          import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
      }
    ]
  },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [adminGuard, authGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadComponent: () => 
          import('./features/admin/dashboard/dashboard.component')
            .then(m => m.DashboardComponent)
      },
      {
        path: 'cars',
        loadChildren: () => 
          import('./features/admin/cars/admin-car.routes')
            .then(m => m.ADMIN_CARS_ROUTES)
      },
      {
        path: 'brands',
        loadComponent: () => 
          import('./features/admin/brands/brands-management.component')
            .then(m => m.BrandsManagementComponent)
      },
      {
        path: 'models',
        loadComponent: () => 
          import('./features/admin/models/models-management.component')
            .then(m => m.ModelsManagementComponent)
      }
    ]
  }
];