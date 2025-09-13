import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterUserRequest, UpdateUserRequest, UserResponse, ApiResponse } from '../models/user.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5141/api/users';

  constructor(private http: HttpClient) { }

  registerUser(user: RegisterUserRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/register`, user);
  }

  getUsers(): Observable<UserResponse[]> {
    return this.http.get<UserResponse[]>(this.apiUrl);
  }

  getUser(username: string): Observable<UserResponse> {
    return this.http.get<UserResponse>(`${this.apiUrl}/${username}`);
  }

  updateUser(username: string, user: UpdateUserRequest): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}/${username}`, user);
  }

  deleteUser(username: string): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}/${username}`);
  }
}
