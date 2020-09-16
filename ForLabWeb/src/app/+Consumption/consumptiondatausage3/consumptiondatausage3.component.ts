import { Component, OnInit, Input,ViewChild,TemplateRef ,Output, EventEmitter} from '@angular/core';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
import { ActivatedRoute } from '@angular/router';
import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';
const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';
@Component({
  selector: 'app-consumptiondatausage3',
  templateUrl: './consumptiondatausage3.component.html',
  styles: ['.table tr.active td {   background-color:#123456 !important;    color: white;  }'],
})
export class Consumptiondatausage3Component implements OnInit {
  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;
  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };
  public loading = false;
  categorylist = new Array();
  sitelist = new Array();
  controlArray = new Array();
  datalist = new Array();
  forecastid: number;
  datausage3: FormGroup;
  selectedRow: number;
  selectedcategoryid: number;
  colspan: number;
  param: string = "";
  disaddproduct: boolean = true;
  disactualconsumption: boolean = true;
  disremoveconsumption: boolean = true;
  forecastsiteproductid: number;
  @Input() model:any;
  @Input() model2:any;
  @Input() model1:any;
  @Input() model3:any;
  @Input() RecforecastID1: number;
  @Output()
  nextStep = new EventEmitter<string>();
  constructor(private _avRoute: ActivatedRoute, private _fb: FormBuilder, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) {


    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];
    }


    if (this.RecforecastID1>0)
    {
      this.forecastid = this.RecforecastID1
    }
    this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "GetcategoryList").subscribe((data) => {
      this.categorylist = data;
    })
    this._APIwithActionService.GetDataUsage.subscribe((data: any) => {
     this.makeformarray(data)

    }

    )
    this._APIwithActionService.Getsitesfordatausage2.subscribe((data: any) => {

      data.forEach(element => {

        if (this.sitelist.findIndex(x => x.siteID == element.siteID) < 0) {
          this.sitelist.push(element);
        }
        this.param = this.param + element.siteID + ','
      });
      this.param = this.param + this.selectedcategoryid + ','
      this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Addsiteincategory", "param=" + this.param).subscribe((data) => {
        this.param = "";
      })

    });
  }
  Previousclick()
  {
    this.nextStep.emit('step1,P,'+this.forecastid)
  }
  nextclick()
  {
   this.nextStep.emit('step3,N,'+this.forecastid)
  }
  ngOnInit() {
    if (this.RecforecastID1>0)
    {
          this.forecastid = this.RecforecastID1;
    }
    this.datausage3 = this._fb.group({

      // selectedsiteList: this._fb.array([]),
      categoryname: '',
      _datausage: this._fb.array([])
    })

  }
  openimportmodel()
  {
    this.model3.show()
  }
  initdatausge() {
    // Initialize add Box form field
    let boxForm: FormGroup = this._fb.group({
      productid: 0,
      productname: [{ value: '', disabled: true }],
      value: this._fb.array([])
    });
    return boxForm;

  }
  AddCategory() {
    if (this.datausage3.controls["categoryname"].value != "") {
      this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Addcategory", "param=" + this.datausage3.controls["categoryname"].value).subscribe((data) => {
        if (data.removedatausage == "Duplicate data") {
          this._GlobalAPIService.FailureMessage("Duplicate category name")
       
        }
        else {
          this.categorylist.push({
            categoryname: this.datausage3.controls["categoryname"].value,
            categoryid: data.removedatausage
          })
        }

      })
    }
    else {
      this._GlobalAPIService.FailureMessage("Category Should not be empty");

    }
  }
  adddatausge() {

    (<FormArray>this.datausage3.controls["_datausage"]).push(
      this.initdatausge()
    );
    // get box length for box name like box 1,box 2 in sidebar boxes combo list
    let connection_boxes_length = (<FormArray>(
      this.datausage3.controls["_datausage"]
    )).length;

  }
  selectcategoryfordatausage3(data1, index) {
    this.selectedRow = index;
    this.selectedcategoryid = data1.categoryid;


    if (this.selectedcategoryid != 0) {
      this._APIwithActionService.getDatabyID(this.selectedcategoryid, "Consumption", "Getcategorysite").subscribe((data) => {

        this.sitelist = data;

      });
this.loading=true;
      this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Bindforecastsiteproduct", "siteorcatid=" + this.selectedcategoryid).subscribe((data) => {
     this.makeformarray(data);


this.loading=false;

      })
      this.disaddproduct = false;

    }
  }
  openconsumption() {
    this.model1.show()
  }
  openmodel() {

    this.model2.show();
  }

  openmodel2() {
    this.model.show();
  }
  Enableconsumptionbtn(i: any, j: any, duration: string) {
    let aa = (<FormGroup>((<FormArray>(
      (<FormGroup>(
        (<FormArray>this.datausage3.controls["_datausage"])
          .controls[i]
      )).controls["value"]
    )).controls[j])).controls[duration + "-id"].value
   
    this.forecastsiteproductid = aa;
    this.disactualconsumption = false;
    this.disremoveconsumption = false;
  }

  getadjustedvolumn(i: any, j: any, duration: string) {
    let postobj: object;
    let frmgrp: FormGroup
    frmgrp = (<FormGroup>((<FormArray>(
      (<FormGroup>(
        (<FormArray>this.datausage3.controls["_datausage"])
          .controls[i]
      )).controls["value"]
    )).controls[j]))
    postobj = {
      Id: frmgrp.get(duration + "-id").value,
      protestid: (<FormGroup>(
        (<FormArray>this.datausage3.controls["_datausage"])
          .controls[i]
      )).get('productid').value,
      type: frmgrp.get("column1").value,
      value: frmgrp.get(duration).value,
    }
    this._APIwithActionService.putAPI(this.forecastid, postobj, "Consumption", "GetAdjustedVolume").subscribe((data) => {
      this.makeformarray(JSON.parse(data["_body"]));
    })
  }

  makeformarray(data:any)
  {
    this.controlArray = data.controls;
    this.colspan = this.controlArray.length + 1;
    this.datalist = data.datausage;
  
    let ss = (<FormArray>this.datausage3.controls["_datausage"]);
    ss.controls = [];
    for (let boxIndex = 0; boxIndex < this.datalist.length; boxIndex++) {
      this.adddatausge();
      (<FormGroup>(
        (<FormArray>this.datausage3.controls["_datausage"]).controls[
        boxIndex
        ]
      )).patchValue({
        productid: this.datalist[boxIndex].productid,
        productname: this.datalist[boxIndex].productname
      });
      let values: Array<any> = this.datalist[boxIndex].value;

      for (let packIndex = 0; packIndex < values.length; packIndex++) {


        (<FormArray>(
          (<FormGroup>(
            (<FormArray>this.datausage3.controls["_datausage"])
              .controls[boxIndex]
          )).controls["value"]
        )).push(this._fb.group(values[packIndex]))

      }


     
    }

  }
  removeconsumption() {
    let msg;
    this._APIwithActionService.getDatabyID(this.forecastsiteproductid, "Consumption", "Removedatausagefromsite", "param=" + this.forecastid).subscribe((data) => {
      msg = data.removedatausage
      if (msg != "") {
        this._GlobalAPIService.FailureMessage(msg);
      }
      else {
        this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Bindforecastsiteproduct", "siteorcatid=" + this.selectedcategoryid).subscribe((data) => {
      this.makeformarray(data);

        })
      }

    })
  }

  deleteseletedsitefromcategory(item) {
    this._APIwithActionService.deleteData(this.selectedcategoryid, "Consumption", "deletesiteformcategory", "param=" + item.siteID).subscribe((data) => {

      if (data["_body"] != "") {
        this.sitelist.splice(this.sitelist.findIndex(x => x.siteID == item.siteID), 1);
        this._GlobalAPIService.SuccessMessage(item.siteName + ' site deleted successfully');
      }
    })
  }
  deleteseletedcategory(item)
  {
    this._APIwithActionService.deleteData(item.categoryid,"Consumption","deletecategorydatausage").subscribe((data)=>{
      if (data["_body"] != "") {
        this.categorylist.splice(this.categorylist.findIndex(x => x.categoryid == item.categoryid), 1);
        this.sitelist=[];
        this.controlArray=[]
        let ss = (<FormArray>this.datausage3.controls["_datausage"]);
        ss.controls = [];
        this._GlobalAPIService.SuccessMessage(item.categoryname + " Category deleted successfully");
      }
    })
  }
}
