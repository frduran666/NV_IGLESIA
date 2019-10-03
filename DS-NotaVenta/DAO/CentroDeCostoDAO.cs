﻿using System;
using System.Collections.Generic;
using cl.riobaker.DAL.Data;
using System.Data;
using DS_NotaVenta.Models;

namespace DS_NotaVenta.DAO
{
    public class CentroDeCostoDAO : BaseDAO
    {
        public static List<CentrodeCostoModels> listarCC()
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_ListarCentroDeCosto", CommandType.StoredProcedure))
                {
                    return dc.executeQuery<CentrodeCostoModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }
    }
}