import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {copydefaultdataRouting} from "./copydefaultdata.routing";
import {CopydefaultdataComponent} from "./copydefaultdata.component"
import { FormsModule }   from '@angular/forms';
import { APIwithActionService } from "../shared/APIwithAction.service" ;
import { GlobalAPIService } from "../shared/GlobalAPI.service" ;
import { SmartadminModule } from '../shared/smartadmin.module';

@NgModule({
  imports: [
    CommonModule,

    copydefaultdataRouting,
    FormsModule,
    SmartadminModule

  ],
  declarations: [CopydefaultdataComponent],
 
  providers:[APIwithActionService,GlobalAPIService]
})
export class copydefaultdataModule { }