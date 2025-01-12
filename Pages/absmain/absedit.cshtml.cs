using Cardicon.Model;
using Cardicon.wwwroot.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Dapper;

namespace Cardicon.Pages.absmain
{
    public class abseditModel : PageModel
    {
        public xapi.abstracts abst = new xapi.abstracts();
        public string success = "";
        public string errmsg = "";

        private IWebHostEnvironment Environment;
        public abseditModel(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        public void OnGet(long id)
        {
            using (IDbConnection cn = new SqlConnection(mLib.getCon()))
            {
                abst = cn.QuerySingle<xapi.abstracts>("select * from abstracts where status<>'Deleted' and atn=" + id.ToString());
            }
        }

        public void OnPost(xapi.abstracts nwabs)
        {

            if (isValid(nwabs) != "")
            {
                abst = nwabs;
                return;
            }

            try
            {
                nwabs.abs_head  = nwabs.abs_head.ToUpper();
                nwabs.dt = DateTime.Now;
                
                nwabs.saveby = "";
                if (HttpContext.Session.GetString("User") != null)
                {
                    nwabs.saveby = HttpContext.Session.GetString("User");
                }

                string inst = @"Update abstracts set abs_cat=@abs_cat, abs_head=@abs_head, abs_desc=@abs_desc, status=@status, saveby=@saveby, dt=@dt Where atn=@atn";

                using (IDbConnection cn = new SqlConnection(mLib.getCon()))
                {
                    cn.Execute(inst, nwabs);
                    if (nwabs.postedFile == null || nwabs.postedFile.Length == 0)
                    {

                    }
                    else
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        //string contentPath = this.Environment.ContentRootPath;

                        string path = Path.Combine(this.Environment.WebRootPath, "docs");
                        string fileName = nwabs.atn.ToString() + "-abstract-" + DateTime.Now.Millisecond.ToString() + Path.GetExtension(nwabs.postedFile.FileName);
                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            nwabs.postedFile.CopyTo(stream);
                        }
                        cn.Execute("Update abstracts set abs_link='" + fileName + "' where atn=" + nwabs.atn.ToString());
                    }

                }

                success = "Abstract Information is submitted Successfully.";

            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                abst = nwabs;
            }
            //RedirectToPage("/Index");
        }

        public string isValid(xapi.abstracts nwabs)
        {
            errmsg = "";
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
                        
Enresp:;
            return errmsg;

        }
    }
}
