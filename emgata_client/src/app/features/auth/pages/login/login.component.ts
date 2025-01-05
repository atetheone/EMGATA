import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '#services/auth.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterModule],
  template: `
    <div class="min-h-screen flex items-center justify-center">
      <div class="card w-96 bg-base-100 shadow-xl">
        <div class="card-body">
          <h2 class="card-title justify-center mb-4">Connexion</h2>
          
          <form (ngSubmit)="onSubmit()" #loginForm="ngForm">
            <div class="form-control">
              <label class="label">
                <span class="label-text">Email</span>
              </label>
              <input 
                type="email" 
                class="input input-bordered" 
                name="email"
                [(ngModel)]="email"
                required
                email
              />
            </div>
            
            <div class="form-control">
              <label class="label">
                <span class="label-text">Mot de passe</span>
              </label>
              <input 
                type="password" 
                class="input input-bordered" 
                name="password"
                [(ngModel)]="password"
                required
                minlength="6"
              />
            </div>
            
            <div class="text-error mt-2" *ngIf="error">
              {{error}}
            </div>
            
            <div class="form-control mt-6">
              <button 
                class="btn btn-primary" 
                type="submit"
                [disabled]="!loginForm.form.valid || isLoading"
              >
                <span *ngIf="isLoading" class="loading loading-spinner"></span>
                Se connecter
              </button>
            </div>

            <div class="text-center mt-4">
              <p>Pas encore de compte ? 
                <a routerLink="/auth/register" class="link link-primary">S'inscrire</a>
              </p>
            </div>
          </form>
        </div>
      </div>
    </div>
  `
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit() {
    if (!this.email || !this.password) return;

    this.isLoading = true;
    this.error = '';

    this.authService.login({
      email: this.email,
      password: this.password
    }).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response.isSuccess) {
          this.router.navigate(['/']);
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.error = error.message || 'Une erreur est survenue';
      }
    });
  }
}