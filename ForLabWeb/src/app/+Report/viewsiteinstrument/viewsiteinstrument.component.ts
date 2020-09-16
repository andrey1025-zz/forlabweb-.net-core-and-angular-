import { Component, OnInit ,ViewChild} from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { ActivatedRoute } from '@angular/router';  
import {DatatableComponent} from "../../shared/ui/datatable/datatable.component";

@Component({
  selector: 'app-viewsiteinstrument',
  templateUrl: './viewsiteinstrument.component.html'
})
export class ViewsiteinstrumentComponent implements OnInit {
  regionid:number;
  catid:number;
  areaid:string;
  @ViewChild(DatatableComponent) DataView: DatatableComponent
  @ViewChild('aa') aa: DatatableComponent
  constructor(private _APIwithActionService:APIwithActionService,private _avRoute:ActivatedRoute) { 


    if (this._avRoute.snapshot.params["regid"]) {
      this.regionid = this._avRoute.snapshot.params["regid"];
    }
    if (this._avRoute.snapshot.params["catid"]) {
      this.catid = this._avRoute.snapshot.params["catid"];
    }

    if (this._avRoute.snapshot.params["areaid"]) {
      this.areaid = this._avRoute.snapshot.params["areaid"];
    }

  }

  ngOnInit() {
    this.DataView.options=
    {
       dom: "Bfrtip",
     
       //  aaData:this.getdata
       
     //,
      ajax:(data, callback, settings) => {
        console.log('yy')
      this._APIwithActionService.getDataList('Report','Getsiteinstruentlist','param='+this.catid+','+this.regionid+','+this.areaid)
          .subscribe((data) => {


            
          console.log('yy')
            callback({
              aaData: data
            
            })
          }
        
          )
        },
        columns: [
          { data: 'regionName' },
          { data: 'categoryName' },
          { data: 'siteName' },
          { data: 'testingArea' },
          { data: 'instrumentName' },
          { data: 'quantity' },
          { data: 'testRunPercentage' }],
        buttons: [
          'copy', 'csv', 'excel', 'pdf', 'print'
          ]
    };  
  }

}
