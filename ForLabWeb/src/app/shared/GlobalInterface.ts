export interface testingArea {
    testingAreaID: Number;
    AreaName: string;
}


export interface ProductType {
    typeID: Number;
    typeName: string;
}
export interface ProductPrice {
    id: Number;
    price: Number;
    packSize: Number;
    fromDate: Date;
    ProductId: Number;
}
export interface Product {
    ProductID: Number;
    ProductName: string;
    SerialNo: string;
    BasicUnit: string;

}
export interface Instrumentlist {
    InstrumentID: Number;
    InstrumentName: string;
    MaxThroughPut: Number;
    MonthMaxTPut: Number;
    testingArea: string;
    MaxTestBeforeCtrlTest: Number;
    WeeklyCtrlTest: Number;
    MonthlyCtrlTest: Number;
    QuarterlyCtrlTest: Number;
    CtrlTestDuration: string;
    CtrlTestNoOfRun: Number;

}
export interface ProductUsageRatelist {
    Id: Number;
    ProductId: Number;
    ProductName: string;
    InstrumentId: Number;
    InstrumentName: string;
    Rate: Number;
    ProductUsedIn: string;
    IsForControl: boolean;
    TestId: Number
}
export interface Siteinstrumentlist {
    id: Number;
    instrumentID: Number;
    instrumentName: string;
    testingareaId: Number;
    testingareaName: string;
    siteID: Number;
    quantity: Number;
    testRunPercentage: Number;

}
export interface postforecastparameter {
    id: Number,
    forecastMethod: Number,
    forecastMethodname: String,
    variableName: String,
    variableDataType: Number,
    variableDataTypename: String,
    useOn: String,
    variableFormula: String,
    ProgramId: Number,
    VarCode: String,
    IsPrimaryOutput: Boolean,
    VariableEffect: Boolean,
    isActive: String
};