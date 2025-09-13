import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TextGenerationRequest, TextGenerationResponse, ApiResponse } from '../models/ai-services.models';

@Injectable({
  providedIn: 'root'
})
export class BedrockService {
  private baseUrl = 'https://localhost:7109/api/bedrock'; // Update with your API URL

  constructor(private http: HttpClient) { }

  generateText(request: TextGenerationRequest): Observable<ApiResponse<TextGenerationResponse>> {
    return this.http.post<ApiResponse<TextGenerationResponse>>(`${this.baseUrl}/generate-text`, request);
  }

  getAvailableModels(): Observable<any> {
    return this.http.get(`${this.baseUrl}/models`);
  }
}
