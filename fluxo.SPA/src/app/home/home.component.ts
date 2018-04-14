import { Component } from '@angular/core';
import * as iScroll from 'iscroll';
declare var $: JQuery;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  constructor() { }

  // tslint:disable-next-line:use-life-cycle-interface
  ngAfterViewInit() {
    this.setupParallax();
  }

  setupParallax() {
    const ua = navigator.userAgent,
          isMobileWebkit = /WebKit/.test(ua) && /Mobile/.test(ua);

    if (isMobileWebkit) {
      /*let iScrollInstance;
      $('html').addClass('mobile');

      iScrollInstance = new iScroll('#wrapper');

      $('#scroller').stellar({
        scrollProperty: 'transform',
        positionProperty: 'transform',
        horizontalScrolling: false
      });*/
    } else {
      $.stellar({
        horizontalScrolling: false
      });
    }
  }

}
