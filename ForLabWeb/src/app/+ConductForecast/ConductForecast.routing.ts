
import { Routes, RouterModule } from "@angular/router";

import { ConductForecastComponent } from "./ConductForecast.component";
import { ForecastComparisonComponent } from "./ForecastComparison/ForecastComparison.component";

export const routes: Routes = [
  {
    path: '',
    component: ConductForecastComponent,
  },
  {
    path: 'CostsComparison',
    component: ForecastComparisonComponent,
  }

];


export const routing = RouterModule.forChild(routes);

