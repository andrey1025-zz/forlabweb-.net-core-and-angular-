import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    selector: 'app-TestAreaAdd',
    templateUrl: './TestingAreaAdd.component.html',
    styleUrls: ['TestingAreaAdd.component.css']
})

export class TestingAreaAddComponent implements OnInit {

    public event: EventEmitter<any> = new EventEmitter();
    itemID: any;
    submitBtnName = "Add Testing Area";
    testAreaForm: FormGroup;
    errorMessage: any;
    constructor(private _fb: FormBuilder, public bsModalRef: BsModalRef, private _GlobalAPIService: GlobalAPIService) { }

    ngOnInit() {
        this.testAreaForm = this._fb.group({
            TestingAreaID: 0,
            areaName: ['', Validators.compose([Validators.required, Validators.maxLength(64)])]
        })
        if (this.itemID > 0) {
            this.submitBtnName = "Update Testing Area";
            this._GlobalAPIService.getDatabyID(this.itemID, 'TestArea').subscribe((resp) => {
                this.testAreaForm.patchValue({
                    TestingAreaID: resp["testingAreaID"],
                    areaName: resp["areaName"]
                });
            }, error => this.errorMessage = error);
        }
    }

    save() {
        if (!this.testAreaForm.valid) {
            return;
        }
        if (!this.itemID) {
            this._GlobalAPIService.postAPI(this.testAreaForm.value, 'TestArea').subscribe((data) => {
                if (data["_body"] == "Success") {
                    this._GlobalAPIService.SuccessMessage("Testing Area Saved Successfully");
                    this.event.emit(this.testAreaForm.value);
                    this.bsModalRef.hide();
                } else {
                    this._GlobalAPIService.FailureMessage("Testing Area already exists");
                }
            }, error => this.errorMessage = error)
        } else {
            this._GlobalAPIService.putAPI(this.itemID, this.testAreaForm.value, 'TestArea').subscribe((data) => {
                if (data["_body"] == "Success") {
                    this._GlobalAPIService.SuccessMessage("Testing Area Updated Successfully");
                    this.event.emit(this.testAreaForm.value);
                    this.bsModalRef.hide();
                } else {
                    this._GlobalAPIService.FailureMessage("Testing Area already exists");
                }
            }, error => this.errorMessage = error)
        }
    }

    onCloseModal() {
        this.bsModalRef.hide();
    }
}  