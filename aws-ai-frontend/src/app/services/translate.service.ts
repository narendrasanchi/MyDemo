import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TranslateTextRequest, TranslateTextResponse, ApiResponse } from '../models/ai-services.models';

@Injectable({
  providedIn: 'root'
})
export class TranslateService {
  private baseUrl = 'https://localhost:7109/api/translate'; // Update with your API URL

  constructor(private http: HttpClient) { }

  translateText(request: TranslateTextRequest): Observable<ApiResponse<TranslateTextResponse>> {
    return this.http.post<ApiResponse<TranslateTextResponse>>(`${this.baseUrl}/translate`, request);
  }

  getSupportedLanguages(): Observable<ApiResponse<string[]>> {
    return this.http.get<ApiResponse<string[]>>(`${this.baseUrl}/supported-languages`);
  }
}
