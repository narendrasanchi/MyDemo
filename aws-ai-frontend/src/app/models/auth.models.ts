// Auth Models
export interface UserRegistrationRequest {
  username: string;
  password: string;
  email: string;
  phoneNumber?: string;
  userAttributes?: { [key: string]: string };
}

export interface UserRegistrationResponse {
  userSub: string;
  success: boolean;
  errorMessage?: string;
  emailVerificationRequired: boolean;
  phoneVerificationRequired: boolean;
}

export interface UserLoginRequest {
  username: string;
  password: string;
}

export interface UserLoginResponse {
  accessToken: string;
  idToken: string;
  refreshToken: string;
  expiresIn: number;
  success: boolean;
  errorMessage?: string;
  challengeName?: string;
  session?: string;
}

// Common Response
export interface ApiResponse<T> {
  model: T;
  code: number;
  messages: any[];
  isSuccess: boolean;
}