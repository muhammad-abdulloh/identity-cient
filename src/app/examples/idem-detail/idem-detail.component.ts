import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-idem-detail',
  templateUrl: './idem-detail.component.html',
  styleUrl: './idem-detail.component.scss'
})
export class IdemDetailComponent {

  @Input() item = '';
  @Input() itemTwo = '';
}
