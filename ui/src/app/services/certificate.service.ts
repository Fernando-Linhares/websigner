import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {SessionService} from "./session.service";
import {lastValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CertificateService {

  private baseURL = 'http://localhost:5000';

  constructor(
    private http: HttpClient,
    private sessionService: SessionService
  ) {}

  public async addCert(body: any): Promise<any> {

    const form = new FormData();
    form.append('name', body.alias);
    form.append('file', body.file, body.file.name);

    const headers = new HttpHeaders({
      'Authorization': 'Bearer ' + this.sessionService.user().token
    });

    return lastValueFrom(this.http.post(this.baseURL + '/certificate', form , {headers}));
  }

  public async select(id: number): Promise<any> {
    const headers = new HttpHeaders({
      'Authorization': 'Bearer ' + this.sessionService.user().token
    });

    return lastValueFrom(this.http.patch(this.baseURL + '/certificate/'+ id, {}, {headers}));
  }

  public async remove(id: number): Promise<any> {
    const headers = new HttpHeaders({
      'Authorization': 'Bearer ' + this.sessionService.user().token
    });

    return lastValueFrom(this.http.delete(this.baseURL + '/certificate/' + id, {headers}));
  }

  public async list(): Promise<any> {
    const headers = new HttpHeaders({
      'Authorization': 'Bearer ' + this.sessionService.user().token
    });

    return lastValueFrom(this.http.get(this.baseURL + '/certificate', {headers}));
  }
}
