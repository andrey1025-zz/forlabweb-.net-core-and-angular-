import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { routing } from "./auth.routing";
import { AuthComponent } from './auth.component';
import { FormsModule } from '@angular/forms';
import { TermsModalComponent } from './+register/terms-modal/terms-modal.component';
import { APIwithActionService } from "../shared/APIwithAction.service";
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { SmartadminModule } from '../shared/smartadmin.module';
import { ReactiveFormsModule } from '@angular/forms';
import { VerifylinkComponent } from './verifylink/verifylink.component';
import { ForgotComponent } from './+forgot/forgot.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { SharedequalModule } from '../shared/Equalvalidateshared.module';
import { D3Module } from "../shared/graphs/d3/d3.module";
import { NgxLoadingModule } from 'ngx-loading';
import { LandingpageComponent } from './landingpage/landingpage.component';
import { LandingpagenewComponent } from './landingpagenew/landingpagenew.component';
import { LoginModalComponent } from "./loginmodal/loginmodal.component"
import { LoginComponent } from './+login/login.component';
import {RegisterComponent} from './+register/register.component';
import { RegisterModalComponent } from './registermodal/registermodal.component';
import { BsDropdownModule } from 'ngx-bootstrap';
import { ForgotpwdModalComponent } from './forgotpwdmodal/forgotpwdmodal.component';
@NgModule({
  imports: [
    CommonModule,
    SharedequalModule,
    ReactiveFormsModule,
    routing,
    FormsModule, NgxLoadingModule.forRoot({}),
    SmartadminModule,
    D3Module,
    BsDropdownModule.forRoot()

  ],
  declarations: [LoginComponent,RegisterComponent,AuthComponent, LoginModalComponent, RegisterModalComponent, ForgotpwdModalComponent, TermsModalComponent, VerifylinkComponent, ForgotComponent, ResetpasswordComponent, LandingpageComponent, LandingpagenewComponent],
  entryComponents: [AuthComponent],
  providers: [APIwithActionService, GlobalAPIService]
})
export class AuthModule { }
