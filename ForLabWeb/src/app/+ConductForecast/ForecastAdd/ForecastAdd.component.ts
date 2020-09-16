import { Component, OnInit, EventEmitter } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { ForecastTestAddComponent } from '../ForecastTestAdd/ForecastTestAdd.component';

@Component({
    selector: 'app-forecast-add',
    templateUrl: './ForecastAdd.component.html',
    styleUrls: ['ForecastAdd.component.css']
})

export class ForecastAddComponent implements OnInit {
    public event: EventEmitter<any> = new EventEmitter();
    myForecastType: any;
    myForecastTime: any;
    forecastTypeList: any[] = ['National', 'Custom', 'Global'];
    forecastTimeList: any[] = ['Bimonthly', 'Monthly', 'Quarterly', 'Yearly'];

    constructor(
        private _avRoute: ActivatedRoute,
        private _router: Router,
        private _APIwithActionService: APIwithActionService,
        public bsModalRef: BsModalRef,
        private modalService: BsModalService
    ) {
        this.myForecastType = 1;
        this.myForecastTime = 1;
    }

    ngAfterViewChecked() {
    }

    ngOnInit() {
    }

    save() {
    }

    handleForecastType(index: any) {
        this.myForecastType = index;
    }

    handleForecastTime(index: any) {
        this.myForecastTime = index;
    }

    onCloseModal() {
        this.bsModalRef.hide();
    }

    openNextModal() {
        this.bsModalRef.hide();
        this.bsModalRef = null;

        this.bsModalRef = this.modalService.show(ForecastTestAddComponent, { class: 'modal-forecastadd', ignoreBackdropClick: false });
        this.bsModalRef.content.event.subscribe(res => {
            console.log("res", res);
        })
    }

}

