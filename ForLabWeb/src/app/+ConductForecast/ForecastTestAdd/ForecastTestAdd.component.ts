import { Component, OnInit, EventEmitter } from '@angular/core';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    selector: 'app-forecast-test-add',
    templateUrl: './ForecastTestAdd.component.html',
    styleUrls: ['ForecastTestAdd.component.css']
})

export class ForecastTestAddComponent implements OnInit {
    public event: EventEmitter<any> = new EventEmitter();
    myForecastType: any;
    myForecastTime: any;
    forecastTypeList: any[] = ['National', 'Custom', 'Global'];
    forecastTimeList: any[] = ['Bimonthly', 'Monthly', 'Quarterly', 'Yearly'];
    currentForecastTest = 1;

    checkboxArray = ["", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",];
    tagArray = ["", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",];

    constructor(
        private _avRoute: ActivatedRoute,
        private _router: Router,
        private _APIwithActionService: APIwithActionService,
        public bsModalRef: BsModalRef
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

    handleSelectForecastTest(index) {
        this.currentForecastTest = index;
    }

    deleteTag(index) {
        if (index < 0) return;
        this.tagArray.splice(index, 1);
    }

}


