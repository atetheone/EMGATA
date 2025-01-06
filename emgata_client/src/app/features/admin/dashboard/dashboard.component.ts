import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CarService } from '#services/car.service';
import { CarStatus } from '#models/car.model';
import { ConfirmDialogComponent } from '#shared/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterModule],
  template: `
    <div class="p-4">
      <h1 class="text-2xl font-bold mb-6">Tableau de Bord</h1>

      <!-- Stats Cards -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
        <div class="stats shadow">
          <div class="stat">
            <div class="stat-title">Total Véhicules</div>
            <div class="stat-value">{{statistics.totalCars}}</div>
            <div class="stat-desc">En inventaire</div>
          </div>
        </div>

        <div class="stats shadow">
          <div class="stat">
            <div class="stat-title">Véhicules Disponibles</div>
            <div class="stat-value">{{statistics.availableCars}}</div>
            <div class="stat-desc">Prêts à la vente</div>
          </div>
        </div>

        <div class="stats shadow">
          <div class="stat">
            <div class="stat-title">Véhicules Vendus</div>
            <div class="stat-value">{{statistics.soldCars}}</div>
            <div class="stat-desc">Ce mois</div>
          </div>
        </div>

        <div class="stats shadow">
          <div class="stat min-w-[300px]"> <!-- Added minimum width -->
            <div class="stat-title">Valeur Inventaire</div>
            <div class="stat-value text-2xl lg:text-3xl break-words"> <!-- Responsive text size and word breaking -->
              {{statistics.totalValue | number:'1.0-0'}} FCFA
            </div>
            <div class="stat-desc">Total des véhicules disponibles</div>
          </div>
        </div>
      </div>

      <!-- Quick Actions -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
        <button class="btn btn-primary" routerLink="/admin/cars">
          Gérer les Véhicules
        </button>
        <button class="btn btn-secondary" routerLink="/admin/brands">
          Gérer les Marques
        </button>
        <button class="btn btn-accent" routerLink="/admin/models">
          Gérer les Modèles
        </button>
        <button class="btn btn-ghost border-2" routerLink="/admin/cars/add">
          Ajouter un Véhicule
        </button>
      </div>

      <!-- Recent Cars -->
      <div class="bg-base-100 p-4 rounded-lg shadow">
        <h2 class="text-xl font-bold mb-4">Véhicules Récents</h2>
        <div class="overflow-x-auto">
          <table class="table">
            <thead>
              <tr>
                <th>Marque</th>
                <th>Modèle</th>
                <th>Année</th>
                <th>Prix</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              @for (car of recentCars; track car.id) {
                <tr>
                  <td>{{car.model.brand.name}}</td>
                  <td>{{car.model.name}}</td>
                  <td>{{car.year}}</td>
                  <td>{{car.price}} FCFA</td>
                  <td>
                    <span class="badge" 
                        [ngClass]="{
                          'badge-success': car.status === CarStatus.Available,
                          'badge-warning': car.status === CarStatus.Sold,
                          'badge-error': car.status === CarStatus.NotAvailable
                        }">
                      {{getStatusLabel(car.status)}}
                    </span>
                  </td>
                  <td class="space-x-2">
                    <button 
                      class="btn btn-ghost btn-sm" 
                      [routerLink]="['/admin/cars', car.id, 'edit']"
                      title="Éditer"
                    >
                      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                      </svg>
                    </button>
                    <button 
                      class="btn btn-ghost btn-sm text-error" 
                      (click)="deleteCar(car.id)"
                      title="Supprimer"
                    >
                      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                      </svg>
                    </button>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `
})
export class DashboardComponent implements OnInit {
  statistics = {
    totalCars: 0,
    availableCars: 0,
    soldCars: 0,
    totalValue: 0
  };

  CarStatus = CarStatus;

  recentCars: any[] = [];

  constructor(
    private carService: CarService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.loadStatistics();
    this.loadRecentCars();
  }

  loadStatistics() {
    this.carService.getAllCars().subscribe(cars => {
      this.statistics.totalCars = cars.length;
      this.statistics.availableCars = cars.filter(car => car.status === CarStatus.Available).length;
      this.statistics.soldCars = cars.filter(car => car.status === CarStatus.Sold).length;
      this.statistics.totalValue = cars
        .filter(car => car.status === CarStatus.Available)
        .reduce((sum, car) => sum + car.price, 0);
    });
  }

  loadRecentCars() {
    this.carService.getAllCars().subscribe(cars => {
      this.recentCars = cars.slice(0, 5); // Get last 5 cars
    });
  }

  getStatusLabel(status: number): string {
    switch (status) {
      case 1:
        return 'Disponible';
      case 2:
        return 'Vendu';
      case 3:
        return 'Non disponible';
      default:
        return 'Inconnu';
    }
  }

  deleteCar(carId: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { message: 'Êtes-vous sûr de vouloir supprimer ce véhicule ?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.carService.deleteCar(carId).subscribe(() => {
          this.loadDashboardData();
        });
      }
    });
  }

  loadDashboardData() {
    this.loadStatistics();
    this.loadRecentCars();  
  }
}