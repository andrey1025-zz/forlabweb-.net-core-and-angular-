import { Component, OnInit,ViewChild } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Globals } from '../../shared/Globals';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, FormArray, Validators, NgForm } from "@angular/forms";
import { formatDate } from 'ngx-bootstrap/chronos';
import {PatientGroupRatioComponent} from '../patient-group-ratio/patient-group-ratio.component';
import {AggregrateforecastComponent} from '../aggregrateforecast/aggregrateforecast.component';
import {TestingprotocolComponent} from '../testingprotocol/testingprotocol.component';
import {PatientAssumptionComponent} from '../patient-assumption/patient-assumption.component';
import {ProductassumptionComponent} from '../productassumption/productassumption.component';
import {LineargrowthComponent} from '../lineargrowth/lineargrowth.component';
import {ForecastchartComponent} from '../forecastchart/forecastchart.component';
import * as XLSX from 'xlsx';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse, HttpHeaders } from '@angular/common/http'
import { ModalDirective } from 'ngx-bootstrap';
// import{DemograhicListComponent} from '../DemographicList/DemographicList.component';
// import{DemographicAddComponent} from '../DemographicAdd/DemographicAdd.component';
@Component({
  selector: 'app-demographicwizard',
  templateUrl: './demographicwizard.component.html',
  styleUrls: ['./demographicwizard.component.css']
})
export class DemographicwizardComponent implements OnInit {
  demosettingadd: FormGroup;
  FormTitle: string = "";
  Foracstinfoobj: Object;
  classname: string = "";
  programid: number = 0;
  forecastid: number = 0;
  type:string;
  date: Date;
  RegionList = new Array();
  Forecasttype: string = "A";
  static forecastid1;
  Demographicmenu=new Array;
  model: any
  programForm: NgForm;
  wb: XLSX.WorkBook;
  Sheetarr = new Array();
  arrayBuffer: any;
  file: File;
  formData = new FormData();
  @ViewChild(PatientGroupRatioComponent) PatientGroup: PatientGroupRatioComponent;
  @ViewChild(TestingprotocolComponent) Testingprotocol: TestingprotocolComponent;
  @ViewChild(PatientAssumptionComponent) PatientAssumption: PatientAssumptionComponent;
  @ViewChild(ProductassumptionComponent) Productassumption: ProductassumptionComponent;
  @ViewChild(LineargrowthComponent) lineargrowth: LineargrowthComponent;
  @ViewChild(ForecastchartComponent) forecastchart: ForecastchartComponent;
  @ViewChild(AggregrateforecastComponent) Aggregate: AggregrateforecastComponent;
  @ViewChild('lgModal3') public lgModal3: ModalDirective;//for import
  // @ViewChild(DemograhicListComponent) Demographiclist:DemograhicListComponent;
  // @ViewChild(DemographicAddComponent) DemographicAdd:DemographicAddComponent;
  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService, private _Globals: Globals,private http:HttpClient) {
      if (this._avRoute.snapshot.params["id1"]) {
        this.programid = this._avRoute.snapshot.params["id1"];
  
      }
      if (this._avRoute.snapshot.params["F"]) {  
        this.type = this._avRoute.snapshot.params["F"]; 
      } 
      if(this.type=="S")
      {
        this.Forecasttype = "S";
        this.activeStep = this.steps[1]
      }
      else if (this.type  == "step1A") {
        this.Forecasttype = "A";
      
        this.activeStep = this.steps[1]
    this.Aggregate.forecastid= this.forecastid;
    this.Aggregate.ngOnInit();
      }
      this._APIwithActionService.getDataList('MMProgram',"Get").subscribe((data)=>{
        this.Demographicmenu=data;
      
        }
        )
       
  }
 
  public steps = [
    // {
    //   key: 'step0',
    //   title: 'Select Program',
    //   valid: false,
    //   checked: false,
    //   submitted: false,
    // },
    // {
    //   key: 'step11',
    //   title: 'List of Forecast',
    //   valid: false,
    //   checked: false,
    //   submitted: false,
    // },
    {
      key: 'step1',
      title: 'Forecast Definition',
      valid: false,
      checked: false,
      submitted: false,
    },
    {
      key: 'step2',
      title: 'Site by Site Forecast/Aggragrate Forecast',
      valid: false,
      checked: false,
      submitted: false,
    },

    {
      key: 'step3',
      title: 'Patient Group Ratio',
      valid: true,
      checked: false,
      submitted: false,
    },
    {
      key: 'step4',
      title: 'Testing Protocol',
      valid: true,
      checked: false,
      submitted: false,
    },
    {
      key: 'step5',
      title: 'Patient Assumption',
      valid: true,
      checked: false,
      submitted: false,
    },
    {
      key: 'step6',
      title: 'Product Assumption',
      valid: true,
      checked: false,
      submitted: false,
    },
    {
      key: 'step7',
      title: 'Linear Growth',
      valid: true,
      checked: false,
      submitted: false,
    },
    {
      key: 'step8',
      title: 'Forecast Chart',
      valid: true,
      checked: false,
      submitted: false,
    },
  ];

  public activeStep = this.steps[0];

  ngOnInit() {
    if(this.type=="C")
    {
    this.activeStep=this.steps[7];
    }
    this.model = {
      Program: '',
    
    }

  }


  getlist()
  {
   this.nextStep('step11,N')
  }
  setActiveStep(steo) {
    this.activeStep = steo
  }

  prevStep() {
    let idx = this.steps.indexOf(this.activeStep);
    if (idx > 0) {
      this.activeStep = this.steps[idx - 1]
    }
  }

  nextStep(event) {
    let stringarr = new Array();
    stringarr = event.split(',')
    this.forecastid=stringarr[2]
 

    if (stringarr[1] == "N") {
     
      if (stringarr[0] == "step1S") {
        this.Forecasttype = "S";
        this.activeStep = this.steps[1]
      }
      else if (stringarr[0]  == "step1A") {
        this.Forecasttype = "A";
      
        this.activeStep = this.steps[1]
    this.Aggregate.forecastid= this.forecastid;
    this.Aggregate.ngOnInit();
      }
      else if (stringarr[0]  == "step2") {
        this.PatientGroup.ngOnInit();
        this.activeStep = this.steps[2]
      }
      else if (stringarr[0]  == "step3") {
        this.Testingprotocol.ngOnInit();
        this.activeStep = this.steps[3]
      }
      else if (stringarr[0]  == "step4") {
        this.PatientAssumption.ngOnInit();
        this.activeStep = this.steps[4]
      }
      else if (stringarr[0]  == "step5") {
        this.Productassumption.ngOnInit();
        this.activeStep = this.steps[5]
      }
      else if (stringarr[0]  == "step6") {
        this.lineargrowth.ngOnInit();
        this.activeStep = this.steps[6]
      }
      else if (stringarr[0]  == "step7") {
        this.forecastchart.ngOnInit();
        this.activeStep = this.steps[7]
      }
     
    }
    else
    {
      if (stringarr[0]  == "step2S") {
        this.Forecasttype = "S";
        this.activeStep = this.steps[1]
      }
      else if (stringarr[0]  == "step2A") {
        this.Forecasttype = "A";
        this.activeStep = this.steps[1]
      }
      else if (stringarr[0]  == "step1") {
        this.activeStep = this.steps[0]
      }
      else if (stringarr[0]  == "step3") {
        this.activeStep = this.steps[2]
      }
      else if (stringarr[0]  == "step4") {
        this.activeStep = this.steps[3]
      }
      else if (stringarr[0]  == "step5") {
        this.activeStep = this.steps[4]
      }
      else if (stringarr[0]  == "step6") {
        this.activeStep = this.steps[5]
      }
      else if (stringarr[0]  == "step7") {
        this.activeStep = this.steps[6]
      }
      else if (stringarr[0]  == "step8") {
        this.activeStep = this.steps[7]
      }
    }
    //this._router.navigate(["/Demographic/DemographicAdd",this.programid,this.forecastid]);


    //   this.activeStep.submitted = true;
    //   if (this.activeStep.key == "step1") {

    //     this.activeStep = this.steps[1]
    // }
  }


  onWizardComplete(data) {

  }


  private lastModel;

  // custom change detection
  backtotabs(event) {
    alert('jj');
    if (event == "step2S") {
      this.Forecasttype = "S";
      this.activeStep = this.steps[1]
    }
    else if (event == "step2A") {
      this.Forecasttype = "A";
      this.activeStep = this.steps[1]
    }
    else if (event == "step1") {
      this.activeStep = this.steps[0]
    }
    else if (event == "step3") {
      this.activeStep = this.steps[2]
    }
    else if (event == "step4") {
      this.activeStep = this.steps[3]
    }
    else if (event == "step5") {
      this.activeStep = this.steps[4]
    }
    else if (event == "step6") {
      this.activeStep = this.steps[5]
    }
    else if (event == "step7") {
      this.activeStep = this.steps[6]
    }
    else if (event == "step8") {
      this.activeStep = this.steps[7]
    }




    // if (this.CurrentTab == 7) {

    //     let el: HTMLElement = this.testingprotocol1.nativeElement as HTMLElement;
    //     el.click();

  }


  incomingfile(event) {

    this.file = event.target.files[0];
  
      this.formData.append(this.file.name, this.file);
    

    // const target: DataTransfer = <DataTransfer>(event.target);
    // if (target.files.length !== 1) throw new Error('Cannot use multiple files');
    // const reader: FileReader = new FileReader();
    // reader.onload = (e: any) => {
    //   /* read workbook */
    //   const bstr: string = e.target.result;
    //   this.wb = XLSX.read(bstr, { type: 'binary' });

    //   /* grab first sheet */
    //   this.Sheetarr = this.wb.SheetNames;
    //   const wsname: string = this.wb.SheetNames[0];
    //   const ws: XLSX.WorkSheet = this.wb.Sheets["Hist consumption corrected"];
    //   console.log(this.Sheetarr);
    //   /* save data */
    //   var data = XLSX.utils.sheet_to_json(ws, { header: 1 });
    // };
    // reader.readAsBinaryString(target.files[0]);
 
  
  }

    savepatientnumber()
    {

      var   token = localStorage.getItem("jwt");
      const uploadReq = new HttpRequest('PUT', `http://localhost:53234/api/Import/Importpatient/`+ this.forecastid, this.formData, {
        headers:new HttpHeaders({ "Authorization": "Bearer " + token,'userid':localStorage.getItem("userid"),'countryid':localStorage.getItem("countryid")}),
        reportProgress: true,
      });
    //   this.http.post("http://localhost:53234/api/Import/Uploadfile",formData).subscribe((data)=>{
    //     console.log(data["_body"])
    //   })
//   this._APIwithActionService.postAPI(formData,"Import","Uploadfile").subscribe((data)=>{
//       console.log(data["_body"])
//   })

      this.http.request(uploadReq).subscribe(event => {

        this._APIwithActionService.Getpatientnumber.emit(event["body"])
        
        this.lgModal3.hide();
      })
// this._APIwithActionService.putAPI(this.forecastid,this.formData,"Import","Importpatient").subscribe((data)=>

// {
//   console.log(data)
// }
// )
    }
  
}



