using Microsoft.VisualBasic;
using System.Net;

namespace Cardicon.Model
{
    public class mLib
    {
        public static string getCon()
        {
            string conn = @"Data Source=.\SQLEXPRESS;Initial Catalog=cardicondb;Integrated Security=True";
            
            return conn;
        }

        public static string repInj(string tx, string mthd = "GET")
        {
            if (tx == null)
                return "";
            tx = tx.Replace("select ", "",StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("insert", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("delete", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("update", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("script", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("exec ", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("execute", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("alter ", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace(" union ", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("bit.ly", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("github", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace(">", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("<", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace("'", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace(" into ", "", StringComparison.OrdinalIgnoreCase);
            tx = tx.Replace(" not ", "", StringComparison.OrdinalIgnoreCase);
            if (mthd == "GET")
            {
                // tx = Microsoft.VisualBasic.Replace(tx, " and ", "",,, CompareMethod.Text)
                // tx = Microsoft.VisualBasic.Replace(tx, " or ", "",,, CompareMethod.Text)
                tx = tx.Replace("(", "", StringComparison.OrdinalIgnoreCase);
                tx = tx.Replace(")", "", StringComparison.OrdinalIgnoreCase);
            }
            return tx;

            
        }

        public string sendSMS(string mob, string msg)
        {
            //8809612442480
            
            using (WebClient wc = new WebClient())
            {
                string resp = wc.DownloadString(ad);
                if (resp.Contains("successfully"))
                {
                    resp= "success";
                } else
                {
                    resp = "error";
                }
                return resp;
            }
            
        }

    }
}
