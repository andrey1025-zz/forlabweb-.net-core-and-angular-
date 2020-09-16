import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router, ActivatedRoute } from '@angular/router';
import * as Highcharts from 'highcharts';
import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
@Component({
  selector: 'app-demographicchart',
  templateUrl: './demographicchart.component.html',
  styleUrls: ['./demographicchart.component.css']
})
export class DemographicchartComponent implements OnInit {
  forecastid:number;
  programid:number;
  producttype =new Array();
  productpriceData=new Array();
  chartOptions: Object;
  testArearatioData=new Array();
  nooftestData=new Array();
  tests= new Array();
  noofpatientData=new Array();
  months=new Array();
  Totalcost:Number=0;
  QCcost:Number=0;
  Ccost:Number=0;
  getdata=new Array();
  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) { }

  ngOnInit() {

    if (this._avRoute.snapshot.params["id"]) {  
      this.forecastid = this._avRoute.snapshot.params["id"];  
     
  
  }  
  if (this._avRoute.snapshot.params["pid"]) {  
    this.programid = this._avRoute.snapshot.params["pid"];  
   

  }
  this._APIwithActionService.getDatabyID( this.forecastid, 'Conductforecast', 'getdemocostparameter').subscribe((data)=>{
    this.Totalcost= data.totalcost


    this.QCcost= data.qccost 
    this.Ccost= data.cccost
  })
  this.getnoofpatient() ;
  }


  getproductprice()
  {
    this._APIwithActionService.getDatabyID(this.forecastid, 'Dasboard', 'GetChartProductprice').subscribe((data => {
      this.productpriceData = data;


 
    
      this._APIwithActionService.getDatabyID(this.forecastid,'Dasboard', 'getproducttype').subscribe((data) => {
          
          data.forEach(element => {
            this.producttype.push(element);
        });
         
        Highcharts.chart('container1', {
          chart: {
              type: 'area'
          },
          title: {
              text: 'FORECAST TREND BY PRODUCT TYPE'
          },
       
          xAxis: {
              categories: this.producttype,
              tickmarkPlacement: 'on',
              title: {
                  enabled: false
              }
          },
          yAxis: {
            //  title: {
                 // text: 'Billions'
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
          series:this.productpriceData
      });
          // this.chartOptions = Highcharts.chart('container1', {
          //     chart: {
          //        // plotShadow:true,
          //         type: 'column'
          //     },
          //     credits: {
          //       enabled: false
          //   },
          //     title: {
          //         text: 'Cost of Product by Site',
          //         style: {
          //           fontSize: '14px'
          //       }
          //     },
          //     xAxis: {
          //         categories: this.producttype,
          //         title: {
          //             text: 'Product Type'
          //         }
          //     },
          //     yAxis: {
          //         min: 0,
          //         title: {
          //             text: 'Cost'
          //         }
          //     },
          //     tooltip: {
          //         pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
          //         shared: true
          //     },
          //     plotOptions: {
          //         column: {
          //             stacking: 'number'
          //         }
          //     },
          //     series:  this.productpriceData
          // });




          this._APIwithActionService.getDatabyID(this.forecastid, 'Conductforecast', 'GetdemoProducttypecostratioNEW').subscribe((resp) => {
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
                text: 'COST BY PRODUCT TYPE'
              },
              tooltip: {
                pointFormat: '{series.name}: <b>{point.y}</b>'
              },
              plotOptions: {
                pie: {
                  allowPointSelect: true,
                  cursor: 'pointer',
                  dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.y} ',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                },
                  showInLegend: true
                }
              },
              series: [{
          
                 colorByPoint: true,
                data: resp
              }]
            });
          
          
          });
          


          this._APIwithActionService.getDatabyID(this.forecastid, 'Report', 'Getdemographicsummary')
.subscribe((data4) => {
  console.log(data4.data);
  
  //this.controlArray = data4.header;
 // this.columnname = data4.column;
 this.getdata = data4.data;


})
      })

  }
  ), err => {
      console.log(err);
  })


  
  }
 
  getnooftest() {
    this._APIwithActionService.getDatabyID(this.forecastid, 'Dasboard', 'GetChartNooftest').subscribe((data => {
      this.nooftestData = data;
      this.getratio();
    
    
      this._APIwithActionService.getDatabyID(this.forecastid,'Dasboard', 'gettstname').subscribe((data) => {
          
          data.forEach(element => {
            this.tests.push(element);
        });
      


      })

  }
  ), err => {
      console.log(err);
  })

    


     
  }
  getnoofpatient() {

    this._APIwithActionService.getDatabyID(this.forecastid,'Dasboard', 'Getnoofpatientpermonth').subscribe((data) => {
    
      this.noofpatientData = data;
     
   
      this._APIwithActionService.getDatabyID(this.forecastid,'Dasboard', 'Getmonthbyforecast').subscribe((data) => {
          
          data.forEach(element => {
            this.months.push(element.columnname);
        });
       
          
          this.getnooftest();
      })

  }
  ), err => {
      console.log(err);
  }


    
  
  }

  getratio()
  {
      this._APIwithActionService.getDatabyID(this.forecastid,'Dasboard', 'Getratiobytestarea').subscribe((data) => {
          this.testArearatioData = data;

          this.getproductprice();
       
      })
  }


  nextclick()
{
  this._router.navigate(["Report/viewforecastsummary",this.forecastid,'Demographic'])
}
}
