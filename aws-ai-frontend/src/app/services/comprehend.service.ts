import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  SentimentAnalysisRequest, 
  SentimentAnalysisResponse, 
  EntityDetectionRequest, 
  EntityDetectionResponse,
  ApiResponse 
} from '../models/ai-services.models';

@Injectable({
  providedIn: 'root'
})
export class ComprehendService {
  private baseUrl = 'https://localhost:7109/api/comprehend'; // Update with your API URL

  constructor(private http: HttpClient) { }

  analyzeSentiment(request: SentimentAnalysisRequest): Observable<ApiResponse<SentimentAnalysisResponse>> {
    return this.http.post<ApiResponse<SentimentAnalysisResponse>>(`${this.baseUrl}/sentiment`, request);
  }

  detectEntities(request: EntityDetectionRequest): Observable<ApiResponse<EntityDetectionResponse>> {
    return this.http.post<ApiResponse<EntityDetectionResponse>>(`${this.baseUrl}/entities`, request);
  }
}
