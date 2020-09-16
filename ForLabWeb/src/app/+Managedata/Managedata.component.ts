import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";
import { APIwithActionService } from "../shared/APIwithAction.service";
import { GlobalAPIService } from "../shared/GlobalAPI.service";
declare var $: any;

@Component({
  selector: 'app-managedata',
  templateUrl: './Managedata.component.html',
  styleUrls: ['./Managedata.component.css']
})
export class managedatacomponent {
  activeTab: number = 0;
  totalcount = new Array();
  public state: any = {
    tabs: {
      demo1: 0
    }
  }
  constructor(private _APIwithActionService: APIwithActionService, private _Location: Location, private _avRoute: ActivatedRoute,
    private _GlobalAPIService: GlobalAPIService, private _router: Router, private _location: Location) {
    if (this._avRoute.snapshot.params["tab"]) {
      console.log('this.avRoute', this._avRoute.snapshot)
      this.state.tabs.demo1 = this._avRoute.snapshot.params["tab"]
    }
    this._APIwithActionService.getDataList('Test', 'Gettotalcount').subscribe((data) => {
      this.totalcount = data;
    })
  }

  onAdjustColumn() {
    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
  }

}