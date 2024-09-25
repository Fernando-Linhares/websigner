import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SignInService {

  private baseUrl: string = 'http://localhost:5000';

  constructor(
    private http: HttpClient
  ) {}


  public login(email: string, password: string): Observable<any> {
    return this.http.post(this.baseUrl + '/auth/login', {
      email,
      password
    })
  }
}
