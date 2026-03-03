import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UrlApiService {
  // TODO: Replace 7124 with the port number you found in Step 1
  private apiUrl = 'https://localhost:7135/api'; 

  constructor(private http: HttpClient) { }

  // Requirement: Submit a URL and mark as "Private" [cite: 38, 39]
  createUrl(longUrl: string, isPrivate: boolean): Observable<any> {
    return this.http.post(`${this.apiUrl}/url`, { longUrl, isPrivate });
  }

  // Requirement: List public short URLs [cite: 38]
  getPublicUrls(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/public`);
  }

  // Requirement: Search and delete a short URL [cite: 41]
  deleteUrl(code: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${code}`);
  }
}