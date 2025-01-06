import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { Brand } from '#models/brand.model';
import { BrandService } from '#services/brand.service';

@Component({
  selector: 'app-brand-dialog',
  imports: [
    CommonModule, 
    FormsModule, 
    MatDialogModule, 
    MatButtonModule
  ],
  template: `
     <h2 mat-dialog-title>{{data.brand ? 'Modifier la marque' : 'Ajouter une marque'}}</h2>
    
    <mat-dialog-content>
      <form (ngSubmit)="onSubmit()" #brandForm="ngForm">
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
          ></textarea>
        </div>
      </form>
    </mat-dialog-content>
    
    <mat-dialog-actions align="end">
      <button mat-button (click)="dialogRef.close()">Annuler</button>
      <button 
        mat-button 
        color="primary"
        (click)="onSubmit()"
        [disabled]="!brandForm.form.valid || isLoading"
      >
        <span *ngIf="isLoading" class="loading loading-spinner"></span>
        {{data.brand ? 'Modifier' : 'Ajouter'}}
      </button>
    </mat-dialog-actions>
  `
})
export class BrandDialogComponent {
  formData = {
    name: '',
    description: ''
  };
  isLoading = false;

  constructor(
    public dialogRef: MatDialogRef<BrandDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { brand?: Brand },
    private brandService: BrandService
  ) {
    if (data.brand) {
      this.formData.name = data.brand.name;
      this.formData.description = data.brand.description || '';
    }
  }

  close() {
    // Handle dialog close
  }

  onSubmit() {
    if (!this.formData.name || this.isLoading) return;

    this.isLoading = true;
    
    if (this.data.brand) {
      this.brandService.updateBrand(this.data.brand.id, this.formData).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error(error);
          this.isLoading = false;
        }
      });
    } else {
      this.brandService.createBrand(this.formData).subscribe({
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