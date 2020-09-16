import { Component, OnInit, ChangeDetectorRef, ViewChild, EventEmitter, Output } from '@angular/core';

import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Instrumentlist, ProductType, testingArea, Product, ProductUsageRatelist } from '../../shared/GlobalInterface';
import { Location } from '@angular/common';
import { ModalDirective } from "ngx-bootstrap";
import { element } from 'protractor';
import { group } from '@angular/core/src/animation/dsl';
import { ModalDatatableComponent } from 'app/shared/ui/datatable/ModalDatatable.component';

// import { IMultiSelectOption,IMultiSelectTexts,IMultiSelectSettings } from 'angular-2-dropdown-multiselect';
@Component({
    selector: 'app-TestAdd',
    templateUrl: './Testadd.component.html',
    styleUrls: ['./TestAdd.component.css']
})

export class TestAddComponent implements OnInit {


    @Output() close = new EventEmitter()
    testForm: FormGroup;
    consumableform: FormGroup;
    title: string = "Create";
    id: number;
    PriceId: number;
    errorMessage: any;
    Nflag: boolean;
    date: Date;
    testinsID: Number;
    testinsname: String;
    testproID: Number;
    testproname: String;
    controlinsID: Number;
    controlinsname: String;
    controlproID: Number;
    controlproname: String;
    consumtestproID: Number;
    consumtestproname: String;
    consumtesttypeID: Number;
    consumtesttypename: String;
    consumcontypeID: Number;
    consumcontypename: String;
    consumconperiodID: Number;
    consumconperiod: String;
    consuminstypeID: Number;
    consuminstypename: String;
    consumconproID: Number;
    consumconproname: String;
    consuminsID: Number;
    consuminsname: String;
    consuminsproID: Number;
    consuminsproname: String;
    consuminsperiodID: Number;
    consuminsperiodname: String;
    public instrumentList: Instrumentlist[];
    public productTypeList: ProductType[];
    public productList: Product[];
    public testingAreaList: testingArea[];
    private ProductUsageRatelist: ProductUsageRatelist[];
    private ProductPrice: any = {};
    buttonstatus = true;
    saUiDatepicker: any;
    datepickerModel: Date;
    //fill product usages rate using these array
    testprod = new Array();
    controlprd = new Array();
    consumbletestprd = new Array();
    consumbleperiodprd = new Array();
    consumbleinstrumentprd = new Array();
    selectedItems = new Array();
    Delidsproductusage: string = "";
    Delidscontrolusage: string = "";
    Delidsconsumusage: string = "";

    currentTestArea = 2;

    currentPerPeriod = 1;
    currentPerInstr = 1;
    perPeriodArray: any[] = ['Daily', 'Weekly', 'Monthly', 'Quarterly', 'Yearly'];
    perInstrArray: any[] = ['Daily', 'Weekly', 'Monthly', 'Quarterly', 'Yearly'];

    @ViewChild('mdModal') public mdModal: ModalDirective;
    @ViewChild(ModalDatatableComponent) DataView: ModalDatatableComponent

    constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
        private _APIwithActionService: APIwithActionService, private _router: Router,
        private _GlobalAPIService: GlobalAPIService, private ref: ChangeDetectorRef

    ) {
        this.testForm = this._fb.group({
            testID: 0,
            testName: ['', Validators.compose([Validators.required, Validators.maxLength(64)])],
            TestingAreaID: ['', [Validators.required]],
            nooftest: [''],
            testproductlist: [],
            controlproductlist:[],
            consumtestproductlist:[],

       
            consumperiodproductlist:[],
            consuminsproductlist:[],
            ProductUsageArray: this._fb.array([]),
            ControlUsageArray: this._fb.array([]),
            ConsumableTestArray: this._fb.array([]),
            ConsumablePeriodArray: this._fb.array([]),
            ConsumableInsArray: this._fb.array([])
        })

        this.consumableform = this._fb.group({
            MasterCID: [''],
            TestingAreaId: [''],
            TestId: [''],
            _consumablesUsages: this._fb.array([

            ])

        })
        this.getTestingArea();
        //  this.ref.markForCheck()
        if (this._avRoute.snapshot.params["id"]) {
            this.id = this._avRoute.snapshot.params["id"];
        }
        if (this.id > 0) {

            this.title = "Edit";

            this._APIwithActionService.getDatabyID(this.id, 'Test', 'GetbyId')
                .subscribe((resp) => {

                    this.testForm.patchValue({
                        testID: resp["testID"],
                        testName: resp["testName"],
                        TestingAreaID: resp["testingAreaID"],
                    })

                    this.testprod = resp["productusage"];
                    this.controlprd = resp["controlusage"];
                    this.consumbletestprd = resp["consumablepertest"];
                    this.consumbleperiodprd = resp["consumableperperiod"];
                    this.consumbleinstrumentprd = resp["consumableperins"];
                    console.log(this.testForm.value);
                    this.testForm.get('TestingAreaID').disable();
                    console.log(this.testForm.getRawValue());
                    this.getinstrumentlist(this.testForm.get('TestingAreaID').value)

                    this.updateExistingValue();
                }
                , error => this.errorMessage = error);
        }
    }

    ngOnInit() {
        this.getTestingArea();
        // this.getinstrumentlist();
        this.getProductType();
        //   const control:FormArray = this.Productform.get('_productPrice') as FormArray;

        this.DataView.options = {
            dom: "Bfrtip",
            aaSorting: [],
            ajax: (data, callback, settings) => {

                this._APIwithActionService.getDataList('Test', 'GetAll')
                .subscribe((data) => {
                    callback({
                    aaData: data.aaData,

                    })

                })
            },
            columns: [
                { data: 'testName' },
                {
                    render: () => {
                        return `<div class="mt-touch"><input type="number" class="form-control color-darkblue" pattern="[0-9]+" minlength="0" maxlength="4" required></div>`;
                    }
                },
                {
                    render: (data, type, fullRow, meta) => {
                        return `<a class='del icon-trash' data-test-id="${fullRow.testID}"></a>`;
                    }
                }
            ],
        };

    }
    openproductpage()
    {
        this._router.navigate(["/Managedata/ProductAdd"])
    }
    inittestprod() {
        // Initialize add Box form field
        let boxForm: FormGroup = this._fb.group({
            name: [{ value: '', disabled: true }, [Validators.required]],
            values: this._fb.array([])
        });
        return boxForm;
    }

    /*
     * Function Declare to Add Multiple Boxes
     * */
    addtestprod() {
        (<FormArray>this.testForm.controls["ProductUsageArray"]).push(
            this.inittestprod()
        );
        // get box length for box name like box 1,box 2 in sidebar boxes combo list
        let connection_boxes_length = (<FormArray>(
            this.testForm.controls["ProductUsageArray"]
        )).length;
    }

    /*
     * Operation for connection pack
     * This function also used for create Dynamic Packs
     * */
    initvalue() {
        let packs: FormGroup = this._fb.group({
            id: 0,
            productId: 0,
            productName: [{ value: '', disabled: true }],
            instrumentId: 0,
            instrumentName: [""],
            rate: 1.0000000000,
            isForControl: false,
            testId: 0
        });
        return packs;
    }

    /*
     * This Function add packs Dynamically
     * */
    addvalue(boxIndex: number) {
        //declare to add packs
        (<FormArray>(
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ProductUsageArray"]).controls[
                boxIndex
                ]
            )).controls["values"]
        )).push(this.initvalue());
    }







    initcontrolUsage() {
        // Initialize add Box form field
        let boxForm: FormGroup = this._fb.group({
            name: [{ value: '', disabled: true }, [Validators.required]],
            values: this._fb.array([])
        });
        return boxForm;
    }

    /*
     * Function Declare to Add Multiple Boxes
     * */
    addcontrolusage() {
        (<FormArray>this.testForm.controls["ControlUsageArray"]).push(
            this.initcontrolUsage()
        );
        // get box length for box name like box 1,box 2 in sidebar boxes combo list
        let connection_boxes_length = (<FormArray>(
            this.testForm.controls["ControlUsageArray"]
        )).length;
    }

    /*
     * Operation for connection pack
     * This function also used for create Dynamic Packs
     * */
    initcontrolUsagevalue() {
        let packs: FormGroup = this._fb.group({
            id: 0,
            productId: 0,
            productName: [{ value: '', disabled: true }],
            instrumentId: 0,
            instrumentName: [""],
            rate: 1.0000000000,
            isForControl: true,
            testId: 0
        });
        return packs;
    }

    /*
     * This Function add packs Dynamically
     * */
    addcontrolUsagevalue(boxIndex: number) {
        //declare to add packs
        (<FormArray>(
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ControlUsageArray"]).controls[
                boxIndex
                ]
            )).controls["values"]
        )).push(this.initcontrolUsagevalue());
    }


    initconsumableusage() {
        // Initialize add Box form field
        let boxForm: FormGroup = this._fb.group({
            name: [{ value: '', disabled: true }, [Validators.required]],
            values: this._fb.array([])
        });
        return boxForm;
    }

    /*
     * Function Declare to Add Multiple Boxes
     * */
    addconsumableusage() {
        (<FormArray>this.testForm.controls["ConsumableTestArray"]).push(
            this.initconsumableusage()
        );
        // get box length for box name like box 1,box 2 in sidebar boxes combo list
        let connection_boxes_length = (<FormArray>(
            this.testForm.controls["ConsumableTestArray"]
        )).length;
    }

    /*
     * Operation for connection pack
     * This function also used for create Dynamic Packs
     * */
    initconsumableusagevalue() {
        let packs: FormGroup = this._fb.group({

            id: 0,
            consumableId: 0,
            testId: 0,
            productId: 0,
            productName: [{ value: '', disabled: true }],
            productTypeId: 0,
            productTypeName: [""],
            instrumentId: 0,
            instrumentName: [""],
            usageRate: 1.0000000000,
            perTest: true,
            perPeriod: false,
            perInstrument: false,
            noOfTest: 0,
            period: [{ value: '', disabled: true }]

        });
        return packs;
    }

    /*
     * This Function add packs Dynamically
     * */
    addconsumableusagevalue(boxIndex: number) {
        //declare to add packs
        (<FormArray>(
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ConsumableTestArray"]).controls[
                boxIndex
                ]
            )).controls["values"]
        )).push(this.initconsumableusagevalue());
    }

    initconsumableusageperiod() {
        // Initialize add Box form field
        let boxForm: FormGroup = this._fb.group({
            name: [{ value: '', disabled: true }, [Validators.required]],
            values: this._fb.array([])
        });
        return boxForm;
    }

    /*
     * Function Declare to Add Multiple Boxes
     * */
    addconsumableusageperiod() {
        (<FormArray>this.testForm.controls["ConsumablePeriodArray"]).push(
            this.initconsumableusageperiod()
        );
        // get box length for box name like box 1,box 2 in sidebar boxes combo list
        let connection_boxes_length = (<FormArray>(
            this.testForm.controls["ConsumablePeriodArray"]
        )).length;
    }

    /*
     * Operation for connection pack
     * This function also used for create Dynamic Packs
     * */
    initconsumableusageperiodvalue() {
        let packs: FormGroup = this._fb.group({

            id: 0,
            consumableId: 0,
            testId: 0,
            productId: 0,
            productName: [{ value: '', disabled: true }],
            productTypeId: 0,
            productTypeName: [""],
            instrumentId: 0,
            instrumentName: [""],
            usageRate: 1.0000000000,
            perTest: false,
            perPeriod: true,
            perInstrument: false,
            noOfTest: 0,
            period: [{ value: '', disabled: true }]

        });
        return packs;
    }

    /*
     * This Function add packs Dynamically
     * */
    addconsumableusageperiodvalue(boxIndex: number) {
        //declare to add packs
        (<FormArray>(
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ConsumablePeriodArray"]).controls[
                boxIndex
                ]
            )).controls["values"]
        )).push(this.initconsumableusageperiodvalue());
    }


    initconsumableusageins() {
        // Initialize add Box form field
        let boxForm: FormGroup = this._fb.group({
            name: [{ value: '', disabled: true }, [Validators.required]],
            values: this._fb.array([])
        });
        return boxForm;
    }

    /*
     * Function Declare to Add Multiple Boxes
     * */
    addconsumableusageins() {
        (<FormArray>this.testForm.controls["ConsumableInsArray"]).push(
            this.initconsumableusageins()
        );
        // get box length for box name like box 1,box 2 in sidebar boxes combo list
        let connection_boxes_length = (<FormArray>(
            this.testForm.controls["ConsumableInsArray"]
        )).length;
    }

    /*
     * Operation for connection pack
     * This function also used for create Dynamic Packs
     * */
    initconsumableusageinsvalue() {
        let packs: FormGroup = this._fb.group({

            id: 0,
            consumableId: 0,
            testId: 0,
            productId: 0,
            productName: [{ value: '', disabled: true }],
            productTypeId: 0,
            productTypeName: [""],
            instrumentId: 0,
            instrumentName: [""],
            usageRate: 1.0000000000,
            perTest: false,
            perPeriod: false,
            perInstrument: true,
            noOfTest: 0,
            period: [{ value: '', disabled: true }]

        });
        return packs;
    }

    /*
     * This Function add packs Dynamically
     * */
    addconsumableusageinsvalue(boxIndex: number) {
        //declare to add packs
        (<FormArray>(
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ConsumableInsArray"]).controls[
                boxIndex
                ]
            )).controls["values"]
        )).push(this.initconsumableusageinsvalue());
    }

    getTestingArea() {
        this._GlobalAPIService.getDataList('TestArea').subscribe((data) => {
            this.testingAreaList = data.aaData
            //console.log(this.Instrumentlist)
        }
        ), err => {
            console.log(err);
        }

    }
    getinstrumentlist(Ins: any) {
        this._APIwithActionService.getDatabyID(Ins, 'Instrument', 'getInsbyareaid').subscribe((data) => {
            this.instrumentList = data
            //console.log(this.Instrumentlist)
        }
        ), err => {
            console.log(err);
        }

    }
    getProductList(args, type: string = "") {

        this._APIwithActionService.getDatabyID(args.target.value, 'Product', 'GetAllProductByType').subscribe((data) => {
            this.productList = data
            // console.log(this.Instrumentlist)
        }
        ), err => {
            console.log(err);
        }
        if (type == "Test") {
            this.consumtesttypeID = args.target.value;
            this.consumtesttypename = args.target.options[args.target.selectedIndex].text;
        }
        else if (type == "period") {
            this.consumcontypeID = args.target.value;
            this.consumcontypename = args.target.options[args.target.selectedIndex].text;
        }
        else if (type == "Ins") {
            this.consuminstypeID = args.target.value;
            this.consuminstypename = args.target.options[args.target.selectedIndex].text;

        }
    }
    getProductType() {
        this._GlobalAPIService.getDataList('ProductType').subscribe((data) => {
            this.productTypeList = data.aaData
            // console.log(this.ProductTypeList)
        }
        ), err => {
            console.log(err);
        }

    }

    selectTestInstChange(args) {
        this.testinsID = args.target.value;
        this.testinsname = args.target.options[args.target.selectedIndex].text;

    }
    selectTestProductChange(args) {
        this.testproID = args.target.value;
        this.testproname = args.target.options[args.target.selectedIndex].text;
    }
    selectcontrolInschange(args) {
        this.controlinsID = args.target.value;
        this.controlinsname = args.target.options[args.target.selectedIndex].text;


    }
    selectconsumperiodproductchange(args) {
        this.consumconproID = args.target.value;
        this.consumconproname = args.target.options[args.target.selectedIndex].text;

    }
    selectconsuminschange(args) {
        this.consuminsID = args.target.value;
        this.consuminsname = args.target.options[args.target.selectedIndex].text;
    }
    selectconsuminsprochange(args) {
        this.consuminsproID = args.target.value;
        this.consuminsproname = args.target.options[args.target.selectedIndex].text;
    }
    selectconsuminsperiodchange(args) {
        this.consuminsperiodID = args.target.value;
        this.consuminsperiodname = args.target.options[args.target.selectedIndex].text;
    }

    updateExistingValue() {

        for (let boxIndex = 0; boxIndex < this.testprod.length; boxIndex++) {
            this.addtestprod();
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ProductUsageArray"]).controls[
                boxIndex
                ]
            )).patchValue({
                name: this.testprod[boxIndex].name
            });
            let values: Array<any> = this.testprod[boxIndex].value;

            for (let packIndex = 0; packIndex < values.length; packIndex++) {
                this.addvalue(boxIndex);
                (<FormGroup>(
                    (<FormArray>(
                        (<FormGroup>(
                            (<FormArray>this.testForm.controls["ProductUsageArray"])
                                .controls[boxIndex]
                        )).controls["values"]
                    )).controls[packIndex]
                )).patchValue({
                    id: values[packIndex].id,
                    productId: values[packIndex].productId,
                    productName: values[packIndex].productName,
                    instrumentId: values[packIndex].instrumentId,
                    instrumentName: values[packIndex].instrumentName,
                    rate: values[packIndex].rate,
                    isForControl: values[packIndex].isForControl,
                    testId: values[packIndex].testId

                });
            }
        }


        for (let boxIndex = 0; boxIndex < this.controlprd.length; boxIndex++) {
            this.addcontrolusage();
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ControlUsageArray"]).controls[
                boxIndex
                ]
            )).patchValue({
                name: this.controlprd[boxIndex].name
            });
            let values: Array<any> = this.controlprd[boxIndex].value;

            for (let packIndex = 0; packIndex < values.length; packIndex++) {
                this.addcontrolUsagevalue(boxIndex);
                (<FormGroup>(
                    (<FormArray>(
                        (<FormGroup>(
                            (<FormArray>this.testForm.controls["ControlUsageArray"])
                                .controls[boxIndex]
                        )).controls["values"]
                    )).controls[packIndex]
                )).patchValue({
                    id: values[packIndex].id,
                    productId: values[packIndex].productId,
                    productName: values[packIndex].productName,
                    instrumentId: values[packIndex].instrumentId,
                    instrumentName: values[packIndex].instrumentName,
                    rate: values[packIndex].rate,
                    isForControl: values[packIndex].isForControl,
                    testId: values[packIndex].testId

                });
            }



        }
        console.log(this.consumbletestprd);
        for (let boxIndex = 0; boxIndex < this.consumbletestprd.length; boxIndex++) {
            this.addconsumableusage();
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ConsumableTestArray"]).controls[
                boxIndex
                ]
            )).patchValue({
                name: this.consumbletestprd[boxIndex].name
            });
            let values: Array<any> = this.consumbletestprd[boxIndex].value;

            for (let packIndex = 0; packIndex < values.length; packIndex++) {
                this.addconsumableusagevalue(boxIndex);
                (<FormGroup>(
                    (<FormArray>(
                        (<FormGroup>(
                            (<FormArray>this.testForm.controls["ConsumableTestArray"])
                                .controls[boxIndex]
                        )).controls["values"]
                    )).controls[packIndex]
                )).patchValue({
                    id: values[packIndex].id,
                    consumableId: values[packIndex].consumableId,
                    testId: values[packIndex].testId,
                    productId: values[packIndex].productId,
                    productName: values[packIndex].productName,
                    productTypeId: values[packIndex].productTypeId,
                    productTypeName: values[packIndex].productTypeName,
                    instrumentId: values[packIndex].instrumentId,
                    instrumentName: values[packIndex].instrumentName,
                    usageRate: values[packIndex].usageRate,
                    perTest: values[packIndex].perTest,
                    perPeriod: values[packIndex].perPeriod,
                    perInstrument: values[packIndex].perInstrument,
                    noOfTest: values[packIndex].noOfTest,
                    period: values[packIndex].period


                });
            }



        }
        console.log(this.consumbleperiodprd);
        for (let boxIndex = 0; boxIndex < this.consumbleperiodprd.length; boxIndex++) {
            this.addconsumableusageperiod();
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ConsumablePeriodArray"]).controls[
                boxIndex
                ]
            )).patchValue({
                name: this.consumbleperiodprd[boxIndex].name
            });
            let values: Array<any> = this.consumbleperiodprd[boxIndex].value;

            for (let packIndex = 0; packIndex < values.length; packIndex++) {
                this.addconsumableusageperiodvalue(boxIndex);
                (<FormGroup>(
                    (<FormArray>(
                        (<FormGroup>(
                            (<FormArray>this.testForm.controls["ConsumablePeriodArray"])
                                .controls[boxIndex]
                        )).controls["values"]
                    )).controls[packIndex]
                )).patchValue({
                    id: values[packIndex].id,
                    consumableId: values[packIndex].consumableId,
                    testId: values[packIndex].testId,
                    productId: values[packIndex].productId,
                    productName: values[packIndex].productName,
                    productTypeId: values[packIndex].productTypeId,
                    productTypeName: values[packIndex].productTypeName,
                    instrumentId: values[packIndex].instrumentId,
                    instrumentName: values[packIndex].instrumentName,
                    usageRate: values[packIndex].usageRate,
                    perTest: values[packIndex].perTest,
                    perPeriod: values[packIndex].perPeriod,
                    perInstrument: values[packIndex].perInstrument,
                    noOfTest: values[packIndex].noOfTest,
                    period: values[packIndex].period


                });
            }



        }



        for (let boxIndex = 0; boxIndex < this.consumbleinstrumentprd.length; boxIndex++) {
            this.addconsumableusageins();
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ConsumableInsArray"]).controls[
                boxIndex
                ]
            )).patchValue({
                name: this.consumbleinstrumentprd[boxIndex].name
            });
            let values: Array<any> = this.consumbleinstrumentprd[boxIndex].value;

            for (let packIndex = 0; packIndex < values.length; packIndex++) {
                this.addconsumableusageinsvalue(boxIndex);
                (<FormGroup>(
                    (<FormArray>(
                        (<FormGroup>(
                            (<FormArray>this.testForm.controls["ConsumableInsArray"])
                                .controls[boxIndex]
                        )).controls["values"]
                    )).controls[packIndex]
                )).patchValue({
                    id: values[packIndex].id,
                    consumableId: values[packIndex].consumableId,
                    testId: values[packIndex].testId,
                    productId: values[packIndex].productId,
                    productName: values[packIndex].productName,
                    productTypeId: values[packIndex].productTypeId,
                    productTypeName: values[packIndex].productTypeName,
                    instrumentId: values[packIndex].instrumentId,
                    instrumentName: values[packIndex].instrumentName,
                    usageRate: values[packIndex].usageRate,
                    perTest: values[packIndex].perTest,
                    perPeriod: values[packIndex].perPeriod,
                    perInstrument: values[packIndex].perInstrument,
                    noOfTest: values[packIndex].noOfTest,
                    period: values[packIndex].period


                });
            }



        }
    }
    save() {


        //  console.log( this.testprod.concat(this.controlprd).values)


        if (!this.testForm.valid) {

            return;
        }

        console.log(this.testForm.value);
        let productusage = <FormArray>this.testForm.controls["ProductUsageArray"]

        let postproductusage = new Array();

        let consumproductusage = new Array();
        productusage.getRawValue().forEach(element => {
            element.values.forEach(x => {
                postproductusage.push(x)
            });
        });

        let controlusage = <FormArray>this.testForm.controls["ControlUsageArray"]

        controlusage.getRawValue().forEach(element => {
            element.values.forEach(x => {
                postproductusage.push(x)
            });
        });

        let consumpertestusage = <FormArray>this.testForm.controls["ConsumableTestArray"]
        consumpertestusage.getRawValue().forEach(element => {
            element.values.forEach(x => {
                consumproductusage.push(x)
            });
        });


        let consumperperiodusage = <FormArray>this.testForm.controls["ConsumablePeriodArray"]
        consumperperiodusage.getRawValue().forEach(element => {
            element.values.forEach(x => {
                consumproductusage.push(x)
            });
        });
        let consumperinsusage = <FormArray>this.testForm.controls["ConsumableInsArray"]
        consumperinsusage.getRawValue().forEach(element => {
            element.values.forEach(x => {
                consumproductusage.push(x)
            });
        });


        console.log(postproductusage);
        let newobject = new Object();
        //console.log(this.testForm.get('testId').value)

        newobject = {
            testID: this.testForm.value.testID,
            testName: this.testForm.value.testName,
            TestingAreaID: this.testForm.getRawValue().TestingAreaID,
            nooftest: this.testForm.value.nooftest,
            ProductUsageArray: postproductusage

        }
        let consumableobj = new Object();
        if (consumproductusage.length > 0) {
            consumableobj = {
                MasterCID: consumproductusage[0].consumableId,
                TestingAreaId: this.testForm.getRawValue().TestingAreaID,
                _consumablesUsages: consumproductusage,
                TestId: this.testForm.value.testID

            }
        }
        if (this.title == "Create") {
            this._APIwithActionService.postAPI(newobject, 'Test', 'Post01')
                .subscribe((data) => {
                    if (data["_body"] != 0) {
                        if (consumproductusage.length > 0) {
                            consumableobj["TestId"] = data["_body"]
                            this._APIwithActionService.postAPI(consumableobj, 'Test', 'postconsumable')
                                .subscribe((data) => {



                                    this._APIwithActionService.deleteData(this.Delidsconsumusage, 'Test', 'DelConsumableUsage').subscribe((data) => {
                                    })

                                })
                        }
                        this._APIwithActionService.deleteData(this.Delidsproductusage, 'Test', 'Delproductusage').subscribe((data) => {
                        })
                        this._GlobalAPIService.SuccessMessage("test Saved Successfully");
                        this._router.navigate(['/Managedata/ManagedataList', 2]);
                    }
                    else {
                        this._GlobalAPIService.FailureMessage("Test Name Already exist");
                    }


                }, error => this.errorMessage = error)
        }
        else if (this.title == "Edit") {

            this._APIwithActionService.putAPI(this.id, newobject, 'Test', 'Put01')
                .subscribe((data) => {
                    if (data["_body"] != 0) {

                        if (consumproductusage.length > 0) {
                            this._APIwithActionService.putAPI(consumproductusage[0].consumableId, consumableobj, 'Test', 'Put02')
                                .subscribe((data) => {

                                    this._APIwithActionService.deleteData(this.Delidsconsumusage, 'Test', 'DelConsumableUsage').subscribe((data) => {
                                    })

                                })
                        }
                        this._APIwithActionService.deleteData(this.Delidsproductusage, 'Test', 'Delproductusage').subscribe((data) => {
                        })

                        this._GlobalAPIService.SuccessMessage("Test Updated Successfully");
                        this._router.navigate(['/Managedata/ManagedataList', 2]);
                    }
                    else {
                        this._GlobalAPIService.FailureMessage("Test Name Already exist");
                    }

                }, error => this.errorMessage = error)
        }
    }

    clearctrl() {
        // this._router.navigate(['/Managedata/ManagedataList', 2]);
        this.close.emit(true);
    }
    addTestUsageRate() {

        if (this.testinsID > 0 && this.testproID > 0) {
            let index = this.testprod.indexOf(this.testprod.find(x => x.name == this.testinsname))
            if (index >= 0) {
                let index2 = this.testprod[index].value.indexOf(this.testprod[index].value.find(x => x.productName == this.testproname));
                if (index2 >= 0) { }
                else {

                    this.testprod[index].value.push({
                        id: 0, productId: this.testproID, productName: this.testproname,
                        instrumentId: this.testinsID, instrumentName: this.testinsname, rate: 1, productUsedIn: null, isForControl: false,
                        testId: this.testForm.controls["testID"].value
                    })
                    let packIndex = this.testprod[index].value.length - 1

                    let values = this.testprod[index].value
                    this.addvalue(index);
                    (<FormGroup>(
                        (<FormArray>(
                            (<FormGroup>(
                                (<FormArray>this.testForm.controls["ProductUsageArray"])
                                    .controls[index]
                            )).controls["values"]
                        )).controls[packIndex]
                    )).patchValue({
                        id: values[packIndex].id,
                        productId: values[packIndex].productId,
                        productName: values[packIndex].productName,
                        instrumentId: values[packIndex].instrumentId,
                        instrumentName: values[packIndex].instrumentName,
                        rate: values[packIndex].rate,
                        isForControl: values[packIndex].isForControl,
                        testId: values[packIndex].testId

                    });
                }
            }

            else {
                this.testprod.push({
                    name: this.testinsname,
                    value: [{
                        id: 0, productId: this.testproID, productName: this.testproname,
                        instrumentId: this.testinsID, instrumentName: this.testinsname, rate: 1, productUsedIn: null, isForControl: false,
                        testId: this.testForm.controls["testID"].value
                    }]

                })
                let boxIndex = this.testprod.length - 1
                this.addtestprod();
                (<FormGroup>(
                    (<FormArray>this.testForm.controls["ProductUsageArray"]).controls[
                    boxIndex
                    ]
                )).patchValue({
                    name: this.testprod[boxIndex].name
                });
                let packIndex = this.testprod[boxIndex].value.length - 1

                let values = this.testprod[boxIndex].value
                this.addvalue(boxIndex);
                (<FormGroup>(
                    (<FormArray>(
                        (<FormGroup>(
                            (<FormArray>this.testForm.controls["ProductUsageArray"])
                                .controls[boxIndex]
                        )).controls["values"]
                    )).controls[packIndex]
                )).patchValue({
                    id: values[packIndex].id,
                    productId: values[packIndex].productId,
                    productName: values[packIndex].productName,
                    instrumentId: values[packIndex].instrumentId,
                    instrumentName: values[packIndex].instrumentName,
                    rate: values[packIndex].rate,
                    isForControl: values[packIndex].isForControl,
                    testId: values[packIndex].testId

                });
            }

        }

    }
    AddControlUsageRate() {
        if (this.controlproID > 0 && this.controlinsID > 0) {
            let index = this.controlprd.indexOf(this.controlprd.find(x => x.name == this.controlinsname))
            if (index >= 0) {
                let index2 = this.controlprd[index].value.indexOf(this.controlprd[index].value.find(x => x.productName == this.controlproname));
                if (index2 >= 0) { }
                else {

                    this.controlprd[index].value.push({
                        id: 0, productId: this.controlproID, productName: this.controlproname,
                        instrumentId: this.controlinsID, instrumentName: this.controlinsname, rate: 1, productUsedIn: null, isForControl: true,
                        testId: this.testForm.controls["testID"].value
                    })

                    let packIndex = this.controlprd[index].value.length - 1

                    let values = this.controlprd[index].value
                    this.addcontrolUsagevalue(index);
                    (<FormGroup>(
                        (<FormArray>(
                            (<FormGroup>(
                                (<FormArray>this.testForm.controls["ControlUsageArray"])
                                    .controls[index]
                            )).controls["values"]
                        )).controls[packIndex]
                    )).patchValue({
                        id: values[packIndex].id,
                        productId: values[packIndex].productId,
                        productName: values[packIndex].productName,
                        instrumentId: values[packIndex].instrumentId,
                        instrumentName: values[packIndex].instrumentName,
                        rate: values[packIndex].rate,
                        isForControl: values[packIndex].isForControl,
                        testId: values[packIndex].testId

                    });
                }
            }
            else {
                this.controlprd.push({
                    name: this.controlinsname,
                    value: [{
                        id: 0, productId: this.controlproID, productName: this.controlproname,
                        instrumentId: this.controlinsID, instrumentName: this.controlinsname, rate: 1, productUsedIn: null, isForControl: true,
                        testId: this.testForm.controls["testID"].value
                    }]

                })

                let boxIndex = this.controlprd.length - 1
                this.addcontrolusage();
                (<FormGroup>(
                    (<FormArray>this.testForm.controls["ControlUsageArray"]).controls[
                    boxIndex
                    ]
                )).patchValue({
                    name: this.controlprd[boxIndex].name
                });
                let packIndex = this.controlprd[boxIndex].value.length - 1

                let values = this.controlprd[boxIndex].value
                this.addcontrolUsagevalue(boxIndex);
                (<FormGroup>(
                    (<FormArray>(
                        (<FormGroup>(
                            (<FormArray>this.testForm.controls["ControlUsageArray"])
                                .controls[boxIndex]
                        )).controls["values"]
                    )).controls[packIndex]
                )).patchValue({
                    id: values[packIndex].id,
                    productId: values[packIndex].productId,
                    productName: values[packIndex].productName,
                    instrumentId: values[packIndex].instrumentId,
                    instrumentName: values[packIndex].instrumentName,
                    rate: values[packIndex].rate,
                    isForControl: values[packIndex].isForControl,
                    testId: values[packIndex].testId

                });
            }
        }
    }
    AddconsumtestUsageRate() {
        if (this.testForm.controls["nooftest"].value != "" && this.consumtestproID > 0) {
            let index = this.consumbletestprd.indexOf(this.consumbletestprd.find(x => x.name == this.consumtesttypename))
            if (index >= 0) {
                let index2 = this.consumbletestprd[index].value.indexOf(this.consumbletestprd[index].value.find(x => x.productName == this.consumtestproname));
                if (index2 >= 0) { }
                else {

                    this.consumbletestprd[index].value.push({
                        id: 0,
                        testId: this.testForm.controls["testID"].value, productId: this.consumtestproID, productName: this.consumtestproname, productTypeId: this.consumtesttypeID, productTypeName: this.consumtesttypename,
                        instrumentId: null, instrumentName: null, usageRate: 1.0000000000, perTest: true, perPeriod: false, perInstrument: false, noOfTest: this.testForm.controls["nooftest"].value, period: null
                    })

                    let packIndex = this.consumbletestprd[index].value.length - 1

                    let values = this.consumbletestprd[index].value
                    this.addconsumableusagevalue(index);
                    (<FormGroup>(
                        (<FormArray>(
                            (<FormGroup>(
                                (<FormArray>this.testForm.controls["ConsumableTestArray"])
                                    .controls[index]
                            )).controls["values"]
                        )).controls[packIndex]
                    )).patchValue({

                        id: values[packIndex].id,
                        consumableId: values[packIndex].consumableId,
                        testId: values[packIndex].testId,
                        productId: values[packIndex].productId,
                        productName: values[packIndex].productName,
                        productTypeId: values[packIndex].productTypeId,
                        productTypeName: values[packIndex].productTypeName,
                        instrumentId: values[packIndex].instrumentId,
                        instrumentName: values[packIndex].instrumentName,
                        usageRate: values[packIndex].usageRate,
                        perTest: values[packIndex].perTest,
                        perPeriod: values[packIndex].perPeriod,
                        perInstrument: values[packIndex].perInstrument,
                        noOfTest: values[packIndex].noOfTest,
                        period: values[packIndex].period,

                    });
                }
            }
            else {
                this.consumbletestprd.push({
                    name: this.consumtesttypename,
                    value: [{
                        id: 0,
                        testId: this.testForm.controls["testID"].value, productId: this.consumtestproID, productName: this.consumtestproname, productTypeId: this.consumtesttypeID, productTypeName: this.consumtesttypename,
                        instrumentId: null, instrumentName: null, usageRate: 1.0000000000, perTest: true, perPeriod: false, perInstrument: false, noOfTest: this.testForm.controls["nooftest"].value, period: null

                    }]

                })


                let boxIndex = this.consumbletestprd.length - 1
                this.addconsumableusage();
                (<FormGroup>(
                    (<FormArray>this.testForm.controls["ConsumableTestArray"]).controls[
                    boxIndex
                    ]
                )).patchValue({
                    name: this.consumbletestprd[boxIndex].name
                });
                let packIndex = this.consumbletestprd[boxIndex].value.length - 1

                let values = this.consumbletestprd[boxIndex].value
                this.addconsumableusagevalue(boxIndex);
                (<FormGroup>(
                    (<FormArray>(
                        (<FormGroup>(
                            (<FormArray>this.testForm.controls["ConsumableTestArray"])
                                .controls[boxIndex]
                        )).controls["values"]
                    )).controls[packIndex]
                )).patchValue({

                    id: values[packIndex].id,
                    consumableId: values[packIndex].consumableId,
                    testId: values[packIndex].testId,
                    productId: values[packIndex].productId,
                    productName: values[packIndex].productName,
                    productTypeId: values[packIndex].productTypeId,
                    productTypeName: values[packIndex].productTypeName,
                    instrumentId: values[packIndex].instrumentId,
                    instrumentName: values[packIndex].instrumentName,
                    usageRate: values[packIndex].usageRate,
                    perTest: values[packIndex].perTest,
                    perPeriod: values[packIndex].perPeriod,
                    perInstrument: values[packIndex].perInstrument,
                    noOfTest: values[packIndex].noOfTest,
                    period: values[packIndex].period,

                });
            }
        }

    }
    AddconsumperiodUsageRate() {
        if (this.consumconproID > 0 && this.consumconperiodID > 0) {
            let index = this.consumbleperiodprd.indexOf(this.consumbleperiodprd.find(x => x.name == this.consumcontypename))
            if (index >= 0) {
                let index2 = this.consumbleperiodprd[index].value.indexOf(this.consumbleperiodprd[index].value.find(x => x.productName == this.consumconproname));
                if (index2 >= 0) { }
                else {

                    this.consumbleperiodprd[index].value.push({
                        id: 0,
                        testId: this.testForm.controls["testID"].value, productId: this.consumconproID, productName: this.consumconproname, productTypeId: this.consumcontypeID, productTypeName: this.consumcontypename,
                        instrumentId: null, instrumentName: null, usageRate: 1.0000000000, perTest: false, perPeriod: true, perInstrument: false, noOfTest: 0, period: this.consumconperiod
                    })
                    let packIndex = this.consumbleperiodprd[index].value.length - 1

                    let values = this.consumbleperiodprd[index].value
                    this.addconsumableusageperiodvalue(index);
                    (<FormGroup>(
                        (<FormArray>(
                            (<FormGroup>(
                                (<FormArray>this.testForm.controls["ConsumablePeriodArray"])
                                    .controls[index]
                            )).controls["values"]
                        )).controls[packIndex]
                    )).patchValue({

                        id: values[packIndex].id,
                        consumableId: values[packIndex].consumableId,
                        testId: values[packIndex].testId,
                        productId: values[packIndex].productId,
                        productName: values[packIndex].productName,
                        productTypeId: values[packIndex].productTypeId,
                        productTypeName: values[packIndex].productTypeName,
                        instrumentId: values[packIndex].instrumentId,
                        instrumentName: values[packIndex].instrumentName,
                        usageRate: values[packIndex].usageRate,
                        perTest: values[packIndex].perTest,
                        perPeriod: values[packIndex].perPeriod,
                        perInstrument: values[packIndex].perInstrument,
                        noOfTest: values[packIndex].noOfTest,
                        period: values[packIndex].period,

                    });
                }
            }
            else {
                this.consumbleperiodprd.push({
                    name: this.consumcontypename,
                    value: [{
                        id: 0,
                        testId: this.testForm.controls["testID"].value, productId: this.consumconproID, productName: this.consumconproname, productTypeId: this.consumcontypeID, productTypeName: this.consumcontypename,
                        instrumentId: null, instrumentName: null, usageRate: 1.0000000000, perTest: false, perPeriod: true, perInstrument: false, noOfTest: 0, period: this.consumconperiod
                    }]
                })

                let boxIndex = this.consumbleperiodprd.length - 1
                this.addconsumableusageperiod();
                (<FormGroup>(
                    (<FormArray>this.testForm.controls["ConsumablePeriodArray"]).controls[
                    boxIndex
                    ]
                )).patchValue({
                    name: this.consumbleperiodprd[boxIndex].name
                });
                let packIndex = this.consumbleperiodprd[boxIndex].value.length - 1

                let values = this.consumbleperiodprd[boxIndex].value
                this.addconsumableusageperiodvalue(boxIndex);
                (<FormGroup>(
                    (<FormArray>(
                        (<FormGroup>(
                            (<FormArray>this.testForm.controls["ConsumablePeriodArray"])
                                .controls[boxIndex]
                        )).controls["values"]
                    )).controls[packIndex]
                )).patchValue({

                    id: values[packIndex].id,
                    consumableId: values[packIndex].consumableId,
                    testId: values[packIndex].testId,
                    productId: values[packIndex].productId,
                    productName: values[packIndex].productName,
                    productTypeId: values[packIndex].productTypeId,
                    productTypeName: values[packIndex].productTypeName,
                    instrumentId: values[packIndex].instrumentId,
                    instrumentName: values[packIndex].instrumentName,
                    usageRate: values[packIndex].usageRate,
                    perTest: values[packIndex].perTest,
                    perPeriod: values[packIndex].perPeriod,
                    perInstrument: values[packIndex].perInstrument,
                    noOfTest: values[packIndex].noOfTest,
                    period: values[packIndex].period,

                });
            }
        }
    }
    AddconsuminsUsageRate() {
        if (this.consuminsproID > 0 && this.consuminsperiodID > 0) {
            let index = this.consumbleinstrumentprd.indexOf(this.consumbleinstrumentprd.find(x => x.name == this.consuminsname))
            if (index >= 0) {
                let index2 = this.consumbleinstrumentprd[index].value.indexOf(this.consumbleinstrumentprd[index].value.find(x => x.productName == this.consuminsproname));
                if (index2 >= 0) { }
                else {

                    this.consumbleinstrumentprd[index].value.push({
                        id: 0,
                        testId: this.testForm.controls["testID"].value, productId: this.consuminsproID, productName: this.consuminsproname, productTypeId: 0, productTypeName: null,
                        instrumentId: this.consuminsID, instrumentName: this.consuminsname, usageRate: 1.0000000000, perTest: false, perPeriod: false, perInstrument: true, noOfTest: 0, period: this.consuminsperiodname
                    })


                    let packIndex = this.consumbleinstrumentprd[index].value.length - 1

                    let values = this.consumbleinstrumentprd[index].value
                    this.addconsumableusageinsvalue(index);
                    (<FormGroup>(
                        (<FormArray>(
                            (<FormGroup>(
                                (<FormArray>this.testForm.controls["ConsumableInsArray"])
                                    .controls[index]
                            )).controls["values"]
                        )).controls[packIndex]
                    )).patchValue({

                        id: values[packIndex].id,
                        consumableId: values[packIndex].consumableId,
                        testId: values[packIndex].testId,
                        productId: values[packIndex].productId,
                        productName: values[packIndex].productName,
                        productTypeId: values[packIndex].productTypeId,
                        productTypeName: values[packIndex].productTypeName,
                        instrumentId: values[packIndex].instrumentId,
                        instrumentName: values[packIndex].instrumentName,
                        usageRate: values[packIndex].usageRate,
                        perTest: values[packIndex].perTest,
                        perPeriod: values[packIndex].perPeriod,
                        perInstrument: values[packIndex].perInstrument,
                        noOfTest: values[packIndex].noOfTest,
                        period: values[packIndex].period,

                    });
                }
            }
            else {
                this.consumbleinstrumentprd.push({
                    name: this.consuminsname,
                    value: [{
                        id: 0,
                        testId: this.testForm.controls["testID"].value, productId: this.consuminsproID, productName: this.consuminsproname, productTypeId: 0, productTypeName: null,
                        instrumentId: this.consuminsID, instrumentName: this.consuminsname, usageRate: 1.0000000000, perTest: false, perPeriod: false, perInstrument: true, noOfTest: 0, period: this.consuminsperiodname
                    }]

                })


                let boxIndex = this.consumbleinstrumentprd.length - 1
                this.addconsumableusageins();
                (<FormGroup>(
                    (<FormArray>this.testForm.controls["ConsumableInsArray"]).controls[
                    boxIndex
                    ]
                )).patchValue({
                    name: this.consumbleinstrumentprd[boxIndex].name
                });
                let packIndex = this.consumbleinstrumentprd[boxIndex].value.length - 1

                let values = this.consumbleinstrumentprd[boxIndex].value
                this.addconsumableusageinsvalue(boxIndex);
                (<FormGroup>(
                    (<FormArray>(
                        (<FormGroup>(
                            (<FormArray>this.testForm.controls["ConsumableInsArray"])
                                .controls[boxIndex]
                        )).controls["values"]
                    )).controls[packIndex]
                )).patchValue({

                    id: values[packIndex].id,
                    consumableId: values[packIndex].consumableId,
                    testId: values[packIndex].testId,
                    productId: values[packIndex].productId,
                    productName: values[packIndex].productName,
                    productTypeId: values[packIndex].productTypeId,
                    productTypeName: values[packIndex].productTypeName,
                    instrumentId: values[packIndex].instrumentId,
                    instrumentName: values[packIndex].instrumentName,
                    usageRate: values[packIndex].usageRate,
                    perTest: values[packIndex].perTest,
                    perPeriod: values[packIndex].perPeriod,
                    perInstrument: values[packIndex].perInstrument,
                    noOfTest: values[packIndex].noOfTest,
                    period: values[packIndex].period,

                });
            }
        }
    }

    deltest(boxIndex, packIndex) {
        let delid: String
        delid = (<FormGroup>(
            (<FormArray>(
                (<FormGroup>(
                    (<FormArray>this.testForm.controls["ProductUsageArray"])
                        .controls[boxIndex]
                )).controls["values"]
            )).controls[packIndex]
        )).controls["id"].value
        this.Delidsproductusage = this.Delidsproductusage + "," + delid;

        (<FormArray>(
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ProductUsageArray"])
                    .controls[boxIndex]
            )).controls["values"]
        )).removeAt(packIndex)

        console.log(this.Delidsproductusage)
    }
    delcontrol(boxIndex, packIndex) {
        let delid: String
        delid = (<FormGroup>(
            (<FormArray>(
                (<FormGroup>(
                    (<FormArray>this.testForm.controls["ControlUsageArray"])
                        .controls[boxIndex]
                )).controls["values"]
            )).controls[packIndex]
        )).controls["id"].value
        this.Delidsproductusage = this.Delidsproductusage + "," + delid;

        (<FormArray>(
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ControlUsageArray"])
                    .controls[boxIndex]
            )).controls["values"]
        )).removeAt(packIndex)

        console.log(this.Delidscontrolusage)
    }

    delpertestcon(boxIndex, packIndex) {
        let delid: String
        delid = (<FormGroup>(
            (<FormArray>(
                (<FormGroup>(
                    (<FormArray>this.testForm.controls["ConsumableTestArray"])
                        .controls[boxIndex]
                )).controls["values"]
            )).controls[packIndex]
        )).controls["id"].value
        this.Delidsconsumusage = this.Delidsconsumusage + "," + delid;

        (<FormArray>(
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ConsumableTestArray"])
                    .controls[boxIndex]
            )).controls["values"]
        )).removeAt(packIndex)
        console.log(this.Delidsconsumusage)
    }
    delperperiodcon(boxIndex, packIndex) {
        let delid: String
        delid = (<FormGroup>(
            (<FormArray>(
                (<FormGroup>(
                    (<FormArray>this.testForm.controls["ConsumablePeriodArray"])
                        .controls[boxIndex]
                )).controls["values"]
            )).controls[packIndex]
        )).controls["id"].value
        this.Delidsconsumusage = this.Delidsconsumusage + "," + delid;

        (<FormArray>(
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ConsumablePeriodArray"])
                    .controls[boxIndex]
            )).controls["values"]
        )).removeAt(packIndex)
        console.log(this.Delidsconsumusage)
    }

    delperinscon(boxIndex, packIndex) {
        let delid: String
        delid = (<FormGroup>(
            (<FormArray>(
                (<FormGroup>(
                    (<FormArray>this.testForm.controls["ConsumableInsArray"])
                        .controls[boxIndex]
                )).controls["values"]
            )).controls[packIndex]
        )).controls["id"].value
        this.Delidsconsumusage = this.Delidsconsumusage + "," + delid;

        (<FormArray>(
            (<FormGroup>(
                (<FormArray>this.testForm.controls["ConsumableInsArray"])
                    .controls[boxIndex]
            )).controls["values"]
        )).removeAt(packIndex)
        console.log(this.Delidsconsumusage)
    }

    handleSelectTestArea(index) {
        this.currentTestArea = index;
    }

    handleControlPerPeriod(index) {
        this.currentPerPeriod = index;
    }

    handleControlPerInstr(index) {
        this.currentPerInstr = index;
    }

}

