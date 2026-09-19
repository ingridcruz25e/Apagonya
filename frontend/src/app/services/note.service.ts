import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Note, NoteDto } from '../models/note.model';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class NoteService {
  //La URL del backend
  private apiUrl = 'http://localhost:5289/api/Note';

  constructor(private http: HttpClient, private authService: AuthService) { }


  private getHeaders(): HttpHeaders{
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
    });
  }

  create(dto: NoteDto): Observable<Note>{
    return this.http.post<Note>(this.apiUrl, dto,{
      headers: this.getHeaders()
    });
  }

  getMyNotes(): Observable<Note[]>{
    return this.http.get<Note[]>(this.apiUrl, {
      headers: this.getHeaders()
    });
  }
}
