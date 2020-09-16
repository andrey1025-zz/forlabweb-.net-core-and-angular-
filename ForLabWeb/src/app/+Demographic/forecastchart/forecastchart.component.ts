import { Component, OnInit, Input } from '@angular/core';
import * as Highcharts from 'highcharts';
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
import { Router, ActivatedRoute } from '@angular/router';
//import { DemographicwizardComponent } from '../demographicwizard/demographicwizard.component';
@Component({
  selector: 'app-forecastchart',
  template: `
  <div id="content" >
  <div class="row">
  <div class="col-md-6">
  <div id="container" style="min-width: 310px; height: 400px; margin: 0 auto">
  </div></div>
  <div class="col-md-6">
  <div id="container1" style="min-width: 310px; height: 400px; margin: 0 auto">
  </div></div>
  </div>

  </div>
  <div id="content" >
  <div class="row">
  <div class="col-md-6">
  <div id="container3" style="min-width: 310px; height: 400px; margin: 0 auto">
  </div></div>
  <div class="col-md-6">
  <div id="container4" style="min-width: 310px; height: 400px; margin: 0 auto">
  </div></div>
  </div> </div>`,

})
export class ForecastchartComponent implements OnInit {
  chartOptions: Object;
  Dashboard1 = new Array();
  Regionlist = new Array();
  region = new Array();
  months= new Array();
  tests =new Array();
  producttype =new Array();
  //  Highcharts1 = Highchart
  forecastid: number = 0;
  istestnocal:boolean=false;
  productpriceData: Array<string> = [];
  nooftestData: Array<string> = [];
  noofpatientData: Array<string> = [];
  testArearatioData: Array<string> = [];
  @Input() RecforecastID:number;
  constructor(private _avRoute: ActivatedRoute, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) {


   
    
   
  }

  ngOnInit() {
    if (this._avRoute.snapshot.params["id2"]) {
        this.forecastid = this._avRoute.snapshot.params["id2"];
  
      }
      if (this.RecforecastID>0)
      {
            this.forecastid = this.RecforecastID;
      }
     if (this.forecastid>0)
     {
      this.getnoofpatient();
     }
  }
  getproductprice()
  {
    this._APIwithActionService.getDatabyID(this.forecastid, 'Dasboard', 'GetChartProductprice').subscribe((data => {
      this.productpriceData = data;


 
    
      this._APIwithActionService.getDatabyID(this.forecastid,'Dasboard', 'getproducttype').subscribe((data) => {
          
          data.forEach(element => {
            this.producttype.push(element);
        });
         

          this.chartOptions = Highcharts.chart('container4', {
              chart: {
                 // plotShadow:true,
                  type: 'column'
              },
              credits: {
                enabled: false
            },
              title: {
                  text: 'Cost of Product by Site',
                  style: {
                    fontSize: '14px'
                }
              },
              xAxis: {
                  categories: this.producttype,
                  title: {
                      text: 'Product Type'
                  }
              },
              yAxis: {
                  min: 0,
                  title: {
                      text: 'Cost'
                  }
              },
              tooltip: {
                  pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
                  shared: true
              },
              plotOptions: {
                  column: {
                      stacking: 'number'
                  }
              },
              series:  this.productpriceData
          });

      })

  }
  ), err => {
      console.log(err);
  })


    // this._APIwithActionService.getDatabyID(this.forecastid, 'Dasboard', 'GetChartProductprice').subscribe((data => {
    //   this.productpriceData = data;


    //   Highcharts.chart('container4', {
    //     chart: {
    //       plotShadow: true,
    //       type: 'column'
    //     },
    //     title: {
    //       text: 'Cost of Product by Product Type'
    //     },

    //     xAxis: {
    //       type: 'category',
    //       title: {
    //         text: 'Product Type'
    //       }
    //     },
    //     yAxis: {
    //       title: {
    //         text: 'Cost'
    //       }

    //     },
    //     legend: {
    //       enabled: false
    //     },
    //     plotOptions: {
    //       series: {
    //         borderWidth: 0,
    //         dataLabels: {
    //           enabled: true,
    //           // format: '{point.y:.1f}'
    //         }
    //       }
    //     },

    //     tooltip: {

    //       pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y}</b><br/>'
    //     },

    //     series: [
    //       {

    //         "colorByPoint": true,
    //         "data": this.productpriceData
    //       }
    //     ],

    //   });
    // }))
  }
  getnooftest() {
    this._APIwithActionService.getDatabyID(this.forecastid, 'Dasboard', 'GetChartNooftest').subscribe((data => {
      this.nooftestData = data;
      this.getratio();
    
    
      this._APIwithActionService.getDatabyID(this.forecastid,'Dasboard', 'gettstname').subscribe((data) => {
          
          data.forEach(element => {
            this.tests.push(element);
        });
      

          this.chartOptions = Highcharts.chart('container1', {
              chart: {
                //  plotShadow:true,
                  type: 'column'
              },
              credits: {
                enabled: false
            },
              title: {
                  text: 'No. of Tests Per Site',
                  style: {
                    fontSize: '14px'
                }
              },
              xAxis: {
                  categories: this.tests,
                  title: {
                      text: 'Test'
                  }
              },
              yAxis: {
                  min: 0,
                  title: {
                      text: 'No. of Tests'
                  }
              },
              tooltip: {
                  pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
                  shared: true
              },
              plotOptions: {
                  column: {
                      stacking: 'number'
                  }
              },
              series: this.nooftestData
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
       
          this.chartOptions = Highcharts.chart('container', {
              chart: {
               //   plotShadow:true,
                  type: 'column'
              },
              credits: {
                enabled: false
            },
              title: {
                  text: 'No. of Patient Per Month',
                  style: {
                    fontSize: '14px'
                }
              },
              xAxis: {
                  categories: this.months,
                  title: {
                      text: 'Months'
                  }
              },
              yAxis: {
                  min: 0,
                  title: {
                      text: 'No. of Patient'
                  }
              },
              tooltip: {
                  pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
                  shared: true
              },
              plotOptions: {
                  column: {
                      stacking: 'number'
                  }
              },
              series: this.noofpatientData
          });
          this.getnooftest();
      })

  }
  ), err => {
      console.log(err);
  }


    
    // this._APIwithActionService.getDatabyID(this.forecastid, 'Dasboard', 'GetChartPatient').subscribe((data => {
    //   this.noofpatientData = data;


    //   Highcharts.chart('container', {
    //     chart: {
    //       plotShadow: true,
    //       type: 'column'
    //     },
    //     title: {
    //       text: 'Patient number over forecast'
    //     },

    //     xAxis: {
    //       type: 'category',
    //       title: {
    //         text: 'Month'
    //       }
    //     },
    //     yAxis: {
    //       title: {
    //         text: 'No. of patient'
    //       }

    //     },
    //     legend: {
    //       enabled: false
    //     },
    //     plotOptions: {
    //       series: {
    //         borderWidth: 0,
    //         dataLabels: {
    //           enabled: true,
    //           // format: '{point.y:.1f}'
    //         }
    //       }
    //     },

    //     tooltip: {

    //       pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y}</b><br/>'
    //     },

    //     series: [
    //       {

           
    //         "data": this.noofpatientData
    //       }
    //     ],

    //   });
    // }))
  }

  getratio()
  {
      this._APIwithActionService.getDatabyID(this.forecastid,'Dasboard', 'Getratiobytestarea').subscribe((data => {
          this.testArearatioData = data;

          this.getproductprice();
          Highcharts.chart('container3', {
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
                  text: 'Ratio of forecasted test by testing Area',
                  style: {
                    fontSize: '14px'
                }
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
               
                 // colorByPoint: true,
                  data:this.testArearatioData
              }]
          });
      }))
  }
}
