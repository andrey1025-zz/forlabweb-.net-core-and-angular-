import { Component, OnInit, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';

import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { APIwithActionService } from '../../shared/APIwithAction.service';

@Component({
    selector: 'app-forecast-instr-list',
    templateUrl: './ForecastInstrList.component.html',
    styleUrls: ['ForecastInstrList.component.css']
})

export class ForecastInstrListComponent implements OnInit {
    public event: EventEmitter<any> = new EventEmitter();
    forecastId: number;
    selectedins: FormGroup;
    instrumentlist = new Array();
    _siteins = new Array();

    constructor(private _fb: FormBuilder, private _APIwithActionService: APIwithActionService, public bsModalRef: BsModalRef, private _GlobalAPIService: GlobalAPIService) { }

    ngAfterViewChecked() {
    }

    ngOnInit() {
        this._APIwithActionService.getDatabyID(this.forecastId, "Instrument", "getAllforecastinstrumentlist").subscribe((data) => {
            this.instrumentlist = data;
            for (var boxIndex = 0; boxIndex < this.instrumentlist.length; boxIndex++) {
                this.addsiteinstrument();
                (<FormGroup>((<FormArray>this.selectedins.controls["_instrument"]).controls[boxIndex])).patchValue({
                    instrumentID: this.instrumentlist[boxIndex].insID,
                    instrumentName: this.instrumentlist[boxIndex].instrumentName,
                    areaName: this.instrumentlist[boxIndex].areaName,
                    forecastid: this.instrumentlist[boxIndex].forecastID,
                    quantity: this.instrumentlist[boxIndex].quantity,
                    TestRunPercentage: this.instrumentlist[boxIndex].testRunPercentage,
                    TestingAreaID: this.instrumentlist[boxIndex].testingAreaID,
                });
            }
        });
        this.selectedins = this._fb.group({
            _instrument: this._fb.array([]),
        });

    }

    initsiteinstrument() {
        let siteinstrument: FormGroup = this._fb.group({
            instrumentID: 0,
            instrumentName: [{ value: "" }],
            TestingAreaID: 0,
            areaName: [{ value: "" }],
            forecastid: 0,
            quantity: 0,
            TestRunPercentage: [{ value: 0 }],
        });
        return siteinstrument;
    }

    addsiteinstrument() {
        (<FormArray>this.selectedins.controls["_instrument"]).push(
            this.initsiteinstrument()
        );
    }

    delproductprice(index) {

    }

    onCloseModal() {
        this.bsModalRef.hide();
    }

    openNextModal() {
        this._siteins = [];
        let _siteins1 = <FormArray>this.selectedins.controls["_instrument"];
        _siteins1.getRawValue().forEach((x) => {
            this._siteins.push({
                InsID: x.instrumentID,
                forecastID: x.forecastid,
                UserId: parseInt(localStorage.getItem("userid")),
                Quantity: parseInt(x.quantity),
                TestRunPercentage: parseFloat(x.TestRunPercentage),
            });
        });
        let diffareaid = new Array();
        for (let index = 0; index < _siteins1.getRawValue().length; index++) {
            const element = _siteins1.getRawValue()[index];
            let j = diffareaid.findIndex((x) => x.areaid === element.TestingAreaID);
            if (j >= 0) {
                diffareaid[j].testrunpercentage = parseFloat(diffareaid[j].testrunpercentage) + parseFloat(element.TestRunPercentage);
            } else {
                diffareaid.push({
                    areaid: element.TestingAreaID,
                    testrunpercentage: element.TestRunPercentage,
                });
            }
        }
        for (let index = 0; index < diffareaid.length; index++) {
            const element = diffareaid[index];
            if (element.testrunpercentage > 100 || element.testrunpercentage < 100) {
                this._GlobalAPIService.FailureMessage("Test Run Percentage should be equal 100 for same area");
                return;
            }
        }

        for (let index = 0; index < this._siteins.length; index++) {
            if (this._siteins[index].Quantity == 0 || this._siteins[index].Quantity < 0) {
                this._GlobalAPIService.FailureMessage("Quantity Should be greater than zero");
                return;
            }

            if (this._siteins[index].TestRunPercentage > 100) {
                this._GlobalAPIService.FailureMessage("Test Run Percentage Should be less than one hundred");
                return;
            } else if (this._siteins[index].TestRunPercentage < 0) {
                this._GlobalAPIService.FailureMessage("Test Run Percentage Should be equal or greater than zero");
                return;
            }
        }

        var insobject = {
            ForecastIns: this._siteins,
        };
        console.log("insobject", insobject);
        // this._APIwithActionService.postAPI(insobject, "Instrument", "Updateforecastinstrument").subscribe((data) => {
        this.bsModalRef.hide();
        this.event.emit({ type: "next" });
        // });
    }

    openPreviousModal() {
        this.bsModalRef.hide();
        this.event.emit({ type: "back" });
    }

}

