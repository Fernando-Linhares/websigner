import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {lastValueFrom} from "rxjs";
import {BaseServiceService} from "./base-service.service";

@Injectable({
  providedIn: 'root'
})
export class RegisterService  extends BaseServiceService
{
  constructor(
    private http: HttpClient
  ) {
    super();
  }

  public register(name: string, email: string, password: string): Promise<any> {
    return lastValueFrom(this.http.post(this.baseUrl + '/auth/register', {
      name,
      email,
      password
    }))
  }
}
