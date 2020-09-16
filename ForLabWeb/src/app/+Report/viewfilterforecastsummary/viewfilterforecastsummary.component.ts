import { Component, OnInit, ViewChild } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { ActivatedRoute } from '@angular/router';
import { DatatableComponent } from "../../shared/ui/datatable/datatable.component";

@Component({
  selector: 'app-viewfilterforecastsummary',
  templateUrl: './viewfilterforecastsummary.component.html'
})
export class ViewfilterforecastsummaryComponent implements OnInit {

  controlArray = new Array();
  xx: boolean = false;
  columnname = new Array();
  getdata = new Array();
  id: number;
  title:string="";
  Period:string="";
  Finalcost:string="";
  Name:string="Product Type";
  method: string;
  @ViewChild(DatatableComponent) DataView: DatatableComponent

  constructor(private _APIwithActionService: APIwithActionService, private _avRoute: ActivatedRoute) {



    if (this._avRoute.snapshot.params["id"]) {
      this.id = this._avRoute.snapshot.params["id"];
    }
    if (this._avRoute.snapshot.params["method"]) {
      this.method = this._avRoute.snapshot.params["method"];
    }





  }

  ngOnInit() {
    if (this.method=="CONSUMPTION")
    {
    this._APIwithActionService.getDatabyID(this.id, 'Report', 'Getconsumptionsummary')
      .subscribe((data4) => {
        console.log(data4)
        this.controlArray = data4.header;
        this.columnname = data4.column;
        this.getdata = data4.data;
        this.title=data4.title;
        this.Period=data4.forecastperiod;
this.Finalcost=data4.finalcost;
        this.xx = false
this.Name="Product Type";

      })
    }
      else if(this.method=="SERVICE STATSTICS")
      {
        this._APIwithActionService.getDatabyID(this.id, 'Report', 'Getconsumptionsummary')
        .subscribe((data4) => {
          console.log(data4)
          this.controlArray = data4.header;
          this.columnname = data4.column;
          this.getdata = data4.data;
          this.title=data4.title;
          this.Period=data4.forecastperiod;
  this.Finalcost=data4.finalcost;
          this.xx = false
  
          this.Name="Product Type";
        })
      }
      else 
      {
        this._APIwithActionService.getDatabyID(this.id, 'Report', 'Getdemographicsummary')
        .subscribe((data4) => {
          console.log(data4)
          this.controlArray = data4.header;
          this.columnname = data4.column;
          this.getdata = data4.data;
          this.title=data4.title;
          this.Period=data4.forecastperiod;
  this.Finalcost=data4.finalcost;
          this.xx = true
          this.Name="SiteCategory";
  
        })
      }
  }

  Print(): void {
    let printContents, popupWin;
    printContents = document.getElementById('print-section').innerHTML;
    popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.open();
    popupWin.document.write(`
      <html>
        <head>
         
          
        </head>
    <body onload="window.print();window.close()">${printContents}</body>
      </html>`
    );
    popupWin.document.close();
}
}
