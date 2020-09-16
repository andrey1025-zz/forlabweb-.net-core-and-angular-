
import { NgModule } from '@angular/core';

import { SmartadminModule } from '../shared/smartadmin.module';

import { routing } from './ConstructMorbidity.routing';
import { APIwithActionService } from "../shared/APIwithAction.service"
import { JqueryUiModule } from "../shared/ui/jquery-ui/jquery-ui.module";
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { ConstructMorbidityComponent } from './ConstructMorbidity.component';
import { SmartadminDatatableModule } from 'app/shared/ui/datatable/smartadmin-datatable.module';
import { ProgramAddComponent } from './ProgramAdd/ProgramAdd.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    ConstructMorbidityComponent,
    ProgramAddComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    SmartadminModule,
    routing,
    JqueryUiModule,
    SmartadminDatatableModule
  ],

  entryComponents: [ConstructMorbidityComponent],
  providers: [APIwithActionService, GlobalAPIService]
})
export class ConstructMorbidityModule {

}

