import { Component, OnInit, EventEmitter } from '@angular/core';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Location } from '@angular/common';
import { testingArea } from '../../shared/GlobalInterface';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    selector: 'app-insadd',
    templateUrl: './InstrumentAdd.component.html',
    styleUrls: ['InstrumentAdd.component.css']
})

export class InstrumentAddComponent implements OnInit {
    public event: EventEmitter<any> = new EventEmitter();
    itemID: any;
    submitBtnName = "Add Instrument";
    instrumentForm: FormGroup;
    title: string = "Create";
    errorMessage: any;
    idxControlReq: any;
    controlReqArray: any[] = ['Daily', 'Weekly', 'Monthly', 'Quarterly', 'Per Test'];
    idxTestArea: any;
    public testingAreaList: testingArea[];
    testAreaDisabled: boolean = false;

    constructor(private _fb: FormBuilder, public bsModalRef: BsModalRef, private _GlobalAPIService: GlobalAPIService,
        private _APIwithActionService: APIwithActionService) {
    }

    ngAfterViewChecked() {
    }

    ngOnInit() {
        if (this.itemID) {
            this.submitBtnName = "Update Instrument";
        } else {
            this.idxControlReq = 1;
            this.idxTestArea = 1;
        }
        this.instrumentForm = this._fb.group({
            InstrumentID: 0,
            InstrumentName: ['', Validators.compose([Validators.required, Validators.maxLength(64)])],
            TestingArea: [''],
            Frequency: [''],
            MaxThroughPut: [0, [Validators.required]],
            noofrun: [0, [Validators.required]],
        })
        this._GlobalAPIService.getDataList('TestArea').subscribe((data) => {
            this.testingAreaList = data.aaData
            if (this.itemID > 0) {
                let Frequency1;
                let noofrun1 = -1;
                this.title = "Edit";
                this._APIwithActionService.getDatabyID(this.itemID, 'Instrument', 'GetbyId').subscribe((resp) => {
                    if (resp["dailyCtrlTest"] != "0") {
                        Frequency1 = 1;
                        noofrun1 = resp["dailyCtrlTest"];
                    }
                    else if (resp["weeklyCtrlTest"] != "0") {
                        Frequency1 = 2;
                        noofrun1 = resp["weeklyCtrlTest"];
                    }
                    else if (resp["monthlyCtrlTest"] != "0") {
                        Frequency1 = 3;
                        noofrun1 = resp["monthlyCtrlTest"];
                    }
                    else if (resp["quarterlyCtrlTest"] != "0") {
                        Frequency1 = 4;
                        noofrun1 = resp["quarterlyCtrlTest"];
                    }
                    else if (resp["maxTestBeforeCtrlTest"] != "0") {
                        Frequency1 = 5;
                        noofrun1 = resp["maxTestBeforeCtrlTest"];
                    }
                    this.idxTestArea = this.testingAreaList.findIndex(x => x.testingAreaID == resp["testingArea"].testingAreaID);
                    this.instrumentForm.setValue({
                        InstrumentID: resp["instrumentID"],
                        InstrumentName: resp["instrumentName"],
                        TestingArea: [this.idxTestArea],
                        Frequency: [''],
                        MaxThroughPut: resp["maxThroughPut"],
                        noofrun: noofrun1
                    });
                    this.idxControlReq = +Frequency1 - 1;
                    // this.instrumentForm.get('TestingArea').disable();
                    this.testAreaDisabled = true;
                }, error => this.errorMessage = error);
            }
        }), err => {
            console.log(err);
        }
    }

    save() {
        this.instrumentForm.patchValue({
            TestingArea: this.testingAreaList[this.idxTestArea].testingAreaID,
            Frequency: (this.idxControlReq + 1).toString()
        });
        console.log('this.instrumentForm.value', this.instrumentForm.value);
        if (this.title == "Create") {
            this._APIwithActionService.postAPI(this.instrumentForm.value, 'Instrument', 'Post01').subscribe((data) => {
                if (data["_body"] == "Success") {
                    this._GlobalAPIService.SuccessMessage("Instrument Saved Successfully");
                    this.event.emit(this.instrumentForm.value);
                    this.bsModalRef.hide();
                }
                else {
                    this._GlobalAPIService.FailureMessage("Instrument Already exists");
                }
            }, error => this.errorMessage = error)
        }
        else if (this.title == "Edit") {
            this._APIwithActionService.putAPI(this.itemID, this.instrumentForm.value, 'Instrument', 'Put01').subscribe((data) => {
                if (data["_body"] == "Success") {
                    this._GlobalAPIService.SuccessMessage("Instrument Updated Successfully");
                    this.event.emit(this.instrumentForm.value);
                    this.bsModalRef.hide();
                }
                else {
                    this._GlobalAPIService.FailureMessage("Duplicate Instrument Name");
                }
            }, error => this.errorMessage = error)
        }
    }

    handleTestingArea(index) {
        this.idxTestArea = index;
    }
    handleControlReq(index: any) {
        this.idxControlReq = index;
    }

    onCloseModal() {
        this.bsModalRef.hide();
    }
}