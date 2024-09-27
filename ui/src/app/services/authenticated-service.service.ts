import { Injectable } from '@angular/core';
import {BaseServiceService} from "./base-service.service";
import {SessionService} from "./session.service";

@Injectable({
  providedIn: 'root'
})
export abstract class AuthenticatedServiceService extends BaseServiceService
{
  protected constructor(
    private session: SessionService
  ) {
    super();
  }

  protected withBearerToken(): any {
    return {headers: {Authorization: 'Bearer '+ this.session.user().token}};
  }
}
