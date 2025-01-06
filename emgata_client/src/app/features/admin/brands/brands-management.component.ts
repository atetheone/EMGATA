
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmDialogComponent } from '#shared/confirm-dialog/confirm-dialog.component';
import { BrandDialogComponent } from './brand-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { BrandService } from '#services/brand.service';
import { Brand } from '#models/brand.model';

@Component({
  selector: 'app-brands-management',
  imports: [CommonModule],
  template: `
    <div class="p-4">
      <div class="flex justify-between items-center mb-6">
        <h1 class="text-2xl font-bold">Gestion des Marques</h1>
        <button class="btn btn-primary" (click)="openAddDialog()">
          Ajouter une Marque
        </button>
      </div>

      <div class="overflow-x-auto bg-base-100 rounded-lg shadow">
        <table class="table table-zebra">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nom</th>
              <th>Description</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let brand of brands">
              <td>{{brand.id}}</td>
              <td>{{brand.name}}</td>
              <td>{{brand.description}}</td>
              <td class="space-x-2">
                <button class="btn btn-sm" (click)="openEditDialog(brand)">
                  Éditer
                </button>
                <button class="btn btn-error btn-sm" (click)="deleteBrand(brand.id)">
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
export class BrandsManagementComponent implements OnInit {
  brands: Brand[] = [];

  constructor(
    private brandService: BrandService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.loadBrands();
  }

  loadBrands() {
    this.brandService.getAllBrands().subscribe({
      next: (brands) => {
        console.log(JSON.stringify(brands));
        this.brands = brands;
      },
    });
  }

  openAddDialog() {
    const dialogRef = this.dialog.open(BrandDialogComponent, {
      width: '500px',
      data: { brand: null }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadBrands();
      }
    });
  }

  openEditDialog(brand: Brand) {
    const dialogRef = this.dialog.open(BrandDialogComponent, {
      width: '500px',
      data: { brand }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadBrands();
      }
    });
  }

  deleteBrand(brandId: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { message: 'Êtes-vous sûr de vouloir supprimer cette marque ?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.brandService.deleteBrand(brandId).subscribe(() => {
          this.loadBrands();
        });
      }
    });
  }
}