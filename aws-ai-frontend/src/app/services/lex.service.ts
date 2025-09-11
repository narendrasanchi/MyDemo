import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChatMessage, ChatResponse, ApiResponse } from '../models/ai-services.models';

@Injectable({
  providedIn: 'root'
})
export class LexService {
  private baseUrl = 'https://localhost:7109/api/lex'; // Update with your API URL

  constructor(private http: HttpClient) { }

  sendMessage(message: ChatMessage): Observable<ApiResponse<ChatResponse>> {
    return this.http.post<ApiResponse<ChatResponse>>(`${this.baseUrl}/send-message`, message);
  }

  createSession(): Observable<any> {
    return this.http.post(`${this.baseUrl}/create-session`, {
      botName: 'YourBotName',
      botAlias: 'YourBotAlias',
      userId: 'user-' + Date.now(),
      localeId: 'en_US'
    });
  }
}
