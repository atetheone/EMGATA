import { Brand } from "./brand.model";
import { Car } from "./car.model";

export interface Model {
  id: number;
  brandId: number;
  name: string;
  description: string;
  createdAt?: Date;
  updatedAt?: Date;
  brand: Brand;
  cars?: Car[];
}

export interface CreateModelDto {
  brandId: number;
  name: string;
  description: string;
}

export interface UpdateModelDto {
  brandId: number;
  name: string;
  description: string;
}