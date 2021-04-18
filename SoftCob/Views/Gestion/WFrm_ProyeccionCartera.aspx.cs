namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ProyeccionCartera : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataSet _dtsx = new DataSet();
        DataTable _dtb = new DataTable();
        DataTable _tblagre = new DataTable();
        DataTable _tblbuscar = new DataTable();
        DataTable _dtbPresupuesto = new DataTable();
        DataRow _filagre, _result, _addfil;
        DataRow[] _resultado;
        DataTable _dtbproyecc = new DataTable();
        ImageButton _imgeselecc = new ImageButton();
        string _meslabel, _preslabel, _pagado, _codigoesp, _respuesta, _observacion, _estado, _vpresupuesto;
        int _mes, _year, _codigocpce, _codigogest, _codigocede, _codigobrmc, _maxcodigo, _codigoprcb;
        decimal _presupuesto, _totalpagos, _porcenvalores, _totalsumapagos, _totalvalores, _totalsumavalores,
            _porcenpagos;
        bool _lexiste;
        decimal _valorpago;
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
                ViewState["CodigoBRMC"] = Request["CodigoBRMC"];
                Lbltitulo.Text = "Proyecciones";

                _dtbPresupuesto.Columns.Add("Codigo");
                _dtbPresupuesto.Columns.Add("VPresupuesto");
                _dtbPresupuesto.Columns.Add("VProyeccion");
                _dtbPresupuesto.Columns.Add("VPorcenProyecc");
                _dtbPresupuesto.Columns.Add("VPagos");
                _dtbPresupuesto.Columns.Add("VPorcenCumplimiento");
                ViewState["DatosPresupuesto"] = _dtbPresupuesto;

                ViewState["TotalPagos"] = "0.00";
                ViewState["TotalValores"] = "0.00";
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
                    ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
            else
            {
                GrdvDatos.DataSource = (DataTable)ViewState["Proyeccion"];
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento(int opcion)
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(227, int.Parse(Session["usuCodigo"].ToString()),
                0, 0, "", "", "", Session["Conectar"].ToString());

            if (_dts.Tables[0].Rows.Count > 0)
            {
                _codigobrmc = int.Parse(_dts.Tables[0].Rows[0]["CodigoBRMC"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(228, _codigobrmc, 0, 0, "", "", "",
                    Session["Conectar"].ToString());

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

                _dts = new PagoCarteraDAO().FunGetPagoCartera(15, _codigocede, _codigocpce, "", "", "", "", "", "", "",
                    "", "", _codigogest, _year, _mes, 0, "", Session["Conectar"].ToString());

                _dtsx = new PagoCarteraDAO().FunGetPagoCartera(16, _codigocede, _codigocpce, "", "", "", "", "", "", "", "", "",
                    _codigogest, _year, _mes, 0, "", Session["Conectar"].ToString());

                if (_dtsx.Tables[0].Rows.Count == 0)
                {
                    _dtsx = new PagoCarteraDAO().FunGetPagoCartera(17, _codigocede, _codigocpce, "", "", "", "", "", "",
                        _meslabel, _presupuesto.ToString().Replace(",", "."), "", _codigogest, _year, _mes, 0, "",
                        Session["Conectar"].ToString());

                    _resultado = _dtsx.Tables[0].Select("Eliminado='NO'");
                    _dtbproyecc = _resultado.CopyToDataTable();

                    ViewState["Proyeccion"] = _dtbproyecc;
                    GrdvDatos.DataSource = _dtbproyecc;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    _codigoprcb = int.Parse(_dtsx.Tables[0].Rows[0]["CodigoPRCB"].ToString());

                    ViewState["Proyeccion"] = _dtsx.Tables[0];

                    _tblagre = (DataTable)ViewState["Proyeccion"];

                    foreach (DataRow _drfila in _dts.Tables[0].Rows)
                    {
                        _result = _tblagre.Select("Cedula='" + _drfila["Cedula"].ToString() + "' and FechaPago='" +
                            _drfila["FechaPago"].ToString() + "'").FirstOrDefault();

                        if (_result == null)
                        {
                            _result = _tblagre.Select("CodigoRESP='" + _drfila["CodigoRESP"].ToString() + "'").FirstOrDefault();

                            if (_result == null)
                            {
                                _filagre = _tblagre.NewRow();
                                _filagre["CodigoRESP"] = _drfila["CodigoRESP"].ToString();
                                _filagre["Cedula"] = _drfila["Cedula"].ToString();
                                _filagre["Cliente"] = _drfila["Cliente"].ToString();
                                _filagre["FechaPago"] = _drfila["FechaPago"].ToString();
                                _filagre["Anio"] = _year.ToString();
                                _filagre["Mes"] = _mes.ToString();
                                _filagre["Valor"] = _drfila["Valor"].ToString();
                                _filagre["EstadoPago"] = _drfila["EstadoPago"].ToString();
                                _filagre["Observacion"] = _drfila["Observacion"].ToString();
                                _filagre["Eliminado"] = _drfila["Eliminado"].ToString();
                                _filagre["Respuesta"] = _drfila["Respuesta"].ToString();
                                _filagre["CodigoPRCB"] = _codigoprcb.ToString();
                                _tblagre.Rows.Add(_filagre);

                                new PagoCarteraDAO().FunGetPagoCartera(18, 0, 0, _drfila["Cedula"].ToString(), "", "",
                                    _drfila["FechaPago"].ToString(), _drfila["Valor"].ToString().Replace(",", "."), "",
                                    _drfila["Cliente"].ToString(), _drfila["EstadoPago"].ToString(),
                                    _drfila["Respuesta"].ToString(), _codigoprcb, int.Parse(_drfila["CodigoRESP"].ToString()),
                                    int.Parse(_drfila["CodigoARRE"].ToString()), 0, "", Session["Conectar"].ToString());
                            }
                        }
                        else
                        {
                            _result["Valor"] = _drfila["Valor"].ToString();
                            _result["EstadoPago"] = _drfila["EstadoPago"].ToString();
                            _result["Respuesta"] = _drfila["Respuesta"].ToString();
                            _result["Observacion"] = "";
                            _result["CodigoARRE"] = _drfila["CodigoARRE"].ToString();
                            _tblagre.AcceptChanges();
                        }
                    }

                    _tblagre.DefaultView.Sort = "Cedula,FechaPago";
                    _tblagre = _tblagre.DefaultView.ToTable();

                    _resultado = _tblagre.Select("Eliminado='NO'");
                    _dtbproyecc = _resultado.CopyToDataTable();
                    ViewState["Proyeccion"] = _dtbproyecc;

                    GrdvDatos.DataSource = _dtbproyecc;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                _totalsumavalores = decimal.Parse(ViewState["TotalValores"].ToString());
                _totalsumapagos = decimal.Parse(ViewState["TotalPagos"].ToString());

                if (_totalsumavalores > 0) _porcenvalores = Math.Round((_totalsumavalores / _presupuesto) * 100, 2);
                else _porcenvalores = 0;

                if (_totalsumapagos > 0) _porcenpagos = Math.Round((_totalsumapagos / _presupuesto) * 100, 2);
                else _porcenpagos = 0;

                _dtbPresupuesto = (DataTable)ViewState["DatosPresupuesto"];
                _addfil = _dtbPresupuesto.NewRow();
                _addfil["Codigo"] = 1;
                _addfil["VPresupuesto"] = "$" + string.Format("{0:n}", _presupuesto);
                _addfil["VProyeccion"] = "$" + string.Format("{0:n}", _totalsumavalores);
                _addfil["VPorcenProyecc"] = _porcenvalores.ToString() + "%";
                _addfil["VPagos"] = "$" + string.Format("{0:n}", _totalsumapagos);
                _addfil["VPorcenCumplimiento"] = _porcenpagos.ToString() + "%";
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
                _totalvalores = 0;
                _totalsumavalores = 0;
                _totalsumapagos = 0;
                _estado = "";
                _valorpago = decimal.Parse(TxtValor.Text.Trim(), CultureInfo.InvariantCulture);
                _observacion = TxtObservacion.Text.Trim();

                SoftCob_PAGOSCARTERA _pagos = new PagoCarteraDAO().FunGetPagados(LblIdentificacion.InnerText,
                    TxtFecha.Text.Trim());

                if (_pagos != null)
                {
                    _valorpago = _pagos.pacp_valorpago;
                    _estado = "PAGADO";
                    _observacion = "";
                }

                _result = _tblbuscar.Select("CodigoRESP='" + ViewState["CodigoRESP"].ToString() + "'").FirstOrDefault();
                _result["FechaPago"] = TxtFecha.Text.Trim();
                _result["EstadoPago"] = _estado;
                _result["Valor"] = _valorpago;
                _result["Observacion"] = _observacion;
                _tblbuscar.AcceptChanges();

                new PagoCarteraDAO().FunGetPagoCartera(19, 0, 0, "", "", "",
                    TxtFecha.Text.Trim(), _valorpago.ToString().Replace(",", "."), "", _observacion, _estado, "",
                    int.Parse(ViewState["CodigoRESP"].ToString()), 0, 0, 0, "", Session["Conectar"].ToString());

                _tblbuscar.DefaultView.Sort = "Cedula,FechaPago";
                _tblbuscar = _tblbuscar.DefaultView.ToTable();
                ViewState["Proyeccion"] = _tblbuscar;

                _resultado = _tblbuscar.Select("Eliminado='NO'");

                _dtbproyecc = _resultado.CopyToDataTable();
                GrdvDatos.DataSource = _dtbproyecc;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

                _totalsumavalores = decimal.Parse(ViewState["TotalValores"].ToString());
                _totalsumapagos = decimal.Parse(ViewState["TotalPagos"].ToString());

                if (_totalsumavalores > 0) _porcenvalores = Math.Round((_totalsumavalores / _presupuesto) * 100, 2);
                else _porcenvalores = 0;

                if (_totalsumapagos > 0) _porcenpagos = Math.Round((_totalsumapagos / _presupuesto) * 100, 2);
                else _porcenpagos = 0;

                _dtbPresupuesto = (DataTable)ViewState["DatosPresupuesto"];
                _result = _dtbPresupuesto.Select("Codigo=1").FirstOrDefault();
                _result["VProyeccion"] = "$" + string.Format("{0:n}", _totalsumavalores);
                _result["VPorcenProyecc"] = _porcenvalores.ToString() + "%";
                _result["VPagos"] = "$" + string.Format("{0:n}", _totalsumapagos);
                _result["VPorcenCumplimiento"] = _porcenpagos.ToString() + "%";
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

                //_dtmfechaactual = DateTime.ParseExact(String.Format("{0}", DateTime.Now.ToString("yyyy-MM-dd")),
                //     "yyyy-MM-dd", CultureInfo.InvariantCulture);

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

                //_vpresupuesto = ViewState["Presupuesto"].ToString();
                _presupuesto = decimal.Parse(ViewState["Presupuesto"].ToString().Replace(",", "."),
                    CultureInfo.InvariantCulture);

                _totalpagos = 0;
                _totalvalores = 0;
                _totalsumavalores = 0;
                _totalsumapagos = 0;
                _estado = "";
                _respuesta = "NUEVO-AGREGADO";
                _observacion = TxtObservacion.Text.Trim();
                _valorpago = decimal.Parse(TxtValor.Text.Trim(), CultureInfo.InvariantCulture);

                SoftCob_PAGOSCARTERA _pagos = new PagoCarteraDAO().FunGetPagados(LblIdentificacion.InnerText,
                    TxtFecha.Text.Trim());

                if (_pagos != null)
                {
                    _valorpago = _pagos.pacp_valorpago;
                    _estado = "PAGADO";
                    _respuesta = "";
                    _observacion = "";
                }

                _tblagre = (DataTable)ViewState["Proyeccion"];
                _codigoprcb = _tblagre.AsEnumerable().Max(row => int.Parse((string)row["CodigoPRCB"]));

                _filagre = _tblagre.NewRow();
                _filagre["CodigoRESP"] = _maxcodigo + 1;
                _filagre["Cedula"] = LblIdentificacion.InnerText;
                _filagre["Cliente"] = LblCliente.InnerText;
                _filagre["FechaPago"] = TxtFecha.Text.Trim();
                _filagre["Valor"] = _valorpago;
                _filagre["EstadoPago"] = _estado;
                _filagre["Respuesta"] = _respuesta;
                _filagre["Observacion"] = _observacion;
                _filagre["Eliminado"] = "NO";
                _filagre["CodigoARRE"] = "0";
                _filagre["CodigoPRCB"] = _codigoprcb.ToString();
                _tblagre.Rows.Add(_filagre);

                new PagoCarteraDAO().FunGetPagoCartera(18, 0, 0, LblIdentificacion.InnerText, "", "",
                    TxtFecha.Text.Trim(), TxtValor.Text.Trim(), _respuesta, LblCliente.InnerText, _estado,
                    _observacion, _codigoprcb, _maxcodigo + 1, 0, 0, "", Session["Conectar"].ToString());

                _tblagre.DefaultView.Sort = "Cedula,FechaPago";
                _tblagre = _tblagre.DefaultView.ToTable();
                ViewState["Proyeccion"] = _tblagre;

                _resultado = _tblagre.Select("Eliminado='NO'");

                _dtbproyecc = _resultado.CopyToDataTable();
                GrdvDatos.DataSource = _dtbproyecc;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

                _totalsumavalores = decimal.Parse(ViewState["TotalValores"].ToString());
                _totalsumapagos = decimal.Parse(ViewState["TotalPagos"].ToString());

                if (_totalsumavalores > 0) _porcenvalores = Math.Round((_totalsumavalores / _presupuesto) * 100, 2);
                else _porcenvalores = 0;

                if (_totalsumapagos > 0) _porcenpagos = Math.Round((_totalsumapagos / _presupuesto) * 100, 2);
                else _porcenpagos = 0;

                _dtbPresupuesto = (DataTable)ViewState["DatosPresupuesto"];
                _result = _dtbPresupuesto.Select("Codigo=1").FirstOrDefault();
                _result["VProyeccion"] = "$" + string.Format("{0:n}", _totalsumavalores);
                _result["VPorcenProyecc"] = _porcenvalores.ToString() + "%";
                _result["VPagos"] = "$" + string.Format("{0:n}", _totalsumapagos);
                _result["VPorcenCumplimiento"] = _porcenpagos.ToString() + "%";
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
                _result["Eliminado"] = "SI";
                _tblbuscar.AcceptChanges();

                _totalpagos = 0; _totalsumavalores = 0; _totalsumapagos = 0; _porcenvalores = 0; _porcenpagos = 0;
                _totalvalores = 0;

                new PagoCarteraDAO().FunGetPagoCartera(20, 0, 0, LblIdentificacion.InnerText, "", "", "", "", "", "", "SI", "",
                    int.Parse(ViewState["CodigoRESP"].ToString()), 0, 0, 0, "", Session["Conectar"].ToString());

                _tblbuscar.DefaultView.Sort = "Cedula,FechaPago";
                _tblbuscar = _tblbuscar.DefaultView.ToTable();
                ViewState["Proyeccion"] = _tblbuscar;

                _resultado = _tblbuscar.Select("Eliminado='NO'");

                _dtbproyecc = _resultado.CopyToDataTable();
                ViewState["Proyeccion"] = _dtbproyecc;
                GrdvDatos.DataSource = _dtbproyecc;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

                FunLimpiar();

                _presupuesto = decimal.Parse(ViewState["Presupuesto"].ToString().Replace(",", "."),
                    CultureInfo.InvariantCulture);

                _totalsumavalores = decimal.Parse(ViewState["TotalValores"].ToString());
                _totalsumapagos = decimal.Parse(ViewState["TotalPagos"].ToString());

                if (_totalsumavalores > 0) _porcenvalores = Math.Round((_totalsumavalores / _presupuesto) * 100, 2);
                else _porcenvalores = 0;

                if (_totalsumapagos > 0) _porcenpagos = Math.Round((_totalsumapagos / _presupuesto) * 100, 2);
                else _porcenpagos = 0;

                _dtbPresupuesto = (DataTable)ViewState["DatosPresupuesto"];
                _result = _dtbPresupuesto.Select("Codigo=1").FirstOrDefault();
                _result["VProyeccion"] = "$" + string.Format("{0:n}", _totalsumavalores);
                _result["VPorcenProyecc"] = _porcenvalores.ToString() + "%";
                _result["VPagos"] = "$" + string.Format("{0:n}", _totalsumapagos);
                _result["VPorcenCumplimiento"] = _porcenpagos.ToString() + "%";
                _dtbPresupuesto.AcceptChanges();
                GrdvProyeccion.DataSource = _dtbPresupuesto;
                GrdvProyeccion.DataBind();
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
                "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=550px, height=500px, " +
                "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            //Response.Redirect("WFrm_ListaDeudores.aspx?CodigoCPCE=" + ViewState["CodigoCPCE"].ToString() + "&CodigoGEST=" +
            //    ViewState["CodigoGEST"].ToString(), true);
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
                    _pagado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["EstadoPago"].ToString();
                    _respuesta = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Respuesta"].ToString();

                    if (_pagado == "PAGADO")
                    {
                        e.Row.Cells[5].BackColor = System.Drawing.Color.Red;
                        _imgeselecc.ImageUrl = "~/Botones/editargris.png";
                        _imgeselecc.Enabled = false;
                    }

                    if (_respuesta == "NUEVO-AGREGADO")
                    {
                        e.Row.Cells[6].BackColor = System.Drawing.Color.Bisque;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_pagado == "PAGADO")
                    {
                        _totalpagos += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Valor"));
                    }
                    if (_pagado == "")
                    {
                        _totalvalores += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Valor"));
                    }
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    ViewState["TotalPagos"] = _totalpagos.ToString();
                    ViewState["TotalValores"] = _totalvalores.ToString();
                    //e.Row.Cells[2].Text = "TOTAL:";
                    //e.Row.Cells[3].Text = _totalpagos.ToString();
                    //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    //e.Row.Font.Bold = true;
                    //e.Row.Font.Size = 9;
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
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvDatos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigoesp = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoRESP"].ToString();
                //ViewState["CodigoPRCB"] = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPRCB"].ToString();

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
                TxtObservacion.Enabled = true;

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