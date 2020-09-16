
import { NgModule } from '@angular/core';

import { SmartadminModule } from '../shared/smartadmin.module';

import { routing } from './ImportData.routing';

import {ImportDataComponent} from "./ImportData.component";

import {APIwithActionService} from "../shared/APIwithAction.service"

import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { NgxLoadingModule } from 'ngx-loading';
import { FormsModule,ReactiveFormsModule }   from '@angular/forms';

import {TreeViewModule} from "../shared/ui/tree-view/tree-view.module";

@NgModule({
  declarations: [
    ImportDataComponent
  ],
  imports: [
    SmartadminModule,
    routing,  NgxLoadingModule.forRoot({}),
 
    ReactiveFormsModule,
    TreeViewModule


  ],

  entryComponents: [ImportDataComponent],
  providers:[APIwithActionService,GlobalAPIService]
})
export class ImportDataModule {

}

