import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TextToSpeechRequest, ApiResponse } from '../models/ai-services.models';

@Injectable({
  providedIn: 'root'
})
export class PollyService {
  private baseUrl = 'https://localhost:7109/api/polly'; // Update with your API URL

  constructor(private http: HttpClient) { }

  synthesizeSpeech(request: TextToSpeechRequest): Observable<Blob> {
    return this.http.post(`${this.baseUrl}/synthesize`, request, { 
      responseType: 'blob',
      headers: {
        'Content-Type': 'application/json'
      }
    });
  }

  getVoices(languageCode?: string): Observable<ApiResponse<string[]>> {
    const params = languageCode ? { languageCode } : {};
    return this.http.get<ApiResponse<string[]>>(`${this.baseUrl}/voices`, { params });
  }
}
