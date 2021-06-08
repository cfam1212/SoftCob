namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_RegLlamadaEntrante : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataSet _dtsx = new DataSet();
        DataSet _dtssegmento = new DataSet();
        DataTable _dtbarbolrespuesta = new DataTable();
        ImageButton _imgphone = new ImageButton();
        ListItem _accion = new ListItem();
        ListItem _efecto = new ListItem();
        ListItem _respuesta = new ListItem();
        ListItem _respusetadel = new ListItem();
        ListItem _contacto = new ListItem();
        ListItem _prefijo = new ListItem();
        DataTable _tblbuscar = new DataTable();
        DataTable _tblagre = new DataTable();
        DataTable _dtbtelefonos = new DataTable();
        DataTable _dtbgestion = new DataTable();

        bool _pago = false, _efectivo = false, _llamar = false, _validar = true;

        string _mensaje = "", _fechapago = "", _valorpago = "", _fechallamar = "", _horallamar = "", _tipopago = "",
            _valor = "", _strrespuesta = "", _telefonoctc = "", _horalogueo = "", _fechalogueo = "",
            _redirect = "", _nuevo = "", _existe = "", _sufijo = "", _cedulagarante = "",
            _tipo = "", _codigogarante = "", _operacion = "", _txtspeech = "", _bcast = "";

        int _codigoltca = 0, _maxcodigo = 0, _totalgestion = 0, _totallamada = 0, _id = 0,
            _lintperfilactitudinal = 0, _lintestilonegociacion = 0, _lintmetaprograma = 0, _lintmodalidad = 0,
            _lintestadoyo = 0, _lintimpulsor = 0, _codigotece = 0, _score = 0, _day = 0, _codigo = 0;

        ImageButton _imgcall = new ImageButton();
        ImageButton _imgeliminar = new ImageButton();
        ImageButton _imgeditar = new ImageButton();
        ImageButton _imgmarcar = new ImageButton();
        CheckBox _chktelefec = new CheckBox();
        CheckBox _chkbcast = new CheckBox();
        Thread _thrmarcar;
        DataRow _filagre;
        DataRow _result;
        DataRow _cambiar;
        string[] _valores;
        bool _lexiste = false, _mostrarpopup = false, _mostrararbol = false;
        TimeSpan _tiempogestion, _tiempollamada, _tiempofingestion, _tgestion, _tllamada;
        decimal _totalexigible = 0.00M, _totalcapital = 0.00M, _totaldeuda = 0.00M, _porcumplido = 0;
        double _sumarsegundos = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            TxtValorAbono.Attributes.Add("onchange", "ValidarDecimales();");
            FunCargarVariablesBlandas();

            if (!IsPostBack)
            {
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());

                pnlDatosDeudor.Height = 150;
                pnlDatosObligacion.Height = 150;
                PnlDatosGarante.Height = 150;
                PnlPresuCompromiso.Height = 150;
                PnlBrenchPagos.Height = 150;

                if (Session["PermisoEspecial"].ToString() == "SI") DivPresupuesto.Visible = true;

                Lbltitulo.Text = "Registro de Datos por Llamada Entrante";
                ViewState["MarcarTest"] = ConfigurationManager.AppSettings["MarcarTest"];

                if (ViewState["MarcarTest"].ToString() == "SI") Session["IPLocalAdress"] = "192.168.20.184";

                Session["TrackNumber"] = "";
                ViewState["Pago"] = "NO";
                ViewState["Llamar"] = "NO";
                ViewState["VerGestiones"] = "1";
                ViewState["TiempoMarcado"] = ConfigurationManager.AppSettings["TiempoMarcado"];
                ViewState["MinutosLlamar"] = ConfigurationManager.AppSettings["MinutosLlamar"];
                Session["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                ViewState["CodigoCEDE"] = Request["CodigoCEDE"];
                Session["CodigoCPCE"] = Request["CodigoCPCE"];
                ViewState["CodigoCLDE"] = Request["CodigoCLDE"];
                ViewState["CodigoPERS"] = Request["CodigoPERS"];
                ViewState["NumeroDocumento"] = Request["NumeroDocumento"];
                ViewState["Operacion"] = Request["Operacion"];
                ViewState["CodigoLTCA"] = Request["CodigoLTCA"];
                ViewState["CodigoUSU"] = Request["CodigoUSU"];
                ViewState["Retornar"] = Request["Retornar"];
                ViewState["CodigoSpeechCab"] = "0";
                ViewState["View1"] = "";
                ViewState["View2"] = "";
                ViewState["View3"] = "";
                ViewState["Cambiar"] = "SI";
                ViewState["DocumentoRef"] = "";
                _mostrarpopup = false;
                _day = (int)DateTime.Now.DayOfWeek;

                _dtsx = new ConsultaDatosDAO().FunConsultaDatos(220, 0, 0, 0, "", "ACCESO ARBOL", "",
                    Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(232, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(Session["usuCodigo"].ToString()), 0, "", "", "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows[0][0].ToString() == "SI")
                {
                    if (_dtsx.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow _drfila in _dtsx.Tables[0].Rows)
                        {
                            if (_day == int.Parse(_drfila["ValirI"].ToString()))
                            {
                                _mostrararbol = true;
                                break;
                            }
                        }
                    }

                    if (_mostrararbol) TrArbol.Visible = true;
                }

                SoftCob_CEDENTE cedente = new CedenteDAO().FunGetCedentePorID(int.Parse(ViewState["CodigoCEDE"].ToString()));
                ViewState["NivelArbol"] = cedente.cede_auxi1;

                switch (ViewState["NivelArbol"].ToString())
                {
                    case "3":
                        LblContacto.Visible = false;
                        DdlContacto.Visible = false;
                        Chkcitacion.Visible = true;
                        DdlCitacion.Visible = true;
                        break;
                    default:
                        GrdvDatosGarante.Columns[7].Visible = false;
                        break;
                }

                if (Session["PermisoEspecial"].ToString() == "SI")
                {
                    ImgComparar.Visible = true;

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(220, 0, 0, 0, "", "DIAS COMPARATIVO", "",
                        Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _dtsx = new ConsultaDatosDAO().FunConsultaDatos(224, int.Parse(Session["CodigoCPCE"].ToString()),
                            _day, int.Parse(Session["usuCodigo"].ToString()), "", "", "", Session["Conectar"].ToString());

                        if (_dtsx.Tables[0].Rows.Count == 0)
                        {
                            foreach (DataRow _drfila in _dts.Tables[0].Rows)
                            {
                                if (_day == int.Parse(_drfila["ValirI"].ToString()))
                                {
                                    _dts = new ConsultaDatosDAO().FunConsultaDatos(225, int.Parse(Session["CodigoCPCE"].ToString()),
                                        _day, int.Parse(Session["usuCodigo"].ToString()), "", "", "", Session["Conectar"].ToString());
                                    _mostrarpopup = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (_mostrarpopup)
                    {
                        ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; " +
                            "var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                            "window.open('../Breanch/WFrm_CompararBrench.aspx?CodigoCPCE=" + Session["CodigoCPCE"].ToString() +
                            "',null,'left=' + posicion_x + " +
                            "', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, " +
                            "toolbar=no, location=no, menubar=no,titlebar=0');", true);
                    }
                }

                PnlDatosOperacion_CollapsiblePanelExtender.Collapsed = false;
                PnlDatosTelefonos_CollapsiblePanelExtender.Collapsed = false;
                PnlOpcionRegistros_CollapsiblePanelExtender.Collapsed = false;
                PnlResultadoGestiones_CollapsiblePanelExtender.Collapsed = false;

                _dts = new ListaTrabajoDAO().FunGetArbolRespuesta(int.Parse(Session["CodigoCPCE"].ToString()));
                ViewState["ArbolContactoEfectivo"] = _dts.Tables[0];

                ViewState["InicioGestion"] = DateTime.Now.ToString("HH:mm:ss");
                Session["InicioLlamada"] = DateTime.Now.AddSeconds(_sumarsegundos).ToString("HH:mm:ss");

                //if (Session["CodigoCPCE"].ToString() == "3") ImgCitacion.Visible = true;

                SoftCob_LOGUEO_TIEMPOS loguintime = new SoftCob_LOGUEO_TIEMPOS();
                {
                    loguintime.USUA_CODIGO = int.Parse(Session["usuCodigo"].ToString());
                    loguintime.empr_codigo = int.Parse(ViewState["CodigoCEDE"].ToString());
                    loguintime.cpce_codigo = int.Parse(Session["CodigoCPCE"].ToString());
                    loguintime.ltca_codigo = 0;
                    loguintime.loti_tipologueo = "IGE";
                    _fechalogueo = DateTime.Now.ToString("MM/dd/yyyy");
                    loguintime.loti_fechalogueo = DateTime.ParseExact(_fechalogueo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    _horalogueo = DateTime.Now.ToString("HH:mm:ss");
                    loguintime.loti_horalogueo = TimeSpan.Parse(_horalogueo);
                    loguintime.loti_fechacompleta = DateTime.Now;
                    loguintime.loti_auxv1 = "";
                    loguintime.loti_auxv2 = "";
                    loguintime.loti_auxv3 = "";
                    loguintime.loti_auxv4 = "";
                    loguintime.loti_auxi1 = 0;
                    loguintime.loti_auxi2 = 0;
                    loguintime.loti_auxi3 = 0;
                    loguintime.loti_auxi4 = 0;
                    loguintime.loti_auxd1 = DateTime.Now;
                    loguintime.loti_auxd2 = DateTime.Now;
                    loguintime.loti_auxd3 = DateTime.Now;
                    loguintime.loti_auxd4 = DateTime.Now;
                    loguintime.loti_fechacreacion = DateTime.Now;
                    loguintime.loti_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    loguintime.loti_terminalcreacion = Session["MachineName"].ToString();
                }
                new ControllerDAO().FunCrearLogueoTiempos(loguintime);
                FunCargarCombos(0);
                FunCargarCombos(1);
                FunCargarCombos(2);
                FunCargarCombos(10);
                FunCargarDatos();
                FunCargarPerfilUsuario(int.Parse(Session["usuPerfil"].ToString()));
                FunCargarValoresVariablesBlandas();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarDatos()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(213, int.Parse(Session["usuCodigo"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), 0, "", "", "", Session["Conectar"].ToString());
                GrdvBrenchGestor.DataSource = _dts;
                GrdvBrenchGestor.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(214, int.Parse(Session["usuCodigo"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), 0, "", "", "", Session["Conectar"].ToString());
                GrdvBrenchPago.DataSource = _dts;
                GrdvBrenchPago.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(32, int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString().ToString());
                GrdvDatosDeudor.DataSource = _dts;
                GrdvDatosDeudor.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(58, int.Parse(Session["CodigoCPCE"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString().ToString());
                ViewState["Catalogo"] = _dts.Tables[0].Rows[0]["Descripcion"].ToString();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(33, int.Parse(ViewState["CodigoCEDE"].ToString()), int.Parse(Session["CodigoCPCE"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), ViewState["Catalogo"].ToString(), "", "", Session["Conectar"].ToString().ToString());

                ViewState["DatosObligacion"] = _dts.Tables[0];
                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();

                //Datos Garante
                _dts = new ConsultaDatosDAO().FunConsultaDatos(45, 0, 0, 0, "", ViewState["NumeroDocumento"].ToString(), "",
                    Session["Conectar"].ToString().ToString());
                GrdvDatosGarante.DataSource = _dts;
                GrdvDatosGarante.DataBind();

                if (_dts.Tables[0].Rows.Count > 0) PnlDatosGarante.Visible = true;
                else PnlDatosGarante.Visible = false;

                _dts = new ConsultaDatosDAO().FunConsultaDatos(141, int.Parse(ViewState["CodigoCEDE"].ToString()), int.Parse(Session["usuCodigo"].ToString()),
                    int.Parse(ViewState["CodigoPERS"].ToString()), "", ViewState["NumeroDocumento"].ToString(), Session["MachineName"].ToString(), Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(35, int.Parse(ViewState["CodigoCEDE"].ToString()), int.Parse(ViewState["CodigoPERS"].ToString()),
                    int.Parse(ViewState["CodigoCLDE"].ToString()), "", "", "", Session["Conectar"].ToString());
                ViewState["TelefonosRegistrados"] = _dts.Tables[0];
                GrdvTelefonos.DataSource = _dts;
                GrdvTelefonos.DataBind();
                FunConsultarGestiones(0);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlAccion.Items.Clear();
                    _accion.Text = "--Seleccione Acción--";
                    _accion.Value = "0";
                    DdlAccion.Items.Add(_accion);

                    DdlEfecto.Items.Clear();
                    _efecto.Text = "--Seleccione Efecto--";
                    _efecto.Value = "0";
                    DdlEfecto.Items.Add(_efecto);

                    DdlRespuesta.Items.Clear();
                    _respuesta.Text = "--Seleccione Respuesta--";
                    _respuesta.Value = "0";
                    DdlRespuesta.Items.Add(_respuesta);

                    DdlRespuestaDel.Items.Clear();
                    _respusetadel.Text = "--seleccione Respuesta--";
                    _respusetadel.Value = "0";
                    DdlRespuestaDel.Items.Add(_respusetadel);

                    DdlContacto.Items.Clear();
                    _contacto.Text = "--Seleccione Contacto--";
                    _contacto.Value = "0";
                    DdlContacto.Items.Add(_contacto);

                    DdlAccionDel.DataSource = new ControllerDAO().FunGetParametroDetalle("ACCION TELEFONO", "--Seleccione Accion--", "S");
                    DdlAccionDel.DataTextField = "Descripcion";
                    DdlAccionDel.DataValueField = "Codigo";
                    DdlAccionDel.DataBind();

                    DdlRespuestaDel.DataSource = new ControllerDAO().FunGetParametroDetalle("RESPUESTA TELEFONO", 
                        "--Seleccione Respuesta--", "S");
                    DdlRespuestaDel.DataTextField = "Descripcion";
                    DdlRespuestaDel.DataValueField = "Codigo";
                    DdlRespuestaDel.DataBind();

                    break;
                case 1:
                    DdlTipTelefono.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO TELEFONO", "--Seleccione Tipo--", "S");
                    DdlTipTelefono.DataTextField = "Descripcion";
                    DdlTipTelefono.DataValueField = "Codigo";
                    DdlTipTelefono.DataBind();

                    DdlPropietario2.DataSource = new ControllerDAO().FunGetParametroDetalle("PROPIEDAD TELEFONO", 
                        "--Seleccione Propietario--", "S");
                    DdlPropietario2.DataTextField = "Descripcion";
                    DdlPropietario2.DataValueField = "Codigo";
                    DdlPropietario2.DataBind();

                    DdlPrefijo.DataSource = new ControllerDAO().FunGetParametroDetalle("PREFIJOS", "--Seleccione Prefijo--", "S");
                    DdlPrefijo.DataTextField = "Descripcion";
                    DdlPrefijo.DataValueField = "Codigo";
                    DdlPrefijo.DataBind();

                    DdlTipoPago.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO PAGOS", "--Seleccione Pago--", "S");
                    DdlTipoPago.DataTextField = "Descripcion";
                    DdlTipoPago.DataValueField = "Codigo";
                    DdlTipoPago.DataBind();
                    break;
                case 2:
                    DdlAccion.DataSource = new SpeechDAO().FunGetArbolNewAccion(int.Parse(Session["CodigoCPCE"].ToString()));
                    DdlAccion.DataTextField = "Descripcion";
                    DdlAccion.DataValueField = "Codigo";
                    DdlAccion.DataBind();
                    break;
                case 3:
                    DdlEfecto.Items.Clear();
                    _efecto.Text = "--Seleccione Efecto--";
                    _efecto.Value = "0";
                    DdlEfecto.Items.Add(_efecto);

                    DdlRespuesta.Items.Clear();
                    _respuesta.Text = "--Seleccione Respuesta--";
                    _respuesta.Value = "0";
                    DdlRespuesta.Items.Add(_respuesta);

                    DdlContacto.Items.Clear();
                    _contacto.Text = "--Seleccione Contacto--";
                    _contacto.Value = "0";
                    DdlContacto.Items.Add(_contacto);

                    DdlEfecto.DataSource = new SpeechDAO().FunGetArbolNewEfecto(int.Parse(DdlAccion.SelectedValue));
                    DdlEfecto.DataTextField = "Descripcion";
                    DdlEfecto.DataValueField = "Codigo";
                    DdlEfecto.DataBind();
                    break;
                case 4:
                    DdlRespuesta.Items.Clear();
                    _respuesta.Text = "--Seleccione Respuesta--";
                    _respuesta.Value = "0";
                    DdlRespuesta.Items.Add(_respuesta);

                    DdlContacto.Items.Clear();
                    _contacto.Text = "--Seleccione Contacto--";
                    _contacto.Value = "0";
                    DdlContacto.Items.Add(_contacto);

                    DdlRespuesta.DataSource = new SpeechDAO().FunGetArbolNewRespuesta(int.Parse(DdlEfecto.SelectedValue));
                    DdlRespuesta.DataTextField = "Descripcion";
                    DdlRespuesta.DataValueField = "Codigo";
                    DdlRespuesta.DataBind();
                    break;
                case 5:
                    DdlContacto.Items.Clear();
                    _contacto.Text = "--Seleccione Contacto--";
                    _contacto.Value = "0";
                    DdlContacto.Items.Add(_contacto);
                    DdlContacto.DataSource = new SpeechDAO().FunGetArbolNewContacto(int.Parse(DdlRespuesta.SelectedValue));
                    DdlContacto.DataTextField = "Descripcion";
                    DdlContacto.DataValueField = "Codigo";
                    DdlContacto.DataBind();
                    break;
                case 6:
                    TxtObservacion.Text = "";
                    TxtFechaPago.Enabled = false;
                    TxtFechaPago.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtValorAbono.Text = "";
                    TxtValorAbono.Enabled = false;
                    DdlTipoPago.SelectedValue = "0";
                    DdlTipoPago.Enabled = false;
                    TxtNumDocumento.Text = "";
                    TxtNumDocumento.Enabled = false;
                    TxtTelefono.Text = "";
                    TxtTelefono.Enabled = false;
                    DdlTipTelefono.SelectedValue = "0";
                    DdlTipTelefono.Enabled = false;
                    DdlPrefijo.SelectedValue = "0";
                    DdlPrefijo.Enabled = false;
                    break;
                case 7:
                    DdlPrefijo.DataSource = new ControllerDAO().FunGetParametroDetalle("PREFIJOS", "--Seleccione Prefijo--", "S");
                    DdlPrefijo.DataTextField = "Descripcion";
                    DdlPrefijo.DataValueField = "Codigo";
                    DdlPrefijo.DataBind();
                    break;
                case 8:
                    DivPagos.Visible = false;
                    DivLLamar.Visible = false;
                    ViewState["Pago"] = "N";
                    ViewState["Llamar"] = "N";
                    TxtFechaLLamar.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtHoraLLamar.Text = DateTime.Now.ToString("HH:mm");
                    TxtFechaPago.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtValorAbono.Text = "";
                    DdlTipoPago.SelectedValue = "0";
                    break;
                case 9:
                    TxtTelefono.Text = "";
                    DdlTipTelefono.SelectedValue = "0";
                    DdlPrefijo.SelectedValue = "0";
                    DdlPropietario2.SelectedValue = "0";
                    DdlAccionDel.SelectedValue = "0";
                    DdlRespuestaDel.SelectedValue = "0";
                    TxtNombres.Text = "";
                    TxtApellidos.Text = "";
                    TxtDocumentoRef.ReadOnly = false;
                    TxtDocumentoRef.Text = "";
                    TrFila1.Visible = false;
                    TrFila2.Visible = false;
                    ImgAddTelefono.Enabled = true;
                    ImgEditelefono.Enabled = false;
                    ViewState["Cambiar"] = "SI";
                    ViewState["DocumentoRef"] = "";
                    break;
                case 10:
                    DdlCitacion.DataSource = new ControllerDAO().FunGetParametroDetalle("CANCELAR CITACION", 
                        "--Seleccione Opcion--", "S");
                    DdlCitacion.DataTextField = "Descripcion";
                    DdlCitacion.DataValueField = "Codigo";
                    DdlCitacion.DataBind();
                    break;
            }
        }

        private string FunConsultarSpeech(int opcion, int cedecodigo, int cpcecodigo, int araccodigo, int arefcodigo,
            int arrecodigo, int arcocodigo, int spcacodigo)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        _dts = new ListaTrabajoDAO().FunGetSpeech(0, cedecodigo, cpcecodigo, 0, 0, 0, 0, 0);
                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            ViewState["CodigoSpeechCab"] = _dts.Tables[0].Rows[0]["CodigoSpeech"].ToString();
                        }
                        break;
                    case 1:
                        _dts = new ListaTrabajoDAO().FunGetSpeech(1, 0, 0, araccodigo, arefcodigo, arrecodigo, arcocodigo,
                            spcacodigo);

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            TxtObservacion.Text = _dts.Tables[0].Rows[0]["Observa"].ToString();
                            ViewState["CodigoSpeechAd"] = _dts.Tables[0].Rows[0]["CodigoSpeech"].ToString();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _txtspeech;
        }

        protected void ImgInformacion_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _operacion = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Operacion"].ToString();

            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; " +
                "var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                "window.open('WFrm_InfAdicional.aspx?CodigoCEDE=" + ViewState["CodigoCEDE"].ToString() +
                "&CodigoCPCE=" + Session["CodigoCPCE"].ToString() + "&CodigoPERS=" + ViewState["CodigoPERS"].ToString() +
                "&Operacion=" + _operacion + "',null,'left=' + posicion_x + " +
                "', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, " +
                "toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        private void FunClearObject()
        {
            _validar = true;
            _imgeditar.Enabled = false;
            GrdvDatos.DataSource = null;
            GrdvDatos.DataBind();
            pnlDatosObligacion.Height = 20;
            FunCargarCombos(0);
            TxtObservacion.Text = "";
            TxtFechaPago.Text = DateTime.Now.ToString("MM/dd/yyyy");
            TxtValorAbono.Text = "";
            DdlTipoPago.SelectedValue = "0";
            TxtNumDocumento.Text = "";
            TxtFechaPago.Enabled = false;
            TxtValorAbono.Enabled = false;
            DdlTipoPago.Enabled = false;
            TxtNumDocumento.Enabled = false;
            TxtTelefono.Text = "";
            DdlTipTelefono.SelectedValue = "0";
            DdlPrefijo.SelectedValue = "0";
            DdlPropietario2.SelectedValue = "0";
            TxtTelefono.Enabled = false;
            DdlTipTelefono.Enabled = false;
            DdlPrefijo.Enabled = false;
            DdlPropietario2.Enabled = false;
            GrdvDatos.DataSource = null;
            GrdvDatos.DataBind();
            GrdvGestiones.DataSource = null;
            GrdvGestiones.DataBind();
            GrdvTelefonos.DataSource = null;
            GrdvTelefonos.DataBind();
            Session["TrackNumber"] = "";
            ViewState["Pago"] = "NO";
            ViewState["Llamar"] = "NO";
            ViewState["VerGestiones"] = "1";
            ViewState["Telefono"] = "";
            LnkGestiones.Text = "Todas las Gestiones";
        }

        private bool FunValidarTelefonos()
        {
            try
            {
                if (DdlTipTelefono.SelectedValue == "CL")
                {
                    if (TxtTelefono.Text.Trim().Length != 10) _validar = false;

                    if (TxtTelefono.Text.Trim().Length == 10)
                    {
                        if (TxtTelefono.Text.Trim().Substring(0, 2) != "09") _validar = false;
                    }
                }

                if (DdlTipTelefono.SelectedValue == "CN")
                {
                    if (TxtTelefono.Text.Trim().Length != 7) _validar = false;

                    if (TxtTelefono.Text.Trim().Length == 7)
                    {
                        if (TxtTelefono.Text.Trim().Substring(0, 2) == "09") _validar = false;
                        if (TxtTelefono.Text.Trim().Substring(0, 1) == "0") _validar = false;
                        if (TxtTelefono.Text.Trim().Substring(0, 1) == "1") _validar = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _validar;
        }

        private void FunConsultarGestiones(int opcion)
        {
            try
            {
                if (opcion == 0) _dts = new ConsultaDatosDAO().FunConsultaDatos(40, int.Parse(ViewState["CodigoCLDE"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());

                if (opcion == 1) _dts = new ConsultaDatosDAO().FunConsultaDatos(41, int.Parse(ViewState["CodigoCLDE"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                GrdvGestiones.DataSource = _dts;
                GrdvGestiones.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        public void FunEjecutarMarcado()
        {
            try
            {
                _sumarsegundos = double.Parse(ViewState["TiempoMarcado"].ToString());
                Session["InicioLlamada"] = DateTime.Now.AddSeconds(_sumarsegundos).ToString("HH:mm:ss");
                Thread.Sleep(100);
                Session["TrackNumber"] = FunDial(ViewState["Telefono"].ToString());
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        public string FunDial(string telefono)
        {
            try
            {
                if (ViewState["TipoTelefono"].ToString() == "CN") telefono = ViewState["Prefijo"].ToString() + telefono;

                _strrespuesta = new ElastixDAO().ElastixDial(Session["IPLocalAdress"].ToString(), 9999, telefono);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _strrespuesta;
        }

        protected void ChkBCast_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                _chktelefec = (CheckBox)(_gvrow.Cells[8].FindControl("ChkBCast"));
                _codigotece = int.Parse(GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_TELEFONOS_CEDENTE _original = _db.SoftCob_TELEFONOS_CEDENTE.Where(t => t.TECE_CODIGO ==
                    _codigotece).FirstOrDefault();
                    _db.SoftCob_TELEFONOS_CEDENTE.Attach(_original);
                    _original.tece_auxi3 = _chktelefec.Checked ? 1 : 0;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvBrenchGestor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _porcumplido = decimal.Parse(GrdvBrenchGestor.DataKeys[e.Row.RowIndex].Values["PorCumplido"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(216, int.Parse(Session["CodigoCPCE"].ToString()), 1,
                        0, "", _porcumplido.ToString(), "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        e.Row.Cells[6].Text = _dts.Tables[0].Rows[0]["Etiqueta"].ToString();
                        e.Row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml(_dts.Tables[0].Rows[0]["Color"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void GrdvBrenchPago_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _porcumplido = decimal.Parse(GrdvBrenchPago.DataKeys[e.Row.RowIndex].Values["PorCumplido"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(216, int.Parse(Session["CodigoCPCE"].ToString()), 1,
                        0, "", _porcumplido.ToString(), "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        e.Row.Cells[6].Text = _dts.Tables[0].Rows[0]["Etiqueta"].ToString();
                        e.Row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml(_dts.Tables[0].Rows[0]["Color"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEquifax_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _operacion = GrdvDatosGarante.DataKeys[gvRow.RowIndex].Values["Operacion"].ToString();

            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; " +
                "var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                "window.open('WFrm_DatosEquifax.aspx?CodigoPERS=" + ViewState["CodigoPERS"].ToString() +
                "&Operacion=" + _operacion + "',null,'left=' + posicion_x + " +
                "', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, " +
                "toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void ImgEditGarante_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _cedulagarante = GrdvDatosGarante.DataKeys[gvRow.RowIndex].Values["CedulaGarante"].ToString();
            _codigogarante = GrdvDatosGarante.DataKeys[gvRow.RowIndex].Values["CodigoGARA"].ToString();

            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                "posicion_y=(screen.height/2)-(600/2); window.open('../Cedente/WFrm_UpdateGarante.aspx?CedulaTitular=" +
                ViewState["NumeroDocumento"].ToString() + "&CedulaGarante=" + _cedulagarante + "&CodigoGARA=" + _codigogarante + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=500px, " +
                "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        private void FunCargarPerfilUsuario(int codperfil)
        {
            try
            {
                _dts = new ControllerDAO().FunGetPerfilUsuarios(codperfil);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["PerfilActitudinal"] = _dts.Tables[0].Rows[0]["PerfActitudinal"].ToString();
                    ViewState["EstilosNegociacion"] = _dts.Tables[0].Rows[0]["EstilosNegocia"].ToString();
                    ViewState["Metaprogramas"] = _dts.Tables[0].Rows[0]["Metaprogramas"].ToString();
                    ViewState["Modalidades"] = _dts.Tables[0].Rows[0]["Modalidades"].ToString();
                    ViewState["EstadosDelYo"] = _dts.Tables[0].Rows[0]["EstadosYo"].ToString();
                    ViewState["Impulsores"] = _dts.Tables[0].Rows[0]["Impulsores"].ToString();

                    if (ViewState["PerfilActitudinal"].ToString() == "True" || ViewState["EstilosNegociacion"].ToString() == "True"
                        || ViewState["Metaprogramas"].ToString() == "True" || ViewState["Modalidades"].ToString() == "True"
                        || ViewState["EstadosDelYo"].ToString() == "True" || ViewState["Impulsores"].ToString() == "True")
                    {
                        ChkPerfilDeudor.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarVariablesBlandas()
        {
            try
            {
                _dts = new ListaTrabajoDAO().FunGetPerfilActitudinal();

                if (_dts.Tables[0].Rows.Count > 0) FunAddVariableBlanda(_dts, pchPerfilActitudinal);

                _dts = new ListaTrabajoDAO().FunGetEstilosNegociacion();

                if (_dts.Tables[0].Rows.Count > 0) FunAddVariableBlanda(_dts, pchEstilosNegociacion);

                _dts = new ListaTrabajoDAO().FunGetMetaprogramas();

                if (_dts.Tables[0].Rows.Count > 0) FunAddVariableBlanda(_dts, pchMetaprogramas);

                _dts = new ListaTrabajoDAO().FunGetModalidades();

                if (_dts.Tables[0].Rows.Count > 0) FunAddVariableBlanda(_dts, pchModalidades);

                _dts = new ListaTrabajoDAO().FunGetEstadosYo();

                if (_dts.Tables[0].Rows.Count > 0) FunAddVariableBlanda(_dts, pchEstadosYo);
                _dts = new ListaTrabajoDAO().FunGetImpulsores();

                if (_dts.Tables[0].Rows.Count > 0) FunAddVariableBlanda(_dts, pchImpulsores);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunAddVariableBlanda(DataSet dts, PlaceHolder pchVarBla)
        {
            Literal table = new Literal();
            table.Text = "<table>";
            pchVarBla.Controls.Add(table);
            foreach (DataRow variable in dts.Tables[0].Rows)
            {
                Literal rowini = new Literal();
                rowini.Text = "<tr><td>";
                pchVarBla.Controls.Add(rowini);

                RadioButton rbtOpcion = new RadioButton();
                rbtOpcion.ID = pchVarBla.ID + "_" + variable["Codigo"].ToString();
                rbtOpcion.GroupName = pchVarBla.ID;
                rbtOpcion.Text = variable["Descripcion"].ToString();
                pchVarBla.Controls.Add(rbtOpcion);

                Literal rowfin = new Literal();
                rowfin.Text = "</tr></td>";
                pchVarBla.Controls.Add(rowfin);
            }

            Literal fintable = new Literal();
            fintable.Text = "</table>";
            pchVarBla.Controls.Add(fintable);
        }

        private void FunCargarValoresVariablesBlandas()
        {
            try
            {
                //int LperfilActitudinal;
                //int LestiloNegociacion;
                //int Lmetaprograma;
                //int Lmodalidad;
                //int LestadoYo;
                //int Limpulsor;
                FunObtenerPerfilDeudor(out int LperfilActitudinal, out int LestiloNegociacion, out int Lmetaprograma,
                    out int Lmodalidad, out int LestadoYo, out int Limpulsor);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunObtenerPerfilDeudor(out int LperfilActitudinal, out int LestiloNegociacion, out int Lmetaprograma,
            out int Lmodalidad, out int LestadoYo, out int Limpulsor)
        {
            LperfilActitudinal = 0;
            LestiloNegociacion = 0;
            Lmetaprograma = 0;
            Lmodalidad = 0;
            LestadoYo = 0;
            Limpulsor = 0;
            try
            {
                _dts = new ListaTrabajoDAO().FunGetPerfilDeudor(int.Parse(ViewState["CodigoPERS"].ToString()));

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    LperfilActitudinal = int.Parse(_dts.Tables[0].Rows[0]["PerfActitudinal"].ToString());

                    if (LperfilActitudinal > 0) FunConfigurarValorSeleccionado(LperfilActitudinal, pchPerfilActitudinal);

                    LestiloNegociacion = int.Parse(_dts.Tables[0].Rows[0]["EstilosNegocia"].ToString());

                    if (LestiloNegociacion > 0) FunConfigurarValorSeleccionado(LestiloNegociacion, pchEstilosNegociacion);

                    Lmetaprograma = int.Parse(_dts.Tables[0].Rows[0]["Metaprogramas"].ToString());

                    if (Lmetaprograma > 0) FunConfigurarValorSeleccionado(Lmetaprograma, pchMetaprogramas);

                    Lmodalidad = int.Parse(_dts.Tables[0].Rows[0]["Modalidades"].ToString());

                    if (Lmodalidad > 0) FunConfigurarValorSeleccionado(Lmodalidad, pchModalidades);

                    LestadoYo = int.Parse(_dts.Tables[0].Rows[0]["EstadosYo"].ToString());

                    if (LestadoYo > 0) FunConfigurarValorSeleccionado(LestadoYo, pchEstadosYo);

                    Limpulsor = int.Parse(_dts.Tables[0].Rows[0]["Impulsores"].ToString());

                    if (Limpulsor > 0) FunConfigurarValorSeleccionado(Limpulsor, pchImpulsores);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunConfigurarValorSeleccionado(int valor, PlaceHolder pchVarBla)
        {
            try
            {
                if (pchVarBla.HasControls())
                {
                    foreach (Control c in pchVarBla.Controls)
                    {
                        Type type = c.GetType();
                        if (type.FullName == "System.Web.UI.WebControls.RadioButton")
                        {
                            if (((RadioButton)c).ID.IndexOf(valor.ToString()) != -1)
                                ((RadioButton)c).Checked = true;
                            else
                                continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private string FunObtenerValorSeleccionado(PlaceHolder pchVarBla)
        {
            try
            {
                if (pchVarBla.HasControls())
                {
                    foreach (Control c in pchVarBla.Controls)
                    {
                        Type type = c.GetType();
                        if (type.FullName == "System.Web.UI.WebControls.RadioButton")
                        {
                            if (((RadioButton)c).Checked)
                                _valor = ((RadioButton)c).Text;
                            else
                                continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _valor;
        }

        private int FunObtenerIDValorSeleccionado(PlaceHolder pchVarBla)
        {
            try
            {
                _id = 0;
                if (pchVarBla.HasControls())
                {
                    foreach (Control c in pchVarBla.Controls)
                    {
                        Type type = c.GetType();
                        if (type.FullName == "System.Web.UI.WebControls.RadioButton")
                        {
                            if (((RadioButton)c).Checked)
                            {
                                _valores = ((RadioButton)c).ID.Split(new char[] { '_' });
                                _id = int.Parse(_valores[1]);
                                break;
                            }
                            else
                                continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _id;
        }
        #endregion

        #region Botones y Eventos
        protected void DdlTipTelefono_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DdlPrefijo.Items.Clear();
                _prefijo.Text = "--Seleccione Prefijo--";
                _prefijo.Value = "0";
                DdlPrefijo.Items.Add(_prefijo);
                if (DdlTipTelefono.SelectedValue == "CN") FunCargarCombos(7);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgCitacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('../BPM/WFrm_NewCitacion.aspx?CodigoPERS=" + ViewState["CodigoPERS"].ToString() + "&CodigoCLDE=" + ViewState["CodigoCLDE"].ToString() + "&Retornar=1" + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlAccionDel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FunCargarCombos(10);
        }

        protected void DdlAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtObservacion.Text = "";
                FunCargarCombos(8);
                DdlEfecto.Items.Clear();
                _efecto.Text = "--Seleccione Efecto--";
                _efecto.Value = "0";
                DdlEfecto.Items.Add(_efecto);
                DdlRespuesta.Items.Clear();
                _respuesta.Text = "--Seleccione Respuesta--";
                _respuesta.Value = "0";
                DdlRespuesta.Items.Add(_respuesta);
                DdlContacto.Items.Clear();
                _contacto.Text = "--Seleccione Contacto--";
                _contacto.Value = "0";
                DdlContacto.Items.Add(_contacto);

                if (DdlAccion.SelectedValue != "0") FunCargarCombos(3);

                _dts = new ListaTrabajoDAO().FunGetSpeech(0, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), 0, 0, 0, 0, 0);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoSpeechCab"] = _dts.Tables[0].Rows[0]["CodigoSpeech"].ToString();
                    FunConsultarSpeech(1, 0, 0, int.Parse(DdlAccion.SelectedValue), 0, 0, 0,
                        int.Parse(ViewState["CodigoSpeechCab"].ToString()));
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlEfecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtObservacion.Text = "";
                DdlRespuesta.Items.Clear();
                _respuesta.Text = "--Seleccione Respuesta--";
                _respuesta.Value = "0";
                DdlRespuesta.Items.Add(_respuesta);
                DdlContacto.Items.Clear();
                _contacto.Text = "--Seleccione Contacto--";
                _contacto.Value = "0";
                DdlContacto.Items.Add(_contacto);
                FunCargarCombos(8);
                FunCargarCombos(4);

                _dts = new ListaTrabajoDAO().FunGetSpeech(0, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), 0, 0, 0, 0, 0);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoSpeechCab"] = _dts.Tables[0].Rows[0]["CodigoSpeech"].ToString();
                    FunConsultarSpeech(1, 0, 0, int.Parse(DdlAccion.SelectedValue), int.Parse(DdlEfecto.SelectedValue),
                        0, 0, int.Parse(ViewState["CodigoSpeechCab"].ToString()));
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlRespuesta_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtObservacion.Text = "";
                DdlContacto.Items.Clear();
                _contacto.Text = "--Seleccione Contacto--";
                _contacto.Value = "0";
                DdlContacto.Items.Add(_contacto);
                FunCargarCombos(8);
                FunCargarCombos(5);

                _dts = new ListaTrabajoDAO().FunGetSpeech(0, int.Parse(ViewState["CodigoCEDE"].ToString()),
                        int.Parse(Session["CodigoCPCE"].ToString()), 0, 0, 0, 0, 0);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoSpeechCab"] = _dts.Tables[0].Rows[0]["CodigoSpeech"].ToString();
                    FunConsultarSpeech(1, 0, 0, int.Parse(DdlAccion.SelectedValue), int.Parse(DdlEfecto.SelectedValue),
                        int.Parse(DdlRespuesta.SelectedValue), 0, int.Parse(ViewState["CodigoSpeechCab"].ToString()));
                }

                _pago = new ListaTrabajoDAO().FunGetValorRespuesta(int.Parse(DdlRespuesta.SelectedValue), 0);
                _llamar = new ListaTrabajoDAO().FunGetValorRespuesta(int.Parse(DdlRespuesta.SelectedValue), 1);

                if (_pago)
                {
                    DivPagos.Visible = true;
                    ViewState["Pago"] = "S";
                }

                if (_llamar)
                {
                    DivLLamar.Visible = true;
                    ViewState["Llamar"] = "S";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlTipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtNumDocumento.Text = "";
        }

        protected void ImgArbol_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtCedula.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Cedula a Consultar..", this);
                    return;
                }

                if (TxtCedula.Text.Trim().Length < 10)
                {
                    new FuncionesDAO().FunShowJSMessage("Dato tiene menos de 10 digitos..", this);
                    return;
                }

                Session["CedulaCookie"] = TxtCedula.Text.Trim();
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; " +
                    "var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                    "window.open('WFrm_ArbolRecursivo.aspx?Cedula=" + TxtCedula.Text.Trim() + "',null,'left=' + posicion_x + " +
                    "', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, " +
                    "toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["Pago"].ToString() == "S")
                {
                    if (TxtFechaPago.Text.Trim() == "")
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese Fecha de Pago..!", this);
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtValorAbono.Text.Trim()) || TxtValorAbono.Text.Trim() == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese valor del pago..!", this);
                        return;
                    }

                    if (DdlTipoPago.SelectedValue != "0")
                    {
                        if (string.IsNullOrEmpty(TxtNumDocumento.Text.Trim()))
                        {
                            new FuncionesDAO().FunShowJSMessage("Ingrese No. de Documento..!", this);
                            return;
                        }
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(240, int.Parse(ViewState["CodigoPERS"].ToString()),
                        int.Parse(DdlContacto.SelectedValue), int.Parse(Session["CodigoCPCE"].ToString()), "", TxtFechaPago.Text,
                        TxtValorAbono.Text, Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Cliente ya tien regitrdo un COMPROMISO en esa Fecha " +
                            "Verifique el registro o Consulte con el Administrador..!", this);
                        return;
                    }
                }

                if (ViewState["Llamar"].ToString() == "S")
                {
                    if (string.IsNullOrEmpty(TxtHoraLLamar.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese Hora de llamada/ sumando 10 minutos a la hora actual..!", this);
                        return;
                    }

                    DateTime LdtmAhora = DateTime.Now.AddMinutes(int.Parse(ViewState["MinutosLlamar"].ToString()));
                    _horallamar = DateTime.Parse(TxtHoraLLamar.Text).ToString("HH:mm");
                    DateTime dtmFechaLLamar = DateTime.ParseExact(String.Format("{0} {1}", TxtFechaLLamar.Text, _horallamar), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);

                    if (dtmFechaLLamar < DateTime.Now.AddMinutes(10))
                    {
                        new FuncionesDAO().FunShowJSMessage("La Fecha/Hora no puede ser menor a la actual..!", this);
                        return;
                    }
                }

                if (DdlAccion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Acción..!", this);
                    return;
                }

                if (DdlEfecto.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Efecto..!", this);
                    return;
                }

                if (DdlRespuesta.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Respuesta..!", this);
                    return;
                }

                if (DdlContacto.Visible == true)
                {
                    if (DdlContacto.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Contacto..!", this);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(TxtObservacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Observación de la Gestión..!", this);
                    return;
                }

                if (ViewState["Telefono"] == null || ViewState["Telefono"].ToString() == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione número de teléfono o ingrese nuevo..!", this);
                    return;
                }

                if (Chkcitacion.Checked)
                {
                    if (DdlCitacion.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Opcion de Cancelacion..!", this);
                        return;
                    }

                    _dts = new ConsultaDatosDAO().FunAgendaCitaciones(8, int.Parse(ViewState["CodigoCLDE"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), DateTime.Now.ToString("MM/dd/yyyy"),
                        0, "0", int.Parse(Session["usuCodigo"].ToString()), DdlCitacion.SelectedItem.ToString(), "", "",
                        "", "", "", "", "", "", "", new byte[0], "", "", "", "", "", "0", 0, "", 0, DdlCitacion.SelectedValue,
                        "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());
                }

                _dtbarbolrespuesta = (DataTable)ViewState["ArbolContactoEfectivo"];
                _result = _dtbarbolrespuesta.Select("Codigo='" + DdlRespuesta.SelectedValue + "'").FirstOrDefault();
                _efectivo = bool.Parse(_result["Contacto"].ToString());

                _fechapago = TxtFechaPago.Text.Trim() == "" ? DateTime.Now.ToString("MM/dd/yyyy") : TxtFechaPago.Text;
                _valorpago = TxtValorAbono.Text.Trim() == "" ? "0.00" : TxtValorAbono.Text;
                _fechallamar = TxtFechaLLamar.Text.Trim() == "" ? DateTime.Now.ToString("MM/dd/yyyy") : TxtFechaLLamar.Text;
                _horallamar = TxtHoraLLamar.Text.Trim() == "" ? "00:00:00" : TxtHoraLLamar.Text.Trim();
                _tipopago = DdlTipoPago.SelectedValue != "0" ? DdlTipoPago.SelectedValue : "";

                _lintperfilactitudinal = FunObtenerIDValorSeleccionado(pchPerfilActitudinal);
                _lintestilonegociacion = FunObtenerIDValorSeleccionado(pchEstilosNegociacion);
                _lintmetaprograma = FunObtenerIDValorSeleccionado(pchMetaprogramas);
                _lintmodalidad = FunObtenerIDValorSeleccionado(pchModalidades);
                _lintestadoyo = FunObtenerIDValorSeleccionado(pchEstadosYo);
                _lintimpulsor = FunObtenerIDValorSeleccionado(pchImpulsores);
                _tiempogestion = TimeSpan.Parse(ViewState["InicioGestion"].ToString());
                _tiempollamada = TimeSpan.Parse(Session["InicioLlamada"].ToString());

                _tiempofingestion = DateTime.Now.TimeOfDay;
                _tgestion = _tiempofingestion - _tiempogestion;

                if (_tiempollamada > _tiempofingestion) _tllamada = _tiempollamada - _tiempofingestion;
                else _tllamada = _tiempofingestion - _tiempollamada;

                ViewState["TiempoGestion"] = _tgestion.Hours.ToString("00") + ":" + _tgestion.Minutes.ToString("00") + ":" + _tgestion.Seconds.ToString("00");
                ViewState["TiempoLLamada"] = _tllamada.Hours.ToString("00") + ":" + _tllamada.Minutes.ToString("00") + ":" + _tllamada.Seconds.ToString("00");
                _totalgestion = ((_tgestion.Hours * 60) * 60) + (_tgestion.Minutes * 60) + _tgestion.Seconds;
                _totallamada = ((_tllamada.Hours * 60) * 60) + (_tllamada.Minutes * 60) + _tllamada.Seconds;

                SoftCob_LOGUEO_TIEMPOS loguintime = new SoftCob_LOGUEO_TIEMPOS();
                {
                    loguintime.USUA_CODIGO = int.Parse(Session["usuCodigo"].ToString());
                    loguintime.empr_codigo = int.Parse(Session["CodigoEMPR"].ToString());
                    loguintime.cpce_codigo = int.Parse(Session["CodigoCPCE"].ToString());
                    loguintime.ltca_codigo = _codigoltca;
                    loguintime.loti_tipologueo = "SGE";
                    _fechalogueo = DateTime.Now.ToString("MM/dd/yyyy");
                    loguintime.loti_fechalogueo = DateTime.ParseExact(_fechalogueo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    _horalogueo = DateTime.Now.ToString("HH:mm:ss");
                    loguintime.loti_horalogueo = TimeSpan.Parse(_horalogueo);
                    loguintime.loti_fechacompleta = DateTime.Now;
                    loguintime.loti_auxv1 = "";
                    loguintime.loti_auxv2 = "";
                    loguintime.loti_auxv3 = "";
                    loguintime.loti_auxv4 = "";
                    loguintime.loti_auxi1 = 0;
                    loguintime.loti_auxi2 = 0;
                    loguintime.loti_auxi3 = 0;
                    loguintime.loti_auxi4 = 0;
                    loguintime.loti_auxd1 = DateTime.Now;
                    loguintime.loti_auxd2 = DateTime.Now;
                    loguintime.loti_auxd3 = DateTime.Now;
                    loguintime.loti_auxd4 = DateTime.Now;
                    loguintime.loti_fechacreacion = DateTime.Now;
                    loguintime.loti_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    loguintime.loti_terminalcreacion = Session["MachineName"].ToString();
                    new ControllerDAO().FunCrearLogueoTiempos(loguintime);
                }

                if (new FuncionesDAO().FunDesencripta(Session["Phone"].ToString()) == "SiActivado")
                {
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                }

                string[] columnas = new[] { "Operacion", "DiasMora" };
                _dtbgestion = (DataTable)ViewState["DatosObligacion"];
                DataView view = new DataView(_dtbgestion);
                _dtbgestion = view.ToTable(true, columnas);
                _mensaje = new GestionTelefonicaDAO().FunRegistrarGestionEntrante(int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), 0, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    int.Parse(ViewState["CodigoCLDE"].ToString()), int.Parse(ViewState["CodigoPERS"].ToString()),
                    ViewState["NumeroDocumento"].ToString(), ViewState["Operacion"].ToString(),
                    int.Parse(Session["usuCodigo"].ToString()), ViewState["Telefono"].ToString(),
                    ViewState["TipoTelefono"].ToString(), ViewState["Propietario"].ToString(),
                    int.Parse(DdlAccion.SelectedValue), int.Parse(DdlEfecto.SelectedValue),
                    int.Parse(DdlRespuesta.SelectedValue), int.Parse(DdlContacto.SelectedValue),
                    TxtObservacion.Text.Trim().ToUpper(), _efectivo, DateTime.Now.ToString("MM/dd/yyyy"),
                    DateTime.Now.ToString("HH:mm:ss"), Session["TrackNumber"].ToString(),
                    ViewState["TiempoGestion"].ToString(), ViewState["TiempoLLamada"].ToString(), _totalgestion,
                    _totallamada, ViewState["Pago"].ToString(), _fechapago, _valorpago.Replace(",", "."),
                    ViewState["Llamar"].ToString(), _fechallamar, _horallamar, ViewState["Telefono"].ToString(),
                    _lintperfilactitudinal, _lintestilonegociacion, _lintmetaprograma, _lintmodalidad, _lintestadoyo,
                    _lintimpulsor, ViewState["Prefijo"].ToString(), _tipopago, TxtNumDocumento.Text.Trim(), "", 0, 0, 0,
                    0, _dtbgestion, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                    "sp_NewGestionEntrante", Session["Conectar"].ToString());

                if (_mensaje == "")
                {
                    _redirect = string.Format("{0}?CodigoCEDE={1}&CodigoCPCE={2}&CodigoCLDE={3}&CodigoPERS={4}" +
                        "&NumeroDocumento={5}&Operacion={6}&CodigoLTCA={7}&CodigoUSU={8}&Retornar={9}" +
                        "&MensajeRetornado={10}", Request.Url.AbsolutePath, ViewState["CodigoCEDE"].ToString(), Session["CodigoCPCE"].ToString(), ViewState["CodigoCLDE"].ToString(), ViewState["CodigoPERS"].ToString(), ViewState["NumeroDocumento"].ToString(), ViewState["Operacion"].ToString(), ViewState["CodigoLTCA"].ToString(), ViewState["CodigoUSU"].ToString(), ViewState["Retornar"].ToString(), "Grabado con Éxito..");
                    Response.Redirect(_redirect);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlContacto_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtObservacion.Text = "";
            _dts = new ListaTrabajoDAO().FunGetSpeech(0, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), 0, 0, 0, 0, 0);
            if (_dts.Tables[0].Rows.Count > 0)
            {
                ViewState["CodigoSpeechCab"] = _dts.Tables[0].Rows[0]["CodigoSpeech"].ToString();
                FunConsultarSpeech(1, 0, 0, int.Parse(DdlAccion.SelectedValue), int.Parse(DdlEfecto.SelectedValue),
                    int.Parse(DdlRespuesta.SelectedValue), int.Parse(DdlContacto.SelectedValue), int.Parse(ViewState["CodigoSpeechCab"].ToString()));
            }
        }

        protected void LnkGestiones_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["VerGestiones"].ToString() == "1")
                {
                    ViewState["VerGestiones"] = "0";
                    LnkGestiones.Text = "Últimas Tres Gestiones";
                    FunConsultarGestiones(1);
                }
                else
                {
                    FunConsultarGestiones(0);
                    ViewState["VerGestiones"] = "1";
                    LnkGestiones.Text = "Todas las Gestiones";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgCall_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (new FuncionesDAO().FunDesencripta(Session["Phone"].ToString()) == "SiActivado")
                {
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                }

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in GrdvTelefonos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvTelefonos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _imgcall = (ImageButton)(gvRow.Cells[5].FindControl("imgCall"));
                _imgcall.ImageUrl = "~/Botones/call_small_disabled.png";
                ViewState["Telefono"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Telefono"].ToString();
                ViewState["Prefijo"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Prefijo"].ToString();
                ViewState["TipoTelefono"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["CodTipo"].ToString();
                ViewState["Propietario"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["CodPro"].ToString();
                _thrmarcar = new Thread(new ThreadStart(FunEjecutarMarcado));
                _thrmarcar.Start();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgBuscarAarbol_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                Session["CedulaCookie"] = GrdvDatosGarante.DataKeys[gvRow.RowIndex].Values["CedulaGarante"].ToString();
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                    "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_ArbolRecursivo.aspx?Cedula=" + Session["CedulaCookie"].ToString() +
                    "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=650px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelectT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                
                foreach (GridViewRow fr in GrdvTelefonos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                TrFila1.Visible = false;
                TrFila2.Visible = false;

                ImgAddTelefono.Enabled = false;
                ImgEditelefono.Enabled = true;
                TxtDocumentoRef.ReadOnly = true;

                DdlPrefijo.Items.Clear();
                _prefijo.Text = "--Seleccione Prefijo--";
                _prefijo.Value = "0";
                DdlPrefijo.Items.Add(_prefijo);
                GrdvTelefonos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = int.Parse(GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                ViewState["CodigoTelefono"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ViewState["Telefonoanterior"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Telefono"].ToString();
                ViewState["Telefono"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Telefono"].ToString();
                TxtTelefono.Text = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Telefono"].ToString();
                DdlTipTelefono.SelectedValue = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["CodTipo"].ToString();
                DdlPropietario2.SelectedValue = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["CodPro"].ToString();
                ViewState["TipoTelefono"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["CodTipo"].ToString();
                ViewState["Propietario"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["CodPro"].ToString();
                ViewState["Prefijo"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Prefijo"].ToString();

                _dtbtelefonos = (DataTable)ViewState["TelefonosRegistrados"];
                _result = _dtbtelefonos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtNombres.Text = _result["Nombres"].ToString();
                TxtApellidos.Text = _result["Apellidos"].ToString();

                TxtDocumentoRef.Text = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["NumDocumento"].ToString();
                ViewState["DocumentoRef"] = TxtDocumentoRef.Text;
                ViewState["InicioLlamada"] = DateTime.Now.ToString("HH:mm:ss");
                _sumarsegundos = double.Parse(ViewState["TiempoMarcado"].ToString());
                Session["InicioLlamada"] = DateTime.Now.AddSeconds(_sumarsegundos).ToString("HH:mm:ss");

                if (ViewState["Propietario"].ToString() != "DE")
                {
                    TrFila1.Visible = true;
                    TrFila2.Visible = true;
                }

                if (DdlTipTelefono.SelectedValue == "CN")
                {
                    FunCargarCombos(7);

                    if (ViewState["Prefijo"].ToString() == "") ViewState["Prefijo"] = "02";

                    DdlPrefijo.SelectedValue = ViewState["Prefijo"].ToString();

                }
                else DdlPrefijo.SelectedValue = "0";

                ViewState["Cambiar"] = "NO";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddTelefono_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlTipTelefono.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione tipo teléfono..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese teléfono..!", this);
                    return;
                }

                if (DdlTipTelefono.SelectedValue == "CN")
                {
                    if (DdlPrefijo.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Prefijo de Marcación..!", this);
                        return;
                    }
                }

                if (DdlPropietario2.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Propietario..!", this);
                    return;
                }
                //validar si el telefono fue eliminado mas de dos veces
                _dts = new ConsultaDatosDAO().FunConsultaDatos(135, 0, 0, 0, "", DdlPrefijo.SelectedValue == "0" ?
                    TxtTelefono.Text.Trim() : DdlPrefijo.SelectedValue + TxtTelefono.Text.Trim(), "", Session["Conectar"].ToString());

                if (int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString()) >= 2)
                {
                    new FuncionesDAO().FunShowJSMessage("El Teléfono fue Eliminado más de una vez..!", this);
                    return;
                }

                if (FunValidarTelefonos())
                {
                    if (ViewState["TelefonosRegistrados"] != null)
                    {
                        _tblbuscar = (DataTable)ViewState["TelefonosRegistrados"];
                        _result = _tblbuscar.Select("Telefono='" + TxtTelefono.Text.Trim() + "'").FirstOrDefault();

                        if (_tblbuscar.Rows.Count > 0) _maxcodigo = _tblbuscar.AsEnumerable().Max(row => int.Parse((string)row["Codigo"]));
                        else _maxcodigo = 0;

                        if (_result != null) _lexiste = true;
                    }

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("Ya existe teléfono..!", this);
                        return;
                    }

                    _mensaje = new GestionTelefonicaDAO().FunConsultarFonoEstado(int.Parse(ViewState["CodigoCEDE"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), TxtTelefono.Text.Trim());

                    if (_mensaje != "")
                    {
                        new FuncionesDAO().FunShowJSMessage("El Teléfono se encuenta INACTIVO, solcite ACTIVAR el mismo..!", this);
                        return;
                    }

                    _codigo = 0;

                    if (DdlPropietario2.SelectedValue != "DE")
                    {

                        if (string.IsNullOrEmpty(TxtDocumentoRef.Text.Trim()))
                        {
                            new FuncionesDAO().FunShowJSMessage("Ingrese No. de Documento..!", this);
                            return;
                        }

                        if (string.IsNullOrEmpty(TxtNombres.Text.Trim()))
                        {
                            new FuncionesDAO().FunShowJSMessage("Ingrese Al Menos Nombre del Contacto..!", this);
                            return;
                        }

                        //VERIFICAR SI EL NUMERO DE DOCUMENTO YA EXISTE

                        _dts = new ConsultaDatosDAO().FunConsultaDatos(241, int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0, "",
                            TxtDocumentoRef.Text.Trim(), "", Session["Conectar"].ToString());

                        _codigo = int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString());

                        SoftCob_DEUDOR_REFERENCIAS addTelefonoref = new SoftCob_DEUDOR_REFERENCIAS();
                        {
                            addTelefonoref.DERE_CODIGO = _codigo;
                            addTelefonoref.pers_codigo = int.Parse(ViewState["CodigoPERS"].ToString());
                            addTelefonoref.dere_tiporeferencia = DdlPropietario2.SelectedValue;
                            addTelefonoref.dere_numdocumento = TxtDocumentoRef.Text;
                            addTelefonoref.dere_nombrereferencia = TxtNombres.Text.Trim().ToUpper();
                            addTelefonoref.dere_apellidoreferencia = TxtApellidos.Text.Trim().ToUpper();
                            addTelefonoref.dere_auxv1 = "";
                            addTelefonoref.dere_auxv2 = "";
                            addTelefonoref.dere_auxv3 = "";
                            addTelefonoref.dere_auxi1 = 0;
                            addTelefonoref.dere_auxi2 = 0;
                            addTelefonoref.dere_auxi3 = 0;
                            addTelefonoref.dere_fechacreacion = DateTime.Now;
                            addTelefonoref.dere_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                            addTelefonoref.dere_terminalcreacion = Session["MachineName"].ToString();
                            addTelefonoref.dere_fum = DateTime.Now;
                            addTelefonoref.dere_uum = int.Parse(Session["usuCodigo"].ToString());
                            addTelefonoref.dere_tum = Session["MachineName"].ToString();
                        }

                        if (_codigo == 0) _codigo = new GestionTelefonicaDAO().FunCrearTelefonoReferencia(addTelefonoref);
                        else new GestionTelefonicaDAO().FunModificarDeudorReferen(addTelefonoref);
                    }

                    SoftCob_TELEFONOS_CEDENTE addTelefono = new SoftCob_TELEFONOS_CEDENTE();
                    {
                        addTelefono.tece_cedecodigo = int.Parse(ViewState["CodigoCEDE"].ToString());
                        addTelefono.tece_perscodigo = int.Parse(ViewState["CodigoPERS"].ToString());
                        addTelefono.tece_referencodigo = _codigo;
                        addTelefono.tece_numero = TxtTelefono.Text.Trim();
                        addTelefono.tece_tipo = DdlTipTelefono.SelectedValue;
                        addTelefono.tece_propietario = DdlPropietario2.SelectedValue;
                        addTelefono.tece_score = 0;
                        addTelefono.tece_estado = true;
                        addTelefono.tece_auxv1 = DdlTipTelefono.SelectedValue == "CL" ? "" : DdlPrefijo.SelectedValue;
                        addTelefono.tece_auxv2 = "SI";
                        addTelefono.tece_auxv3 = "";
                        addTelefono.tece_auxi1 = 99;
                        addTelefono.tece_auxi2 = 0;
                        addTelefono.tece_auxi3 = 0;
                        addTelefono.tece_fechacreacion = DateTime.Now;
                        addTelefono.tece_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                        addTelefono.tece_terminalcreacion = Session["MachineName"].ToString();
                    }
                    new GestionTelefonicaDAO().FunCrearTelefonoCedente(addTelefono);

                    FunCargarCombos(9);

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(35, int.Parse(ViewState["CodigoCEDE"].ToString()),
                           int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "",
                           "", "", Session["Conectar"].ToString());

                    GrdvTelefonos.DataSource = _dts;
                    GrdvTelefonos.DataBind();
                    ViewState["TelefonosRegistrados"] = _dts.Tables[0];
                }
                else new FuncionesDAO().FunShowJSMessage("Teléfono incorrecto..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvGestiones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _tipo = GrdvGestiones.DataKeys[e.Row.RowIndex].Values["Tipo"].ToString();

                    if (_tipo == "C")
                    {
                        e.Row.Cells[0].BackColor = System.Drawing.Color.LightSeaGreen;
                        e.Row.Cells[1].BackColor = System.Drawing.Color.LightSeaGreen;
                        e.Row.Cells[2].BackColor = System.Drawing.Color.LightSeaGreen;
                        e.Row.Cells[3].BackColor = System.Drawing.Color.LightSeaGreen;
                        e.Row.Cells[4].BackColor = System.Drawing.Color.LightSeaGreen;
                        e.Row.Cells[5].BackColor = System.Drawing.Color.LightSeaGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvTelefonos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chktelefec = (CheckBox)(e.Row.Cells[7].FindControl("ChkTelEfec"));
                    _chkbcast = (CheckBox)(e.Row.Cells[8].FindControl("ChkBCast"));
                    _nuevo = GrdvTelefonos.DataKeys[e.Row.RowIndex].Values["Nuevo"].ToString();
                    _score = int.Parse(GrdvTelefonos.DataKeys[e.Row.RowIndex].Values["Score"].ToString());
                    _bcast = GrdvTelefonos.DataKeys[e.Row.RowIndex].Values["BCast"].ToString();

                    if (_nuevo == "SI") e.Row.Cells[2].BackColor = System.Drawing.Color.LightSeaGreen;

                    if (_score == 9) _chktelefec.Checked = true;
                    else _chktelefec.Checked = false;

                    if (_bcast == "SI") _chkbcast.Checked = true;
                }
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
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                _codigo = int.Parse(GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                _cedulagarante = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["NumDocumento"].ToString();
                _telefonoctc = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Telefono"].ToString();
                _sufijo = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Prefijo"].ToString();
                //_dtbtelefonos = (DataTable)ViewState["TelefonosRegistrados"];
                //_dtbtelefonos.Rows.RemoveAt(gvRow.RowIndex);

                FunCargarCombos(9);

                ViewState["Telefono"] = "";
                ViewState["Prefijo"] = "";
                ViewState["Propietario"] = "";
                ViewState["TipoTelefono"] = "";

                _dts = new ConsultaDatosDAO().FunConsultaDatos(88, _codigo, int.Parse(ViewState["CodigoPERS"].ToString()),
                    int.Parse(Session["usuCodigo"].ToString()), "TELEFONO EQUIVOCADO", _cedulagarante, _sufijo + _telefonoctc,
                    Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(35, int.Parse(ViewState["CodigoCEDE"].ToString()),
                       int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "",
                       "", "", Session["Conectar"].ToString());

                GrdvTelefonos.DataSource = _dts;
                GrdvTelefonos.DataBind();
                ViewState["TelefonosRegistrados"] = _dts.Tables[0];

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEditelefono_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlTipTelefono.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione tipo teléfono..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese teléfono..!", this);
                    return;
                }

                if (DdlTipTelefono.SelectedValue == "CN")
                {
                    if (DdlPrefijo.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Prefijo de Marcación..!", this);
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(135, int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0, "",
                    DdlPrefijo.SelectedValue == "0" ? TxtTelefono.Text.Trim() : DdlPrefijo.SelectedValue + TxtTelefono.Text.Trim(), "", Session["Conectar"].ToString());

                if (int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString()) >= 2)
                {
                    new FuncionesDAO().FunShowJSMessage("El Teléfono fue Eliminado más de una vez..!", this);
                    FunCargarCombos(9);
                    return;
                }

                if (FunValidarTelefonos())
                {
                    if (ViewState["TelefonosRegistrados"] != null)
                    {
                        if (ViewState["Telefonoanterior"].ToString() != TxtTelefono.Text.Trim())
                        {
                            _tblbuscar = (DataTable)ViewState["TelefonosRegistrados"];
                            _result = _tblbuscar.Select("Telefono='" + TxtTelefono.Text.Trim() + "'").FirstOrDefault();
                            _tblbuscar.DefaultView.Sort = "Codigo";
                            if (_result != null) _lexiste = true;
                        }
                    }

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("Ya existe teléfono..!", this);
                        return;
                    }

                    if (DdlPropietario2.SelectedValue != "DE")                    
                    {
                        if (string.IsNullOrEmpty(TxtDocumentoRef.Text.Trim()))
                        {
                            new FuncionesDAO().FunShowJSMessage("Ingrese No. de Documento..!", this);
                            return;
                        }

                        if (string.IsNullOrEmpty(TxtNombres.Text.Trim()))
                        {
                            new FuncionesDAO().FunShowJSMessage("Ingrese Al Menos Nombre del Contacto..!", this);
                            return;
                        }
                    }

                    //EDITAR TELEFONO CEDENTE Y REFERENCIAS
                    _mensaje = new ConsultaDatosDAO().FunEditarTelefonos(0, int.Parse(ViewState["CodigoPERS"].ToString()),
                        int.Parse(ViewState["CodigoTelefono"].ToString()), DdlTipTelefono.SelectedValue, DdlPropietario2.SelectedValue,
                        TxtNombres.Text.Trim().ToUpper(), TxtApellidos.Text.Trim().ToUpper(), TxtTelefono.Text.Trim(),
                        DdlPrefijo.SelectedValue == "0" ? "" : DdlPrefijo.SelectedValue, TxtDocumentoRef.Text, "", "", "",
                        int.Parse(ViewState["CodigoCEDE"].ToString()), 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());

                    FunCargarCombos(9);

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(35, int.Parse(ViewState["CodigoCEDE"].ToString()),
                           int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "",
                           "", "", Session["Conectar"].ToString());

                    GrdvTelefonos.DataSource = _dts;
                    GrdvTelefonos.DataBind();
                    ViewState["TelefonosRegistrados"] = _dts.Tables[0];
                }
                else new FuncionesDAO().FunShowJSMessage("Teléfono incorrecto..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlPropietario2_SelectedIndexChanged(object sender, EventArgs e)
        {
            TrFila1.Visible = false;
            TrFila2.Visible = false;

            if (DdlPropietario2.SelectedValue != "DE" && DdlPropietario2.SelectedValue != "0")
            {
                if (string.IsNullOrEmpty(ViewState["DocumentoRef"].ToString()))
                    TxtDocumentoRef.ReadOnly = false;
                TrFila1.Visible = true;
                TrFila2.Visible = true;
            }
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _totalcapital += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CVencido"));
                    _totalexigible += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Exigible"));
                    _totaldeuda += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "MontoGSPBO"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[6].Text = "TOTAL:";
                    e.Row.Cells[7].Text = _totaldeuda.ToString();
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[8].Text = _totalcapital.ToString();
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[9].Text = _totalexigible.ToString();
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkPerfilDeudor_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkPerfilDeudor.Checked)
            {
                DivPerfiles.Visible = true;

                if (ViewState["PerfilActitudinal"].ToString() == "True")
                {
                    divHeaderPerfil.Visible = true;
                    divContentPerfil.Visible = true;
                }

                if (ViewState["EstilosNegociacion"].ToString() == "True")
                {
                    divHeaderEsti.Visible = true;
                    divContentEsti.Visible = true;
                }

                if (ViewState["Metaprogramas"].ToString() == "True")
                {
                    divHeaderMeta.Visible = true;
                    divContentMeta.Visible = true;
                }

                if (ViewState["Modalidades"].ToString() == "True")
                {
                    divHeaderModali.Visible = true;
                    divContentModali.Visible = true;
                }

                if (ViewState["EstadosDelYo"].ToString() == "True")
                {
                    divHeaderEstados.Visible = true;
                    divContentEstados.Visible = true;
                }

                if (ViewState["Impulsores"].ToString() == "True")
                {
                    divHeaderImpul.Visible = true;
                    divContentImpul.Visible = true;
                }
            }
            else DivPerfiles.Visible = false;
        }

        protected void ImgSpeechBV_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('wFrm_SpeechBV.aspx?CodigoCEDE=" + ViewState["CodigoCEDE"].ToString() + "&CodigoCPCE=" + Session["CodigoCPCE"].ToString() + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=750px, height=400px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSpeechAD_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('wFrm_SpeechAD.aspx?CodigoCEDE=" + ViewState["CodigoCEDE"].ToString() +
                    "&CodigoCPCE=" + Session["CodigoCPCE"].ToString() + "&CodigoARAC=" + DdlAccion.SelectedValue + "&CodigoAREF=" + DdlEfecto.SelectedValue +
                    "&CodigoARRE=" + DdlRespuesta.SelectedValue + "&CodigoARCO=" + DdlContacto.SelectedValue + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=750px, height=400px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkTelEfec_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                _chktelefec = (CheckBox)(_gvrow.Cells[7].FindControl("ChkTelEfec"));
                _codigotece = int.Parse(GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_TELEFONOS_CEDENTE _original = _db.SoftCob_TELEFONOS_CEDENTE.Where(t => t.TECE_CODIGO ==
                    _codigotece).FirstOrDefault();
                    _db.SoftCob_TELEFONOS_CEDENTE.Attach(_original);
                    _original.tece_score = _chktelefec.Checked ? 9 : 0;
                    _db.SaveChanges();
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(35, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "", "", "",
                    Session["Conectar"].ToString());

                GrdvTelefonos.DataSource = _dts;
                GrdvTelefonos.DataBind();
                ViewState["TelefonosRegistrados"] = _dts.Tables[0];
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgNotas_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('wFrm_NotasGestion.aspx?CodigoCEDE=" + ViewState["CodigoCEDE"].ToString() +
                    "&CodigoCPCE=" + Session["CodigoCPCE"].ToString() + "&codigoPERS=" + ViewState["CodigoPERS"].ToString() +
                    "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=750px, height=400px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgBuscarFono_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('WFrm_BuscarTelefonos.aspx" +
                "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=650px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);

        }

        protected void ImgArbolGen_Click(object sender, ImageClickEventArgs e)
        {
            Session["CedulaCookie"] = ViewState["NumeroDocumento"].ToString();
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_ArbolRecursivo.aspx?Cedula=" + ViewState["NumeroDocumento"].ToString() +
                "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=650px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void Imgupdate_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                "posicion_y=(screen.height/2)-(600/2); window.open('../Cedente/WFrm_UpdateDeudor.aspx?CodigoPERS=" +
                ViewState["CodigoPERS"].ToString() + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=500px, " +
                "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void GrdvDatosDeudor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgphone = (ImageButton)(e.Row.Cells[6].FindControl("ImgPhoneD"));
                    _existe = GrdvDatosDeudor.DataKeys[e.Row.RowIndex].Values["Existe"].ToString();
                    if (_existe == "SI")
                    {
                        _imgphone.ImageUrl = "~/Botones/busqueda.png";
                        _imgphone.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgPhoneD_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            foreach (GridViewRow fr in GrdvTelefonos.Rows)
            {
                fr.Cells[0].BackColor = System.Drawing.Color.White;
            }
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_OtrosTelefonos.aspx?Cedula=" +
                ViewState["NumeroDocumento"].ToString() + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=500px, " +
                "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void GrdvDatosGarante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgphone = (ImageButton)(e.Row.Cells[6].FindControl("ImgPhoneG"));
                    _existe = GrdvDatosGarante.DataKeys[e.Row.RowIndex].Values["Existe"].ToString();
                    if (_existe == "SI")
                    {
                        _imgphone.ImageUrl = "~/Botones/busqueda.png";
                        _imgphone.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgPhoneG_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            foreach (GridViewRow fr in GrdvDatosGarante.Rows)
            {
                fr.Cells[0].BackColor = System.Drawing.Color.White;
            }
            GrdvDatosGarante.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
            _cedulagarante = GrdvDatosGarante.DataKeys[gvRow.RowIndex].Values["CedulaGarante"].ToString();

            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_OtrosTelefonos.aspx?Cedula=" +
                _cedulagarante + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=500px, " +
                "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void ImgComparar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; " +
                    "var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                    "window.open('../Breanch/WFrm_CompararBrench.aspx?CodigoCPCE=" + Session["CodigoCPCE"].ToString() +
                    "',null,'left=' + posicion_x + " +
                    "', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, " +
                    "toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState["Retornar"].ToString())
                {
                    case "0":
                        Response.Redirect("../ReportesManager/WFrm_ReporteCarteraGestor.aspx?codigoCEDE=" + ViewState["CodigoCEDE"].ToString() + "&codigoCPCE=" + Session["CodigoCPCE"].ToString() + "&codigoUSU=" + ViewState["CodigoUSU"].ToString());
                        break;
                    case "1":
                        Response.Redirect("WFrm_ListaClientesAdmin.aspx", true);
                        break;
                    case "2":
                        Response.Redirect("WFrm_ListaVolveraLLamar.aspx", true);
                        break;
                    case "3":
                        Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + ViewState["CodigoLTCA"].ToString(), true);
                        break;
                    case "4":
                        Response.Redirect("WFrm_ListaPagoAbonos.aspx", true);
                        break;
                    case "5":
                        Response.Redirect("../BPM/WFrm_SeguimientoCitacionAdmin.aspx", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}