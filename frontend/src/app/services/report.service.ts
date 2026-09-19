import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PowerOutageReport, ReportCreateDto, ResolutionDto, Zone } from '../models/report.model';
import { AuthService } from './auth.service';

@Injectable({providedIn:'root'})
export class ReportService {
  private apiUrl='http://localhost:5080/api';
  constructor(private http:HttpClient, private auth:AuthService){}
  private options(){ return {headers:this.auth.headers()}; }
  zones():Observable<Zone[]>{ return this.http.get<Zone[]>(`${this.apiUrl}/Zone`,this.options()); }
  mine():Observable<PowerOutageReport[]>{ return this.http.get<PowerOutageReport[]>(`${this.apiUrl}/Report/mine`,this.options()); }
  byZone(zonaId:string):Observable<PowerOutageReport[]>{ return this.http.get<PowerOutageReport[]>(`${this.apiUrl}/Report/zone/${zonaId}`,this.options()); }
  create(dto:ReportCreateDto):Observable<PowerOutageReport>{ return this.http.post<PowerOutageReport>(`${this.apiUrl}/Report`,dto,this.options()); }
  confirm(id:string){ return this.http.post<PowerOutageReport>(`${this.apiUrl}/Report/${id}/confirm`,{},this.options()); }
  assigned():Observable<PowerOutageReport[]>{ return this.http.get<PowerOutageReport[]>(`${this.apiUrl}/Report/assigned`,this.options()); }
  resolve(id:string,dto:ResolutionDto){ return this.http.post<PowerOutageReport>(`${this.apiUrl}/Report/${id}/resolve`,dto,this.options()); }
  all():Observable<PowerOutageReport[]>{ return this.http.get<PowerOutageReport[]>(`${this.apiUrl}/Report`,this.options()); }
}
