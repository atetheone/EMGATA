import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '#services/auth.service';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterModule],
  template: `
<div class="min-h-screen flex items-center justify-center">
      <div class="card w-96 bg-base-100 shadow-xl">
        <div class="card-body">
          <h2 class="card-title justify-center mb-4">Inscription</h2>
          
          <form (ngSubmit)="onSubmit()" #registerForm="ngForm">
            <div class="form-control">
              <label class="label">
                <span class="label-text">Nom d'utilisateur</span>
              </label>
              <input 
                type="text" 
                class="input input-bordered" 
                name="username"
                [(ngModel)]="formData.username"
                required
                minlength="3"
                #username="ngModel"
              />
              <label class="label" *ngIf="username.invalid && (username.dirty || username.touched)">
                <span class="label-text-alt text-error">Le nom d'utilisateur doit contenir au moins 3 caractères</span>
              </label>
            </div>

            <div class="form-control">
              <label class="label">
                <span class="label-text">Email</span>
              </label>
              <input 
                type="email" 
                class="input input-bordered" 
                name="email"
                [(ngModel)]="formData.email"
                required
                email
                #email="ngModel"
              />
              <label class="label" *ngIf="email.invalid && (email.dirty || email.touched)">
                <span class="label-text-alt text-error">Veuillez entrer un email valide</span>
              </label>
            </div>
            
            <div class="form-control">
              <label class="label">
                <span class="label-text">Mot de passe</span>
              </label>
              <input 
                type="password" 
                class="input input-bordered" 
                name="password"
                [(ngModel)]="formData.password"
                required
                minlength="6"
                #password="ngModel"
                (ngModelChange)="checkPasswords()"
              />
              <label class="label" *ngIf="password.invalid && (password.dirty || password.touched)">
                <span class="label-text-alt text-error">Le mot de passe doit contenir au moins 6 caractères</span>
              </label>
            </div>

            <div class="form-control">
              <label class="label">
                <span class="label-text">Confirmer le mot de passe</span>
              </label>
              <input 
                type="password" 
                class="input input-bordered" 
                name="confirmPassword"
                [(ngModel)]="confirmPassword"
                required
                #confirmPass="ngModel"
                (ngModelChange)="checkPasswords()"
              />
              <label class="label" *ngIf="passwordMismatch && confirmPass.dirty">
                <span class="label-text-alt text-error">Les mots de passe ne correspondent pas</span>
              </label>
            </div>
            
            <div class="text-error mt-2" *ngIf="error">
              {{error}}
            </div>
            
            <div class="form-control mt-6">
              <button 
                class="btn btn-primary" 
                type="submit"
                [disabled]="!registerForm.form.valid || isLoading || passwordMismatch"
              >
                <span *ngIf="isLoading" class="loading loading-spinner"></span>
                S'inscrire
              </button>
            </div>

            <div class="text-center mt-4">
              <p>Déjà inscrit ? 
                <a routerLink="/auth/login" class="link link-primary">Se connecter</a>
              </p>
            </div>
          </form>
        </div>
      </div>
    </div>
  `
})
export class RegisterComponent {
  formData = {
    username: '',
    email: '',
    password: ''
  };
  
  confirmPassword = '';
  passwordMismatch = false;
  error = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  checkPasswords() {
    this.passwordMismatch = this.formData.password !== this.confirmPassword;
  }

  onSubmit() {
    if (!this.formData.username || !this.formData.email || !this.formData.password) return;
    if (this.passwordMismatch) return;

    this.isLoading = true;
    this.error = '';

    this.authService.register(this.formData).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response.isSuccess) {
          this.router.navigate(['/']);
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.error = error.message || 'Une erreur est survenue lors de l\'inscription';
      }
    });
  }
}