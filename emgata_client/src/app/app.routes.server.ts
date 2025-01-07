import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: 'cars/:id',
    renderMode: RenderMode.Dynamic
  },
  {
    path: 'admin/cars/:id/edit',
    renderMode: RenderMode.Dynamic
  },
  {
    path: '**',
    renderMode: RenderMode.Dynamic
  },
  {
    path: 'home',
    renderMode: RenderMode.Prerender
  }
];
