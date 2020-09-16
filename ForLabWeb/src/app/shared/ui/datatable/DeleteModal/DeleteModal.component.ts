import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Location } from '@angular/common';
import { testingArea } from '../../shared/GlobalInterface';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    selector: 'app-delete-modal',
    templateUrl: './DeleteModal.component.html',
    styleUrls: ['DeleteModal.component.css']
})

export class DeleteModalComponent implements OnInit {
    public event: EventEmitter<any> = new EventEmitter();

    constructor(public bsModalRef: BsModalRef) { }

    ngAfterViewChecked() {
    }

    ngOnInit() {
    }

    onYesModal() {
        this.event.emit({ type: "delete" });
        this.bsModalRef.hide();
    }
    onCloseModal() {
        this.bsModalRef.hide();
    }
}