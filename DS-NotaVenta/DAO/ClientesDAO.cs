using System;
using System.Collections.Generic;
using cl.riobaker.DAL.Data;
using System.Data;
using DS_NotaVenta.Models;


namespace DS_NotaVenta.DAO
{
    public class ClientesDAO : BaseDAO
    {

        public static List<ClientesModels> GetClientes(ClientesModels cliente = null)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "JS_ListarClientesCodAuxRut", CommandType.StoredProcedure))
                {
                    //dc.parameters.AddWithValue("vchrRutAux", cliente.RutAux);
                    dc.parameters.AddWithValue("vchrCodAux", cliente.CodAux);
                    return dc.executeQuery<ClientesModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }

        public static List<ClientesModels> GetCliente(ClientesModels cliente = null)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "JS_ListarClientesCodAux", CommandType.StoredProcedure))
                {
                    dc.parameters.AddWithValue("vchrCodAux", cliente.CodAux);
                    return dc.executeQuery<ClientesModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }


        public static List<ClientesModels> listarClientes()
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_ListarClientes", CommandType.StoredProcedure))
                {
                    return dc.executeQuery<ClientesModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }

        public static List<ClientesModels> BuscarClientes(ClientesModels clientes)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_BuscarClientes", CommandType.StoredProcedure))
                {
                    dc.parameters.AddWithValue("RutAux", clientes.RutAux);
                    dc.parameters.AddWithValue("CodAux", clientes.CodAux);
                    return dc.executeQuery<ClientesModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }

        public static List<ClientesModels> ListarClientesTodos()
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_ListarClientesTodos", CommandType.StoredProcedure))
                {
                    return dc.executeQuery<ClientesModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }

        //Procedimiento devuelve clientes por su id//
        public static List<ClientesModels> BuscarMisClientes(ClientesModels RutAux)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_ListarMisClientes", CommandType.StoredProcedure))
                {
                    dc.parameters.AddWithValue("ID", RutAux.ID);

                    return dc.executeQuery<ClientesModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }


        //Busca los clientes por el VenCod del Usuario//
        public static List<ClientesModels> BuscarMisClientesVenCod(UsuariosModels usuario)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "JS_ListarMisClientes", CommandType.StoredProcedure))
                {
                    dc.parameters.AddWithValue("cod", usuario.VenCod);
                    dc.parameters.AddWithValue("ID", usuario.id);

                    return dc.executeQuery<ClientesModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }

        public static List<ClientesModels> BuscarContacto(ClientesModels contacto)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_ListarContactos", CommandType.StoredProcedure))
                {
                    dc.parameters.AddWithValue("CodAuc", contacto.CodAux);
                    dc.parameters.AddWithValue("NomCon", contacto.NomAux);
                    return dc.executeQuery<ClientesModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }

        public static List<ClientesModels> AgregarContacto(ClientesModels contacto)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_AgregarContactos", CommandType.StoredProcedure))
                {
                    dc.parameters.AddWithValue("CodAux", contacto.CodAux);
                    dc.parameters.AddWithValue("NomCon", contacto.NomCon);
                    return dc.executeQuery<ClientesModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }

        public static List<DireccionDespachoModels> BuscarDirecDespach(DireccionDespachoModels DirDes)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_BuscarDirecDespa", CommandType.StoredProcedure))
                {
                    dc.parameters.AddWithValue("CodAxD", DirDes.CodAxD);

                    return dc.executeQuery<DireccionDespachoModels>();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }

        public static bool ActualizarCliente(ClientesModels cliente)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_ActualizarCliente", CommandType.StoredProcedure))
                {
                    dc.parameters.AddWithValue("CodAux", cliente.CodAux);
                    dc.parameters.AddWithValue("RutAux", cliente.RutAux);
                    dc.parameters.AddWithValue("NomAux", cliente.NomAux);
                    dc.parameters.AddWithValue("DirAux", cliente.DirAux);
                    dc.parameters.AddWithValue("NomCon", cliente.NomCon);
                    dc.parameters.AddWithValue("FonCon", cliente.FonCon);

                    return dc.executeNonQuery();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return false;
            }
        }

        public static List<ClientesModels> ObtenerGiro()
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "SP_GET_Giro", CommandType.StoredProcedure))
                    return dc.executeQuery<ClientesModels>();
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
            
        }

        public static List<ClientesModels> ObtenerCiudad()
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "SP_GET_Ciudad", CommandType.StoredProcedure))
                    return dc.executeQuery<ClientesModels>();
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }

        }

        public static List<ClientesModels> ObtenerComuna()
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "SP_GET_Comuna", CommandType.StoredProcedure))
                    return dc.executeQuery<ClientesModels>();
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }

        }

        public static bool AgregarCliente(ClientesModels clientes)
        {
            try
            {
                using (DataContext dc = new DataContext(catalogo, "FR_AgregarCliente", CommandType.StoredProcedure))
                {
                    dc.parameters.AddWithValue("NomAux", clientes.NomAux);
                    dc.parameters.AddWithValue("RutAux", clientes.RutAux);
                    dc.parameters.AddWithValue("EMail", clientes.EMail);
                    dc.parameters.AddWithValue("GirAux", clientes.GirAux);
                    dc.parameters.AddWithValue("ComAux", clientes.ComAux);
                    dc.parameters.AddWithValue("CiuAux", clientes.CiuAux);
                    dc.parameters.AddWithValue("DirAux", clientes.DirAux);
                    dc.parameters.AddWithValue("FonAux1", clientes.FonAux1);
                    dc.parameters.AddWithValue("VenCod", clientes.VenCod);

                    return dc.executeNonQuery();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return false;
            }
        }



            //public static List<ClientesModels> AgregarCliente(ClientesModels clientes)
            //{
            //    try
            //    {
            //        using (DataContext dc = new DataContext(catalogo, "FR_AgregarCliente", CommandType.StoredProcedure))
            //        {
            //            dc.parameters.AddWithValue("NomAux", clientes.NomAux);
            //            dc.parameters.AddWithValue("RutAux", clientes.RutAux);
            //            dc.parameters.AddWithValue("EMail", clientes.EMail);
            //            dc.parameters.AddWithValue("GirAux", clientes.GirAux);
            //            dc.parameters.AddWithValue("ComAux", clientes.ComAux);
            //            dc.parameters.AddWithValue("CiuAux", clientes.CiuAux);
            //            dc.parameters.AddWithValue("DirAux", clientes.DirAux);
            //            dc.parameters.AddWithValue("FonAux1", clientes.FonAux1);
            //            dc.parameters.AddWithValue("VenCod", clientes.VenCod);

            //            return dc.executeQuery<ClientesModels>();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        string error = ex.ToString();
            //        return null;
            //    }
            //}

        }
}