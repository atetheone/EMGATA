// src/app/core/services/car.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Car, CarStatus } from '../models/car.model';

@Injectable({
  providedIn: 'root'
})
export class CarService {
  private apiUrl = `${environment.apiUrl}/cars`;

  constructor(private http: HttpClient) {}

  // Get all cars
  getAllCars(): Observable<Car[]> {
    return this.http.get<Car[]>(this.apiUrl);
  }

  // Get available cars
  getAvailableCars(): Observable<Car[]> {
    return this.http.get<Car[]>(`${this.apiUrl}/available`);
  }

  // Get car by ID
  getCarById(id: number): Observable<Car> {
    return this.http.get<Car>(`${this.apiUrl}/${id}`);
  }

  // Create new car
  createCar(car: Partial<Car>): Observable<Car> {
    return this.http.post<Car>(this.apiUrl, car);
  }

  // Update car
  updateCar(id: number, car: Partial<Car>): Observable<Car> {
    return this.http.put<Car>(`${this.apiUrl}/${id}`, car);
  }

  deleteCar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateCarStatus(id: number, status: CarStatus): Observable<Car> {
    return this.http.patch<Car>(`${this.apiUrl}/${id}/status`, { status });
  }
  // Image management
  uploadCarImage(carId: number, formData: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/${carId}/images`, formData);
  }

  getCarImages(carId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${carId}/images`);
  }

  deleteCarImage(carId: number, imageId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${carId}/images/${imageId}`);
  }

  setMainCarImage(carId: number, imageId: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${carId}/images/${imageId}/set-main`, {});
  }

  // Get cars by various filters
  getCarsByBrand(brandId: number): Observable<Car[]> {
    return this.http.get<Car[]>(`${this.apiUrl}/brand/${brandId}`);
  }

  getCarsByModel(modelId: number): Observable<Car[]> {
    return this.http.get<Car[]>(`${this.apiUrl}/model/${modelId}`);
  }

  getCarsByUser(userId: number): Observable<Car[]> {
    return this.http.get<Car[]>(`${this.apiUrl}/user/${userId}`);
  }

  // Advanced search
  searchCars(params: {
    brandId?: number,
    modelId?: number,
    minYear?: number,
    maxYear?: number,
    minPrice?: number,
    maxPrice?: number,
    status?: CarStatus,
    color?: string,
  }): Observable<Car[]> {
    return this.http.get<Car[]>(`${this.apiUrl}/search`, { params: { ...params } as any });
  }
}
