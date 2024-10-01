import {Component, Input} from '@angular/core';
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ServicesProviderModule} from "../../services-provider/services-provider.module";
import {ActivatedRoute, Router} from "@angular/router";
import swal from "sweetalert";
import {ForgotPasswordServiceService} from "../../services/forgot-password-service.service";
import {SessionService} from "../../services/session.service";
import {HttpErrorResponse} from "@angular/common/http";
import {lastValueFrom} from "rxjs";

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    NgClass,
    FormsModule,
    ServicesProviderModule
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent
{
  token: string = '';
  password = { text: this.token, visible: false, notValid: false };
  confirmPassword = { text: '', visible: false, notValid: false };
  error: Array<any> = [];

  constructor(
    route: ActivatedRoute,
    private sessionService: SessionService,
    private forgotPasswordService: ForgotPasswordServiceService) {
    this.token = route.snapshot.params['token'];
  }

  public async onSubmit(): Promise<void> {
    try {

      if (this.password.text.length == 0) {
        this.password.notValid = true;
        this.error.push({field: "password", message: "field password cannot be empty"});
        return;
      }

      if (this.passwordNotMatching()) {
        this.password.notValid = true;
        this.confirmPassword.notValid = true;
        this.error.push({field: "password", message: "The password don't match"});
        return;
      }

       await this.forgotPasswordService.resetPassword(this.password.text, this.token)
       this.sessionService.gotoLogin()
    }catch (error) {
      swal('Error', 'Expired Token', 'error')
    }
  }

  private passwordNotMatching(): boolean {
    return this.password.text !== this.confirmPassword.text;
  }
}
