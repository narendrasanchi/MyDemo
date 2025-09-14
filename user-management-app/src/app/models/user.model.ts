export interface RegisterUserRequest {
  username: string;
  email: string;
  phoneNumber?: string;
  password: string;
  givenName?: string;
  familyName?: string;
}

export interface UpdateUserRequest {
  email?: string;
  phoneNumber?: string;
  givenName?: string;
  familyName?: string;
}

export interface UserResponse {
  username: string;
  email: string;
  phoneNumber?: string;
  givenName?: string;
  familyName?: string;
  emailVerified: boolean;
  phoneVerified: boolean;
  userStatus: string;
  creationDate?: Date;
  lastModifiedDate?: Date;
}

export interface ApiResponse {
  message: string;
  username?: string;
}