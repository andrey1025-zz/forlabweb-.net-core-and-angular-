import { Component, OnInit, ViewChild, Renderer, AfterViewInit } from '@angular/core';
import { DatatableComponent } from 'app/shared/ui/datatable';
import { Http } from '@angular/http';
import { NotificationService } from 'app/shared/utils/notification.service';
import { Router } from '@angular/router';
import { APIwithActionService } from "../shared/APIwithAction.service";
import { GlobalAPIService } from "../shared/GlobalAPI.service";
@Component({
  selector: 'app-+pendingapprovalentry',
  templateUrl: './pendingapprovalentry.component.html',
  styleUrls: ['./pendingapprovalentry.component.css']
})
export class pendingapprovalentryComponent implements OnInit,AfterViewInit {

 

  public options; 
  @ViewChild(DatatableComponent) DataView: DatatableComponent
//dtOptions: Datatable.Settings = {};
id: number;  
rowindex:number;
  constructor(public http: Http,private notificationService: NotificationService,
    private _router: Router,private _render:Renderer,
     private _GlobalAPIService: GlobalAPIService,private _APIwithActionService:APIwithActionService) {  
     
  }  
 
   
  ngAfterViewInit(): void {
    let approve= new Object();
    document.querySelector('body').addEventListener('click', (event)=> {
      let target = <Element>event.target;// Cast EventTarget into an Element
      let table =document.querySelector('table'); 
      if (target.className=='btn btn-success btn-sm approve') {

        approve={ 
          id :parseInt(target.getAttribute('data-cat-id')),
          mastertype: target.getAttribute('data-master-type'),
           Isapprove :true,
           Isreject:false
          }

        this._APIwithActionService.postAPI(approve,"Approve","Post").subscribe((data)=>{
          this.rowindex= target.parentElement.parentElement["rowIndex"];
          this._render.setElementStyle(table.rows[this.rowindex],'display','none')   
          console.log(data)
        })
      }
      if (target.className=='btn btn-danger btn-sm reject') {
        approve={ 
          id :parseInt(target.getAttribute('data-cat-id')),
          mastertype: target.getAttribute('data-master-type'),
           Isapprove :false,
           Isreject:true
          }

           this._APIwithActionService.postAPI(approve,"Approve","Post").subscribe((data)=>{
            
            this.rowindex= target.parentElement.parentElement["rowIndex"];
            this._render.setElementStyle(table.rows[this.rowindex],'display','none')   
            console.log(data)
          })
      }
  });
 
    }
   
  ngOnInit() {
    const router=this._router;
    this.DataView.options=
    {
        dom: "Bfrtip",
        aaSorting: [],
        ajax: (data, callback, settings) => {

            this._APIwithActionService.getDataList('Approve','getpendingapprovallist')
                .subscribe((data) => {

                  console.log(data)
                        callback({
                            aaData: data,
                           
                        })
          
                })},
              
              
        // },
    //  ajax:GlobalVariable.BASE_API_URL+"SiteCategory",
        columns: [
            { data: 'name' },
            { data: 'mastertype' },
            { data: 'userName' },
            { data: 'country' },
            {
                render: (data, type, fullRow, meta) => {
                    return `
                    <a  class='btn btn-success btn-sm approve' data-cat-id="${fullRow.id}" data-master-type="${fullRow.mastertype}"> Approve</a>
                    <a  class='btn btn-danger btn-sm reject' data-cat-id="${fullRow.id}"  data-master-type="${fullRow.mastertype}"> Reject</a>
                      
                    
                    
                    `;
                }
            }],
          

}
  }
}
