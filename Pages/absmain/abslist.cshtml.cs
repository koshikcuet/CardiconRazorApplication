using Cardicon.Model;
using Cardicon.wwwroot.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace Cardicon.Pages.absmain
{
    [IgnoreAntiforgeryToken]
    public class abslistModel : PageModel
    {
        public List<xapi.abstracts> ablist = new List<xapi.abstracts>();

        public void OnGet()
        {
            ViewData["Log"] = "";
            using (IDbConnection cn = new SqlConnection(mLib.getCon()))
            {
                ablist = cn.Query<xapi.abstracts>("select * from abstracts where status<>'Deleted' order by atn desc").ToList();
            }
            if (HttpContext.Session.GetString("Log")==null)
            {
            }
            else
            {
                ViewData["Log"] = "Logged";
                ViewData["User"] = HttpContext.Session.GetString("User");
            }

        }


        public async void OnPostDelabs()
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
                            await cn.ExecuteAsync("update abstracts set status='Deleted', saveby='" + regn.saveby + "', dt='" + DateTime.Now.ToString() + "' where atn=" + regn.atn);
                            ViewData["Log"] = "Logged";
                            ViewData["User"] = regn.saveby;
                        }


                    }
                }
            }
        }


    }
}
