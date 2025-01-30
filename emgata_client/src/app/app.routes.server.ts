import { RenderMode, ServerRoute } from '@angular/ssr';
import { CarService } from './core/services/car.service';
import { inject } from '@angular/core';

export const serverRoutes: ServerRoute[] = [
  {
    path: '',
    renderMode: RenderMode.Server
  },
  {
    path: 'about',
    renderMode: RenderMode.Prerender
  },
  {
    path: 'contact',
    renderMode: RenderMode.Prerender
  },
  {
    path: 'cars',
    renderMode: RenderMode.Prerender
  },
  // Dynamic routes with parameters - use Server rendering
  {
    path: 'cars/:id',
    renderMode: RenderMode.Prerender,
    async getPrerenderParams() {
      const carService = inject(CarService);
      const cars = await carService.getAllCars().toPromise();
      return cars!.map(car => ({ id: car.id.toString() }));
      // This will prerender paths like: /cars/1, /cars/2, etc.
    },
  },
  {
    path: 'admin/cars/:id/edit',
    renderMode: RenderMode.Prerender,
    async getPrerenderParams() {
      const carService = inject(CarService);
      const cars = await carService.getAllCars().toPromise();
      return cars!.map(car => ({ id: car.id.toString() }));
    },
  },
  {
    path: 'admin/**',
    renderMode: RenderMode.Server
  },
  {
    path: '**',
    renderMode: RenderMode.Prerender
  }
];