import { Component, OnInit, EventEmitter } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';

@Component({
    selector: 'app-forecast-instr-add',
    templateUrl: './ForecastInstrAdd.component.html',
    styleUrls: ['ForecastInstrAdd.component.css']
})

export class ForecastInstrAddComponent implements OnInit {
    public event: EventEmitter<any> = new EventEmitter();
    forecastId: number;
    instrumentList = new Array();
    selectedIns = new Array();
    tagArray = new Array();
    selectedTestArea = 0;

    constructor(private _APIwithActionService: APIwithActionService, public bsModalRef: BsModalRef,) { }

    ngAfterViewChecked() {
    }

    ngOnInit() {
        this.selectedTestArea = 0;
        this._APIwithActionService.getDatabyID(this.forecastId, "Forecsatinfo", "getallinstrumentbyforecasttest").subscribe((data) => {
            this.instrumentList = data;
            for (var idx = 0; idx < this.instrumentList.length; idx++) {
                if (this.selectedTestArea == 0 && this.instrumentList[idx].instruments.length > 0) this.selectedTestArea = idx;
                for (var idx_test = 0; idx_test < this.instrumentList[idx].instruments.length; idx_test++) {
                    if (this.instrumentList[idx].instruments[idx_test].type == "A") {
                        this.tagArray.push({
                            InsID: this.instrumentList[idx].instruments[idx_test].instrumentID,
                            InsName: this.instrumentList[idx].instruments[idx_test].instrumentName
                        })
                    }
                }
            }
        });
    }

    onCloseModal() {
        this.bsModalRef.hide();
    }

    openNextModal() {
        for (var idx = 0; idx < this.tagArray.length; idx++) {
            this.selectedIns.push({
                InsID: this.tagArray[idx].InsID,
                forecastID: this.forecastId,
                userId: localStorage.getItem("userid"),
            })
        }
        console.log('this.selectedIns', this.selectedIns);
        // this._APIwithActionService.postAPI(this.selectedIns, "Instrument", "saveforecastIns").subscribe((data) => {
        this.bsModalRef.hide();
        this.event.emit({ type: "next" });
        // });

    }

    openPreviousModal() {
        this.bsModalRef.hide();
        this.event.emit({ type: "back" });
    }

    handleSelectForecastTest(index) {
        this.selectedTestArea = index;
    }

    deleteTag(removedId) {
        var removedAt = this.tagArray.findIndex(item => item.InsID == removedId);
        if (removedAt > -1) {
            this.tagArray.splice(removedAt, 1);
            for (var idx = 0; idx < this.instrumentList.length; idx++) {
                for (var idx_test = 0; idx_test < this.instrumentList[idx].instruments.length; idx_test++) {
                    if (this.instrumentList[idx].instruments[idx_test].instrumentID == removedId) {
                        this.instrumentList[idx].instruments[idx_test].type = "N";
                    }
                }
            }
        }
    }

    onCheckedChange(InsID: number, InsName: string, InsIndex: number, event: boolean) {
        if (event) {
            this.instrumentList[this.selectedTestArea].instruments[InsIndex].type = "A";
            this.tagArray.push({ InsID: InsID, InsName: InsName });
        } else {
            this.deleteTag(InsID);
        }

    }


}


