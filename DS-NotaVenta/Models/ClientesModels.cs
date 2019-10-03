using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DS_NotaVenta.Models
{
    public class ClientesModels
    {
        public string CodAux { get; set; }
        public string NomAux { get; set; }
        public string RutAux { get; set; }
        public string FonAux1 { get; set; }
        public string DirAux { get; set; }
        public string DirNum { get; set; }
        public string NomCon { get; set; }
        public string FonCon { get; set; }

        public string EMail { get; set; }

        public int ID { get; set; }

        public string Notas { get; set; }

        public string Credito { get; set; }

        public decimal Deuda { get; set; }

        public decimal DeudaVencida { get; set; }


        public string VenCod { get; set; }
        public string VenDes { get; set; }
        public string CodTipV { get; set; }

        public string Usuario { get; set; }

        public string GirDes { get; set; }

        public string GirCod { get; set; }

        public string CiuCod { get; set; }

        public string CiuDes { get; set; }

        public string ComCod { get; set; }

        public string ComDes { get; set; }

        public string GirAux { get; set; }

        public string CiuAux { get; set; }

        public string ComAux { get; set; }
    }
}