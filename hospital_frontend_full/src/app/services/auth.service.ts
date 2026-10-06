
  
  import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class AuthService {
  api = 'https://:5092/api/auth'; // Your backend URL
  constructor(private http: HttpClient) {}

  login(username: string, password: string) {
    return this.http.post<any>(this.api, { username, password });
  }

  isLoggedIn() {
    return !!localStorage.getItem('jwt');
  }

  logout() {
    localStorage.removeItem('jwt');
  }
  
  
  
}
