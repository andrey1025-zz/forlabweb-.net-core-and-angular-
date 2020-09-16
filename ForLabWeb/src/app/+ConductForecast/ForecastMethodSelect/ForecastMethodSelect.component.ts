import { Component, OnInit, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { APIwithActionService } from '../../shared/APIwithAction.service';

@Component({
    selector: 'app-forecast-instr-list',
    templateUrl: './ForecastMethodSelect.component.html',
    styleUrls: ['ForecastMethodSelect.component.css']
})

export class ForecastMethodSelectComponent implements OnInit {
    public event: EventEmitter<any> = new EventEmitter();
    currentMethod = 0;

    constructor(
        private _avRoute: ActivatedRoute,
        private _router: Router,
        private _APIwithActionService: APIwithActionService,
        public bsModalRef: BsModalRef,
        private modalService: BsModalService
    ) {
    }

    ngAfterViewChecked() {
    }

    ngOnInit() {
    }

    save() {
    }

    handleSelectMethod(index) {
        this.currentMethod = index;
    }

    onCloseModal() {
        this.bsModalRef.hide();
    }

    openNextModal() {
        this.bsModalRef.hide();
        this.event.emit({ type: "next" });
    }

    openPreviousModal() {
        this.bsModalRef.hide();
        this.event.emit({ type: "back" });
    }

}

