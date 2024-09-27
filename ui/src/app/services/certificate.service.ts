import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {SessionService} from "./session.service";
import {lastValueFrom} from "rxjs";
import {AuthenticatedServiceService} from "./authenticated-service.service";

@Injectable({
  providedIn: 'root'
})
export class CertificateService extends AuthenticatedServiceService
{
  constructor(
    private http: HttpClient,
    private sessionService: SessionService
  ) {
    super(sessionService);
  }

  public async addCert(body: any): Promise<any> {
    const form = new FormData();
    form.append('name', body.alias);
    form.append('file', body.file, body.file.name);

    return lastValueFrom(
      this.http.post(this.baseUrl + '/certificate', form , this.withBearerToken())
    );
  }

  public async select(id: number): Promise<any> {
    return lastValueFrom(this.http.patch(this.baseUrl + '/certificate/'+ id, {},  this.withBearerToken()));
  }

  public async remove(id: number): Promise<any> {
    return lastValueFrom(this.http.delete(this.baseUrl + '/certificate/' + id,  this.withBearerToken()));
  }

  public async list(): Promise<any> {
    return lastValueFrom(this.http.get(this.baseUrl + '/certificate',  this.withBearerToken()));
  }
}
