import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AdminSidebarComponent } from './components/siderbar.component';
import { AdminHeaderComponent } from './components/header.component';

@Component({
  selector: 'app-admin-layout',
  imports: [CommonModule, RouterOutlet, AdminSidebarComponent, AdminHeaderComponent],
  template: `
    <div class="min-h-screen bg-gray-100">
      <app-admin-header />
      
      <div class="flex">
        <app-admin-sidebar />
        
        <main class="flex-1 p-4">
          <router-outlet />
        </main>
      </div>
    </div>
  `
})
export class AdminLayoutComponent {}