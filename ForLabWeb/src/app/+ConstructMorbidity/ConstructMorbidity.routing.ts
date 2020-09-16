import { Routes, RouterModule } from "@angular/router";
import { ConstructMorbidityComponent } from "./ConstructMorbidity.component";

export const routes: Routes = [
  {
    path: '',
    component: ConstructMorbidityComponent

  }
];

export const routing = RouterModule.forChild(routes);

