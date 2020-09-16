import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router, ActivatedRoute } from '@angular/router';

import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
//import { DemographicwizardComponent } from '../demographicwizard/demographicwizard.component';

@Component({
  selector: 'app-productassumption',
  templateUrl: './productassumption.component.html',

})
export class ProductassumptionComponent implements OnInit {
  productassumption: FormGroup;
  forecastid: number;
  controlArray = new Array();
  parameterlist = new Array();
  outsideRange: boolean;
  typeID: number;
  typeName: string;
  UserId: number;
  Programid:number;
  @Input() RecforecastID:number;
  @Output()
  nextStep = new EventEmitter<string>();
 
  public ProductTypeList = new Array();

  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {


  
  }
  inputvalue(args,i,datatype)
  {
    let name=args.target.name;
    if (datatype==2)
    {
      if (args.target.value>100)
      {
      this._GlobalAPIService.FailureMessage("Percentage should not be greater than 100");
      (<FormArray>(this.productassumption.controls["_productassumption"])).controls[i].patchValue({
        [name]:0
      })
    }

  }
}
  AddnewproductAssumption() {
    let index: number;
    let productassumption = <FormArray>this.productassumption.controls["_productassumption"];
    let isexist: boolean = false;
    productassumption.getRawValue().forEach(x => {

      if (x.productTypeID == this.typeID) {
        isexist = true
        return;
      }
    })
    if (isexist == false) {
      productassumption.push(
        this._fb.group({
          id: 0,
          forecastinfoID: 0,
          productTypeID: 0,
          productTypeName: ''
        })
      )
      index = productassumption.length == 0 ? 0 : productassumption.length - 1

      productassumption.controls[index].patchValue({
        id: 0,
        forecastinfoID: this.forecastid,
        productTypeID: this.typeID,
        productTypeName: this.typeName
      })
      let ss = (<FormGroup>productassumption.controls[index])

      for (let index1 = 0; index1 < this.controlArray.length; index1++) {
        if (this.controlArray[index1].type == "number") {
          ss.addControl(this.controlArray[index1].name, new FormControl(0))
        }
      }
    }
  }
  getproducttype() {
    this._GlobalAPIService.getDataList('ProductType').subscribe((data) => {
      this.ProductTypeList = data.aaData
      console.log(this.ProductTypeList)
    }
    ), err => {
      console.log(err);
    }

  }
  Gettype(args) {
    this.typeID = args.target.value;
    this.typeName = args.target.options[args.target.selectedIndex].text;
  }
  ngOnInit() {
    this.getproducttype();
    if (this._avRoute.snapshot.params["id1"]) {
      this.Programid = this._avRoute.snapshot.params["id1"];

    }
    if (this._avRoute.snapshot.params["id2"]) {
      this.forecastid = this._avRoute.snapshot.params["id2"];

    }
    
    if (this.RecforecastID>0)
    {
          this.forecastid = this.RecforecastID;
    }
    // this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Getprogramid').subscribe((data) => {
    //   this.Programid = data;
    // })
    this.productassumption = this._fb.group({
      typeID: 0,
      _productassumption: this._fb.array([])
    })





    if (this.forecastid > 0) {
      this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GetDynamiccontrol', 'entitytype=4').subscribe((data) => {
        this.controlArray = data
      })
      this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GetproductAssumption').subscribe((data) => {

        this.parameterlist = data[0].table;

        console.log(this.parameterlist)
        let ss = <FormArray>this.productassumption.controls["_productassumption"];
        for (let index = 0; index < this.parameterlist.length; index++) {
          ss.push(this._fb.group(this.parameterlist[index], { validator: this.comparecurrentpatient.bind(this) }))


        }
      })


    }

  }
  comparecurrentpatient(group: FormGroup) {
    for (let index = 0; index < this.controlArray.length; index++) {
      if (this.controlArray[index].type == "number") {
        if (group.value[this.controlArray[index].name] == 0) { this.outsideRange = true; }
        else {
          this.outsideRange = false;
        }

      }
      else {
        this.outsideRange = false;
      }

    }

  }

  saveproductassumption() {
    let productassumption = <FormArray>this.productassumption.controls["_productassumption"];
    let productassumptionlist = new Array();

    let productassumptionvalue = new Array();

    productassumption.getRawValue().forEach(x => {

      productassumptionlist.push({
        ID: x.id,
        ForecastinfoID: x.forecastinfoID,
        ProductTypeID: x.productTypeID,
        UserId: this.UserId

      })
      for (let index = 0; index < this.controlArray.length; index++) {
        if (this.controlArray[index].type == "number") {
          productassumptionvalue.push({

            Parameterid: this.controlArray[index].id,
            Parametervalue: x[this.controlArray[index].name],
            Forecastid: x.forecastinfoID,
            ProductTypeID: x.productTypeID,
            UserId: this.UserId
          })

        }

      }
    });


    this._APIwithActionService.postAPI(productassumptionlist, "Assumption", "SaveproductAssumption").subscribe((data) => {

      this._APIwithActionService.postAPI(productassumptionvalue, 'Assumption', 'saveproductgeneralassumptionvalue').subscribe((data) => {

        if (data["_body"] != 0) {
        //  this._GlobalAPIService.SuccessMessage("Product Assumption Saved Successfully");
        }

      }
      )
    })
// this._router.navigate(["Demographic/lineargrowth",this.forecastid])
this.nextStep.emit('step6,N,'+this.forecastid);
  }
  Previousclick()
  {
    this.nextStep.emit('step5,P,'+this.forecastid);
   // this._router.navigate(["Demographic/PatientAssumption",this.Programid,this.forecastid])
  }
}
