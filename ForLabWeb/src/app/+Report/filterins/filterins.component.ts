import { Component, OnInit } from '@angular/core';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router } from '@angular/router';  

@Component({
  selector: 'app-filterins',
  templateUrl: './filterins.component.html'
})
export class FilterinsComponent implements OnInit {


  testingAreaList:any[];
  constructor(private _GlobalAPIService: GlobalAPIService,private _router:Router) {

    this.getTestingArea();
   }
   public model = {
   
   Areaid:0,
   testingdays:false
  };
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
  console.log(this.model)
  this._router.navigate(["Report/viewins",this.model.Areaid,this.model.testingdays])
}

}
