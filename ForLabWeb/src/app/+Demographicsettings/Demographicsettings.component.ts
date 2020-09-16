import { Component, OnInit, Renderer, Pipe, PipeTransform, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import { ModalDirective } from "ngx-bootstrap";
import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
import { postforecastparameter } from "../shared/GlobalInterface"
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { APIwithActionService } from "../shared/APIwithAction.service";
import { Router, ActivatedRoute } from '@angular/router';
import { NotificationService } from "../shared/utils/notification.service";
import { variable } from '@angular/compiler/src/output/output_ast';
import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';

const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';
@Component({
    selector: 'sa-demosettings',
    templateUrl: './Demographicsettings.component.html',
    styleUrls: ['./Demographicsettings.css']
})



export class Demographicsettingcomponent implements OnInit {
    id: number;
    programname: string;
    nextstatus = false;
    backstatus = true;
    methodstatus = false;
    demosettingform: FormGroup;
    ProgramList = new Array();
    CurrentTab: number = 0;
    selectedprogramid: number = 0;
    selectedyears: number;
    MMForecastParameterList = new Array();
    patientgrouplist = new Array();
    generalassumptionlist = new Array();
    patientassumptionlist = new Array();
    Testingassumptionlist = new Array();
    Productassumptionlist = new Array();
    MMforcastparameter = new Array();
    patientassumption = new Array();
    testingassumption = new Array();
    productassumption = new Array();
    patientgroup = new Array();
    parametername = new Array();
    parameterobject = new Object();
    Defineforlist = new Array();
    Variablelist = new Array();
    assumptionsobject = new Object();
    defineID: number;
    definevariablename: number;
    suggustionlist = new Array();
    filtersuggustionlist = new Array();
    suggestedvariable = new Object();
    @ViewChild('defineprogram1') defineprogram1: ElementRef;
    @ViewChild('Method1') Method1: ElementRef;
    @ViewChild('patientgroup1') patientgroup1: ElementRef;
    @ViewChild('patientassumption1') patientassumption1: ElementRef;
    @ViewChild('testingAssumption1') testingAssumption1: ElementRef;
    @ViewChild('productassumption1') productassumption1: ElementRef;
    @ViewChild('testingprotocol1') testingprotocol1: ElementRef;
    @ViewChild('Review1') Review1: ElementRef;
    @ViewChild('formulatext') formulatext1: ElementRef;
    @ViewChild('lgModal') public lgModal: ModalDirective;
    @ViewChild('lgModal1') public lgModal1: ModalDirective;

    @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
    @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
    public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;
    checkstatus:boolean;
    public primaryColour = PrimaryRed;
    public secondaryColour = SecondaryBlue;
    public coloursEnabled = false;
    public loadingTemplate: TemplateRef<any>;
    formData = new FormData();
    public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };
    public loading = false;

    constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute, private notificationService: NotificationService,
        private _GlobalAPIService: GlobalAPIService, private _router: Router, private _APIwithActionService: APIwithActionService, private _rd: Renderer, private el: ElementRef) {
        if (this._avRoute.snapshot.params["id"]) {
            this.id = this._avRoute.snapshot.params["id"];
        }
        this.getprogramlist();
        this.getprogramparameter();
        this.getpatientgroup();
        this.getsuggustionlist();

        console.log(this.filtersuggustionlist)

    }

    getsuggustionlist() {
        this.loading=true;
        this._APIwithActionService.getDataList("MMProgram", "Getsuggustionlist").subscribe((data) => {
            this.suggustionlist = data;
            this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Program')
            this.loading=false;
        })
    }
    public steps = [
        {
            key: 'step1',
            title: 'Define Program',
            valid: false,
            checked: false,
            submitted: false,
        },
        {
            key: 'step2',
            title: 'Forecasting Method',
            valid: false,
            checked: false,
            submitted: false,
        },
        {
            key: 'step3',
            title: 'Patient/Population Group',
            valid: true,
            checked: false,
            submitted: false,
        },
        {
            key: 'step4',
            title: 'Patient Assumption',
            valid: true,
            checked: false,
            submitted: false,
        },
        {
            key: 'step5',
            title: 'Testing Asssumption',
            valid: true,
            checked: false,
            submitted: false,
        },
        {
            key: 'step6',
            title: 'Product Assumption',
            valid: true,
            checked: false,
            submitted: false,
        },
        {
            key: 'step7',
            title: 'Testing Protocol',
            valid: true,
            checked: false,
            submitted: false,
        },
        {
            key: 'step8',
            title: 'Review',
            valid: true,
            checked: false,
            submitted: false,
        },
    ];

    public activeStep = this.steps[0];

    setActiveStep(steo) {
        if (steo.key == "step1") {
            this.defineprogramclick()
        }
        else if (steo.key == "step2") {
            this.Methodclick()
        }
        else if (steo.key == "step3") {
            this.patientgroupclick()
        }
        else if (steo.key == "step4") {
            this.patientassumptionclick()
        } else if (steo.key == "step5") {
            this.testingAssumptionclick()
        }
        else if (steo.key == "step6") {
            this.productassumptionclick()
        }

        console.log(steo)
        this.activeStep = steo
    }
    smartModEg2(item: any) {
        let index: number;
        let groupoject = new Object();
        this.notificationService.smartMessageBox({
            title: "Operation",
            content: "For Activation/Deactivation " + item.groupName + " Press Yes For no operation Press No",
            buttons: '[No][Yes]'
        }, (ButtonPressed) => {
            if (ButtonPressed === "Yes") {

                index = this.patientgroup.findIndex(x => x.id === item.id);
                if (this.patientgroup[index].isActive == "Yes") {
                    this.patientgroup[index].isActive = "No"
                }
                else {
                    this.patientgroup[index].isActive = "Yes"
                }
                groupoject = {
                    Id: item.id,
                    GroupName: item.groupName,
                    ProgramId: item.programId,
                    IsActive: this.patientgroup[index].isActive
                }
                this._APIwithActionService.putAPI(item.id, groupoject, 'MMProgram', 'updategroup')
                    .subscribe((data) => {

                    })
            }
            if (ButtonPressed === "No") {

            }

        });

    }

    smartModEg1(item: any, Type: string) {
        let variablename: string;
        let index: number;
        let groupoject = new Object();
        let parameterobject = new Object();
        if (Type == "Group") {
            variablename = item.groupName + " Group";
        }
        else {
            variablename = item.variableName + " Variable";
        }
        this.notificationService.smartMessageBox({
            title: "Operation",
            content: "For Activation/Deactivation " + variablename + " Press Yes For no operation Press No",
            buttons: '[No][Yes]'
        }, (ButtonPressed) => {
            if (ButtonPressed === "Yes") {
                if (Type === "Patient") {
                    index = this.patientassumption.findIndex(x => x.id === item.id);
                    if (this.patientassumption[index].isActive == "Yes") {
                        this.patientassumption[index].isActive = "No"
                    }
                    else {
                        this.patientassumption[index].isActive = "Yes"
                    }
                    parameterobject = {
                        Id: item.id,
                        VariableName: item.variableName,
                        VariableDataType: item.variableDataType,
                        VariableDataTypeName: item.variableDataTypeName,
                        UseOn: item.useOn,
                        VariableFormula: item.variableFormula,
                        ProgramId: item.programId,
                        VarCode: item.varCode,
                        AssumptionType: item.assumptionType,
                        AssumptionTypename: item.assumptionTypename,
                        VariableEffect: item.variableEffect,
                        IsActive: item.isActive
                    }
                    this._APIwithActionService.putAPI(item.id, parameterobject, 'MMProgram', 'updategeneralassumptions').subscribe((data) => {

                    })
                }
                else if (Type === "Test") {
                    index = this.testingassumption.findIndex(x => x.id === item.id);
                    if (this.testingassumption[index].isActive == "Yes") {
                        this.testingassumption[index].isActive = "No"
                    }
                    else {
                        this.testingassumption[index].isActive = "Yes"
                    }
                    parameterobject = {
                        Id: item.id,
                        VariableName: item.variableName,
                        VariableDataType: item.variableDataType,
                        VariableDataTypeName: item.variableDataTypeName,
                        UseOn: item.useOn,
                        VariableFormula: item.variableFormula,
                        ProgramId: item.programId,
                        VarCode: item.varCode,
                        AssumptionType: item.assumptionType,
                        AssumptionTypename: item.assumptionTypename,
                        VariableEffect: item.variableEffect,
                        IsActive: item.isActive
                    }
                    this._APIwithActionService.putAPI(item.id, parameterobject, 'MMProgram', 'updategeneralassumptions').subscribe((data) => {

                    })
                }
                else {
                    index = this.productassumption.findIndex(x => x.id === item.id);
                    if (this.productassumption[index].isActive == "Yes") {
                        this.productassumption[index].isActive = "No"
                    }
                    else {
                        this.productassumption[index].isActive = "Yes"
                    }
                    parameterobject = {
                        Id: item.id,
                        VariableName: item.variableName,
                        VariableDataType: item.variableDataType,
                        VariableDataTypeName: item.variableDataTypeName,
                        UseOn: item.useOn,
                        VariableFormula: item.variableFormula,
                        ProgramId: item.programId,
                        VarCode: item.varCode,
                        AssumptionType: item.assumptionType,
                        AssumptionTypename: item.assumptionTypename,
                        VariableEffect: item.variableEffect,
                        IsActive: item.isActive
                    }
                    this._APIwithActionService.putAPI(item.id, parameterobject, 'MMProgram', 'updategeneralassumptions').subscribe((data) => {

                    })
                }



            }
            if (ButtonPressed === "No") {

            }

        });
    }
    // Settingdisplayproperty(displaytype: string) {
    //     let methodel: HTMLElement = this.Method1.nativeElement as HTMLElement;
    //     let patientgroupel: HTMLElement = this.patientgroup1.nativeElement as HTMLElement;
    //     let patientassumptionel: HTMLElement = this.patientassumption1.nativeElement as HTMLElement;
    //     let testingAssumptionel: HTMLElement = this.testingAssumption1.nativeElement as HTMLElement;
    //     let productassumptionel: HTMLElement = this.productassumption1.nativeElement as HTMLElement;
    //     let testingprotocolel: HTMLElement = this.testingprotocol1.nativeElement as HTMLElement;
    //     let Reviewel: HTMLElement = this.Review1.nativeElement as HTMLElement;


    //     this._rd.setElementStyle(methodel, "display", displaytype);
    //     this._rd.setElementStyle(patientgroupel, "display", displaytype);
    //     this._rd.setElementStyle(patientassumptionel, "display", displaytype);
    //     this._rd.setElementStyle(testingAssumptionel, "display", displaytype);
    //     this._rd.setElementStyle(productassumptionel, "display", displaytype);
    //     this._rd.setElementStyle(testingprotocolel, "display", displaytype);
    //     this._rd.setElementStyle(Reviewel, "display", displaytype);
    // }
    ngOnInit() {
        this.demosettingform = this._fb.group({
            id: 0,
            Programname: ['', Validators.compose([Validators.required, Validators.maxLength(64)])],
            groupName: ['', Validators.compose([Validators.required, Validators.maxLength(200)])],
            Isactivegroup: [true],
            variablenamepatient: ['', Validators.compose([Validators.required, Validators.maxLength(200)])],
            variabletypepatient: ['1', [Validators.required]],
            patienteffect: ['Positive'],
            Isactivepatient: [true],
            variablenametesting: ['', Validators.compose([Validators.required, Validators.maxLength(200)])],
            variabletypetesting: ['1', [Validators.required]],
            testingeffect: ['Positive'],
            Isactivetesting: [true],
            variablenameproduct: ['', Validators.compose([Validators.required, Validators.maxLength(200)])],
            variabletypeproduct: ['1', [Validators.required]],
            producteffect: ['Positive'],
            Isactiveproduct: [true],
            Years: ['', [Validators.required]]
        })
        // this.Settingdisplayproperty("none");
    }
    getpatientgroup() {
        this._APIwithActionService.getDataList('MMProgram', 'Getpatientgroup').subscribe((resp) => {
            this.patientgrouplist = resp;
            
            if (this.selectedprogramid != 0) {
                this.patientgroup = this.patientgrouplist.filter(x => x.programId === this.selectedprogramid)
            }
        })
    }
    Addoperators(operatertype: string) {
        let el: HTMLElement = this.formulatext1.nativeElement as HTMLElement;
        el.textContent = el.textContent + operatertype;
    }
    addvarcodeinformula(variablecode: string) {
        let el: HTMLElement = this.formulatext1.nativeElement as HTMLElement;
        el.textContent = el.textContent + variablecode;
    }
    getdefinevariable(args) {
        this.defineID = args.target.value;
        this.definevariablename = args.target.options[args.target.selectedIndex].text;
    }
    saveformula() {
        let el: HTMLElement = this.formulatext1.nativeElement as HTMLElement;
        let index = this.Variablelist.indexOf(this.Variablelist.find(x => x.id == this.defineID))
        if (index >= 0) {
            this.Variablelist[index].variableFormula = el.textContent;
        }
        this.assumptionsobject = {
            Id: this.Variablelist[index].id,
            VariableName: this.Variablelist[index].variableName,
            VariableDataType: this.Variablelist[index].variableDataType,
            VariableDataTypeName: this.Variablelist[index].variableDataTypeName,
            UseOn: this.Variablelist[index].useOn,
            VariableFormula: this.Variablelist[index].variableFormula,
            ProgramId: this.Variablelist[index].programId,
            VarCode: this.Variablelist[index].varCode,
            AssumptionType: this.Variablelist[index].assumptionType,
            AssumptionTypename: this.Variablelist[index].assumptionTypename,
            VariableEffect: this.Variablelist[index].variableEffect,
            IsActive: this.Variablelist[index].isActive
        }
        this._APIwithActionService.putAPI(this.Variablelist[index].id, this.assumptionsobject, 'MMProgram', 'updategeneralassumptions').subscribe(
            (data) => {

            }
        )
    }
    createformula() {
        if (this.CurrentTab == 3) {
            this.Defineforlist = this.patientassumption.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 1);
            this.Variablelist = this.patientassumption.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 1);
        }
        else if (this.CurrentTab == 4) {
            this.Defineforlist = this.testingassumption.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 3);
            this.Variablelist = this.testingassumption.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 3);
        }
        else {
            this.Defineforlist = this.productassumption.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 2);
            this.Variablelist = this.productassumption.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 2);
        }
        this.lgModal.show();
    }
    addAssumption(Assumtiontype:number,variablename:string="") {

        let postassumption = new Object();
        if (Assumtiontype == 1) {
            if (variablename !="")
            {
                postassumption =
                {
                    Id: 0,
                    VariableName: variablename,
                    VariableDataType: this.demosettingform.controls["variabletypepatient"].value,
                    VariableDataTypeName: this.demosettingform.controls["variabletypepatient"].value == 1 ? "Numeric" : "Percentage",
                    UseOn: "OnAllSite",
                    ProgramId: this.selectedprogramid,
                    VarCode: "xx",
                    AssumptionType: 1,
                    AssumptionTypename: "Patient_Number_Assumption",
                    VariableEffect: this.demosettingform.controls["patienteffect"].value == 'Positive' ? true : false,
                    IsActive: this.demosettingform.controls["Isactivepatient"].value == true ? "Yes" : "No"
                }
            }
            else
            {
            postassumption =
                {
                    Id: 0,
                    VariableName: this.demosettingform.controls["variablenamepatient"].value,
                    VariableDataType: this.demosettingform.controls["variabletypepatient"].value,
                    VariableDataTypeName: this.demosettingform.controls["variabletypepatient"].value == 1 ? "Numeric" : "Percentage",
                    UseOn: "OnAllSite",
                    ProgramId: this.selectedprogramid,
                    VarCode: "xx",
                    AssumptionType: 1,
                    AssumptionTypename: "Patient_Number_Assumption",
                    VariableEffect: this.demosettingform.controls["patienteffect"].value == 'Positive' ? true : false,
                    IsActive: this.demosettingform.controls["Isactivepatient"].value == true ? "Yes" : "No"
                }
            }

            this.suggestedvariable = {
                id: 0,
                Name: postassumption["VariableName"],
                Type_name: "Patient Assumption"
            }


        }
        else if (Assumtiontype == 3) {
            if (variablename !="")
            {
                postassumption =
                {
                    Id: 0,
                    VariableName: variablename,
                    VariableDataType: this.demosettingform.controls["variabletypetesting"].value,
                    VariableDataTypeName: this.demosettingform.controls["variabletypetesting"].value == 1 ? "Numeric" : "Percentage",
                    UseOn: "OnAllSite",
                    ProgramId: this.selectedprogramid,
                    VarCode: "xx",
                    AssumptionType: 3,
                    AssumptionTypename: "Test_Assumption",
                    VariableEffect: this.demosettingform.controls["testingeffect"].value == 'Positive' ? true : false,
                    IsActive: this.demosettingform.controls["Isactivetesting"].value == true ? "Yes" : "No"
                }
            }
            else
            {
            postassumption =
                {
                    Id: 0,
                    VariableName: this.demosettingform.controls["variablenametesting"].value,
                    VariableDataType: this.demosettingform.controls["variabletypetesting"].value,
                    VariableDataTypeName: this.demosettingform.controls["variabletypetesting"].value == 1 ? "Numeric" : "Percentage",
                    UseOn: "OnAllSite",
                    ProgramId: this.selectedprogramid,
                    VarCode: "xx",
                    AssumptionType: 3,
                    AssumptionTypename: "Test_Assumption",
                    VariableEffect: this.demosettingform.controls["testingeffect"].value == 'Positive' ? true : false,
                    IsActive: this.demosettingform.controls["Isactivetesting"].value == true ? "Yes" : "No"
                }
            }
            this.suggestedvariable = {
                id: 0,
                Name: postassumption["VariableName"],
                Type_name: "Testing Assumption"
            }

        }
        else {
            if (variablename !="")
            {
                postassumption =
                {
                    Id: 0,
                    VariableName: variablename,
                    VariableDataType: this.demosettingform.controls["variabletypeproduct"].value,
                    VariableDataTypeName: this.demosettingform.controls["variabletypeproduct"].value == 1 ? "Numeric" : "Percentage",
                    UseOn: "OnAllSite",
                    ProgramId: this.selectedprogramid,
                    VarCode: "xx",
                    AssumptionType: 2,
                    AssumptionTypename: "Product_Assumption",
                    VariableEffect: this.demosettingform.controls["producteffect"].value == 'Positive' ? true : false,
                    IsActive: this.demosettingform.controls["Isactiveproduct"].value == true ? "Yes" : "No"
                }

            }
            else
            {
            postassumption =
                {
                    Id: 0,
                    VariableName: this.demosettingform.controls["variablenameproduct"].value,
                    VariableDataType: this.demosettingform.controls["variabletypeproduct"].value,
                    VariableDataTypeName: this.demosettingform.controls["variabletypeproduct"].value == 1 ? "Numeric" : "Percentage",
                    UseOn: "OnAllSite",
                    ProgramId: this.selectedprogramid,
                    VarCode: "xx",
                    AssumptionType: 2,
                    AssumptionTypename: "Product_Assumption",
                    VariableEffect: this.demosettingform.controls["producteffect"].value == 'Positive' ? true : false,
                    IsActive: this.demosettingform.controls["Isactiveproduct"].value == true ? "Yes" : "No"
                }
            }
            this.suggestedvariable = {
                id: 0,
                Name: postassumption["VariableName"],
                Type_name: "Product Assumption"
            }

        }

        // this.patientassumptionlist.push(postassumption)

        this._APIwithActionService.postAPI(postassumption, 'MMProgram', 'savegeneralassumptions').subscribe
            (
                (data) => {
                    if (data["_body"] != 0) {
                      


                        this.getgeneralassumption();
                        this.demosettingform.patchValue({
                            variablenamepatient: '',
                            variabletypepatient: '1',
                            patienteffect: 'Positive',
                            Isactivepatient: true,
                            variablenametesting: '',
                            variabletypetesting: '1',
                            testingeffect: 'Positive',
                            Isactivetesting: true,
                            variablenameproduct: '',
                            variabletypeproduct: '1',
                            producteffect: 'Positive',
                            Isactiveproduct: true
                        })
                        this.demosettingform.controls['variablenamepatient'].markAsUntouched();
                        this.demosettingform.controls['variabletypepatient'].markAsUntouched();
                        this.demosettingform.controls['variablenametesting'].markAsUntouched();
                        this.demosettingform.controls['variabletypetesting'].markAsUntouched();
                        this.demosettingform.controls['variablenameproduct'].markAsUntouched();
                        this.demosettingform.controls['variabletypeproduct'].markAsUntouched();
                        // if (variablename =="")
                        // {
                        //    // this.lgModal1.show()
                        // }
                          
                    }
                    else {
                        this._GlobalAPIService.FailureMessage("Variable Name must not be duplicate");
                    }
                    //  this.patientassumption = this.patientassumptionlist.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 1);
                    //  this.testingassumption = this.Testingassumptionlist.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 3);
                    //  this.productassumption = this.Productassumptionlist.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 3);
                }
            )

    }
    //savegroup
    addnewgroup(groupname: string = "") {
        let newgroup = new Object();
        if (groupname != "") {
            newgroup = {
                Id: 0,
                GroupName: groupname,
                ProgramId: this.selectedprogramid,
                IsActive: true
            }
        }
        else {
            newgroup = {
                Id: 0,
                GroupName: this.demosettingform.controls['groupName'].value,
                ProgramId: this.selectedprogramid,
                IsActive: true
            }
        }
        this.suggestedvariable = {
            id: 0,
            Name: newgroup["GroupName"],
            Type_name: "Patient/Population Group"
        }
        this._APIwithActionService.postAPI(newgroup, 'MMProgram', 'savegroup').subscribe((data) => {
            if (data["_body"] != 0) {
                // this._APIwithActionService.postAPI(this.suggestedvariable, "MMProgram", "savesuggustion").subscribe((data1) => {


                // })
                this._GlobalAPIService.SuccessMessage("Group saved successfully");
                this.demosettingform.patchValue({
                    groupName: ''
                })
                this.demosettingform.controls['groupName'].markAsUntouched();
                this.getpatientgroup();
                // if (groupname == "") {
                //     this.lgModal1.show()
                // }
            }
            else {
                this._GlobalAPIService.FailureMessage("Group Name must not be Duplicate");
            }
        })
    }
    addnewprogram(programname: string = "") {
        let newprogram = new Object();

        if (programname != "") {
            newprogram = {
                Id: this.demosettingform.controls['id'].value,
                ProgramName: programname
            }
        }
        else {
            newprogram = {
                Id: this.demosettingform.controls['id'].value,
                ProgramName: this.demosettingform.controls['Programname'].value
            }
        }
        this.suggestedvariable = {
            id: 0,
            Name: newprogram["ProgramName"],
            Type_name: "Program"
        }
        this._APIwithActionService.postAPI(newprogram, 'MMProgram', 'SaveProgram').subscribe((data) => {
            if (data["_body"] != 0) {
              
            
                this._GlobalAPIService.SuccessMessage("Program saved successfully");
                this.demosettingform.patchValue({
                    Programname: ''
                })
                this.demosettingform.controls['Programname'].markAsUntouched();

               
                this.demosettingform.patchValue({
                    id: data["_body"],
                    Programname: newprogram["ProgramName"],
                    // Years: this.ProgramList[i].noofYear === 2 ? "2" : "1"
                })
                this.selectedprogramid=Number(this.demosettingform.controls['id'].value)
                this.programname = newprogram["ProgramName"]
                //  this.Settingdisplayproperty("block");


                this.getprogramlist();
                this.getgeneralassumption();
               
            }
            else {
                this._GlobalAPIService.FailureMessage("Program Name must not be duplicate");
            }
        })
    }
    savesugguestion()
    {
        this._APIwithActionService.postAPI(this.suggestedvariable, "MMProgram", "savesuggustion").subscribe((data1) => {

            // this.getsuggustionlist()
            // if(this.suggestedvariable["Type_name"]=="Program")
            // {
            //     this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Program')
            // }
            // else if(this.suggestedvariable["Type_name"]=="Patient/Population Group")
            // {
            //     this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Patient/Population Group')
            // }
            // else if(this.suggestedvariable["Type_name"]=="Patient Assumption")
            // {
            //     this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Patient Assumption')
            // }
            // else if(this.suggestedvariable["Type_name"]=="Testing Assumption")
            // {
            //     this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Testing Assumption')
            // }
            // else if(this.suggestedvariable["Type_name"]=="Product Assumption")
            // {
            //     this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Product Assumption')
            // }

      })
      //this.lgModal1.hide();
    }
    /** change tab  */
    defineprogramclick() {
        this.CurrentTab = 0;
        this.nextstatus = false;
        this.backstatus = true;
        this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Program')
    }
    Methodclick() {
        this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Forecasting method')
        if (this.selectedprogramid == 0) {
            this._GlobalAPIService.FailureMessage("Please atleast one program")
            // return;

        }
        else {
            this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Forecasting method')
            this.MMforcastparameter = this.MMForecastParameterList.filter(x => x.programId === this.selectedprogramid)
            if (this.MMforcastparameter.length > 0) {
                this.methodstatus = true;
            }
            else {
                this.methodstatus = false;
            }
            this.CurrentTab = 1;
        }
        this.nextstatus = false;
        this.backstatus = false;

    }
    updateprogramyears() {
        let programobject = new Object();
        programobject = {
            id: this.selectedprogramid,
            ProgramName: this.programname,
            NoofYear: parseInt(this.demosettingform.controls['Years'].value)
        }
        this._APIwithActionService.putAPI(this.selectedprogramid, programobject, 'MMProgram', 'updateProgram').subscribe((data) => {
            if (data['status'] == '200') {
                this._GlobalAPIService.SuccessMessage('Testing Year updated successfully');
            }
            console.log(data)

        })
    }
    patientgroupclick() {
        this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Patient/Population Group')
        if (this.selectedprogramid == 0) {
            this._GlobalAPIService.FailureMessage("Please atleast one program")
        }
        else {
            this.patientgroup = this.patientgrouplist.filter(x => x.programId === this.selectedprogramid)
            this.CurrentTab = 2;
        }
        this.nextstatus = false;
        this.backstatus = false;
    }
    patientassumptionclick() {
        this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Patient Assumption')
        if (this.selectedprogramid == 0) {
            this._GlobalAPIService.FailureMessage("Please atleast one program")
        }
        else {
            this.getgeneralassumption();
            this.CurrentTab = 3;
        }
        this.nextstatus = false;
        this.backstatus = false;
    }
    testingAssumptionclick() {
        this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Testing Assumption')
        if (this.selectedprogramid == 0) {
            this._GlobalAPIService.FailureMessage("Please atleast one program")
        }
        else {

            this.getgeneralassumption();
            this.CurrentTab = 4;
        }
        this.nextstatus = false;
        this.backstatus = false;
    }
    productassumptionclick() {
        this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Product Assumption')
        if (this.selectedprogramid == 0) {
            this._GlobalAPIService.FailureMessage("Please atleast one program")
        }
        else {
            this.getgeneralassumption();
            this.CurrentTab = 5;
        }
        this.nextstatus = false;
        this.backstatus = false;
    }
    testingprotocolclick() {
        if (this.selectedprogramid == 0) {
            this._GlobalAPIService.FailureMessage("Please atleast one program")
        }
        else {
            this.CurrentTab = 6;
        }
        this.nextstatus = false;
        this.backstatus = false;
    }
    Reviewclick() {
        if (this.selectedprogramid == 0) {
            this._GlobalAPIService.FailureMessage("Please atleast one program")
        }
        else {
            this.CurrentTab = 7;
        }
        this.nextstatus = true;
        this.backstatus = false;
    }
    getgeneralassumption() {
        this._APIwithActionService.getDataList('MMProgram', 'GetGeneralAssumptionList').subscribe((resp) => {
            this.generalassumptionlist = resp;
            this.patientassumption = this.generalassumptionlist.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 1)

            this.testingassumption = this.generalassumptionlist.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 3)
            this.productassumption = this.generalassumptionlist.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 2)
        })
    }
    getprogramlist() {
        this._APIwithActionService.getDataList('MMProgram', 'Get').subscribe((resp) => {
            this.ProgramList = resp;
        })
    }
    getprogramparameter() {
        this._APIwithActionService.getDataList('MMProgram', 'Getforecastparameter').subscribe((resp) => {
            this.MMForecastParameterList = resp;
        })
    }
    getprogram(i) {
        this.selectedprogramid = this.ProgramList[i].id
        this.demosettingform.patchValue({
            id: this.ProgramList[i].id,
            Programname: this.ProgramList[i].programName,
            Years: this.ProgramList[i].noofYear === 2 ? "2" : "1"
        })
        this.programname = this.ProgramList[i].programName
        //  this.Settingdisplayproperty("block");
        this.getgeneralassumption();
    }
    setparametername(args) {
        let postforecastparameter = new Array();
        if (args.target.value == 1) {
            this.parametername[0] = "CurrentPatient";
            this.parametername[1] = "TargetPatient";
        }
        else {
            this.parametername[0] = "PopulationNumber";
            this.parametername[1] = "PrevalenceRate";
        }
        console.log(this.parametername)
        for (let index = 0; index < this.parametername.length; index++) {
            postforecastparameter.push(
                {
                    id: 0,
                    forecastMethod: parseInt(args.target.value),
                    forecastMethodname: args.target.options[args.target.selectedIndex].text,
                    variableName: this.parametername[index],
                    variableDataType: 1,
                    variableDataTypename: "Numeric",
                    useOn: "OnAllSite",
                    variableFormula: "",
                    ProgramId: this.selectedprogramid,
                    VarCode: this.parametername[index][0] + this.selectedprogramid,
                    IsPrimaryOutput: false,
                    VariableEffect: true,
                    isActive: "Yes"
                }

            )

        }
        this.MMforcastparameter.splice(0, this.MMforcastparameter.length)
        this.MMforcastparameter = postforecastparameter;
        postforecastparameter.forEach(x => {
            this.MMForecastParameterList.push(x);
        })
        this.parameterobject = {
            id: this.selectedprogramid,
            ProgramName: this.programname,
            _mMForecastParameter: this.MMforcastparameter
        }
        console.log(this.MMForecastParameterList);

    }

    Fillforecastparameter() {

        if (this.selectedprogramid != 0) {
            this.getgeneralassumption();
            console.log(this.CurrentTab);
            if (this.activeStep.key == "step1") {
                this.MMforcastparameter = this.MMForecastParameterList.filter(x => x.programId === this.selectedprogramid)

                this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Forecasting method')
                // let el: HTMLElement = this.Method1.nativeElement as HTMLElement;

                // el.click();
                if (this.MMforcastparameter.length > 0) {
                    this.methodstatus = true;
                }
                else {
                    this.methodstatus = false;
                }
                this.backstatus = false;
                this.activeStep = this.steps[1]
            }
            else if (this.activeStep.key == "step2") {
                console.log(this.parameterobject);
                this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Patient/Population Group')
                if (this.parametername.length != 0) {
                    this._APIwithActionService.postAPI(this.parameterobject, 'MMProgram', 'Saveforecastparameter').subscribe(
                        (data) => {
                            this.parameterobject = "";
                            this.parametername.splice(0, this.parametername.length);
                        }
                    )
                }
                this.patientgroup = this.patientgrouplist.filter(x => x.programId === this.selectedprogramid)


                this.activeStep = this.steps[2]
            }
            else if (this.activeStep.key == "step3") {

                //   this.patientassumption = this.patientassumptionlist.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 1);
                this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Patient Assumption')
                this.activeStep = this.steps[3]

            }
            else if (this.activeStep.key == "step4") {
                //   this.testingassumption = this.Testingassumptionlist.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 3);
                this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Testing Assumption')
                this.activeStep = this.steps[4]

            }
            else if (this.activeStep.key == "step5") {
                // this.productassumption = this.Productassumptionlist.filter(x => x.programId === this.selectedprogramid && x.assumptionType === 2);
                this.filtersuggustionlist = this.suggustionlist.filter(x => x.type_name === 'Product Assumption')
                this.activeStep = this.steps[5]

            }
            else if (this.activeStep.key == "step6") {


                this.activeStep = this.steps[6]
            }
            else if (this.activeStep.key == "step8") {



                //this.nextstatus = true;
                this._router.navigate(["/Demographic/DemographicList", this.selectedprogramid])


            }
            else {
                this.activeStep = this.steps[7]

            }

        }
        else {
            this._GlobalAPIService.FailureMessage("Please atleast one program")
        }
    }
    backtotabs() {
        let idx = this.steps.indexOf(this.activeStep);
        if (idx > 0) {
            this.activeStep = this.steps[idx - 1]
        }
        // if (this.CurrentTab == 7) {

        //     let el: HTMLElement = this.testingprotocol1.nativeElement as HTMLElement;
        //     el.click();





    }
}