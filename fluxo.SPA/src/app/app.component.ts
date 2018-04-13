import { Component } from '@angular/core';
import { Meta } from '@angular/platform-browser';
import * as alertify from 'alertify.js';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'fluxo';

  constructor(private meta: Meta) {
    this.meta.updateTag({ name: 'viewport', content: 'width=device-width, initial-scale=1, maximum-scale=1' });
    alertify.logPosition('bottom right');
  }
}
