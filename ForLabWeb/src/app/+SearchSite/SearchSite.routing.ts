import { Routes, RouterModule } from '@angular/router';
import {SearchSiteComponent} from "./SearchSite.component";

export const searchsiteRoutes: Routes = [{
  path: '',
  component: SearchSiteComponent
}];

export const searchsiteRouting = RouterModule.forChild(searchsiteRoutes);