import { Component, Input, ChangeDetectorRef, OnInit, ElementRef, AfterViewInit, Renderer, ViewChild, OnChanges, SimpleChange, SimpleChanges } from '@angular/core';
import { Http } from '@angular/http';
// import {DataTables} from 'angular-datatables';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';

import { NotificationService } from "../../shared/utils/notification.service";
import { GlobalVariable } from '../../shared/globalclass'
import { DatatableComponent } from "../../shared/ui/datatable/datatable.component";

declare var $: any;


@Component({
  selector: 'sa-demographicList',
  templateUrl: './DemographicList.component.html'
})
export class DemograhicListComponent implements AfterViewInit, OnInit {
  icon: string
  public options;

  @ViewChild(DatatableComponent) DataView: DatatableComponent
  //dtOptions: Datatable.Settings = {};
  id: string;
  rowindex: number;
  constructor(public http: Http, private notificationService: NotificationService, private _changedet: ChangeDetectorRef,
    private _router: Router, private _route: ActivatedRoute, private _render: Renderer,
    private _APIwithActionService: APIwithActionService, private _elref: ElementRef, private _avRoute: ActivatedRoute,private _GlobalAPIService:GlobalAPIService) {
    if (this._avRoute.snapshot.params["id"]) {
      this.id = this._avRoute.snapshot.params["id"];
    

    }

  }
  Addnewforecast() {
    this._router.navigate(["Demographic/DemographicAdd", this.id])
  }
  delete(TestID) {

    let table = document.querySelector('table');
    this._APIwithActionService.deleteData(TestID, 'Test', 'Del01').subscribe((data) => {
      this._render.setElementStyle(table.rows[this.rowindex], 'display', 'none')
    }, error => alert(error))

  }
  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element

      if (target.parentElement.className == 'Edit') {
        this._router.navigate(["/Demographic/DemographicAdd", this.id, target.parentElement.getAttribute('data-fore-id')]);

      }
      if (target.parentElement.className == 'Output') {
        this._router.navigate(["Demographic/DemographicAdd", this.id, target.parentElement.getAttribute('data-fore-id'), 'C'])
      }
      if (target.parentElement.className == 'del') {
        this.id = target.parentElement.getAttribute('data-fore-id');
        this.rowindex = target.parentElement.parentElement.parentElement["rowIndex"];
        this.smartModEg1();

      }
      if(target.parentElement.className == 'lock')
      {
        this.id = target.parentElement.getAttribute('data-fore-id');
        this._APIwithActionService.getDatabyID(this.id,"Forecsatinfo","lockforecastinfo").subscribe(
          
          (data)=>{
if (data.forecastlockstatus !="")
{
  this._GlobalAPIService.SuccessMessage(data.forecastlockstatus)
  this.icon = "<i  class='fa fa-lock' ></i>"
}

          })
      }
    });

  }
  smartModEg1() {
    this.notificationService.smartMessageBox({
      title: "Deletion",
      content: "Do you want to delete " + this.id + " Test",
      buttons: '[No][Yes]'
    }, (ButtonPressed) => {
      if (ButtonPressed === "Yes") {
        this.delete(this.id);
        this.notificationService.smallBox({
          title: "Deletion",
          content: "<i class='fa fa-clock-o'></i> <i>Test Deleted</i>",
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
   
    const router=this._router;
    const id1=this.id;
    this.DataView.options =
      {
        dom: "Bfrtip",
        aaSorting: [],
        ajax: (data, callback, settings) => {
          this._APIwithActionService.getDatabyID(this.id, 'MMProgram', 'Getforecastinfobyprogramid')

            .subscribe((data) => {

              callback({
                aaData: data.slice(0, 100)
              })
            })
        },

        columns: [
          { data: 'forecastID' },
          { data: 'forecastNo' },
          { data: 'startDate' },
          { data: 'forecastDate' },
          { data: 'period' },
          {
            render: (data, type, fullRow, meta) => {

              if (fullRow.forecastlock == true) {
                this.icon = "<i  class='fa fa-lock' ></i>"
              }
              else {
                this.icon = "<i  class='fa fa-unlock' ></i>"
              }
              return `
                      <a  class='Edit' data-fore-id="${fullRow.forecastID}"> <i class='fa fa-pencil-square-o'></i></a>
                      <a  class='Output' data-fore-id="${fullRow.forecastID}"> <i class='fa fa-bar-chart'></i></a>
                      <a  class='lock' data-fore-id="${fullRow.forecastID}">`+ this.icon +`</a>
                       
                     `;
            }
          }
        ],
        buttons: [
          {
            text: 'Add',
            className: 'btn-primary',
            action: function ( e, dt, node, config ) {
             
             
             router.navigate(["/Demographic/DemographicAdd", id1])
             // this._router.navigate(["/Managedata/TestingAreaAdd"])
             
            }
        },
      {
        text: 'Programs',
        className: 'btn-primary',
        action: function ( e, dt, node, config ) {
       
         router.navigate(["/Demographic/Programlist"])

      }
    }
      ]
      };

  }
  // <a  class='del' data-fore-id="${fullRow.forecastID}"> <i class='fa fa-trash-o'></i></a>
}