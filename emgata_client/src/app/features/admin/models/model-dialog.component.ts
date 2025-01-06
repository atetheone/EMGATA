import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { Model, CreateModelDto, UpdateModelDto } from '#models/model.model';
import { Brand } from '#models/brand.model';
import { BrandService } from '#services/brand.service';
import { ModelService } from '#services/model.service';

@Component({
  selector: 'app-model-dialog',
  imports: [
    CommonModule, 
    FormsModule, 
    MatDialogModule, 
    MatButtonModule
  ],
  template: `
    <h2 mat-dialog-title>{{data.model ? 'Modifier le modèle' : 'Ajouter un modèle'}}</h2>
    
    <mat-dialog-content>
      <form (ngSubmit)="onSubmit()" #modelForm="ngForm">
        <div class="form-control">
          <label class="label">
            <span class="label-text">Marque</span>
          </label>
          <select 
            class="select select-bordered" 
            name="brandId"
            [(ngModel)]="formData.brandId"
            required
            #brandId="ngModel"
          >
            <option [ngValue]="null">Sélectionner une marque</option>
            <option *ngFor="let brand of brands" [value]="brand.id">
              {{brand.name}}
            </option>
          </select>
          <label class="label" *ngIf="brandId.invalid && (brandId.dirty || brandId.touched)">
            <span class="label-text-alt text-error">La marque est requise</span>
          </label>
        </div>

        <div class="form-control">
          <label class="label">
            <span class="label-text">Nom</span>
          </label>
          <input 
            type="text" 
            class="input input-bordered" 
            name="name"
            [(ngModel)]="formData.name"
            required
            #name="ngModel"
          />
          <label class="label" *ngIf="name.invalid && (name.dirty || name.touched)">
            <span class="label-text-alt text-error">Le nom est requis</span>
          </label>
        </div>

        <div class="form-control">
          <label class="label">
            <span class="label-text">Description</span>
          </label>
          <textarea 
            class="textarea textarea-bordered" 
            name="description"
            [(ngModel)]="formData.description"
            required
            #description="ngModel"
          ></textarea>
          <label class="label" *ngIf="description.invalid && (description.dirty || description.touched)">
            <span class="label-text-alt text-error">La description est requise</span>
          </label>
        </div>
      </form>
    </mat-dialog-content>
    
    <mat-dialog-actions align="end">
      <button mat-button (click)="dialogRef.close()">Annuler</button>
      <button 
        mat-button 
        color="primary"
        (click)="onSubmit()"
        [disabled]="!modelForm.form.valid || isLoading"
      >
        <span *ngIf="isLoading" class="loading loading-spinner"></span>
        {{data.model ? 'Modifier' : 'Ajouter'}}
      </button>
    </mat-dialog-actions>
  `
})
export class ModelDialogComponent implements OnInit {
  formData = {
    brandId: null as number | null,
    name: '',
    description: ''
  };
  brands: Brand[] = [];
  isLoading = false;

  constructor(
    public dialogRef: MatDialogRef<ModelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { model?: Model },
    private modelService: ModelService,
    private brandService: BrandService
  ) {
    if (data.model) {
      this.formData = {
        brandId: data.model.brand.id,
        name: data.model.name,
        description: data.model.description || ''
      };
    }
  }

  ngOnInit() {
    this.loadBrands();
  }

  loadBrands() {
    this.brandService.getAllBrands().subscribe(brands => {
      this.brands = brands;
    });
  }

  close() {
    // Handle dialog close
  }

  onSubmit() {
    if (!this.formData.brandId || !this.formData.name || this.isLoading) return;

    this.isLoading = true;

    const modelDto = {
      brandId: this.formData.brandId,
      name: this.formData.name,
      description: this.formData.description
    };
    
    if (this.data.model) {
      this.modelService.updateModel(this.data.model.id, modelDto).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error(error);
          this.isLoading = false;
        }
      });
    } else {
      this.modelService.createModel(modelDto).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error(error);
          this.isLoading = false;
        }
      });
    }
  }
}