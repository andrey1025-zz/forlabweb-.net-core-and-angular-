import { Component } from '@angular/core';

import { LayoutService } from '../layout.service';

declare var $: any;

@Component({
  selector: 'sa-minify-menu',
  template: `<span class="btn-minify" (click)="toggle()">
    <img src="assets/img/new/btn_minify.png" alt="btn_minify">
  </span>`,
  styles: ['.btn-minify { display: block; position: absolute; right: 0; cursor: pointer; }']
})
export class MinifyMenuComponent {

  constructor(private layoutService: LayoutService) {
  }

  toggle() {
    this.layoutService.onMinifyMenu()
  }
}
