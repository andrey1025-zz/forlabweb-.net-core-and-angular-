import { Component, OnInit } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { Router,ActivatedRoute } from '@angular/router'; 
@Component({
  selector: 'app-filterforecastcomparision',
  templateUrl: './filterforecastcomparision.component.html'
})
export class FilterforecastcomparisionComponent implements OnInit {

  consumptionlist=new Array();
  servicelist=new Array();
  demographiclist=new Array();
  forecastid:any=0;
  forecastid1:any=0;
  forecastid2:any=0;
  type:string="";
  show:boolean=false;
  constructor(private _avRoute: ActivatedRoute,private _APIwithActionService: APIwithActionService,private _router:Router) {

    if (this._avRoute.snapshot.params["type"]) 
    {
      this.type = this._avRoute.snapshot.params["type"];
    }
    if (this.type=="com")
    {
      this.show=true
    }
   this.getforecastbymethod("CONSUMPTION");
    this.getforecastbymethod("SERVICE_STATISTIC");
    this.getforecastbymethod("MORBIDITY");
   }

  ngOnInit() {

  
  }
  getforecastbymethod(value: any)
   {

      this._APIwithActionService.getDatabyID(value, "MMProgram", "GetForecastInfoByMethodology","CID="+localStorage.getItem("countryid")).subscribe((data) => {
      if(value=="CONSUMPTION")
      {
        this.consumptionlist=data;
      }
      else if(value=="SERVICE_STATISTIC")
      {
        this.servicelist=data;
      }
      else
      {
        this.demographiclist=data;
      }
        
      })
  
  }

  openreport()
{
  if (this.type=="com")
  {
  this._router.navigate(["Report/viewforecastcomparision",this.forecastid+","+this.forecastid1+","+this.forecastid2])
  }
  else
  {
    this._router.navigate(["Report/viewforecastpatientsummary",this.forecastid2])
  }
 
}
}
