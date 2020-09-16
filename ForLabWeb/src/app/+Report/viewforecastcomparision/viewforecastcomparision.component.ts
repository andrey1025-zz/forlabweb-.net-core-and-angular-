import { Component, OnInit } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { ActivatedRoute } from '@angular/router';
import * as Highcharts from 'highcharts';
@Component({
  selector: 'app-viewforecastcomparision',
  template: `<div id="content">
  <sa-widgets-grid>


    <!-- START ROW -->

    <div class="row">

      <!-- NEW COL START -->
      <article class="col-sm-12 col-md-12 col-lg-9">
<div sa-widget [editbutton]="false" [custombutton]="false" [deletebutton]="false">

  <header>
    <span class="widget-icon"> <i class="fa fa-edit"></i> </span>

   

  </header>

  <!-- widget div-->
  <div>


    <!-- widget content -->
    <div class="widget-body no-padding">

    

   

    <form  class="smart-form"  >
      <header>
      <div class="row">
      <div class="col-md-6" style="
  margin-left: 10px;
">
      Forecast comparision Summary
      </div>
      <div class="col-md-5" style="
  text-align: right;
"><button class="btn btn-primary btn-sm" type="button" (click)="Print()">Print</button></div>
  </div>
      </header>
    
      <fieldset id="print-section">
    
        <section>
        <div id="container" >
        </div>
      </section>
      <section> 
      <div class="row" id="content">
      <div class="table-responsive" style="overflow-y: auto;border-style:ridge">



          <table class="table table-bordered table-hover">

              <thead>
                  <tr>
                      <th>Methodology</th>
                      <th>Product Type</th>
                      <th>Cost</th>
                     
                  </tr>
              </thead>

              <tbody>
                  <tr *ngFor="let item of comparisiondata; let i = index;" >
                      <td >
                      {{item.methodology}}
                      </td>
                      <td>
                          {{item.productType}}
                      </td>
                      <td >
                          {{item.cost}}
                      </td>
                  </tr>
              </tbody>
          </table>


      </div>
  </div>
      
      
      </section>
      </fieldset>

      
    
    </form> </div>
    <!-- end widget content -->

  </div>
  <!-- end widget div -->

</div>
</article>
    </div>
</sa-widgets-grid>
</div>`
})
export class ViewforecastcomparisionComponent implements OnInit {
  id: number;
  comparisionsummary=new Array();
  comparisiondata=new Array();
  constructor(private _APIwithActionService: APIwithActionService, private _avRoute: ActivatedRoute) 
  {


    if (this._avRoute.snapshot.params["id"]) 
    {
      this.id = this._avRoute.snapshot.params["id"];
    }

    this._APIwithActionService.getDatabyID(this.id, 'Report', 'Getforecastcomparision').subscribe((data) => {
   
      this.comparisiondata=data;
   
   
    })


   }

  ngOnInit() {
    this._APIwithActionService.getDatabyID(this.id, 'Dasboard', 'Getforecastcomparision').subscribe((data => {
      this.comparisionsummary = data;

    Highcharts.chart('container', {
      chart: {
        plotShadow:true,
          type: 'column'
          
      },
      credits: {
        enabled: false
    },
      title: {
        text: 'Comparision Chart'
      },
      xAxis: {
          categories: [
              'Consumption',
              'Service Statistic',
              'Demographic'
             
          ],
          crosshair: true
      },
      yAxis: {
          min: 0,
          title: {
              text: 'Cost'
          }
      },
      tooltip: {
          headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
          pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
              '<td style="padding:0"><b>{point.y}$</b></td></tr>',
          footerFormat: '</table>',
          shared: true,
          useHTML: true
      },
      plotOptions: {
          column: {
              pointPadding: 0.2,
              borderWidth: 0
          }
      },
      series: this.comparisionsummary
                 
       
  })
}))
  }
 Print(): void {
    let printContents, popupWin;
    printContents = document.getElementById('print-section').innerHTML;
    popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.open();
    popupWin.document.write(`
      <html>
        <head>
          <title>Forecast Comparision</title>
          
        </head>
    <body onload="window.print();window.close()">${printContents}</body>
      </html>`
    );
    popupWin.document.close();
}
}
