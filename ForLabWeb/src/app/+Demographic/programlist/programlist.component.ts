import { Component, OnInit,ViewChild } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { DatatableComponent } from "../../shared/ui/datatable/datatable.component";
import { Router } from '@angular/router';
@Component({
  selector: 'app-programlist',
  templateUrl: './programlist.component.html',
  styleUrls: ['./programlist.component.css']
})
export class ProgramlistComponent implements OnInit {
  @ViewChild(DatatableComponent) DataView: DatatableComponent
  constructor(   private _APIwithActionService: APIwithActionService, private _router: Router) { }
  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element

      if (target.parentElement.className == 'btn btn-success btn-sm Add') {
        this._router.navigate(["/Demographic/DemographicAdd",  target.parentElement.getAttribute('data-prog-id')]);

      }
      if (target.parentElement.className == 'btn btn-success btn-sm Edit') {
        this._router.navigate(["/Demographic/DemographicList",  target.parentElement.getAttribute('data-prog-id')]);

      }
     
    });

  }
  ngOnInit() {
    const router=this._router;
    this.DataView.options =
    {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {
      
    this._APIwithActionService.getDataList('MMProgram',"Get")

          .subscribe((data) => {

            callback({
              aaData: data.slice(0, 100)
            })
          })
      },

      columns: [
        // { data: 'id' },
        { data: 'programName' },
               {
          render: (data, type, fullRow, meta) => {

           
            return `
                    <a  class='btn btn-success btn-sm Add' data-prog-id="${fullRow.id}"> <i >Add Forecast</i></a>                 
                    <a  class='btn btn-success btn-sm Edit' data-prog-id="${fullRow.id}"> <i >Edit Forecast</i></a>  
                    `;
          }
        }
      ],
      buttons: [
        {
          text: 'Add',
          className: 'btn-primary',
          action: function ( e, dt, node, config ) {
           console.log()
           router.navigate(["/Demographicsettings"])
           // this._router.navigate(["/Managedata/TestingAreaAdd"])
           
          }
      }]
    };
  }



}
