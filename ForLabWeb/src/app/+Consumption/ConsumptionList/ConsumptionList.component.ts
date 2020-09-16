import { Component,Input,ChangeDetectorRef, OnInit,ElementRef,AfterViewInit,Renderer,ViewChild,OnChanges,SimpleChange,SimpleChanges} from '@angular/core';  
import { Http } from '@angular/http';  
// import {DataTables} from 'angular-datatables';
import { Router, ActivatedRoute } from '@angular/router';  
import { APIwithActionService } from '../../shared/APIwithAction.service';  


import {NotificationService} from "../../shared/utils/notification.service";
import { GlobalVariable } from '../../shared/globalclass'
import {DatatableComponent} from "../../shared/ui/datatable/datatable.component";

declare var $: any;


@Component({  
    selector: 'sa-consumptionList',  
    templateUrl: './ConsumptionList.component.html'
})  
export class ConsumptionListComponent implements AfterViewInit, OnInit  {  
 
    public options; 

    @ViewChild(DatatableComponent) DataView: DatatableComponent
 //dtOptions: Datatable.Settings = {};
  id: string;  
  rowindex:number;
    constructor(public http: Http,private notificationService: NotificationService,private _changedet:ChangeDetectorRef,
      private _router: Router,private _route:ActivatedRoute,private _render:Renderer,
       private _APIwithActionService: APIwithActionService,private _elref:ElementRef,private _avRoute:ActivatedRoute ) {  
        if (this._avRoute.snapshot.params["id"]) {  
          this.id = this._avRoute.snapshot.params["id"];  
         

      }  
  
       }
    
    delete(TestID) {  
      
            let table =document.querySelector('table'); 
            this._APIwithActionService.deleteData(this.id,'Consumption','Deleteconsumption').subscribe((data) => { 
            this._render.setElementStyle(table.rows[this.rowindex],'display','none')        
            }, error => alert(error))   
     
    } 
    ngAfterViewInit(): void {
      document.querySelector('body').addEventListener('click', (event)=> {
        let target = <Element>event.target;// Cast EventTarget into an Element
     
        if (target.parentElement.className=='Edit') {
          this._router.navigate(["/Consumption/ConsumptionAdd",target.parentElement.getAttribute('data-con-id')]);
          
        }
        if (target.parentElement.className=='del') {
          this.id =target.parentElement.getAttribute('data-con-id');    
          this.rowindex= target.parentElement.parentElement.parentElement["rowIndex"];
          this.smartModEg1();
        
        }
    });

      }
      smartModEg1() {
        this.notificationService.smartMessageBox({
          title: "Deletion",
          content: "Do you want to delete "+ this.id +" Consumption",
          buttons: '[No][Yes]'
        }, (ButtonPressed) => {
          if (ButtonPressed === "Yes") {
            this.delete(this.id);
            this.notificationService.smallBox({
              title: "Deletion",
              content: "<i class='fa fa-clock-o'></i> <i>Consumption Deleted</i>",
              color: "#659265",
              iconSmall: "fa fa-check fa-2x fadeInRight animated",
              timeout: 4000 
             // function:this.delete(SiteCategory)
            });
           
          }
          if (ButtonPressed === "No") {
            this.notificationService.smallBox({
              title: "Cancelation",
              content: "<i class='fa fa-clock-o'></i> <i>Deletion Cancelled</i>",
              color: "#C46A69",
              iconSmall: "fa fa-times fa-2x fadeInRight animated",
              timeout: 4000
             
            });
          }
       
        });
      }
  
      // @Input()
      // set name(id: number) {
      
      //   this.id = _id;
      // }
  
      
    ngOnInit() {
      const router =this._router

      this.DataView.options=
      {
         dom: "Bfrtip",
         aaSorting: [],
         ajax: (data, callback, settings) => {
          this._APIwithActionService.getDatabyID('CONSUMPTION','MMProgram','GetForecastInfoByMethodology',"CID="+localStorage.getItem("countryid"))
            
            .subscribe((data) => {
           
              callback({
                aaData: data.slice(0, 100)
              })
            })
        },
    
          columns: [
              { data: 'forecastID' },
              { data:'status'},
              { data: 'forecastNo' },
              { data:'scopeOfTheForecast'},
              { data: 'startDate',type:Date,format:'dd/mm/yyyy' },
              { data: 'forecastDate' },
              { data: 'period' },
              { data: 'extension' },
           //   { data: 'method' },
              // { data: 'westage' },
              { data: 'lastUpdated' },
              {
                  render: (data, type, fullRow, meta) => {
                      return `
                      <a  class='Edit' data-con-id="${fullRow.forecastID}"> <i class='fa fa-pencil-square-o'></i></a>
                      <a  class='del' data-con-id="${fullRow.forecastID}"> <i class='fa fa-trash-o'></i></a>
                        `;
                  }
              }
          ] , 
          buttons: [
            {
              text: 'Add',
              className: 'btn-primary',
              action: function ( e, dt, node, config ) {
           
               router.navigate(["/Consumption/ConsumptionAdd"])
               // this._router.navigate(["/Managedata/TestingAreaAdd"])
               
              }
            }]
      };   

    }
}