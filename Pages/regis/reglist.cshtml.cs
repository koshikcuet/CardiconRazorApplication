using Cardicon.wwwroot.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Cardicon.Model;
using Newtonsoft.Json;

namespace Cardicon.Pages.regis
{
    [IgnoreAntiforgeryToken]
    public class reglistModel : PageModel
    {
        public List<xapi.registration> reglist = new List<xapi.registration>();
        
        public void OnGet()
        {
            ViewData["Log"] = "";

            using (IDbConnection cn = new SqlConnection(mLib.getCon()))
            {
                reglist = cn.Query<xapi.registration>("select * from registration where status<>'Deleted' order by atn").ToList();
            }
            if (HttpContext.Session.GetString("Log")==null)
            {
            } else
            {
                ViewData["Log"] = "Logged";
                ViewData["User"] = HttpContext.Session.GetString("User");
            }
            
        }
               

        public async void OnPostDelreg()
        {
            ViewData["Log"] = "";
            ViewData["User"] = "";
            xapi.registration dd = new xapi.registration();
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                var regn = JsonConvert.DeserializeObject<xapi.registration>(body);
                if (regn != null)
                {
                    using (IDbConnection cn = new SqlConnection(mLib.getCon()))
                    {
                        regn.dt = DateTime.Now;
                        regn.saveby = "";
                        if (HttpContext.Session.GetString("User") != null)
                        {
                            regn.saveby = HttpContext.Session.GetString("User");
                            await cn.ExecuteAsync("update registration set status='Deleted', saveby='" + regn.saveby + "', dt='" + DateTime.Now.ToString() + "' where atn=" + regn.atn);
                            ViewData["Log"] = "Logged";
                            ViewData["User"] = regn.saveby;
                        }
                        

                    }
                }
            }
        }



    }
}
