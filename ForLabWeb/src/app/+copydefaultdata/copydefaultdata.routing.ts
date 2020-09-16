import { Routes, RouterModule } from '@angular/router';
import {CopydefaultdataComponent} from "./copydefaultdata.component";

export const  copydefaultdataRoutes: Routes = [{
  path: '',
  component: CopydefaultdataComponent
}];

export const  copydefaultdataRouting = RouterModule.forChild(copydefaultdataRoutes);
