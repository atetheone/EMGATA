import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CarService } from '#services/car.service';
import { Car } from '#models/car.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <!-- Hero Section -->
    <div class="hero min-h-[60vh] bg-base-200">
      <div class="hero-content text-center">
        <div class="max-w-md">
          <h1 class="text-5xl font-bold">EMG Voitures</h1>
          <p class="py-6">
            Découvrez notre sélection de véhicules d'occasion de qualité. 
            Nous achetons, réparons et revendons des voitures pour votre satisfaction.
          </p>
          <button class="btn btn-primary" routerLink="/cars">Voir nos véhicules</button>
        </div>
      </div>
    </div>

    <!-- Services Section -->
    <section class="py-12 bg-base-100">
      <div class="container mx-auto px-4">
        <h2 class="text-3xl font-bold text-center mb-8">Nos Services</h2>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
          <div class="card bg-base-200">
            <div class="card-body items-center text-center">
              <h3 class="card-title">Achat de Véhicules</h3>
              <p>Nous achetons vos véhicules à des prix compétitifs</p>
            </div>
          </div>
          
          <div class="card bg-base-200">
            <div class="card-body items-center text-center">
              <h3 class="card-title">Réparation</h3>
              <p>Service de réparation professionnel pour tous types de véhicules</p>
            </div>
          </div>
          
          <div class="card bg-base-200">
            <div class="card-body items-center text-center">
              <h3 class="card-title">Vente</h3>
              <p>Large sélection de véhicules d'occasion de qualité</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Featured Cars Section -->
    <section class="py-12 bg-base-200">
      <div class="container mx-auto px-4">
        <h2 class="text-3xl font-bold text-center mb-8">Véhicules en Vedette</h2>
        
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          @for (car of featuredCars; track car.id) {
            <div class="card bg-base-100 shadow-xl">
              <figure>
                <img [src]="getMainImage(car)" [alt]="car.model.name" 
                    class="w-full h-48 object-cover"/>
              </figure>
              <div class="card-body">
                <h2 class="card-title">{{car.model.brand.name}} {{car.model.name}}</h2>
                <div class="flex justify-between items-center">
                  <div>
                    <p>Année: {{car.year}}</p>
                    <p class="text-xl font-bold">{{ car.price }} FCFA</p>
                  </div>
                  <button class="btn btn-primary" [routerLink]="['/cars', car.id]">
                    Détails
                  </button>
                </div>
              </div>
            </div>
          }
        </div>

        <div class="text-center mt-8">
          <button class="btn btn-outline btn-primary" routerLink="/cars">
            Voir tous nos véhicules
          </button>
        </div>
      </div>
    </section>

    <!-- Why Choose Us Section -->
    <section class="py-12 bg-base-100">
      <div class="container mx-auto px-4">
        <h2 class="text-3xl font-bold text-center mb-8">Pourquoi Nous Choisir ?</h2>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          <div class="card bg-base-200">
            <div class="card-body items-center text-center">
              <h3 class="card-title">Expertise</h3>
              <p>Plus de 10 ans d'expérience dans le domaine automobile</p>
            </div>
          </div>
          
          <div class="card bg-base-200">
            <div class="card-body items-center text-center">
              <h3 class="card-title">Qualité</h3>
              <p>Véhicules rigoureusement sélectionnés et contrôlés</p>
            </div>
          </div>
          
          <div class="card bg-base-200">
            <div class="card-body items-center text-center">
              <h3 class="card-title">Prix Compétitifs</h3>
              <p>Les meilleurs prix du marché pour nos services</p>
            </div>
          </div>
          
          <div class="card bg-base-200">
            <div class="card-body items-center text-center">
              <h3 class="card-title">Service Client</h3>
              <p>Une équipe à votre écoute pour vous accompagner</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Contact CTA -->
    <section class="py-12 bg-base-200">
      <div class="text-center">
        <h2 class="text-3xl font-bold mb-4">Une Question ?</h2>
        <p class="mb-6">Notre équipe est là pour vous accompagner dans votre projet</p>
        <button class="btn btn-primary" routerLink="/contact">Nous Contacter</button>
      </div>
    </section>
  `
})
export class HomeComponent implements OnInit {
  featuredCars: Car[] = [];

  constructor(private carService: CarService) { }

  ngOnInit() {
    this.carService.getAvailableCars()
      .subscribe(cars => {
        // Prenez les 3 premières voitures comme véhicules en vedette
        this.featuredCars = cars.slice(0, 3);
      });
  }

  getMainImage(car: Car): string {
    const mainImage = car.images.find(img => img.isMain);
    return mainImage ? mainImage.imageUrl : 'assets/placeholder-car.jpg';
  }
}