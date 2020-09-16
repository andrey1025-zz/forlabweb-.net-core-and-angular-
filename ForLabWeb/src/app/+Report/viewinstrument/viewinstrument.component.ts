import { Component, OnInit ,ViewChild} from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { ActivatedRoute } from '@angular/router';  
import {DatatableComponent} from "../../shared/ui/datatable/datatable.component";

@Component({
  selector: 'app-viewinstrument',
  templateUrl: './viewinstrument.component.html'
})
export class ViewinstrumentComponent implements OnInit {

  areaid:string;
  show:string;
  @ViewChild(DatatableComponent) DataView: DatatableComponent
  @ViewChild('aa') aa: DatatableComponent
  constructor(private _APIwithActionService:APIwithActionService,private _avRoute:ActivatedRoute) { 


   

    if (this._avRoute.snapshot.params["areaid"]) {
      this.areaid = this._avRoute.snapshot.params["areaid"];
    }
    if (this._avRoute.snapshot.params["show"]) {
      this.show = this._avRoute.snapshot.params["show"];
    }
  }

  ngOnInit() {
    let aa:DatatableComponent =this.aa as DatatableComponent;
    this.DataView.options=
    {
       dom: "Bfrtip",
     
       //  aaData:this.getdata
       
     //,
      ajax:(data, callback, settings) => {
        console.log('yy')
      this._APIwithActionService.getDatabyID(this.areaid,'Report','Getinstrumentlist')
          .subscribe((data) => {


            
          console.log('yy')
            callback({
              aaData: data
            
            })
          }
        
          )
        },
        columns: [
          { data: 'testingArea' },
          { data: 'instrument' },
          { data: 'maxthroughput' },
          { data: 'monthlythroughput' },
          { data: 'aftertestno' },
          { data: 'daily' },
          { data: 'weekly' },
          { data: 'monthly' },
          { data: 'quarterly' }
        ],
        buttons: [
          'copy', 'csv', 'excel', 'pdf', 'print'
          ]
    };  

    if(this.show=="false")
    {
    aa.options.columns.splice(4,aa.options.columns.length);
    }
  }


}
