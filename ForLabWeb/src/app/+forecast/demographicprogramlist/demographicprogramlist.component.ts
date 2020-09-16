import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { ModalDirective } from "ngx-bootstrap";
import {NotificationService} from '../../shared/utils/notification.service';
@Component({
  selector: 'app-demographicprogramlist',
  templateUrl: './demographicprogramlist.component.html',
  styleUrls: ['./demographicprogramlist.component.css']
})
export class DemographicprogramlistComponent implements OnInit {
  Programlist= new Array();
  forecastid:number=0;
  Programid:number=0;
  saveprogramid:number=0;
  model:any
  @ViewChild('lgModal4') public lgModal4: ModalDirective;
  //programName:string="";
  constructor(private notificationService:NotificationService,private _avRoute:ActivatedRoute,
    private _router: Router,private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService:GlobalAPIService) { 

      if (this._avRoute.snapshot.params["id"]) {  
        this.forecastid = this._avRoute.snapshot.params["id"];  
       
    
    }  
    this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','GetbyId').subscribe((resp) => {                  
      this.saveprogramid=  resp["programId"];
      this.Programid=resp["programId"];
    })
    
    this.model = {
      programName: '',
    
    }
      this._APIwithActionService.getDataList('MMProgram',"Get").subscribe((data)=>{
        this.Programlist=data
      })
    }

  ngOnInit() {
  }
  Selectprogram(programid)
  {
this.Programid=programid;

this._APIwithActionService.putAPI(this.forecastid,this.Programid,"Forecsatinfo","updateprogram").subscribe((data)=>{
  this._router.navigate(["/Forecast/Demographicprogrammethodlist", this.forecastid,this.Programid]);
})


  }
  Save()
  {
    let newprogram = new Object();
    let suggestedvariable= new Object();
    if(this.model.programName =="")
    {
      this._GlobalAPIService.FailureMessage("Please fill program name");
      return;
    }
    newprogram = {
      Id: 0,
      ProgramName: this.model.programName
  }
    // suggestedvariable = {
    //     id: 0,
    //     Name: this.model.programName,
    //     Type_name: "Program"
    // }
    this._APIwithActionService.postAPI(newprogram, 'MMProgram', 'SaveProgram').subscribe((data) => {
        if (data["_body"] != 0) {
          
        this.Programid=data["_body"]
            this._GlobalAPIService.SuccessMessage("Program saved successfully");
            this.Programlist.push({
              id:data["_body"],
              programName:this.model.programName
            })
            this.lgModal4.hide();
         console.log(data["_body"]);
        }
        else {
            this._GlobalAPIService.FailureMessage("Program Name must not be duplicate");
        }
    })
  }
  Previousclick()
  {
    this.notificationService.smallBox({
      title: "Conformation!",
      content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/Forecastmethodology/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
      color: "#296191",
      //timeout: 8000,
      icon: "fa fa-bell swing animated"
    });
  }
}
