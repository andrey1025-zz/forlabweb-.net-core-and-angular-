import { Component, OnInit, AfterViewInit, ViewChild, Renderer } from '@angular/core';
import { DatatableComponent } from 'app/shared/ui/datatable';
import { Http } from '@angular/http';
import { NotificationService } from 'app/shared/utils/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from 'app/shared/APIwithAction.service';
import { GlobalAPIService } from 'app/shared/GlobalAPI.service';

@Component({
  selector: 'app-forecastlist',
  templateUrl: './forecastlist.component.html',
  styleUrls: ['./forecastlist.component.css']
})
export class ForecastlistComponent implements AfterViewInit,OnInit {

  icon: string
  public options;

  @ViewChild(DatatableComponent) DataView: DatatableComponent
  //dtOptions: Datatable.Settings = {};
  id: string;
  rowindex: number;
  constructor(public http: Http, private notificationService: NotificationService,
    private _router: Router,  private _render: Renderer,
    private _APIwithActionService: APIwithActionService, private _avRoute: ActivatedRoute,private _GlobalAPIService:GlobalAPIService) {
   

  }
  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element

      if (target.className == 'btn btn-default btn-sm Edit') {
        console.log("Edit" + target.getAttribute('data-fore-id'))
        this._router.navigate(["/Forecast/Defineforecast",  target.getAttribute('data-fore-id')]);

      }
    

    });

  }

  delete(TestID) {

    let table = document.querySelector('table');
    this._APIwithActionService.deleteData(TestID, 'Test', 'Del01').subscribe((data) => {
      this._render.setElementStyle(table.rows[this.rowindex], 'display', 'none')
    }, error => alert(error))

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
          this._APIwithActionService.getDatabyID(0,"Forecsatinfo","getrecentforecast")

            .subscribe((data) => {

              callback({
                aaData: data.slice(0, 100)
              })
            })
        },

        columns: [
          { data: 'name' },
          { data: 'dateofforecast' },
          { data: 'duration' },
          { data: 'forecastvalue' },
         
          {
            render: (data, type, fullRow, meta) => {

             
              return `
          
                      <a  class='btn btn-default btn-sm Edit' data-fore-id="${fullRow.id}">View</a>
                      <a  class="btn btn-default btn-sm" style="background-color: gray;color: white;" data-fore-id="${fullRow.id}">Report</a>
                   
                       
                     `;
            }
          }
        ],
      
        buttons: [
          {
            text: 'Back',
            className: 'btn btn-default btn-sm back',
           
            action: function ( e, dt, node, config ) {
             console.log()
             router.navigate(["Forecast/RecentForecast"])
             // this._router.navigate(["/Managedata/TestingAreaAdd"])
             
            }
        }]
      };

  }

}
