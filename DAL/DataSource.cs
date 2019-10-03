using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DAL
{
    public class DataSource
    {
        public static string coneccionPrimaria = (ConfigurationManager.ConnectionStrings["Disofi"].ConnectionString);
        //public static string coneccionPrimaria = (ConfigurationManager.ConnectionStrings["DSNOTAVENTA"].ConnectionString);
        public static bool cache;

        public static void SetParametros(string conn1)
        {
            //coneccionPrimaria = "Initial Catalog=DSNV;Data Source=srv-disofi;User ID=sa;password=Softland2018";
            //coneccionPrimaria = "Initial Catalog=DSNV;Data Source=SRV_SOFTLAND\\SOSQL2016;User ID=sa;password=Softland00";
        }

        public static void SetCache(bool hasCache)
        {
            cache = hasCache;
        }

        public string ConeccionPrimaria { get { return coneccionPrimaria; } }
    }
}
