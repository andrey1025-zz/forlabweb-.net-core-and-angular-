import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';

@Component({
    selector: 'app-CateAdd',
    templateUrl: './CategoryAdd.component.html',
    styleUrls: ['./CategoryAdd.component.css']
})

export class AddSitecategoryComponent implements OnInit {
    @Output() close = new EventEmitter()
    categoryForm: FormGroup;
    title: string = "Create";
    id: number;
    errorMessage: any;
    catList = new Array();

    constructor(
        private _fb: FormBuilder,
        private _avRoute: ActivatedRoute,
        private _GlobalAPIService: GlobalAPIService,
        private _router: Router
    ) {
        if (this._avRoute.snapshot.params["id"]) {
            this.id = this._avRoute.snapshot.params["id"];
        }
    }

    getSiteCategories() {
        this._GlobalAPIService.getDataList('SiteCategory').subscribe((data) => {
            this.catList = data
        }), err => {
            console.log(err);
        }
    }

    ngOnInit() {
        this.categoryForm = this._fb.group({
            CategoryID: 0,
            categoryName: ['', Validators.compose([Validators.required, Validators.maxLength(64)])]
        })

        if (this.id > 0) {
            this.title = "Edit";
            this._GlobalAPIService.getDatabyID(this.id, 'SiteCategory')
                .subscribe((resp) => {
                    this.categoryForm.setValue({
                        CategoryID: resp["categoryID"],
                        categoryName: resp["categoryName"]
                    });
                }, error => this.errorMessage = error);
        }
    }

    save() {
        if (this.title == "Create") {
            this._GlobalAPIService.postAPI(this.categoryForm.value, 'SiteCategory')
                .subscribe((data) => {
                    if (data["_body"] != "0") {

                        this._GlobalAPIService.SuccessMessage("SiteCategory Saved Successfully");
                        this._router.navigate(['/Managedata/ManagedataList', 6]);
                    }
                    else {
                        this._GlobalAPIService.FailureMessage("SiteCategory already exists");
                    }

                }, error => this.errorMessage = error)
        }
        else if (this.title == "Edit") {
            this._GlobalAPIService.putAPI(this.id, this.categoryForm.value, 'SiteCategory')
                .subscribe((data) => {
                    if (data["_body"] == "Success") {
                        this._GlobalAPIService.SuccessMessage("SiteCategory Updated Successfully");
                        this._router.navigate(['/Managedata/ManagedataList', 6]);
                    }
                    else {
                        this._GlobalAPIService.FailureMessage("SiteCategory already exists");
                    }
                }, error => this.errorMessage = error)
        }
    }

    clearctrl() {
        this.close.emit(true);
    }
}  