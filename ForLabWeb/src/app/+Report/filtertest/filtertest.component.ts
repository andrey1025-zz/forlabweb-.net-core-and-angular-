import { Component, OnInit } from '@angular/core';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router } from '@angular/router';  

@Component({
  selector: 'app-filtertest',
  templateUrl: './filtertest.component.html'
})
export class FiltertestComponent implements OnInit {
  testingAreaList=new Array();
  Areaid:number=0;
  constructor(private _GlobalAPIService:GlobalAPIService,private _router:Router) { 
   
    this.getTestingArea();
  }
  
  ngOnInit() {
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
  
  this._router.navigate(["Report/viewtest",this.Areaid])
  }

}
