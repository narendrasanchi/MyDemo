import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { 
  UserRegistrationRequest, 
  UserRegistrationResponse, 
  UserLoginRequest, 
  UserLoginResponse,
  ApiResponse 
} from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'https://localhost:7109/api'; // Update with your API URL
  private currentUserSubject = new BehaviorSubject<any>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    // Check if user is already logged in
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUserSubject.next(JSON.parse(storedUser));
    }
  }

  register(request: UserRegistrationRequest): Observable<ApiResponse<UserRegistrationResponse>> {
    return this.http.post<ApiResponse<UserRegistrationResponse>>(`${this.baseUrl}/cognito/register`, request);
  }

  login(request: UserLoginRequest): Observable<ApiResponse<UserLoginResponse>> {
    return this.http.post<ApiResponse<UserLoginResponse>>(`${this.baseUrl}/cognito/login`, request)
      .pipe(
        map(response => {
          if (response.isSuccess && response.model.success) {
            // Store user details and jwt token in local storage
            localStorage.setItem('currentUser', JSON.stringify(response.model));
            localStorage.setItem('accessToken', response.model.accessToken);
            this.currentUserSubject.next(response.model);
          }
          return response;
        })
      );
  }

  logout(): void {
    // Remove user from local storage and set current user to null
    localStorage.removeItem('currentUser');
    localStorage.removeItem('accessToken');
    this.currentUserSubject.next(null);
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('accessToken');
    return !!token;
  }

  getToken(): string | null {
    return localStorage.getItem('accessToken');
  }
}
