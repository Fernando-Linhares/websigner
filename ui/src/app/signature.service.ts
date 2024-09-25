import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {lastValueFrom} from "rxjs";
import {SessionService} from "./services/session.service";

@Injectable({
  providedIn: 'root'
})
export class SignatureService {

  private baseUrl = 'http://localhost:5000';

  constructor(
    private http: HttpClient,
    private sessionService: SessionService
  ) { }

  public async signPdf(pin: string, file: File, certId: number): Promise<any> {
    const headers = {
      'Authorization': 'Bearer ' + this.sessionService.user().token
    }

    const form = new FormData();
    form.append('pin', pin);
    form.append('file', file, file.name);
    form.append('certId', String(certId));

    return await lastValueFrom<any>(this.http.post(this.baseUrl + '/sign/pdf', form, {headers}))
  }
}
