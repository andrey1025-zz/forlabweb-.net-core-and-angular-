
import {Routes, RouterModule} from "@angular/router";
import {ConsumptionListComponent} from "./ConsumptionList/ConsumptionList.component";
import {ConsumptionComponent} from "./Consumption.component";
import {ConsumptionwizardComponent} from "./consumptionwizard/consumptionwizard.component"
import { ConsumptionAddComponent } from './consumption-add/consumption-add.component';
export const routes: Routes = [
  {
    path: '',
    component: ConsumptionComponent,
    children: [
    
      {
        path: 'ConsumptionAdd/:id',
        component: ConsumptionwizardComponent
      },
      {
        path: 'ConsumptionAdd',
        component: ConsumptionwizardComponent
      },
      {
        path: 'ConsumptionList',
        component: ConsumptionListComponent
      }
    ]
  }
];


export const routing = RouterModule.forChild(routes);

