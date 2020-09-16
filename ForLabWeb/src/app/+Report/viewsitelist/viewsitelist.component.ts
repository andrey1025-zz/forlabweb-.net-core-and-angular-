import { Component, OnInit ,ViewChild} from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { ActivatedRoute } from '@angular/router';  
import {DatatableComponent} from "../../shared/ui/datatable/datatable.component";
@Component({
  selector: 'app-viewsitelist',
  templateUrl: './viewsitelist.component.html',

})
export class ViewsitelistComponent implements OnInit {
  controlArray=new Array();
  xx:boolean=false;
  columnname=new Array();
  getdata=new Array();
  regionid:number;
  catid:number;
  show:string;
  @ViewChild(DatatableComponent) DataView: DatatableComponent
  @ViewChild('aa') aa: DatatableComponent
  constructor(private _APIwithActionService:APIwithActionService,private _avRoute:ActivatedRoute) { 
 
 

    if (this._avRoute.snapshot.params["regid"]) {
      this.regionid = this._avRoute.snapshot.params["regid"];
    }
    if (this._avRoute.snapshot.params["catid"]) {
      this.catid = this._avRoute.snapshot.params["catid"];
    }

    if (this._avRoute.snapshot.params["show"]) {
      this.show = this._avRoute.snapshot.params["show"];
    }



  }

  ngOnInit() {

    this._APIwithActionService.getDatabyID(this.regionid,'Report','Getsitelist','categoryid='+this.catid)
    .subscribe((data4) => {
      console.log(data4)
      this.controlArray=data4.header;
      this.columnname=data4.column;
       this.getdata=data4.data;

this.xx=true
let aa:DatatableComponent =this.aa as DatatableComponent;

//    this.DataView.options=
//   {
//     aaData: this.getdata,  
//     columns:this.columnname ,     
//       dom: "Bfrtip",
      
    
 
     
//        buttons: [
//          'copy', 'excel', 'pdf', 'print'
//          ]
         
//    };  
 
   console.log('4')
        if(this.show=="false")
           {
            this.columnname.splice(3, this.columnname.length);
           }
    });
 
  
  }

}
