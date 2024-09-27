import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {SignInService} from "../services/sign-in.service";
import {HttpClientModule} from "@angular/common/http";
import {RegisterService} from "../services/register.service";
import {SessionService} from "../services/session.service";
import {CertificateService} from "../services/certificate.service";
import {SignatureService} from "../services/signature.service";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {FilesService} from "../services/files.service";

@NgModule({
  declarations: [],
  providers: [
    SignInService,
    RegisterService,
    SessionService,
    CertificateService,
    SignatureService,
    FilesService
  ],
  imports: [
    HttpClientModule,
    CommonModule,
    MatProgressSpinnerModule
  ]
})
export class ServicesProviderModule { }
