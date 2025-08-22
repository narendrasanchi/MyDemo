import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface SelectionTypesSchema {
  type: string;
  items: {
    type: string;
    enum: string[];
  };
}

export interface ProcessSelectionResult {
  isSuccess: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class SelectionTypesService {
  private readonly apiUrl = 'http://localhost:5108/api/selection-types';

  constructor(private http: HttpClient) { }

  getSelectionTypes(): Observable<SelectionTypesSchema> {
    return this.http.get<SelectionTypesSchema>(this.apiUrl);
  }

  processSelectionTypes(selectedValues: string[]): Observable<ProcessSelectionResult> {
    return this.http.post<ProcessSelectionResult>(this.apiUrl, selectedValues);
  }
}
