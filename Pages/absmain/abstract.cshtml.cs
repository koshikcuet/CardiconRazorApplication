using Cardicon.Model;
using Cardicon.wwwroot.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Dapper;
using Newtonsoft.Json;

namespace Cardicon.Pages.absmain
{
    //[IgnoreAntiforgeryToken]
    public class abstractModel : PageModel
    {
        public xapi.abstracts abst = new xapi.abstracts();
        public string success = "";
        public string errmsg = "";
        
        private IWebHostEnvironment Environment;
        public abstractModel(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        public void OnGet()
        {
        }
        public JsonResult OnGetFind(string mobile)
        {
            xapi.registration reginfo = new xapi.registration();
            using (IDbConnection cn = new SqlConnection(mLib.getCon()))
            {
                reginfo = cn.QuerySingleOrDefault<xapi.registration>("select * from registration where mobile='" + mobile + "' and status='Confirmed'");
                
                if (reginfo == null)
                {
                    reginfo = new xapi.registration();
                    reginfo.nam = "invalid";
                }
            }
            return new JsonResult(reginfo);
        }

        public void OnPost(xapi.abstracts nwabs)
        {
            
            if (nwabs.nam == "")
            {
                errmsg = "Please Enter valid Mobile number and Click on Find button.";
                return;
            }
            

            if (isValid(nwabs) != "")
            {  
                abst = nwabs;
                return;
            }

            try
            {
                nwabs.abs_head  = nwabs.abs_head.ToUpper();
                nwabs.dt = DateTime.Now;
                nwabs.status = "Pending";

                string inst = @"Insert into abstracts (reg_no, nam, mobile, email, abs_cat, abs_head, abs_desc, status, dt) OUTPUT INSERTED.[atn] Values (@reg_no, @nam, @mobile, @email, @abs_cat, @abs_head, @abs_desc, @status, @dt)";

                using (IDbConnection cn = new SqlConnection(mLib.getCon()))
                {
                    xapi.registration reginfo = cn.QuerySingleOrDefault<xapi.registration>("select * from registration where mobile='" + nwabs.mobile + "' and status='Confirmed'");
                    if (reginfo != null)
                    {
                        nwabs.reg_no = (int)reginfo.atn;
                        nwabs.email = reginfo.email;
                        nwabs.nam = reginfo.title + " " + reginfo.nam;
                    } else
                    {
                        throw new Exception("This mobile number is not registered");
                    }

                    long id = cn.QuerySingle<long>(inst, nwabs);
                    if (id > 0)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string path = Path.Combine(this.Environment.WebRootPath, "docs");
                        string fileName = id.ToString() + "-abstract-" + DateTime.Now.Millisecond.ToString() + Path.GetExtension(nwabs.postedFile.FileName);
                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            nwabs.postedFile.CopyTo(stream);
                        }
                        cn.Execute("Update abstracts set abs_link='" + fileName + "' where atn=" + id.ToString());
                    }
                }

                success = "Abstract is submitted for review.";

            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                abst = nwabs;
            }
        }

        public string isValid(xapi.abstracts  nwabs)
        {
            errmsg = "";
            if (nwabs.abs_cat == null || nwabs.abs_cat == "")
            {
                errmsg = "Please Select Presentation Type.";
                goto Enresp;
            }
            if (nwabs.abs_head == null || nwabs.abs_head.Length < 5)
            {
                errmsg = "Please Enter Abstract Name.";
                goto Enresp;
            }
            if (nwabs.abs_desc == null || nwabs.abs_desc.Length < 0)
            {
                errmsg = "Please Enter a short description.";
                goto Enresp;
            }
                        
            if (nwabs.postedFile == null || nwabs.postedFile.Length == 0)
            {
                errmsg = "Please Upload Abstract (PDF / PowerPoint / MS Word Document)";
                goto Enresp;
            }
Enresp:;
            return errmsg;

        }

        
    }
}
