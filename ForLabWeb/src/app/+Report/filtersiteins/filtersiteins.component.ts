import { Component, OnInit } from '@angular/core';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { Router } from '@angular/router';  
@Component({
  selector: 'app-filtersiteins',
  templateUrl: './filtersiteins.component.html'
})
export class FiltersiteinsComponent implements OnInit {
  RegionList: any[];
  CategoryList: any[];
  testingAreaList:any[];
  constructor(private _GlobalAPIService: GlobalAPIService,private _router:Router,private _APIwithActionService:APIwithActionService) {
    this.getCategory();
    this.getRegion();
    this.getTestingArea();
   }
   public model = {
    RegionId: 0,
    CategoryID: 0,
   Areaid:0
  };
  ngOnInit() {
  }
  getRegion() {
    let Countryid:any
    if (localStorage.getItem("role")=="admin")
    {
      Countryid=0
    }
    else
    {
      Countryid=localStorage.getItem("countryid")
    }
    this._APIwithActionService.getDatabyID(Countryid, 'Site', 'GetregionbyCountryID')
    .subscribe((data) => {
      this.RegionList = data

    }
    ), err => {
      console.log(err);
    }

}
getCategory() {
  this._GlobalAPIService.getDataList('SiteCategory').subscribe((data) => {
      this.CategoryList = data.aaData
      console.log(this.CategoryList)
  }
  ), err => {
      console.log(err);
  }

}
getTestingArea() {
  this._GlobalAPIService.getDataList('TestArea').subscribe((data) => {
      this.testingAreaList = data.aaData
      //console.log(this.Instrumentlist)
  }
  ), err => {
      console.log(err);
  }

}
openreport()
{
  console.log(this.model)
  this._router.navigate(["Report/viewsiteins",this.model.RegionId,this.model.CategoryID,this.model.Areaid])
}
}
