import { Component, OnInit  } from '@angular/core';  


import { Router } from '@angular/router';  

import { GlobalAPIService } from '../../shared/GlobalAPI.service';  


@Component({
  selector: 'app-regionreport',
  templateUrl: './regionlist.component.html',
 
})
export class RegionreportComponent implements OnInit {
viewreport:boolean=false;
Show:boolean=false;
logic:string=">";
options: Object;
regionlist=new Array();
url1:string="";
  constructor(
    private _GlobalAPIService: GlobalAPIService, private _router: Router) { 

// console.log(this.logic);
// this.url1="http://localhost:53234/api/Report/Getregionlist/4?logic="+this.logic;

    }
public noofsites=0
  ngOnInit() {
    
   this._GlobalAPIService.getDataList('Region').subscribe((data)=>{
     this.regionlist=data;
   })
  }
 
  onchecked(ischecked:boolean)
  {
    if (ischecked==true)
    {
      this.Show=true;
    }
    else
    {
      this.Show=false;
    }
  }
  Onchange(args)
  {
    this.logic=args.target.options[args.target.selectedIndex].text
   
  }
  openreport()
  {
    console.log(this.noofsites);
    this._router.navigate(["Report/viewreport",this.noofsites,this.logic,this.Show])

  }
}
