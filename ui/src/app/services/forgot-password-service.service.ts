import { Injectable } from '@angular/core';
import {BaseServiceService} from "./base-service.service";
import {HttpClient} from "@angular/common/http";
import {lastValueFrom, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ForgotPasswordServiceService extends BaseServiceService
{
  constructor(private http: HttpClient) {
    super();
  }

  public async sendEmailTo(email: string): Promise<void> {
    await lastValueFrom(this.http.post(this.baseUrl + '/forgot-password', {
      email
    }));
  }

  public async resetPassword(password: string, token: string): Promise<void> {
    const headers = { Authorization: `Bearer ${token}` };
     await lastValueFrom(this.http.post(this.baseUrl + '/reset-password', {password}, {headers}));
  }
}
