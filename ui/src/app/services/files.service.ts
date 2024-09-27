import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {lastValueFrom} from "rxjs";
import {AuthenticatedServiceService} from "./authenticated-service.service";
import {SessionService} from "./session.service";

@Injectable({
  providedIn: 'root'
})
export class FilesService extends AuthenticatedServiceService {
  constructor(
    private http: HttpClient,
    sessionService: SessionService
  ) {
    super(sessionService);
  }

  public list(): Promise<any> {
    return lastValueFrom(this.http.get(this.baseUrl + '/files', this.withBearerToken()))
  }
}
