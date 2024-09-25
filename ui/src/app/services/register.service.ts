import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  private baseUrl: string = 'http://localhost:5080';

  constructor(
    private http: HttpClient
  ) { }

  public register(name: string, email: string, password: string): Observable<any> {
    return this.http.post(this.baseUrl + '/auth/register', {
      name,
      email,
      password
    })
  }
}
