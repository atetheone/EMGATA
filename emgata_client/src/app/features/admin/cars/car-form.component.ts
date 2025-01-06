// src/app/features/admin/cars/car-form.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { CarService } from '#services/car.service';
import { BrandService } from '#services/brand.service';
import { ModelService } from '#services/model.service';
import { Car, CarStatus } from '#models/car.model';
import { Brand } from '#models/brand.model';
import { Model } from '#models/model.model';

@Component({
  selector: 'app-car-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="p-4">
      <div class="flex justify-between items-center mb-6">
        <h1 class="text-2xl font-bold">{{isEditMode ? 'Modifier' : 'Ajouter'}} un Véhicule</h1>
        <button class="btn" routerLink="/admin/cars">
          Retour
        </button>
      </div>

      <form (ngSubmit)="onSubmit()" #carForm="ngForm" class="space-y-6">
        <!-- Brand and Model Selection -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div class="form-control">
            <label class="label">
              <span class="label-text">Marque</span>
            </label>
            <select 
              class="select select-bordered" 
              name="brandId"
              [(ngModel)]="selectedBrandId"
              (change)="onBrandChange()"
              required
              #brandSelect="ngModel"
            >
              <option [ngValue]="null">Sélectionner une marque</option>
              <option *ngFor="let brand of brands" [value]="brand.id">
                {{brand.name}}
              </option>
            </select>
          </div>

          <div class="form-control">
            <label class="label">
              <span class="label-text">Modèle</span>
            </label>
            <select 
              class="select select-bordered" 
              name="modelId"
              [(ngModel)]="formData.modelId"
              required
              #modelSelect="ngModel"
              [disabled]="!selectedBrandId"
            >
              <option [ngValue]="null">Sélectionner un modèle</option>
              <option *ngFor="let model of models" [value]="model.id">
                {{model.name}}
              </option>
            </select>
          </div>
        </div>

        <!-- Car Details -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div class="form-control">
            <label class="label">
              <span class="label-text">Année</span>
            </label>
            <input 
              type="number" 
              class="input input-bordered" 
              name="year"
              [(ngModel)]="formData.year"
              required
              min="2010"
              max="2025"
              #year="ngModel"
            />
          </div>

          <div class="form-control">
            <label class="label">
              <span class="label-text">Couleur</span>
            </label>
            <input 
              type="text" 
              class="input input-bordered" 
              name="color"
              [(ngModel)]="formData.color"
              required
              #color="ngModel"
            />
          </div>

          <div class="form-control">
            <label class="label">
              <span class="label-text">Prix</span>
            </label>
            <input 
              type="number" 
              class="input input-bordered" 
              name="price"
              [(ngModel)]="formData.price"
              required
              min="0"
              #price="ngModel"
            />
          </div>
        </div>

        <!-- Status Selection -->
        <div class="form-control">
          <label class="label">
            <span class="label-text">Statut</span>
          </label>
          <select 
            class="select select-bordered" 
            name="status"
            [(ngModel)]="formData.status"
            required
            #status="ngModel"
          >
            <option [ngValue]="CarStatus.Available">Disponible</option>
            <option [ngValue]="CarStatus.Sold">Vendu</option>
            <option [ngValue]="CarStatus.NotAvailable">Non Disponible</option>
          </select>
        </div>

        <!-- Description -->
        <div class="form-control">
          <label class="label">
            <span class="label-text">Description</span>
          </label>
          <textarea 
            class="textarea textarea-bordered h-24" 
            name="description"
            [(ngModel)]="formData.description"
            required
            #description="ngModel"
          ></textarea>
        </div>

        <!-- Images -->
        <div class="form-control">
          <label class="label">
            <span class="label-text">Images</span>
          </label>
          
          <!-- Existing Images (Edit Mode) -->
          <div class="grid grid-cols-2 md:grid-cols-4 gap-4 mb-4" *ngIf="isEditMode && car?.images?.length">
            @for (image of car?.images; track image.id) {
              <div class="relative">
                <img [src]="image.imageUrl" class="w-full h-32 object-cover rounded-lg">
                <button 
                  type="button"
                  class="btn btn-circle btn-error btn-sm absolute top-2 right-2"
                  (click)="deleteImage(image.id)"
                >
                  ×
                </button>
                <button 
                  type="button"
                  class="btn btn-circle btn-sm absolute top-2 left-2"
                  [class.btn-primary]="image.isMain"
                  [class.btn-ghost]="!image.isMain"
                  (click)="setMainImage(image.id)"
                  [disabled]="image.isMain"
                >
                <svg 
                  xmlns="http://www.w3.org/2000/svg" 
                  viewBox="0 0 24 24" 
                  class="w-4 h-4"
                  [class.fill-white]="image.isMain"
                  [class.stroke-current]="!image.isMain"
                  stroke-width="2"
                >
                  <path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z"/>
                </svg>
                </button>
              </div>
            }
            
          </div>

          <!-- New Images Upload -->
          <input 
            type="file" 
            class="file-input file-input-bordered w-full" 
            (change)="onFileSelected($event)"
            accept="image/*"
            multiple
          />
        </div>

        <!-- Preview of New Images -->
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4" *ngIf="selectedFiles.length">
          @for (file of selectedFiles; track file; let i = $index) {
            <div class="relative">
              <img [src]="previewUrls[i]" class="w-full h-32 object-cover rounded-lg">
              <button 
                type="button"
                class="btn btn-circle btn-error btn-sm absolute top-2 right-2"
                (click)="removeSelectedFile(i)"
              >
                ×
              </button>
            </div>
          }
          
        </div>

        <!-- Error Messages -->
        <div class="alert alert-error" *ngIf="error">
          {{error}}
        </div>

        <!-- Submit Button -->
        <div class="flex justify-end space-x-4">
          <button 
            type="button" 
            class="btn" 
            routerLink="/admin/cars"
            [disabled]="isLoading"
          >
            Annuler
          </button>
          <button 
            type="submit" 
            class="btn btn-primary"
            [disabled]="!carForm.form.valid || isLoading"
          >
            <span *ngIf="isLoading" class="loading loading-spinner"></span>
            {{isEditMode ? 'Modifier' : 'Ajouter'}}
          </button>
        </div>
      </form>
    </div>
  `
})
export class CarFormComponent implements OnInit {
  car?: Car;
  isEditMode = false;
  isLoading = false;
  error = '';

  brands: Brand[] = [];
  models: Model[] = [];
  selectedBrandId: number | null = null;

  selectedFiles: File[] = [];
  previewUrls: string[] = [];

  CarStatus = CarStatus;

  formData = {
    modelId: null as number | null,
    year: new Date().getFullYear(),
    color: '',
    price: 0,
    description: '',
    status: CarStatus.Available as CarStatus
  };

  constructor(
    private carService: CarService,
    private brandService: BrandService,
    private modelService: ModelService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.loadBrands();
    const carId = this.route.snapshot.params['id'];
    if (carId) {
      this.isEditMode = true;
      this.loadCar(carId);
    }
  }

  loadBrands() {
    this.brandService.getAllBrands().subscribe(brands => {
      this.brands = brands;
    });
  }

  loadCar(id: number) {
    this.carService.getCarById(id).subscribe(car => {
      this.car = car;
      this.selectedBrandId = car.model.brand.id;
      this.onBrandChange();
      
      this.formData = {
        modelId: car.model.id,
        year: car.year,
        color: car.color,
        price: car.price,
        description: car.description,
        status: car.status
      };
    });
  }

  onBrandChange() {
    if (this.selectedBrandId) {
      this.modelService.getModelsByBrand(this.selectedBrandId).subscribe(models => {
        this.models = models;
      });
    } else {
      this.models = [];
      this.formData.modelId = null;
    }
  }

  onFileSelected(event: any) {
    const files = event.target.files;
    if (files) {
      for (let i = 0; i < files.length; i++) {
        this.selectedFiles.push(files[i]);
        
        // Create preview URL
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.previewUrls.push(e.target.result);
        };
        reader.readAsDataURL(files[i]);
      }
    }
  }

  removeSelectedFile(index: number) {
    this.selectedFiles.splice(index, 1);
    this.previewUrls.splice(index, 1);
  }

  deleteImage(imageId: number) {
    if (confirm('Êtes-vous sûr de vouloir supprimer cette image ?')) {
      this.carService.deleteCarImage(this.car!.id, imageId).subscribe(() => {
        this.loadCar(this.car!.id);
      });
    }
  }

  setMainImage(imageId: number) {
    this.carService.setMainCarImage(this.car!.id, imageId).subscribe(() => {
      this.loadCar(this.car!.id);
    });
  }

  onSubmit() {
    if (!this.formData.modelId) return;
  
    this.isLoading = true;
    this.error = '';
  
    if (this.isEditMode) {
      console.log(JSON.stringify(this.formData))
      this.carService.updateCar(this.car!.id, this.formData).subscribe({
        next: (updatedCar) => {
          if (this.selectedFiles.length > 0) {
            this.uploadImages(updatedCar.id);
          } else {
            this.isLoading = false;
            this.router.navigate(['/admin/cars']);
          }
        },
        error: (error) => {
          console.error(error);
          this.error = error.message || 'Une erreur est survenue lors de la mise à jour';
          this.isLoading = false;
        }
      });
    } else {
      this.carService.createCar(this.formData).subscribe({
        next: (newCar) => {
          if (this.selectedFiles.length > 0) {
            this.uploadImages(newCar.id);
          } else {
            this.isLoading = false;
            this.router.navigate(['/admin/cars']);
          }
        },
        error: (error) => {
          console.error(error);
          this.error = error.message || 'Une erreur est survenue lors de la création';
          this.isLoading = false;
        }
      });
    }
  }
  
  private uploadImages(carId: number) {
    let completedUploads = 0;
    
    this.selectedFiles.forEach(file => {
      const formData = new FormData();
      formData.append('file', file);
      
      this.carService.uploadCarImage(carId, formData).subscribe({
        next: () => {
          completedUploads++;
          if (completedUploads === this.selectedFiles.length) {
            this.isLoading = false;
            this.router.navigate(['/admin/cars']);
          }
        },
        error: (error) => {
          console.error(error);
          this.error = 'Une erreur est survenue lors du téléchargement des images';
          this.isLoading = false;
        }
      });
    });
  }
}
