using ForLabApi.DataInterface;
using ForLabApi.Models;
using ForLabApi.Utility;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace ForLabApi.Repositories
{
    public class UserAccessRepositories : IUserService<User, Importdata, updatepassword, GlobalRegion>
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications


        ForLabContext ctx;
        private readonly Appsettings _appSettings;
     
        public UserAccessRepositories(IOptions<Appsettings> appSettings, ForLabContext c)
        {
            _appSettings = appSettings.Value;

            ctx = c;

        }

        public void updatelogincount(int userid)
        {
            var User = ctx.User.Find(userid);
            User.logincnt = 1;
            ctx.SaveChanges();
        }
        public int verifyaccount(int id)
        {
            int res = 0;
            var user = ctx.User.Find(id);
            if (user != null)
            {
                user.Emailverify = true;
                ctx.SaveChanges();
                res = user.Id;
            }
            return res;
        }
        public string[] savedata(User b)
        {
            string[] res = new string[2];

            var user = ctx.User.Where(c => c.UserName == b.UserName).FirstOrDefault();
            if (user != null)
            {
                res[0] = "User Name already exists";
                res[1] = "0";
                return res;
            }
            else
            {
                var useremail = ctx.User.Where(c => c.Email == b.Email).FirstOrDefault();
                if (useremail != null)
                {
                    res[0] = "emailid already exists";
                    res[1] = "0";
                    return res;
                }
                else
                {
                    b.logincnt = 0;
                    ctx.User.Add(b);
                    ctx.SaveChanges();

                    string str = sendmail(b.Email, "create");

                    res[0] = str;
                    res[1] = Convert.ToString(b.Id);
                }
            }
            return res;

        }
        public int updateuserpassword(int id, updatepassword password)
        {
            var user = ctx.User.Find(id);
            user.Password = password.newpassword;
            ctx.SaveChanges();
            return user.Id;

        }
        public string Resetpassword(string email)
        {
            string res = "";
            res = sendmail(email,"reset");
            return res;
        }
        private string sendmail(string useremail,string type)
        {
            string str = "";
            try
            {
              
                var id = ctx.User.Where(b => b.Email == useremail).Select(x => x.Id).FirstOrDefault();
                var username = ctx.User.Where(b => b.Email == useremail).Select(x => x.UserName).FirstOrDefault();
                //var tokenHandler = new JwtSecurityTokenHandler();
                //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                //var tokenDescriptor = new SecurityTokenDescriptor
                //{
                //    Subject = new ClaimsIdentity(new Claim[]
                //    {
                //        new Claim(ClaimTypes.Name, b.Id.ToString())

                //    }),
                //    Issuer = "http://localhost:53234",
                //    Audience = "http://localhost:53234",


                //    //Issuer = "http://Forlab.dataman.net.in/webapi",
                //    //Audience = "http://Forlab.dataman.net.in/webapi",
                //    Expires = DateTime.UtcNow.AddMinutes(60),
                //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                //};
                //var token = tokenHandler.CreateToken(tokenDescriptor);
                //b.Token = tokenHandler.WriteToken(token);
                //ctx.SaveChanges();
                string htmlMessage = "";
                string subject = "";
                if (type == "create")
                {


                htmlMessage = "Dear " + username + ", <br><br> You are successfully registered on ForLab+.To Activate your account Please<br> <a href='http://forlabplus.com/#/verifylink/" + id + "' style= 'width:140px;background:linear-gradient(to bottom,#007fb8 1%,#6ebad5 3%,#007fb8 7%,#007fb8 100%);background-color:#007fb8;text-align:center;border:#004b91 solid -1px;padding:4px 0;text-decoration:none;border-radius:2px;display:block;color:#fff;font-size:13px'> Click here</a><br>If you didn't request this registration, you can safely ignore this email. Someone else might have typed your email address by mistake.<br>Thanks,<br>The ForLab+ Team ";
                    //   htmlMessage = "Dear Sir/ Mam, <br><br> You are successfully registered on ForLab.To Activate your account Please <a href='https://www.google.com' style= 'width:140px;background:linear-gradient(to bottom,#007fb8 1%,#6ebad5 3%,#007fb8 7%,#007fb8 100%);background-color:#007fb8;text-align:center;border:#004b91 solid -1px;padding:4px 0;text-decoration:none;border-radius:2px;display:block;color:#fff;font-size:13px'> Click here</a><br>Thanks.<br>ForLab Team ";

                    // htmlMessage = "Dear Sir/ Mam, <br><br> You are successfully registered on ForLab.To Activate your account Please <a href='http://localhost:4200/#/verifylink/" + id + "' style= 'width:140px;background:linear-gradient(to bottom,#007fb8 1%,#6ebad5 3%,#007fb8 7%,#007fb8 100%);background-color:#007fb8;text-align:center;border:#004b91 solid -1px;padding:4px 0;text-decoration:none;border-radius:2px;display:block;color:#fff;font-size:13px'> Click here</a><br>Thanks.<br>ForLab Team ";
                    subject = "Verification Eamil for ForLab+";
                }
                else
                {
                htmlMessage = "Dear " + username + ",<br><br>To Reset the password for ForLab+. Please <a href='http://forlabplus.com/#/resetpassword/" + id + "' style= 'width:140px;background:linear-gradient(to bottom,#007fb8 1%,#6ebad5 3%,#007fb8 7%,#007fb8 100%);background-color:#007fb8;text-align:center;border:#004b91 solid -1px;padding:4px 0;text-decoration:none;border-radius:2px;display:block;color:#fff;font-size:13px' > Click here</a><br>Thanks.<br>The ForLab+ Team  ";

                    // htmlMessage = "Dear Sir/ Mam,<br><br>To Reset the password for ForLab. Please <a href='http://localhost:4200/#/resetpassword/" + id + "' style= 'width:140px;background:linear-gradient(to bottom,#007fb8 1%,#6ebad5 3%,#007fb8 7%,#007fb8 100%);background-color:#007fb8;text-align:center;border:#004b91 solid -1px;padding:4px 0;text-decoration:none;border-radius:2px;display:block;color:#fff;font-size:13px' > Click here</a><br>Thanks. ";
                    subject = "Reset Password for ForLab+";
                }

                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("admin@forlabplus.com", "ZtcTetkS");
          
           
            
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("admin@forlabplus.com");
                mailMessage.To.Add(useremail);
                mailMessage.Body = htmlMessage;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.SubjectEncoding = System.Text.Encoding.Default;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
               
                client.Send(mailMessage);

                //var client = new SmtpClient("smtp.gmail.com", 587)
                //{
                //    Credentials = new NetworkCredential("jyotiawasthi198902@gmail.com", "jyoti@123"),
                //    EnableSsl = false
                //};
                //client.SendMailAsync(
                //    new MailMessage("jyotiawasthi198902@gmail.com", b.Email, "Verification Eamil for ForLab", htmlMessage) { IsBodyHtml = true }
                //);

                str = "success";

                return str;
            }
            catch(Exception ex)
            {
                return "Something went wrong";
            }
        }

        public IEnumerable<GlobalRegion> Getallglobalregions()
        {
            var Globalregion = ctx.GlobalRegion.ToList();
            return Globalregion;
        }
        public string Getglobalregion(int id)
        {
            var user = ctx.Country.Where(b => b.Id == id).Select(x => x.Region).FirstOrDefault();
            return user;
        }
        public string Importdefaultdata(Importdata importdata)
        {
            string res = "";
            int userid = 0;
            userid = ctx.User.Where(b => b.Role == "admin").Select(g => g.Id).FirstOrDefault();
            //var tests = ctx.Test.Where(b => importdata.Testingareaids.Contains(b.TestingAreaID) && b.UserId==userid).Select(g=> new Test
            //{
            //    TestID = g.TestID,
            //    TestingAreaID = g.TestingAreaID,
            //    TestName = g.TestName,
            //    UserId = importdata.userid,



            //}).ToList();
            if (importdata.importtest == true)
            {
                var testingarea = ctx.TestingArea.Where(b => importdata.Testingareaids.Contains(b.TestingAreaID) && b.UserId == userid).ToList();
                foreach (var item in testingarea)
                {
                    TestingArea TA = new TestingArea();
                    var tests = ctx.Test.Where(b => b.TestingAreaID == item.TestingAreaID && b.UserId == userid).ToList();
                    TA = ctx.TestingArea.Where(b => b.AreaName == item.AreaName && b.UserId == importdata.userid).FirstOrDefault();
                    if (TA != null)
                    {

                    }
                    else
                    {
                        TA = new TestingArea();
                        TA.AreaName = item.AreaName;
                        TA.Category = item.Category;
                        TA.UseInDemography = item.UseInDemography;
                        TA.UserId = importdata.userid;
                        ctx.TestingArea.Add(TA);
                        ctx.SaveChanges();
                    }

                    for (int i = 0; i < tests.Count; i++)
                    {
                        Test TS = new Test();
                        if (importdata.importproductusage == true)
                        {

                            List<ProductUsage> PU = new List<ProductUsage>();






                            var Product1 = ctx.MasterProduct.Where(b => importdata.Producttypeids.Contains(b.ProductTypeId) && b.UserId == userid).ToArray();
                            var productusage = ctx.ProductUsage.Where(b => b.TestId == tests[i].TestID ).ToList();
                            TS = ctx.Test.Where(b => b.TestName == tests[i].TestName && b.UserId == importdata.userid).FirstOrDefault();
                            if (TS != null)
                            {

                            }
                            else
                            {
                                TS = new Test();
                                TS.TestingAreaID = TA.TestingAreaID;
                                TS.TestingDuration = tests[i].TestingDuration;
                                TS.TestName = tests[i].TestName;
                                TS.TestType = tests[i].TestType;
                                TS.UserId = importdata.userid;
                                ctx.Test.Add(TS);
                                ctx.SaveChanges();
                            }
                            foreach (var item1 in productusage)
                            {
                                var instrument = ctx.Instrument.Where(b => b.InstrumentID == item1.InstrumentId).FirstOrDefault();
                                Instrument Ins = new Instrument();
                                if (instrument != null)
                                {
                                    Ins = ctx.Instrument.Where(b => b.InstrumentName == instrument.InstrumentName && b.UserId == importdata.userid).FirstOrDefault();
                                    if (Ins != null)
                                    {
                                        Ins.testingArea = TA;
                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        Ins = new Instrument();
                                        Ins.CtrlTestDuration = instrument.CtrlTestDuration;
                                        Ins.CtrlTestNoOfRun = instrument.CtrlTestNoOfRun;
                                        Ins.DailyCtrlTest = instrument.DailyCtrlTest;
                                        Ins.InstrumentName = instrument.InstrumentName;
                                        Ins.MaxTestBeforeCtrlTest = instrument.MaxTestBeforeCtrlTest;
                                        Ins.MaxThroughPut = instrument.MaxThroughPut;
                                        Ins.MonthlyCtrlTest = instrument.MonthlyCtrlTest;
                                        Ins.MonthMaxTPut = instrument.MonthMaxTPut;
                                        Ins.QuarterlyCtrlTest = instrument.QuarterlyCtrlTest;
                                        Ins.testingArea = TA;
                                        Ins.UserId = importdata.userid;
                                        Ins.WeeklyCtrlTest = instrument.WeeklyCtrlTest;
                                        ctx.Instrument.Add(Ins);
                                        ctx.SaveChanges();
                                    }
                                }


                                var Product = ctx.MasterProduct.Where(b => b.ProductID == item1.ProductId).FirstOrDefault();

                                var producttype = ctx.ProductType.Where(b => b.TypeID == Product.ProductTypeId).FirstOrDefault();

                                ProductType PT = new ProductType();
                              
                                PT = ctx.ProductType.Where(b => b.TypeName == producttype.TypeName && b.UserId == importdata.userid).FirstOrDefault();
                                if (PT != null)
                                {

                                }
                                else
                                {
                                    PT = new ProductType();
                                    PT.ClassOfTest = producttype.ClassOfTest;
                                    PT.Description = producttype.Description;
                                    PT.TypeName = producttype.TypeName;
                                    PT.UseInDemography = producttype.UseInDemography;
                                    PT.UserId = importdata.userid;
                                    ctx.ProductType.Add(PT);
                                    ctx.SaveChanges();

                                }
                                MasterProduct MP = new MasterProduct();
                                MP = ctx.MasterProduct.Where(b => b.ProductTypeId == PT.TypeID && b.ProductName == Product.ProductName && b.UserId == importdata.userid).FirstOrDefault();
                                if (MP != null)
                                {
                                    MP.ProductTypeId = PT.TypeID;
                                    ctx.SaveChanges();
                                }
                                else
                                {
                                    MP = new MasterProduct();
                                    MP.ProductName = Product.ProductName;
                                    MP.ProductNote = Product.ProductNote;
                                    MP.ProductTypeId = PT.TypeID;
                                    MP.RapidTestGroup = Product.RapidTestGroup;
                                    MP.SerialNo = Product.SerialNo;
                                    MP.SlowMoving = Product.SlowMoving;
                                    MP.Specification = Product.Specification;
                                    MP.UserId = importdata.userid;
                                    MP.MinimumPackPerSite = Product.MinimumPackPerSite;
                                    MP.BasicUnit = Product.BasicUnit;

                                    ctx.MasterProduct.Add(MP);
                                    ctx.SaveChanges();
                                }



                                PU.Add(new ProductUsage
                                {
                                    InstrumentId = Ins.InstrumentID,
                                    IsForControl = item1.IsForControl,
                                    ProductId = MP.ProductID,
                                    ProductUsedIn = item1.ProductUsedIn,
                                    Rate = item1.Rate,
                                    TestId = TS.TestID,
                                    UserId = importdata.userid
                                });
                            }

                            ctx.ProductUsage.AddRange(PU);
                            ctx.SaveChanges();
                        }
                        else
                        {

                            TS.TestingAreaID = tests[i].TestingAreaID;
                            TS.TestingDuration = tests[i].TestingDuration;
                            TS.TestName = tests[i].TestName;
                            TS.TestType = tests[i].TestType;
                            TS.UserId = importdata.userid;
                            ctx.Test.Add(TS);
                            ctx.SaveChanges();
                        }

                    }
                }
            }

            if (importdata.importproduct == true)
            {///insert product

                var Producttype = ctx.ProductType.Where(b => importdata.Producttypeids.Contains(b.TypeID) && b.UserId == userid).ToList();
                foreach (var item in Producttype)
                {
                    ProductType PT = new ProductType();
                    var Product = ctx.MasterProduct.Where(b => b.ProductTypeId == item.TypeID && b.UserId == userid).ToList();
                    PT = ctx.ProductType.Where(b => b.TypeName == item.TypeName && b.UserId == importdata.userid).FirstOrDefault();
                    if (PT != null)
                    {

                    }
                    else
                    {
                        PT = new ProductType();
                        PT.ClassOfTest = item.ClassOfTest;
                        PT.Description = item.Description;
                        PT.TypeName = item.TypeName;
                        PT.UseInDemography = item.UseInDemography;
                        PT.UserId = importdata.userid;
                        ctx.ProductType.Add(PT);
                        ctx.SaveChanges();

                    }
                    for (int i = 0; i < Product.Count; i++)
                    {
                        MasterProduct MP = new MasterProduct();
                        var productprice = ctx.ProductPrice.Where(b => b.ProductId == Product[i].ProductID).ToList();
                        MP = ctx.MasterProduct.Where(b => b.ProductTypeId == PT.TypeID && b.ProductName == Product[i].ProductName && b.UserId == importdata.userid).FirstOrDefault();
                        if (MP != null)
                        {

                        }
                        else
                        {
                            MP = new MasterProduct();
                            MP.ProductName = Product[i].ProductName;
                            MP.ProductNote = Product[i].ProductNote;
                            MP.ProductTypeId = PT.TypeID;
                            MP.RapidTestGroup = Product[i].RapidTestGroup;
                            MP.SerialNo = Product[i].SerialNo;
                            MP.SlowMoving = Product[i].SlowMoving;
                            MP.Specification = Product[i].Specification;
                            MP.UserId = importdata.userid;
                            MP.MinimumPackPerSite = Product[i].MinimumPackPerSite;
                            MP.BasicUnit = Product[i].BasicUnit;

                            ctx.MasterProduct.Add(MP);
                            ctx.SaveChanges();
                        }
                        List<ProductPrice> PP = new List<ProductPrice>();
                        foreach (var item1 in productprice)
                        {
                            PP.Add(new ProductPrice
                            {
                                FromDate = item1.FromDate,
                                PackSize = item1.PackSize,
                                Price = item1.Price,
                                ProductId = MP.ProductID,
                                UserId = importdata.userid

                            });
                        }

                        ctx.ProductPrice.AddRange(PP);
                        ctx.SaveChanges();
                    }
                }

            }
            //insert product usage


            if (importdata.importprogram == true)
            {
                var mmprogram = ctx.MMProgram.Where(b => importdata.Programids.Contains(b.Id) && b.UserId == userid).ToList();
                for (int i = 0; i < mmprogram.Count; i++)
                {
                    MMProgram MM = new MMProgram();

                    var generalassumption = ctx.MMGeneralAssumption.Where(b => b.ProgramId == mmprogram[i].Id).ToList();
                    var forecastparameter = ctx.MMForecastParameter.Where(b => b.ProgramId == mmprogram[i].Id).ToList();
                    var mmgroup = ctx.MMGroup.Where(b => b.ProgramId == mmprogram[i].Id).ToList();

                    MM = ctx.MMProgram.Where(b => b.ProgramName == mmprogram[i].ProgramName && b.UserId == importdata.userid).FirstOrDefault();
                    if (MM != null)
                    {

                    }
                    else
                    {
                        MM = new MMProgram();
                        MM.NoofYear = mmprogram[i].NoofYear;
                        MM.ProgramName = mmprogram[i].ProgramName;
                        MM.Rtp = mmprogram[i].Rtp;
                        MM.Description = mmprogram[i].Description;
                        MM.Gtp = mmprogram[i].Gtp;
                        MM.UserId = importdata.userid;

                        ctx.MMProgram.Add(MM);
                        ctx.SaveChanges();
                    }
                    List<MMGeneralAssumption> MGA = new List<MMGeneralAssumption>();
                    foreach (var item in generalassumption)
                    {
                        MGA.Add(new MMGeneralAssumption
                        {
                            AssumptionType = item.AssumptionType,
                            Entity_type_id = item.Entity_type_id,
                            IsActive = item.IsActive,
                            ProgramId = MM.Id,
                            UseOn = item.UseOn,
                            UserId = importdata.userid,
                            VarCode = item.VarCode,
                            VariableDataType = item.VariableDataType,
                            VariableEffect = item.VariableEffect,
                            VariableFormula = item.VariableFormula,
                            VariableName = item.VariableName

                        });

                    }

                    ctx.MMGeneralAssumption.AddRange(MGA);
                    ctx.SaveChanges();
                    List<MMForecastParameter> MMF = new List<MMForecastParameter>();
                    foreach (var item in forecastparameter)
                    {
                        MMF.Add(new MMForecastParameter
                        {
                            ForecastMethod = item.ForecastMethod,
                            Entity_type_id = item.Entity_type_id,
                            IsActive = item.IsActive,
                            ProgramId = MM.Id,
                            UseOn = item.UseOn,
                            UserId = importdata.userid,
                            VarCode = item.VarCode,
                            VariableDataType = item.VariableDataType,
                            VariableEffect = item.VariableEffect,
                            VariableFormula = item.VariableFormula,
                            VariableName = item.VariableName,
                            IsPrimaryOutput = item.IsPrimaryOutput

                        });
                    }
                    ctx.MMForecastParameter.AddRange(MMF);
                    ctx.SaveChanges();
                    List<MMGroup> MMG = new List<MMGroup>();
                    foreach (var item in mmgroup)
                    {
                        MMG.Add(new MMGroup
                        {
                            GroupName = item.GroupName,
                            IsActive = item.IsActive,
                            ProgramId = MM.Id,
                            UserId = importdata.userid

                        });
                    }
                    ctx.MMGroup.AddRange(MMG);
                    ctx.SaveChanges();

                }
            }
            return res;
        }
        public User Authenticate(string username, string password)
        {
            var user = ctx.User.Where(x => (x.Email == username || x.UserName==username) && x.Password == password).FirstOrDefault();

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())

                }),
                //Issuer = "http://localhost:53234",
                //Audience = "http://localhost:53234",


                Issuer = "https://forlab-174007.appspot.com",
                Audience = "https://forlab-174007.appspot.com",
                //Issuer = "http://forlab.aspwork.co.in/webapi",
                //Audience = "http://forlab.aspwork.co.in/webapi",

                //Issuer = "http://forlab.aspwork.co.in",
                //Audience = "http://forlab.aspwork.co.in",
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            if (user.Emailverify==null)
            {
                user.Emailverify = false;
            }
            // remove password before returning
            user.Password = null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {

            return ctx.User.ToList();// return users without passwords
            //return _users.Select(x => {
            //    x.Password = null;
            //    return x;
            //});
        }
    }


}
