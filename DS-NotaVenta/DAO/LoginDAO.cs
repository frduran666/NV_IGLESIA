using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using cl.riobaker.DAL.Data;
using DS_NotaVenta.Models;

namespace DS_NotaVenta.DAO
{
    public class LoginDAO : BaseDAO
    {
        public static List<UsuariosModels> Login(UsuariosModels usuario)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "SP_Login", CommandType.StoredProcedure))
                {
                    dc.parameters.AddWithValue("usuario", usuario.Usuario);
                    dc.parameters.AddWithValue("password", usuario.Password);
                    List<UsuariosModels>  usuariosModels = dc.executeQuery<UsuariosModels>();

                    return usuariosModels;
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