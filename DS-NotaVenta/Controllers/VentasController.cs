using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DS_NotaVenta.Models;
using DS_NotaVenta.DAO;
using ET.Objetos;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using System.IO;
using System.Web;
using System.Data.SqlClient;
using System.Globalization;
using BLL;
using System.Linq;
using DS_NotaVenta.Util.Archivo;

namespace DS_NotaVenta.Controllers
{
    public class VentasController : Controller
    {
        public ActionResult AddCliente()
        {
            ClientesModels clientes = new ClientesModels();

            IEnumerable<SelectListItem> clientesGiro = ClientesDAO.ObtenerGiro().Select(c => new SelectListItem()
            {
                Text = c.GirDes,
                Value = c.GirCod
            }).ToList();
            ViewBag.Giro = clientesGiro;

            IEnumerable<SelectListItem> clientesCiudad = ClientesDAO.ObtenerCiudad().Select(c => new SelectListItem()
            {
                Text = c.CiuDes,
                Value = c.CiuCod
            }).ToList();
            ViewBag.Ciudad = clientesCiudad;

            IEnumerable<SelectListItem> clientesComuna = ClientesDAO.ObtenerComuna().Select(c => new SelectListItem()
            {
                Text = c.ComDes,
                Value = c.ComCod
            }).ToList();
            ViewBag.Comuna = clientesComuna;

            return View();
        }

        [HttpPost]
        public ActionResult AddCliente(string _NomAux, string _RutAux, string _Email, string _GirAux,
            string _ComAux, string _CiuAux, string _FonAux1, string _DirAux)
        {
            {
                var validador = false;
                try
                {
                    if (!string.IsNullOrEmpty(_NomAux) && !string.IsNullOrEmpty(_RutAux) && !string.IsNullOrEmpty(_Email)
                        && !string.IsNullOrEmpty(_GirAux) && !string.IsNullOrEmpty(_ComAux) && !string.IsNullOrEmpty(_CiuAux))
                    {
                        var Clientes = new ClientesModels()
                        {
                            NomAux = _NomAux,
                            RutAux = _RutAux,
                            EMail = _Email,
                            GirAux = _GirAux,
                            ComAux = _ComAux,
                            CiuAux = _CiuAux,
                            FonAux1 = _FonAux1,
                            DirAux = _DirAux,
                            VenCod = Session["VenCod"].ToString(),
                        };

                        validador = ClientesDAO.AgregarCliente(Clientes);

                    }
                    else
                    {
                        validador = false;
                    }
                }
                catch (Exception ex)
                {
                    var error = ex.ToString();
                    validador = false;
                    throw;
                }

                return (Json(validador));
            }
        }


        //[HttpPost, ValidateInput(false)]
        //public ActionResult AddCliente(FormCollection frm)
        //{
        //    ClientesModels Clientes = new ClientesModels();
        //    Clientes.NomAux = Request.Form["NomAux"];
        //    Clientes.RutAux = Request.Form["RutAux"];
        //    Clientes.EMail = Request.Form["Email"];
        //    Clientes.GirAux = Request.Form["GirAux"];
        //    Clientes.ComAux = Request.Form["ComAux"];
        //    Clientes.CiuAux = Request.Form["CiuAux"];
        //    Clientes.FonAux1 = Request.Form["FonAux1"];
        //    Clientes.DirAux = Request.Form["DirAux"];
        //    Clientes.VenCod = Session["VenCod"].ToString();

        //    List<ClientesModels> AddClientes = ClientesDAO.AgregarCliente(Clientes);
        //    if (AddClientes.Count == 0 || AddClientes == null)
        //    {
        //        TempData["Mensaje"] = "ERROR - CLIENTE EXISTENTE. <br>";
        //        return RedirectToAction("AddCliente", "Ventas");
        //    }
        //    else
        //    {
        //        TempData["Mensaje"] = "CLIENTE CREADO EXITOSAMENTE. <br>";
        //        return RedirectToAction("MisClientes", "Ventas");
        //    }
        //    //return RedirectToAction("AddCliente", "Ventas");
        //}


        public String ImagePath
        {
            get
            {
                return "~/DNCAMARON/Image/logo.JPG";
            }

        }

        string MakeImageSrcData(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] filebytes = new byte[fs.Length];
            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
            return "data:image/png;base64," +
              Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
        }

        public ActionResult Todolosclientes()
        {
            List<ClientesModels> lclientes = new List<ClientesModels>();
            var todosClientes = ClientesDAO.ListarClientesTodos();

            if (lclientes != null)
            {
                lclientes = todosClientes;
            }

            ViewBag.clientes = lclientes;

            return View();
        }

        public ActionResult MisClientes(string cod, int ID)
        {
            UsuariosModels usr = new UsuariosModels();

            List<ClientesModels> lclientes = new List<ClientesModels>();


            usr.VenCod = cod;
            usr.id = ID;
            var misClientes = ClientesDAO.BuscarMisClientesVenCod(usr);

            if (misClientes != null)
            {
                lclientes = misClientes;
            }

            ViewBag.clientes = lclientes;

            return View();
        }

        public ActionResult NotaDeVenta(string CodAux, string NomAux, string anterior)
        {
            NotadeVentaCabeceraModels NVC = new NotadeVentaCabeceraModels();
            ViewBag.CodAux = CodAux;
            ViewBag.NomAux = NomAux;
            ViewBag.Anterior = anterior;
            NVC.NVNumero = 0;
            NVC.NumOC = "";
            NVC.NumReq = 0;

            ClientesModels cliente = new ClientesModels
            {
                CodAux = CodAux
            };

            List<ClientesModels> clientes = ClientesDAO.GetClientes(cliente);

            ViewBag.CorreoCliente = clientes[0].EMail;

            //Se agrega la cabecera
            List<NotadeVentaCabeceraModels> lnvc = NotaDeVentaDAO.AgregarNV(NVC);


            ViewBag.numeronota = lnvc;

            CondicionVentasModels conven = new CondicionVentasModels();

            conven.CodAux = CodAux.ToString();

            //Se lista(n) la(s) condicion(es) de venta(s)
            List<CondicionVentasModels> lcondicion = CondicionesVentaDAO.listarConVen(conven);

            ViewBag.condicion = lcondicion;

            ClientesModels contacto = new ClientesModels();

            contacto.CodAux = CodAux.ToString();
            contacto.NomAux = Session["nombre"].ToString();

            //Se ubica la lista de contactos
            List<ClientesModels> contactos = ClientesDAO.BuscarContacto(contacto);

            if (contactos == null)
            {
                ViewBag.contactos = contactos;
                ViewBag.vcontactos = 0;
            }
            else
            {
                ViewBag.contactos = contactos;
                ViewBag.vcontactos = 1;
            }

            DireccionDespachoModels direc = new DireccionDespachoModels();

            direc.CodAxD = CodAux.ToString();

            //Se lista(n) la(s) dirección(es) de despacho
            List<DireccionDespachoModels> direciones = ClientesDAO.BuscarDirecDespach(direc);

            if (direciones == null)
            {
                ViewBag.vdirecc = 0;
            }
            else
            {
                ViewBag.vdirecc = 1;
            }

            ViewBag.direccion = direciones;
            ViewBag.codigo = CodAux;
            ViewBag.nombre = NomAux;

            if (anterior == "1")
            {
                ViewBag.anterior = "Mis Clientes";
                ViewBag.page = "Misclientes";
            }
            else if (anterior == "2")
            {
                ViewBag.anterior = "Mis Clientes";
                ViewBag.page = "";
            }
            else if (anterior == "3")
            {
                ViewBag.anterior = "Ruta";
                ViewBag.page = "";
            }

            ListaDePrecioModels ListPrecio = new ListaDePrecioModels();

            ListPrecio.CodAux = CodAux.ToString();

            //Se listan los precios
            //List<ListaDePrecioModels> ListDePrecios = ListaDePrecioDAO.listarListaDePrecio(ListPrecio);

            //ViewBag.lista = ListDePrecios;

            //Se listan los centros de costos
            List<CentrodeCostoModels> lcc = CentroDeCostoDAO.listarCC();
            ViewBag.cc = lcc;

            //NumNV Softland
            //List<NotadeVentaCabeceraModels> NVSoft = NotaDeVentaDAO.BuscarNvNumSoft();
            //ViewBag.NVnum = NVSoft[0].NVNumero;

            return View();
        }

        public void Detallenv(string CodProd, string DetProd, double nvCant, string CodUMed, double nvPrecio, string CodLista,
        double nvSubTotal, double nvTotLinea, int NVNumero, double nvLinea, DateTime nvFecCompr)
        {
            double porcentajefinal = 0;
            double descuento = 0;

            if (nvTotLinea != nvSubTotal)
            {
                porcentajefinal = 100 - ((nvTotLinea * 100) / nvSubTotal);
                descuento = nvSubTotal - nvTotLinea;
            }

            #region"NVD"
            var NVD = new NotaDeVentaDetalleModels
            {

                CodProd = CodProd,
                NVNumero = NVNumero,
                nvLinea = nvLinea,
                nvCorrela = 0,
                nvFecCompr = nvFecCompr,
                nvCant = nvCant,
                nvPrecio = nvPrecio,
                nvEquiv = 1,
                nvSubTotal = nvSubTotal,
                nvDPorcDesc01 = porcentajefinal,
                nvDDescto01 = descuento,
                nvDPorcDesc02 = 0,
                nvDDescto02 = 0,
                nvDPorcDesc03 = 0,
                nvDDescto03 = 0,
                nvDPorcDesc04 = 0,
                nvDDescto04 = 0,
                nvDPorcDesc05 = 0,
                nvDDescto05 = 0,
                nvTotDesc = 0,
                nvTotLinea = nvTotLinea,
                nvCantDesp = 0,
                nvCantProd = 0,
                nvCantFact = 0,
                nvCantDevuelto = 0,
                nvCantNC = 0,
                nvCantBoleta = 0,
                nvCantOC = 0,
                DetProd = DetProd,
                CheckeoMovporAlarmaVtas = "N",
                KIT = "",
                CodPromocion = 0,
                CodUMed = CodUMed,
                CantUVta = nvCant,
                Partida = "",
                Pieza = ""
            };
            #endregion

            List<NotaDeVentaDetalleModels> nv = NotaDeVentaDAO.DetalleNV(NVD);
        }

        //[NonAction]
        //public void VerificationEmail(int nvnumero)
        //{
        //    Control Acceso = new Control();
        //    var de = "";
        //    var clavecorreo = "";
        //    IEnumerable<_NotaDeVentaDetalleModels> datosUser = Acceso.DatosCorreoVend(nvnumero);
        //    foreach (_NotaDeVentaDetalleModels ot in datosUser)
        //    {
        //        de = ot.EmailVend;
        //        clavecorreo = ot.PassCorreo;
        //    }


        //    string to = System.Configuration.ConfigurationManager.AppSettings.Get("Para");
        //    string from = de;
        //    string displayName = System.Configuration.ConfigurationManager.AppSettings.Get("Remitente");
        //    string password = clavecorreo;
        //    string host = System.Configuration.ConfigurationManager.AppSettings.Get("Host");
        //    int port = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Port"));
        //    bool enableSs1 = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("EnableSsl"));
        //    bool useDefaultCredentials = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("UseDefaultCredentials"));


        //    var fromEmail = new MailAddress(from, displayName);
        //    var toEmail = new MailAddress(to);

        //    var smtp = new SmtpClient
        //    {
        //        Host = host,
        //        Port = port,
        //        EnableSsl = enableSs1,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = useDefaultCredentials,
        //        Credentials = new NetworkCredential(fromEmail.Address, password)
        //    };


        //    MailMessage mailWithImg = GetMailWithImg(nvnumero);
        //    if (mailWithImg != null)
        //    {
        //        smtp.Send(mailWithImg);
        //        //smtp.SendAsync(mailWithImg, mailWithImg);
        //    }
        //}

        //private MailMessage GetMailWithImg(int nvnumero)
        //{
        //    Control Acceso = new Control();
        //    var de = "";
        //    var clavecorreo = "";
        //    IEnumerable<_NotaDeVentaDetalleModels> datosUser = Acceso.DatosCorreoVend(nvnumero);
        //    foreach (_NotaDeVentaDetalleModels ot in datosUser)
        //    {
        //        de = ot.EmailVend;
        //        clavecorreo = ot.PassCorreo;
        //    }
        //    string from = de;
        //    string subject = string.Format("Nota de Venta {0}", nvnumero);

        //    NotadeVentaCabeceraModels NVentaCabecera = new NotadeVentaCabeceraModels
        //    {
        //        NVNumero = nvnumero
        //    };
        //    List<NotadeVentaCabeceraModels> NVentaCabeceras = NotaDeVentaDAO.BuscarNVPorNumero(NVentaCabecera);

        //    List<NotaDeVentaDetalleModels> NVentaDetalles = NotaDeVentaDAO.BuscarNVDETALLEPorNumero(NVentaCabecera);

        //    ClientesModels cliente = new ClientesModels
        //    {
        //        CodAux = NVentaCabeceras[0].CodAux
        //    };

        //    List<ClientesModels> clientes = ClientesDAO.GetClientes(cliente);

        //    ClientesModels Vendedor = new ClientesModels
        //    {
        //        VenCod = NVentaCabeceras[0].VenCod
        //    };

        //    List<ClientesModels> vendedores = VendedoresSoftlandDAO.GetVendedores(Vendedor);

        //    MailMessage mail = new MailMessage
        //    {
        //        IsBodyHtml = true
        //    };

        //    mail.AlternateViews.Add(GetEmbeddedImage(NVentaCabeceras, NVentaDetalles, clientes));
        //    mail.From = new MailAddress(from);

        //    if (clientes != null)
        //    {
        //        mail.To.Add(vendedores[0].EMail);
        //        if (clientes[0].EMail == "" || clientes[0].EMail == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            mail.To.Add(clientes[0].EMail);
        //        }
        //        mail.Subject = subject;
        //        return mail;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        private AlternateView GetEmbeddedImage(List<NotadeVentaCabeceraModels> NVentaCabeceras,
        List<NotaDeVentaDetalleModels> NVentaDetalles, List<ClientesModels> Clientes)
        {
            char[] blanco = { ' ' };

            string htmlBody = String.Format(
            "<html><body>" +
            "<img src='~/Image/logo.jpg' />" +
            "<H1> Nota de Venta </H1>" +
            @"<H4> Nº de Nota de Venta: " + NVentaCabeceras[0].NVNumero + @" </H4>" +
            @"<H4> Fecha Pedido: " + NVentaCabeceras[0].nvFem.ToShortDateString() + @" </H4>" +
            @"<H4> Cliente: " + NVentaCabeceras[0].NomAux + @" </H4>" +
            @"<H4> Dirección: " + Clientes[0].DirAux + @" </H4>" +
            @"<H4> Fecha Entrega: " + NVentaCabeceras[0].nvFeEnt.ToShortDateString() + @" </H4>" +
            @"<H4> Observaciones: " + NVentaCabeceras[0].nvObser + @" </H4>" +
            @"<H4> Vendedor: " + Session["VenDes"].ToString() + @" </H4>" +
            @"<table border = ""1"" >" +
            @"<tr>" +
            @"<td>ID</td>" +
            @"<th nowrap=""nowrap"">Codigo Producto</th>" +
            //@"<th>Imagen</th>" +
            @"<th>Descripcion</th>" +
            @"<th>Cantidad</th>" +
            @"<th>Precio</th>" +
            @"<th>Sub-Total</th>" +
            @"<th>Iva    </th>" +
            @"<th>Total   </th>" +
            @"</tr>");

            ProductosModels producto = new ProductosModels();
            List<LinkedResource> resources = new List<LinkedResource>();
            foreach (NotaDeVentaDetalleModels nvd in NVentaDetalles)
            {
                double precioConIVa = Math.Round(nvd.nvSubTotal * 1.19);
                double Iva = (precioConIVa - nvd.nvSubTotal);
                producto.CodProd = nvd.CodProd;
                htmlBody += @"<tr>" +
                           @"<td>" + nvd.nvLinea + @"</td>" +
                           @"<td>" + nvd.CodProd + @"</td>" +
                           @"<td>" + nvd.DesProd + @"</td>" +
                           @"<td>" + nvd.nvCant + @"</td>" +
                           @"<td>" + nvd.nvPrecio + @"</td>" +
                           @"<td>" + nvd.nvSubTotal + @"</td>" +
                           @"<td>" + Iva + @"</td>" +
                           @"<td>" + precioConIVa + @"</td>" +
                           @"</tr>";
            }
            htmlBody += @"<tr><th colspan =" + 7 + @">Total</th><td>" + NVentaCabeceras[0].TotalBoleta + @"</td></tr>";
            htmlBody += @"</body></html>";

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            foreach (LinkedResource r in resources)
            {
                alternateView.LinkedResources.Add(r);
            }
            return alternateView;
        }

        public void AgregarContacto(string CodAux, string NomCon)
        {
            ClientesModels cli = new ClientesModels();
            cli.CodAux = CodAux;
            cli.NomCon = NomCon;

            List<ClientesModels> nv = ClientesDAO.AgregarContacto(cli);
        }



        #region"--- Web Métodos ---"
        [HttpPost, ValidateInput(false)]
        public JsonResult AgregarNV(FormCollection frm, int NVNumero, DateTime nvFem, DateTime nvFeEnt, string CodAux, string VenCod,
        string CodLista, string nvObser, string CveCod, string NomCon, string CodiCC, double nvSubTotal, double nvMonto,
        double nvNetoAfecto, string Usuario, string UsuarioGeneraDocto, DateTime FechaHoraCreacion, double TotalBoleta,
        string id, string CodLugarDesp)
        {
            int numSoft = 0;
            try
            {
                #region"NVC"
                NotadeVentaCabeceraModels NVC = new NotadeVentaCabeceraModels
                {
                    NVNumero = NVNumero,
                    nvFem = nvFem,
                    nvEstado = "A",
                    nvEstFact = 0,
                    nvEstDesp = 0,
                    nvEstRese = 0,
                    nvEstConc = 0,
                    nvFeEnt = nvFeEnt,
                    CodAux = CodAux,
                    VenCod = VenCod,
                    CodMon = "01",
                    CodLista = CodLista,
                    nvObser = nvObser,
                    CveCod = CveCod,
                    NomCon = NomCon,
                    CodiCC = CodiCC,
                    nvSubTotal = nvSubTotal,
                    nvPorcDesc01 = 0,
                    nvDescto01 = 0,
                    nvPorcDesc02 = 0,
                    nvDescto02 = 0,
                    nvPorcDesc03 = 0,
                    nvDescto03 = 0,
                    nvPorcDesc04 = 0,
                    nvDescto04 = 0,
                    nvPorcDesc05 = 0,
                    nvDescto05 = 0,
                    nvMonto = nvMonto,
                    NumGuiaRes = 0,
                    nvPorcFlete = 0,
                    nvValflete = 0,
                    nvPorcEmb = 0,
                    nvEquiv = 1,
                    nvNetoExento = 0,
                    nvNetoAfecto = nvNetoAfecto,
                    nvTotalDesc = 0,
                    ConcAuto = "N",
                    CheckeoPorAlarmaVtas = "N",
                    EnMantencion = 0,
                    Usuario = Usuario,
                    UsuarioGeneraDocto = UsuarioGeneraDocto,
                    FechaHoraCreacion = FechaHoraCreacion,
                    Sistema = "NW",
                    ConcManual = "N",
                    proceso = "Notas de Venta",
                    TotalBoleta = TotalBoleta,
                    NumReq = 0,
                    CodVenWeb = "5",
                    CodLugarDesp = CodLugarDesp
                };
                #endregion

                List<ParametrosModels> para = ParametrosDAO.BuscarParametros();

                if (para[0].Aprobador == 1)
                {
                    NVC.EstadoNP = "P";
                }
                else
                {
                    NVC.EstadoNP = "A";
                }


                List<NotadeVentaCabeceraModels> NV = NotaDeVentaDAO.EditarNV(NVC);

                if (para[0].Aprobador == 1)
                {
                    // no insertar en softlad
                }
                else
                {
                    List<NotadeVentaCabeceraModels> NVSoft = NotaDeVentaDAO.InsertarNvSoftland(NVC);
                    ViewBag.NVnum = NVSoft[0].NVNumero;
                    numSoft = NVSoft[0].NVNumero;
                }


                //EMail
                //VerificationEmail(NVNumero);
                //return Json(NV);
            }
            catch (Exception ex)
            {
                Archivo.CrearArchivo(Server, "ERROR: " + ex.Message);
            }

            return Json(new { ID = id, NVNUM = numSoft });

        }

        [HttpPost]
        public JsonResult ProductSearch(string CodProd, string CodLista)
        {
            ProductosModels prod = new ProductosModels();
            prod.CodProd = CodProd;
            prod.DesProd = CodProd;
            prod.CodLista = CodLista;

            List<ProductosModels> pro = ProductosDAO.BuscarProducto(prod);

            return Json(pro);
        }

        [HttpPost]
        public JsonResult QuickProductSearch(string CodRapido, string CodLista)
        {
            ProductosModels prod = new ProductosModels();
            prod.CodRapido = CodRapido;
            prod.CodLista = CodLista;

            List<ProductosModels> pro = ProductosDAO.BuscarProductoRapido(prod);

            return Json(pro);
        }

        [HttpPost]
        public JsonResult Multiplicar(string punitario, string cantidad)
        {
            double u = double.Parse(punitario);
            double c = double.Parse(cantidad);

            double t = u * c;

            return Json(t.ToString());
        }

        [HttpPost]
        public JsonResult Porcentaje(string valor, string porcenta)
        {
            CultureInfo culture = new CultureInfo("en-US");

            double u = double.Parse(valor);
            double c = double.Parse(porcenta, CultureInfo.InvariantCulture);
            double Decimal = c / 100;
            double t = u * Decimal;
            double total = u - t;

            return Json(total.ToString());
        }
        #endregion

    }
}
