import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { AppUser, LoginResponse, RegisterResponse } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5080/api';
  constructor(private http: HttpClient) {}

  register(data: {email:string; password:string; displayName:string; username:string; phoneNumber:string; birthDate:string; country:string; bio:string}): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/Auth/register`, data);
  }
  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/Auth/login`, { email, password }).pipe(
      tap(r => this.saveToken(r.idToken))
    );
  }
  saveToken(token: string) { localStorage.setItem('token', token); }
  getToken() { return localStorage.getItem('token'); }
  logout() { localStorage.removeItem('token'); localStorage.removeItem('user'); }
  isLoggedIn() { return !!this.getToken(); }
  getProfile(): Observable<AppUser> { return this.http.get<AppUser>(`${this.apiUrl}/User`, { headers: this.headers() }); }
  headers() { return { Authorization: `Bearer ${this.getToken() ?? ''}` }; }
}
