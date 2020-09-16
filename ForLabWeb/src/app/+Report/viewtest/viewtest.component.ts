import { Component, OnInit ,ViewChild} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalVariable } from '../../shared/globalclass';
import {DatatableComponent} from "../../shared/ui/datatable/datatable.component";

@Component({
  selector: 'app-viewtest',
  templateUrl: './viewtest.component.html'
})
export class ViewtestComponent implements OnInit {no: number;
 
  areaid:string;

  @ViewChild(DatatableComponent) DataView: DatatableComponent
 
  constructor(private _avRoute: ActivatedRoute) {

    if (this._avRoute.snapshot.params["areaid"]) {
      this.areaid = this._avRoute.snapshot.params["areaid"];
    }
   
  }

  ngOnInit() {



    this.DataView.options=
    {
       dom: "Bfrtip",
      
      ajax:GlobalVariable.BASE_API_URL+"Report/Gettestlist/" +this.areaid,
        columns: [
            { data: 'area' },
            { data: 'test' }
           
          
        ] , 
        buttons: [
          'copy', 'csv', 'excel', 'pdf', 'print'
          ]
    };   
  
  }

}
