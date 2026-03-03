import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // Required for *ngFor
import { FormsModule } from '@angular/forms'; // Required for [(ngModel)]
import { UrlApiService } from './services/url-api'; 

@Component({
  selector: 'app-root',
  standalone: true, // This is key for newer Angular versions
  imports: [CommonModule, FormsModule], // Import these here
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class AppComponent implements OnInit {
  longUrl: string = '';
  isPrivate: boolean = false;
  publicUrls: any[] = [];

  constructor(private apiService: UrlApiService) {}

  ngOnInit() {
    this.loadUrls();
  }

  loadUrls() {
    this.apiService.getPublicUrls().subscribe(res => this.publicUrls = res);
  }

  generateUrl() {
    if(!this.longUrl) return;
    this.apiService.createUrl(this.longUrl, this.isPrivate).subscribe(() => {
      this.longUrl = '';
      this.loadUrls();
    });
  }

  deleteUrl(code: string) {
    this.apiService.deleteUrl(code).subscribe(() => this.loadUrls());
  }

  copyToClipboard(code: string) {
  	const fullUrl = `https://localhost:7135/${code}`;
  	navigator.clipboard.writeText(fullUrl).then(() => {
    	// You could replace this alert with a toast message for better UI
    	alert('Short URL copied to clipboard!');
  	});
 }
}