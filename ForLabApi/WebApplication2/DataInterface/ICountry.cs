using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface ICountry<CountryList, MastDiseases, Countrylistusedortrained>
    {
        string Importhistoricaldata(IFormFile file);
        void Savecountry(CountryList CL);
        List<MastDiseases> GetMastDiseaseslist();
        CountryList getcountrydatabyid(int Id);

        List<Countrylistusedortrained> Countrylistusedortraine();
    }
}
