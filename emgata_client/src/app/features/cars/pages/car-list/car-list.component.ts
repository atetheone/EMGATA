import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CarService } from '#services/car.service';
import { Car } from '#models/car.model';

@Component({
  selector: 'app-car-list',
  imports: [CommonModule, RouterLink],
  template: `
    <div class="container mx-auto px-4 py-8">
      <h1 class="text-3xl font-bold mb-6">Nos Véhicules</h1>
      
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        @for (car of cars; track car.id) {
          <div class="card bg-base-100 shadow-xl">
            <figure>
              <img [src]="getMainImage(car)" [alt]="car.model.name" class="w-full h-48 object-cover"/>
            </figure>
            <div class="card-body">
              <h2 class="card-title">{{car.model.brand.name}} {{car.model.name}}</h2>
              <p>Année: {{car.year}}</p>
              <p>Prix: {{car.price | currency:'XOF':'symbol':'1.0-0'}}</p>
              <div class="card-actions justify-end">
                <button class="btn btn-primary" [routerLink]="['/cars', car.id]">
                  Voir Détails
                </button>
              </div>
            </div>
          </div>
        }
      
      </div>
    </div>
  `
})
export class CarListComponent implements OnInit {
  cars: Car[] = [];

  constructor(private carService: CarService) {}

  ngOnInit() {
    this.carService.getAvailableCars()
      .subscribe(cars => this.cars = cars);
  }

  getMainImage(car: Car): string {
    const mainImage = car.images.find(img => img.isMain);
    return mainImage ? mainImage.imageUrl : 'assets/placeholder-car.jpg';
  }
}