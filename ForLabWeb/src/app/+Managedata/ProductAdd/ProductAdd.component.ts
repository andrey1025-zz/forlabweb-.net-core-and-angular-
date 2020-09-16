import { Component, OnInit, ChangeDetectorRef, ViewChild, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { ProductType, ProductPrice } from '../../shared/GlobalInterface';
import { Location } from '@angular/common';
import { ModalDirective, BsModalRef } from "ngx-bootstrap";
import { GlobalVariable } from 'app/shared/globalclass';
import { ModalDatatableComponent } from 'app/shared/ui/datatable/ModalDatatable.component';

@Component({
    selector: 'app-ProAdd',
    templateUrl: './ProductAdd.component.html',
    styleUrls: ['./ProductAdd.component.css']
})

export class ProductAddComponent implements OnInit {
    public event: EventEmitter<any> = new EventEmitter();
    itemID: any;
    submitBtnName = "Add Product";

    productForm: FormGroup;
    Priceform: FormGroup;
    PriceId: number;
    errorMessage: any;
    Nflag: boolean;
    date: Date;
    public ProductTypeList: ProductType[];
    public ProductPriceList = new Array();
    private ProductPrice: any = {};
    buttonstatus = true;
    saUiDatepicker: any;
    datepickerModel: Date;
    ProductPriceids: string = "";
    currentTestArea = 2;
    @ViewChild(ModalDatatableComponent) DataView: ModalDatatableComponent;

    constructor(private _fb: FormBuilder, public bsModalRef: BsModalRef, private _APIwithActionService: APIwithActionService,
        private _router: Router, private _GlobalAPIService: GlobalAPIService) {
        //  this.ref.markForCheck()
        if (this.itemID > 0) {
            this._APIwithActionService.getDatabyID(this.itemID, 'Product', 'GetbyId').subscribe((resp) => {
                console.log(resp)
                this.productForm.patchValue({
                    ProductID: resp["productID"],
                    ProductName: resp["productName"],
                    ProductTypeId: resp["productTypeId"],
                    SerialNo: resp["serialNo"],
                    BasicUnit: resp["basicUnit"],
                    MinimumPackPerSite: resp["minimumPackPerSite"]
                });
                this.ProductPriceList = resp["_productPrices"]
                this.fillproductprice()
                this.productForm.get('ProductTypeId').disable();
            }, error => this.errorMessage = error);
        }
    }
    initproductprice() {
        let productprice: FormGroup = this._fb.group({
            id: 0,
            Price: [''],
            PackSize: [''],
            FromDate: null
        });
        return productprice;
    }
    addproductprice() {
        (<FormArray>this.productForm.controls["_productPrices"]).push(
            this.initproductprice()
        );
    }
    delproductprice(i) {
        this.ProductPriceList.splice(i, 1);
        let delid: String
        delid = (<FormGroup>(<FormArray>this.productForm.controls["_productPrices"]).controls[i]).controls["id"].value
        if (delid != "0") {
            this.ProductPriceids = this.ProductPriceids + "," + delid;
        }
        (<FormArray>this.productForm.controls["_productPrices"]).removeAt(i)
    }

    addNewProductPrice() {
        let price = parseFloat(this.productForm.controls["Price"].value)
        let packsize = parseFloat(this.productForm.controls["PackSize"].value)
        let AsOfDate = this.productForm.controls["AsOfDate"].value

        if (packsize != 0 && AsOfDate != null) {
            let index = this.ProductPriceList.indexOf(this.ProductPriceList.find(x => x.packSize === packsize && x.price === price))
            if (index >= 0) {
                this._GlobalAPIService.FailureMessage('Price already added for same packsize and Date');
            }
            else {
                this.ProductPriceList.push({
                    id: 0,
                    price: price,
                    packSize: packsize,
                    fromDate: new Date(AsOfDate),
                    ProductId: this.productForm.controls["ProductID"].value
                })
                let boxIndex = this.ProductPriceList.length - 1;
                this.addproductprice();
                (<FormGroup>(
                    (<FormArray>this.productForm.controls["_productPrices"]).controls[boxIndex]
                )).patchValue({
                    id: this.ProductPriceList[boxIndex].id,
                    Price: this.ProductPriceList[boxIndex].price,
                    PackSize: this.ProductPriceList[boxIndex].packSize,
                    FromDate: new Date(this.ProductPriceList[boxIndex].fromDate)

                });
            }
        }
        this.productForm.patchValue({
            Price: '',
            PackSize: '',
            AsOfDate: null
        })

        this.productForm.controls['Price'].markAsPristine();
        this.productForm.controls['PackSize'].markAsPristine();
        this.productForm.controls['AsOfDate'].markAsPristine();
    }
    getproducttype() {
        this._GlobalAPIService.getDataList('ProductType').subscribe((data) => {
            this.ProductTypeList = data.aaData
            console.log(this.ProductTypeList)
        }
        ), err => {
            console.log(err);
        }

    }

    ngOnInit() {
        this.getproducttype();

        //   const control:FormArray = this.productForm.get('_productPrice') as FormArray;
        this.productForm = this._fb.group({
            ProductID: 0,
            ProductName: ['', Validators.compose([Validators.required, Validators.maxLength(150)])],
            ProductTypeId: ['', [Validators.required]],
            SerialNo: [''],
            BasicUnit: ['', [Validators.required]],
            MinimumPackPerSite: 0,
            Price: [''],
            PackSize: [''],
            AsOfDate: null,
            _productPrices: this._fb.array([

            ])
        });
    }

    save() {
        console.log(this.productForm.valid)
        if (!this.productForm.valid) {
            return;
        }
        if (this.ProductPriceList.length == 0) {
            this._GlobalAPIService.FailureMessage("Price Should not be empty")
            return;
        }

        console.log(this.productForm.value)

        if (!this.itemID) {
            this._APIwithActionService.postAPI(this.productForm.value, 'Product', 'Post01').subscribe((data) => {
                if (data["_body"] != 0) {
                    this._GlobalAPIService.SuccessMessage("Product Saved Successfully");
                    this._router.navigate(['/Managedata/ManagedataList', 4]);
                }
                else {
                    this._GlobalAPIService.FailureMessage("Product Name Already exist");
                }


            }, error => this.errorMessage = error)
        } else {
            this._APIwithActionService.putAPI(this.itemID, this.productForm.value, 'Product', 'Put01').subscribe((data) => {
                if (data["_body"] != 0) {

                    this._APIwithActionService.deleteData(this.ProductPriceids, 'Product', 'Deleteproudctprice')
                        .subscribe((data) => {
                        })
                    this._GlobalAPIService.SuccessMessage("Product Updated Successfully");
                    this._router.navigate(['/Managedata/ManagedataList', 4]);
                }
                else {
                    this._GlobalAPIService.FailureMessage("Product Name Already exist");
                }

            }, error => this.errorMessage = error)
        }
    }

    fillproductprice() {
        for (let boxIndex = 0; boxIndex < this.ProductPriceList.length; boxIndex++) {
            this.addproductprice();
            (<FormGroup>(
                (<FormArray>this.productForm.controls["_productPrices"]).controls[boxIndex]
            )).patchValue({
                id: this.ProductPriceList[boxIndex].id,
                Price: this.ProductPriceList[boxIndex].price,
                PackSize: this.ProductPriceList[boxIndex].packSize,
                FromDate: new Date(this.ProductPriceList[boxIndex].fromDate),

            });
        }
    }

    handleSelectTestArea(index) {
        this.currentTestArea = index;
    }

    onCloseModal() {
        this.bsModalRef.hide();
    }
}

