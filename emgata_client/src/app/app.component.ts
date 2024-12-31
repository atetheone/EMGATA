import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  template: `
    <div class="custom-container">
      <div class="hero min-h-screen bg-base-200">
        <div class="hero-content text-center">
          <div class="max-w-md">
            <h1 class="text-5xl font-bold">Hello EMGATA</h1>
            <p class="py-6">Testing Tailwind + SCSS setup</p>
            <button class="btn btn-primary">Get Started</button>
          </div>
        </div>
      </div>
    </div>
    <router-outlet/>
  `,
  styleUrl: './app.component.sass'
})
export class AppComponent {
  title = 'emgata_client';
}
