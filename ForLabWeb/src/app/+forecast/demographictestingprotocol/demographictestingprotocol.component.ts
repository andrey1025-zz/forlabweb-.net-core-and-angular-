import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { ModalDirective } from "ngx-bootstrap";
import {NotificationService} from '../../shared/utils/notification.service';
import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
@Component({
  selector: 'app-demographictestingprotocol',
  templateUrl: './demographictestingprotocol.component.html',
  styleUrls: ['./demographictestingprotocol.component.css']
})
export class DemographictestingprotocolComponent implements OnInit {
  forecastid: number;
  variableHeaderArray = new Array();
  invalidsiteList = new Array();
  testingAreaList = new Array();
  TestList = new Array();
  Selectedtestlist = new Array();
  Addtestlist = new Array();
  HeaderArray = new Array();
  oldAddtestlist = new Array();
  demographictestingprotocol: FormGroup;

  demographictestingprotocolmodel: FormGroup;
  checktest: boolean;
  controlArray = new Array();
  parameterlist = new Array()
  disableinput: boolean;
  testname: string;
  UserId: number = 0;
  testingprotocollist = new Array();
  Programid: number;
  testingprotocolvalue = new Array();
  selectedareaid: number = 0;
  selectedtestid: number = 0;

  selectedtestid1: number = 0;
  selectedpatientgroupid: number = 0;
  parameterlistvar = new Array();
  // @Input() RecforecastID:number;
  // @Output()
  // nextStep = new EventEmitter<string>();

  @ViewChild('mdModal') public mdModal: ModalDirective;
  @ViewChild('lgModal4') public lgModal4: ModalDirective;
  constructor(private notificationService:NotificationService,private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {



  }
  validate(group: FormGroup) {
    if (group.value.percentagePanel > 100) {
      this._GlobalAPIService.FailureMessage("Percentage should not be greater than 100");
      group.patchValue({
        percentagePanel: 0
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
    if (this.Selectedtestlist.length > 0) {

    }
    this.Selectedtestlist.forEach(element => {
      if (this.Addtestlist.findIndex(x => x.testID == element.testID) < 0) {
        this.Addtestlist.push(element);
      }
    });
    // this.Addtestlist=this.Selectedtestlist;
  }
  deltest(testID: FormGroup, index: number) {
    console.log(testID.controls["testID"].value)
    this._APIwithActionService.deleteData(testID.controls["testID"].value, "Assumption", "deletetestingprotocol", "param=" + this.forecastid + "," + testID.controls["patientGroupID"].value).subscribe((data) => {
      if (this.Addtestlist.findIndex(x => x.testID == testID.controls["testID"].value) >= 0) {
        this.Addtestlist.splice(this.Addtestlist.findIndex(x => x.testID == testID.controls["testID"].value), 1)


      }

      (<FormArray>this.demographictestingprotocol.controls["_testingprotocol"]).removeAt(index);


    })
  }

  edit(testID: FormGroup, index: number) {
    let ss = <FormArray>this.demographictestingprotocolmodel.controls["_testingprotocolVariable"];
    ss.controls = [];
    this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GettestforecastAssumptionnewbytestId', 'param=' + testID.controls["testID"].value + "," + testID.controls["patientGroupID"].value).subscribe((data2) => {
      this.parameterlistvar = data2;
      console.log(this.parameterlistvar)
      ss.controls = [];
      for (let index = 0; index < this.parameterlistvar.length; index++) {
        ss.push(this._fb.group(this.parameterlistvar[index]))


      }
    })

    this.selectedpatientgroupid = testID.controls["patientGroupID"].value;
    this.selectedtestid1 = testID.controls["testID"].value;
    console.log(ss);
    this.lgModal4.show();


  }
  addgeneralassumption() {

    let ss = <FormArray>this.demographictestingprotocolmodel.controls["_testingprotocolVariable"];
    console.log(ss)
    ss.getRawValue().forEach(x => {


      let index = this.testingprotocolvalue.findIndex(z => z.Parameterid == x.id && z.TestID == this.selectedtestid1 && z.PatientGroupID == this.selectedpatientgroupid)
      if (index < 0) {
        this.testingprotocolvalue.push({

          Parameterid: x.id,
          Parametervalue: parseFloat(x.value),
          Forecastid: this.forecastid,
          TestID: this.selectedtestid1,
          PatientGroupID: this.selectedpatientgroupid,
          UserId: this.UserId
        })
      }
      else {
        this.testingprotocolvalue[index].Parametervalue = parseFloat(x.value);
      }
    })


    if (this.testingprotocolvalue.length > 0) {
      this._APIwithActionService.postAPI(this.testingprotocolvalue, 'Assumption', 'savetestinggeneralassumptionvalue').subscribe((data) => {

        if (data["_body"] != 0) {
          // this._GlobalAPIService.SuccessMessage("Testing Protocol Saved Successfully");
        }

      }
      )
    }
    this.lgModal4.hide();
    ss.controls=[];
    console.log(this.testingprotocolvalue)
  }
  inputvalue(args, i, datatype) {
    let name = args.target.name;

    if (name == "percentagePanel") {
      if (args.target.value > 100) {
        this._GlobalAPIService.FailureMessage("Percentage should not be greater than 100");
        (<FormArray>(this.demographictestingprotocol.controls["_testingprotocol"])).controls[i].patchValue({
          percentagePanel: 0
        })
      }
    }
    else if (datatype == 2) {
      if (args.target.value > 100) {
        this._GlobalAPIService.FailureMessage("Percentage should not be greater than 100");
        (<FormArray>(this.demographictestingprotocol.controls["_testingprotocol"])).controls[i].patchValue({
          [name]: 0
        })
      }



    }
    //  alert(name);

  }
  savedata() {
    let ss = <FormArray>this.demographictestingprotocol.controls["_testingprotocol"];


    this.testingprotocollist.splice(0, this.testingprotocollist.length);
    //  this.testingprotocolvalue.splice(0, this.testingprotocolvalue.length);
    ss.getRawValue().forEach(x => {
      if (x.testID > 0) {
        this.testingprotocollist.push({
          ID: x.id,
          TestID: x.testID,
          PatientGroupID: x.patientGroupID,
          ForecastinfoID: x.forecastinfoID,
          PercentagePanel: x.percentagePanel,
          TotalTestPerYear: x.totalTestPerYear,
          Baseline: x.baseline,
          UserId: this.UserId

        })
        // for (let index = 0; index < this.controlArray.length; index++) {
        //   if (this.controlArray[index].type == "number" && this.controlArray[index].id != 0) {
        //     this.testingprotocolvalue.push({

        //       Parameterid: this.controlArray[index].id,
        //       Parametervalue: x[this.controlArray[index].name],
        //       Forecastid: x.forecastinfoID,
        //       TestID: x.testID,
        //       PatientGroupID: x.patientGroupID,
        //       UserId: this.UserId
        //     })

        //   }

        // }

      }
      else {
        return;
      }
    });
    if (this.testingprotocollist.length > 0) {
      this._APIwithActionService.postAPI(this.testingprotocollist, "Assumption", "Savetestingprotocol").subscribe((data) => {
        // if (this.testingprotocolvalue.length > 0) {
        //   this._APIwithActionService.postAPI(this.testingprotocolvalue, 'Assumption', 'savetestinggeneralassumptionvalue').subscribe((data) => {

        //     if (data["_body"] != 0) {
        //       // this._GlobalAPIService.SuccessMessage("Testing Protocol Saved Successfully");
        //     }

        //   }
        //   )
        // }
      })
    }
  }
  savetestingprotocol() {
    this.savedata();
    this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'vaidationforsiteinstrument').subscribe((data) => {
      if (data.length == 0) {
        // this.nextStep.emit('step4,N,'+this.forecastid);
        //this._router.navigate(['Demographic/PatientAssumption', this.Programid, this.forecastid])
      }
      else {
        this.invalidsiteList = data;
        this.mdModal.show();
      }
    }


    )
    this._router.navigate(['Forecast/Demographicpatient', this.forecastid, this.Programid])
  }

  Previousclick()
  {
    this.notificationService.smallBox({
      title: "Conformation!",
      content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/Demographipatientgroup/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
      color: "#296191",
      //timeout: 8000,
      icon: "fa fa-bell swing animated"
    });
  }
  Redirecttopatientassumption() {
    this.mdModal.hide();
    // this.nextStep.emit('step4,N,'+this.forecastid);
    // this._router.navigate(['Demographic/PatientAssumption', this.Programid, this.forecastid])
  }
  addtestdata() {
    let newarray = (<FormArray>this.demographictestingprotocol.controls["_testingprotocol"]).controls;
    let ss = <FormArray>this.demographictestingprotocol.controls["_testingprotocol"];
    let isexist: boolean = false;
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
    if (isexist == false) {
      this.savedata();
      this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GettestforecastAssumption', 'testid=' + this.selectedtestid
      ).subscribe((data11) => {


        this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GetforecastDynamiccontrol', 'entitytype=5').subscribe((data) => {
          this.controlArray = data;
          console.log(this.controlArray)
          this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'getdynamicheader', 'entitytype=5').subscribe((data2) => {
            this.HeaderArray = data2

            this.parameterlist = data11;
            console.log(this.parameterlist)
            ss.controls = [];
            for (let index = 0; index < this.parameterlist.length; index++) {
              ss.push(this._fb.group(this.parameterlist[index]))


            }
          })
        })

      })


      // for (let index = 0; index < ss.length; index++) {
      //   ss.controls[index].setValue({
      //     testID: data.testid

      //   })

      // }
    }

  }
  ontextchange(data: any) {
    this.selectedtestid = data;
    //this.addtestdata() 
  }

  gettest() {
    this._APIwithActionService.getDatabyID(this.forecastid, 'Test', 'GetAllTestsByforecastid').subscribe((data) => {
      this.TestList = data
    })

  }

  ngOnInit() {
    if (this._avRoute.snapshot.params["pid"]) {
      this.Programid = this._avRoute.snapshot.params["pid"];

    }
    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];

    }
    this.gettest();
    this.demographictestingprotocolmodel = this._fb.group({
      _testingprotocolVariable: this._fb.array([]),


    })
    // if (this.RecforecastID>0)
    // {
    //       this.forecastid = this.RecforecastID
    // }
    // let ss = <FormArray>this.demographictestingprotocol.controls["_testingprotocol"];
    if (this.forecastid > 0) {

      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'vaidationforsiteinstrument').subscribe((data) => {
      });
      //let ss = <FormArray>this.demographictestingprotocol.controls["_testingprotocol"];
      this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GettestforecastAssumptionnew').subscribe((data11) => {


        this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GetforecastDynamiccontrol', 'entitytype=5').subscribe((data) => {
          this.controlArray = data;
          console.log(this.controlArray)
          this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'getdynamicheader', 'entitytype=5').subscribe((data2) => {
            this.HeaderArray = data2

            this.parameterlist = data11;
            console.log(this.parameterlist)
            let ss = <FormArray>this.demographictestingprotocol.controls["_testingprotocol"];
            ss.controls = [];
            for (let index = 0; index < this.parameterlist.length; index++) {
              ss.push(this._fb.group(this.parameterlist[index]))


            }
          })
        })

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

    this.demographictestingprotocol = this._fb.group({
      _testingprotocol: this._fb.array([]),


    })
  }
 
}
