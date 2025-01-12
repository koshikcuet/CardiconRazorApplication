using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cardicon.Model;
using Cardicon.wwwroot.Model;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Dapper;

namespace Cardicon.Pages
{
    [IgnoreAntiforgeryToken]
    public class loginModel : PageModel
    {
        public string errmsg = "";
        //[BindProperty]
        //public xapi.registration ddq { get; set; }

        //[ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public void OnGet()
        {
        }
        public void OnPost() {
            string usr = "" + Request.Form["usr"];
            string pss = "" + Request.Form["pass"];
            usr = mLib.repInj(usr);
            pss = mLib.repInj(pss);
            //System.Diagnostics.Debug.WriteLine("Here I");
            using (IDbConnection cn = new SqlConnection(mLib.getCon()))
            {
                try
                {
                    
                    string nam = cn.ExecuteScalar<string>("select usr from [cardicondb].[cardicondb_usr].[login] where usr='" + usr + "' and pass='" + pss + "'");
                    if (nam == null || nam == "") 
                    {
                        throw new Exception("Invalid User name or Password");
                    } else
                    {
                        HttpContext.Session.SetString("User", usr);
                        HttpContext.Session.SetString("Log", "Logged");
                        ViewData["Log"] = "Logged";
                        ViewData["User"] = usr;
                        errmsg= "Logged In";
                        //RedirectToPage("/regis/reglist");
                    }
                } catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
            }
            //return null;
        }



        //public async Task<IActionResult> OnPostGetTime()
        //{
        //    xapi.registration dd = new xapi.registration();
        //    using (var reader = new StreamReader(Request.Body))
        //    {
        //        var body = await reader.ReadToEndAsync();
        //        var obj = JsonConvert.DeserializeObject<xapi.registration>(body);
        //                if (obj != null)
        //                {
        //                    dd.nam = obj.nam;
        //                    //dd.lnam = obj.lnam;
        //                }
        //    }
        //    if (HttpContext.Session.GetString("User") == null)
        //    {
        //        //dd.lnam = ddq.lnam;
        //    } else
        //    {
        //        dd.nam = HttpContext.Session.GetString("User");
        //    }
        //    return new JsonResult(dd);
        //}

        //public IActionResult OnGetReg(string nam, string lnm)
        //{
        //    xapi.registration dd = new xapi.registration();
        //    dd.nam = nam;
        //    //dd.lnam = lnm;

        //    return new JsonResult(dd);
             
        //}

    }
}
