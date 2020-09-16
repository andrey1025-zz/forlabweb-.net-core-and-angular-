import { Component, OnInit, ViewChild, Output, EventEmitter, Input } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDirective } from "ngx-bootstrap";
import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
//import { DemographicwizardComponent } from '../demographicwizard/demographicwizard.component';
@Component({
  selector: 'app-testingprotocol',
  templateUrl: './testingprotocol.component.html'

})
export class TestingprotocolComponent implements OnInit {
  forecastid: number;
  invalidsiteList=new Array();
  testingAreaList = new Array();
  TestList = new Array();
  Selectedtestlist = new Array();
  Addtestlist = new Array();
  HeaderArray=new Array();
  oldAddtestlist = new Array();
  testingprotocol: FormGroup;
  checktest: boolean;
  controlArray = new Array();
  parameterlist = new Array()
  disableinput: boolean;
  testname: string;
  UserId: number = 0;
  testingprotocollist = new Array();
  Programid: number;
  testingprotocolvalue = new Array();
  selectedareaid:number=0;
  selectedtestid:number=0;
  @Input() RecforecastID:number;
  @Output()
  nextStep = new EventEmitter<string>();

  @ViewChild('mdModal') public mdModal: ModalDirective;
  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {


    
  }
  validate(group: FormGroup)
  {
    if(group.value.percentagePanel>100)
    {
      this._GlobalAPIService.FailureMessage("Percentage should not be greater than 100");
      group.patchValue({
        percentagePanel:0
      })
    }
console.log(group);
  }
  selectalltest(isChecked: boolean) {

    if (isChecked) {
      this.Selectedtestlist = this.TestList;
      this.checktest = true;
    }
    else {
      this.checktest = false;
      if (this.Selectedtestlist.length > 0) {
        this.Selectedtestlist.splice(0, this.Selectedtestlist.length)
      }

    }
  }
  Addselectedtest() {
    if (this.Selectedtestlist.length>0)
    {
     
    }
    this.Selectedtestlist.forEach(element => {
      if (this.Addtestlist.findIndex(x => x.testID == element.testID) < 0)
      {
        this.Addtestlist.push(element);
      }
    });
   // this.Addtestlist=this.Selectedtestlist;
  }
  deltest(testID:FormGroup,index:number)
  {
  console.log(testID.controls["testID"].value)
    this._APIwithActionService.deleteData(testID.controls["testID"].value,"Assumption","deletetestingprotocol","param="+this.forecastid+","+testID.controls["patientGroupID"].value).subscribe((data)=>{
      if (this.Addtestlist.findIndex(x => x.testID == testID.controls["testID"].value) >= 0) 
      {
        this.Addtestlist.splice(this.Addtestlist.findIndex(x => x.testID == testID.controls["testID"].value), 1)
  
    
      }

      (<FormArray>this.testingprotocol.controls["_testingprotocol"]).removeAt(index);


    })
  }
  inputvalue(args,i,datatype)
  {
    let name=args.target.name;

    if (name=="percentagePanel")
    {
      if (args.target.value>100)
      {
      this._GlobalAPIService.FailureMessage("Percentage should not be greater than 100");
      (<FormArray>(this.testingprotocol.controls["_testingprotocol"])).controls[i].patchValue({
        percentagePanel:0
      })
    }
  }
    else if (datatype==2)
    {
      if (args.target.value>100)
      {
      this._GlobalAPIService.FailureMessage("Percentage should not be greater than 100");
      (<FormArray>(this.testingprotocol.controls["_testingprotocol"])).controls[i].patchValue({
        [name]:0
      })
    }

 

    }
  //  alert(name);
   
  }
  savedata() {
    let ss = <FormArray>this.testingprotocol.controls["_testingprotocol"];


    this.testingprotocollist.splice(0, this.testingprotocollist.length);
    this.testingprotocolvalue.splice(0, this.testingprotocolvalue.length);
    ss.getRawValue().forEach(x => {
      if (x.testID > 0) {
        this.testingprotocollist.push({
          ID: x.id,
          TestID: x.testID,
          PatientGroupID: x.patientGroupID,
          ForecastinfoID: x.forecastinfoID,
          PercentagePanel: x.percentagePanel,
          TotalTestPerYear: x.totalTestPerYear,
          Baseline:x.baseline,
          UserId: this.UserId

        })
        for (let index = 0; index < this.controlArray.length; index++) {
          if (this.controlArray[index].type == "number" && this.controlArray[index].id != 0) {
            this.testingprotocolvalue.push({

              Parameterid: this.controlArray[index].id,
              Parametervalue: x[this.controlArray[index].name],
              Forecastid: x.forecastinfoID,
              TestID: x.testID,
              PatientGroupID: x.patientGroupID,
              UserId: this.UserId
            })

          }

        }

      }
      else {
        return;
      }
    });
    if (this.testingprotocollist.length > 0) {
      this._APIwithActionService.postAPI(this.testingprotocollist, "Assumption", "Savetestingprotocol").subscribe((data) => {

        this._APIwithActionService.postAPI(this.testingprotocolvalue, 'Assumption', 'savetestinggeneralassumptionvalue').subscribe((data) => {

          if (data["_body"] != 0) {
           // this._GlobalAPIService.SuccessMessage("Testing Protocol Saved Successfully");
          }

        }
        )
      })
    }
  }
  savetestingprotocol() {
    this.savedata();
    this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','vaidationforsiteinstrument').subscribe((data)=>
    {
        if (data.length==0)
        {
          this.nextStep.emit('step4,N,'+this.forecastid);
          //this._router.navigate(['Demographic/PatientAssumption', this.Programid, this.forecastid])
        }
        else{
          this.invalidsiteList=data;
          this.mdModal.show();
        }
    }
    )
 
  }
  Redirecttopatientassumption()
  {
    this.mdModal.hide();
    this.nextStep.emit('step4,N,'+this.forecastid);
   // this._router.navigate(['Demographic/PatientAssumption', this.Programid, this.forecastid])
  }
  addtestdata() {
    let newarray = (<FormArray>this.testingprotocol.controls["_testingprotocol"]).controls;
    let ss = <FormArray>this.testingprotocol.controls["_testingprotocol"];
    let isexist:boolean=false;
console.log(ss);
    for (let index = 0; index < newarray.length; index++) {
      if ((<FormGroup>newarray[index]).controls.testID.value == this.selectedtestid) {
        isexist = true
        break;
      }
      else {
        isexist = false
      }

    }
    if (isexist==false)
    {
    this.savedata();
    this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GettestAssumption', 'testid=' + this.selectedtestid
    ).subscribe((data) => {
   
      this.parameterlist = data;
      console.log(this.parameterlist)
     // ss.controls = [];
      for (let index = 0; index < this.parameterlist.length; index++) {
        ss.push(this._fb.group(this.parameterlist[index]))


      }

    })


    // for (let index = 0; index < ss.length; index++) {
    //   ss.controls[index].setValue({
    //     testID: data.testid

    //   })

    // }
  }
  
  }
  ontextchange(data: any) {
 this.selectedtestid=data;
  }
  getTestingArea() {
    this._GlobalAPIService.getDataList('TestArea').subscribe((data) => {
      this.testingAreaList = data.aaData
      //console.log(this.Instrumentlist)
    }
    ), err => {
      console.log(err);
    }


  }

  gettest(AreaID) {
    this._APIwithActionService.getDatabyID(AreaID, 'Test', 'GetAllTestsByAreaId').subscribe((data) => {
      this.TestList = data
    })
this.selectedareaid=AreaID;
  }
  ngOnInit() {
    if (this._avRoute.snapshot.params["id1"]) {
      this.Programid = this._avRoute.snapshot.params["id1"];

    }
    if (this._avRoute.snapshot.params["id2"]) {
      this.forecastid = this._avRoute.snapshot.params["id2"];

    }
    if (this.RecforecastID>0)
    {
          this.forecastid = this.RecforecastID
    }
    
  if (this.forecastid > 0) {
   
    this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GettestAssumption','testid=0').subscribe((data1) => {

      this.parameterlist = data1;
      this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GetDynamiccontrol', 'entitytype=5').subscribe((data) => {
        this.controlArray = data;
        this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'getdynamicheader', 'entitytype=5').subscribe((data2) => {
          this.HeaderArray = data2
        console.log(this.parameterlist)
        let ss = <FormArray>this.testingprotocol.controls["_testingprotocol"];
        ss.controls=[];
        for (let index = 0; index < this.parameterlist.length; index++) {
      ss.push(this._fb.group(this.parameterlist[index]//, {
          //   validator: this.validate.bind(this),
         
          // }
          ))
     

        }
      })


      })

      // 

      //    
    })
    for (let index = 0; index < this.controlArray.length; index++) {
      if (this.controlArray[index].type == "text") {
        this.disableinput = true
      }
    }
    this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'Gettestfromtestingprotocol').subscribe((data) => {
      this.Addtestlist = data;
    })
  }
  this.getTestingArea();
    this.testingprotocol = this._fb.group({
      _testingprotocol: this._fb.array([]),


    })
  }
  Previousclick()
  {
    this.nextStep.emit('step3,P,'+this.forecastid);
    //this._router.navigate(['Demographic/PatientGroupRatio', this.Programid, this.forecastid])
  }
}
