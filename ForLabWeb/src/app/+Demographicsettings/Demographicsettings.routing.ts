
import { Routes, RouterModule } from '@angular/router';
import {Demographicsettingcomponent} from "./Demographicsettings.component";

export const  DemographicsettingRoutes: Routes = [{
  path: '',
  component: Demographicsettingcomponent
}];

export const  DemographicsettingRouting = RouterModule.forChild(DemographicsettingRoutes);
