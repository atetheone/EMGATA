import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';
import { environment } from "#env/environment";
import { LoginRequest, AuthResponse } from "#models/auth.model";
import { RegisterRequest } from "../models/auth.model";
import { Router } from "@angular/router";


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;
  private tokenKey = 'token';
  
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  private isAdminSubject = new BehaviorSubject<boolean>(false);

  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  isAdmin$ = this.isAdminSubject.asObservable();

  private http = inject(HttpClient);
  private router = inject(Router);

  constructor() {
    const token = this.getToken();
    if (token) {
      this.isAuthenticatedSubject.next(true);
      this.checkAdminStatus(token);
    }  
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(
      tap(response => {
        if (response.isSuccess && response.token) {
          localStorage.setItem(this.tokenKey, response.token);
          this.isAuthenticatedSubject.next(true);
          this.checkAdminStatus(response.token);
        }
      }),
      catchError(error => {
        console.error('Login error:', error);
        return throwError(() => error.error?.message || 'Login failed');
      })
    );
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, request).pipe(
      catchError(error => {
        console.error('Registration error:', error);
        return throwError(() => error.error?.message || 'Registration failed');
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    if (typeof localStorage !== 'undefined') {
      return localStorage.getItem(this.tokenKey);
    }
    return null;  
  }

  private checkAdminStatus(token: string): void {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const roles = Array.isArray(payload.role) ? payload.role : [payload.role];
      this.isAdminSubject.next(roles.includes('Admin'));
    } catch {
      this.isAdminSubject.next(false);
    }
  }
}