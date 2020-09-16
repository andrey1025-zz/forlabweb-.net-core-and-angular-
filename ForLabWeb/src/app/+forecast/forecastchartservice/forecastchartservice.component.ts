
import { Component, OnInit, ElementRef, ViewChild, Renderer ,TemplateRef, Input, Output, EventEmitter} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as Highcharts from 'highcharts';
import { APIwithActionService } from "../../shared/APIwithAction.service"
import { GlobalAPIService } from  "../../shared/GlobalAPI.service";
import {NotificationService} from '../../shared/utils/notification.service';
@Component({
  selector: 'app-forecastchartservice',
  templateUrl: './forecastchartservice.component.html',
  styleUrls: ['./forecastchartservice.component.css']
})
export class ForecastchartserviceComponent implements OnInit {
  forecastid:number;
  durationlist= new Array();
  Name:String;
  Totalcost:Number=0;
  QCcost:Number=0;
  Ccost:Number=0;
  columnname= new Array();
  getdata= new Array();
  constructor(private notificationService:NotificationService,private _router: Router,private _fb: FormBuilder, private _rd: Renderer, private _GlobalAPIService: GlobalAPIService, private _avRoute: ActivatedRoute, private _APIwithActionService: APIwithActionService) { 


    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];


    }
  }

  ngOnInit() {

    this._APIwithActionService.getDatabyID( this.forecastid, 'Conductforecast', 'getcostparameter').subscribe((data)=>{
      this.Totalcost= data.totalcost


      this.QCcost= data.qccost 
      this.Ccost= data.cccost
    })
    this._APIwithActionService.getDatabyID( this.forecastid, 'Conductforecast', 'Getforecastsummarydurationforsiteservice').subscribe((resp) => {


      this._APIwithActionService.getDatabyID( this.forecastid , 'Conductforecast', 'Getdistinctdurationservice').subscribe((resp1) => {
      
        resp1.forEach(element => {
          this.durationlist.push(element.duration);
        });
    Highcharts.chart('container1', {
      chart: {
          type: 'area'
      },
      title: {
          text: 'FORECAST TREND BY TESTING AREA'
      },
   
      xAxis: {
          categories:this.durationlist,
          tickmarkPlacement: 'on',
          title: {
              enabled: false
          }
      },
      yAxis: {
       //   title: {
           //   text: 'Billions'
      //    },
          labels: {
              formatter: function () {
                  return this.value / 1000;
              }
          }
      },
      tooltip: {
          split: true,
          valueSuffix: ' millions'
      },
      plotOptions: {
          area: {
              stacking: 'normal',
              lineColor: '#666666',
              lineWidth: 1,
              marker: {
                  lineWidth: 1,
                  lineColor: '#666666'
              }
          }
      },
      series:resp
  });
})
})

this._APIwithActionService.getDatabyID(this.forecastid, 'Conductforecast', 'GettestingareacostratioNEW').subscribe((resp) => {
  Highcharts.chart('container2', {
    chart: {
      plotBackgroundColor: null,
      plotBorderWidth: null,
     // plotShadow: true,
      type: 'pie'
    },
    credits: {
      enabled: false
  },
    title: {
      text: 'COST BY TESTING AREA'
    },
    tooltip: {
      pointFormat: '{series.name}: <b>{point.y}</b>'
    },
    plotOptions: {
      pie: {
     
        allowPointSelect: true,
        cursor: 'pointer',
        // dataLabels: {
        //   enabled: true,
        // //  distance: '-50%',
        //   format: '<b>{point.name}</b>: {point.y} ',
        //   style: {
        //     color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
        //   }
        // },
        showInLegend: true
      }
    },
    series: [{

      // colorByPoint: true,
      data: resp
    }]
  });


});

this._APIwithActionService.getDatabyID(this.forecastid, 'Report', 'Getconsumptionsummarynew')
.subscribe((data4) => {
  console.log(data4)
  //this.controlArray = data4.header;
 // this.columnname = data4.column;

 data4.forEach(element => {
   if(element.price!=0)
   {
    this.getdata.push(element)
   }

   
 });

 console.log(this.getdata)
 // this.getdata = data4;
 // this.title=data4.title;
  //this.Period=data4.forecastperiod;
//this.Finalcost=data4.finalcost;
  //this.xx = false
this.Name="Product Type";

})




  }

  previousclick()
  {
    this.notificationService.smallBox({
      title: "Conformation!",
      content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/forecastcalculationmethod/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
      color: "#296191",
      //timeout: 8000,
      icon: "fa fa-bell swing animated"
    });
  // this._router.navigate(["/Forecast/forecastcalculationmethod", this.forecastid]);
  }
nextclick()
{
  this._router.navigate(["Report/viewforecastsummary",this.forecastid,'SERVICE STATSTICS'])
}


}
