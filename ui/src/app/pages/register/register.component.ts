import { Component } from '@angular/core';
import {NgForOf, NgIf, NgClass} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {Router} from "@angular/router";
import {RegisterService} from "../../services/register.service";
import {ServicesProviderModule} from "../../services-provider/services-provider.module";
import {SessionService} from "../../services/session.service";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    NgClass,
    FormsModule,
    ServicesProviderModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  name = { text: '', visible: false, notValid: false };
  email = { text: '', visible: false, notValid: false };
  password = { text: '', visible: false, notValid: false };
  confirmPassword = { text: '', visible: false, notValid: false };
  spinOpen: boolean = false;
  error: Array<any> = [];

  constructor(
    private router: Router,
    private service: RegisterService,
    private sessionService: SessionService
  ) {
  }

  ngOnInit(): void {
    if(this.sessionService.user()) {
      this.sessionService.gotoDashboard()
    }
  }

  public async onSubmit(): Promise<void> {
    try {
      this.spinOpen = true;

      if (this.hasEmptyFields()) {
        return;
      }

      if (this.passwordNotMatching()) {
        this.password.notValid = true;
        this.confirmPassword.notValid = true;
        this.error.push({field: "password", message: "The password don't match"});
        return;
      }

      if (this.invalidEmail(this.email.text)) {
        this.email.notValid = true;
        this.error.push({field: "email", message: "email is not valid"});
        return;
      }

      const {data} = await this.service.register(
        this.name.text.trim(),
        this.email.text.trim(),
        this.password.text.trim()
      )

      this.sessionService.create(
        data.id,
        data.token
      )
      this.sessionService.open();

      }finally {
      this.spinOpen = false;
    }
  }

  private invalidEmail(email: string): boolean {
    return email.trim().match(/.*[@].*[.].*/i) == null;
  }

  private passwordNotMatching(): boolean {
    return this.password.text !== this.confirmPassword.text;
  }

  private hasEmptyFields(): boolean
  {
    if (this.password.text.length == 0) {
      this.password.notValid = true;
      this.error.push({field: "password", message: "field password cannot be empty"});
    }

    if (this.name.text.length == 0) {
      this.name.notValid = true;
      this.error.push({field: "name", message: "field name cannot be empty"});
    }

    if (this.email.text.length == 0)  {
      this.email.notValid = true;
      this.error.push({field: "email", message: "field email cannot be empty"});
    }

    return this.password.notValid || this.email.notValid || this.name.notValid;
  }

  public gotoLogin(): void {
    this.router.navigate(['/']);
  }
}
