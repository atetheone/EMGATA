import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-admin-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  template: `
    <aside class="bg-base-200 w-64 min-h-screen p-4">
      <div class="space-y-4">
        <h2 class="text-xl font-bold mb-6">Admin Panel</h2>
        
        <ul class="menu bg-base-200 rounded-box">
          <li>
            <a routerLink="/admin/dashboard" routerLinkActive="active">
              Dashboard
            </a>
          </li>
          <li>
            <a routerLink="/admin/cars" routerLinkActive="active">
             Gestion de Voitures
            </a>
          </li>
          <li>
            <a routerLink="/admin/brands" routerLinkActive="active">
              Gestion de marques
            </a>
          </li>
          <li>
            <a routerLink="/admin/models" routerLinkActive="active">
              Gestion de modèles
            </a>
          </li>
        </ul>
      </div>
    </aside>
  `
})
export class AdminSidebarComponent {}