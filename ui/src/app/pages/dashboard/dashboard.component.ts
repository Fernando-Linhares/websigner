import {Component} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {SessionService} from "../../services/session.service";
import {ServicesProviderModule} from "../../services-provider/services-provider.module";
import {ModalComponent} from "../../components/modal/modal.component";
import {CertificateService} from "../../services/certificate.service";
import {DomSanitizer} from "@angular/platform-browser";
import {SignatureService} from "../../signature.service";
import {MatProgressSpinner} from "@angular/material/progress-spinner";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    ServicesProviderModule,
    FormsModule,
    NgForOf,
    NgIf,
    ModalComponent,
    NgClass,
    MatProgressSpinner
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

  public user: any;
  public folderOpen: boolean = false;
  public spinOpen: boolean = false;
  public certOpen: boolean = false;
  public certForm: any = {
    file:  { name: 'Add file (.pfx)' },
    alias: ''
  }
  public certificates: Array<any> = [];
  public pdfPreview: any = {src: null, file: null, name: null, open: false}
  public signOpen: boolean = false;
  public formSign =  {
    pin: {
      text: '',
      notValid: false,
      visible: false
    }
  };

  constructor(
    private sessionService: SessionService,
    private certService: CertificateService,
    private sanitizer: DomSanitizer,
    private signature: SignatureService
  ) {
  }

   ngOnInit(): void {
       this.fetchUserData();
       this.fetchCertificates();
  }

  private async fetchUserData(): Promise<void> {
    const {data} = await this.sessionService.me();
    this.user = data;
  }

  private async fetchCertificates(): Promise<void> {
    const { data } = await this.certService.list();
    this.certificates = data;
  }

  public openFolder(): void {
    this.folderOpen = true;
  }

  public openAddCert(): void {
    this.certOpen = true;
  }

  public closeFolder(): void {
    this.folderOpen = false;
  }

  public closeCert(): void {
    this.certOpen = false;
    this.certForm.file = null;
    this.certForm.alias = null;
  }

  public receiveCert(event: any): void {
    if(event.target?.files.length > 0) {
      this.certForm.file = event.target?.files[0];
      this.certForm.alias = event.target.files[0].name;
    }
  }

  public async removeCert(id: number): Promise<void> {
    await this.certService.remove(id);
    await this.fetchCertificates();
  }

  public async selectCert(id: number) {
    await this.certService.select(id);
    await this.fetchCertificates();
  }

  public async submitCert(): Promise<void> {
    try {
      this.spinOpen = true;
      const alias: string = this.certForm.alias;
      const file: File = this.certForm.file;
      await this.certService.addCert({
        file,
        alias
      });
      await this.fetchCertificates();
      this.closeCert();
    }
    finally {
      this.spinOpen = false;
    }
  }

  public logout(): void {
    this.sessionService.abort();
  }

  private pdfUrl(fileUrl: any){
    return this.sanitizer.bypassSecurityTrustResourceUrl(fileUrl);
  }

  public putPdfFile(event: any): void {
    if(event.target?.files.length > 0) {
      let file = event.target.files[0]
      let blob = new Blob([file], {type: file.type})
      let url = URL.createObjectURL(blob);
      this.pdfPreview.src = this.pdfUrl(url);
      this.pdfPreview.name = file.name;
      this.pdfPreview.file = file
      this.pdfPreview.open = true;
    }
  }

  public getCurrentCertificate(): any
  {
     for (let cert of this.certificates)
       if (cert.isActive)
         return cert;
  }

  public async submitSignature(): Promise<void> {
    try {
      this.spinOpen = true;
      let cert = this.getCurrentCertificate();
      let {file} = this.pdfPreview;
      let {pin} = this.formSign;

      let res = await this.signature.signPdf(pin.text, file, cert.id);
      let {url} = res.data;
      this.pdfPreview.src = this.pdfUrl(url);
      this.signOpen = false;
    }
    finally {
      this.spinOpen = false;
    }
  }

  public rollback(): void {
    this.pdfPreview.src = null;
    this.pdfPreview.name = null;
    this.pdfPreview.open = false;
  }
}
