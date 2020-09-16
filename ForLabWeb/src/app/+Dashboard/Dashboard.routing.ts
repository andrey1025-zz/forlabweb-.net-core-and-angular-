

import { Routes, RouterModule } from "@angular/router";



import { DashboardComponent } from "./Dashboard.component";

export const routes: Routes = [
  {
    path: '',
    component: DashboardComponent

  }
];

export const routing = RouterModule.forChild(routes);

