import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UrlApiService {
  
  private apiUrl = '/api'; // for local https://localhost:7135/api

  constructor(private http: HttpClient) { }

  // URL and mark as "Private"
  createUrl(longUrl: string, isPrivate: boolean): Observable<any> {
    return this.http.post(`${this.apiUrl}/url`, { longUrl, isPrivate });
  }

  // List public short URLs 
  getPublicUrls(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/public`);
  }

  // Search and delete a short URL
  deleteUrl(code: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${code}`);
  }
}