import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {BaseServiceService} from "./base-service.service";

@Injectable({
  providedIn: 'root'
})
export class SignInService extends BaseServiceService
{
  constructor(
    private http: HttpClient
  ) {
    super();
  }


  public login(email: string, password: string): Observable<any> {
    return this.http.post(this.baseUrl + '/auth/login', {
      email,
      password
    })
  }
}
