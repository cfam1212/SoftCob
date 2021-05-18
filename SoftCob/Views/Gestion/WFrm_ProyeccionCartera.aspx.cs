namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ProyeccionCartera : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        DataTable _tblbuscar = new DataTable();
        DataTable _dtbPresupuesto = new DataTable();
        DataRow _result, _addfil;
        DataTable _dtbproyecc = new DataTable();
        ImageButton _imgeselecc = new ImageButton();
        ImageButton _imgverificar = new ImageButton();

        string _meslabel = "", _preslabel = "", _pagado = "", _codigoesp = "0", _respuesta = "", _observacion = "",
            _estado = "", _cedula = "", _documento = "";

        int _mes = 0, _year = 0, _codigocpce = 0, _codigogest = 0, _codigocede = 0, _codigobrmc = 0, _maxcodigo = 0, _codigoprcb;

        decimal _presupuesto, _totalpagos, _porcenproyecc, _totalsumapagos, _totalproyecc, _totalsumaproyecc,
            _porcenpagos, _valorpago, _totalnoefectivos, _totallost, _totalsumalost;
        bool _lexiste;
        DateTime _dtmFecha;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            TxtValor.Attributes.Add("onchange", "ValidarDecimales();");

            if (!IsPostBack)
            {
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                ViewState["CodigoBRMC"] = Request["CodigoBRMC"];
                Lbltitulo.Text = "Proyecciones";

                _dtbPresupuesto.Columns.Add("Codigo");
                _dtbPresupuesto.Columns.Add("VPresupuesto");
                _dtbPresupuesto.Columns.Add("VProyeccion");
                _dtbPresupuesto.Columns.Add("VPorcenProyecc");
                _dtbPresupuesto.Columns.Add("VPagos");
                _dtbPresupuesto.Columns.Add("VPorcenCumplimiento");
                _dtbPresupuesto.Columns.Add("NoEfectivos");
                _dtbPresupuesto.Columns.Add("Lost");
                ViewState["DatosPresupuesto"] = _dtbPresupuesto;

                ViewState["TotalPagos"] = "0.00";
                ViewState["TotalProyecc"] = "0.00";
                ViewState["CodigoPRCB"] = "0";
                TxtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");

                PnlProyeccion_CollapsiblePanelExtender.Collapsed = false;

                if (Session["CedulaProyecc"] != null)
                {
                    LblIdentificacion.InnerText = Session["CedulaProyecc"].ToString();
                    LblCliente.InnerText = Session["ClienteProyecc"].ToString();
                    TxtFecha.Enabled = true;
                    TxtValor.Enabled = true;
                    TxtObservacion.Enabled = true;
                    ImgAgregar.Enabled = true;
                }

                FunCargarMantenimiento(0);

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page,
                    "::GS-BPO GLOBAL SERVICES::", Request["MensajeRetornado"].ToString());
            }
            else
            {
                _dtb = (DataTable)ViewState["Proyeccion"];
                if (_dtb != null)
                {
                    GrdvDatos.DataSource = (DataTable)ViewState["Proyeccion"];
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento(int opcion)
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(227, int.Parse(Session["usuCodigo"].ToString()),
                0, 0, "", "", "", ViewState["Conectar"].ToString());

            if (_dts.Tables[0].Rows.Count > 0)
            {

                _codigobrmc = int.Parse(_dts.Tables[0].Rows[0]["CodigoBRMC"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(228, _codigobrmc, 0, 0, "", "", "",
                    ViewState["Conectar"].ToString());

                _meslabel = _dts.Tables[0].Rows[0]["MesLabel"].ToString();
                _preslabel = _dts.Tables[0].Rows[0]["PresuLabel"].ToString();
                _mes = int.Parse(_dts.Tables[0].Rows[0]["Mes"].ToString());
                _year = int.Parse(_dts.Tables[0].Rows[0]["Anio"].ToString());
                ViewState["Year"] = _year;
                ViewState["Mes"] = _mes;
                _codigocede = int.Parse(_dts.Tables[0].Rows[0]["CodigoCPCE"].ToString());
                _codigocpce = int.Parse(_dts.Tables[0].Rows[0]["CodigoCPCE"].ToString());
                ViewState["CodigoCPCE"] = _codigocpce;
                _codigogest = int.Parse(_dts.Tables[0].Rows[0]["CodigoGEST"].ToString());
                ViewState["CodigoGEST"] = _codigogest;
                _presupuesto = decimal.Parse(_dts.Tables[0].Rows[0]["Presupuesto"].ToString());

                ViewState["Presupuesto"] = _presupuesto;
                Lbltitulo.Text = "PROYECCION " + _meslabel + " " + _year.ToString();
            }

            _dts = new PagoCarteraDAO().FunGetPagoCartera(17, _codigocede, _codigocpce, "", "", "", "", "", "",
                _meslabel, _presupuesto.ToString().Replace(",", "."), "", _codigogest, _year, _mes, 0, "",
                ViewState["Conectar"].ToString());

            if (_dts.Tables[0].Rows.Count > 0)
            {
                ImgBuscar.Enabled = true;

                ViewState["Proyeccion"] = _dts.Tables[0]; ;
                GrdvDatos.DataSource = _dts.Tables[0]; ;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

                _totalsumaproyecc = decimal.Parse(ViewState["TotalProyecc"].ToString());
                _totalsumapagos = decimal.Parse(ViewState["TotalPagos"].ToString());
                _totalnoefectivos = decimal.Parse(ViewState["TotalNoEfectivo"].ToString());
                _totalsumalost = decimal.Parse(ViewState["TotalLost"].ToString());

                if (_totalsumaproyecc > 0) _porcenproyecc = Math.Round((_totalsumaproyecc / _presupuesto) * 100, 2);
                else _porcenproyecc = 0;

                if (_totalsumapagos > 0) _porcenpagos = Math.Round((_totalsumapagos / _presupuesto) * 100, 2);
                else _porcenpagos = 0;

                _dtbPresupuesto = (DataTable)ViewState["DatosPresupuesto"];
                _addfil = _dtbPresupuesto.NewRow();
                _addfil["Codigo"] = 1;
                _addfil["VPresupuesto"] = "$" + string.Format("{0:n}", _presupuesto);
                _addfil["VProyeccion"] = "$" + string.Format("{0:n}", _totalsumaproyecc);
                _addfil["VPorcenProyecc"] = _porcenproyecc.ToString() + "%";
                _addfil["VPagos"] = "$" + string.Format("{0:n}", _totalsumapagos);
                _addfil["VPorcenCumplimiento"] = _porcenpagos.ToString() + "%";
                _addfil["NoEfectivos"] = "$" + string.Format("{0:n}", _totalnoefectivos);
                _addfil["Lost"] = "$" + string.Format("{0:n}", _totalsumalost);

                _dtbPresupuesto.Rows.Add(_addfil);
                ViewState["DatosPresupuesto"] = _dtbPresupuesto;
                GrdvProyeccion.DataSource = _dtbPresupuesto;
                GrdvProyeccion.DataBind();
            }
        }

        private void FunLimpiar()
        {
            LblIdentificacion.InnerText = "";
            LblCliente.InnerText = "";
            TxtFecha.Text = DateTime.Now.ToString("yyyy/MM/dd");
            TxtValor.Text = "0.00";
            TxtObservacion.Text = "";
            TxtFecha.Enabled = false;
            TxtValor.Enabled = false;
            TxtObservacion.Enabled = false;
            TxtDocumento.Text = "";
            TxtDocumento.Enabled = false;
            ImgActualizar.Enabled = false;
            ImgEliminar.Enabled = false;
            ImgAgregar.Enabled = false;
        }
        #endregion

        #region Botones y Eventos
        protected void ImgActualizar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtmFecha = DateTime.ParseExact(string.Format("{0}", TxtFecha.Text),
                     "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (_dtmFecha.Year != int.Parse(ViewState["Year"].ToString()) ||
                    _dtmFecha.Month != int.Parse(ViewState["Mes"].ToString()))
                {
                    new FuncionesDAO().FunShowJSMessage("La Proyeccion debe ser del Año y Mes en curso", this);
                    return;
                }

                _tblbuscar = (DataTable)ViewState["Proyeccion"];

                if (ViewState["FechaPago"].ToString() != TxtFecha.Text.Trim())
                {
                    _result = _tblbuscar.Select("Cedula='" + LblIdentificacion.InnerText.Trim() + "' " +
                        "and FechaPago='" + TxtFecha.Text.Trim() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Registro con Fecha Pago ya existe..!", this);
                    return;
                }

                _presupuesto = decimal.Parse(ViewState["Presupuesto"].ToString().Replace(",", "."),
                    CultureInfo.InvariantCulture);
                _totalpagos = 0;
                _totalproyecc = 0;
                _totalsumaproyecc = 0;
                _totalsumapagos = 0;
                _totalnoefectivos = 0;
                _estado = ViewState["EstadoPago"].ToString();
                _valorpago = decimal.Parse(TxtValor.Text.Trim(), CultureInfo.InvariantCulture);
                _observacion = TxtObservacion.Text.Trim();

                SoftCob_PAGOSCARTERA _pagos = new PagoCarteraDAO().FunGetPagados(LblIdentificacion.InnerText,
                    TxtFecha.Text.Trim(), TxtDocumento.Text.Trim());

                if (_pagos != null)
                {
                    _valorpago = _pagos.pacp_valorpago;
                    _estado = "PAGO REGISTRADO";
                    _documento = _pagos.pacp_documento;
                }

                new PagoCarteraDAO().FunGetPagoCartera(19, 0, 0, "", "", _documento, TxtFecha.Text.Trim(),
                    _valorpago.ToString().Replace(",", "."), "", _observacion, _estado, _documento,
                    int.Parse(ViewState["CodigoRESP"].ToString()), int.Parse(Session["usuCodigo"].ToString()), 0, 0, "",
                    ViewState["Conectar"].ToString());

                _dts = new PagoCarteraDAO().FunGetPagoCartera(22, 0, 0, "", "", "", "", "", "", "", "", "",
                    int.Parse(ViewState["CodigoPRCB"].ToString()), 0, 0, 0, "", ViewState["Conectar"].ToString());

                ViewState["Proyeccion"] = _dts.Tables[0];

                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

                _totalsumaproyecc = decimal.Parse(ViewState["TotalProyecc"].ToString());
                _totalsumapagos = decimal.Parse(ViewState["TotalPagos"].ToString());
                _totalnoefectivos = decimal.Parse(ViewState["TotalNoEfectivo"].ToString());
                _totalsumalost = decimal.Parse(ViewState["TotalLost"].ToString());

                if (_totalsumaproyecc > 0) _porcenproyecc = Math.Round((_totalsumaproyecc / _presupuesto) * 100, 2);
                else _porcenproyecc = 0;

                if (_totalsumapagos > 0) _porcenpagos = Math.Round((_totalsumapagos / _presupuesto) * 100, 2);
                else _porcenpagos = 0;

                _dtbPresupuesto = (DataTable)ViewState["DatosPresupuesto"];
                _result = _dtbPresupuesto.Select("Codigo=1").FirstOrDefault();
                _result["VProyeccion"] = "$" + string.Format("{0:n}", _totalsumaproyecc);
                _result["VPorcenProyecc"] = _porcenproyecc.ToString() + "%";
                _result["VPagos"] = "$" + string.Format("{0:n}", _totalsumapagos);
                _result["VPorcenCumplimiento"] = _porcenpagos.ToString() + "%";
                _result["NoEfectivos"] = "$" + string.Format("{0:n}", _totalnoefectivos);
                _result["Lost"] = "$" + string.Format("{0:n}", _totalsumalost);
                _dtbPresupuesto.AcceptChanges();
                GrdvProyeccion.DataSource = _dtbPresupuesto;
                GrdvProyeccion.DataBind();

                FunLimpiar();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtmFecha = DateTime.ParseExact(string.Format("{0}", TxtFecha.Text),
                     "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (_dtmFecha.Year != int.Parse(ViewState["Year"].ToString()) ||
                    _dtmFecha.Month != int.Parse(ViewState["Mes"].ToString()))
                {
                    new FuncionesDAO().FunShowJSMessage("La Proyeccion debe ser del Año y Mes en curso", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtValor.Text.Trim()) || TxtValor.Text.Trim() == "0" ||
                    TxtValor.Text.Trim() == "0.00" || TxtValor.Text.Trim() == "0.0")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese valor del pago..!", this);
                    return;
                }

                if (ViewState["Proyeccion"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["Proyeccion"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoRESP"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("Cedula='" + LblIdentificacion.InnerText + "' and FechaPago='" +
                        TxtFecha.Text.Trim() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha ya se encuentra registrada..!", this);
                    return;
                }

                _presupuesto = decimal.Parse(ViewState["Presupuesto"].ToString().Replace(",", "."),
                    CultureInfo.InvariantCulture);

                _totalpagos = 0;
                _totalproyecc = 0;
                _totalsumaproyecc = 0;
                _totalsumapagos = 0;
                _estado = "PRESUPUESTO";
                _respuesta = "NUEVO-AGREGADO";
                _observacion = TxtObservacion.Text.Trim();
                _valorpago = decimal.Parse(TxtValor.Text.Trim(), CultureInfo.InvariantCulture);

                new PagoCarteraDAO().FunGetPagoCartera(18, 0, 0, LblIdentificacion.InnerText, "", "",
                    TxtFecha.Text.Trim(), TxtValor.Text.Trim(), _respuesta, LblCliente.InnerText, _estado,
                    _observacion, int.Parse(ViewState["CodigoPRCB"].ToString()), _maxcodigo + 1, 0, 0, "",
                    ViewState["Conectar"].ToString());

                _dts = new PagoCarteraDAO().FunGetPagoCartera(22, 0, 0, "", "", "", "", "", "", "", "", "",
                    int.Parse(ViewState["CodigoPRCB"].ToString()), 0, 0, 0, "", ViewState["Conectar"].ToString());

                ViewState["Proyeccion"] = _dts.Tables[0];

                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

                _totalsumaproyecc = decimal.Parse(ViewState["TotalProyecc"].ToString());
                _totalsumapagos = decimal.Parse(ViewState["TotalPagos"].ToString());
                _totalnoefectivos = decimal.Parse(ViewState["TotalNoEfectivo"].ToString());

                if (_totalsumaproyecc > 0) _porcenproyecc = Math.Round((_totalsumaproyecc / _presupuesto) * 100, 2);
                else _porcenproyecc = 0;

                if (_totalsumapagos > 0) _porcenpagos = Math.Round((_totalsumapagos / _presupuesto) * 100, 2);
                else _porcenpagos = 0;

                _dtbPresupuesto = (DataTable)ViewState["DatosPresupuesto"];
                _result = _dtbPresupuesto.Select("Codigo=1").FirstOrDefault();
                _result["VProyeccion"] = "$" + string.Format("{0:n}", _totalsumaproyecc);
                _result["VPorcenProyecc"] = _porcenproyecc.ToString() + "%";
                _result["VPagos"] = "$" + string.Format("{0:n}", _totalsumapagos);
                _result["VPorcenCumplimiento"] = _porcenpagos.ToString() + "%";
                _result["NoEfectivos"] = "$" + string.Format("{0:n}", _totalnoefectivos);

                _dtbPresupuesto.AcceptChanges();
                GrdvProyeccion.DataSource = _dtbPresupuesto;
                GrdvProyeccion.DataBind();

                FunLimpiar();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tblbuscar = (DataTable)ViewState["Proyeccion"];

                _result = _tblbuscar.Select("CodigoRESP='" + ViewState["CodigoRESP"].ToString() + "'").FirstOrDefault();
                _result.Delete();
                _tblbuscar.AcceptChanges();

                _totalpagos = 0; _totalsumaproyecc = 0; _totalsumapagos = 0; _porcenproyecc = 0; _porcenpagos = 0;
                _totalproyecc = 0; _totalnoefectivos = 0;

                new PagoCarteraDAO().FunGetPagoCartera(20, 0, 0, "", "", "", "", "", "", "", "", "",
                    int.Parse(ViewState["CodigoRESP"].ToString()), int.Parse(Session["usuCodigo"].ToString()), 0, 0, "",
                    ViewState["Conectar"].ToString());

                ViewState["Proyeccion"] = _tblbuscar;

                GrdvDatos.DataSource = _tblbuscar;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

                FunLimpiar();

                _presupuesto = decimal.Parse(ViewState["Presupuesto"].ToString().Replace(",", "."),
                    CultureInfo.InvariantCulture);

                _totalsumaproyecc = decimal.Parse(ViewState["TotalProyecc"].ToString());
                _totalsumapagos = decimal.Parse(ViewState["TotalPagos"].ToString());
                _totalnoefectivos = decimal.Parse(ViewState["TotalNoEfectivo"].ToString());

                if (_totalsumaproyecc > 0) _porcenproyecc = Math.Round((_totalsumaproyecc / _presupuesto) * 100, 2);
                else _porcenproyecc = 0;

                if (_totalsumapagos > 0) _porcenpagos = Math.Round((_totalsumapagos / _presupuesto) * 100, 2);
                else _porcenpagos = 0;

                _dtbPresupuesto = (DataTable)ViewState["DatosPresupuesto"];
                _result = _dtbPresupuesto.Select("Codigo=1").FirstOrDefault();
                _result["VProyeccion"] = "$" + string.Format("{0:n}", _totalsumaproyecc);
                _result["VPorcenProyecc"] = _porcenproyecc.ToString() + "%";
                _result["VPagos"] = "$" + string.Format("{0:n}", _totalsumapagos);
                _result["VPorcenCumplimiento"] = _porcenpagos.ToString() + "%";
                _result["NoEfectivos"] = "$" + string.Format("{0:n}", _totalnoefectivos);
                _dtbPresupuesto.AcceptChanges();
                GrdvProyeccion.DataSource = _dtbPresupuesto;
                GrdvProyeccion.DataBind();

                //FunCargarProyeccion(0);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; " +
                "var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_ListaDeudores.aspx?CodigoCPCE=" +
                ViewState["CodigoCPCE"].ToString() + "&CodigoGEST=" + ViewState["CodigoGEST"].ToString() +
                "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=550px, height=550px, " +
                "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgeselecc = (ImageButton)(e.Row.Cells[6].FindControl("ImgSeleccionar"));
                    _imgverificar = (ImageButton)(e.Row.Cells[7].FindControl("ImgVerificar"));
                    _pagado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["EstadoPago"].ToString();
                    _respuesta = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Respuesta"].ToString();

                    switch (_pagado)
                    {
                        case "PAGO REGISTRADO":
                            e.Row.Cells[0].BackColor = System.Drawing.Color.Orange;
                            _imgeselecc.ImageUrl = "~/Botones/editargris.png";
                            _imgeselecc.Enabled = false;
                            break;
                        case "NO REGISTRADO":
                            e.Row.Cells[0].BackColor = System.Drawing.Color.Cyan;
                            _imgverificar.ImageUrl = "~/Botones/verificar.png";
                            _imgverificar.Enabled = true;
                            break;
                        case "PRESUPUESTO":
                            e.Row.Cells[0].BackColor = System.Drawing.Color.SeaGreen;
                            break;
                        case "PAGO PERDIDO":
                            e.Row.Cells[0].BackColor = System.Drawing.Color.DarkSalmon;
                            _imgeselecc.ImageUrl = "~/Botones/editargris.png";
                            _imgeselecc.Enabled = false;
                            _imgverificar.ImageUrl = "~/Botones/verificar.png";
                            _imgverificar.Enabled = true;
                            break;
                    }

                    if (_respuesta == "NUEVO-AGREGADO")
                    {
                        e.Row.Cells[6].BackColor = System.Drawing.Color.Bisque;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    switch (_pagado)
                    {
                        case "PAGO REGISTRADO":
                            _totalpagos += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Valor"));
                            break;
                        case "NO REGISTRADO":
                            _totalnoefectivos += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Valor"));
                            break;
                        case "PRESUPUESTO":
                            _totalproyecc += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Valor"));
                            break;
                        case "PAGO PERDIDO":
                            _totallost += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Valor"));
                            break;
                    }
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    ViewState["TotalPagos"] = _totalpagos.ToString();
                    ViewState["TotalProyecc"] = _totalproyecc.ToString();
                    ViewState["TotalNoEfectivo"] = _totalnoefectivos.ToString();
                    ViewState["TotalLost"] = _totallost.ToString();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccionar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvDatos.Rows)
                {
                    fr.Cells[1].BackColor = System.Drawing.Color.White;
                }

                GrdvDatos.Rows[gvRow.RowIndex].Cells[1].BackColor = System.Drawing.Color.Coral;
                _codigoesp = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoRESP"].ToString();
                ViewState["CodigoPRCB"] = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPRCB"].ToString();
                ViewState["EstadoPago"] = GrdvDatos.DataKeys[gvRow.RowIndex].Values["EstadoPago"].ToString();

                _dtbproyecc = (DataTable)ViewState["Proyeccion"];

                _result = _dtbproyecc.Select("CodigoRESP='" + _codigoesp + "'").FirstOrDefault();

                ViewState["CodigoRESP"] = _codigoesp;
                LblIdentificacion.InnerText = _result["Cedula"].ToString();
                LblCliente.InnerText = _result["Cliente"].ToString();
                TxtFecha.Text = _result["FechaPago"].ToString();
                TxtValor.Text = _result["Valor"].ToString().Replace(",", ".");
                TxtObservacion.Text = _result["Observacion"].ToString().Replace(",", ".");
                ViewState["FechaPago"] = _result["FechaPago"].ToString();

                ImgActualizar.Enabled = true;
                ImgAgregar.Enabled = true;
                ImgEliminar.Enabled = true;
                TxtFecha.Enabled = true;
                TxtValor.Enabled = true;
                TxtDocumento.Enabled = true;
                TxtObservacion.Enabled = true;

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgVerificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvDatos.Rows)
                {
                    fr.Cells[1].BackColor = System.Drawing.Color.White;
                }

                GrdvDatos.Rows[gvRow.RowIndex].Cells[1].BackColor = System.Drawing.Color.Coral;
                _cedula = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Cedula"].ToString();

                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; " +
                    "var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                    "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_PagosRegistrados.aspx?CodigoCPCE=" +
                    ViewState["CodigoCPCE"].ToString() + "&Year=" + ViewState["Year"].ToString() +
                    "&Cedula=" + _cedula + "&Month=" + ViewState["Mes"].ToString() +
                    "&Gestor=" + Session["usuCodigo"].ToString() +
                    "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=560px, height=200px, " +
                    "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}