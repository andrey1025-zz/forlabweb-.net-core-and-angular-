import { Component, OnInit } from '@angular/core';
import { LayoutService } from "../layout.service";
import { Router } from "@angular/router";

declare var $: any;
@Component({
  selector: 'sa-ribbon',
  templateUrl: './ribbon.component.html',
  styleUrls: ['./ribbon.component.css']
})
export class RibbonComponent implements OnInit {

  constructor(private layoutService: LayoutService, private router: Router) { }

  ngOnInit() {
  }

  resetWidgets() {
    this.layoutService.factoryReset()
  }
  searchMobileActive = false;

  toggleSearchMobile() {
    this.searchMobileActive = !this.searchMobileActive;

    $('body').toggleClass('search-mobile', this.searchMobileActive);
  }

  onSubmit() {
    this.router.navigate(['/miscellaneous/search']);

  }
}
