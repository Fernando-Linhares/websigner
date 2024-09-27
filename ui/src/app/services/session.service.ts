import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Router } from "@angular/router";
import { lastValueFrom } from "rxjs";
import {BaseServiceService} from "./base-service.service";

@Injectable({
  providedIn: 'root'
})
export class SessionService extends BaseServiceService
{
  public isOpen: boolean = false;
  constructor(private http: HttpClient, private router: Router) {
    super();
  }
  public create(id: number, token: string): void {
    localStorage.setItem('user', JSON.stringify({
      id,
      token
    }));
  }

  public user(): any {
    return JSON.parse(<string>localStorage.getItem('user'));
  }

  public async me()
  {
      const headers = new HttpHeaders({
        Accept: 'application/json',
        'Authorization': 'Bearer ' + this.user().token
      });

      return await lastValueFrom<any>(this.http.get(this.baseUrl + '/me', {headers})).catch((err) => console.log(err));
  }

  public open(): void {
    this.isOpen = true;
    this.gotoDashboard()
  }

  public gotoDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  public gotoLogin(): void {
    this.router.navigate(['/']);
  }

  public abort(): void {
    localStorage.removeItem('user');
    this.isOpen = false;
    this.gotoLogin();
  }
}
