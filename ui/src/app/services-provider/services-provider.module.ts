import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {SignInService} from "../services/sign-in.service";
import {HttpClientModule} from "@angular/common/http";
import {RegisterService} from "../services/register.service";
import {SessionService} from "../services/session.service";
import {CertificateService} from "../services/certificate.service";
import {SignatureService} from "../signature.service";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  declarations: [],
  providers: [SignInService, RegisterService, SessionService, CertificateService, SignatureService],
  imports: [
    HttpClientModule,
    CommonModule,
    MatProgressSpinnerModule
  ]
})
export class ServicesProviderModule { }
