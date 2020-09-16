import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import {NotificationService} from '../../shared/utils/notification.service';
@Component({
  selector: 'app-demographicsitebysite',
  templateUrl: './demographicsitebysite.component.html',
  styleUrls: ['./demographicsitebysite.component.css']
})
export class DemographicsitebysiteComponent implements OnInit {
  demographicsitebysite:FormGroup
  forecastid:number
 // programid:number
  Forecastmethod: number = 0;
  Show: boolean = false;
  Editdata=new Array();
  outsideRange:boolean=false;
  selectedRow: Number;
  constructor(private notificationService:NotificationService,private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) { 
    if (this._avRoute.snapshot.params["id"]) {  
      this.forecastid = this._avRoute.snapshot.params["id"];  
     
  
  }  
  // if (this._avRoute.snapshot.params["pid"]) {  
  //   this.programid = this._avRoute.snapshot.params["pid"];  
   

  // }


  if (this.forecastid > 0) {
    this._APIwithActionService.getDatabyID(this.forecastid, 'MMProgram', 'Getforecastparameterbyforecastid').subscribe((resp) => {

      this.Forecastmethod = resp[0]["forecastMethod"];
      console.log(this.Forecastmethod);
      if (this.Forecastmethod == 1) {
        this.Show = true
      }
      else {
        this.Show = false
      }
      console.log(this.Show);
    })
  }
  // this._APIwithActionService.Getpatientnumber.subscribe((data: any) => {
  //   console.log(data);
  //   if(data !=undefined)
  //   {
  //   for (let boxIndex = 0; boxIndex < data.length; boxIndex++) {
  //     this.addpatientnumber();
  //     (<FormGroup>(
  //       (<FormArray>this.demographicsitebysite.controls["_patientnumber"]).controls[
  //       boxIndex
  //       ]
  //     )).patchValue({

  //       id: data[boxIndex].id,
  //       ForecastinfoID: data[boxIndex].forecastinfoID,
  //       SiteID: data[boxIndex].siteID,
  //       SiteName: data[boxIndex].siteName,
  //       Currentpatient: data[boxIndex].currentPatient,
  //       Targetpatient: data[boxIndex].targetPatient,
  //       PopulationNumber: data[boxIndex].populationNumber,
  //       PrevalenceRate: data[boxIndex].prevalenceRate,

  //     });


  //   }
  // }
  // });
  if (this.forecastid != 0) {
    this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Getbyforecastid').subscribe((data) => {
     this.Editdata=data;
      for (let boxIndex = 0; boxIndex < data.length; boxIndex++) {
        this.addpatientnumber();
        (<FormGroup>(
          (<FormArray>this.demographicsitebysite.controls["_patientnumber"]).controls[
          boxIndex
          ]
        )).patchValue({

          id: data[boxIndex].id,
          ForecastinfoID: data[boxIndex].forecastinfoID,
          SiteID: data[boxIndex].siteID,
          SiteName: data[boxIndex].siteName,
          Currentpatient: data[boxIndex].currentPatient,
          Targetpatient: data[boxIndex].targetPatient,
          PopulationNumber: data[boxIndex].populationNumber,
          PrevalenceRate: data[boxIndex].prevalenceRate,

        });


      }
    })

  }
  }
  Previousclick()
  {
    this.notificationService.smallBox({
      title: "Conformation!",
      content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/Importforecastdata/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
      color: "#296191",
      //timeout: 8000,
      icon: "fa fa-bell swing animated"
    });
  }
  ngOnInit() {

    this.demographicsitebysite = this._fb.group({
     
      _patientnumber: this._fb.array([])

    })
  }
  comparecurrentpatient(group: FormGroup) {

    if (group.value.Currentpatient > group.value.Targetpatient) {
      
        this.outsideRange=  true;
 
      }
      else {
        this.outsideRange = false;
      }
        return this.outsideRange
      
    
  }
  addpatientnumber() {
    (<FormArray>this.demographicsitebysite.controls["_patientnumber"]).push(
      this.initpatientnumber()
    );
  }
  initpatientnumber() {
    let patientnumber: FormGroup = this._fb.group({
      id: 0,
      ForecastinfoID: 0,
      SiteID: 0,
      SiteName: [{ value: '', disabled: true }],
      Currentpatient: "0",
      Targetpatient: "0",
      PopulationNumber: "0",
      PrevalenceRate: "0"
    }
    // , {
    //     validator: this.comparecurrentpatient.bind(this),
     
    //   }
    );
      console.log(patientnumber);
    return patientnumber;
  }
  setClickedRow(index) {
    this.selectedRow = index;
    console.log(this.selectedRow);
  }


  savesitebysiteinfo() {

    let patientnumber = <FormArray>this.demographicsitebysite.controls["_patientnumber"]

    let patientnumberusage = new Array();
    console.log( patientnumber.getRawValue());

 if(patientnumber.getRawValue().length>0)
 {
    for (let index = 0; index <  patientnumber.getRawValue().length; index++) {
      if (patientnumber.getRawValue()[index].Currentpatient>patientnumber.getRawValue()[index].Targetpatient)
      {
        this._GlobalAPIService.FailureMessage(patientnumber.getRawValue()[index].SiteName + " Should have Current Patient less than target Patient")

        return;
      }
      else
      {
       patientnumberusage.push(patientnumber.getRawValue()[index])
      }
    }
  }
  else
  {

  this._GlobalAPIService.FailureMessage("Please Select atleast one site")
  return;
  }
    // patientnumber.getRawValue().forEach(x => {
    //  if (x.Currentpatient>x.Targetpatient)
    //  {

    //  }
    //   patientnumberusage.push(x)

    // });

    console.log("yyy");
    console.log(patientnumberusage);
    let newobject = new Object();
    //console.log(this.Testform.get('testId').value)
    console.log(patientnumberusage);
    newobject = {
      patientnumberusage
    }

    this._APIwithActionService.postAPI(newobject, 'Forecsatinfo', 'saveforecastsiteinfo').subscribe((data) => {
      if (data["_body"] != 0) {
   
        //    if(this.Editdata.length==0)
        //    {
        // this._GlobalAPIService.SuccessMessage("SitebySite information saved successfully");
        //    }
      }
      this._router.navigate(["Forecast/Demographipatientgroup", this.forecastid]);  
      // if (this.Delidpatientno != "0" && this.Delidpatientno != "" ) {
      //   this.Delidpatientno = this.Delidpatientno 
      //   this._APIwithActionService.deleteData(this.Delidpatientno, 'Forecsatinfo', 'Delforecastsiteinfo').subscribe((data) => {
      //     this.Delidpatientno="0";
      //   })
      // }
    //this.nextStep.emit('step2,N,'+this.forecastid);
 //  this._router.navigate(["Demographic/PatientGroupRatio", this.programid, this.forecastid]);
    })
  }
}
