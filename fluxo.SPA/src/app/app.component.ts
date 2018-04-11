import { Component } from '@angular/core';
import * as alertify from 'alertify.js';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'fluxo';

  constructor() {
    alertify.logPosition('bottom right');
  }
}
