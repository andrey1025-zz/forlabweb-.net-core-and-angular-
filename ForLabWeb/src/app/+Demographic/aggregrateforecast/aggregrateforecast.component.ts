import { Component, OnInit, Output, EventEmitter, Input,ViewChild } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router, ActivatedRoute } from '@angular/router';
import { JsonApiService } from "app/core/api/json-api.service";
import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
//import { DemographicwizardComponent } from '../demographicwizard/demographicwizard.component';
@Component({
  selector: 'app-aggregrateforecast',
  templateUrl: './aggregrateforecast.component.html'
})
export class AggregrateforecastComponent implements OnInit {

  RegionList = new Array();
  SiteList = new Array();
  selectedsite=new Array();
  aggregrateforecast: FormGroup;
  forecastid: number;
  programid: number;
  selectedRow: Number;
  Delidpatientno: string = "";
  Delsiteids: string = "";
  Forecastmethod: number = 0;
  Show: boolean = false;
  Showsitetable: boolean = false;
  checksite: boolean = false;
  checkallsite: boolean = false;
  outsideRange: boolean = true;
  CategoryList: any[];
  CategoryID: number;
  CategoryName: string;
  regionids: string = "";
  listheader:string="Regions/Facility Type";
  divsubtable:number;
  sitediv:boolean=false;
  sitecategorylist=new Array();
  ischecked:boolean=false;
  @Input() RecforecastID1: number;

  @Output()
  nextStep = new EventEmitter<string>();
  backstep = new EventEmitter<string>();
  Showdiv: boolean = false;
  selectedregions=new Array();
 
  @ViewChild('myTable') table: any;

  rows: any[] = [];
  expanded: any = {};
  timeout: any;
  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService,private jsonApiService:JsonApiService) {


    this._APIwithActionService.getDatabyID(localStorage.getItem("countryid"), 'Site', 'GetregionbyCountryID')
    .subscribe((data) => {
      this.RegionList = data;
    })
    this.aggregrateforecast = this._fb.group({
      CategoryID: 0,
      CategoryName:['', [Validators.required]],
      methodtype:'catregions',
      regionlist: this._fb.array([]),
      selectedsiteList: this._fb.array([]),
      sitecategorylist: this._fb.array([]),
      _patientnumber: this._fb.array([])

    })
  }

  ngOnInit() {
    this.jsonApiService.fetch('/tables/datatables.filters.json').subscribe(data=> {
      this.rows = data;
    })

    if (this._avRoute.snapshot.params["id1"]) {
      this.programid = this._avRoute.snapshot.params["id1"];

    }
    if (this._avRoute.snapshot.params["id2"]) {
      this.forecastid = this._avRoute.snapshot.params["id2"];



    }
    if (this.RecforecastID1 > 0) {
      this.forecastid = this.RecforecastID1;
    }
 
    
    if (this.programid > 0) {
      this._APIwithActionService.getDatabyID(this.programid, 'MMProgram', 'Getforecastparameterbyprogramid').subscribe((resp) => {

        this.Forecastmethod = resp[0]["forecastMethod"];
      
        if (this.Forecastmethod == 1) {
          this.Show = true
        }
        else {
          this.Show = false
        }
       
      })
    }
    if (this.forecastid > 0) {
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Getcategoryinfobyforecastid').subscribe((data) => {
        if(data.length>0)
        {
        for (let boxIndex = 0; boxIndex < data.length; boxIndex++) {
          this.addpatientnumber();
          (<FormGroup>(
            (<FormArray>this.aggregrateforecast.controls["_patientnumber"]).controls[
            boxIndex
            ]
          )).patchValue({

            id: data[boxIndex].id,
            ForecastinfoID: data[boxIndex].forecastinfoID,
            SiteCategoryId: data[boxIndex].siteCategoryId,
            SiteCategoryName: data[boxIndex].siteCategoryName,
            Currentpatient: data[boxIndex].currentPatient,
            Targetpatient: data[boxIndex].targetPatient,
            PopulationNumber: data[boxIndex].populationNumber,
            PrevalenceRate: data[boxIndex].prevalenceRate,

          });


        }
        this.aggregrateforecast.patchValue({
          methodtype:data[0].methodtype
        })
        this.showdiv(data[0].methodtype);
        this.aggregrateforecast.get('methodtype').disable();
      }
      })
     
    }
    this.getCategory()
   
  }
  Addcategory()
  {
   let category= new Object();

   let isexist: boolean = false;
  
    let boxindex: number = 0;
    this.Showsitetable = true;
   category={
    CategoryID :0,
    CategoryName:this.aggregrateforecast.controls["CategoryName"].value
   }
    this._GlobalAPIService.postAPI(category,'SiteCategory')               
    .subscribe((data) => {  
        if (data["_body"] !="0")
        {
          
          this.aggregrateforecast.patchValue({
            CategoryID:data["_body"]
          })
            let newarray = (<FormArray>this.aggregrateforecast.controls["_patientnumber"]).controls;
      
            for (let index = 0; index < newarray.length; index++) {
              if ((<FormGroup>newarray[index]).controls.SiteCategoryId.value ==data["_body"]) {
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
                (<FormArray>this.aggregrateforecast.controls["_patientnumber"]).controls[
                boxindex
                ]
              )).patchValue({
                ForecastinfoID: this.forecastid,
                SiteCategoryId:data["_body"],
                SiteCategoryName: category["CategoryName"],
                Currentpatient: 0,
                Targetpatient: 0,
                PopulationNumber: 0,
                PrevalenceRate: 0,
                
      
              })
            }
      
            /////////////add sites
      
         
        }
      })
  }
  initsitecategory() {
    let site: FormGroup = this._fb.group({
      ID: 0,
      ForecastInfoID: 0,
      CategoryID: 0,
      SiteID: 0,
      SiteName: ''

    });
    return site;
  }
  addsitecategory() {
    (<FormArray>this.aggregrateforecast.controls["sitecategorylist"]).push(
      this.initsitecategory()
    );
  }
  initsite() {
    let site: FormGroup = this._fb.group({

      SiteID: 0,
      SiteName: ''

    });
    return site;
  }
  addsite() {
    (<FormArray>this.aggregrateforecast.controls["selectedsiteList"]).push(
      this.initsite()
    );
  }
  initpatientnumber() {
    let patientnumber: FormGroup = this._fb.group({
      id: 0,
      ForecastinfoID: 0,
      SiteCategoryId: 0,
      SiteCategoryName: [{ value: '', disabled: true }],
      Currentpatient: 0,
      Targetpatient: 0,
      PopulationNumber: 0,
      PrevalenceRate: 0
    }
      // , {
      //     validator: this.comparecurrentpatient.bind(this),

      //   }


    );
  
    return patientnumber;
  }
  comparecurrentpatient(group: FormGroup) {

    if (group.value.Currentpatient > group.value.Targetpatient) {

      this.outsideRange = true;



    }
    else {
      this.outsideRange = false;
    }
    return this.outsideRange

  }
  addpatientnumber() {
    (<FormArray>this.aggregrateforecast.controls["_patientnumber"]).push(
      this.initpatientnumber()
    );
  }
  Getregions(args) {
    let Countryid: any
    if (localStorage.getItem("role") == "admin") {
      Countryid = 0
    }
    else {
      Countryid = localStorage.getItem("countryid")
    }
    this.CategoryID = args.target.value;
    this.CategoryName = args.target.options[args.target.selectedIndex].text;
    this._APIwithActionService.getDatabyID(this.CategoryID, 'Site', 'GetregionbycategoryID', 'id2=' + Countryid).subscribe((data) => {
      this.RegionList = data
      this.SiteList.splice(0, this.SiteList.length)
      this.checkallsite = false
      this.regionids = "";
    }
    ), err => {
    
    }

  }
  getCategory() {
    this._GlobalAPIService.getDataList('SiteCategory').subscribe((data) => {
      this.CategoryList = data.aaData
     
    }
    ), err => {
    
    }

  }


  selectallsites(isChecked: boolean) {
    const selectedsiteList = <FormArray>this.aggregrateforecast.controls.selectedsiteList;
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
          (<FormArray>this.aggregrateforecast.controls["selectedsiteList"]).controls[
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
  onsiteChange(data: any, isChecked: boolean,index:number) {
    let patientnumber = <FormArray>this.aggregrateforecast.controls["_patientnumber"]
    if (isChecked) {
      this.selectedsite.push({
        siteID:data.siteID,
        siteName:data.siteName,
        categoryID:patientnumber.getRawValue()[index].SiteCategoryId,
        forecastInfoID: data.forecastInfoID,
         iD :  data.iD,
      })
      //regionlist.push(new FormControl(regionid));


    } else {
      let index =  this.selectedsite.findIndex(x => x.siteID == data.siteID)
      this.selectedsite.splice(index);
    }
   
  }
  Addsitepatient() {
let selectedsitecustomecat= new Array();
    if(this.aggregrateforecast.controls["methodtype"].value !="customcategory")
    {
    let isexist: boolean = false;
    let issiteexist: boolean = false;
    let boxindex: number = 0;
    this.Showsitetable = true;

    const selectedsiteList = <FormArray>this.aggregrateforecast.controls.selectedsiteList;
    for (let index1 = 0; index1 < this.selectedregions.length; index1++) {
    
      let newarray = (<FormArray>this.aggregrateforecast.controls["_patientnumber"]).controls;

      for (let index = 0; index < newarray.length; index++) {
        if ((<FormGroup>newarray[index]).controls.SiteCategoryId.value == this.selectedregions[index1]["regionID"]) {
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
          (<FormArray>this.aggregrateforecast.controls["_patientnumber"]).controls[
          boxindex
          ]
        )).patchValue({
          ForecastinfoID: this.forecastid,
          SiteCategoryId:this.selectedregions[index1]["regionID"],
          SiteCategoryName: this.selectedregions[index1]["regionName"],
          Currentpatient: 0,
          Targetpatient: 0,
          PopulationNumber: 0,
          PrevalenceRate: 0,
          

        })
      }

      /////////////add sites

    }
  }
  else{
    this.selectedregions.forEach(x=>{
      selectedsitecustomecat.push({
        siteID:x.regionID,
        siteName:x.regionName,
        categoryID:this.aggregrateforecast.controls["CategoryID"].value,
        forecastInfoID: this.forecastid,
         iD : 0,
      })
    
    })
    this._APIwithActionService.postAPI(selectedsitecustomecat,"Forecsatinfo","savecategorysiteinfo").subscribe((data) => {
      selectedsitecustomecat=[];
      this.selectedregions=[];
      this.ischecked=false;
        });
  }

  }
  delremovepatientno(index) {


    let delid: String
    let categoryID: string
    categoryID = (<FormGroup>(<FormArray>this.aggregrateforecast.controls["_patientnumber"])
      .controls[index]
    ).controls["SiteCategoryId"].value
    delid = (<FormGroup>(<FormArray>this.aggregrateforecast.controls["_patientnumber"])
      .controls[index]
    ).controls["id"].value
    if (delid != "0") {
      this.Delidpatientno = this.Delidpatientno + "," + delid;
    }
    (<FormArray>this.aggregrateforecast.controls["_patientnumber"]).removeAt(index);


    let sitecategorylist = (<FormArray>this.aggregrateforecast.controls["sitecategorylist"])
    sitecategorylist.getRawValue().forEach(x => {
      if (x.CategoryID == categoryID) {
        this.Delsiteids = this.Delsiteids + "," + x.ID;
        sitecategorylist.removeAt(x);
      }

    });

  }
  delsite(index) {
    let delid: String
    delid = (<FormGroup>(<FormArray>this.aggregrateforecast.controls["sitecategorylist"])
      .controls[index]
    ).controls["ID"].value
    if (delid != "0") {
      this.Delsiteids = this.Delsiteids + "," + delid;
    }
    (<FormArray>this.aggregrateforecast.controls["sitecategorylist"]).removeAt(index);

  }
  setClickedRow(index) {
    this.selectedRow = index;
   
  }
  savesitebycategoryinfo() {

    let patientnumber = <FormArray>this.aggregrateforecast.controls["_patientnumber"]
    let sitesarray = <FormArray>this.aggregrateforecast.controls["sitecategorylist"]
    let patientnumberusage = new Array();
    let categorysite = new Array();



    if (patientnumber.getRawValue().length > 0) {
      for (let index = 0; index < patientnumber.getRawValue().length; index++) {
        if ((Number)(patientnumber.getRawValue()[index].Currentpatient) > (Number)(patientnumber.getRawValue()[index].Targetpatient)) {
          this._GlobalAPIService.FailureMessage(patientnumber.getRawValue()[index].SiteCategoryName + " Should have Current Patient less than target Patient")

          return;
        }
        else {
          patientnumberusage.push({
            ForecastinfoID: patientnumber.getRawValue()[index].ForecastinfoID,
            SiteCategoryId:patientnumber.getRawValue()[index].SiteCategoryId,
            SiteCategoryName: patientnumber.getRawValue()[index].SiteCategoryName,
            Currentpatient: patientnumber.getRawValue()[index].Currentpatient,
            Targetpatient: patientnumber.getRawValue()[index].Targetpatient,
            PopulationNumber:patientnumber.getRawValue()[index].PopulationNumber,
            PrevalenceRate:patientnumber.getRawValue()[index].PrevalenceRate,
            methodtype:this.aggregrateforecast.controls["methodtype"].value
          })
            
            
          
        }
      }
    }
    else {

      this._GlobalAPIService.FailureMessage("Please Select atleast one category")
      return;
    }
    // patientnumber.getRawValue().forEach(x => {

    //   patientnumberusage.push(x)

    // });


    sitesarray.getRawValue().forEach(x => {

      categorysite.push(x)

    });
    let newobject = new Object();


    newobject = {
      patientnumberusage,
      categorysite
    }

    this._APIwithActionService.postAPI(newobject, 'Forecsatinfo', 'saveforecastcategoryinfo').subscribe((data) => {
      if (data["_body"] != 0) {

        //this._GlobalAPIService.SuccessMessage("Aggregrate Forecast saved successfully");
      }
      if (this.Delsiteids != "0") {
        this._APIwithActionService.deleteData(this.Delsiteids, 'Forecsatinfo', 'delforecastcategorysiteinfo').subscribe((data) => {

          

        })
      }
      if (this.Delidpatientno != "0") {
        this._APIwithActionService.deleteData(this.Delidpatientno, 'Forecsatinfo', 'delforecastcategoryinfo').subscribe((data) => {
          this.Delsiteids = "0";
          this.Delidpatientno = "0";
        })
      }
      // this._router.navigate(["Demographic/PatientGroupRatio", this.programid, this.forecastid]);
      this.nextStep.emit('step2,N,' + this.forecastid);
    })
  }
  Previousclick() {
    this.nextStep.emit('step1,P,' + this.forecastid);
  }
 
  onChange(data: any, isChecked: boolean) {
  

    if (isChecked) {
      this.selectedregions.push(data)
      //regionlist.push(new FormControl(regionid));


    } else {
      let index =  this.selectedregions.findIndex(x => x.regionId == data.regionID)
      this.selectedregions.splice(index);
    }
  
 
  }
  showdiv(value: any) {
    if (value == "customcategory") {
      this.Showdiv = true;

      this.RegionList =[]
      this.listheader="Sites"
      this._APIwithActionService.getDatabyID(localStorage.getItem("countryid"),'Site','GetAll')
      .subscribe((data) => {
        data.aaData.forEach(element => {
          this.RegionList.push({
            regionID: element.siteID ,
            regionName: element.siteName
          })
        });
      })
   
    }
    else if (value == "catfacilitytype") {
      this.Showdiv = false;
      this.RegionList=[];
      this.listheader="Regions/Facility Type"

      this._GlobalAPIService.getDataList('SiteCategory')
        .subscribe((data) => {  
         
          data.aaData.forEach(element => {
            this.RegionList.push({
              regionID: element.categoryID,
              regionName: element.categoryName
            })
          });
        }
        )
    }
    else {
      this.Showdiv = false;
      this.RegionList =[]
      this.listheader="Regions/Facility Type"
      this._APIwithActionService.getDatabyID(localStorage.getItem("countryid"), 'Site', 'GetregionbyCountryID')
        .subscribe((data) => {
          this.RegionList = data;
        })
    }
  }
  onPage(event) {
    clearTimeout(this.timeout);
    this.timeout = setTimeout(() => {
      
    }, 100);
  }


  toggleExpandRow(index:any) {
    let patientnumber = <FormArray>this.aggregrateforecast.controls["_patientnumber"]
 
    this.divsubtable=index;
   let categorysite=new Array();
   
    if(this.sitediv==false)
    {
      this.sitediv=true
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Getcategorysiteinfobyforecastid','categoryid='+patientnumber.getRawValue()[index].SiteCategoryId).subscribe((data) => {
        if (data.length > 0) {
  
  
          this.sitecategorylist=data;
      
          this.Showsitetable = true;
        }
      })
    }
    else
    {

    this._APIwithActionService.postAPI(this.selectedsite,"Forecsatinfo","savecategorysiteinfo").subscribe((data) => {
  this.selectedsite=[];
    });

      this.sitediv=false
    }
   
  }

  onDetailToggle(event) {
 
  }
}
