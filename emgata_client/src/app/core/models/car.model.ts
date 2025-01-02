import { Model } from "./model.model";

export interface Car {
  id: number;
  model: Model;
  status: CarStatus;
  year: number;
  color: string;
  price: number;
  description: string;
  images: CarImage[];
  createdAt?: Date;
  updatedAt?: Date;
}

export interface CarImage {
  id: number;
  imageUrl: string;
  isMain: boolean;
}

export enum CarStatus {
  Available = 1,
  Sold = 2,
  NotAvailable = 3
}
