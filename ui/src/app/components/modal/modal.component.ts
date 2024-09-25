import {Component, EventEmitter, Input} from '@angular/core';
import {NgIf} from "@angular/common";

@Component({
  selector: 'modal-component',
  standalone: true,
  imports: [
    NgIf
  ],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.css'
})
export class ModalComponent {
  @Input() isOpen: boolean = false;
}
