import { Component, OnInit, ViewChild } from '@angular/core';
import { Http } from '@angular/http';
// import {DataTables} from 'angular-datatables';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';

@Component({
  selector: 'app-recent-forecast',
  templateUrl: './recent-forecast.component.html',
 styleUrls: ['./recent-forecast.component.css']
})
export class RecentForecastComponent implements OnInit {
 
  //dtOptions: Datatable.Settings = {};
  ForecastList=new Array();
  id: string;
  rowindex: number;
  constructor(public http: Http, 
    private _router: Router,
    private _APIwithActionService: APIwithActionService) { }

  ngOnInit() {
   
   this._APIwithActionService.getDatabyID(5,"Forecsatinfo","getrecentforecast").subscribe((data)=>{
    this.ForecastList=data;
console.log(data)
   })
  }
  ViewReport(data:any)
  {
    if (data.methodology == 'CONSUMPTION')
    this._router.navigate(["/Forecast/ForecastChartnew", data.id]);

  else if (data.methodology == 'SERVICE STATSTICS')
  
  this._router.navigate(["/Forecast/ForecastChartservice", data.id]);
  }
}
