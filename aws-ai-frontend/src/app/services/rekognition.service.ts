import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ImageAnalysisResponse, FaceDetectionResponse, ApiResponse } from '../models/ai-services.models';

@Injectable({
  providedIn: 'root'
})
export class RekognitionService {
  private baseUrl = 'https://localhost:7109/api/rekognition'; // Update with your API URL

  constructor(private http: HttpClient) { }

  detectLabels(imageFile: File, maxLabels: number = 10, minConfidence: number = 50): Observable<ApiResponse<ImageAnalysisResponse>> {
    const formData = new FormData();
    formData.append('imageFile', imageFile);
    
    return this.http.post<ApiResponse<ImageAnalysisResponse>>(
      `${this.baseUrl}/detect-labels?maxLabels=${maxLabels}&minConfidence=${minConfidence}`, 
      formData
    );
  }

  detectFaces(imageFile: File): Observable<ApiResponse<FaceDetectionResponse>> {
    const formData = new FormData();
    formData.append('imageFile', imageFile);
    
    return this.http.post<ApiResponse<FaceDetectionResponse>>(`${this.baseUrl}/detect-faces`, formData);
  }

  detectText(imageFile: File, minConfidence: number = 50): Observable<ApiResponse<ImageAnalysisResponse>> {
    const formData = new FormData();
    formData.append('imageFile', imageFile);
    
    return this.http.post<ApiResponse<ImageAnalysisResponse>>(
      `${this.baseUrl}/detect-text?minConfidence=${minConfidence}`, 
      formData
    );
  }
}
