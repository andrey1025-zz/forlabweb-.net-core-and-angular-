using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.Models
{

    public class Countrylistusedortrained
    {
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public int Z { get; set; }
      
    }
    public class CountryList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Regionid { get; set; }
        public string Period { get; set; }
        public decimal Population { get; set; }

        public string Shortcode { get; set; }
        public List<Historicaldatacountry> CountryDiseasedetail { get; set; }

    }
    public class CountryDiseasedetail
    {
        public int Id { get; set; }
        public int DiseaseId { get; set; }
        public string Year { get; set; }
        public decimal Population { get; set; }
        public decimal Incidence { get; set; }
        public decimal Incidenceper1000population { get; set; }
        public decimal Incidenceper100kPopulation { get; set; }
        public decimal Prevalencerate { get; set; }
        public decimal Prevalenceper1000population { get; set; }
        public decimal Prevalenceper100kpopulation { get; set; }
        public string Note { get; set; }
        public int CountryId { get; set; }
    }
    public class Historicaldatacountry
    {
        public string Disease { get; set; }

        public string Year { get; set; }
        public decimal Population { get; set; }
        public decimal Incidence { get; set; }
        public decimal Incidenceper1000population { get; set; }
        public decimal Incidenceper100kPopulation { get; set; }
        public decimal Prevalencerate { get; set; }
        public decimal Prevalenceper1000population { get; set; }
        public decimal Prevalenceper100kpopulation { get; set; }

                
    }

    public class MastDiseases
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
}
