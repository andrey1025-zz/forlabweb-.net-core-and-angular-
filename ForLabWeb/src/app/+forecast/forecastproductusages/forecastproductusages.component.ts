import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import {NotificationService} from '../../shared/utils/notification.service';
@Component({
  selector: 'app-forecastproductusages',
  templateUrl: './forecastproductusages.component.html',
  styleUrls: ['./forecastproductusages.component.css']
})
export class ForecastproductusagesComponent implements OnInit {
  testname:string;
  forecastid: number;
  controlproID: Number;
  consuminsID: Number;
  consuminsname: String;
  controlproname: String;
  consuminsperiodID: Number;
  consuminsperiodname: String;
  consuminsproID: Number;
  consuminsproname: String;
  Instrumentlist=new Array()
  testingAreaList = new Array();
  selectedtests = new Array();
  completelist = new Array();
  forecastproductusages:FormGroup;
  testprod = new Array();
  testcontrolprod = new Array();
  consumbletestprd = new Array();
  consumbleperiodprd = new Array();
  consumbleinstrumentprd = new Array();
  ProductTypeList= new Array();
  ProductCntrollist=new Array();
  completelist1 = new Array();
  Productlist= new Array();
  flag: boolean = false;
  activeTab:number=0;
selectedtestID:number=0;
testinsID: Number;
testinsname: String;
    testproID: Number;
    testproname: String;
    controlinsID: Number;
    controlinsname: String;
  consumtestproID: Number;
  consumtestproname: String;
  consumtesttypeID: Number;
  consumtesttypename: String;
  consumcontypeID: Number;
  consumcontypename: String;
  consumconperiodID: Number;
  consumconperiod: String;
  consuminstypeID: Number;
  consuminstypename: String;
  consumconproID: Number;
  consumconproname: String;
  bckcolor:string;
  selectedtest:Number;
  isHighlight:boolean=false;
  public state: any = {
    tabs: {
      demo1: 0
    }
  }
  public state1: any = {
    tabs: {
      demo1: 0
    }
  }
  constructor(private notificationService:NotificationService,private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {


    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];


    }
    this.forecastproductusages = this._fb.group({
      nooftest: [''],
      ProductUsageArray: this._fb.array([]),
      ControlUsageArray: this._fb.array([]),
      
      ConsumableTestArray: this._fb.array([]),
      ConsumablePeriodArray: this._fb.array([]),
      ConsumableInsArray: this._fb.array([])

    
  })
    this._APIwithActionService.getDatabyID(this.forecastid, "Test", "getAlltestbytestingarea").subscribe((data) => {


      for (let index = 0; index < data.length; index++) {
        const element = data[index];
        for (let index1 = 0; index1 < data[index].tests.length; index1++) {

          if (data[index].tests[index1].type == "A") {
            this.testingAreaList.push({
              testingAreaID: element.testareaid,
              areaName: element.testareaname

            })
            break;
          }


        }

      }

      for (let index = 0; index < data.length; index++) {
        const element = data[index];
        this.selectedtests = []
        for (let index1 = 0; index1 < data[index].tests.length; index1++) {

          if (data[index].tests[index1].type == "A") {
            this.flag = true;
            this.selectedtests.push(

              data[index].tests[index1]

            )

          }
       


        }
        if (this.flag == true) {
          this.completelist.push({
            testareaid: element.testareaid,
            testareaname: element.testareaname,
            tests: this.selectedtests
          })
          this.completelist1.push({
            testareaid: element.testareaid,
            testareaname: element.testareaname,
            tests: this.selectedtests
          })
        }
        this.flag =false
      }
     
      console.log(this.completelist)

    })
  }
  getProductType() {
    this._GlobalAPIService.getDataList('ProductType').subscribe((data) => {
        this.ProductTypeList = data.aaData
        // console.log(this.ProductTypeList)
    }
    ), err => {
        console.log(err);
    }

}
getproductlist(args, type: string = "") {

  this._APIwithActionService.getDatabyID(args.target.value, 'Product', 'GetAllProductByType').subscribe((data) => {
      this.Productlist = data
      // console.log(this.Instrumentlist)
  }
  ), err => {
      console.log(err);
  }

 
  
  if (type == "Test") {
      this.consumtesttypeID = args.target.value;
      this.consumtesttypename = args.target.options[args.target.selectedIndex].text;
  }
  else if (type == "period") {
      this.consumcontypeID = args.target.value;
      this.consumcontypename = args.target.options[args.target.selectedIndex].text;
  }
  else if (type == "Ins") {
      this.consuminstypeID = args.target.value;
      this.consuminstypename = args.target.options[args.target.selectedIndex].text;

  }
}
selecttestProductchange(args) {
  this.testproID = args.target.value;
  this.testproname = args.target.options[args.target.selectedIndex].text;
}
  ngOnInit() {
    this.getProductType();
  }
  inittestprod() {
    // Initialize add Box form field
    let boxForm: FormGroup = this._fb.group({
        name: [{ value: '' }, [Validators.required]],
        values: this._fb.array([])
    });
    return boxForm;
}

/*
 * Function Declare to Add Multiple Boxes
 * */
addtestprod() {
    (<FormArray>this.forecastproductusages.controls["ProductUsageArray"]).push(
        this.inittestprod()
    );
    // get box length for box name like box 1,box 2 in sidebar boxes combo list
    let connection_boxes_length = (<FormArray>(
        this.forecastproductusages.controls["ProductUsageArray"]
    )).length;
}

/*
 * Operation for connection pack
 * This function also used for create Dynamic Packs
 * */
initvalue() {
    let packs: FormGroup = this._fb.group({
        id: 0,
        productId: 0,
        productName: [{ value: '' }],
        instrumentId: 0,
        instrumentName: [""],
        rate: 1.0000000000,
        isForControl: false,
        testId: 0,
        forecastID:this.forecastid
    });
    return packs;
}

/*
 * This Function add packs Dynamically
 * */
addvalue(boxIndex: number) {
    //declare to add packs
    (<FormArray>(
        (<FormGroup>(
            (<FormArray>this.forecastproductusages.controls["ProductUsageArray"]).controls[
            boxIndex
            ]
        )).controls["values"]
    )).push(this.initvalue());
}



initcontrolUsage() {
  // Initialize add Box form field
  let boxForm: FormGroup = this._fb.group({
      name: [{ value: '' }, [Validators.required]],
      values: this._fb.array([])
  });
  return boxForm;
}

/*
* Function Declare to Add Multiple Boxes
* */
addcontrolusage() {
  (<FormArray>this.forecastproductusages.controls["ControlUsageArray"]).push(
      this.initcontrolUsage()
  );
  // get box length for box name like box 1,box 2 in sidebar boxes combo list
  let connection_boxes_length = (<FormArray>(
      this.forecastproductusages.controls["ControlUsageArray"]
  )).length;
}

/*
* Operation for connection pack
* This function also used for create Dynamic Packs
* */
initcontrolUsagevalue() {
  let packs: FormGroup = this._fb.group({
      id: 0,
      productId: 0,
      productName: [{ value: ''}],
      instrumentId: 0,
      instrumentName: [""],
      rate: 1.0000000000,
      isForControl: true,
      testId: 0,
      forecastID:this.forecastid
  });
  return packs;
}

/*
* This Function add packs Dynamically
* */
addcontrolUsagevalue(boxIndex: number) {
  //declare to add packs
  (<FormArray>(
      (<FormGroup>(
          (<FormArray>this.forecastproductusages.controls["ControlUsageArray"]).controls[
          boxIndex
          ]
      )).controls["values"]
  )).push(this.initcontrolUsagevalue());
}

initconsumableusage() {
  // Initialize add Box form field
  let boxForm: FormGroup = this._fb.group({
      name: [{ value: '', disabled: true }, [Validators.required]],
      values: this._fb.array([])
  });
  return boxForm;
}

/*
* Function Declare to Add Multiple Boxes
* */
addconsumableusage() {
  (<FormArray>this.forecastproductusages.controls["ConsumableTestArray"]).push(
      this.initconsumableusage()
  );
  // get box length for box name like box 1,box 2 in sidebar boxes combo list
  let connection_boxes_length = (<FormArray>(
      this.forecastproductusages.controls["ConsumableTestArray"]
  )).length;
}

/*
* Operation for connection pack
* This function also used for create Dynamic Packs
* */
initconsumableusagevalue() {
  let packs: FormGroup = this._fb.group({

      id: 0,
      consumableId: 0,
      testId: 0,
      productId: 0,
      productName: [{ value: '', disabled: true }],
      productTypeId: 0,
      productTypeName: [""],
      instrumentId: 0,
      instrumentName: [""],
      usageRate: 1.0000000000,
      perTest: true,
      perPeriod: false,
      perInstrument: false,
      noOfTest: 0,
      period: [{ value: '', disabled: true }],
      forecastID:this.forecastid

  });
  return packs;
}

/*
* This Function add packs Dynamically
* */
addconsumableusagevalue(boxIndex: number) {
  //declare to add packs
  (<FormArray>(
      (<FormGroup>(
          (<FormArray>this.forecastproductusages.controls["ConsumableTestArray"]).controls[
          boxIndex
          ]
      )).controls["values"]
  )).push(this.initconsumableusagevalue());
}

initconsumableusageperiod() {
  // Initialize add Box form field
  let boxForm: FormGroup = this._fb.group({
      name: [{ value: '', disabled: true }, [Validators.required]],
      values: this._fb.array([])
  });
  return boxForm;
}

/*
* Function Declare to Add Multiple Boxes
* */
addconsumableusageperiod() {
  (<FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"]).push(
      this.initconsumableusageperiod()
  );
  // get box length for box name like box 1,box 2 in sidebar boxes combo list
  let connection_boxes_length = (<FormArray>(
      this.forecastproductusages.controls["ConsumablePeriodArray"]
  )).length;
}

/*
* Operation for connection pack
* This function also used for create Dynamic Packs
* */
initconsumableusageperiodvalue() {
  let packs: FormGroup = this._fb.group({

      id: 0,
      consumableId: 0,
      testId: 0,
      productId: 0,
      productName: [{ value: '', disabled: true }],
      productTypeId: 0,
      productTypeName: [""],
      instrumentId: 0,
      instrumentName: [""],
      usageRate: 1.0000000000,
      perTest: false,
      perPeriod: true,
      perInstrument: false,
      noOfTest: 0,
      period: [{ value: '', disabled: true }],
      forecastID:this.forecastid

  });
  return packs;
}

/*
* This Function add packs Dynamically
* */
addconsumableusageperiodvalue(boxIndex: number) {
  //declare to add packs
  (<FormArray>(
      (<FormGroup>(
          (<FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"]).controls[
          boxIndex
          ]
      )).controls["values"]
  )).push(this.initconsumableusageperiodvalue());
}


initconsumableusageins() {
  // Initialize add Box form field
  let boxForm: FormGroup = this._fb.group({
      name: [{ value: '', disabled: true }, [Validators.required]],
      values: this._fb.array([])
  });
  return boxForm;
}

/*
* Function Declare to Add Multiple Boxes
* */
addconsumableusageins() {
  (<FormArray>this.forecastproductusages.controls["ConsumableInsArray"]).push(
      this.initconsumableusageins()
  );
  // get box length for box name like box 1,box 2 in sidebar boxes combo list
  let connection_boxes_length = (<FormArray>(
      this.forecastproductusages.controls["ConsumableInsArray"]
  )).length;
}

/*
* Operation for connection pack
* This function also used for create Dynamic Packs
* */
initconsumableusageinsvalue() {
  let packs: FormGroup = this._fb.group({

      id: 0,
      consumableId: 0,
      testId: 0,
      productId: 0,
      productName: [{ value: '', disabled: true }],
      productTypeId: 0,
      productTypeName: [""],
      instrumentId: 0,
      instrumentName: [""],
      usageRate: 1.0000000000,
      perTest: false,
      perPeriod: false,
      perInstrument: true,
      noOfTest: 0,
      period: [{ value: '', disabled: true }],
      forecastID:this.forecastid

  });
  return packs;
}

/*
* This Function add packs Dynamically
* */
addconsumableusageinsvalue(boxIndex: number) {
  //declare to add packs
  (<FormArray>(
      (<FormGroup>(
          (<FormArray>this.forecastproductusages.controls["ConsumableInsArray"]).controls[
          boxIndex
          ]
      )).controls["values"]
  )).push(this.initconsumableusageinsvalue());
}
  Previousclick() {

    this.notificationService.smallBox({
        title: "Conformation!",
        content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/ForecastSelectedIns/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
        color: "#296191",
        //timeout: 8000,
        icon: "fa fa-bell swing animated"
      });
   // this._router.navigate(["/Forecast/ForecastSelectedIns", this.forecastid]);
  }
  Saveforecastproductusage() {

    this._router.navigate(["/Forecast/Forecastmethodology", this.forecastid]);

  }
  Filterselectedtestarea(testareaid: any) 
  {
    if(testareaid=="0")
    {
      this.completelist1=this.completelist
    }
    else
    {
    this.completelist1=this.completelist.filter(x=>x.testareaid===parseInt(testareaid));
    }

  }
  onlstclick(data1:any,areaid:number) {
    this.selectedtest = data1.testID;
if(this.selectedtestID !=0)
{

  let savemodel=new Object;
    console.log(this.forecastproductusages.value);
    let productusage = <FormArray>this.forecastproductusages.controls["ProductUsageArray"]

    let postproductusage = new Array();

    let consumproductusage = new Array();
    productusage.getRawValue().forEach(element => {
        element.values.forEach(x => {
            postproductusage.push(x)
        });
    });

    let controlusage = <FormArray>this.forecastproductusages.controls["ControlUsageArray"]

    controlusage.getRawValue().forEach(element => {
        element.values.forEach(x => {
            postproductusage.push(x)
        });
    });

    let consumpertestusage = <FormArray>this.forecastproductusages.controls["ConsumableTestArray"]
    consumpertestusage.getRawValue().forEach(element => {
        element.values.forEach(x => {
            consumproductusage.push(x)
        });
    });


    let consumperperiodusage = <FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"]
    consumperperiodusage.getRawValue().forEach(element => {
        element.values.forEach(x => {
            consumproductusage.push(x)
        });
    });
    let consumperinsusage = <FormArray>this.forecastproductusages.controls["ConsumableInsArray"]
    consumperinsusage.getRawValue().forEach(element => {
        element.values.forEach(x => {
            consumproductusage.push(x)
        });
    });

savemodel={
  ForecastProductUsage:postproductusage,
  ForecastConsumableUsage:consumproductusage
}
this._APIwithActionService.postAPI(savemodel, 'Forecsatinfo', 'saveforecastusges').subscribe((data) => {
  // (<FormArray>this.forecastproductusages.controls["ProductUsageArray"]).controls=[];
  // (<FormArray>this.forecastproductusages.controls["ControlUsageArray"]).controls=[];
  // (<FormArray>this.forecastproductusages.controls["ConsumableTestArray"]).controls=[];
  // (<FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"]).controls=[];
  // (<FormArray>this.forecastproductusages.controls["ConsumableInsArray"]).controls=[];
})
}

    this._APIwithActionService.getDatabyID(areaid, 'Instrument', 'getInsbyareaid').subscribe((data) => {
      this.Instrumentlist = data
      //console.log(this.Instrumentlist)
  }
  ), err => {
      console.log(err);
  }
    this._APIwithActionService.getDatabyID(this.forecastid, "Forecsatinfo", "getForecastProductUsage","testid=" + data1.testID + "").subscribe((data11) => {
      this.testprod=[];
      this.testprod=data11
    if(data11.length>0)
    {
  
    this.updateExistingValue() ;
    }
    else
    {
        (<FormArray>this.forecastproductusages.controls["ProductUsageArray"]).controls=[];
    }
  //  console.log(data);
    })

this.selectedtestID=data1.testID ;
this.testname=data1.testName;

    this._APIwithActionService.getDatabyID(this.forecastid, "Forecsatinfo", "getControlProductUsage","testid=" + data1.testID + "").subscribe((data12) => {
      this.testcontrolprod=[];
      this.testcontrolprod=data12
      if(data12.length>0)
      {
       
      this.updateExistingValuecontrol() ;
      }
      else
      {
          (<FormArray>this.forecastproductusages.controls["ControlUsageArray"]).controls=[];
      }
    //  console.log(data);
      })
      this._APIwithActionService.getDatabyID(this.forecastid, "Forecsatinfo", "getForecastConsumbleUsagePertest","testid=" + data1.testID + "").subscribe((data13) => {
        this.consumbletestprd=[];
        this.consumbletestprd=data13
        if(data13.length>0)
        {
        
        this.updateExistingValueconsumbletestprd() ;
        }
        else
        {
            (<FormArray>this.forecastproductusages.controls["ConsumableTestArray"]).controls=[];
        }
      
        })

        this._APIwithActionService.getDatabyID(this.forecastid, "Forecsatinfo", "getForecastConsumbleUsagePerPeriod","testid=" + data1.testID + "").subscribe((data14) => {
          this.consumbleperiodprd=[];
          this.consumbleperiodprd=data14
          if(data14.length>0)
          {
          
          this.updateExistingValueconsumbleperiodprd() ;
          }
          else
          {
              (<FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"]).controls=[];
          }
        
          })

          this._APIwithActionService.getDatabyID(this.forecastid, "Forecsatinfo", "getForecastConsumbleUsagePerinstrument","testid=" + data1.testID + "").subscribe((data15) => {
           
            this.consumbleinstrumentprd=[]; this.consumbleinstrumentprd=data15
            if(data15.length>0)
            {
             
              this.updateExistingValueconsumbleinstrumentprd() ;
            }
            else
            {
                (<FormArray>this.forecastproductusages.controls["ConsumableInsArray"]).controls=[];
            }
            })
  

  }
  updateExistingValue() {
    (<FormArray>this.forecastproductusages.controls["ProductUsageArray"]).controls=[];
    for (let boxIndex = 0; boxIndex < this.testprod.length; boxIndex++) {
        this.addtestprod();
        (<FormGroup>(
            (<FormArray>this.forecastproductusages.controls["ProductUsageArray"]).controls[
            boxIndex
            ]
        )).patchValue({
            name: this.testprod[boxIndex].name
        });
        let values: Array<any> = this.testprod[boxIndex].value;

        for (let packIndex = 0; packIndex < values.length; packIndex++) {
            this.addvalue(boxIndex);
            (<FormGroup>(
                (<FormArray>(
                    (<FormGroup>(
                        (<FormArray>this.forecastproductusages.controls["ProductUsageArray"])
                            .controls[boxIndex]
                    )).controls["values"]
                )).controls[packIndex]
            )).patchValue({
                id: values[packIndex].id,
                productId: values[packIndex].productId,
                productName: values[packIndex].productName,
                instrumentId: values[packIndex].instrumentId,
                instrumentName: values[packIndex].instrumentName,
                rate: values[packIndex].rate,
                isForControl: values[packIndex].isForControl,
                testId: values[packIndex].testId

            });
        }
    }
    console.log( (<FormArray>this.forecastproductusages.controls["ProductUsageArray"]).controls)
  }

  updateExistingValuecontrol() {
    (<FormArray>this.forecastproductusages.controls["ControlUsageArray"]).controls=[];
    for (let boxIndex = 0; boxIndex < this.testcontrolprod.length; boxIndex++) {
        this.addcontrolusage();
        (<FormGroup>(
            (<FormArray>this.forecastproductusages.controls["ControlUsageArray"]).controls[
            boxIndex
            ]
        )).patchValue({
            name: this.testcontrolprod[boxIndex].name
        });

        this._APIwithActionService.getDatabyID(this.testcontrolprod[boxIndex].id, 'Product', 'GetAllProductByType').subscribe((data) => {
          this.ProductCntrollist = data
          // console.log(this.Instrumentlist)
      }
      ), err => {
          console.log(err);
      }
        let values: Array<any> = this.testcontrolprod[boxIndex].value;

        for (let packIndex = 0; packIndex < values.length; packIndex++) {
            this.addcontrolUsagevalue(boxIndex);
            (<FormGroup>(
                (<FormArray>(
                    (<FormGroup>(
                        (<FormArray>this.forecastproductusages.controls["ControlUsageArray"])
                            .controls[boxIndex]
                    )).controls["values"]
                )).controls[packIndex]
            )).patchValue({
                id: values[packIndex].id,
                productId: values[packIndex].productId,
                productName: values[packIndex].productName,
                instrumentId: values[packIndex].instrumentId,
                instrumentName: values[packIndex].instrumentName,
                rate: values[packIndex].rate,
                isForControl: values[packIndex].isForControl,
                testId: values[packIndex].testId

            });
        }
    }
  }


  updateExistingValueconsumbletestprd()
  {
    (<FormArray>this.forecastproductusages.controls["ConsumableTestArray"]).controls=[];
  console.log(this.consumbletestprd);
  for (let boxIndex = 0; boxIndex < this.consumbletestprd.length; boxIndex++) {
      this.addconsumableusage();
      (<FormGroup>(
          (<FormArray>this.forecastproductusages.controls["ConsumableTestArray"]).controls[
          boxIndex
          ]
      )).patchValue({
          name: this.consumbletestprd[boxIndex].name
      });
      let values: Array<any> = this.consumbletestprd[boxIndex].value;

      for (let packIndex = 0; packIndex < values.length; packIndex++) {
          this.addconsumableusagevalue(boxIndex);
          (<FormGroup>(
              (<FormArray>(
                  (<FormGroup>(
                      (<FormArray>this.forecastproductusages.controls["ConsumableTestArray"])
                          .controls[boxIndex]
                  )).controls["values"]
              )).controls[packIndex]
          )).patchValue({
              id: values[packIndex].id,
              consumableId: values[packIndex].consumableId,
              testId: values[packIndex].testId,
              productId: values[packIndex].productId,
              productName: values[packIndex].productName,
              productTypeId: values[packIndex].productTypeId,
              productTypeName: values[packIndex].productTypeName,
              instrumentId: values[packIndex].instrumentId,
              instrumentName: values[packIndex].instrumentName,
              usageRate: values[packIndex].usageRate,
              perTest: values[packIndex].perTest,
              perPeriod: values[packIndex].perPeriod,
              perInstrument: values[packIndex].perInstrument,
              noOfTest: values[packIndex].noOfTest,
              period: values[packIndex].period


          });
      }



  }
}
updateExistingValueconsumbleperiodprd()
{
  (<FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"]).controls=[];
  console.log(this.consumbleperiodprd);
  for (let boxIndex = 0; boxIndex < this.consumbleperiodprd.length; boxIndex++) {
      this.addconsumableusageperiod();
      (<FormGroup>(
          (<FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"]).controls[
          boxIndex
          ]
      )).patchValue({
          name: this.consumbleperiodprd[boxIndex].name
      });
      let values: Array<any> = this.consumbleperiodprd[boxIndex].value;

      for (let packIndex = 0; packIndex < values.length; packIndex++) {
          this.addconsumableusageperiodvalue(boxIndex);
          (<FormGroup>(
              (<FormArray>(
                  (<FormGroup>(
                      (<FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"])
                          .controls[boxIndex]
                  )).controls["values"]
              )).controls[packIndex]
          )).patchValue({
              id: values[packIndex].id,
              consumableId: values[packIndex].consumableId,
              testId: values[packIndex].testId,
              productId: values[packIndex].productId,
              productName: values[packIndex].productName,
              productTypeId: values[packIndex].productTypeId,
              productTypeName: values[packIndex].productTypeName,
              instrumentId: values[packIndex].instrumentId,
              instrumentName: values[packIndex].instrumentName,
              usageRate: values[packIndex].usageRate,
              perTest: values[packIndex].perTest,
              perPeriod: values[packIndex].perPeriod,
              perInstrument: values[packIndex].perInstrument,
              noOfTest: values[packIndex].noOfTest,
              period: values[packIndex].period


          });
      }



  }

}
updateExistingValueconsumbleinstrumentprd()
{
  (<FormArray>this.forecastproductusages.controls["ConsumableInsArray"]).controls=[];
  for (let boxIndex = 0; boxIndex < this.consumbleinstrumentprd.length; boxIndex++) {
      this.addconsumableusageins();
      (<FormGroup>(
          (<FormArray>this.forecastproductusages.controls["ConsumableInsArray"]).controls[
          boxIndex
          ]
      )).patchValue({
          name: this.consumbleinstrumentprd[boxIndex].name
      });
      let values: Array<any> = this.consumbleinstrumentprd[boxIndex].value;

      for (let packIndex = 0; packIndex < values.length; packIndex++) {
          this.addconsumableusageinsvalue(boxIndex);
          (<FormGroup>(
              (<FormArray>(
                  (<FormGroup>(
                      (<FormArray>this.forecastproductusages.controls["ConsumableInsArray"])
                          .controls[boxIndex]
                  )).controls["values"]
              )).controls[packIndex]
          )).patchValue({
              id: values[packIndex].id,
              consumableId: values[packIndex].consumableId,
              testId: values[packIndex].testId,
              productId: values[packIndex].productId,
              productName: values[packIndex].productName,
              productTypeId: values[packIndex].productTypeId,
              productTypeName: values[packIndex].productTypeName,
              instrumentId: values[packIndex].instrumentId,
              instrumentName: values[packIndex].instrumentName,
              usageRate: values[packIndex].usageRate,
              perTest: values[packIndex].perTest,
              perPeriod: values[packIndex].perPeriod,
              perInstrument: values[packIndex].perInstrument,
              noOfTest: values[packIndex].noOfTest,
              period: values[packIndex].period


          });
      }



  }
}
selectconsuminschange(args) {
  this.consuminsID = args.target.value;
  this.consuminsname = args.target.options[args.target.selectedIndex].text;
}
AddTestUsageRate() {

  if (this.testinsID > 0 && this.testproID > 0) {
      let index = this.testprod.indexOf(this.testprod.find(x => x.name == this.testinsname))
      if (index >= 0) {
          let index2 = this.testprod[index].value.indexOf(this.testprod[index].value.find(x => x.productName == this.testproname));
          if (index2 >= 0) { }
          else {

              this.testprod[index].value.push({
                  id: 0, productId: this.testproID, productName: this.testproname,
                  instrumentId: this.testinsID, instrumentName: this.testinsname, rate: 1, productUsedIn: null, isForControl: false,
                  testId: this.selectedtestID
              })
              let packIndex = this.testprod[index].value.length - 1

              let values = this.testprod[index].value
              this.addvalue(index);
              (<FormGroup>(
                  (<FormArray>(
                      (<FormGroup>(
                          (<FormArray>this.forecastproductusages.controls["ProductUsageArray"])
                              .controls[index]
                      )).controls["values"]
                  )).controls[packIndex]
              )).patchValue({
                  id: values[packIndex].id,
                  productId: values[packIndex].productId,
                  productName: values[packIndex].productName,
                  instrumentId: values[packIndex].instrumentId,
                  instrumentName: values[packIndex].instrumentName,
                  rate: values[packIndex].rate,
                  isForControl: values[packIndex].isForControl,
                  testId: values[packIndex].testId

              });
          }
      }

      else {
          this.testprod.push({
              name: this.testinsname,
              value: [{
                  id: 0, productId: this.testproID, productName: this.testproname,
                  instrumentId: this.testinsID, instrumentName: this.testinsname, rate: 1, productUsedIn: null, isForControl: false,
                  testId:this.selectedtestID
              }]

          })
          let boxIndex = this.testprod.length - 1
          this.addtestprod();
          (<FormGroup>(
              (<FormArray>this.forecastproductusages.controls["ProductUsageArray"]).controls[
              boxIndex
              ]
          )).patchValue({
              name: this.testprod[boxIndex].name
          });
          let packIndex = this.testprod[boxIndex].value.length - 1

          let values = this.testprod[boxIndex].value
          this.addvalue(boxIndex);
          (<FormGroup>(
              (<FormArray>(
                  (<FormGroup>(
                      (<FormArray>this.forecastproductusages.controls["ProductUsageArray"])
                          .controls[boxIndex]
                  )).controls["values"]
              )).controls[packIndex]
          )).patchValue({
              id: values[packIndex].id,
              productId: values[packIndex].productId,
              productName: values[packIndex].productName,
              instrumentId: values[packIndex].instrumentId,
              instrumentName: values[packIndex].instrumentName,
              rate: values[packIndex].rate,
              isForControl: values[packIndex].isForControl,
              testId: values[packIndex].testId

          });
      }

  }

}
selecttestInschange(args) {
  this.testinsID = args.target.value;
  this.testinsname = args.target.options[args.target.selectedIndex].text;

}
selectcontrolInschange(args) {
  this.controlinsID = args.target.value;
  this.controlinsname = args.target.options[args.target.selectedIndex].text;

}
selectcontrolProductchange(args) {
  this.controlproID = args.target.value;
  this.controlproname = args.target.options[args.target.selectedIndex].text;
}
AddControlUsageRate() {
  if (this.controlproID > 0 && this.controlinsID > 0) {
      let index = this.testcontrolprod.indexOf(this.testcontrolprod.find(x => x.name == this.controlinsname))
      if (index >= 0) {
          let index2 = this.testcontrolprod[index].value.indexOf(this.testcontrolprod[index].value.find(x => x.productName == this.controlproname));
          if (index2 >= 0) { }
          else {

              this.testcontrolprod[index].value.push({
                  id: 0, productId: this.controlproID, productName: this.controlproname,
                  instrumentId: this.controlinsID, instrumentName: this.controlinsname, rate: 1, productUsedIn: null, isForControl: true,
                  testId: this.selectedtestID
              })

              let packIndex = this.testcontrolprod[index].value.length - 1

              let values = this.testcontrolprod[index].value
              this.addcontrolUsagevalue(index);
              (<FormGroup>(
                  (<FormArray>(
                      (<FormGroup>(
                          (<FormArray>this.forecastproductusages.controls["ControlUsageArray"])
                              .controls[index]
                      )).controls["values"]
                  )).controls[packIndex]
              )).patchValue({
                  id: values[packIndex].id,
                  productId: values[packIndex].productId,
                  productName: values[packIndex].productName,
                  instrumentId: values[packIndex].instrumentId,
                  instrumentName: values[packIndex].instrumentName,
                  rate: values[packIndex].rate,
                  isForControl: values[packIndex].isForControl,
                  testId: values[packIndex].testId

              });
          }
      }
      else {
          this.testcontrolprod.push({
              name: this.controlinsname,
              value: [{
                  id: 0, productId: this.controlproID, productName: this.controlproname,
                  instrumentId: this.controlinsID, instrumentName: this.controlinsname, rate: 1, productUsedIn: null, isForControl: true,
                  testId: this.selectedtestID
              }]

          })

          let boxIndex = this.testcontrolprod.length - 1
          this.addcontrolusage();
          (<FormGroup>(
              (<FormArray>this.forecastproductusages.controls["ControlUsageArray"]).controls[
              boxIndex
              ]
          )).patchValue({
              name: this.testcontrolprod[boxIndex].name
          });
          let packIndex = this.testcontrolprod[boxIndex].value.length - 1

          let values = this.testcontrolprod[boxIndex].value
          this.addcontrolUsagevalue(boxIndex);
          (<FormGroup>(
              (<FormArray>(
                  (<FormGroup>(
                      (<FormArray>this.forecastproductusages.controls["ControlUsageArray"])
                          .controls[boxIndex]
                  )).controls["values"]
              )).controls[packIndex]
          )).patchValue({
              id: values[packIndex].id,
              productId: values[packIndex].productId,
              productName: values[packIndex].productName,
              instrumentId: values[packIndex].instrumentId,
              instrumentName: values[packIndex].instrumentName,
              rate: values[packIndex].rate,
              isForControl: values[packIndex].isForControl,
              testId: values[packIndex].testId

          });
      }
  }
}

AddconsumtestUsageRate() {
  if (this.forecastproductusages.controls["nooftest"].value != "" && this.consumtestproID > 0) {
      let index = this.consumbletestprd.indexOf(this.consumbletestprd.find(x => x.name == this.consumtesttypename))
      if (index >= 0) {
          let index2 = this.consumbletestprd[index].value.indexOf(this.consumbletestprd[index].value.find(x => x.productName == this.consumtestproname));
          if (index2 >= 0) { }
          else {

              this.consumbletestprd[index].value.push({
                  id: 0,
                  testId: this.selectedtestID, productId: this.consumtestproID, productName: this.consumtestproname, productTypeId: this.consumtesttypeID, productTypeName: this.consumtesttypename,
                  instrumentId: null, instrumentName: null, usageRate: 1.0000000000, perTest: true, perPeriod: false, perInstrument: false, noOfTest: this.forecastproductusages.controls["nooftest"].value, period: null
              })

              let packIndex = this.consumbletestprd[index].value.length - 1

              let values = this.consumbletestprd[index].value
              this.addconsumableusagevalue(index);
              (<FormGroup>(
                  (<FormArray>(
                      (<FormGroup>(
                          (<FormArray>this.forecastproductusages.controls["ConsumableTestArray"])
                              .controls[index]
                      )).controls["values"]
                  )).controls[packIndex]
              )).patchValue({

                  id: values[packIndex].id,
                  consumableId: values[packIndex].consumableId,
                  testId: values[packIndex].testId,
                  productId: values[packIndex].productId,
                  productName: values[packIndex].productName,
                  productTypeId: values[packIndex].productTypeId,
                  productTypeName: values[packIndex].productTypeName,
                  instrumentId: values[packIndex].instrumentId,
                  instrumentName: values[packIndex].instrumentName,
                  usageRate: values[packIndex].usageRate,
                  perTest: values[packIndex].perTest,
                  perPeriod: values[packIndex].perPeriod,
                  perInstrument: values[packIndex].perInstrument,
                  noOfTest: values[packIndex].noOfTest,
                  period: values[packIndex].period,

              });
          }
      }
      else {
          this.consumbletestprd.push({
              name: this.consumtesttypename,
              value: [{
                  id: 0,
                  testId: this.selectedtestID, productId: this.consumtestproID, productName: this.consumtestproname, productTypeId: this.consumtesttypeID, productTypeName: this.consumtesttypename,
                  instrumentId: null, instrumentName: null, usageRate: 1.0000000000, perTest: true, perPeriod: false, perInstrument: false, noOfTest: this.forecastproductusages.controls["nooftest"].value, period: null

              }]

          })


          let boxIndex = this.consumbletestprd.length - 1
          this.addconsumableusage();
          (<FormGroup>(
              (<FormArray>this.forecastproductusages.controls["ConsumableTestArray"]).controls[
              boxIndex
              ]
          )).patchValue({
              name: this.consumbletestprd[boxIndex].name
          });
          let packIndex = this.consumbletestprd[boxIndex].value.length - 1

          let values = this.consumbletestprd[boxIndex].value
          this.addconsumableusagevalue(boxIndex);
          (<FormGroup>(
              (<FormArray>(
                  (<FormGroup>(
                      (<FormArray>this.forecastproductusages.controls["ConsumableTestArray"])
                          .controls[boxIndex]
                  )).controls["values"]
              )).controls[packIndex]
          )).patchValue({

              id: values[packIndex].id,
              consumableId: values[packIndex].consumableId,
              testId: values[packIndex].testId,
              productId: values[packIndex].productId,
              productName: values[packIndex].productName,
              productTypeId: values[packIndex].productTypeId,
              productTypeName: values[packIndex].productTypeName,
              instrumentId: values[packIndex].instrumentId,
              instrumentName: values[packIndex].instrumentName,
              usageRate: values[packIndex].usageRate,
              perTest: values[packIndex].perTest,
              perPeriod: values[packIndex].perPeriod,
              perInstrument: values[packIndex].perInstrument,
              noOfTest: values[packIndex].noOfTest,
              period: values[packIndex].period,

          });
      }
  }

}
AddconsumperiodUsageRate() {
  if (this.consumconproID > 0 && this.consumconperiodID > 0) {
      let index = this.consumbleperiodprd.indexOf(this.consumbleperiodprd.find(x => x.name == this.consumcontypename))
      if (index >= 0) {
          let index2 = this.consumbleperiodprd[index].value.indexOf(this.consumbleperiodprd[index].value.find(x => x.productName == this.consumconproname));
          if (index2 >= 0) { }
          else {

              this.consumbleperiodprd[index].value.push({
                  id: 0,
                  testId: this.selectedtestID, productId: this.consumconproID, productName: this.consumconproname, productTypeId: this.consumcontypeID, productTypeName: this.consumcontypename,
                  instrumentId: null, instrumentName: null, usageRate: 1.0000000000, perTest: false, perPeriod: true, perInstrument: false, noOfTest: 0, period: this.consumconperiod
              })
              let packIndex = this.consumbleperiodprd[index].value.length - 1

              let values = this.consumbleperiodprd[index].value
              this.addconsumableusageperiodvalue(index);
              (<FormGroup>(
                  (<FormArray>(
                      (<FormGroup>(
                          (<FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"])
                              .controls[index]
                      )).controls["values"]
                  )).controls[packIndex]
              )).patchValue({

                  id: values[packIndex].id,
                  consumableId: values[packIndex].consumableId,
                  testId: values[packIndex].testId,
                  productId: values[packIndex].productId,
                  productName: values[packIndex].productName,
                  productTypeId: values[packIndex].productTypeId,
                  productTypeName: values[packIndex].productTypeName,
                  instrumentId: values[packIndex].instrumentId,
                  instrumentName: values[packIndex].instrumentName,
                  usageRate: values[packIndex].usageRate,
                  perTest: values[packIndex].perTest,
                  perPeriod: values[packIndex].perPeriod,
                  perInstrument: values[packIndex].perInstrument,
                  noOfTest: values[packIndex].noOfTest,
                  period: values[packIndex].period,

              });
          }
      }
      else {
          this.consumbleperiodprd.push({
              name: this.consumcontypename,
              value: [{
                  id: 0,
                  testId: this.selectedtestID, productId: this.consumconproID, productName: this.consumconproname, productTypeId: this.consumcontypeID, productTypeName: this.consumcontypename,
                  instrumentId: null, instrumentName: null, usageRate: 1.0000000000, perTest: false, perPeriod: true, perInstrument: false, noOfTest: 0, period: this.consumconperiod
              }]
          })

          let boxIndex = this.consumbleperiodprd.length - 1
          this.addconsumableusageperiod();
          (<FormGroup>(
              (<FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"]).controls[
              boxIndex
              ]
          )).patchValue({
              name: this.consumbleperiodprd[boxIndex].name
          });
          let packIndex = this.consumbleperiodprd[boxIndex].value.length - 1

          let values = this.consumbleperiodprd[boxIndex].value
          this.addconsumableusageperiodvalue(boxIndex);
          (<FormGroup>(
              (<FormArray>(
                  (<FormGroup>(
                      (<FormArray>this.forecastproductusages.controls["ConsumablePeriodArray"])
                          .controls[boxIndex]
                  )).controls["values"]
              )).controls[packIndex]
          )).patchValue({

              id: values[packIndex].id,
              consumableId: values[packIndex].consumableId,
              testId: values[packIndex].testId,
              productId: values[packIndex].productId,
              productName: values[packIndex].productName,
              productTypeId: values[packIndex].productTypeId,
              productTypeName: values[packIndex].productTypeName,
              instrumentId: values[packIndex].instrumentId,
              instrumentName: values[packIndex].instrumentName,
              usageRate: values[packIndex].usageRate,
              perTest: values[packIndex].perTest,
              perPeriod: values[packIndex].perPeriod,
              perInstrument: values[packIndex].perInstrument,
              noOfTest: values[packIndex].noOfTest,
              period: values[packIndex].period,

          });
      }
  }
}
selectconsuminsprochange(args) {
  this.consuminsproID = args.target.value;
  this.consuminsproname = args.target.options[args.target.selectedIndex].text;
}
selectconsuminsperiodchange(args) {
  this.consuminsperiodID = args.target.value;
  this.consuminsperiodname = args.target.options[args.target.selectedIndex].text;
}
selectconsumperiodproductchange(args) {
  this.consumconproID = args.target.value;
  this.consumconproname = args.target.options[args.target.selectedIndex].text;

}

selectconsumtestproductchange(args) {
  this.consumtestproID = args.target.value;
  this.consumtestproname = args.target.options[args.target.selectedIndex].text;
}
selectconsumperiodchange(args) {
  this.consumconperiodID = args.target.value;
  this.consumconperiod = args.target.options[args.target.selectedIndex].text;

}
AddconsuminsUsageRate() {
  if (this.consuminsproID > 0 && this.consuminsperiodID > 0) {
      let index = this.consumbleinstrumentprd.indexOf(this.consumbleinstrumentprd.find(x => x.name == this.consuminsname))
      if (index >= 0) {
          let index2 = this.consumbleinstrumentprd[index].value.indexOf(this.consumbleinstrumentprd[index].value.find(x => x.productName == this.consuminsproname));
          if (index2 >= 0) { }
          else {

              this.consumbleinstrumentprd[index].value.push({
                  id: 0,
                  testId: this.selectedtestID, productId: this.consuminsproID, productName: this.consuminsproname, productTypeId: 0, productTypeName: null,
                  instrumentId: this.consuminsID, instrumentName: this.consuminsname, usageRate: 1.0000000000, perTest: false, perPeriod: false, perInstrument: true, noOfTest: 0, period: this.consuminsperiodname
              })


              let packIndex = this.consumbleinstrumentprd[index].value.length - 1

              let values = this.consumbleinstrumentprd[index].value
              this.addconsumableusageinsvalue(index);
              (<FormGroup>(
                  (<FormArray>(
                      (<FormGroup>(
                          (<FormArray>this.forecastproductusages.controls["ConsumableInsArray"])
                              .controls[index]
                      )).controls["values"]
                  )).controls[packIndex]
              )).patchValue({

                  id: values[packIndex].id,
                  consumableId: values[packIndex].consumableId,
                  testId: values[packIndex].testId,
                  productId: values[packIndex].productId,
                  productName: values[packIndex].productName,
                  productTypeId: values[packIndex].productTypeId,
                  productTypeName: values[packIndex].productTypeName,
                  instrumentId: values[packIndex].instrumentId,
                  instrumentName: values[packIndex].instrumentName,
                  usageRate: values[packIndex].usageRate,
                  perTest: values[packIndex].perTest,
                  perPeriod: values[packIndex].perPeriod,
                  perInstrument: values[packIndex].perInstrument,
                  noOfTest: values[packIndex].noOfTest,
                  period: values[packIndex].period,

              });
          }
      }
      else {
          this.consumbleinstrumentprd.push({
              name: this.consuminsname,
              value: [{
                  id: 0,
                  testId: this.selectedtestID, productId: this.consuminsproID, productName: this.consuminsproname, productTypeId: 0, productTypeName: null,
                  instrumentId: this.consuminsID, instrumentName: this.consuminsname, usageRate: 1.0000000000, perTest: false, perPeriod: false, perInstrument: true, noOfTest: 0, period: this.consuminsperiodname
              }]

          })


          let boxIndex = this.consumbleinstrumentprd.length - 1
          this.addconsumableusageins();
          (<FormGroup>(
              (<FormArray>this.forecastproductusages.controls["ConsumableInsArray"]).controls[
              boxIndex
              ]
          )).patchValue({
              name: this.consumbleinstrumentprd[boxIndex].name
          });
          let packIndex = this.consumbleinstrumentprd[boxIndex].value.length - 1

          let values = this.consumbleinstrumentprd[boxIndex].value
          this.addconsumableusageinsvalue(boxIndex);
          (<FormGroup>(
              (<FormArray>(
                  (<FormGroup>(
                      (<FormArray>this.forecastproductusages.controls["ConsumableInsArray"])
                          .controls[boxIndex]
                  )).controls["values"]
              )).controls[packIndex]
          )).patchValue({

              id: values[packIndex].id,
              consumableId: values[packIndex].consumableId,
              testId: values[packIndex].testId,
              productId: values[packIndex].productId,
              productName: values[packIndex].productName,
              productTypeId: values[packIndex].productTypeId,
              productTypeName: values[packIndex].productTypeName,
              instrumentId: values[packIndex].instrumentId,
              instrumentName: values[packIndex].instrumentName,
              usageRate: values[packIndex].usageRate,
              perTest: values[packIndex].perTest,
              perPeriod: values[packIndex].perPeriod,
              perInstrument: values[packIndex].perInstrument,
              noOfTest: values[packIndex].noOfTest,
              period: values[packIndex].period,

          });
      }
  }
}

}
