import { Component, OnInit, Renderer, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-ProTypeAdd',
  templateUrl: './ProductTypeAdd.component.html',
  styleUrls: ['./ProductTypeAdd.component.css']
})

export class ProductTypeAddComponent implements OnInit {
  public event: EventEmitter<any> = new EventEmitter();
  itemID: any;
  submitBtnName = "Add Product Type";
  productTypeForm: FormGroup;
  errorMessage: any;

  constructor(private _fb: FormBuilder, public bsModalRef: BsModalRef, private _GlobalAPIService: GlobalAPIService) { }

  ngOnInit() {
    this.productTypeForm = this._fb.group({
      typeID: 0,
      typeName: ['', Validators.compose([Validators.required, Validators.maxLength(64)])]
    })

    if (this.itemID > 0) {
      this.submitBtnName = "Update Product Type";
      this._GlobalAPIService.getDatabyID(this.itemID, 'ProductType').subscribe((resp) => {
        console.log("resp", resp);
        this.productTypeForm.setValue({
          typeID: resp["typeID"],
          typeName: resp["typeName"]
        });
      }, error => this.errorMessage = error);
    }
  }

  save() {
    if (!this.productTypeForm.valid) {
      return;
    }
    if (!this.itemID) {
      this._GlobalAPIService.postAPI(this.productTypeForm.value, 'ProductType').subscribe((data) => {
        if (data["_body"] == "Success") {
          this._GlobalAPIService.SuccessMessage("ProductType Saved Successfully");
          this.event.emit(this.productTypeForm.value);
          this.bsModalRef.hide();
        } else {
          this._GlobalAPIService.FailureMessage("ProductType already exists");
        }
      }, error => this.errorMessage = error)
    } else {
      this._GlobalAPIService.putAPI(this.itemID, this.productTypeForm.value, 'ProductType').subscribe((data) => {
        if (data["_body"] == "Success") {
          this._GlobalAPIService.SuccessMessage("ProductType Updated Successfully");
          this.event.emit(this.productTypeForm.value);
          this.bsModalRef.hide();
        } else {
          this._GlobalAPIService.FailureMessage("ProductType already exists");
        }
      }, error => this.errorMessage = error)
    }
  }

  onCloseModal() {
    this.bsModalRef.hide();
  }
}  