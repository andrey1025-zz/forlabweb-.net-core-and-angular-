import {Routes, RouterModule} from "@angular/router";
import {ServiceListComponent} from "./ServiceStatisticList/ServiceStatisticList.component";
import {ServiceComponent} from "./ServiceStatistic.component";
import {ServicewizardComponent} from "./servicewizard/servicewizard.component";
import { ServicestatisticaddComponent } from './servicestatisticadd/servicestatisticadd.component';
export const routes: Routes = [
  {
    path: '',
    component: ServiceComponent,
    children: [
      {
        path: 'ServiceAdd/:id',
        component: ServicewizardComponent
      },
      {
        path: 'ServiceAdd',
        component: ServicewizardComponent
      },
          {
        path: 'ServiceList',
        component: ServiceListComponent
      }
    ]
  }
];


export const routing = RouterModule.forChild(routes);

