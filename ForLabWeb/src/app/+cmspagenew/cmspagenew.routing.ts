
import {Routes, RouterModule} from "@angular/router";

import {CmspagenewComponent} from "./cmspagenew.component";

export const routes: Routes = [
  {
    path: '',
    component: CmspagenewComponent,
   
  }
];


export const routing = RouterModule.forChild(routes);