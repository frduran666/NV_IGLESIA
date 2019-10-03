﻿using System;
using System.Collections.Generic;
using cl.riobaker.DAL.Data;
using System.Data;
using DS_NotaVenta.Models;

namespace DS_NotaVenta.DAO
{
    public class UsuariosTiposDAO : BaseDAO
    {
        public static List<UsuariosTiposModels> listarTipo()
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_ListarUsuariosTipos", CommandType.StoredProcedure))
                {
                    return dc.executeQuery<UsuariosTiposModels>();
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