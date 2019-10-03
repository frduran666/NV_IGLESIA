using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DS_NotaVenta.DAO
{
    public class BaseDAO
    {
        public static string catalogo = ConfigurationManager.AppSettings.Get("Catalogo");
    }
}