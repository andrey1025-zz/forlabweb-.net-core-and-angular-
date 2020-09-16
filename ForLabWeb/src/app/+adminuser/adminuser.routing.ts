
import {Routes, RouterModule} from "@angular/router";



import {adminuserComponent} from "./adminuser.component";
export const routes: Routes = [
  {
    path: '',
    component: adminuserComponent,
   
  }
];


export const routing = RouterModule.forChild(routes);