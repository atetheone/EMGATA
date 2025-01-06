import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ModelService } from '#services/model.service';
import { Model } from '#models/model.model';
import { ModelDialogComponent } from './model-dialog.component';
import { ConfirmDialogComponent } from '#shared/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-models-management',
  imports: [CommonModule],
  template: `
    <div class="p-4">
      <div class="flex justify-between items-center mb-6">
        <h1 class="text-2xl font-bold">Gestion des Modèles</h1>
        <button class="btn btn-primary" (click)="openAddDialog()">
          Ajouter un Modèle
        </button>
      </div>

      <div class="overflow-x-auto bg-base-100 rounded-lg shadow">
        <table class="table table-zebra">
          <thead>
            <tr>
              <th>ID</th>
              <th>Marque</th>
              <th>Nom</th>
              <th>Description</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let model of models">
              <td>{{model.id}}</td>
              <td>{{model.brand.name}}</td>
              <td>{{model.name}}</td>
              <td>{{model.description}}</td>
              <td class="space-x-2">
                <button class="btn btn-sm" (click)="openEditDialog(model)">
                  Éditer
                </button>
                <button class="btn btn-error btn-sm" (click)="deleteModel(model.id)">
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
export class ModelsManagementComponent implements OnInit {
  models: Model[] = [];

  constructor(
    private modelService: ModelService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.loadModels();
  }

  loadModels() {
    this.modelService.getAllModels().subscribe(models => {
      this.models = models;
    });
  }

  openAddDialog() {
    const dialogRef = this.dialog.open(ModelDialogComponent, {
      width: '500px',
      data: { model: null }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadModels();
      }
    });
  }

  openEditDialog(model: Model) {
    const dialogRef = this.dialog.open(ModelDialogComponent, {
      width: '500px',
      data: { model }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadModels();
      }
    });
  }

  deleteModel(modelId: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { message: 'Êtes-vous sûr de vouloir supprimer ce modèle ?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.modelService.deleteModel(modelId).subscribe(() => {
          this.loadModels();
        });
      }
    });
  }
}