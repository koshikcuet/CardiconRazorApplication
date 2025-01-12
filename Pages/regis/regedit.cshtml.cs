using Cardicon.Model;
using Cardicon.wwwroot.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Cardicon.Pages.regis
{
    public class regeditModel : PageModel
    {
        public xapi.registration regad = new xapi.registration();
        public string success = "";
        public string errmsg = "";

        private IWebHostEnvironment Environment;
        public regeditModel(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        public void OnGet(long id)
        {
            using (IDbConnection cn = new SqlConnection(mLib.getCon()))
            {
                regad = cn.QuerySingle<xapi.registration>("select * from registration where status<>'Deleted' and atn=" + id.ToString());
            }
        }

        public void OnPost(xapi.registration regn)
        {

            if (isValid(regn) != "")
            {
                regad = regn;
                return;
            }

            try
            {
                regn.nam = regn.nam.ToUpper();
                if (regn.degree != null) { regn.degree = regn.degree.ToUpper(); } else { regn.degree = ""; }
                regn.institute = regn.institute.ToUpper();
                if (regn.designation != null) { regn.designation = regn.designation.ToUpper(); } else { regn.designation = ""; }
                regn.city   = regn.city.ToUpper();
                regn.country=   regn.country.ToUpper();
                regn.dt = DateTime.Now;

                regn.saveby = "";
                if (HttpContext.Session.GetString("User") != null)
                { 
                    regn.saveby = HttpContext.Session.GetString("User");
                }

                string inst = @"Update registration set title=@title, nam=@nam, degree=@degree, institute=@institute, designation=@designation, city=@city, country=@country, mobile=@mobile, email=@email, dt=@dt, bmdc=@bmdc, remark=@remark, tk=@tk, status=@status, cat=@cat,  saveby=@saveby Where atn=@atn";

                using (IDbConnection cn = new SqlConnection(mLib.getCon()))
                {
                    cn.Execute(inst, regn);
                    if (regn.postedFile == null || regn.postedFile.Length == 0)
                    {
                        
                    } else
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        //string contentPath = this.Environment.ContentRootPath;

                        string path = Path.Combine(this.Environment.WebRootPath, "docs");
                        string fileName = regn.atn.ToString() + "-slip-" + DateTime.Now.Millisecond.ToString() + Path.GetExtension(regn.postedFile.FileName);
                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            regn.postedFile.CopyTo(stream);
                        }
                        cn.Execute("Update registration set slip='" + fileName + "' where atn=" + regn.atn.ToString());
                    }
                        
                }
                
                success = "Registration Information is submitted Successfully.";

            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                regad = regn;
            }
            //RedirectToPage("/Index");
        }

        public string isValid(xapi.registration regn)
        {
            errmsg = "";
            if (regn.title == null || regn.title=="0")
            {
                errmsg = "Please Select Title.";
                goto Enresp;
            }
            if (regn.nam == null || regn.nam.Length < 5)
            {
                errmsg = "Please Enter Name.";
                goto Enresp;
            }
            //if (regn.degree == null || regn.degree.Length < 2)
            //{
            //    errmsg = "Please Enter Degree.";
            //    goto Enresp;
            //}
            if (regn.institute == null || regn.institute.Length < 0)
            {
                errmsg = "Please Enter Organization Name.";
                goto Enresp;
            }
            //if (regn.designation == null || regn.designation.Length < 2)
            //{
            //    errmsg = "Please Enter Present Position.";
            //    goto Enresp;
            //}
            if (regn.city == null || regn.city.Length < 2)
            {
                errmsg = "Please Enter City Name.";
                goto Enresp;
            }
            if (regn.country == null || regn.country.Length < 0)
            {
                errmsg = "Please Enter Country Name.";
                goto Enresp;
            }
            if (regn.mobile == null || regn.mobile.Length < 10)
            {
                errmsg = "Please Enter Mobile Number.";
                goto Enresp;
            }
            else
            {
                Regex regex = new Regex(@"^01[23456789]{1}\d{8}$");
                Match match = regex.Match(regn.mobile);
                if (match.Success == false)
                {
                    errmsg = "Please Enter Valid Mobile Number.";
                    goto Enresp;
                }
            }
            //if (regn.email == null || regn.email.Length < 5)
            //{
            //    errmsg = "Please Enter Email Number.";
            //    goto Enresp;
            //}
            //if (IsValidMail(regn.email) == false)
            //{
            //    errmsg = "Please Enter valid Email Number.";
            //    goto Enresp;
            //}
            if (regn.cat == "0")
            {
                errmsg = "Please Select Participant Category";
                goto Enresp;
            }
            if (regn.tk < 1000)
            {
                errmsg = "Please Enter Correct Payment Amount.";
                goto Enresp;
            }
            
Enresp:;
            return errmsg;

        }

        public bool IsValidMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
