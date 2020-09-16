using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Utility
{
    public enum MainMenuTag
    {
        DASHBOARD,
        SETTINGS,
        LOCATION,
        TEST,
        PRODUCT,
        INSTRUMENT,
        METHODOLOGY,
        PROTOCOL,
        RAPIDTEST,
        PROTOCOLS,
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
    public enum DataUsageEnum
    {
        [Description(" Data Usage1 (Site Consumption) ")]
        DATA_USAGE1,
        [Description("Data Usage2 (Reported Site Consumption)")]
        DATA_USAGE2,
        [Description("Data Usage3 (Consumption per Category)")]
        DATA_USAGE3
    }

    public enum MethodologyEnum
    {
        CONSUMPTION,
        SERVICE_STATISTIC,
        DEMOGRAPHIC

        //    ,
        //MORBIDITY

    }

    public enum ForecastStatusEnum
    {
        OPEN,
        REOPEN,
        CLOSED
    }

    public enum ForecastPeriodEnum
    {
        Bimonthly,
        Monthly,
        Quarterly,
        Yearly
    }

    public enum ForecastingMethodEnum
    {
        Linear,
        //Polynomial,
        Exponential,
        // Logarithmic,
        //Power
        MovingAverage
    }

    public enum OReports
    {
        Folder = 0,
        Default = 1,
        ConsumptionSummary = 2,
        ForecastSummary = 3,
        ForecastResultSummary = 4,
        ServiceQSummary = 5,
        FullQSummary = 6,
        MorbiditySupplyForecast = 7,
        MorbiditySupplyProcurement = 8,
        CD4TestNumberForecast = 9,
        HemaandViralTestNumberForecast = 10,
        PatientNumberForecast = 11,
        ChemistryNumberofTestForecast = 12,
        rptRegion = 13,
        rptRegionWithSiteCount = 14,
        rptSitewithworkingdays = 15,
        rptSite = 16,
        rptSiteInstrumentList = 17,
        rptInstrumentListwithControlTdays = 18,
        rptInstrumentList = 19,
        rptProductList = 20,
        rptProductPriceList = 21,
        rptTestList = 22,
        rptProductUsageList = 23,
        forecastcomparision = 24,
        Dforcastcostsummary = 25,
        forcastpatientsummarywithgraph = 26,
        Forecastresulttestbyregion = 27,
        sandcsummary = 28,
        /// <summary>
        /// //
        /// </summary>
        rptConsumptionCostSummary = 29,
        rptServiceCostSummary = 30,
        ProductsummaryReport = 31,

    }

    public enum DataUsageType
    {
        Test,
        Product
    }

    public enum RegionTemp
    {
        Region,
        Short_Name
    }
    public enum SiteTemp
    {
        Region,
        Site_Category,
        Site_Name,
        Site_Level,
        Working_Days,
        CD4_Tesing_Day_per_Month,
        Chemistry_Tesing_Day_per_Month,
        Hematology_Tesing_Day_per_Month,
        ViralLoad_Tesing_Day_per_Month,
        Other_Tesing_Day_per_Month,
        CD4_Referal_Site,
        Chemistry_Referal_Site,
        Hematology_Referal_Site,
        ViralLoad_Referal_Site,
        Other_Referal_Site,
        Opening_Date
    }

    public enum ProductTemp
    {
        Product_Name,
        Product_Type,
        Serial_No,
        Specification,
        Basic_Unit,
        Min_Packs_Per_Site,
        Test_Specification,
        Price,
        Packaging_Size,
        Price_As_of_Date
    }

    public enum InstrumentTemp
    {
        Testing_Area,
        Instrument_Name,
        Max_Through_Put,
        Per_Test_Control,
        Daily_Control_Test,
        Weekly_Control_Test,
        Monthly_Control_Test,
        Quarterly_control_Test
    }

    public enum TestTemp
    {
        Test_Name,
        Area_Name,
        Group_Name
    }

    public enum DataUsageTemp
    {
        Test_Name,
        Instrument,
        Product,
        Rate,
    }

    public enum SiteInstrumentTemp
    {
        Region_Name,
        Site_Name,
        Testing_Area,
        Instrument,
        Quantity,
        Percentage_Run
    }
    public enum ServiceStatsticTemp
    {
        Region_Name,
        Site,
        Test_Name,
        Usage

    }
    public enum ServiceStatsticByCategoryTemp
    {
        Site_Category,
        Test,
        Reporting_Period,
        Test_Performed,
        StockOut,
        Instrument_Downtime

    }

    public enum ConsumptionTemp
    {
        Region_Name,
        Site,
        Product_Name,
        Usage
    }

    public enum DataUsageImport
    {
        AmountUsed,
        Stockout,
        InstrumentDowntime,
        Adjusted
    }
    public enum ConsumptionByCategoryTemp
    {
        Category,
        Product,
        Reporting_Period,
        Consumption,
        StockOut,
        Instrument_Downtime
    }

    public enum ARTTestQuantificationVariableTemp
    {
        Product_Name,
        Usage_Rate,
        Quantify_According_to,
        Applied_to
    }
    public enum PatientType
    {
        Adults_In_Treatment,
        Pediatric_In_Treatment,
        Adult_Pre_ART,
        Pediatric_Pre_ART
    }

    public enum MorbidityCtrEnum
    {
        SiteSelection,
        CategorySelection,
        OptRecentData,
        OptPopulationBased,
        OptTargetBased,
        OptDynamicBased,
        OptPatientGroupChar,
        OptOtherGenralAssumption,
        NewGeneralProtocol,
        NewRapidTestProtocol,
        NewReviewadjustment,
        NewCheckupForm,
        RecentPatientData,
        FromRecentData,
        FromOldData,
        OptTreatmentTarget,
        OptArtPatientTarget,
        OpEverStartedPatientTarget,
        OptPreTreatmentPatientTargets,
        SiteTargetCalculator,
        SiteTargetCalculatorPreART,
        PatientNumbersSites,
        RapidTestSerial,
        RapidTestParallel,
        TestingInformation,
        AdultPatientBehavior,
        PediatricPatientBehavior,
        PreTxNumbersSites,
        TestingEfficiency,
        EverStartedRecentData,
        EverStartedOldData,
        EverStartedNoData,
        CalculateForm,
        InvAssumption,
        CheckupForm,
        Nothing,
        Dashboard,
        RapidTestProtocol,
        TestProtocolsCd4,
        TestProtocolsChem,
        TestProtocolsHem,
        TestProtocolsVir,
        TestProtocolsOther,
    }

    public enum MonthNameEnum
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public enum OptInitialPatientDataEnum
    {
        RecentData = 1,
        OldData = 2
    }

    public enum OptPatientTreatmentTargetEnum
    {
        OnTreatment = 1,
        OnEverStarted = 2
    }

    public enum OptArtPatinetTargetEnum
    {
        NationalTarget = 1,
        SiteGrowth = 2,
        SelectSite = 3,
        AllSite = 4
    }

    public enum OptPreTreatmentPatinetTargetEnum
    {
        NationalTarget = 1,
        SiteGrowth = 2,
        SelectSite = 3,
        AllSite = 4,
        TestingEfficiency = 5
    }
    public enum OptEverStartedPatientTargetEnum
    {
        RecentData = 1,
        OldData = 2,
        NoData = 3
    }

    public enum AlgorithmType
    {
        Serial,
        Parallel
    }

    public enum TestingSpecificationGroup
    {
        Screening,
        Confirmatory,
        Tie_Breaker
    }

    public enum ClassOfMorbidityTestEnum
    {
        CD4 = 1,
        Chemistry = 2,
        Hematology = 3,
        ViralLoad = 4,
        OtherTest = 5,
        RapidTest = 6,
        Consumable = 7
    }

    public enum TestTypeEnum
    {
        Test,
        ControlTest,
        PerInstrument,
        PerDay,
        General,
        SamplesRunOn
    }

    public enum TestingDurationEnum
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        PerTest,
        TotalControl
    }

    public enum CollectionSupplieAppliedToEnum
    {
        Testing,
        Collection
    }

    public enum GeneralQuantifyMenuEnum
    {
        /// <summary>
        /// Other
        /// </summary>
        Total_Positive_Diagnoses,
        Total_Positive_Diagnoses_to_Receive_CD4,
        Total_Blood_Draws,
        Blood_Draws_Adult,
        Blood_Draws_Pediatric,

        PerDay_PerSite,
        PerWeek_PerSite,
        PerMonth_PerSite,
        PerQuarter_PerSite,
        PerYear_PerSite,

        //Rapid test
        Total_Rapid_Tests,
        Total_Screenings,
        Total_Confirmatory_Tests,
        Total_Tie_Breaker_Tests,
        Total_Screenings_Plus_Confirmatory,
        Total_Confirmatory_Plus_Tie_Breaker,
        Total_Screenings_Plus_Tie_Breaker
    }

    //public enum ChemistryTestNameEnum
    //{
    //    ALT,
    //    AST,
    //    CHO,
    //    GLC,
    //    CRE,
    //    TG,
    //    GGT,
    //    ALP,
    //    AMY,
    //    CO2,
    //    Electrolyte_Panel,
    //    Urea
    //}

    //public enum OtherTestNameEnum
    //{
    //    Syphilis_RPR,
    //    TB_AFB,
    //    Hepatitis_HBsAG,
    //    Hepatitis_Anti_HCV,
    //    TB_Culture,
    //    TB_DST,
    //    PAP_Smear,
    //    Genotype_Resistance_Testing
    //}
    public enum SiteLevelEnum
    {
        Level_I,
        Level_II,
        Level_III,
        Level_IV,
        NGO,
        Other
    }

    public enum TestingAreaEnum
    {
        CD4 = 1,
        Chemistry = 2,
        Hematology = 3,
        ViralLoad = 4,
        OtherTest = 5
    }

    public enum InstrumentControlPeriod
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Quarterly = 4,
        Per_Test = 5
    }

    public enum PeriodEnum
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly
    }

    public enum QuanifyConsumableBasedOnEnum
    {
        PerTest,
        PerPeriod,
        PerInstrument
    }

    public enum MorbidityForecastingMethod
    {
        Target_Based = 1,
        Population_Based = 2,
        Custom_User_Defined = 3
    }

    public enum MorbidityVariablesDataType
    {
        Numeric = 1,
        Percentage = 2
    }

    public enum MorbidityVariableUsage
    {
        OnEachSite = 1,
        OnAggSite = 2,
        OnAllSite = 3
    }

    public enum MorbidityGeneralAssumptionTypes
    {
        // Patient_Population_Assumption = 1,
        Patient_Number_Assumption = 1,
        Product_Assumption = 2,
        Test_Assumption = 3
    }

}
