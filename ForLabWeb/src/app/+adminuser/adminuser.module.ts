import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {routing} from "./adminuser.routing";
import { adminuserComponent } from './adminuser.component';

import { FormsModule }   from '@angular/forms';
import { APIwithActionService } from "../shared/APIwithAction.service" ;
import { GlobalAPIService } from "../shared/GlobalAPI.service" ;
import { SmartadminModule } from '../shared/smartadmin.module';

import {SharedequalModule} from '../shared/Equalvalidateshared.module';
@NgModule({
  imports: [
    CommonModule,
    SharedequalModule,
    routing,
    FormsModule,
    SmartadminModule

  ],
  declarations: [ adminuserComponent],
  entryComponents:[adminuserComponent],
  providers:[APIwithActionService,GlobalAPIService]
})
export class adminuserModule
{

}