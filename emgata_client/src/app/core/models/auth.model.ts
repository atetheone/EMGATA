export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface AuthResponse {
  isSuccess: boolean;
  message: string;
  token?: string;
  expiration?: Date;
}
