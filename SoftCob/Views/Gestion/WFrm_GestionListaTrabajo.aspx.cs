namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_GestionListaTrabajo : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataSet _dtsx = new DataSet();
        DataSet _dtssegmento = new DataSet();
        DataSet _dtscontactos = new DataSet();
        ListItem _efecto = new ListItem();
        ListItem _respuesta = new ListItem();
        ListItem _respusetadel = new ListItem();
        ListItem _contacto = new ListItem();
        ListItem _prefijo = new ListItem();
        CheckBox _chkgestion = new CheckBox();
        CheckBox _chktelefec = new CheckBox();
        CheckBox _chkbcast = new CheckBox();
        ImageButton _imgeliminar = new ImageButton();
        ImageButton _imgmarcar = new ImageButton();
        ImageButton _imgeditar = new ImageButton();
        ImageButton _imgphone = new ImageButton();
        DataTable _dtbgestion = new DataTable();
        DataTable _dtbtelefonos = new DataTable();
        DataTable _dtbarbolaccion = new DataTable();
        DataTable _dtbarbolrespuesta = new DataTable();
        DataTable _dtbcontacto = new DataTable();
        DataTable _tblbuscar = new DataTable();
        DataTable _tblagre = new DataTable();
        DataRow _filagre;
        DataRow _result;

        string _operacion = "", _strrespuesta = "", _valor = "", _strtiempogestion = "", _strtiempollamada = "",
            _dtmfechaactual = "", _fechapago = "", _fechallamar = "", _valorpago = "", _horallamar = "", _mensaje = "",
            _redirect = "", _cedulagarante = "", _telefonoctc = "", _proxtelefono = "",
            _fechalogueo = "", _horalogueo = "", _txtspeech = "", _nuevo = "", _segmento = "", _existe = "", 
             _sufijo = "", _tipo = "", _codigogarante = "", _bcast = "";

        bool _llamar = false, _pago = false, _lexiste = false, _validar = true, _efectivo = false, _nummarcado = false,
            _asignarsig = false, _citacion = false, _mostrarpopup = false, _mostrararbol = false;

        int _codigo = 0, _maxcodigo = 0, _id = 0, _seg = 0, _min = 0, _hor = 0, _contador = 0, _segl = 0, _minl = 0, _horl = 0,
            _totalgestion = 0, _totallamada = 0, _lintperfilactitudinal = 0, _lintestilonegociacion = 0, _lintmetaprograma = 0,
            _lintmodalidad = 0, _lintestadoyo = 0, _lintimpulsor = 0, _contar = 0, _codigotece = 0, _score = 0,
            _opcion = 0, _day = 0;

        string[] _valores, _pathroot;

        DataRow _cambiar;
        DataRow[] _ctctelefonos;
        Thread _thrmarcar, _thrvolveramarcar;
        TimeSpan _tiempogestion, _tiempollamada, _tiempofingestion, _tgestion, _tllamada;
        decimal _totalexigible = 0.00M, _totalcapital = 0.00M, _totaldeuda = 0.00M, _porcumplido = 0.00M;
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
                _dtbcontacto.Columns.Add("Cliente");
                _dtbcontacto.Columns.Add("CodigoCLDE");
                _dtbcontacto.Columns.Add("IdSecuencial");
                _dtbcontacto.Columns.Add("Gestionado");
                ViewState["Contactos"] = _dtbcontacto;
                Session["CargarBaseCTC"] = "SI";
                ViewState["Estado"] = "I";
                ViewState["InciarTimer"] = "NO";
                ViewState["StatusPhone"] = "WRAP UP";
                Session["IN-CALL"] = "NO";
                Session["Salir"] = "NO";
                ViewState["CodigoSpeechCab"] = "0";
                ViewState["TelefonosMarcados"] = "NO";
                ViewState["TiempoGestion"] = "00:00:00";
                ViewState["TiempoLLamada"] = "00:00:00";
                ViewState["PerfilActitudinal"] = "false";
                ViewState["EstilosNegociacion"] = "false";
                ViewState["Metaprogramas"] = "false";
                ViewState["Modalidades"] = "false";
                ViewState["EstadosDelYo"] = "false";
                ViewState["Impulsores"] = "false";
                ViewState["Pago"] = "N";
                ViewState["Llamar"] = "N";
                ViewState["Citacion"] = "N";
                ViewState["UltimoNumeroAgregado"] = "";
                ViewState["CodigoLlamar"] = "0";
                ViewState["TipoTelefono"] = "0";
                ViewState["Propietario"] = "0";
                ViewState["VerGestiones"] = "1";
                ViewState["CodigoCLDEaux"] = "0";
                ViewState["DialerNumberaux"] = "";
                ViewState["PrefijoMarcacion"] = "";
                ViewState["DialerNumber"] = "";
                ViewState["PersCodigo"] = "0";
                ViewState["CodigoCLDE"] = "0";
                ViewState["TimerCall"] = "SI";
                ViewState["Cambiar"] = "SI";
                ViewState["DocumentoRef"] = "";
                Session["InicioLlamada"] = DateTime.Now.AddSeconds(_sumarsegundos).ToString("HH:mm:ss");
                FunCargarPerfilUsuario(int.Parse(Session["usuPerfil"].ToString()));
                ViewState["MarcarTest"] = ConfigurationManager.AppSettings["MarcarTest"];
                ViewState["Progresivo"] = ConfigurationManager.AppSettings["Progresivo"];
                ViewState["TiempoMarcado"] = ConfigurationManager.AppSettings["TiempoMarcado"];
                ViewState["MinutosLlamar"] = ConfigurationManager.AppSettings["MinutosLlamar"];
                TxtFechaLLamar.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaPago.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaCitacion.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Session["IdListaCabecera"] = Request["IdListaCabecera"];
                if (ViewState["MarcarTest"].ToString() == "SI") Session["IPLocalAdress"] = "192.168.100.111";

                _day = (int)DateTime.Now.DayOfWeek;

                _dtsx = new ConsultaDatosDAO().FunConsultaDatos(220, 0, 0, 0, "", "ACCESO ARBOL", "",
                    Session["Conectar"].ToString());

                FunCargarMantenimiento(int.Parse(Session["IdListaCabecera"].ToString()));

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

                ViewState["View1"] = "";
                ViewState["View2"] = "";
                ViewState["View3"] = "";

                _mostrarpopup = false;

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

                //if (Session["CodigoCPCE"].ToString() == "3") ImgCitacion.Visible = true;

                Lbltitulo.Text = "Gestion Lista de Trabajo " + ViewState["Catalogo"].ToString();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(36, int.Parse(Session["IdListaCabecera"].ToString()),
                        int.Parse(Session["usuCodigo"].ToString()), int.Parse(ViewState["Operaciones"].ToString()), "",
                        "", "", Session["Conectar"].ToString());
                GrdvEstadisticas.DataSource = _dts;
                GrdvEstadisticas.DataBind();
                _dts = new ListaTrabajoDAO().FunGetArbolRespuesta(int.Parse(Session["CodigoCPCE"].ToString()));
                ViewState["ArbolContactoEfectivo"] = _dts.Tables[0];

                FunCargarCombos(0);
                FunCargarCombos(1);
                FunCargarCombos(5);
                FunCargarCombos(9);
                pnlEstadisticas.Height = 120;
                //pnlTelefonos.Height = 150;
                pnlDatosDeudor.Height = 150;
                PnlDatosGarante.Height = 150;
                pnlDatosGetion.Height = 150;
                PnlPresuCompromiso.Height = 120;
                PnlBrenchPagos.Height = 120;
                PnlResultadoGestiones.Height = 180;

                if (Session["PermisoEspecial"].ToString() == "SI") DivPresupuesto.Visible = true;

                FunConsultarAgendamiento(int.Parse(Session["IdListaCabecera"].ToString()));

                if (int.Parse(ViewState["CodigoLlamar"].ToString()) > 0)
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(32, int.Parse(ViewState["PersCodigoaux"].ToString()), 0, 0,
                            "", "", "", Session["Conectar"].ToString().ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _mensaje = String.Format(@"Estimado Gestor, por favor realice la gestión al cliente:  Sr./Sra: {0} Agendado el: {1}", _dts.Tables[0].Rows[0]["Cliente"].ToString(),
                          ViewState["FechaLLamar"] != null ? ViewState["FechaLLamar"].ToString() + " " + ViewState["HoraLlamar"].ToString() : "");
                        //lblerror.Text = mensaje;
                        LnkSeguimiento.Text = _mensaje;
                    }
                }

                PnlDatosRegGestion_CollapsiblePanelExtender.Collapsed = false;
                PnlDatosDeudorOperacion_CollapsiblePanelExtender.Collapsed = false;
                PnlDatosTelefonos_CollapsiblePanelExtender.Collapsed = false;
                PnlResultadoGestiones_CollapsiblePanelExtender.Collapsed = false;
                //PnlPresupuesto_CollapsiblePanelExtender.Collapsed = false;

                if (ViewState["TipoMarcado"].ToString() == "DI")
                {
                    BtnPasar.Visible = true;
                    DivRegistrarTele.Visible = true;
                    DivDeudor.Visible = true;
                    DivBotones.Visible = true;
                    DivGestion.Visible = true;
                    DivOpciones.Visible = true;

                    ViewState["InicioGestion"] = DateTime.Now.ToString("HH:mm:ss");

                    if (ViewState["GestorApoyo"].ToString() == "0")
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(31, int.Parse(Session["IdListaCabecera"].ToString()),
                            int.Parse(Session["usuCodigo"].ToString()), 0, "", "", "", Session["Conectar"].ToString());
                    }
                    else
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(144, int.Parse(Session["IdListaCabecera"].ToString()),
                            int.Parse(Session["usuCodigo"].ToString()), 0, "", "", "", Session["Conectar"].ToString());
                    }

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        ViewState["IdSecuencial"] = _dts.Tables[0].Rows[0]["IdSecuencial"].ToString();
                        ViewState["CodigoCLDE"] = _dts.Tables[0].Rows[0]["CodigoCLDE"].ToString();
                        ViewState["PersCodigo"] = _dts.Tables[0].Rows[0]["PersCodigo"].ToString();
                        FunConsultarGestiones(0);
                        //Datos del deudor
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(32, int.Parse(ViewState["PersCodigo"].ToString()), 0, 0,
                            "", "", "", Session["Conectar"].ToString().ToString());
                        ViewState["NumeroDocumento"] = _dts.Tables[0].Rows[0]["Cedula"].ToString();
                        ViewState["Cliente"] = _dts.Tables[0].Rows[0]["Cliente"].ToString();
                        GrdvDatosDeudor.DataSource = _dts;
                        GrdvDatosDeudor.DataBind();
                        //Datos Obligacion
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(33, int.Parse(ViewState["CodigoCedente"].ToString()),
                            int.Parse(Session["CodigoCPCE"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()),
                            ViewState["Catalogo"].ToString(), "", "", Session["Conectar"].ToString().ToString());
                        GrdvDatosObligacion.DataSource = _dts;
                        GrdvDatosObligacion.DataBind();
                        ViewState["DatosObligacion"] = _dts.Tables[0];
                        //Datos Garante
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(45, 0, 0, 0, "", ViewState["NumeroDocumento"].ToString(), "", 
                            Session["Conectar"].ToString().ToString());
                        GrdvDatosGarante.DataSource = _dts;
                        GrdvDatosGarante.DataBind();

                        if (_dts.Tables[0].Rows.Count > 0) PnlDatosGarante.Visible = true;
                        else PnlDatosGarante.Visible = false;

                        FunCargarTelefonoRegistrados(int.Parse(ViewState["CodigoCedente"].ToString()), 
                            int.Parse(ViewState["PersCodigo"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()));
                        FunConsultarTelefonosDeudor(ViewState["TipoMarcado"].ToString());

                        if (ViewState["TipoTelefono"].ToString() == "CN")
                            lblNumMarcado.Text = ViewState["DialerNumber"] == null ? "" : "Marcando..." +
                                ViewState["PrefijoMarcacion"].ToString() + ViewState["DialerNumber"].ToString() + " - " + 
                                ViewState["Cliente"].ToString();
                        else lblNumMarcado.Text = ViewState["DialerNumber"] == null ? "" : "Marcando..." +
                            ViewState["DialerNumber"].ToString() + " - " + ViewState["Cliente"].ToString();

                        ViewState["InciarTimer"] = "SI";
                        FunCargarValoresVariablesBlandas();
                        LnkGestiones.Enabled = true;
                        ChkAgregar.Visible = true;
                        List<TelefonoPredictivoDTO> Telefonos = (List<TelefonoPredictivoDTO>)Session["TelefonoPredictivo"];
                        Telefonos.RemoveAll(x => x.Telefono.Equals(ViewState["DialerNumber"].ToString()));
                        Session["TelefonoPredictivo"] = Telefonos;

                        if (new FuncionesDAO().FunDesencripta(ViewState["Progresivo"].ToString()) == "SiDialer")
                        {
                            _thrmarcar = new Thread(new ThreadStart(FunEjecutarMarcado));
                            _thrmarcar.Start();
                        }
                    }
                }
                else LnkGestiones.Enabled = false;
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunConsultarGestiones(int opcion)
        {
            try
            {
                if (opcion == 0) _dts = new ConsultaDatosDAO().FunConsultaDatos(40, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    0, 0, "", "", "", Session["Conectar"].ToString());

                if (opcion == 1) _dts = new ConsultaDatosDAO().FunConsultaDatos(41, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    0, 0, "", "", "", Session["Conectar"].ToString());

                GrdvGestiones.DataSource = _dts;
                GrdvGestiones.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunConsultarAgendamiento(int idListaCabecera)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(39, idListaCabecera, int.Parse(Session["usuCodigo"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), "", "", "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    if (int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString()) > 0)
                    {
                        ViewState["CodigoLlamar"] = _dts.Tables[0].Rows[0]["Codigo"].ToString();
                        ViewState["IdSecuencialaux"] = _dts.Tables[0].Rows[0]["IdSecuencial"].ToString();
                        ViewState["CodigoCLDEaux"] = _dts.Tables[0].Rows[0]["CodigoCLDE"].ToString();
                        ViewState["PersCodigoaux"] = _dts.Tables[0].Rows[0]["Perscodigo"].ToString();
                        ViewState["FechaLLamar"] = _dts.Tables[0].Rows[0]["FechaLlamar"].ToString();
                        ViewState["HoraLlamar"] = _dts.Tables[0].Rows[0]["HoraLlamar"].ToString();
                        ViewState["NumDocumentoaux"] = _dts.Tables[0].Rows[0]["NumeroDocumento"].ToString();
                        ViewState["Operacionaux"] = _dts.Tables[0].Rows[0]["Operacion"].ToString();
                    }
                    else ViewState["CodigoLlamar"] = "0";
                }
                else ViewState["CodigoLlamar"] = "0";

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
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
                _dts = new ListaTrabajoDAO().FunGetPerfilDeudor(int.Parse(ViewState["PersCodigo"].ToString()));

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

        private void FunCargarTelefonoRegistrados(int cedecodigo, int perscodigo, int cldecodigo)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(35, cedecodigo, perscodigo, cldecodigo, "", "", "", 
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

        protected void FunCargarMantenimiento(int idListaTrabajo)
        {
            try
            {
                _dts = new ListaTrabajoDAO().FunGetListaTrabajo(idListaTrabajo);
                ViewState["ListaTrabajo"] = _dts.Tables[0].Rows[0]["Lista"].ToString();
                ViewState["TipoMarcado"] = _dts.Tables[0].Rows[0]["TipoMarcado"].ToString();
                ViewState["CodigoCedente"] = _dts.Tables[0].Rows[0]["CedeCodigo"].ToString();
                ViewState["CodigoProducto"] = _dts.Tables[0].Rows[0]["PrceCodigo"].ToString();
                ViewState["Producto"] = _dts.Tables[0].Rows[0]["Producto"].ToString();
                Session["CodigoCPCE"] = _dts.Tables[0].Rows[0]["CpceCodigo"].ToString();
                ViewState["CodigoCEDE"] = _dts.Tables[0].Rows[0]["CedeCodigo"].ToString();
                ViewState["Catalogo"] = _dts.Tables[0].Rows[0]["Catalogo"].ToString();
                ViewState["Operaciones"] = _dts.Tables[0].Rows[0]["Operaciones"].ToString();
                ViewState["GestorApoyo"] = _dts.Tables[0].Rows[0]["GestorApoyo"].ToString();
                SoftCob_CEDENTE cedente = new CedenteDAO().FunGetCedentePorID(int.Parse(ViewState["CodigoCEDE"].ToString()));
                ViewState["NivelArbol"] = cedente.cede_auxi1;

                _dts = new ConsultaDatosDAO().FunConsultaDatos(213, int.Parse(Session["usuCodigo"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), 0, "", "", "", Session["Conectar"].ToString());
                GrdvBrenchGestor.DataSource = _dts;
                GrdvBrenchGestor.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(214, int.Parse(Session["usuCodigo"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), 0, "", "", "", Session["Conectar"].ToString());
                GrdvBrenchPago.DataSource = _dts;
                GrdvBrenchPago.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunConsultarTelefonosDeudor(string marcador)
        {
            try
            {
                _dts = new ListaTrabajoDAO().FunGetTelefonoPredicitivoPorId(int.Parse(ViewState["CodigoCedente"].ToString()), int.Parse(ViewState["PersCodigo"].ToString()));
                switch (marcador)
                {
                    case "DI":
                        List<TelefonoPredictivoDTO> Telefonos = new List<TelefonoPredictivoDTO>();
                        _contar = 0;
                        foreach (DataRow dr in _dts.Tables[0].Rows)
                        {
                            TelefonoPredictivoDTO strDialerFones = new TelefonoPredictivoDTO();
                            strDialerFones.Telefono = dr["Telefono"].ToString();
                            strDialerFones.Tipo = dr["Tipo"].ToString();
                            strDialerFones.Score = int.Parse(dr["Score"].ToString());
                            strDialerFones.Propietario = dr["Propietario"].ToString();
                            strDialerFones.Prefijo = dr["Prefijo"].ToString();
                            strDialerFones.Marcado = false;

                            if (_contar == 0)
                            {
                                strDialerFones.Marcado = true;
                                ViewState["DialerNumber"] = strDialerFones.Telefono;
                                ViewState["TipoTelefono"] = strDialerFones.Tipo;
                                ViewState["Propietario"] = strDialerFones.Propietario;
                                ViewState["PrefijoMarcacion"] = strDialerFones.Prefijo;
                            }

                            Telefonos.Add(strDialerFones);
                            _contar++;
                        }
                        Session["TelefonoPredictivo"] = Telefonos;
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
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
                            updObservacion.Update();
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

        protected void FunCargarCombos(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        DddlEfecto.Items.Clear();
                        _efecto.Text = "--Seleccione Efecto--";
                        _efecto.Value = "0";
                        DddlEfecto.Items.Add(_efecto);

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

                        DdlPrefijo.Items.Clear();
                        _prefijo.Text = "--Seleccione Prefijo--";
                        _prefijo.Value = "0";
                        DdlPrefijo.Items.Add(_prefijo);

                        DdlAccionDel.DataSource = new ControllerDAO().FunGetParametroDetalle("ACCION TELEFONO", "--Seleccione Accion--",
                            "S");
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
                        DdlAccion.DataSource = new SpeechDAO().FunGetArbolNewAccion(int.Parse(Session["CodigoCPCE"].ToString()));
                        DdlAccion.DataTextField = "Descripcion";
                        DdlAccion.DataValueField = "Codigo";
                        DdlAccion.DataBind();
                        break;
                    case 2:
                        DddlEfecto.Items.Clear();
                        _efecto.Text = "--Seleccione Efecto--";
                        _efecto.Value = "0";
                        DddlEfecto.Items.Add(_efecto);

                        DdlRespuesta.Items.Clear();
                        _respuesta.Text = "--Seleccione Respuesta--";
                        _respuesta.Value = "0";
                        DdlRespuesta.Items.Add(_respuesta);

                        DdlContacto.Items.Clear();
                        _contacto.Text = "--Seleccione Contacto--";
                        _contacto.Value = "0";
                        DdlContacto.Items.Add(_contacto);

                        DddlEfecto.DataSource = new SpeechDAO().FunGetArbolNewEfecto(int.Parse(DdlAccion.SelectedValue));
                        DddlEfecto.DataTextField = "Descripcion";
                        DddlEfecto.DataValueField = "Codigo";
                        DddlEfecto.DataBind();
                        break;
                    case 3:
                        DdlRespuesta.Items.Clear();
                        _respuesta.Text = "--Seleccione Respuesta--";
                        _respuesta.Value = "0";
                        DdlRespuesta.Items.Add(_respuesta);

                        DdlContacto.Items.Clear();
                        _contacto.Text = "--Seleccione Contacto--";
                        _contacto.Value = "0";
                        DdlContacto.Items.Add(_contacto);

                        DdlRespuesta.DataSource = new SpeechDAO().FunGetArbolNewRespuesta(int.Parse(DddlEfecto.SelectedValue));
                        DdlRespuesta.DataTextField = "Descripcion";
                        DdlRespuesta.DataValueField = "Codigo";
                        DdlRespuesta.DataBind();
                        break;
                    case 4:
                        DdlContacto.Items.Clear();
                        _contacto.Text = "--Seleccione Contacto--";
                        _contacto.Value = "0";
                        DdlContacto.Items.Add(_contacto);

                        DdlContacto.DataSource = new SpeechDAO().FunGetArbolNewContacto(int.Parse(DdlRespuesta.SelectedValue));
                        DdlContacto.DataTextField = "Descripcion";
                        DdlContacto.DataValueField = "Codigo";
                        DdlContacto.DataBind();
                        break;
                    case 5:
                        DdlTipTelefono2.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO TELEFONO", "--Seleccione Tipo--",
                            "S");
                        DdlTipTelefono2.DataTextField = "Descripcion";
                        DdlTipTelefono2.DataValueField = "Codigo";
                        DdlTipTelefono2.DataBind();

                        DdlPropietario2.DataSource = new ControllerDAO().FunGetParametroDetalle("PROPIEDAD TELEFONO", 
                            "--Seleccione Propietario--", "S");
                        DdlPropietario2.DataTextField = "Descripcion";
                        DdlPropietario2.DataValueField = "Codigo";
                        DdlPropietario2.DataBind();

                        DdlTipoPago.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO PAGOS", "--Seleccione Pago--", "S");
                        DdlTipoPago.DataTextField = "Descripcion";
                        DdlTipoPago.DataValueField = "Codigo";
                        DdlTipoPago.DataBind();
                        break;
                    case 6:
                        DdlPrefijo.DataSource = new ControllerDAO().FunGetParametroDetalle("PREFIJOS", "--Seleccione Prefijo--", "S");
                        DdlPrefijo.DataTextField = "Descripcion";
                        DdlPrefijo.DataValueField = "Codigo";
                        DdlPrefijo.DataBind();
                        break;
                    case 7:
                        //DdlRespuestaDel.DataSource = new ConsultaDatosDAO().FunConsultaDatos(134, int.Parse(DdlAccionDel.SelectedValue), 0,
                        //    0, "", "", "", Session["Conectar"].ToString());
                        //DdlRespuestaDel.DataTextField = "Descripcion";
                        //DdlRespuestaDel.DataValueField = "Codigo";
                        //DdlRespuestaDel.DataBind();
                        break;
                    case 8:
                        TxtTelefono.Text = "";
                        DdlTipTelefono2.SelectedValue = "0";
                        DdlPropietario2.SelectedValue = "0";
                        DdlPrefijo.SelectedValue = "0";
                        TxtNombres.Text = "";
                        TxtApellidos.Text = "";
                        ImgEditelefono.Enabled = false;
                        ImgAddTelefono.Enabled = true;
                        DdlAccionDel.SelectedValue = "0";
                        DdlRespuestaDel.SelectedValue = "0";
                        TxtDocumentoRef.ReadOnly = false;
                        TxtDocumentoRef.Text = "";
                        TrFila1.Visible = false;
                        TrFila2.Visible = false;
                        ViewState["Cambiar"] = "SI";
                        ViewState["DocumentoRef"] = "";
                        break;
                    case 9:
                        DdlCitacion.DataSource = new ControllerDAO().FunGetParametroDetalle("CANCELAR CITACION", 
                            "--Seleccione Opcion--", "S");
                        DdlCitacion.DataTextField = "Descripcion";
                        DdlCitacion.DataValueField = "Codigo";
                        DdlCitacion.DataBind();
                        break;

                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
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

        protected void ImgInformacion_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _operacion = GrdvDatosObligacion.DataKeys[gvRow.RowIndex].Values["Operacion"].ToString();

            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; " +
                "var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                "window.open('WFrm_InfAdicional.aspx?CodigoCEDE=" + ViewState["CodigoCEDE"].ToString() +
                "&CodigoCPCE=" + Session["CodigoCPCE"].ToString() + "&CodigoPERS=" + ViewState["PersCodigo"].ToString() +
                "&Operacion=" + _operacion + "',null,'left=' + posicion_x + " +
                "', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, " +
                "toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void ChkBCast_CheckedChanged(object sender, EventArgs e)
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

        private void FunSpeechArbol(int araccodigo, int arefcodigo, int arrecodigo, int arcocodigo)
        {
            try
            {
                _txtspeech = FunConsultarSpeech(1, int.Parse(ViewState["CodigoCedente"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), araccodigo, arefcodigo, arrecodigo, arcocodigo,
                    int.Parse(ViewState["CodigoSpeechCab"].ToString()));
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunClearObject(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        divPagos.Visible = false;
                        divLLamar.Visible = false;
                        divCitacion.Visible = false;
                        _efectivo = false;
                        ViewState["Pago"] = "N";
                        ViewState["Llamar"] = "N";
                        ViewState["Citacion"] = "N";
                        Lblerror.Text = "";
                        TxtObservacion.Text = "";
                        updCabecera.Update();
                        break;
                    case 1:
                        lblNumMarcado.Text = "";
                        divPagos.Visible = false;
                        divLLamar.Visible = false;
                        TxtFechaPago.Text = DateTime.Now.ToString("MM/dd/yyyy"); ;
                        TxtValorAbono.Text = "";
                        TxtFechaLLamar.Text = DateTime.Now.ToString("MM/dd/yyyy"); ;
                        TxtHoraLLamar.Text = "";
                        RdbNo.Checked = false;
                        RdbSi.Checked = false;
                        TxtTelefono.Text = "";
                        DdlTipTelefono2.SelectedValue = "0";
                        DdlPropietario2.SelectedValue = "0";
                        DdlAccion.SelectedValue = "0";
                        TxtObservacion.Text = "";
                        DdlTipoPago.SelectedValue = "0";
                        TxtNumDocumento.Text = "";
                        DdlPrefijo.SelectedValue = "0";
                        ChkPerfilDeudor.Checked = false;
                        divPerfiles.Visible = false;
                        ImgAddTelefono.Enabled = true;
                        ImgEditelefono.Enabled = false;
                        ViewState["UltimoNumeroAgregado"] = "";
                        LnkGestiones.Text = "Todas las Gestiones";
                        ViewState["VerGestiones"] = "1";
                        TxtNombres.Text = "";
                        TxtApellidos.Text = "";
                        TxtNombres.Enabled = false;
                        TxtApellidos.Enabled = false;
                        updCabecera.Update();
                        break;
                }
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
                Thread.Sleep(300);
                Session["IN-CALL"] = "SI";
                Session["TrackNumber"] = FunDial(ViewState["DialerNumber"].ToString());
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDatoEqui_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _operacion = GrdvDatosGarante.DataKeys[gvRow.RowIndex].Values["Operacion"].ToString();

            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; " +
                "var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                "window.open('WFrm_DatosEquifax.aspx?CodigoPERS=" + ViewState["PersCodigo"].ToString() +
                "&Operacion=" + _operacion + "',null,'left=' + posicion_x + " +
                "', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, " +
                "toolbar=no, location=no, menubar=no,titlebar=0');", true);
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

        public string FunDial(string telefono)
        {
            try
            {
                if (ViewState["TipoTelefono"].ToString() == "CN") telefono = ViewState["PrefijoMarcacion"].ToString() + telefono;
                _strrespuesta = new ElastixDAO().ElastixDial(Session["IPLocalAdress"].ToString(), 9999, telefono);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _strrespuesta;
        }

        private void FunInsertarGestion()
        {
            try
            {
                if (new FuncionesDAO().FunDesencripta(Session["Phone"].ToString()) == "SiActivado")
                {
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                }

                Thread.Sleep(300);
                _nummarcado = true;

                if (ViewState["TipoMarcado"].ToString() == "DI")
                {
                    List<TelefonoPredictivoDTO> ConsultarTelefono = (List<TelefonoPredictivoDTO>)Session["TelefonoPredictivo"];
                    if (ConsultarTelefono.Count() == 0) _nummarcado = true;
                    else _nummarcado = false;
                }

                if (_nummarcado) ViewState["TelefonosMarcados"] = "SI";

                if (_efectivo) ViewState["TelefonosMarcados"] = "SI";

                string[] columnas = new[] { "Operacion", "DiasMora" };

                _dtbgestion = (DataTable)ViewState["DatosObligacion"];
                DataView view = new DataView(_dtbgestion);
                _dtbgestion = view.ToTable(true, columnas);
                _dtbtelefonos = (DataTable)ViewState["TelefonosRegistrados"];
                _mensaje = new GestionTelefonicaDAO().FunRegistrarGestionTelefonica(int.Parse(ViewState["CodigoCedente"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), int.Parse(Session["IdListaCabecera"].ToString()),
                    int.Parse(ViewState["IdSecuencial"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()),
                    int.Parse(ViewState["PersCodigo"].ToString()), ViewState["NumeroDocumento"].ToString(),
                    int.Parse(Session["usuCodigo"].ToString()), ViewState["DialerNumber"].ToString(), ViewState["TipoTelefono"].ToString(),
                    ViewState["Propietario"].ToString(), int.Parse(DdlAccion.SelectedValue), int.Parse(DddlEfecto.SelectedValue),
                    int.Parse(DdlRespuesta.SelectedValue), int.Parse(DdlContacto.SelectedValue), TxtObservacion.Text.Trim().ToUpper(),
                    _efectivo, DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("HH:mm:ss"), Session["TrackNumber"].ToString(),
                    ViewState["TiempoGestion"].ToString(), ViewState["TiempoLLamada"].ToString(), _totalgestion, _totallamada,
                    ViewState["Pago"].ToString(), _fechapago, _valorpago.Replace(",", "."), ViewState["Llamar"].ToString(), _fechallamar,
                    _horallamar, ViewState["UltimoNumeroAgregado"].ToString(), _lintperfilactitudinal, _lintestilonegociacion,
                    _lintmetaprograma, _lintmodalidad, _lintestadoyo, _lintimpulsor,
                    DdlTipoPago.SelectedValue, TxtNumDocumento.Text.Trim(), ViewState["PrefijoMarcacion"].ToString(), "", 0, 0,
                    0, 0, _dtbgestion, _dtbtelefonos, int.Parse(Session["usuCodigo"].ToString()),
                    Session["MachineName"].ToString(), "sp_NewGestionTelefonica", Session["Conectar"].ToString());

                Session["TrackNumber"] = null;

                if (_mensaje == "")
                {

                    //GRABAR CITACION CANCELADA
                    if (Chkcitacion.Checked)
                    {
                        _dts = new ConsultaDatosDAO().FunAgendaCitaciones(8, int.Parse(ViewState["CodigoCLDE"].ToString()),
                            int.Parse(ViewState["PersCodigo"].ToString()), DateTime.Now.ToString("MM/dd/yyyy"),
                            0, "0", int.Parse(Session["usuCodigo"].ToString()), DdlCitacion.SelectedItem.ToString(), "", "",
                            "", "", "", "", "", "", "", new byte[0], "", "", "", "", "", "0", 0, "", 0, DdlCitacion.SelectedValue,
                            "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                            Session["MachineName"].ToString(), Session["Conectar"].ToString());
                    }

                    if (ViewState["TipoMarcado"].ToString() == "DI")
                    {
                        //if (Chkcitacion.Checked)
                        //{
                        //    Response.Redirect("../BPM/WFrm_NuevaCitacion.aspx?CodigoPERS=" + ViewState["PersCodigo"].ToString() + "&CodigoCLDE=" + ViewState["CodigoCLDE"].ToString() + "&Retornar=2", true);
                        //}

                        if (ViewState["TelefonosMarcados"].ToString() == "SI")
                        {
                            if (!ChkMantenerReg.Checked)
                            {
                                new ConsultaDatosDAO().FunConsultaDatos(49, int.Parse(ViewState["CodigoCLDE"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());

                                _dts = new ConsultaDatosDAO().FunConsultaDatos(38, int.Parse(Session["IdListaCabecera"].ToString()), 
                                    int.Parse(ViewState["CodigoCLDE"].ToString()), 0, "", "", "", Session["Conectar"].ToString());

                                if (_dts.Tables[0].Rows[0][0].ToString() == "OK")
                                {
                                    Session["IN-CALL"] = "NO";

                                    if (Session["Salir"].ToString() == "SI") BtnSalir_Click(null, null);
                                    else
                                    {
                                        _redirect = string.Format("{0}?IdListaCabecera={1}", Request.Url.AbsolutePath,
                                            Session["IdListaCabecera"].ToString());
                                        Response.Redirect(_redirect);
                                    }
                                }
                                else Response.Redirect("WFrm_SeleccionListaTrabajo.aspx", true);
                            }
                            else
                            {
                                _dtbtelefonos = (DataTable)ViewState["TelefonosRegistrados"];
                                _cambiar = _dtbtelefonos.Select("Telefono='" + ViewState["DialerNumber"].ToString() + "'").FirstOrDefault();

                                if (_cambiar != null) _cambiar["Nuevo"] = "";

                                _dtbtelefonos.AcceptChanges();
                                ViewState["TelefonosRegistrados"] = _dtbtelefonos;
                                _dtbtelefonos.DefaultView.Sort = "Score desc, Telefono asc";
                                _dtbtelefonos = _dtbtelefonos.DefaultView.ToTable();
                                GrdvTelefonos.DataSource = _dtbtelefonos;
                                GrdvTelefonos.DataBind();
                                FunClearObject(1);
                                ChkMantenerReg.Checked = false;
                                FunCargarCombos(0);
                                FunConsultarGestiones(0);

                                if (Session["Salir"].ToString() == "SI") BtnSalir_Click(null, null);

                                List<TelefonoPredictivoDTO> Telefonos = (List<TelefonoPredictivoDTO>)Session["TelefonoPredictivo"];
                                var resultado = Telefonos.OrderByDescending(t => t.Score).OrderBy(t => t.Telefono).Where(t => t.Marcado == false).FirstOrDefault();

                                if (resultado != null)
                                {
                                    ViewState["DialerNumber"] = resultado.Telefono;
                                    ViewState["TipoTelefono"] = resultado.Tipo;
                                    ViewState["Propietario"] = resultado.Propietario;
                                    ViewState["PrefijoMarcacion"] = resultado.Prefijo;

                                    if (ViewState["TipoTelefono"].ToString() == "CN")
                                        lblNumMarcado.Text = "Marcando.." + ViewState["PrefijoMarcacion"].ToString() +
                                            resultado.Telefono + " - " + ViewState["Cliente"].ToString();
                                    else lblNumMarcado.Text = "Marcando.." + resultado.Telefono + " - " + ViewState["Cliente"].ToString();

                                    Thread.Sleep(800);

                                    if (new FuncionesDAO().FunDesencripta(ViewState["Progresivo"].ToString()) == "SiDialer")
                                    {
                                        _thrvolveramarcar = new Thread(new ThreadStart(FunEjecutarMarcado));
                                        _thrvolveramarcar.Start();
                                    }

                                    Telefonos.RemoveAll(x => x.Telefono.Equals(ViewState["DialerNumber"].ToString()));
                                    Session["TelefonoPredictivo"] = Telefonos;
                                }
                                ChkAgregar.Checked = false;
                            }
                        }
                        else
                        {
                            _dtbtelefonos = (DataTable)ViewState["TelefonosRegistrados"];
                            _cambiar = _dtbtelefonos.Select("Telefono='" + ViewState["DialerNumber"].ToString() + "'").FirstOrDefault();

                            if (_cambiar != null) _cambiar["Nuevo"] = "";

                            _dtbtelefonos.AcceptChanges();
                            ViewState["TelefonosRegistrados"] = _dtbtelefonos;
                            _dtbtelefonos.DefaultView.Sort = "Score desc, Telefono asc";
                            _dtbtelefonos = _dtbtelefonos.DefaultView.ToTable();
                            GrdvTelefonos.DataSource = _dtbtelefonos;
                            GrdvTelefonos.DataBind();
                            Session["IN-CALL"] = "NO";

                            if (Session["Salir"].ToString() == "SI") BtnSalir_Click(null, null);

                            ViewState["Estado"] = "I";
                            ViewState["InciarTimer"] = "SI";
                            ViewState["TimerCall"] = "SI";
                            FunClearObject(1);
                            FunCargarCombos(0);
                            FunConsultarGestiones(0);
                            List<TelefonoPredictivoDTO> Telefonos = (List<TelefonoPredictivoDTO>)Session["TelefonoPredictivo"];
                            var resultado = Telefonos.OrderByDescending(t => t.Score).OrderBy(t => t.Telefono).Where(t => t.Marcado == false).FirstOrDefault();

                            if (resultado != null)
                            {
                                ViewState["DialerNumber"] = resultado.Telefono;
                                ViewState["TipoTelefono"] = resultado.Tipo;
                                ViewState["Propietario"] = resultado.Propietario;
                                ViewState["PrefijoMarcacion"] = resultado.Prefijo;
                                if (ViewState["TipoTelefono"].ToString() == "CN")
                                    lblNumMarcado.Text = "Marcando.." + ViewState["PrefijoMarcacion"].ToString() +
                                        resultado.Telefono + " - " + ViewState["Cliente"].ToString();
                                else lblNumMarcado.Text = "Marcando.." + resultado.Telefono + " - " + ViewState["Cliente"].ToString();
                                Thread.Sleep(300);

                                if (new FuncionesDAO().FunDesencripta(ViewState["Progresivo"].ToString()) == "SiDialer")
                                {
                                    _thrvolveramarcar = new Thread(new ThreadStart(FunEjecutarMarcado));
                                    _thrvolveramarcar.Start();
                                }

                                Telefonos.RemoveAll(x => x.Telefono.Equals(ViewState["DialerNumber"].ToString()));
                                Session["TelefonoPredictivo"] = Telefonos;
                            }
                            ChkAgregar.Checked = false;
                        }
                    }
                    else //CLICK TO CALL
                    {
                        Session["IN-CALL"] = "NO";
                        ViewState["Estado"] = "F";
                        ViewState["InciarTimer"] = "NO";
                        ViewState["TimerCall"] = "NO";
                        FunClearObject(0);
                        FunClearObject(1);
                        FunCargarCombos(0);
                        ViewState["TelefonosMarcados"] = "NO";
                        if (!ChkMantenerReg.Checked)
                        {
                            new ConsultaDatosDAO().FunConsultaDatos(49, int.Parse(ViewState["CodigoCLDE"].ToString()), 0, 0, "", "", "",
                                Session["Conectar"].ToString());
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(38, int.Parse(Session["IdListaCabecera"].ToString()),
                               int.Parse(ViewState["CodigoCLDE"].ToString()), 0, "", "", "", Session["Conectar"].ToString());

                            if (_dts.Tables[0].Rows[0][0].ToString() == "OK")
                            {
                                _dts = new ConsultaDatosDAO().FunConsultaDatos(36, int.Parse(Session["IdListaCabecera"].ToString()),
                                    int.Parse(Session["usuCodigo"].ToString()), int.Parse(ViewState["Operaciones"].ToString()),
                                    "", "", "", Session["Conectar"].ToString());
                                GrdvEstadisticas.DataSource = _dts;
                                GrdvEstadisticas.DataBind();
                                _dtbcontacto = (DataTable)ViewState["Contactos"];
                                _result = _dtbcontacto.Select("IdSecuencial='" + ViewState["IdSecuencial"].ToString() + "'").FirstOrDefault();
                                _result["Gestionado"] = "SI";
                                _result.Delete();
                                _dtbcontacto.AcceptChanges();
                                ViewState["Contactos"] = _dtbcontacto;

                                GrdvDatosDeudor.DataSource = null;
                                GrdvDatosDeudor.DataBind();
                                GrdvDatosObligacion.DataSource = null;
                                GrdvDatosObligacion.DataBind();
                                GrdvGestiones.DataSource = null;
                                GrdvGestiones.DataBind();
                                GrdvTelefonos.DataSource = null;
                                GrdvTelefonos.DataBind();
                                GrdvDatosGarante.DataSource = null;
                                GrdvDatosGarante.DataBind();
                                ImgAddTelefono.Enabled = false;
                                LnkGestiones.Enabled = false;
                                ChkAgregar.Visible = false;
                                ChkAgregar.Checked = false;
                                ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "code", "Refrescar();", true);
                            }
                        }
                        else FunConsultarGestiones(0);
                    }
                    if (Session["Salir"].ToString() == "SI") BtnSalir_Click(null, null);
                }
                else Lblerror.Text = _mensaje;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private TreeNode FunLlenarArbolCTC(TreeNode node, int idListaTrabajo, string tipoMarcado)
        {
            try
            {
                if (ViewState["TipoMarcado"].ToString() == "DI") //DIALER
                {
                    TreeNode unnode = new TreeNode("PROGRESIVO", "DI");
                    node.ChildNodes.Add(unnode);
                }

                if (ViewState["TipoMarcado"].ToString() == "CT") //CLICK TO CALL
                {
                    TreeNode unnode = new TreeNode("CLICK TO CALL", "CT");
                    //LLAMAR A FUNCION PARA CARGAR CLIENTES
                    if (Session["CodigoCPCE"].ToString() == "3")
                        unnode = FunCargarGrupoCTC(unnode, int.Parse(Session["IdListaCabecera"].ToString()));
                    else
                        unnode = FunCargarItemCTC(unnode, int.Parse(Session["IdListaCabecera"].ToString()), "");

                    node.ChildNodes.Add(unnode);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return node;
        }

        private TreeNode FunCargarGrupoCTC(TreeNode node, int idListaTrabajo)
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(74, int.Parse(Session["CodigoCPCE"].ToString()), 0, 0,
                    "", "", "", Session["Conectar"].ToString());
                foreach (DataRow drfila in _dts.Tables[0].Rows)
                {
                    TreeNode unNode = new TreeNode(drfila["Descripcion"].ToString(), drfila["Codigo"].ToString());

                    if (drfila["Codigo"].ToString() == "TARJETA")
                    {
                        unNode = FunCargarItemCTC(unNode, int.Parse(Session["IdListaCabecera"].ToString()),
                            drfila["Codigo"].ToString());
                    }

                    if (drfila["Codigo"].ToString() == "CREDITO")
                    {
                        unNode = FunCargarGrupoCRE(unNode, drfila["Codigo"].ToString());
                    }

                    node.ChildNodes.Add(unNode);
                    unNode.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return node;
        }

        private TreeNode FunCargarGrupoCRE(TreeNode node, string tipocartera)
        {
            try
            {
                TreeNode unNode = new TreeNode("Individual", "I");
                unNode = FunCargarItemGrupo(unNode, int.Parse(Session["IdListaCabecera"].ToString()), tipocartera,
                    "I");
                node.ChildNodes.Add(unNode);

                TreeNode unNodeg = new TreeNode("Grupal", "G");
                unNodeg = FunCargarItemGrupo(unNodeg, int.Parse(Session["IdListaCabecera"].ToString()), tipocartera,
                    "G");
                node.ChildNodes.Add(unNodeg);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return node;
        }

        private TreeNode FunCargarItemCTC(TreeNode node, int idListaTrabajo, string tipocartera)
        {
            try
            {
                if (Session["CargarBaseCTC"].ToString() == "SI")
                {
                    if (ViewState["GestorApoyo"].ToString() == "0")
                    {
                        if (tipocartera == "")
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(42, idListaTrabajo, int.Parse(Session["usuCodigo"].ToString()), 0,
                                "", "", "", Session["Conectar"].ToString());
                        }
                        else
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(75, idListaTrabajo, int.Parse(Session["usuCodigo"].ToString()), 0,
                                        "", tipocartera, "", Session["Conectar"].ToString());
                        }
                    }
                    else
                    {
                        if (tipocartera == "")
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(145, idListaTrabajo, int.Parse(Session["usuCodigo"].ToString()), 0,
                                "", tipocartera, "", Session["Conectar"].ToString());
                        }
                        else
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(78, idListaTrabajo, int.Parse(Session["usuCodigo"].ToString()), 0,
                                        "", "", "", Session["Conectar"].ToString());
                        }
                    }

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _dtbcontacto = (DataTable)ViewState["Contactos"];
                        foreach (DataRow fila in _dts.Tables[0].Rows)
                        {
                            _filagre = _dtbcontacto.NewRow();
                            _filagre["Cliente"] = fila["Cliente"].ToString();
                            _filagre["CodigoCLDE"] = fila["CodigoCLDE"].ToString();
                            _filagre["IdSecuencial"] = fila["IdSecuencial"].ToString();
                            _filagre["Gestionado"] = "NO";
                            _dtbcontacto.Rows.Add(_filagre);
                            TreeNode unNode = new TreeNode(fila["Cliente"].ToString(), fila["CodigoCLDE"].ToString());
                            unNode.CollapseAll();
                            node.ChildNodes.Add(unNode);
                        }
                        ViewState["Contactos"] = _dtbcontacto;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return node;
        }

        private TreeNode FunCargarItemGrupo(TreeNode node, int idListaTrabajo, string tipocartera, string tipogrupo)
        {
            try
            {
                if (Session["CargarBaseCTC"].ToString() == "SI")
                {
                    if (tipogrupo == "I") _opcion = 76;
                    if (tipogrupo == "G") _opcion = 77;
                    if (ViewState["GestorApoyo"].ToString() == "0")

                        _dts = new ConsultaDatosDAO().FunConsultaDatos(_opcion, idListaTrabajo, int.Parse(Session["usuCodigo"].ToString()), 0, "", tipocartera, "C", Session["Conectar"].ToString());
                    else
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(145, idListaTrabajo, int.Parse(Session["usuCodigo"].ToString()), 0, "", "", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _dtbcontacto = (DataTable)ViewState["Contactos"];
                        foreach (DataRow fila in _dts.Tables[0].Rows)
                        {
                            _filagre = _dtbcontacto.NewRow();
                            _filagre["Cliente"] = fila["Cliente"].ToString();
                            _filagre["CodigoCLDE"] = fila["CodigoCLDE"].ToString();
                            _filagre["IdSecuencial"] = fila["IdSecuencial"].ToString();
                            _filagre["Gestionado"] = "NO";
                            _dtbcontacto.Rows.Add(_filagre);
                            TreeNode unNode = new TreeNode(fila["Cliente"].ToString(), fila["CodigoCLDE"].ToString());
                            unNode.CollapseAll();
                            node.ChildNodes.Add(unNode);
                        }
                        ViewState["Contactos"] = _dtbcontacto;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return node;
        }

        protected void FunCargarDatosCTC(int codigoLTCA, int codigoCLDE)
        {
            try
            {
                LnkGestiones.Enabled = true;
                FunCargarCombos(0);
                FunCargarCombos(1);
                FunClearObject(0);
                FunClearObject(1);
                _dts = new ConsultaDatosDAO().FunConsultaDatos(43, codigoLTCA, codigoCLDE, int.Parse(Session["usuCodigo"].ToString()), "", "", "", Session["Conectar"].ToString().ToString());
                ViewState["IdSecuencial"] = _dts.Tables[0].Rows[0]["IdSecuencial"].ToString();
                ViewState["CodigoCLDE"] = codigoCLDE;
                ViewState["PersCodigo"] = _dts.Tables[0].Rows[0]["PersCodigo"].ToString();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(32, int.Parse(ViewState["PersCodigo"].ToString()), 0, 0,
                    "", "", "", Session["Conectar"].ToString().ToString());
                ViewState["NumeroDocumento"] = _dts.Tables[0].Rows[0]["Cedula"].ToString();
                ViewState["Cliente"] = _dts.Tables[0].Rows[0]["Cliente"].ToString();
                GrdvDatosDeudor.DataSource = _dts;
                GrdvDatosDeudor.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(33, int.Parse(ViewState["CodigoCedente"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), codigoCLDE, ViewState["Catalogo"].ToString(), "", "",
                    Session["Conectar"].ToString().ToString());
                GrdvDatosObligacion.DataSource = _dts;
                GrdvDatosObligacion.DataBind();
                ViewState["DatosObligacion"] = _dts.Tables[0];

                _dts = new ConsultaDatosDAO().FunConsultaDatos(45, 0, 0, 0, "", ViewState["NumeroDocumento"].ToString(), "",
                    Session["Conectar"].ToString().ToString());
                GrdvDatosGarante.DataSource = _dts;
                GrdvDatosGarante.DataBind();

                if (_dts.Tables[0].Rows.Count > 0) PnlDatosGarante.Visible = true;
                else PnlDatosGarante.Visible = false;

                FunCargarTelefonoRegistrados(int.Parse(ViewState["CodigoCedente"].ToString()), int.Parse(ViewState["PersCodigo"].ToString()), codigoCLDE);
                FunCargarValoresVariablesBlandas();
                FunConsultarGestiones(0);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private TreeNode FunConsultarTelefonoCTC(TreeNode Node, int codigoLTCA, int codigoCLDE)
        {
            try
            {
                LnkGestiones.Enabled = true;
                if (Session["CargarBaseCTC"].ToString() == "SI")
                {
                    FunClearObject(0);
                    FunClearObject(1);
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(43, codigoLTCA, codigoCLDE, int.Parse(Session["usuCodigo"].ToString()), "", "",
                        "", Session["Conectar"].ToString().ToString());
                    ViewState["IdSecuencial"] = _dts.Tables[0].Rows[0]["IdSecuencial"].ToString();
                    ViewState["CodigoCLDE"] = codigoCLDE;
                    ViewState["PersCodigo"] = _dts.Tables[0].Rows[0]["PersCodigo"].ToString();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(32, int.Parse(ViewState["PersCodigo"].ToString()), 0, 0,
                        "", "", "", Session["Conectar"].ToString().ToString());
                    ViewState["NumeroDocumento"] = _dts.Tables[0].Rows[0]["Cedula"].ToString();
                    ViewState["Cliente"] = _dts.Tables[0].Rows[0]["Cliente"].ToString();
                    GrdvDatosDeudor.DataSource = _dts;
                    GrdvDatosDeudor.DataBind();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(33, int.Parse(ViewState["CodigoCedente"].ToString()),
                        int.Parse(Session["CodigoCPCE"].ToString()), codigoCLDE, ViewState["Catalogo"].ToString(), "", "",
                        Session["Conectar"].ToString().ToString());
                    GrdvDatosObligacion.DataSource = _dts;
                    GrdvDatosObligacion.DataBind();
                    ViewState["DatosObligacion"] = _dts.Tables[0];

                    FunCargarTelefonoRegistrados(int.Parse(ViewState["CodigoCedente"].ToString()), int.Parse(ViewState["PersCodigo"].ToString()), codigoCLDE);
                    FunCargarValoresVariablesBlandas();
                    FunConsultarGestiones(0);

                    _dtscontactos = (DataSet)ViewState["Contactos"];
                    _dtscontactos.Tables["Telefonos"].Rows.Cast<DataRow>().Where(x => int.Parse(x.ItemArray[0].ToString()) == codigoCLDE).ToList().ForEach(x => x.Delete());
                    _dtscontactos.AcceptChanges();
                    ViewState["Contactos"] = _dtscontactos;

                    _dts = new ListaTrabajoDAO().FunGetTelefonoPredicitivoPorId(int.Parse(ViewState["CodigoCedente"].ToString()), int.Parse(ViewState["PersCodigo"].ToString()));

                    if (_dts.Tables[0].Rows.Count > 0 && _dts != null)
                    {
                        if (int.Parse(ViewState["CodigoCLDEaux"].ToString()) == codigoCLDE)
                        {
                            _ctctelefonos = _dtscontactos.Tables["Telefonos"].Select("CodigoCLDE='" + codigoCLDE + "' and Telefono='" + _dts.Tables[0].Rows[0]["Telefono"].ToString() + "'");

                            if (_ctctelefonos.Length == 0)
                            {
                                _filagre = _dtscontactos.Tables["Telefonos"].NewRow();
                                _filagre["CodigoCLDE"] = codigoCLDE;
                                _filagre["Telefono"] = ViewState["DialerNumberaux"].ToString();
                                _filagre["Tipo"] = _dts.Tables[0].Rows[0]["Tipo"].ToString();
                                _filagre["Propietario"] = _dts.Tables[0].Rows[0]["Propietario"].ToString();
                                _filagre["Marcado"] = "True";
                            }
                        }
                        else
                        {
                            foreach (DataRow fila in _dts.Tables[0].Rows)
                            {
                                _ctctelefonos = _dtscontactos.Tables["Telefonos"].Select("CodigoCLDE='" + codigoCLDE + "' and Telefono='" + fila[0].ToString() + "'");

                                if (_ctctelefonos.Length == 0)
                                {
                                    _filagre = _dtscontactos.Tables["Telefonos"].NewRow();
                                    _filagre["CodigoCLDE"] = codigoCLDE;
                                    _filagre["Telefono"] = fila[0].ToString();
                                    _filagre["Tipo"] = fila[1].ToString();
                                    _filagre["Propietario"] = fila[2].ToString();

                                    if (_contador == 0)
                                    {
                                        _filagre["Marcado"] = "True";
                                        //unNode = new TreeNode(fila[0].ToString(), fila[0].ToString() + "|0");
                                        //unNode.ImageUrl = @"~/Botones/call_small.png";
                                        _contador++;
                                    }
                                    else
                                    {
                                        _filagre["Marcado"] = "False";
                                        //unNode = new TreeNode(fila[0].ToString(), fila[0].ToString() + "|1");
                                        //unNode.ImageUrl = @"~/Botones/call_small.png";
                                    }
                                    _dtscontactos.Tables["Telefonos"].Rows.Add(_filagre);
                                    //Node.ChildNodes.Add(unNode);
                                }
                            }
                        }
                        ViewState["Contactos"] = _dtscontactos;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return Node;
        }

        private void FunLlenarListaTrabajo(TreeNode treeNode)
        {
            try
            {
                TreeNode node = new TreeNode(ViewState["ListaTrabajo"].ToString(), Session["IdListaCabecera"].ToString());
                node = FunLlenarArbolCTC(node, int.Parse(Session["IdListaCabecera"].ToString()), ViewState["TipoMarcado"].ToString());
                treeNode.ChildNodes.Add(node);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunArmarArbolDTS()
        {
            try
            {
                TrvListaTrabajo.Nodes.Clear();
                TreeNode treeNode = new TreeNode("Lista de Trabajo", "Lista de Trabajo");
                TreeNode node = new TreeNode(ViewState["ListaTrabajo"].ToString(), Session["IdListaCabecera"].ToString());
                node = FunLlenarArbolCTC(node, int.Parse(Session["IdListaCabecera"].ToString()), ViewState["TipoMarcado"].ToString());
                treeNode.ChildNodes.Add(node);
                TrvListaTrabajo.Nodes.Add(treeNode);
                TrvListaTrabajo.ExpandAll();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private bool FunValidarTelefonos()
        {
            try
            {
                if (DdlTipTelefono2.SelectedValue == "CL")
                {
                    if (TxtTelefono.Text.Trim().Length != 10) _validar = false;

                    if (TxtTelefono.Text.Trim().Length == 10)
                    {
                        if (TxtTelefono.Text.Trim().Substring(0, 2) != "09") _validar = false;
                    }
                }

                if (DdlTipTelefono2.SelectedValue == "CN")
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

        private void FunMarcarCTC(int codigoLTCA, int codigoCLDE)
        {
            try
            {
                _dtbtelefonos = (DataTable)ViewState["TelefonosRegistrados"];

                if (_dtbtelefonos.Rows.Count > 0)
                {
                    DataRow[] buscar = _dtbtelefonos.Select("Telefono='" + ViewState["DialerNumber"].ToString() + "'");
                    ViewState["TipoTelefono"] = buscar[0]["CodTipo"].ToString();
                    ViewState["Propietario"] = buscar[0]["CodPro"].ToString();
                    ViewState["PrefijoMarcacion"] = buscar[0]["Prefijo"].ToString();
                }
                else
                {
                    ViewState["TipoTelefono"] = "CL";
                    ViewState["Propietario"] = "DE";
                }

                if (ViewState["TipoTelefono"].ToString() == "CN")
                {
                    if (ViewState["PrefijoMarcacion"].ToString() == "") ViewState["PrefijoMarcacion"] = "02";

                    lblNumMarcado.Text = ViewState["DialerNumber"] == null ? "" : "Marcando..." + ViewState["PrefijoMarcacion"].ToString() +
                        ViewState["DialerNumber"].ToString() + " - " + ViewState["Cliente"].ToString();
                }
                else lblNumMarcado.Text = ViewState["DialerNumber"] == null ? "" : "Marcando..." +
                    ViewState["DialerNumber"].ToString() + " - " + ViewState["Cliente"].ToString();

                ViewState["TimerCall"] = "SI";
                FunCambiarTelefonoMarcado();
                _thrmarcar = new Thread(new ThreadStart(FunEjecutarMarcado));
                _thrmarcar.Start();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCambiarTelefonoMarcado()
        {
            try
            {
                _dtscontactos = (DataSet)ViewState["Contactos"];
                _telefonoctc = ViewState["DialerNumber"].ToString();
                TreeNode busca = TrvListaTrabajo.FindNode("Lista de Trabajo/" + Session["IdListaCabecera"].ToString() + "/CT/" + ViewState["CodigoCLDE"].ToString());
                foreach (TreeNode nodo in busca.ChildNodes)
                {
                    string[] telf = nodo.Value.Split(new char[] { '|' });
                    if (telf.Count() == 2)
                    {
                        if (telf[0].ToString() == _telefonoctc)
                        {
                            nodo.ImageUrl = @"~/Botones/call_small_disabled.png";
                            nodo.Value = _telefonoctc + "|1";
                            foreach (DataRow droFila in _dtscontactos.Tables["Telefonos"].Rows)
                            {
                                if (_asignarsig && _proxtelefono == "")
                                {
                                    _proxtelefono = droFila["Telefono"].ToString();
                                    droFila["Marcado"] = "True";
                                    _asignarsig = false;
                                }

                                if (droFila["CodigoCLDE"].ToString() == ViewState["CodigoCLDE"].ToString() && droFila["Telefono"].ToString() == _telefonoctc)
                                {
                                    droFila["Marcado"] = "False";
                                    _asignarsig = true;
                                }
                            }
                        }

                        if (telf[0].ToString() == _proxtelefono)
                        {
                            nodo.ImageUrl = @"~/Botones/call_small.png";
                            nodo.Value = _proxtelefono + "|0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private string FunGetSegmento(int diasmora)
        {
            try
            {
                _dtssegmento = new CedenteDAO().FunGetSegmentoCabecera(int.Parse(ViewState["CodigoCedente"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), 0);

                if (_dtssegmento.Tables[0].Rows.Count == 0) _segmento = "SIN DEFINIR";
                else
                {
                    _segmento = "SIN DEFINIR";
                    foreach (DataRow dr in _dtssegmento.Tables[0].Rows)
                    {
                        _segmento = dr["Segmento"].ToString();
                        if (new FuncionesDAO().FunBetweenUno(int.Parse(dr["ValorI"].ToString()), int.Parse(dr["ValorF"].ToString()), diasmora) == 1)
                            break;
                        else _segmento = "SIN DEFINIR";
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _segmento;

        }
        #endregion

        #region Botones y Eventos
        protected void ChkAgregar_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkAgregar.Checked)
            {
                PnlAgregarTelefono.Visible = true;
                ImgAddTelefono.Enabled = true;
            }
            else
            {
                PnlAgregarTelefono.Visible = false;
                ImgAddTelefono.Enabled = false;
            }
        }

        protected void TrvListaTrabajo_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.ChildNodes.Count == 0)
            {
                switch (e.Node.Depth)
                {
                    case 0:
                        FunLlenarListaTrabajo(e.Node);
                        break;
                }
            }
        }

        protected void DdlAccionDel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FunCargarCombos(7);
        }

        protected void DdlAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtObservacion.Text = "";
                updObservacion.Update();
                FunClearObject(0);
                FunCargarCombos(2);
                _dts = new ListaTrabajoDAO().FunGetSpeech(0, int.Parse(ViewState["CodigoCedente"].ToString()),
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
                updObservacion.Update();
                FunClearObject(0);
                FunCargarCombos(3);
                _dts = new ListaTrabajoDAO().FunGetSpeech(0, int.Parse(ViewState["CodigoCedente"].ToString()),
                    int.Parse(Session["CodigoCPCE"].ToString()), 0, 0, 0, 0, 0);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoSpeechCab"] = _dts.Tables[0].Rows[0]["CodigoSpeech"].ToString();
                    FunConsultarSpeech(1, 0, 0, int.Parse(DdlAccion.SelectedValue), int.Parse(DddlEfecto.SelectedValue),
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
                updObservacion.Update();
                FunClearObject(0);

                _dts = new ListaTrabajoDAO().FunGetSpeech(0, int.Parse(ViewState["CodigoCedente"].ToString()),
                        int.Parse(Session["CodigoCPCE"].ToString()), 0, 0, 0, 0, 0);

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoSpeechCab"] = _dts.Tables[0].Rows[0]["CodigoSpeech"].ToString();
                    FunConsultarSpeech(1, 0, 0, int.Parse(DdlAccion.SelectedValue), int.Parse(DddlEfecto.SelectedValue),
                        int.Parse(DdlRespuesta.SelectedValue), 0, int.Parse(ViewState["CodigoSpeechCab"].ToString()));
                }

                if (DdlRespuesta.SelectedValue != "0")
                {
                    FunCargarCombos(4);
                    _pago = new ListaTrabajoDAO().FunGetValorRespuesta(int.Parse(DdlRespuesta.SelectedValue), 0);
                    _llamar = new ListaTrabajoDAO().FunGetValorRespuesta(int.Parse(DdlRespuesta.SelectedValue), 1);
                    _citacion = new ListaTrabajoDAO().FunGetValorRespuesta(int.Parse(DdlRespuesta.SelectedValue), 2);

                    if (_pago)
                    {
                        divPagos.Visible = true;
                        ViewState["Pago"] = "S";
                    }

                    if (_llamar)
                    {
                        divLLamar.Visible = true;
                        ViewState["Llamar"] = "S";
                        TxtHoraLLamar.Text = DateTime.Now.ToString("HH:mm");
                    }

                    if (_citacion)
                    {
                        divCitacion.Visible = true;
                        ViewState["Citacion"] = "S";
                        TxtHoraCitacion.Text = DateTime.Now.ToString("HH:mm");
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlContacto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtObservacion.Text = "";
                updObservacion.Update();
                _dts = new ListaTrabajoDAO().FunGetSpeech(0, int.Parse(ViewState["CodigoCedente"].ToString()),
                        int.Parse(Session["CodigoCPCE"].ToString()), 0, 0, 0, 0, 0);
                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoSpeechCab"] = _dts.Tables[0].Rows[0]["CodigoSpeech"].ToString();
                    FunConsultarSpeech(1, 0, 0, int.Parse(DdlAccion.SelectedValue), int.Parse(DddlEfecto.SelectedValue),
                        int.Parse(DdlRespuesta.SelectedValue), int.Parse(DdlContacto.SelectedValue), int.Parse(ViewState["CodigoSpeechCab"].ToString()));
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkGestionar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkgestion = (CheckBox)(gvRow.Cells[6].FindControl("chkGestionar"));
                _dtbgestion = (DataTable)ViewState["DatosObligacion"];
                _operacion = GrdvDatosObligacion.DataKeys[gvRow.RowIndex].Values["Operacion"].ToString();
                DataRow[] result = _dtbgestion.Select("Operacion='" + _operacion + "'");
                result[0]["Seleccion"] = _chkgestion.Checked ? "SI" : "NO";
                _dtbgestion.AcceptChanges();
                ViewState["DatosObligacion"] = _dtbgestion;
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

        protected void ImgAddTelefono_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlTipTelefono2.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione tipo teléfono..!", this);
                    return;
                }

                if (DdlPropietario2.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione propietario teléfono..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese teléfono..!", this);
                    return;
                }

                if (DdlTipTelefono2.SelectedValue == "CN")
                {
                    if (DdlPrefijo.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Prefijo de Marcación..!", this);
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(135, int.Parse(ViewState["PersCodigo"].ToString()), 0, 0, "",
                    DdlPrefijo.SelectedValue == "0" ? TxtTelefono.Text.Trim() : DdlPrefijo.SelectedValue + TxtTelefono.Text.Trim(), "", Session["Conectar"].ToString());

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
                    //BUSCAR SI YA EXISTE EL TELEFONO INGRESADO Y ESTA INACTIVADO
                    _mensaje = new GestionTelefonicaDAO().FunConsultarFonoEstado(int.Parse(ViewState["CodigoCedente"].ToString()), 
                        int.Parse(ViewState["PersCodigo"].ToString()), TxtTelefono.Text.Trim());

                    if (_mensaje != "")
                    {
                        new FuncionesDAO().FunShowJSMessage("Telefono ya Existe..!..!", this);
                        return;
                    }

                    ViewState["UltimoNumeroAgregado"] = TxtTelefono.Text.Trim();
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

                        _dts = new ConsultaDatosDAO().FunConsultaDatos(241, int.Parse(ViewState["PersCodigo"].ToString()), 0, 0, "",
                            TxtDocumentoRef.Text.Trim(), "", Session["Conectar"].ToString());

                        _codigo = int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString());

                        SoftCob_DEUDOR_REFERENCIAS addTelefonoref = new SoftCob_DEUDOR_REFERENCIAS();
                        {
                            addTelefonoref.DERE_CODIGO = _codigo;
                            addTelefonoref.pers_codigo = int.Parse(ViewState["CodigoCLDE"].ToString());
                            addTelefonoref.dere_numdocumento = TxtDocumentoRef.Text;
                            addTelefonoref.dere_tiporeferencia = DdlPropietario2.SelectedValue;
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
                        addTelefono.tece_cedecodigo = int.Parse(ViewState["CodigoCedente"].ToString());
                        addTelefono.tece_perscodigo = int.Parse(ViewState["PersCodigo"].ToString());
                        addTelefono.tece_referencodigo = _codigo;
                        addTelefono.tece_numero = TxtTelefono.Text.Trim();
                        addTelefono.tece_tipo = DdlTipTelefono2.SelectedValue;
                        addTelefono.tece_propietario = DdlPropietario2.SelectedValue;
                        addTelefono.tece_score = 0;
                        addTelefono.tece_estado = true;
                        addTelefono.tece_auxv1 = DdlTipTelefono2.SelectedValue == "CL" ? "" : DdlPrefijo.SelectedValue;
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

                    if (ViewState["TipoMarcado"].ToString() == "DI")
                    {
                        List<TelefonoPredictivoDTO> AgregarTelefono = (List<TelefonoPredictivoDTO>)Session["TelefonoPredictivo"];
                        TelefonoPredictivoDTO strDialerFones = new TelefonoPredictivoDTO();
                        strDialerFones.Telefono = TxtTelefono.Text.Trim();
                        strDialerFones.Tipo = DdlTipTelefono2.SelectedValue;
                        strDialerFones.Score = 3;
                        strDialerFones.Propietario = DdlPropietario2.SelectedValue;
                        strDialerFones.Prefijo = DdlTipTelefono2.SelectedValue == "CL" ? "" : DdlPrefijo.SelectedValue;
                        strDialerFones.Marcado = false;
                        AgregarTelefono.Add(strDialerFones);
                        Session["TelefonoPredictivo"] = AgregarTelefono;
                    }

                    FunCargarCombos(8);

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(35, int.Parse(ViewState["CodigoCEDE"].ToString()),
                           int.Parse(ViewState["PersCodigo"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "",
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

        protected void ImgDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                //if (DdlAccionDel.SelectedValue == "0")
                //{
                //    new FuncionesDAO().FunShowJSMessage("Seleccione Acción Para Eliminar..!", this);
                //    return;
                //}

                //if (DdlRespuestaDel.SelectedValue == "0")
                //{
                //    new FuncionesDAO().FunShowJSMessage("Seleccione Respuesta Para Eliminar..!", this);
                //    return;
                //}

                _codigotece = int.Parse(GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                _telefonoctc = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Telefono"].ToString();
                _sufijo = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Prefijo"].ToString();
                _dtbtelefonos = (DataTable)ViewState["TelefonosRegistrados"];
                _dtbtelefonos.Rows.RemoveAt(gvRow.RowIndex);

                new ConsultaDatosDAO().FunConsultaDatos(88, _codigotece, int.Parse(ViewState["PersCodigo"].ToString()),
                    int.Parse(ViewState["CodigoCLDE"].ToString()), "TELEFONO" + " - " + "INCORRECTO", _sufijo +
                    _telefonoctc, Session["usuCodigo"].ToString(), ViewState["Conectar"].ToString());

                if (ViewState["TipoMarcado"].ToString() == "DI")
                {
                    List<TelefonoPredictivoDTO> Telefonos = (List<TelefonoPredictivoDTO>)Session["TelefonoPredictivo"];
                    Telefonos.RemoveAll(x => x.Telefono.Equals(_telefonoctc));
                    Session["TelefonoPredictivo"] = Telefonos;
                }

                FunCargarCombos(8);
                ChkAgregar.Checked = false;
                PnlAgregarTelefono.Visible = false;

                _dts = new ConsultaDatosDAO().FunConsultaDatos(35, int.Parse(ViewState["CodigoCedente"].ToString()),
                    int.Parse(ViewState["PersCodigo"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "", "",
                    "", ViewState["Conectar"].ToString());

                GrdvTelefonos.DataSource = _dts;
                GrdvTelefonos.DataBind();
                ViewState["TelefonosRegistrados"] = _dts.Tables[0];
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void RdbSi_CheckedChanged(object sender, EventArgs e)
        {
            RdbNo.Checked = false;
        }

        protected void RdbNo_CheckedChanged(object sender, EventArgs e)
        {
            RdbSi.Checked = false;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlAccion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Acción..!", this);
                    return;
                }

                if (DddlEfecto.SelectedValue == "0")
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

                if (ViewState["DatosObligacion"] == null)
                {
                    new FuncionesDAO().FunShowJSMessage("No tiene Datos de Obligación para el Registro..!", this);
                    return;
                }

                _dtbgestion = (DataTable)ViewState["DatosObligacion"];
                _contador = _dtbgestion.Select("Seleccion='SI'").Count();

                if (_contador == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione al menos una operación de Datos Gestión..!", this);
                    return;
                }

                if (ViewState["Pago"].ToString() == "S")
                {
                    if (TxtFechaPago.Text.Trim() == "")
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese Fecha de Pago..!", this);
                        return;
                    }

                    DateTime dtmFechaAbono = DateTime.ParseExact(String.Format("{0}", TxtFechaPago.Text.Trim()), "MM/dd/yyyy", 
                        CultureInfo.InvariantCulture);

                    _dtmfechaactual = DateTime.Now.ToString("MM/dd/yyyy");

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
                }

                if (ViewState["Llamar"].ToString() == "S")
                {
                    if (string.IsNullOrEmpty(TxtHoraLLamar.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese Hora de llamada/ sumando 10 minutos a la hora actual..!", this);
                        return;
                    }

                    if (RdbSi.Checked == false && RdbNo.Checked == false)
                    {
                        new FuncionesDAO().FunShowJSMessage("Elija al mismo número o agregue nuevo..!", this);
                        return;
                    }

                    DateTime LdtmAhora = DateTime.Now.AddMinutes(int.Parse(ViewState["MinutosLlamar"].ToString()));
                    _horallamar = DateTime.Parse(TxtHoraLLamar.Text).ToString("HH:mm");
                    DateTime dtmFechaLLamar = DateTime.ParseExact(String.Format("{0} {1}", TxtFechaLLamar.Text,
                        _horallamar), "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);

                    if (dtmFechaLLamar < DateTime.Now.AddMinutes(10))
                    {
                        new FuncionesDAO().FunShowJSMessage("La Fecha/Hora no puede ser menor a la actual..!", this);
                        return;
                    }

                    if (RdbNo.Checked)
                    {
                        if (ViewState["UltimoNumeroAgregado"].ToString() == "")
                        {
                            new FuncionesDAO().FunShowJSMessage("Ingrese el número de contacto..!", this);
                            return;
                        }
                    }

                    if (RdbSi.Checked) ViewState["UltimoNumeroAgregado"] = ViewState["DialerNumber"].ToString();
                }

                if (new FuncionesDAO().FunDesencripta(Session["Phone"].ToString()) == "SiActivado")
                {
                    if (Session["TrackNumber"] == null)
                    {
                        new FuncionesDAO().FunShowJSMessage("Datos de Marcado no Disponibles, por favor espere un momento porfavor..!", this);
                        return;
                    }
                }
                else Session["TrackNumber"] = "Sin_Phone";

                if (Chkcitacion.Checked)
                {
                    if (DdlCitacion.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Opcion de Cancelacion..!", this);
                        return;
                    }
                }

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

                _dtbarbolrespuesta = (DataTable)ViewState["ArbolContactoEfectivo"];
                _result = _dtbarbolrespuesta.Select("Codigo='" + DdlRespuesta.SelectedValue + "'").FirstOrDefault();
                _efectivo = bool.Parse(_result["Contacto"].ToString());

                _fechapago = TxtFechaPago.Text.Trim() == "" ? DateTime.Now.ToString("MM/dd/yyyy") : TxtFechaPago.Text;
                _valorpago = TxtValorAbono.Text.Trim() == "" ? "0.00" : TxtValorAbono.Text;
                _fechallamar = TxtFechaLLamar.Text.Trim() == "" ? DateTime.Now.ToString("MM/dd/yyyy") : TxtFechaLLamar.Text;
                _horallamar = TxtHoraLLamar.Text.Trim() == "" ? "00:00:00" : TxtHoraLLamar.Text.Trim();

                //VALIDAR SI EXISTE UN REGISTRO DE PAGO CON LA MISMA FECHA
                if (ViewState["Pago"].ToString() == "S")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(240, int.Parse(ViewState["PersCodigo"].ToString()),
                        int.Parse(DdlContacto.SelectedValue), int.Parse(Session["CodigoCPCE"].ToString()), "", TxtFechaPago.Text,
                        TxtValorAbono.Text, Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Cliente ya tien regitrdo un COMPROMISO en esa Fecha " +
                            "Verifique el registro o Consulte con el Administrador..!", this);
                        return;
                    }
                }

                FunInsertarGestion();
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
                divPerfiles.Visible = true;
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
            else divPerfiles.Visible = false;
        }

        protected void TmrTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["Estado"].ToString() == "I")
                {
                    ViewState["segundosG"] = null;
                    ViewState["segundosL"] = null;
                    ViewState["contador"] = null;
                    _seg = 0;
                    _min = 0;
                    _hor = 0;
                    ViewState["Estado"] = "F";
                    ViewState["StatusPhone"] = "WRAP UP";
                }

                if (ViewState["InciarTimer"].ToString() == "SI")
                {
                    if (ViewState["contador"] == null)
                    {
                        _contador = 0;
                        ViewState["contador"] = "1";
                    }
                    else
                    {
                        if (ViewState["TimerCall"].ToString() == "SI")
                        {
                            _contador = int.Parse(ViewState["contador"].ToString()) + 1;
                        }
                    }

                    if (_contador == int.Parse(ViewState["TiempoMarcado"].ToString()))
                    {
                        ViewState["StatusPhone"] = "Connected";
                        _contador = 0;
                    }

                    ViewState["contador"] = _contador;

                    if (ViewState["segundosG"] == null)
                    {
                        ViewState["Estado"] = "C";
                        _seg = 0;
                        _min = 0;
                        _hor = 0;
                        ViewState["segundosG"] = "1";
                        ViewState["minutosG"] = "0";
                        ViewState["horasG"] = "0";
                    }
                    else _seg = int.Parse(ViewState["segundosG"].ToString()) + 1;

                    if (_seg == 60)
                    {
                        _seg = 0;
                        ViewState["segundosG"] = "0";
                        _min = int.Parse(ViewState["minutosG"].ToString()) + 1;
                        ViewState["minutosG"] = _min;

                        if (_min == 60)
                        {
                            _min = 0;
                            ViewState["minutosG"] = "0";
                            _hor = int.Parse(ViewState["horasG"].ToString()) + 1;
                            ViewState["horasG"] = _hor;
                        }
                    }

                    ViewState["segundosG"] = _seg;
                    _min = int.Parse(ViewState["minutosG"].ToString());
                    _hor = int.Parse(ViewState["horasG"].ToString());
                    _strtiempogestion = _hor.ToString("D2") + ":" + _min.ToString("D2") + ":" + _seg.ToString("D2");
                    ViewState["TiempoGestion"] = _strtiempogestion;

                    if (ViewState["StatusPhone"].ToString() == "Connected")
                    {
                        if (ViewState["segundosL"] == null)
                        {
                            _segl = 0;
                            _minl = 0;
                            _horl = 0;
                            ViewState["segundosL"] = "1";
                            ViewState["minutosL"] = "0";
                            ViewState["horasL"] = "0";
                        }
                        else _segl = int.Parse(ViewState["segundosL"].ToString()) + 1;

                        if (_segl == 60)
                        {
                            _segl = 0;
                            ViewState["segundosL"] = "0";
                            _minl = int.Parse(ViewState["minutosL"].ToString()) + 1;
                            ViewState["minutosL"] = _minl;

                            if (_minl == 60)
                            {
                                _minl = 0;
                                ViewState["minutosL"] = "0";
                                _hor = int.Parse(ViewState["horasL"].ToString()) + 1;
                                ViewState["horasL"] = _hor;
                            }
                        }

                        ViewState["segundosL"] = _segl;
                        _minl = int.Parse(ViewState["minutosL"].ToString());
                        _horl = int.Parse(ViewState["horasL"].ToString());
                        _strtiempollamada = _horl.ToString("D2") + ":" + _minl.ToString("D2") + ":" + _segl.ToString("D2");
                        ViewState["TiempoLLamada"] = _strtiempollamada;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
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

                GrdvTelefonos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = int.Parse(GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                _dtbtelefonos = (DataTable)ViewState["TelefonosRegistrados"];
                _result = _dtbtelefonos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                TxtTelefono.Text = _result["Telefono"].ToString();
                DdlTipTelefono2.SelectedValue = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["CodTipo"].ToString();
                DdlPrefijo.Items.Clear();
                _prefijo.Text = "--Seleccione Prefijo--";
                _prefijo.Value = "0";
                DdlPrefijo.Items.Add(_prefijo);
                DdlPropietario2.SelectedValue = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["CodPro"].ToString();
                TxtNombres.Text = _result["Nombres"].ToString();
                TxtApellidos.Text = _result["Apellidos"].ToString();
                TxtDocumentoRef.Text = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["NumDocumento"].ToString();
                ViewState["NumDocumentoRef"] = TxtDocumentoRef.Text.Trim();
                ViewState["UltimoNumeroAgregado"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Telefono"].ToString();
                ViewState["Telefonoanterior"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Telefono"].ToString();
                ViewState["CodigoTelefono"] = _codigo;
                ViewState["PrefijoMarcacion"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Prefijo"].ToString();
                ViewState["DialerNumber"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Telefono"].ToString();
                ViewState["TipoTelefono"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["CodTipo"].ToString();
                ViewState["Propietario"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["CodPro"].ToString();
                ViewState["Score"] = GrdvTelefonos.DataKeys[gvRow.RowIndex].Values["Score"].ToString();

                if (string.IsNullOrEmpty(TxtDocumentoRef.Text.Trim())) TxtDocumentoRef.ReadOnly = false;
                else TxtDocumentoRef.ReadOnly = true;

                ImgAddTelefono.Enabled = false;
                ImgEditelefono.Enabled = true;
                ChkAgregar.Checked = true;
                PnlAgregarTelefono.Visible = true;

                if (ViewState["Propietario"].ToString() != "DE")
                {
                    TrFila1.Visible = true;
                    TrFila2.Visible = true;
                }

                if (ViewState["TipoTelefono"].ToString() == "CN")
                {
                    FunCargarCombos(6);

                    if (ViewState["PrefijoMarcacion"].ToString() == "") ViewState["PrefijoMarcacion"] = "02";

                    DdlPrefijo.SelectedValue = ViewState["PrefijoMarcacion"].ToString();

                    lblNumMarcado.Text = ViewState["DialerNumber"] == null ? "" : "Marcando..." + ViewState["PrefijoMarcacion"].ToString() +
                        ViewState["DialerNumber"].ToString() + " - " + ViewState["Cliente"].ToString();
                }
                else lblNumMarcado.Text = ViewState["DialerNumber"] == null ? "" : "Marcando..." +
                    ViewState["DialerNumber"].ToString() + " - " + ViewState["Cliente"].ToString();

                ViewState["Cambiar"] = "NO";

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
                if (DdlTipTelefono2.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione tipo teléfono..!", this);
                    return;
                }

                if (DdlPropietario2.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione propietario teléfono..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese teléfono..!", this);
                    return;
                }

                if (DdlTipTelefono2.SelectedValue == "CN")
                {
                    if (DdlPrefijo.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Prefijo de Marcación..!", this);
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(135, int.Parse(ViewState["PersCodigo"].ToString()), 0, 0, "",
                    DdlPrefijo.SelectedValue == "0" ? TxtTelefono.Text.Trim() : DdlPrefijo.SelectedValue + TxtTelefono.Text.Trim(),
                    "", Session["Conectar"].ToString());

                if (int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString()) >= 2)
                {
                    new FuncionesDAO().FunShowJSMessage("El Teléfono fue Eliminado más de una vez..!", this);
                    FunCargarCombos(8);
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
                            if (_result != null) _lexiste = true;
                        }
                    }

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("Ya existe teléfono..!", this);
                        return;
                    }

                    //if (ViewState["NumDocumentoRef"].ToString() != TxtDocumentoRef.Text.Trim())
                    //{
                    //    _tblbuscar = (DataTable)ViewState["TelefonosRegistrados"];
                    //    _result = _tblbuscar.Select("NumDocumento='" + TxtDocumentoRef.Text.Trim() + "'").FirstOrDefault();
                    //    if (_result != null) _lexiste = true;

                    //    if (_lexiste)
                    //    {
                    //        new FuncionesDAO().FunShowJSMessage("No. de Documento ya Existe..!", this);
                    //        return;
                    //    }
                    //}

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
                    _mensaje = new ConsultaDatosDAO().FunEditarTelefonos(0, int.Parse(ViewState["PersCodigo"].ToString()),
                        int.Parse(ViewState["CodigoTelefono"].ToString()), DdlTipTelefono2.SelectedValue, DdlPropietario2.SelectedValue,
                        TxtNombres.Text.Trim().ToUpper(), TxtApellidos.Text.Trim().ToUpper(), TxtTelefono.Text.Trim(),
                        DdlPrefijo.SelectedValue == "0" ? "" : DdlPrefijo.SelectedValue, TxtDocumentoRef.Text, "", "", "",
                        int.Parse(ViewState["CodigoCedente"].ToString()), 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());

                    PnlAgregarTelefono.Visible = false;
                    ChkAgregar.Checked = false;
                    FunCargarCombos(8);

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(35, int.Parse(ViewState["CodigoCEDE"].ToString()),
                           int.Parse(ViewState["PersCodigo"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "",
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

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["TelefonosMarcados"] = "SI";
                if (Session["IN-CALL"].ToString() == "SI")
                {
                    new FuncionesDAO().FunShowJSMessage("Se encuentra en Llamada, en cuanto termine la gestión podrá salir de la Lista de Trabajo..!", this);
                    Session["Salir"] = "SI";
                }
                else
                {
                    SoftCob_LOGUEO_TIEMPOS loguintime = new SoftCob_LOGUEO_TIEMPOS();
                    {
                        loguintime.USUA_CODIGO = int.Parse(Session["usuCodigo"].ToString());
                        loguintime.empr_codigo = int.Parse(Session["CodigoEMPR"].ToString());
                        loguintime.cpce_codigo = int.Parse(Session["CodigoCPCE"].ToString());
                        loguintime.ltca_codigo = int.Parse(Session["IdListaCabecera"].ToString());
                        loguintime.loti_tipologueo = "SG";
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
                    SoftCob_LISTATRABAJO_ACTIVAS listaActiva = new SoftCob_LISTATRABAJO_ACTIVAS();
                    {
                        listaActiva.lsac_listatrabajo = int.Parse(Session["IdListaCabecera"].ToString());
                        listaActiva.lsac_gestorasignado = int.Parse(Session["usuCodigo"].ToString());
                        listaActiva.lsac_estado = false;
                    }
                    new ListaTrabajoDAO().FunActualizarListaActiva(listaActiva);
                    ScriptManager.RegisterClientScriptBlock(updOpciones, updOpciones.GetType(), "Cerrar", "CloseFrame();", true);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void LnkGestiones_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["VerGestiones"].ToString() == "1")
                {
                    ViewState["VerGestiones"] = "0";
                    LnkGestiones.Text = "Últimas Diez Gestiones";
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

        protected void TrvListaTrabajo_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                TreeView arbol = ((TreeView)sender);
                TreeNode Node = arbol.SelectedNode;

                DivRegistrarTele.Visible = false;
                DivDeudor.Visible = false;
                DivBotones.Visible = false;
                DivGestion.Visible = false;
                DivOpciones.Visible = false;

                switch (Node.Depth)
                {

                    case 1:
                        var variable = Node.Value;
                        Session["IdListaCabecera"] = int.Parse(Node.Value);
                        _redirect = string.Format("{0}?IdListaCabecera={1}", Request.Url.AbsolutePath, Session["IdListaCabecera"].ToString());
                        Response.Redirect(_redirect);
                        break;
                    case 2:
                        switch (Node.Value)
                        {
                            case "DI":
                                Session["CambiarMarcado"] = "DIALER";
                                _redirect = string.Format("{0}?IdListaCabecera={1}", Request.Url.AbsolutePath,
                                        Session["IdListaCabecera"].ToString());
                                Response.Redirect(_redirect);
                                break;
                            case "CT":
                                Session["CambiarMarcado"] = "CT";
                                _redirect = string.Format("{0}?IdListaCabecera={1}", Request.Url.AbsolutePath,
                                        Session["IdListaCabecera"].ToString());
                                Response.Redirect(_redirect);
                                break;
                        }
                        break;
                    case 3:
                        Session["CargarBaseCTC"] = "SI";
                        DivRegistrarTele.Visible = true;
                        DivDeudor.Visible = true;
                        DivBotones.Visible = true;
                        DivGestion.Visible = true;
                        DivOpciones.Visible = true;
                        ViewState["Estado"] = "I";
                        ViewState["InciarTimer"] = "SI";
                        ViewState["TimerCall"] = "NO";
                        ViewState["InicioGestion"] = DateTime.Now.ToString("HH:mm:ss");
                        ViewState["InicioLlamada"] = DateTime.Now.ToString("HH:mm:ss");
                        ImgAddTelefono.Enabled = true;
                        LnkGestiones.Enabled = true;
                        ChkAgregar.Visible = true;
                        _pathroot = Node.ValuePath.Split(new char[] { '/' });
                        if (new FuncionesDAO().IsNumber(_pathroot[3]))
                        {
                            FunCargarDatosCTC(int.Parse(_pathroot[1]), int.Parse(Node.Value));
                        }
                        break;
                    case 4:
                        Session["CargarBaseCTC"] = "SI";
                        ViewState["Estado"] = "I";
                        ViewState["InciarTimer"] = "SI";
                        ViewState["TimerCall"] = "NO";
                        ViewState["InicioGestion"] = DateTime.Now.ToString("HH:mm:ss");
                        ViewState["InicioLlamada"] = DateTime.Now.ToString("HH:mm:ss");
                        ImgAddTelefono.Enabled = true;
                        LnkGestiones.Enabled = true;
                        ChkAgregar.Visible = true;
                        _pathroot = Node.ValuePath.Split(new char[] { '/' });
                        if (new FuncionesDAO().IsNumber(_pathroot[4]))
                        {
                            FunCargarDatosCTC(int.Parse(_pathroot[1]), int.Parse(Node.Value));
                        }
                        break;
                    case 5:
                        Session["CargarBaseCTC"] = "SI";
                        ViewState["Estado"] = "I";
                        ViewState["InciarTimer"] = "SI";
                        ViewState["TimerCall"] = "NO";
                        ViewState["InicioGestion"] = DateTime.Now.ToString("HH:mm:ss");
                        ViewState["InicioLlamada"] = DateTime.Now.ToString("HH:mm:ss");
                        ImgAddTelefono.Enabled = true;
                        LnkGestiones.Enabled = true;
                        ChkAgregar.Visible = true;
                        _pathroot = Node.ValuePath.Split(new char[] { '/' });
                        FunCargarDatosCTC(int.Parse(_pathroot[1]), int.Parse(Node.Value));
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnRefrescar_Click(object sender, EventArgs e)
        {
            Session["CargarBaseCTC"] = "SI";
            ViewState["CodigoCLDEaux"] = "0";
            ViewState["DialerNumberaux"] = "";
            ViewState["TelefonosMarcados"] = "NO";
            DivRegistrarTele.Visible = false;
            DivDeudor.Visible = false;
            DivBotones.Visible = false;
            DivGestion.Visible = false;
            DivOpciones.Visible = false;
            FunArmarArbolDTS();
        }

        protected void GrdvRefeDeudor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    if (ViewState["PrefijoMarcacion"].ToString() + ViewState["DialerNumber"].ToString() == e.Row.Cells[3].Text.Trim()) e.Row.BackColor = System.Drawing.Color.Beige;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
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
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            foreach (GridViewRow _fr in GrdvTelefonos.Rows)
            {
                _fr.Cells[0].BackColor = System.Drawing.Color.White;
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
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

            foreach (GridViewRow _fr in GrdvDatosGarante.Rows)
            {
                _fr.Cells[0].BackColor = System.Drawing.Color.White;
            }

            GrdvDatosGarante.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
            _cedulagarante = GrdvDatosGarante.DataKeys[_gvrow.RowIndex].Values["CedulaGarante"].ToString();

            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_OtrosTelefonos.aspx?Cedula=" +
                _cedulagarante + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=500px, " +
                "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void DdlTipTelefono2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlPrefijo.Items.Clear();
            _prefijo.Text = "--Seleccione Prefijo--";
            _prefijo.Value = "0";
            DdlPrefijo.Items.Add(_prefijo);
            if (DdlTipTelefono2.SelectedValue == "CN") FunCargarCombos(6);
        }

        protected void ImgCall_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvTelefonos.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                _codigotece = int.Parse(GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                using (SoftCobEntities _db = new SoftCobEntities())
                {
                    SoftCob_TELEFONOS_CEDENTE _original = _db.SoftCob_TELEFONOS_CEDENTE.Where(t => t.TECE_CODIGO ==
                    _codigotece).FirstOrDefault();
                    _db.SoftCob_TELEFONOS_CEDENTE.Attach(_original);
                    _original.tece_auxv2 = "";
                    _db.SaveChanges();
                }

                GrdvTelefonos.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                ViewState["PrefijoMarcacion"] = GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["Prefijo"].ToString();
                ViewState["DialerNumber"] = GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["Telefono"].ToString();
                ViewState["TipoTelefono"] = GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["CodTipo"].ToString();
                ViewState["Propietario"] = GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["CodPro"].ToString();

                _imgmarcar = (ImageButton)(_gvrow.Cells[5].FindControl("imgCall"));

                _imgmarcar.ImageUrl = "~/Botones/call_small_disabled.png";

                if (ViewState["TipoTelefono"].ToString() == "CN")
                {
                    if (ViewState["PrefijoMarcacion"].ToString() == "") ViewState["PrefijoMarcacion"] = "02";
                    lblNumMarcado.Text = ViewState["DialerNumber"] == null ? "" : "Marcando..." + ViewState["PrefijoMarcacion"].ToString() +
                        ViewState["DialerNumber"].ToString() + " - " + ViewState["Cliente"].ToString();
                }
                else lblNumMarcado.Text = ViewState["DialerNumber"] == null ? "" : "Marcando..." +
                    ViewState["DialerNumber"].ToString() + " - " + ViewState["Cliente"].ToString();

                if (new FuncionesDAO().FunDesencripta(Session["Phone"].ToString()) == "SiActivado")
                {
                    _thrmarcar = new Thread(new ThreadStart(FunEjecutarMarcado));
                    _thrmarcar.Start();
                }
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
                if (!string.IsNullOrEmpty(ViewState["DocumentoRef"].ToString()))
                    TxtDocumentoRef.ReadOnly = true;
                TrFila1.Visible = true;
                TrFila2.Visible = true;
            }
        }

        protected void DdlTipoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtNumDocumento.Text = "";
        }

        protected void GrdvDatosObligacion_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void BtnPasar_Click(object sender, EventArgs e)
        {
            try
            {
                if (new FuncionesDAO().FunDesencripta(Session["Phone"].ToString()) == "SiActivado")
                {
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                }

                new ConsultaDatosDAO().FunConsultaDatos(49, int.Parse(Session["IdListaCabecera"].ToString()), 0, 0, "", "", "",
                    Session["Conectar"].ToString());
                _dts = new ConsultaDatosDAO().FunConsultaDatos(38, int.Parse(Session["IdListaCabecera"].ToString()),
                    int.Parse(ViewState["CodigoCLDE"].ToString()), 0, "", "", "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows[0][0].ToString() == "OK")
                {
                    Session["IN-CALL"] = "NO";

                    if (Session["Salir"].ToString() == "SI") BtnSalir_Click(null, null);
                    else
                    {
                        _redirect = string.Format("{0}?IdListaCabecera={1}", Request.Url.AbsolutePath,
                            Session["IdListaCabecera"].ToString());
                        Response.Redirect(_redirect);
                    }
                }
                else Response.Redirect("WFrm_SeleccionListaTrabajo.aspx", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgSpeechBV_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", 
                    "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                    "window.open('WFrm_SpeechBV.aspx?CodigoCEDE=" + ViewState["CodigoCEDE"].ToString() + "&CodigoCPCE=" + 
                    Session["CodigoCPCE"].ToString() + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=750px, height=400px," +
                    " status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
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
                    "&CodigoCPCE=" + Session["CodigoCPCE"].ToString() + "&CodigoARAC=" + DdlAccion.SelectedValue + "&CodigoAREF=" + DddlEfecto.SelectedValue +
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

                _dts = new ConsultaDatosDAO().FunConsultaDatos(35, int.Parse(ViewState["CodigoCedente"].ToString()), 
                    int.Parse(ViewState["PersCodigo"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "", "", "",
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
                if (ViewState["PersCodigo"].ToString() != "")
                {
                    ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('wFrm_NotasGestion.aspx?CodigoCEDE=" + ViewState["CodigoCedente"].ToString() +
                        "&CodigoCPCE=" + Session["CodigoCPCE"].ToString() + "&codigoPERS=" + ViewState["PersCodigo"].ToString() +
                        "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=750px, height=400px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void LnkSeguimiento_Click(object sender, EventArgs e)
        {
            try
            {
                Session["IN-CALL"] = "NO";

                new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);

                Response.Redirect("WFrm_RegLlamadaEntrante.aspx?CodigoCEDE=" + ViewState["CodigoCEDE"].ToString() +
                    "&CodigoCPCE=" + Session["CodigoCPCE"].ToString() + "&CodigoCLDE=" + ViewState["CodigoCLDEaux"].ToString() +
                "&CodigoPERS=" + ViewState["PersCodigoaux"].ToString() + "&NumeroDocumento=" + ViewState["NumDocumentoaux"].ToString() +
                "&Operacion=" + ViewState["Operacionaux"].ToString() + "&CodigoLTCA=" + Session["IdListaCabecera"].ToString() +
                "&CodigoUSU=0&Retornar=3", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgBuscarFono_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; " +
                "posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('WFrm_BuscarTelefonos.aspx" +
                "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=650px, status=no,resizable= yes, " +
                "scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
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
                ViewState["PersCodigo"].ToString() + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=500px, " +
                "status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
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

        protected void ImgCitacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; " +
                    "var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                    "window.open('../BPM/WFrm_NewCitacion.aspx?CodigoPERS=" + ViewState["PersCodigo"].ToString() +
                    "&CodigoCLDE=" + ViewState["CodigoCLDE"].ToString() + "&Retornar=0" + "',null,'left=' + posicion_x + " +
                    "', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, " +
                    "toolbar=no, location=no, menubar=no,titlebar=0');", true);
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
        #endregion
    }
}