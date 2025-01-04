import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '#services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  template: `
    <nav class="bg-base-100 shadow-lg">
      <div class="navbar container mx-auto">
        <div class="navbar-start">
          <a routerLink="/" class="btn btn-ghost text-xl">EMG Voitures</a>
        </div>
        
        <div class="navbar-center">
          <ul class="menu menu-horizontal px-1">
            <li>
              <a routerLink="/" routerLinkActive="active">Accueil</a>
            </li>
            <li>
              <a routerLink="/cars" routerLinkActive="active">Nos voiture</a>
            </li>
            <li>
              <a routerLink="/about" routerLinkActive="active">A propos</a>
            </li>
            <li>
              <a routerLink="/contact" routerLinkActive="active">Contact</a>
            </li>
          </ul>
        </div>
        
        <div class="navbar-end space-x-2">
          @if(!isLoggedIn) {
            <a routerLink="/auth/login" class="btn btn-primary">Se connecter</a>
          } @else {
            <div class="dropdown dropdown-end">
              <button class="btn btn-ghost">
                <span>Mon compte</span>
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                  <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
                </svg>
              </button>
              <ul tabindex="0" class="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-52">
                @if (isAdmin) {
                  <li>
                    <a routerLink="/admin" class="flex items-center space-x-2">
                      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                        <path d="M10.707 2.293a1 1 0 00-1.414 0l-7 7a1 1 0 001.414 1.414L4 10.414V17a1 1 0 001 1h2a1 1 0 001-1v-2a1 1 0 011-1h2a1 1 0 011 1v2a1 1 0 001 1h2a1 1 0 001-1v-6.586l.293.293a1 1 0 001.414-1.414l-7-7z" />
                      </svg>
                      <span>Dashboard Admin</span>
                    </a>
                  </li>
                }
                
                <li><a (click)="logout()">Se déconnecter</a></li>
              </ul>
            </div>          
          }
        </div>
      </div>
    </nav>
  `
})
export class NavbarComponent {
  isLoggedIn = false;
  isAdmin = false;

  constructor(private authService: AuthService) {
    this.authService.isAuthenticated$.subscribe(
      isAuth => this.isLoggedIn = isAuth
    );
    this.authService.isAdmin$.subscribe(
      isAdmin => this.isAdmin = isAdmin
    );
  }

  logout() {
    this.authService.logout();
  }
}