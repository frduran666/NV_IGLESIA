using DAL;
using ET.Objetos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    class AccesoDatos
    {
        public List<_NotaDeVentaDetalleModels> DatosCorreoVend(int NvNUmero)
        {
            var DatosUser = new List<_NotaDeVentaDetalleModels>();
            try
            {
                var data = new DBConector().EjecutarProcedimientoAlmacenado("SP_GET_DatosCorreoVend", new System.Collections.Hashtable()
                                                                                            {
                                                                                                {"NvNumero", NvNUmero}
                                                                                            });
                if (data.Rows.Count > 0)
                {
                    for (var i = 0; i < data.Rows.Count; i++)
                    {
                        var validador = new object();
                        var resultadoListado = new _NotaDeVentaDetalleModels();

                        validador = data.Rows[i].Field<object>("email");
                        resultadoListado.EmailVend = validador != null ? data.Rows[i].Field<string>("email") : "NO ASIGNADO";

                        validador = data.Rows[i].Field<object>("ContrasenaCorreo");
                        resultadoListado.PassCorreo = validador != null ? data.Rows[i].Field<string>("ContrasenaCorreo") : "NO ASIGNADO";

                        DatosUser.Add(resultadoListado);
                    }
                }
            }
            catch (SqlException ex)
            {
                new CapturaExcepciones(ex);
            }
            catch (Exception ex)
            {
                new CapturaExcepciones(ex);
            }
            return DatosUser;
        }

        public int ActualizaCorreo(_UsuariosModels Usuario)
        {
            int respuesta = 0;
            try
            {
                var data = new DBConector().EjecutarProcedimientoAlmacenado("DS_SET_ActualizaCorreo", new System.Collections.Hashtable()
                                                                                            {
                                                                {"VendCod", Usuario.VenCod.Trim()},
                                                                {"Email", Usuario.email},
                                                                {"Contrasena", Usuario.Password}
                });
                if (data.Rows.Count > 0)
                {
                    respuesta = int.Parse(data.Rows[0][0].ToString());
                }
            }
            catch (SqlException ex)
            {
                new CapturaExcepciones(ex);
            }
            catch (Exception ex)
            {
                new CapturaExcepciones(ex);
            }
            return respuesta;
        }

    }
}
