import { Component, OnInit, ViewChild } from '@angular/core';
import { LoginInfoComponent } from "../../user/login-info/login-info.component";
import { APIwithActionService } from "../../APIwithAction.service"
import { DemograhicListComponent } from "../../../+Demographic/DemographicList/DemographicList.component"
import { Router } from '@angular/router';
import { GlobalVariable } from '../../shared/globalclass';
@Component({

  selector: 'sa-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {
  @ViewChild(DemograhicListComponent) list: DemograhicListComponent
  Demographicmenu = new Array()
  GlobalAdmin: boolean = false;
  constructor(private _APIwithActionService: APIwithActionService, private _route: Router) {

    if (localStorage.getItem("role") == "admin") {
      this.GlobalAdmin = true
    }
  }

  ngOnInit() {

  }


  navigatedemo(id) {
    this._route.navigateByUrl('/Managedata', { skipLocationChange: true }).then(() =>
      this._route.navigate(["/Demographic/DemographicList", id]));
  }
  openmange(tab) {
    this._route.navigate(["/Managedata/ManagedataList", tab]);
  }
}
