import { Component } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ServicesProviderModule} from "../../services-provider/services-provider.module";
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {ForgotPasswordServiceService} from "../../services/forgot-password-service.service";

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    ServicesProviderModule,
    NgClass,
    FormsModule,
    NgForOf,
    NgIf
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
  email = { text: '', visible: false, notValid: false, sent: false };
  error: Array<any> = [];

  constructor(
    private forgotPasswordService: ForgotPasswordServiceService
  ) {
  }
  public async onSubmit(): Promise<void>
  {
    if (this.invalidEmail(this.email.text)) {
      this.email.notValid = true;
      this.error.push({field: "email", message: "email is not valid"});
      return;
    }

    await this.forgotPasswordService.sendEmailTo(this.email.text);

    this.email.sent = true;
  }

  private invalidEmail(email: string): boolean {
    return email.trim().match(/.*[@].*[.].*/i) == null;
  }
}
