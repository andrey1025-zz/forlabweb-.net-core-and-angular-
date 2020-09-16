
import { Routes, RouterModule } from '@angular/router';
import {SearchProductComponent} from "./SerachProduct.component";

export const searchproductRoutes: Routes = [{
  path: '',
  component: SearchProductComponent
}];

export const searchproductRouting = RouterModule.forChild(searchproductRoutes);
