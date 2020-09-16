using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string  Region { get; set; }

        public int Regionid { get; set; }
        public string Period { get; set; }
        public string Latitude { get; set; }
        public string Langitude { get; set; }
        public string ShortCode { get; set; }
        public decimal? Population { get; set; }
    }
    public class Regioncountryfromwholist
    {
        public List<Regioncountryfromwho> value { get; set; }
    }
    public class Populationdatalist
    {
        public List<Populationdata> value { get; set; }
    }
    public class Populationdata
    {
        public string SpatialDimType { get; set; }
        public string SpatialDim { get; set; }
        public string TimeDim { get; set; }
        public decimal NumericValue { get; set; }
    }
    public class Annualgrowthrate
    {
        public string SpatialDimType { get; set; }
        public string SpatialDim { get; set; }
        public string TimeDim { get; set; }
        public decimal NumericValue { get; set; }
    }

    public class Annualgrowthratelist
    {
        public List<Annualgrowthrate> value { get; set; }
    }
    public class Regioncountryfromwho
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string ParentDimension { get; set; }
        public string Dimension { get; set; }
        public string ParentCode { get; set; }
        public string ParentTitle { get; set; }
    }
    public class GlobalRegion
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Shortcode { get; set; }


    }
    public class Site
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SiteID { get; set; }
        public string SiteName { get; set; }

        public int CategoryID { get; set; }
        public int CD4TestingDaysPerMonth { get; set; }
        public int ChemistryTestingDaysPerMonth { get; set; }
        public int HematologyTestingDaysPerMonth { get; set; }
        public int ViralLoadTestingDaysPerMonth { get; set; }
        public int OtherTestingDaysPerMonth { get; set; }
        public int regionid { get; set; }
       
        // public SiteCategory siteCategory { get; set; }
        public int CD4RefSite { get; set; }
        public int ChemistryRefSite { get; set; }
        public int HematologyRefSite { get; set; }
        public int ViralLoadRefSite { get; set; }
        public int OtherRefSite { get; set; }
        public int WorkingDays { get; set; }
        public string SiteLevel { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int? UserId { get; set; }
        public IList<SiteStatus> _siteStatuses { get; set; }
        public IList<SiteInstrumentList> _siteInstruments { get; set; }

        public IList<SiteTestingdaysList> _sitetestingdays { get; set; }
        public IList<ReferralLinkageList> _ReferralLinkage { get; set; }
        public int? CountryId { get; set; }
        public bool Isapprove { get; set; }


        public bool Isreject { get; set; }


    }
    public class sitebyregion
    {

        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string RegionName { get; set; }
        public string CategoryName { get; set; }
        public string CountryName { get; set; }
        public string CurrentlyOpen { get; set; }
        public int WorkingDays { get; set; }
        public int Countryid { get; set; }
        public string GetLastOpenDate { get; set; }
        public string GetLastClosedDate { get; set; }
        public int? UserId { get; set; }
        public bool Isapprove { get; set; }
    }
    public class defaultsitelist
    {
        public string Region { get; set; }


        public string SiteCategory { get; set; }
        public string SiteName { get; set; }
    
        public int WorkingDays { get; set; }
 


    }
    public class defaultsite
    {
        public List<Region> Region { get; set; }
        public List<defaultsitelist> Site { get; set; }
    }
  
}
