import { Component, OnInit } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-viewpatientsummary',
  templateUrl: './viewpatientsummary.component.html'
})
export class ViewpatientsummaryComponent implements OnInit {
  id: number;
  comparisionsummary=new Array();
  forecastdescription=new Array();
  comparisiondata=new Array();
  header=new Array();
  controlArray=new Array();
  constructor(private _APIwithActionService: APIwithActionService, private _avRoute: ActivatedRoute) {

    if (this._avRoute.snapshot.params["id"]) 
    {
      this.id = this._avRoute.snapshot.params["id"];
    }

    this._APIwithActionService.getDatabyID(this.id, 'Report', 'Getnoofpatientsummary').subscribe((res) => {
   
   this.comparisiondata=res.data;
   this.header =res.header;
   this.controlArray=res.column;
   console.log(this.controlArray);
   console.log(this.comparisiondata);
    })
this._APIwithActionService.getDatabyID(this.id,'Report','GetForecastdescription').subscribe((res)=>{

  this.forecastdescription=res;
})
   }

  ngOnInit() {
  }
  Print(): void {
    let printContents, popupWin;
    printContents = document.getElementById('print-section').innerHTML;
    popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.open();
    popupWin.document.write(`
      <html>
        <head>
          <title>Demographic No. of Patient Summary</title>
          
        </head>
    <body onload="window.print();window.close()">${printContents}</body>
      </html>`
    );
    popupWin.document.close();
}
}
