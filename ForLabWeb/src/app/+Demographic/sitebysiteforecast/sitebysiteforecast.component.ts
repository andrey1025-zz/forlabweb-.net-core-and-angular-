import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Globals } from '../../shared/Globals';
import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";

//import { DemographicwizardComponent } from '../demographicwizard/demographicwizard.component';
@Component({
  selector: 'app-sitebysiteforecast',
  templateUrl: './sitebysiteforecast.component.html',
  providers: [Globals],

})
export class SitebysiteforecastComponent implements OnInit {
  RegionList = new Array();
  SiteList = new Array();
  sitebysiteforecast: FormGroup;
  forecastid: number;
  programid: number;
  selectedRow: Number;
  Delidpatientno: string = "";
  Forecastmethod: number = 0;
  Show: boolean = false;
  checksite: boolean = false;
  outsideRange:boolean=false;
  Editdata=new Array();
  @Input() RecforecastID:number;
  @Input() model3:any;
//  @Input('Recforecastid') recid;
  @Output()
  nextStep = new EventEmitter<string>();

  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService, private _Globals: Globals) {



    if (this._avRoute.snapshot.params["id1"]) {
      this.programid = this._avRoute.snapshot.params["id1"];

    }
    if (this._avRoute.snapshot.params["id2"]) {
      this.forecastid = this._avRoute.snapshot.params["id2"];

    }


  
    if (this.programid > 0) {
      this._APIwithActionService.getDatabyID(this.programid, 'MMProgram', 'Getforecastparameterbyprogramid').subscribe((resp) => {

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
    this._APIwithActionService.Getpatientnumber.subscribe((data: any) => {
      console.log(data);
      if(data !=undefined)
      {
      for (let boxIndex = 0; boxIndex < data.length; boxIndex++) {
        this.addpatientnumber();
        (<FormGroup>(
          (<FormArray>this.sitebysiteforecast.controls["_patientnumber"]).controls[
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
    }
    });
    if (this.forecastid != 0) {
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Getbyforecastid').subscribe((data) => {
       this.Editdata=data;
        for (let boxIndex = 0; boxIndex < data.length; boxIndex++) {
          this.addpatientnumber();
          (<FormGroup>(
            (<FormArray>this.sitebysiteforecast.controls["_patientnumber"]).controls[
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
    this.getregions();
  }
  showdiv(value:any)
  {
    if(value=="importexcel")
    {
      this.model3.show();
    }
    else
    {
      this.model3.hide();
    }

  }
  ngOnInit() {
    if (this.RecforecastID>0)
    {
          this.forecastid = this.RecforecastID;
    }
    
    this.sitebysiteforecast = this._fb.group({
      regionlist: this._fb.array([]),
      selectedsiteList: this._fb.array([]),
      _patientnumber: this._fb.array([])

    })
  }
  initsite() {
    let site: FormGroup = this._fb.group({

      SiteID: 0,
      SiteName: ''

    });
    return site;
  }
  addsite() {
    (<FormArray>this.sitebysiteforecast.controls["selectedsiteList"]).push(
      this.initsite()
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
    (<FormArray>this.sitebysiteforecast.controls["_patientnumber"]).push(
      this.initpatientnumber()
    );
  }
  getregions() {
    let Countryid:any
    if (localStorage.getItem("role")=="admin")
    {
      Countryid=0
    }
    else
    {
      Countryid=localStorage.getItem("countryid")
    }
    this._APIwithActionService.getDatabyID(Countryid, 'Site', 'GetregionbyCountryID')
    .subscribe((data) => {
      this.RegionList = data

    }
    ), err => {
      console.log(err);
    }

  }
  selectallsites(isChecked: boolean) {
    const selectedsiteList = <FormArray>this.sitebysiteforecast.controls.selectedsiteList;
    if (isChecked) {
      if (selectedsiteList.controls.length > 0) {
        for (let index1 = selectedsiteList.controls.length - 1; index1 >= 0; index1--) {
          selectedsiteList.removeAt(index1);
        }
      }
      this.checksite = true;
      for (let index1 = 0; index1 < this.SiteList.length; index1++) {
        this.addsite();
        (<FormGroup>(
          (<FormArray>this.sitebysiteforecast.controls["selectedsiteList"]).controls[
          index1
          ]
        )).patchValue({
          SiteID: this.SiteList[index1].siteID,
          SiteName: this.SiteList[index1].siteName
        })
      }

    }
    else {
      this.checksite = false;
      if (selectedsiteList.controls.length > 0) {
        for (let index1 = selectedsiteList.controls.length - 1; index1 >= 0; index1--) {
          selectedsiteList.removeAt(index1);
        }
      }
    }
  }
  onsiteChange(data: any, isChecked: boolean) {
    const selectedsiteList = <FormArray>this.sitebysiteforecast.controls.selectedsiteList;
    let index: number
    let regionids: string = "";
    if (isChecked) {

      index = selectedsiteList.length;
      this.addsite();
      (<FormGroup>(
        (<FormArray>this.sitebysiteforecast.controls["selectedsiteList"]).controls[
        index
        ]
      )).patchValue({
        SiteID: data.siteID,
        SiteName: data.siteName
      })


    } else {
      for (let index1 = 0; index1 < selectedsiteList.controls.length; index1++) {
        const element = <FormGroup>selectedsiteList.controls[index1];
        if (element.controls.SiteID.value == data.siteID) {
          selectedsiteList.removeAt(index1);
          break;
        }

      }

    }

    console.log(selectedsiteList);
  }
  Addsitepatientnew(){
    let isexist: boolean = false;
    let boxindex: number = 0;
    const selectedsiteList = <FormArray>this.sitebysiteforecast.controls.selectedsiteList;
    for (let index1 = 0; index1 < selectedsiteList.controls.length; index1++) {
      const element = <FormGroup>selectedsiteList.controls[index1];
      let newarray = (<FormArray>this.sitebysiteforecast.controls["_patientnumber"]).controls;

      for (let index = 0; index < newarray.length; index++) {
        if ((<FormGroup>newarray[index]).controls.SiteID.value == element.controls.SiteID.value) {
          isexist = true
          break;
        }
        else {
          isexist = false
        }

      }



      if (isexist == false) {
        boxindex = newarray.length;
        this.addpatientnumber();
        (<FormGroup>(
          (<FormArray>this.sitebysiteforecast.controls["_patientnumber"]).controls[
          boxindex
          ]
        )).patchValue({
          ForecastinfoID: this.forecastid,
          SiteID: element.controls.SiteID.value,
          SiteName: element.controls.SiteName.value,
          Currentpatient: "0",
          Targetpatient: "0",
          PopulationNumber: "0",
          PrevalenceRate: "0"

        })
      }
    }


  }
  delremovepatientno(index) {


    let delid: String
    delid = (<FormGroup>(<FormArray>this.sitebysiteforecast.controls["_patientnumber"])
      .controls[index]
    ).controls["id"].value
    if (delid != "0") {
      this.Delidpatientno = this.Delidpatientno + "," + delid;
    }
    (<FormArray>this.sitebysiteforecast.controls["_patientnumber"]).removeAt(index);

  }
  setClickedRow(index) {
    this.selectedRow = index;
    console.log(this.selectedRow);
  }
  savesitebysiteinfo() {

    let patientnumber = <FormArray>this.sitebysiteforecast.controls["_patientnumber"]

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

      if (this.Delidpatientno != "0" && this.Delidpatientno != "" ) {
        this.Delidpatientno = this.Delidpatientno 
        this._APIwithActionService.deleteData(this.Delidpatientno, 'Forecsatinfo', 'Delforecastsiteinfo').subscribe((data) => {
          this.Delidpatientno="0";
        })
      }
    this.nextStep.emit('step2,N,'+this.forecastid);
    //  this._router.navigate(["Demographic/PatientGroupRatio", this.programid, this.forecastid]);
    })
  }
  Previousclick() {
  this.nextStep.emit('step1,P,'+this.forecastid);
    //  this._router.navigate(["Demographic/DemographicAdd", this.programid, this.forecastid]);
  }
  onChange(regionid: string, isChecked: boolean) {
    const selectedsiteList = <FormArray>this.sitebysiteforecast.controls.regionlist;
    let regionids: string = "";
    if (isChecked) {
      selectedsiteList.push(new FormControl(regionid));

      console.log(selectedsiteList)
    } else {
      let index = selectedsiteList.controls.findIndex(x => x.value == regionid)
      selectedsiteList.removeAt(index);
    }
    if (selectedsiteList.length > 0) {
      for (let index = 0; index < selectedsiteList.length; index++) {
        regionids = regionids + "," + selectedsiteList.controls[index].value;

      }
      this._APIwithActionService.getDatabyID(regionids, 'Site', 'GetSitebyregions').subscribe((data) => {
        this.SiteList = data;
      })
    }
    else {
      this.SiteList.splice(0, this.SiteList.length);
    }
  }
}
