import { Component, OnInit } from '@angular/core';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router } from '@angular/router';  

@Component({
  selector: 'app-filterproductusage',
  templateUrl: './filterproductusage.component.html'
})
export class FilterproductusageComponent implements OnInit{ ProductTypeList=new Array();

testingAreaList=new Array();
constructor(private _GlobalAPIService:GlobalAPIService,private _router:Router) { 
  this.getproducttype();
  this.getTestingArea();
}
public model={
  Areaid:0,
  typeId:0
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
getproducttype() {  
  this._GlobalAPIService.getDataList('ProductType').subscribe( (data) => {this.ProductTypeList = data.aaData
console.log(this.ProductTypeList) }
  )   ,err=>{  
      console.log(err);  
    }  
 
}  
openreport()
{

this._router.navigate(["Report/viewproductusage",this.model.Areaid,this.model.typeId])
}

}
