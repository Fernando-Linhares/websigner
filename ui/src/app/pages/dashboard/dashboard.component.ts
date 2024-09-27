import {Component} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {DatePipe, NgClass, NgForOf, NgIf, SlicePipe} from "@angular/common";
import {SessionService} from "../../services/session.service";
import {ServicesProviderModule} from "../../services-provider/services-provider.module";
import {ModalComponent} from "../../components/modal/modal.component";
import {CertificateService} from "../../services/certificate.service";
import {DomSanitizer} from "@angular/platform-browser";
import {SignatureService} from "../../services/signature.service";
import {MatProgressSpinner} from "@angular/material/progress-spinner";
import {FilesService} from "../../services/files.service";
import {environment} from "../../../environments/environment";

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
    MatProgressSpinner,
    DatePipe,
    SlicePipe
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
  public files: Array<any> = [];
  public pdfPreview: any = {src: null, file: null, name: null, open: false}
  public signOpen: boolean = false;
  public formSign =  {
    pin: {
      text: '',
      notValid: false,
      visible: false
    }
  };
  public viewSignedPdf: any = {
    open: false,
    src: null,
    name: null
  }

  constructor(
    private sessionService: SessionService,
    private certService: CertificateService,
    private sanitizer: DomSanitizer,
    private signature: SignatureService,
    private filesService: FilesService
  ) {
  }

   ngOnInit(): void {
       this.fetchUserData();
       this.fetchCertificates();
       this.fetchFiles();
  }

  private async fetchUserData(): Promise<void> {
    try {
    const {data} = await this.sessionService.me();
    this.user = data;
    }catch (error) {
      this.logout();
    }
  }

  private async fetchFiles(): Promise<void> {
    const {data} = await this.filesService.list();
    this.files = data;
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
      await this.fetchFiles()
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

  public viewPdfSigned(file: any): void {
    this.viewSignedPdf.name = file.fileName;
    this.viewSignedPdf.open = true;
    this.viewSignedPdf.src = this.pdfUrl(environment.baseUrl + '/pdfs/' + file.fileName);
  }
}
