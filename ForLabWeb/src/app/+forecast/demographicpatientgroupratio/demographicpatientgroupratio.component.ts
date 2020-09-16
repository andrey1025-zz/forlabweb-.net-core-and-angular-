import { Component, OnInit, Output, EventEmitter, Input,AfterViewInit } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router, ActivatedRoute } from '@angular/router';

import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
import * as Highcharts from 'highcharts';
@Component({
  selector: 'app-demographicpatientgroupratio',
  template: `<div id="content">
  <sa-widgets-grid>


    <!-- START ROW -->

    <div class="row">

      <!-- NEW COL START -->
      <article class="col-sm-12 col-md-12 col-lg-12">
        <div sa-widget [editbutton]="false" [custombutton]="false" [deletebutton]="false">

          <header>
            <span class="widget-icon">
              <i class="fa fa-edit"></i>
            </span>



          </header>

          <!-- widget div-->
          <div>


            <!-- widget content -->
            <div class="widget-body no-padding">
              <form class="smart-form" novalidate="novalidate"  [formGroup]="demographicpatientgroup">
                <!-- [formGroup]="RecentForecast" -->
                <header>
                  <h3 style="
                color: gray;
            ">
                 WHAT % OF THE PATIENTS IN THIS FORECAST ARE IN EACH OF THESE GROUP 
                  </h3>
                </header>

                <fieldset>
<div class="row">
                  <section class="col col-6">
                 
                    <div class="table-responsive"  style="overflow-y: auto;height: 318px;border-style: ridge;margin-top: 26px" formArrayName="_patientgroup">
      
      
      
                        <table class="table table-bordered table-striped table-hover">
      
                            <thead>
                                <tr>
                                    <th>Patient Group </th>
                                
                                    <th >% of these Group</th>
                                    <th  style="display: none;" >Calculated Value From Target Patient</th>                            
                                    <th style="display: none;" >Remove</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr  *ngFor="let item of demographicpatientgroup.get('_patientgroup')['controls']; let i = index;" [formGroupName]="i" >
        
                                    <td  >
                                        <label class="input">
                                        {{item.getRawValue().PatientGroupName}}
                                            
                                        </label>
                                    </td>
                                
                                    <td >
                                        
                                        <label class="input">
                                              <input type="text" class="form-control" formControlName="PatientPercentage"
                                              pattern="[0-9]+" minlength="0" maxlength="4" style="text-align: right"  (input)="calculatepatientratio($event.target.value,i)"   >
          
                                            <!-- <input type="number" name="PatientPercentage" formControlName="PatientPercentage" style="text-align: right" (input)="calculatepatientratio($event.target.value,i)"/> -->
                                        </label>
                                       
                                        <div class="note note-error" *ngIf="demographicpatientgroup.controls['_patientgroup'].controls[i].controls.PatientPercentage.invalid">Character are not allowed in number field</div>
                                    </td>
                                    <td style="display: none;" >
                                        <label class="input">
                                              <input type="text" class="form-control" formControlName="PatientRatio"
                                              pattern="[0-9]+" minlength="0" maxlength="4" style="text-align: right"    >
      
                                            <!-- <input type="number" name="PatientRatio" formControlName="PatientRatio" style="text-align: right" /> -->
                                        </label>
                                    </td>
                              
                                    <td style="display: none;" >
                                        <label class="input">
                                            <a type="button" (click)="delremovepatientgroup(i)">
                                                <i class="fa fa-trash-o"></i>
                                            </a>
                                        </label>
                                    </td>
                               
                                </tr>
                            </tbody>
                        </table>
      
      
                    </div>
      
                </section>
                <section class="col col-6">
                  <div id="container4" >
                  </div>
                  </section>
            </div>
                </fieldset>

                <footer>
                  <a [routerLink]="['/Forecast/Demographictestassumption',forecastid,programid]" class="btn btn-default"
                    style="float: left;"><b>
                      << Previous</b> </a> <button type="button" (click)="savepatientgroup()" [disabled]="demographicpatientgroup.controls['_patientgroup'].invalid"  class="btn btn-default" style="float: right;"><b>Next
                      >></b></button>
                </footer>

              </form>

            </div>
            <!-- end widget content -->

          </div>
          <!-- end widget div -->

        </div>
      </article>
    </div>
  </sa-widgets-grid>
</div>
`,
  styleUrls: ['./demographicpatientgroupratio.component.css']
})
export class DemographicpatientgroupratioComponent implements OnInit,AfterViewInit {
  forecastid: number;
  programid: number;
  demographicpatientgroup: FormGroup;
  totalpercentage: number = 0;
  oldtoltalpercentage:number=0;
  totaltargetpatient: number = 0;
  forecasttype: string;
  patiengrouparr= new Array();
  @Input() RecforecastID:number;
  @Output()
  nextStep = new EventEmitter<string>();

  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {

     
    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];


    }
    // if (this._avRoute.snapshot.params["pid"]) {
    //   this.programid = this._avRoute.snapshot.params["pid"];


    // }

   
  
  }

 ngAfterViewInit()
 {
   console.log('after view')
 }
  calculatepatientratio(searchValue: any, index: any) {
let groupname=(<FormGroup>(
  (<FormArray>this.demographicpatientgroup.controls["_patientgroup"]).controls[
  index
  ]
)).controls["PatientGroupName"].value
   if (searchValue<=100)
   {
     console.log((<FormGroup>(
      (<FormArray>this.demographicpatientgroup.controls["_patientgroup"]).controls[
      index
      ]
    )).controls["PatientGroupName"].value)
    console.log(this.totalpercentage);
    (<FormGroup>(
      (<FormArray>this.demographicpatientgroup.controls["_patientgroup"]).controls[
      index
      ]
    )).patchValue({
      PatientRatio: (parseFloat(searchValue) * this.totaltargetpatient) / 100
    })
    index = this.patiengrouparr.findIndex(x => x.name === groupname)
    if (index >= 0) {
      this.patiengrouparr[index]["y"]=parseInt(searchValue);
    }
    else
    {
      this.patiengrouparr.push({
        name:groupname,
       y:parseInt(searchValue)
      })
    }
     console.log(this.patiengrouparr)
  
     this.Createchart(this.patiengrouparr)

    // series: [{
    //   type: 'pie',
    //   name: 'Browser share',
    //   data: [
    //       ['Chrome 13',   20.0],
    //       ['IE 9',       14.0],
    //       ['Firefox 12',   12.0],
    //       ['IE 8',       12.0],
    //       {
    //           name: 'Chrome 10',
    //           y: 10.8,
    //           sliced: true,
    //           selected: true
    //       },
    //       ['Safari',    8.5],
    //       ['Opera',     6.2],
    //       ['Something',   5.7],          
    //       ['Else',     5.5],
    //       ['Others',   5.4]
    //   ]
 // }]



  }
else{
  this._GlobalAPIService.FailureMessage("Percentage Should not be greater than 100");
  (<FormGroup>(
    (<FormArray>this.demographicpatientgroup.controls["_patientgroup"]).controls[
    index
    ]
  )).patchValue({
    PatientRatio: 0,
    PatientPercentage:0
  })
}

  }
  ngOnInit() {
   
   
    if (this.RecforecastID>0)
    {
          this.forecastid = this.RecforecastID;
       
    }
 
    if (this.forecastid > 0) {
      this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','GetbyId').subscribe((resp) => {                  
        this.programid =  resp["programId"];
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'getforecasttype').subscribe((data) => {

        this.forecasttype = data.forecasttype;
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Getpatientgroupbydemoforecastid', "programid=" + this.programid).subscribe((data) => {
        for (let boxIndex = 0; boxIndex < data.length; boxIndex++) {

          this.patiengrouparr.push({
            name:data[boxIndex].patientGroupName,
           y:parseInt(data[boxIndex].patientPercentage)
          })

         this.oldtoltalpercentage=this.oldtoltalpercentage+parseFloat(data[boxIndex].patientPercentage);
          this.addsitecategory();
          (<FormGroup>(
            (<FormArray>this.demographicpatientgroup.controls["_patientgroup"]).controls[
            boxIndex
            ]
          )).patchValue({

            ID: data[boxIndex].id,
            ForecastInfoID: data[boxIndex].forecastinfoID,
            PatientGroupName: data[boxIndex].patientGroupName,
            PatientPercentage: data[boxIndex].patientPercentage,
            PatientRatio: data[boxIndex].patientRatio,
            GroupID: data[boxIndex].groupID
          });


        }


       this.Createchart(this.patiengrouparr);

      })


     
     
     
  
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Gettotaltargetpatient', "programid=" + this.programid).subscribe((data) => {

        this.totaltargetpatient = data;
      })
    })
   
      })
      
    }
    this.demographicpatientgroup = this._fb.group({
      _patientgroup: this._fb.array([]),

    })
    this.demographicpatientgroup.controls._patientgroup.valueChanges.subscribe((change) => {
      const calculateAmount = (patientgroup: any[]): number => {
        return patientgroup.reduce((acc, current) => {
  
          this.totalpercentage = acc + parseFloat(current.PatientPercentage || 0);
          return acc + parseFloat(current.PatientPercentage || 0);
        }, 0);
      }
      calculateAmount(this.demographicpatientgroup.controls._patientgroup.value);
  
    });
  }
  initsitecategory() {
    let patientgp: FormGroup = this._fb.group({
      ID: 0,
      GroupID: 0,
      ForecastInfoID: 0,
      PatientGroupName: [{ value: '', disabled: true }],
      PatientPercentage: 0,
      PatientRatio: [{ value: 0, disabled: true }],

    });
    return patientgp;
  }
  addsitecategory() {
    (<FormArray>this.demographicpatientgroup.controls["_patientgroup"]).push(
      this.initsitecategory()
    );
  }
  delremovepatientgroup(index: any) {
    let groupid = (<FormGroup>(<FormArray>this.demographicpatientgroup.controls["_patientgroup"])
      .controls[index]
    ).controls["GroupID"].value
    this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'getgroupexistintestingprotocol', "groupid=" + groupid).subscribe((data) => {
      if (data > 0) {
        this._GlobalAPIService.FailureMessage("You can't delete " + (<FormGroup>(<FormArray>this.demographicpatientgroup.controls["_patientgroup"])
          .controls[index]
        ).controls["PatientGroupName"].value + " it is already use in testing protocol")
        return
      }
      else {
        this._APIwithActionService.deleteData(this.forecastid, 'Forecsatinfo', 'delpatientgroup', "groupid=" + groupid).subscribe((data) => {
          this._GlobalAPIService.SuccessMessage("Patient Group Deleted Successfully");
          (<FormArray>this.demographicpatientgroup.controls["_patientgroup"]).removeAt(index);
        })
      }


    })
  }
  savepatientgroup() {
    let patientgpnumber = <FormArray>this.demographicpatientgroup.controls["_patientgroup"]

    let patientgroupusage = new Array();

    patientgpnumber.getRawValue().forEach(x => {

      patientgroupusage.push(x)

    });
    console.log(patientgroupusage);
    if (this.totalpercentage > 100 || this.totalpercentage<100) {
      this._GlobalAPIService.FailureMessage("Ratio of Group should be equal to 100")
      return;
    }
    else {
      this._APIwithActionService.postAPI(patientgroupusage, 'Forecsatinfo', 'savepatientgroup').subscribe((data) => {
        if (data["_body"] != "0") {
          this._router.navigate(["/Forecast/Demographitestingprotocol", this.forecastid,this.programid]);  
          if(       this.oldtoltalpercentage==0)
          {
    
            
          //this._GlobalAPIService.SuccessMessage("Patient Group Saved Successfully");
          }
      
        }
      })

    }
   //this.nextStep.emit('step3,N,'+this.forecastid);
  }
  Previousclick()
  {
    if (this.forecasttype == 'S') {
      this._router.navigate(["/Forecast/Demographicsitebysite", this.forecastid]);
    }
    else {
      this._router.navigate(["/Forecast/DemographicAggregrate", this.forecastid]);
    }
   
  }
Createchart(arr:any)
{
  Highcharts.chart('container4', {
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
        text: '',
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
     
        colorByPoint: true,
        data:arr
    }]
});




}
} 
