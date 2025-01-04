import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '#services/auth.service';

@Component({
  selector: 'app-admin-header',
  imports: [CommonModule, RouterModule],
  template: `
    <header class="navbar bg-base-100 shadow-lg px-4">
      <!-- Menu Mobile button -->
      <div class="flex-none">
        <button class="btn btn-square btn-ghost lg:hidden">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" class="inline-block w-5 h-5 stroke-current">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"></path>
          </svg>
        </button>
      </div>
      
      <!-- Title -->
      <div class="flex-1">
        <h1 class="text-xl font-bold">Administration EMG Voitures</h1>
      </div>
      
      <!-- Right section -->
      <div class="flex-none gap-2">
        <!-- View Toggle -->
        <a routerLink="/" class="btn btn-ghost">
          Vue Publique
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
          </svg>
        </a>

        <!-- Notifications -->
        <div class="dropdown dropdown-end">
          <div tabindex="0" role="button" class="btn btn-ghost btn-circle">
            <div class="indicator">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
              </svg>
              <span class="badge badge-sm indicator-item">3</span>
            </div>
          </div>
          <div tabindex="0" class="mt-3 z-[1] card card-compact dropdown-content w-52 bg-base-100 shadow">
            <div class="card-body">
              <span class="font-bold text-lg">3 Notifications</span>
              <div class="text-sm">
                <p>Nouvelle voiture ajoutée</p>
                <p>2 ventes en attente</p>
                <p>Mise à jour requise</p>
              </div>
              <div class="card-actions">
                <button class="btn btn-primary btn-sm btn-block">Voir tout</button>
              </div>
            </div>
          </div>
        </div>
        
        <!-- Profile dropdown -->
        <div class="dropdown dropdown-end">
          <div tabindex="0" role="button" class="btn btn-ghost btn-circle avatar">
            <div class="w-10 rounded-full">
              <img alt="Admin Avatar" src="https://daisyui.com/images/stock/photo-1534528741775-53994a69daeb.jpg" />
            </div>
          </div>
          <ul tabindex="0" class="menu menu-sm dropdown-content mt-3 z-[1] p-2 shadow bg-base-100 rounded-box w-52">
            <li>
              <a class="justify-between">
                Profil
                <span class="badge">Admin</span>
              </a>
            </li>
            <li><a>Paramètres</a></li>
            <li><a (click)="logout()">Déconnexion</a></li>
          </ul>
        </div>
      </div>
    </header>
  `
})
export class AdminHeaderComponent {
  private authService = inject(AuthService);

  logout() {
    this.authService.logout();
  }
}