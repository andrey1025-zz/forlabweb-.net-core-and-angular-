import { Component, OnInit } from '@angular/core';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router } from '@angular/router';  
@Component({
  selector: 'app-filterproduct',
  templateUrl: './filterproduct.component.html'
})
export class FilterproductComponent implements OnInit {
  ProductTypeList=new Array();
  typeId:number=0;
  constructor(private _GlobalAPIService:GlobalAPIService,private _router:Router) { 
    this.getproducttype();
  }

  ngOnInit() {
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
 
  this._router.navigate(["Report/viewproduct",this.typeId])
}
}
