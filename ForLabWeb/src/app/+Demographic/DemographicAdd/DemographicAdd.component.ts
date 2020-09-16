import {
  Component, OnInit, DoCheck, Output, EventEmitter, Input
} from '@angular/core';
import { Http } from '@angular/http';  
import {
  trigger,
  state,
  style,
  transition,
  animate
} from '@angular/animations'
import { APIwithActionService } from '../../shared/APIwithAction.service';  
import { GlobalAPIService } from '../../shared/GlobalAPI.service';  
import { Globals } from '../../shared/Globals';  
import { Router, ActivatedRoute } from '@angular/router';  
import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
import { formatDate } from 'ngx-bootstrap/chronos';
import { DemographicwizardComponent } from '../demographicwizard/demographicwizard.component';
@Component({
  selector: 'sa-DemographicAdd',
  templateUrl: './DemographicAdd.component.html',
  providers:[Globals],
  animations: [
    trigger('changePane', [
      state('out', style({
        height: 0,
      })),
      state('in', style({
        height: '*',
      })),
      transition('out => in', animate('250ms ease-out')),
      transition('in => out', animate('250ms 300ms ease-in'))
    ])
  ]
})
//, DoCheck 
export class DemographicAddComponent implements OnInit{
  demosettingadd: FormGroup;
  FormTitle: string = "";
  Foracstinfoobj: Object;
  classname: string = "";
  programid:number=0;
  forecastid:number=0;
  date:Date;
  RegionList=new Array();
  type:string="";
  @Input() RecforecastID:number;
 
  @Output()
  nextStep = new EventEmitter<string>();
  backstep = new EventEmitter<string>();
  constructor(private _fb: FormBuilder,private _avRoute:ActivatedRoute,
    private _router: Router,private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService:GlobalAPIService,private _Globals:Globals) {
  
    if (this._avRoute.snapshot.params["id1"]) {  
      this.programid = this._avRoute.snapshot.params["id1"];  
        
  }  
  if (this._avRoute.snapshot.params["id2"]) {  
    this.forecastid = this._avRoute.snapshot.params["id2"];  
   

}  
if (this._avRoute.snapshot.params["F"]) {  
this.type = this._avRoute.snapshot.params["F"];  
 

}  
// if (this.type=="S")
// {
//   this.nextStep.emit('step1S,N,'+this.forecastid);
// }
// else
// {
//   this.nextStep.emit('step1A,N,'+this.forecastid);

// }

if (this.forecastid > 0) {  
         


  this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','GetbyId').subscribe((resp) => {                  
     
          this.demosettingadd.patchValue({
    
            ForecastID: resp["forecastNo"],
            forecastdate: new Date(resp["forecastDate"]),
            startdate: new Date(resp["startDate"]),
            Period:resp["period"],
            sitebysite: resp["forecastType"]=='S'?'sitebysite':'Aggregate'
         //_productPrices:this.ProductPriceList
        
            });
           
     
            this.demosettingadd.get('sitebysite').disable();
      

    
          })
}   



this.date =  new Date(); 
    
setInterval(() => {
    this.date =  new Date();
 }, 1000);
  }
  getregions() {
    this._GlobalAPIService.getDataList('Region').subscribe((data) => {
      this.RegionList = data.aaData
      //const controls = this.ProductTypeList.map(c => new FormControl(false));
      //  controls[0].setValue(true); // Set the first checkbox to true (checked)
    



    }
    ), err => {
      
    }

  }
  ngOnInit() {
   
if(this.RecforecastID>0)
{
  this.forecastid=this.RecforecastID;
}
    this.demosettingadd = this._fb.group({
      ForecastID: ['',Validators.compose([Validators.required,Validators.maxLength(32)])],
      forecastdate:[null, [Validators.required]],
      startdate: [null, [Validators.required]],
      Period: 'Monthly',
      sitebysite: 'sitebysite'
    
                 
  
  })
    this.getregions();
    this.FormTitle = "Demographic Forecasting Definition";
    this.classname = "col-sm-12 col-md-12 col-lg-5";
  
  }

  public steps = [
    {
      key: 'step1',
      title: 'Basic information',
      valid: false,
      checked: false,
      submitted: false,
    },
    {
      key: 'step2',
      title: 'Billing information',
      valid: false,
      checked: false,
      submitted: false,
    },
    {
      key: 'step3',
      title: 'Domain Setup',
      valid: true,
      checked: false,
      submitted: false,
    },
    {
      key: 'step4',
      title: 'Save Form',
      valid: true,
      checked: false,
      submitted: false,
    },
  ];

  public activeStep = this.steps[0];
  changeforecsattype()
  {
   
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

  save() {

 let Id:number;
   let forecasttype = ""
        if (this.demosettingadd.controls['sitebysite'].value == 'sitebysite') {
          forecasttype = 'S'
        }
        else {
          forecasttype = 'C'
        }
        this.Foracstinfoobj = {
          ForecastID:this.forecastid,
          ForecastNo:  this.demosettingadd.controls['ForecastID'].value,
          Methodology: "MORBIDITY",
          DataUsage: "DATA_USAGE1",
          Status: "OPEN",
          StartDate: formatDate(this.demosettingadd.controls['startdate'].value,"DD/MMM/YYYY"),
          Period: this.demosettingadd.controls['Period'].value,
          ForecastDate:formatDate(this.demosettingadd.controls['forecastdate'].value,"DD/MMM/YYYY"),
          Method: "Linear",
          SlowMovingPeriod: this.demosettingadd.controls['Period'].value,
          ForecastType: forecasttype,
          ProgramId:this.programid,
          Extension: 4,
          LastUpdated: this.date ,
          Countryid: localStorage.getItem("countryid")
        }
     
        this._APIwithActionService.postAPI(this.Foracstinfoobj,'Forecsatinfo','saveforecastinfo')               
        .subscribe((data) => {  
            if (data["_body"] !=0)
            {
             // this.Foracstinfoobj["ForecastID"] =Number(data["_body"])
              Id=  data["_body"]   
             
               if (this.forecastid==0)
               {                                      
                this._GlobalAPIService.SuccessMessage("Forecast Info Saved Successfully");
              
                if(this.demosettingadd.controls['sitebysite'].value=="sitebysite")
                {
                  this._router.navigate(["/Demographic/DemographicAdd", this.programid, Id,'S']);
                //  this.nextStep.emit('step1S,N,'+Id);
                }
                else
                {
                  this._router.navigate(["/Demographic/DemographicAdd", this.programid, Id,'A']);
                 // this.nextStep.emit('step1A,N,'+Id);
             //     this._router.navigate(["Demographic/aggregrateforecast",this.programid,Id])
                  
                }
              }
              else
              {
                if(this.demosettingadd.controls['sitebysite'].value=="sitebysite")
                {
                 this.nextStep.emit('step1S,N,'+Id);
                }
                else
                {
                  this.nextStep.emit('step1A,N,'+Id);
            
                  
                }
              }
            
               
            }
            else
            {
                this._GlobalAPIService.FailureMessage("Duplicate ForecastID");
                
            
            }
            
          
             })


             this.classname = "col-sm-12 col-md-12 col-lg-12";
          
      }

     


  onWizardComplete(data) {

  }


  private lastModel;

 

}


