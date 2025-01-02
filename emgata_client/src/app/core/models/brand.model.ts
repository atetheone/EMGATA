import { Model } from "./model.model";

export interface Brand {
  id: number;
  name: string;
  description?: string;
  createdAt?: Date;
  updatedAt?: Date;
  models?: Model[];
}

export interface CreateBrandDto {
  name: string;
  description?: string;
}

export interface UpdateBrandDto {
  name: string;
  description?: string;
}
