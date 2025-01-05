import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { CarService } from '#services/car.service';
import { Car, CarImage, CarStatus } from '#models/car.model';

@Component({
  selector: 'app-car-detail',
  imports: [CommonModule],
  template: `
    <div class="container mx-auto px-4 py-8" *ngIf="car">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
        <!-- Image Gallery -->
        <div class="space-y-4">
          <!-- Main Image Display -->
          <div class="w-full aspect-video relative rounded-lg overflow-hidden">
            <img 
            [src]="currentImage.imageUrl" 
            [alt]="car.model.name"
              class="w-full h-full object-cover" 
            />
            
            <!-- Navigation Arrows -->
            <div class="absolute inset-0 flex items-center justify-between p-4">
              <button 
                class="btn btn-circle btn-sm"
                (click)="previousImage()"
                *ngIf="sortedImages.length > 1"
              >❮</button>
              <button 
                class="btn btn-circle btn-sm"
                (click)="nextImage()"
                *ngIf="sortedImages.length > 1"
              >❯</button>
            </div>
          </div>
          
          <!-- Thumbnails -->
          <div class="flex gap-2 overflow-x-auto py-2">
            @for (image of sortedImages; track image.id; let i = $index) {
              <div 
                class="w-20 h-20 flex-shrink-0 cursor-pointer"
                (click)="showImage(i)"
              >
                <img 
                  [src]="image.imageUrl" 
                  [alt]="car.model.name"
                  class="w-full h-full object-cover rounded-lg hover:opacity-75 transition-opacity"
                  [class.ring-2]="currentImageIndex === i"
                  [class.ring-primary]="currentImageIndex === i"
                />
              </div>
            }
          </div>
        </div>
        
        <!-- Car Details -->
        <div class="space-y-4">
          <h1 class="text-3xl font-bold">{{car.model.brand.name}} {{car.model.name}}</h1>
          
          <div class="stats shadow">
            <div class="stat">
              <div class="stat-title">Année</div>
              <div class="stat-value">{{car.year}}</div>
            </div>
            <div class="stat">
              <div class="stat-title">Prix</div>
              <div class="stat-value">{{car.price | number:'1.0-0'}} FCFA</div>
            </div>
          </div>
          
          <div class="card bg-base-100 shadow-xl">
            <div class="card-body">
              <h2 class="card-title">Description</h2>
              <p>{{car.description}}</p>
              <div class="card-actions justify-end">
                @if (car.status === CarStatus.Available) {
                  <button class="btn btn-primary">Contacter le vendeur</button>
                }
              </div>
            </div>
          </div>

          <!-- Additional Details -->
          <div class="card bg-base-100 shadow-xl">
            <div class="card-body">
              <h2 class="card-title">Caractéristiques</h2>
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <span class="font-semibold">Couleur:</span>
                  <span class="ml-2">{{car.color}}</span>
                </div>
                <div>
                  <span class="font-semibold">État:</span>
                  <span class="ml-2">{{getStatusLabel(car.status)}}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class CarDetailComponent implements OnInit {
  car?: Car;
  CarStatus = CarStatus;
  currentImageIndex = 0;

  constructor(
    private route: ActivatedRoute,
    private carService: CarService
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.carService.getCarById(+id)
        .subscribe(car => {
          this.car = car;
          // Set initial image to main image if exists
          const mainImageIndex = this.sortedImages.findIndex(img => img.isMain);
          this.currentImageIndex = mainImageIndex >= 0 ? mainImageIndex : 0;
      });
    }
  }

  showImage(index: number) {
    this.currentImageIndex = index;
  }

  nextImage() {
    if (!this.car?.images) return;
    this.currentImageIndex = (this.currentImageIndex + 1) % this.sortedImages.length;
  }

  previousImage() {
    if (!this.car?.images) return;
    this.currentImageIndex = (this.currentImageIndex - 1 + this.sortedImages.length) % this.sortedImages.length;
  }

  get currentImage(): CarImage {
    return this.sortedImages[this.currentImageIndex];
  }

  get sortedImages(): CarImage[] {
    if (!this.car?.images) return [];
    return [...this.car.images].sort((a, b) => (b.isMain ? 1 : 0) - (a.isMain ? 1 : 0));
  }

  getStatusLabel(status: CarStatus): string {
    switch(status) {
      case CarStatus.Available:
        return 'Disponible';
      case CarStatus.Sold:
        return 'Vendu';
      case CarStatus.NotAvailable:
        return  'Indisponible';
      default:
        return 'Inconnu';
    }
  }
}