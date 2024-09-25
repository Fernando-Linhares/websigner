import { Component } from '@angular/core';
import {FormsModule} from "@angular/forms";
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {Router} from "@angular/router";
import {SignInService} from "../../services/sign-in.service";
import {ServicesProviderModule} from "../../services-provider/services-provider.module";
import {SessionService} from "../../services/session.service";
import {MatProgressSpinner} from "@angular/material/progress-spinner";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    ServicesProviderModule,
    NgClass,
    NgIf,
    NgForOf,
    MatProgressSpinner
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  password = { text: 'abcd1234', visible: false, notValid: false };
  login = { text: 'nando2@mail.com.br', notValid: false};
  error: Array<any> = [];
  spinOpen: boolean = false;

  constructor(
    private router: Router,
    private signInservice: SignInService,
    private sessionService: SessionService
    ) {
  }

   ngOnInit(): void {
    if(this.sessionService.user()) {
      this.sessionService.gotoDashboard()
    }
   }

  public onSubmit(): void {
    try {
      this.spinOpen = true;

      if (this.hasEmptyFields()) {
        return;
      }

      if (this.invalidEmail(this.login.text)) {
        this.login.notValid = true;
        this.error.push({field: "login", message: "email is not valid"});
        return;
      }

      this.signInservice.login(
        this.login.text.trim(),
        this.password.text,
      )
        .subscribe({
          next: response => this.sessionOpen(response.data),
          error: console.error,
        })

    }finally {
      this.spinOpen = false;
    }
  }

  private sessionOpen(data: any): void {
    this.sessionService.create(
      data.id,
      data.token
    )
    this.sessionService.open();
  }

  private hasEmptyFields(): boolean
  {
    if (this.password.text.length == 0) {
      this.password.notValid = true;
      this.error.push({field: "password", message: "field password cannot be empty"});
    }

    if (this.login.text.length == 0) {
      this.login.notValid = true;
      this.error.push({field: "login", message: "field login cannot be empty"});
    }

    return this.password.notValid || this.login.notValid;
  }

  private invalidEmail(email: string): boolean {
    return email.trim().match(/.*[@].*[.].*/i) == null;
  }

  public gotoRegister()  {
    this.router.navigate(['/register']);
  }
}
