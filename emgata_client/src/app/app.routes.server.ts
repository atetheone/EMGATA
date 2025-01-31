import { RenderMode, ServerRoute } from '@angular/ssr';
import { CarService } from './core/services/car.service';
import { inject } from '@angular/core';
import { environment } from '#env/environment';

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
    renderMode: RenderMode.Client,
    // getPrerenderParams: async () => {
    //   try {
    //     // Récupérer tous les IDs de voitures via votre API
    //     const response = await fetch(`${environment.apiUrl}/cars`);
    //     const cars = await response.json();
        
    //     // Retourner un tableau d'objets de paramètres pour chaque voiture
    //     return cars.map(car => ({
    //       id: car.id.toString()
    //     }));
    //   } catch (error) {
    //     console.error('Error fetching car IDs:', error);
    //     return []; // Retourner un tableau vide en cas d'erreur
    //   }
    // }
  },
  {
    path: 'admin/cars/:id/edit',
    renderMode: RenderMode.Prerender,
    // async getPrerenderParams() {
    //   const carService = inject(CarService);
    //   const cars = await carService.getAllCars().toPromise();
    //   return cars!.map(car => ({ id: car.id.toString() }));
    // },
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