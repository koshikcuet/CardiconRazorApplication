using Microsoft.AspNetCore.Mvc;
using Cardicon.Model;
using Dapper;
using Cardicon.wwwroot.Model;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cardicon
{
    [Route("api/[controller]")]
    [ApiController]
    public class wapiController : ControllerBase
    {
        
        // GET api/wapi/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return (id * 5).ToString();
        }

        [HttpGet("regsms/{id}")]
        public JsonResult sendsms(int id) //JsonResult, ActionResult<>, Ok
        {
            mLib cf = new mLib();
            xapi.registration regad = new xapi.registration();
            using (IDbConnection cn = new SqlConnection(mLib.getCon()))
            {
                try
                {
                   regad = cn.QuerySingle<xapi.registration>("select * from registration where status<>'Deleted' and atn=" + id.ToString());
                    string msg = "Dear " + regad.nam + ","  + Environment.NewLine + "your registration for CARDICON CTG 2023 is successful. BDT " + regad.tk + " is Received."  + Environment.NewLine + "Your Registraion ID: CNC-" + regad.atn.ToString().PadLeft(3, '0') + Environment.NewLine + Environment.NewLine + "Thank You.";
                   regad.nam = cf.sendSMS(regad.mobile, msg);
                    if (regad.nam == "success")
                    {
                        regad.nam = "SMS Sent Successfully.";
                    } else
                    {
                        throw new Exception("SMS Error");
                    }
                } catch(Exception e)
                {
                    regad.nam = "invalid";
                }
                
            }
                       
            return new JsonResult(regad);
        }

        // POST api/<wapiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

       

        
    }
}
