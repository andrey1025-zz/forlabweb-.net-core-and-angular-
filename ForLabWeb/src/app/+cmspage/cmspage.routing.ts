
import {Routes, RouterModule} from "@angular/router";

import {cmspageComponent} from "./cmspage.component";

export const routes: Routes = [
  {
    path: '',
    component: cmspageComponent,
   
  }
];


export const routing = RouterModule.forChild(routes);