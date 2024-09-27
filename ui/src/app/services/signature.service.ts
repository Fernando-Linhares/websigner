import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {lastValueFrom} from "rxjs";
import {SessionService} from "./session.service";
import {AuthenticatedServiceService} from "./authenticated-service.service";

@Injectable({
  providedIn: 'root'
})
export class SignatureService extends AuthenticatedServiceService
{
  constructor(
    private http: HttpClient,
    private sessionService: SessionService
  ) {
    super(sessionService);
  }

  public async signPdf(pin: string, file: File, certId: number): Promise<any> {

    const form = new FormData();
    form.append('pin', pin);
    form.append('file', file, file.name);
    form.append('certId', String(certId));

    return await lastValueFrom<any>(this.http.post(
      this.baseUrl + '/sign/pdf',
      form,
      this.withBearerToken()
      ));
  }
}
