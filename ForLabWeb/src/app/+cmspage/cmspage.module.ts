
import { NgModule } from '@angular/core';
import { SmartadminModule } from '../shared/smartadmin.module';
import {SmartadminDatatableModule} from "../shared/ui/datatable/smartadmin-datatable.module";
import { routing } from './cmspage.routing';
import {SmartadminEditorsModule} from "../shared/forms/editors/smartadmin-editors.module";

import {JqueryUiModule} from "../shared/ui/jquery-ui/jquery-ui.module";
import { ReactiveFormsModule }   from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap';
import {APIwithActionService} from '../shared/APIwithAction.service';
import {GlobalAPIService} from '../shared/GlobalAPI.service';
import {cmspageComponent} from './cmspage.component'
import { NgxLoadingModule } from 'ngx-loading';
import { CKEditorModule } from 'ckeditor4-angular';
@NgModule({
  declarations: [
    cmspageComponent,
   
  ],
  imports: [
    CKEditorModule,
    SmartadminModule,
    routing,
    SmartadminEditorsModule,
    SmartadminDatatableModule,
    JqueryUiModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    NgxLoadingModule.forRoot({})
  ],

  entryComponents: [cmspageComponent],
  providers:[APIwithActionService,GlobalAPIService]

})
export class cmspageModule{

}