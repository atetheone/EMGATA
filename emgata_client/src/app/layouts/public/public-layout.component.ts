import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './components/navbar.component'
import { FooterComponent } from './components/footer.component';

@Component({
  selector: 'app-public-layout',
  imports: [CommonModule, RouterOutlet, NavbarComponent, FooterComponent],
  template: `
    <div class="min-h-screen flex flex-col">
      <app-navbar />
      
      <main class="flex-grow">
        <router-outlet />
      </main>

      <app-footer />
    </div>
  `
})
export class PublicLayoutComponent {}