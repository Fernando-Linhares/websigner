import { Injectable } from '@angular/core';
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export abstract class BaseServiceService {
  protected readonly baseUrl: string;
  protected constructor() {
    this.baseUrl = environment.baseUrl;
  }
}
