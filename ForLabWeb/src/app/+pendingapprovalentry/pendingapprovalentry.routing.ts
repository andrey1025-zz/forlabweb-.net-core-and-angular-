

import {Routes, RouterModule} from "@angular/router";



import {pendingapprovalentryComponent} from "./pendingapprovalentry.component";

export const routes: Routes = [
  {
    path: '',
    component: pendingapprovalentryComponent
   
  }
];


export const routing = RouterModule.forChild(routes);

