import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '#shared/confirm-dialog/confirm-dialog.component';
import { CarService } from '#services/car.service';
import { Car, CarStatus } from '#models/car.model';

@Component({
  selector: 'app-cars-management',
  imports: [CommonModule, RouterLink],
  template: `
    <div class="p-4">
      <div class="flex justify-between items-center mb-6">
        <h1 class="text-2xl font-bold">Gestion des Véhicules</h1>
        <button class="btn btn-primary" routerLink="add">
          Ajouter un Véhicule
        </button>
      </div>

      <div class="overflow-x-auto bg-base-100 rounded-lg shadow">
        <table class="table table-zebra">
          <thead>
            <tr>
              <th>ID</th>
              <th>Marque</th>
              <th>Modèle</th>
              <th>Année</th>
              <th>Prix</th>
              <th>Statut</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let car of cars">
              <td>{{car.id}}</td>
              <td>{{car.model.brand.name}}</td>
              <td>{{car.model.name}}</td>
              <td>{{car.year}}</td>
              <td>{{car.price}} FCFA</td>
              <td>
                <span 
                  class="badge"
                  [ngClass]="{
                    'badge-success': car.status === CarStatus.Available,
                    'badge-error': car.status === CarStatus.Sold,
                    'badge-warning': car.status === CarStatus.NotAvailable
                  }"
                >
                  {{ getStatusLabel(car.status) }}
                </span>
              </td>
              <td class="space-x-2">
                <button class="btn btn-sm" [routerLink]="[car.id, 'edit']">
                  Éditer
                </button>
                <button class="btn btn-error btn-sm" (click)="deleteCar(car.id)">
                  Supprimer
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  `
})
export class CarsManagementComponent implements OnInit {
  cars: Car[] = [];
  CarStatus = CarStatus;

  constructor(private carService: CarService, private dialog: MatDialog) {}

  ngOnInit() {
    this.loadCars();
  }

  loadCars() {
    this.carService.getAllCars().subscribe(cars => {
      this.cars = cars;
    });
  }


  deleteCar(carId: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { message: 'Êtes-vous sûr de vouloir supprimer ce véhicule ?' }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.carService.deleteCar(carId).subscribe(() => {
          this.loadCars();
        });
      }
    });
  }

  getStatusLabel(status: CarStatus): string {
    switch (status) {
      case CarStatus.Available:
        return 'Disponible';
      case CarStatus.Sold:
        return 'Vendu';
      case CarStatus.NotAvailable:
        return 'Non Disponible';
      default:
        return 'Inconnu';
    }
  }
}