using ForLabApi.DataInterface;
using ForLabApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ForLabApi.Repositories
{
   
    public class CountryAccessRepositories : ICountry<CountryList, MastDiseases, Countrylistusedortrained>
    {
        ForLabContext ctx;
        private IHostingEnvironment _hostingEnvironment;
        public CountryAccessRepositories(ForLabContext c)
        {
            ctx = c;
            //return ctx;
        }
        public string Importhistoricaldata(IFormFile file)
        {
            string folderName = "Upload";
            string res = "";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            ISheet sheet;
            IWorkbook workbook = null;
            IFont Font;
            ICellStyle cellStyle;
            List<Region> Rgl = new List<Region>();
            List<CountryDiseasedetail> FS = new List<CountryDiseasedetail>();
            string str;

            var adminuserid = ctx.User.Where(b => b.Role == "admin").Select(x =>

   x.Id

).FirstOrDefault();
            try
            {

           
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            if (file.Length > 0)
            {
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string fullPath = Path.Combine(newPath, fileName);
                // string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                //string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();

                using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite))
                {

                    if (sFileExtension == ".xls")
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheet("Sheet1"); //get first sheet from workbook  
                        Font = hssfwb.CreateFont();
                        cellStyle = hssfwb.CreateCellStyle();
                        workbook = hssfwb;
                    }
                    else
                    {

                        // file.CopyTo(stream);
                        stream.Position = 0;
                        //InputStream ExcelFileToRead = new FileInputStream("C:/Test.xlsx");
                        XSSFWorkbook hssfwb = new XSSFWorkbook((FileStream)stream); //This will read 2007 Excel format  

                        sheet = hssfwb.GetSheet("Sheet1"); //get first sheet from workbook   
                        Font = hssfwb.CreateFont();
                        cellStyle = hssfwb.CreateCellStyle();
                        workbook = hssfwb;
                    }

                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    if (11 > cellCount)
                    {
                        res = "Imported Sheet has less columns than needed.";
                        return res;
                    }
                    if (11 < cellCount)
                    {
                            res = "Imported Sheet has too many columns.";
                        return res;
                    }
                    if (sheet.LastRowNum == 0)
                    {
                            res = "Imported Sheet is empty.";
                         return res;
                    }


                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        //for (int j = row.FirstCellNum; j < cellCount; j++)
                        //{

                        if (row.GetCell(0) != null && row.GetCell(0).ToString() != "")
                        {
                            var country = ctx.Country.Where(b => b.Name == row.GetCell(0).ToString() ).FirstOrDefault();
                            if (country != null)
                            {

                                if (row.GetCell(1) != null && row.GetCell(1).ToString() != "")
                                {
                                    var disease = ctx.MastDiseases.Where(b => b.Name == row.GetCell(1).ToString()).FirstOrDefault();
                                    {
                                        var CountryDiseasedetail = ctx.CountryDiseasedetail.Where(c => c.CountryId == country.Id && c.DiseaseId == disease.id && c.Year== row.GetCell(2).ToString()).FirstOrDefault();
                                        if (CountryDiseasedetail == null )
                                        {
                                            CountryDiseasedetail FS1 = new CountryDiseasedetail();
                                            FS1.CountryId = country.Id;
                                            FS1.DiseaseId= disease.id;
                                            FS1.Year = row.GetCell(2).ToString();
                                            FS1.Population = Convert.ToDecimal(row.GetCell(3).ToString());
                                            if (row.GetCell(4).ToString().Trim('<').IndexOf("[")>0)
                                            {
                                                FS1.Incidence = Convert.ToDecimal(row.GetCell(4).ToString().Trim('<').Substring(row.GetCell(4).ToString().IndexOf("[")+1, row.GetCell(4).ToString().Trim('<').Length - 1 - row.GetCell(4).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                FS1.Incidence = Convert.ToDecimal(row.GetCell(4).ToString().Trim('<'));
                                            }


                                            if (row.GetCell(5).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                FS1.Incidenceper1000population = Convert.ToDecimal(row.GetCell(5).ToString().Trim('<').Substring(row.GetCell(5).ToString().IndexOf("[") + 1, row.GetCell(5).ToString().Trim('<').Length - 1 - row.GetCell(5).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                FS1.Incidenceper1000population = Convert.ToDecimal(row.GetCell(5).ToString().Trim('<'));
                                            }


                                            if (row.GetCell(6).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                FS1.Incidenceper100kPopulation = Convert.ToDecimal(row.GetCell(6).ToString().Trim('<').Substring(row.GetCell(6).ToString().IndexOf("[") + 1, row.GetCell(6).ToString().Trim('<').Length - 1 - row.GetCell(6).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                FS1.Incidenceper100kPopulation = Convert.ToDecimal(row.GetCell(6).ToString().Trim('<'));
                                            }
                                            if (row.GetCell(7).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                FS1.Prevalencerate = Convert.ToDecimal(row.GetCell(7).ToString().Trim('<').Substring(row.GetCell(7).ToString().IndexOf("[") + 1, row.GetCell(7).ToString().Trim('<').Length - 1 - row.GetCell(7).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                FS1.Prevalencerate = Convert.ToDecimal(row.GetCell(7).ToString().Trim('<'));
                                            }



                                            if (row.GetCell(8).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                FS1.Prevalenceper1000population = Convert.ToDecimal(row.GetCell(8).ToString().Trim('<').Substring(row.GetCell(8).ToString().IndexOf("[") + 1, row.GetCell(8).ToString().Trim('<').Length - 1 - row.GetCell(8).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                FS1.Prevalenceper1000population = Convert.ToDecimal(row.GetCell(8).ToString().Trim('<'));
                                            }

                                            if (row.GetCell(9).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                FS1.Prevalenceper100kpopulation = Convert.ToDecimal(row.GetCell(9).ToString().Trim('<').Substring(row.GetCell(9).ToString().IndexOf("[") + 1, row.GetCell(9).ToString().Trim('<').Length - 1 - row.GetCell(9).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                FS1.Prevalenceper100kpopulation = Convert.ToDecimal(row.GetCell(9).ToString().Trim('<'));
                                            }
                                            FS1.Note = row.GetCell(10).ToString();
                                            FS.Add(FS1);
                                        }
                                        else
                                        {
                                            CountryDiseasedetail.CountryId = country.Id;
                                            CountryDiseasedetail.DiseaseId = disease.id;
                                            CountryDiseasedetail.Year = row.GetCell(2).ToString();
                                            CountryDiseasedetail.Population = Convert.ToDecimal(row.GetCell(3).ToString());
                                            if (row.GetCell(4).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                CountryDiseasedetail.Incidence = Convert.ToDecimal(row.GetCell(4).ToString().Trim('<').Substring(row.GetCell(4).ToString().IndexOf("[") + 1, row.GetCell(4).ToString().Trim('<').Length - 1 - row.GetCell(4).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                CountryDiseasedetail.Incidence = Convert.ToDecimal(row.GetCell(4).ToString().Trim('<'));
                                            }


                                            if (row.GetCell(5).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                CountryDiseasedetail.Incidenceper1000population = Convert.ToDecimal(row.GetCell(5).ToString().Trim('<').Substring(row.GetCell(5).ToString().IndexOf("[") + 1, row.GetCell(5).ToString().Trim('<').Length - 1 - row.GetCell(5).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                CountryDiseasedetail.Incidenceper1000population = Convert.ToDecimal(row.GetCell(5).ToString().Trim('<'));
                                            }


                                            if (row.GetCell(6).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                CountryDiseasedetail.Incidenceper100kPopulation = Convert.ToDecimal(row.GetCell(6).ToString().Trim('<').Substring(row.GetCell(6).ToString().IndexOf("[") + 1, row.GetCell(6).ToString().Trim('<').Length - 1 - row.GetCell(6).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                CountryDiseasedetail.Incidenceper100kPopulation = Convert.ToDecimal(row.GetCell(6).ToString().Trim('<'));
                                            }
                                            if (row.GetCell(7).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                CountryDiseasedetail.Prevalencerate = Convert.ToDecimal(row.GetCell(7).ToString().Trim('<').Substring(row.GetCell(7).ToString().IndexOf("[") + 1, row.GetCell(7).ToString().Trim('<').Length - 1 - row.GetCell(7).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                CountryDiseasedetail.Prevalencerate = Convert.ToDecimal(row.GetCell(7).ToString().Trim('<'));
                                            }



                                            if (row.GetCell(8).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                CountryDiseasedetail.Prevalenceper1000population = Convert.ToDecimal(row.GetCell(8).ToString().Trim('<').Substring(row.GetCell(8).ToString().IndexOf("[") + 1, row.GetCell(8).ToString().Trim('<').Length - 1 - row.GetCell(8).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                CountryDiseasedetail.Prevalenceper1000population = Convert.ToDecimal(row.GetCell(8).ToString().Trim('<'));
                                            }

                                            if (row.GetCell(9).ToString().Trim('<').IndexOf("[") > 0)
                                            {
                                                CountryDiseasedetail.Prevalenceper100kpopulation = Convert.ToDecimal(row.GetCell(9).ToString().Trim('<').Substring(row.GetCell(9).ToString().IndexOf("[") + 1, row.GetCell(9).ToString().Trim('<').Length - 1 - row.GetCell(9).ToString().Trim('<').IndexOf("]")));
                                            }
                                            else
                                            {
                                                CountryDiseasedetail.Prevalenceper100kpopulation = Convert.ToDecimal(row.GetCell(9).ToString().Trim('<'));
                                            }
                                            CountryDiseasedetail.Note = row.GetCell(10).ToString();
                                            ctx.SaveChanges();

                                        }
                                    }
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            //str = str + "Error in Row No." + (i + 1) + " : Region Name Required,";
                            //error = error + 1;
                            //continue;
                        }

                    }

                    //str = str + noofsave + " Regions/Districts/Provinces  are imported and saved successfully.,";
                    //if (error > 0)
                    //{
                    //    str = str + error + " Regions/Districts/Provinces  Failed.Please check excel and correct Regions/Districts/Provinces Entry";
                    //}
                }

                if (FS.Count > 0)
                {
                    ctx.CountryDiseasedetail.AddRange(FS);
                    ctx.SaveChanges();
                }
                    res = "success";

            }
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }
        public void Savecountry(CountryList CL)
        {
            var country = ctx.Country.Find(CL.Id);
            Country country1 = new Country();
            if (country == null)
            {
                country1.Name = CL.Name;
                country1.Regionid = CL.Regionid;
                country1.Region = ctx.GlobalRegion.Find(CL.Regionid).Name;
                country1.Population = CL.Population;
                country1.ShortCode = CL.Shortcode;
                country1.Period = CL.Period;
                ctx.Country.Add(country1);
                ctx.SaveChanges();
                //for (int i = 0; i < CL.CountryDiseasedetail.Count; i++)
                //{
                //    var isesixt = ctx.CountryDiseasedetail.Where(b => b.CountryId == country1.Id && b.DiseaseId == CL.CountryDiseasedetail[i].DiseaseId).FirstOrDefault();
                //    if (isesixt == null)
                //    {
                //        CL.CountryDiseasedetail[i].CountryId = country1.Id;
                //        ctx.CountryDiseasedetail.Add(CL.CountryDiseasedetail[i]);
                //        ctx.SaveChanges();
                //    }
                //    else
                //    {
                //        isesixt.Incidentrate = CL.CountryDiseasedetail[i].Incidentrate;
                //        isesixt.PrevalanceRate = CL.CountryDiseasedetail[i].PrevalanceRate;
                //        ctx.SaveChanges();

                //    }
                //}

            }
            else
            {
                country.Name = CL.Name;
                country.Regionid = CL.Regionid;
                country.Region = ctx.GlobalRegion.Find(CL.Regionid).Name;
                country.Population = CL.Population;
                country.ShortCode = CL.Shortcode;
                country.Period = CL.Period;
                //for (int i = 0; i < CL.CountryDiseasedetail.Count; i++)
                //{
                //    var isesixt = ctx.CountryDiseasedetail.Where(b => b.CountryId == country.Id && b.DiseaseId == CL.CountryDiseasedetail[i].DiseaseId).FirstOrDefault();
                //    if (isesixt == null)
                //    {
                //        CL.CountryDiseasedetail[i].CountryId = country.Id;
                //        ctx.CountryDiseasedetail.Add(CL.CountryDiseasedetail[i]);
                //        ctx.SaveChanges();
                //    }
                //    else
                //    {
                //        isesixt.Incidentrate = CL.CountryDiseasedetail[i].Incidentrate;
                //        isesixt.PrevalanceRate = CL.CountryDiseasedetail[i].PrevalanceRate;
                //        ctx.SaveChanges();

                //    }
                //}
                ctx.SaveChanges();

            }
        }


        public List<MastDiseases> GetMastDiseaseslist()
        {
            var disease = ctx.MastDiseases.ToList();
            return disease;
        }



        public CountryList getcountrydatabyid(int Id)
        {
            CountryList CL = new CountryList();
            var country = ctx.Country.Find(Id);
            CL.Id = country.Id;
            CL.Name = country.Name;
      //      CL.Population = country.Population;
            CL.Regionid = country.Regionid;
            CL.Period = country.Period;
            CL.Shortcode = country.ShortCode;
            CL.CountryDiseasedetail = ctx.CountryDiseasedetail.Where(b => b.CountryId == country.Id).Join(ctx.MastDiseases, b => b.DiseaseId, c => c.id, (b, c) => new { b, c })
                .Select(s => new Historicaldatacountry
                {
                    Disease = s.c.Name,
                    Incidence=s.b.Incidence,
                    Population=s.b.Population,
                    Incidenceper1000population=s.b.Incidenceper1000population,
                    Incidenceper100kPopulation=s.b.Incidenceper100kPopulation,
                    Prevalenceper1000population=s.b.Prevalenceper1000population,
                    Prevalenceper100kpopulation=s.b.Prevalenceper100kpopulation,
                    Prevalencerate=s.b.Prevalencerate,
                    Year=s.b.Year,
                    

                } ).ToList();

            return CL;
        }

        public List<Countrylistusedortrained> Countrylistusedortraine()
        {
            var countries = ctx.CMSCountry.Join(ctx.Country,b=>b.countryID,c=>c.Id, (b, c) => new { b, c }).Select(s=> new {

                s.c.Name,
                s.c.Langitude,
                s.c.Latitude,
                s.b.type
            }).ToList();
            List<Countrylistusedortrained> CL = new List<Countrylistusedortrained>();
            Countrylistusedortrained CL1 = new Countrylistusedortrained();
            for (int i = 0; i < countries.Count(); i++)
            {
                CL1 = new Countrylistusedortrained();
                CL1.Name = countries[i].Name;
                CL1.Lat = countries[i].Latitude;
                CL1.Lon= countries[i].Langitude;
                if (countries[i].type==1)
                {
                    CL1.Z =1;
                  
                }
                else
                {
                    CL1.Z = 3;
                 
                }
                CL.Add(CL1);
               
            }

            return CL;
        }
    }
}
