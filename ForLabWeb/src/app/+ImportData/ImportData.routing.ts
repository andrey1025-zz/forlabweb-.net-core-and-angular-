

import {Routes, RouterModule} from "@angular/router";



import {ImportDataComponent} from "./ImportData.component";
export const routes: Routes = [
  {
    path: '',
    component: ImportDataComponent,
   
  }
];


export const routing = RouterModule.forChild(routes);

