using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ForLabApi.Models
{
    public class ForLabContext: DbContext
    {
        public ForLabContext(DbContextOptions opts) : base(opts)
        {
        }
        public DbSet<SiteCategory> SiteCategory { get; set; }
        public DbSet<TestingArea> TestingArea { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Site> Site { get; set; }
        public DbSet<SiteInstrument> siteinstrument { get; set; }
        public DbSet<Instrument> Instrument { get; set; }
        public DbSet<SiteStatus> SiteStatus { get; set; }
        public DbSet<MasterProduct> MasterProduct { get; set; }
        public DbSet<ProductPrice> ProductPrice { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<ProductUsage> ProductUsage { get; set; }   
        public DbSet<ConsumableUsage> ConsumableUsage { get; set; }
        public DbSet<MasterConsumable> MasterConsumable { get; set; }
        public DbSet<MMProgram> MMProgram { get; set; }
        public DbSet<MMForecastParameter> MMForecastParameter { get; set; }
        public DbSet<MMGroup> MMGroup { get; set; }
        public DbSet<MMGeneralAssumption> MMGeneralAssumption { get; set; }
        public DbSet<SiteTestingdays> SiteTestingdays { get; set; }
        public DbSet<ReferralLinkage> ReferralLinkage { get; set; }   
        public DbSet<DemographicMMGroup> DemographicMMGroup { get; set; }

        public DbSet<demographicMMGeneralAssumption> demographicMMGeneralAssumption { get; set; }
        public DbSet<ForecastInfo> ForecastInfo { get; set; }

       
        public DbSet<MorbidityTest> MorbidityTest { get; set; }
        public DbSet<QuantificationMetric> QuantificationMetric { get; set; }
        public DbSet<QuantifyMenu> QuantifyMenu { get; set; }
        public DbSet<entity_type> entity_type { get; set; }
        public DbSet<ForecastSiteInfo> ForecastSiteInfo { get; set; }
        public DbSet<ForecastCategoryInfo> ForecastCategoryInfo { get; set; }
        public DbSet<ForecastCategorySiteInfo> ForecastCategorySiteInfo { get; set; }
        public DbSet<PatientGroup> PatientGroup { get; set; }
        public DbSet<TestingProtocol> TestingProtocol { get; set; }
        public DbSet<PatientAssumption> PatientAssumption { get; set; }
        public DbSet<MMGeneralAssumptionValue> MMGeneralAssumptionValue { get; set; }
        public DbSet<TestingAssumption> TestingAssumption { get; set; }
        public DbSet<PatientNumberDetail> PatientNumberDetail { get; set; }
        public DbSet<PatientNumberHeader> PatientNumberHeader { get; set; }
        public DbSet<ForecastedTestByTest> ForecastedTestByTest { get; set; }
        public DbSet<PercentageVal> PercentageVal { get; set; }
        public DbSet<Temptbl1> Temptbl1 { get; set; }
        public DbSet<TestByMonth> TestByMonth { get; set; }
        public DbSet<ForecastCategoryProduct> ForecastCategoryProduct { get; set; }
        public DbSet<ForecastSiteProduct> ForecastSiteProduct { get; set; }
        public DbSet<ForecastSiteTest> ForecastSiteTest { get; set; }
        public DbSet<ForecastSite> ForecastSite { get; set; }
        public DbSet<ForecastCategory> ForecastCategory { get; set; }
        public DbSet<ForecastCategoryTest> ForecastCategoryTest { get; set; }
        public DbSet<ForecastNRSite> ForecastNRSite { get; set; }
        public DbSet<ForecastCategorySite> ForecastCategorySite { get; set; }
        public DbSet<ForecastedResult> ForecastedResult { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<CMSCountry> CMSCountry { get; set; }
        public DbSet<GlobalRegion> GlobalRegion { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<CMSData> CMSData { get; set; }
        public DbSet<cmsnew> cmsnew { get; set; }
        
        public DbSet<ForecastTest> ForecastTest { get; set; }
        public DbSet<ForecastIns> ForecastIns { get; set; }
        public DbSet<Suggustionlist> Suggustionlist { get; set; }

        public DbSet<ForecastProductUsage> ForecastProductUsage { get; set; }
        public DbSet<ForecastConsumableUsage> ForecastConsumableUsage { get; set; }

        public DbSet<CountryDiseasedetail> CountryDiseasedetail { get; set; }


        public DbSet<MastDiseases> MastDiseases { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
