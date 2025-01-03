import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '#env/environment';
import { Model, CreateModelDto, UpdateModelDto } from '#models/model.model';

@Injectable({
  providedIn: 'root'
})
export class ModelService {
  private apiUrl = `${environment.apiUrl}/models`;

  constructor(private http: HttpClient) {}

  getAllModels(): Observable<Model[]> {
    return this.http.get<Model[]>(this.apiUrl);
  }

  getModelById(id: number): Observable<Model> {
    return this.http.get<Model>(`${this.apiUrl}/${id}`);
  }

  createModel(model: CreateModelDto): Observable<Model> {
    return this.http.post<Model>(this.apiUrl, model);
  }

  updateModel(id: number, model: UpdateModelDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, model);
  }

  deleteModel(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getModelsByBrand(brandId: number): Observable<Model[]> {
    return this.http.get<Model[]>(`${this.apiUrl}/brand/${brandId}`);
  }
}