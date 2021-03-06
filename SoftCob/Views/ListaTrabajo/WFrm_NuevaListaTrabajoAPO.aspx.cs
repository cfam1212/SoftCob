namespace SoftCob.Views.ListaTrabajo
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class WFrm_NuevaListaTrabajoAPO : Page
    {
        #region Variables
        ListItem _itemc = new ListItem();
        DataSet _dts = new DataSet();
        DataSet _dts1 = new DataSet();
        DataTable _dtb = new DataTable();
        DataTable _dtbgestion = new DataTable();
        DataTable _dtbestrategia = new DataTable();
        DataTable _dtbgestor = new DataTable();
        DataTable _dtbcodigos = new DataTable();
        DataTable _dtbgstsave = new DataTable();
        DataTable _dtbpreview = new DataTable();
        DataRow _result, _filagre, _filnew;
        DataView view;
        DataRow[] _resultado, _resulgestor;
        string _sql = "", _estrategia = "", _ordenar = "", _fechaactual = "", _mensaje = "", _codigo = "", _fechagestion = "",
            _sql1 = "", _gestor = "", _apoyogestor = "", _filename = "", _apoyo = "";
        string[] _codigosopm, columnas;
        bool _validar = false, _continuar = true;
        int _codlistaarbol = 0, _codgestor = 0, _contar = 0, _otcontar = 0, _pasadas = 0, _efectivo = 0, _contarx = 0;
        DateTime _dtmfechainicio, _dtmfechafin, _dtmfechaactual, _dtmfecini, _dtmfecfin;
        CheckBox _chkselected = new CheckBox();
        ListItem _asignacion = new ListItem();
        ListItem _campania = new ListItem();
        CheckBox _chkselecc = new CheckBox();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ImgExportar);

            if (!IsPostBack)
            {
                _dtbcodigos.Columns.Add("Codigo");
                _dtbcodigos.Columns.Add("Descripcion");
                ViewState["CodigosOPM"] = _dtbcodigos;

                ViewState["CodigoLista"] = Request["CodigoLista"];
                ViewState["CodigoCEDE"] = "0";
                ViewState["CodigoLTCA"] = "0";
                ViewState["CodigoCPCE"] = "0";
                ViewState["CodMarcado"] = "0";

                _dtbgstsave.Columns.Add("CodigoCLDE");
                _dtbgstsave.Columns.Add("CodigoPERS");
                _dtbgstsave.Columns.Add("Gestorasignado");
                _dtbgstsave.Columns.Add("Estado");
                _dtbgstsave.Columns.Add("auxv1");
                _dtbgstsave.Columns.Add("auxv2");
                _dtbgstsave.Columns.Add("auxv3");
                _dtbgstsave.Columns.Add("auxi1");
                _dtbgstsave.Columns.Add("auxi2");
                _dtbgstsave.Columns.Add("auxi3");
                ViewState["DatosSave"] = _dtbgstsave;

                _dtbpreview.Columns.Add("Cliente");
                _dtbpreview.Columns.Add("Identificacion");
                _dtbpreview.Columns.Add("Provincia");
                _dtbpreview.Columns.Add("Ciudad");
                _dtbpreview.Columns.Add("FechaGestion");
                ViewState["DatosPreview"] = _dtbpreview;
                TxtFechaInicio.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaDesde.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaHasta.Text = DateTime.Now.ToString("MM/dd/yyyy");
                LblTotal.InnerText = "0";
                FunCargarCombos(0);
                FunCargarCombos(1);
                if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
                {
                    Lbltitulo.Text = "Nueva Lista de Trabajo << APOYO >>";
                    ViewState["Nuevo"] = "0";
                }
                else
                {
                    ViewState["CodigosOPM"] = _dtbcodigos;
                    ViewState["Nuevo"] = "1";
                    PnlConfiguracion.Enabled = false;
                    PnlGestores.Enabled = false;
                    PnlOpcionGestion.Enabled = false;
                    DdlEstrategia.Enabled = false;
                    DdlCedente.Enabled = false;
                    DdlCatalogo.Enabled = false;
                    RdbOpcionesApoyo.Enabled = false;
                    FunCargarMantenimiento();
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(147, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                    _codigosopm = _dts.Tables[0].Rows[0]["Codigos"].ToString().Split(',');
                    RdbOpcionesApoyo.SelectedValue = _dts.Tables[0].Rows[0]["Opcion"].ToString();
                    _dtbcodigos = (DataTable)ViewState["CodigosOPM"];
                    SoftCob_USUARIO user = new SoftCob_USUARIO();
                    foreach (var datos in _codigosopm)
                    {
                        user = new ControllerDAO().FunGetUsuarioPorID(int.Parse(datos));
                        _result = _dtbcodigos.NewRow();
                        _result["Codigo"] = datos;
                        _result["Descripcion"] = user.usua_nombres + " " + user.usua_apellidos;
                        _dtbcodigos.Rows.Add(_result);
                    }
                    GrdvOrigen.DataSource = _dtbcodigos;
                    GrdvOrigen.DataBind();
                    Lbltitulo.Text = "Editar Lista de Trabajo";
                    lblEstado.Visible = true;
                    ChkEstado.Visible = true;
                    ImgPreview.Enabled = false;
                    TxtFechaInicio.Enabled = false;
                }
            }
            else GrdvPreview.DataSource = ViewState["Preview"];
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(23, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                TxtLista.Text = _dts.Tables[0].Rows[0]["ListaTrabajo"].ToString();
                TxtDescripcion.Text = _dts.Tables[0].Rows[0]["Descripcion"].ToString();
                TxtFechaInicio.Text = _dts.Tables[0].Rows[0]["FechaInicio"].ToString();
                TxtFechaFin.Text = _dts.Tables[0].Rows[0]["FechaFin"].ToString();
                DdlEstrategia.SelectedValue = _dts.Tables[0].Rows[0]["Codigoesca"].ToString();
                DdlCedente.SelectedValue = _dts.Tables[0].Rows[0]["Codigocedente"].ToString();
                FunCargarCombos(1);
                //ViewState["CodigoCPCE"] = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                FunCargarCombos(2);
                DdlGestor.SelectedValue = _dts.Tables[0].Rows[0]["Gestor"].ToString();
                //DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                //DdlCatalogo.DataTextField = "CatalogoProducto";
                //DdlCatalogo.DataValueField = "CodigoCatalogo";
                //DdlCatalogo.DataBind();
                DdlCatalogo.SelectedValue = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                FunCargarCombos(2);
                DdlMarcado.SelectedValue = _dts.Tables[0].Rows[0]["Marcado"].ToString();
                DdlAsignacion.SelectedValue = _dts.Tables[0].Rows[0]["Asignacion"].ToString();
                DdlCampania.SelectedValue = _dts.Tables[0].Rows[0]["Campania"].ToString();
                ChkGestion.Checked = _dts.Tables[0].Rows[0]["PorGestion"].ToString() == "1" ? true : false;
                DdlTipoGestion.SelectedValue = _dts.Tables[0].Rows[0]["TipoGestion"].ToString();
                ChkArbol.Checked = _dts.Tables[0].Rows[0]["PorArbol"].ToString() == "1" ? true : false;
                DdlAccion.SelectedValue = _dts.Tables[0].Rows[0]["Codigoarac"].ToString();
                ChkFecha.Checked = _dts.Tables[0].Rows[0]["PorFecha"].ToString() == "1" ? true : false;
                TxtFechaDesde.Text = _dts.Tables[0].Rows[0]["FechaDesde"].ToString();
                TxtFechaHasta.Text = _dts.Tables[0].Rows[0]["FechaHasta"].ToString();
                ChkEstado.Text = _dts.Tables[0].Rows[0]["Estado"].ToString();
                ChkEstado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                DdlGestor.SelectedValue = _dts.Tables[0].Rows[0]["GestorApoyo"].ToString();
                _dts = new ConsultaDatosDAO().FunConsultaDatos(26, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                GrdvEstrategia.DataSource = _dts;
                GrdvEstrategia.DataBind();
                ViewState["Estrategia"] = _dts.Tables[0];
                ImgPreview.ImageUrl = "~/Botones/Buscargris.png";
                ImgPreview.Enabled = false;
                LblPreview.Visible = false;
                pnlPreview.Visible = false;
                _dts = new ConsultaDatosDAO().FunConsultaDatos(25, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                LblTotal.InnerText = _dts.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunLimpiar()
        {
            GrdvPreview.DataSource = null;
            GrdvPreview.DataBind();
            LblTotal.InnerText = "0";
            _dtbgstsave = (DataTable)ViewState["DatosSave"];
            _dtbgstsave.Clear();
            _dtbpreview = (DataTable)ViewState["DatosPreview"];
            _dtbpreview.Clear();
        }

        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlEstrategia.DataSource = new CedenteDAO().FunGetEstrategiaCab();
                    DdlEstrategia.DataTextField = "Descripcion";
                    DdlEstrategia.DataValueField = "Codigo";
                    DdlEstrategia.DataBind();

                    DdlCedente.DataSource = new CedenteDAO().FunGetCedentes();
                    DdlCedente.DataTextField = "Descripcion";
                    DdlCedente.DataValueField = "Codigo";
                    DdlCedente.DataBind();

                    _itemc.Text = "--Seleccione Catálago/Producto--";
                    _itemc.Value = "0";
                    DdlCatalogo.Items.Add(_itemc);

                    _asignacion.Text = "--Seleccione Asignación--";
                    _asignacion.Value = "0";
                    DdlAsignacion.Items.Add(_asignacion);

                    _campania.Text = "--Seleccione Campaña--";
                    _campania.Value = "0";
                    DdlCampania.Items.Add(_campania);

                    DdlMarcado.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO MARCADO", "--Seleccione Tipo Marcado--", "S");
                    DdlMarcado.DataTextField = "Descripcion";
                    DdlMarcado.DataValueField = "Codigo";
                    DdlMarcado.DataBind();

                    break;
                case 1:
                    GrdvPreview.DataSource = null;
                    GrdvPreview.DataBind();
                    LblTotal.InnerText = "0";

                    DdlGestor.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", 
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestor.DataTextField = "Descripcion";
                    DdlGestor.DataValueField = "Codigo";
                    DdlGestor.DataBind();

                    _dts1 = new ConsultaDatosDAO().FunConsultaDatos(12, int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    ViewState["GestorApoyo"] = _dts1.Tables[0];
                    GrdvOrigen.DataSource = _dts1;
                    GrdvOrigen.DataBind();

                    _dts1 = new ConsultaDatosDAO().FunConsultaDatos(81, int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "",
                        Session["Conectar"].ToString());
                    DdlCatalogo.DataSource = _dts1;
                    DdlCatalogo.DataTextField = "Descripcion";
                    DdlCatalogo.DataValueField = "Codigo";
                    DdlCatalogo.DataBind();
                    break;
                case 2:
                    DdlAccion.DataSource = new SpeechDAO().FunGetArbolNewAccion(int.Parse(DdlCatalogo.SelectedValue));
                    DdlAccion.DataTextField = "Descripcion";
                    DdlAccion.DataValueField = "Codigo";
                    DdlAccion.DataBind();

                    DdlAsignacion.DataSource = new ConsultaDatosDAO().FunConsultaDatos(91, int.Parse(DdlCatalogo.SelectedValue), 0, 0, "",
                        "", "", Session["Conectar"].ToString());
                    DdlAsignacion.DataTextField = "Descripcion";
                    DdlAsignacion.DataValueField = "Codigo";
                    DdlAsignacion.DataBind();

                    DdlCampania.DataSource = new ConsultaDatosDAO().FunConsultaDatos(119, int.Parse(DdlCatalogo.SelectedValue), 0, 0, "",
                        "", "", Session["Conectar"].ToString());
                    DdlCampania.DataTextField = "Descripcion";
                    DdlCampania.DataValueField = "Codigo";
                    DdlCampania.DataBind();
                    break;
                case 3:
                    GrdvPreview.DataSource = null;
                    GrdvPreview.DataBind();
                    TblLista.Visible = false;
                    LblTotal.InnerText = "0";
                    break;
            }
        }

        private bool FunValidarCampos()
        {
            _validar = true;

            if (string.IsNullOrEmpty(TxtLista.Text.Trim()))
            {
                new FuncionesDAO().FunShowJSMessage("Ingrese nombre de la Lista de Trabajo..!", this, "W", "C");
                _validar = false;
            }

            if (int.Parse(DdlCedente.SelectedValue) == 0)
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this, "W", "C");
                _validar = false;
            }

            if (int.Parse(DdlCatalogo.SelectedValue) == 0)
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo/Producto..!", this, "W", "C");
                _validar = false;
            }

            if (!new FuncionesDAO().IsDate(TxtFechaInicio.Text, "MM/dd/yyyy"))
            {
                new FuncionesDAO().FunShowJSMessage("Formato Fecha Incorrecta..!", this, "E", "C");
                _validar = false;
            }

            if (!new FuncionesDAO().IsDate(TxtFechaFin.Text, "MM/dd/yyyy"))
            {
                new FuncionesDAO().FunShowJSMessage("Formato Fecha Incorrecta..!", this, "E", "C");
                _validar = false;
            }

            _fechaactual = DateTime.Now.ToString("MM/dd/yyyy");
            _dtmfechainicio = DateTime.ParseExact(TxtFechaInicio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfechafin = DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfechaactual = DateTime.ParseExact(_fechaactual, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            _dtmfecini = DateTime.ParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfecfin = DateTime.ParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            if (_dtmfechafin < _dtmfechainicio)
            {
                new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser mayor a Fecha Fin..!", this, "E", "C");
                _validar = false;
            }

            TimeSpan _diferencia = _dtmfecfin.Subtract(_dtmfecini);

            if (_diferencia.Days > 31)
            {
                new FuncionesDAO().FunShowJSMessage("Definir la consulta son en rangos de 30 a 31 dias..!", this, "E", "C");
                _validar = false;
            }

            if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
            {
                if (_dtmfechainicio < _dtmfechaactual)
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser menor a la Fecha Actual..!", this, "E", "C");
                    _validar = false;
                }

                _dtbgestor = (DataTable)ViewState["GestorApoyo"];
                _result = _dtbgestor.Select("Selecc='SI'").FirstOrDefault();

                if (_result == null)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione al menos un Gestor..!", this, "W", "C");
                    _validar = false;
                }
            }

            if (RdbOpcionesApoyo.SelectedValue == "")
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Opción Gestor Apoyo", this, "W", "C");
                _validar = false;
            }
            return _validar;
        }
        private string FunCrearSQL()
        {
            try
            {
                _apoyo = ""; _apoyogestor = "";
                _dtbgestor = (DataTable)ViewState["GestorApoyo"];
                _resultado = _dtbgestor.Select("Selecc='SI'");

                foreach (DataRow _row in _resultado)
                {
                    _apoyo += _row["Codigo"] + ",";
                    _apoyogestor += _row["Codigo"] + ",";
                }

                if (RdbOpcionesApoyo.SelectedValue == "2") _apoyo = DdlGestor.SelectedValue + ",";

                _sql += _apoyo;
                _sql = _sql.Remove(_sql.Length - 1) + ")";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _sql;
        }
        private string FunFormarSQL(string nuevoSQL, int tipo)
        {
            try
            {
                _continuar = true; _estrategia = ""; _ordenar = ""; _gestor = "";
                _pasadas = 0;
                _sql = nuevoSQL;

                if (RdbOpcionesApoyo.SelectedValue == "2")
                {
                    _dtbgestor = (DataTable)ViewState["GestorApoyo"];
                    _resultado = _dtbgestor.Select("Selecc='SI'");

                    foreach (DataRow _row in _resultado)
                    {
                        _gestor += _row["Codigo"] + ",";
                    }

                    _sql += _gestor;
                    _sql = _sql.Remove(_sql.Length - 1) + ") AND ";
                }
                else
                {
                    //_sql = _sql.Remove(_sql.Length - 1) + " AND ";
                }

                
                //_sql = _sql.Remove(_sql.Length - 1) + ") AND ";

                if (DdlAsignacion.SelectedValue != "0") _sql += "CDE.ctde_auxv2='" + DdlAsignacion.SelectedValue + "' AND ";

                if (DdlCampania.SelectedValue != "0") _sql += "CDE.ctde_auxi4=" + DdlCampania.SelectedValue + " AND ";

                if (_continuar)
                {
                    _dtbestrategia = (DataTable)ViewState["Estrategia"];

                    foreach (DataRow _row in _dtbestrategia.Rows)
                    {
                        if (_row["Operacion"].ToString() != "BETWEEN")
                            _estrategia += _row["Campo"].ToString() + " " + _row["Operacion"].ToString() + " " + _row["Valor"].ToString() + " AND ";
                        else
                        {
                            if (_pasadas == 0) _estrategia += _row["Campo"].ToString() + " " + _row["Operacion"].ToString() + " " + _row["Valor"].ToString() + " AND ";
                            else _estrategia += _row["Valor"].ToString() + " AND ";
                        }

                        if (tipo == 1)
                        {
                            if (!string.IsNullOrEmpty(_row[4].ToString()))
                            {
                                _ordenar += _row[1].ToString() + " " + _row[4].ToString() + ",";
                            }
                        }
                    }

                    if (tipo == 1)
                    {
                        if (!string.IsNullOrEmpty(_ordenar)) _ordenar = "ORDER BY " + _ordenar.Remove(_ordenar.Length - 1);
                    }

                    _sql += _estrategia;
                    _sql = _sql.Remove(_sql.Length - 4) + " " + _ordenar;
                }
                else _sql = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _sql;
        }

        //private void FunGenerarDatosSave()
        //{
        //    try
        //    {
        //        _dtbgestor = (DataTable)ViewState["GestorApoyo"];
        //        _resultado = _dtbgestor.Select("Selecc='SI'");
        //        _congestor = _resultado.Count();
        //        _operaciones = int.Parse(LblTotal.InnerText);
        //        _dtbpreview = (DataTable)ViewState["CambiarPreview"];
        //        _dtbgstsave = (DataTable)ViewState["DatosSave"];
        //        _totalasignar = _operaciones / _congestor;
        //        _diferencia = _operaciones - (_totalasignar * _congestor);
        //        _codgestor = FunCodigoGestor();
        //        _otcongestor = 1;

        //        foreach (DataRow drfila in _dtbpreview.Rows)
        //        {
        //            _contar++;
        //            _otcontar++;
        //            _filagre = _dtbgstsave.NewRow();
        //            _filagre["CodigoCLDE"] = drfila["CodigoCLDE"].ToString();
        //            _filagre["codigoPERS"] = drfila["codigoPERS"].ToString();
        //            _filagre["gestorasignado"] = _codgestor;
        //            _filagre["estado"] = 1;
        //            _filagre["Operacion"] = drfila["Operacion"].ToString();
        //            _dtbgstsave.Rows.Add(_filagre);

        //            if (_contar == _totalasignar)
        //            {
        //                _codgestor = FunCodigoGestor();
        //                _contar = 0;
        //                _otcongestor++;

        //                if (_congestor == _otcongestor)
        //                    _totalasignar = _totalasignar + _diferencia;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        private int FunCodigoGestor()
        {
            try
            {
                _dtbgestor = (DataTable)ViewState["GestorApoyo"];
                _result = _dtbgestor.Select("Selecc='SI'").FirstOrDefault();
                _codgestor = int.Parse(_result["Codigo"].ToString());
                _result["Selecc"] = "ASIGNADO";
                _dtbgestor.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _codgestor;
        }
        #endregion

        #region Botones y Eventos
        protected void DdlEstrategia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);
                ViewState["Estrategia"] = null;
                ViewState["CodigoEstrategia"] = DdlEstrategia.SelectedValue;

                if (int.Parse(DdlEstrategia.SelectedValue) > 0)
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(22, int.Parse(DdlEstrategia.SelectedValue), 0, 0, "", "", "", 
                        Session["Conectar"].ToString());
                    GrdvEstrategia.DataSource = _dts;
                    GrdvEstrategia.DataBind();
                    ViewState["Estrategia"] = _dts.Tables[0];
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(1);
                FunCargarCombos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(2);
                FunCargarCombos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void RdbOpcionesApoyo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
                LblTotal.InnerText = "0";
                DdlAccion.SelectedValue = "0";
                DdlTipoGestion.Enabled = false;
                DdlAccion.Enabled = false;
                ChkArbol.Checked = false;
                if (ChkGestion.Checked) DdlTipoGestion.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkOpciones_CheckedChanged(object sender, EventArgs e)
        {
            PnlOpcionGestion.Visible = false;

            if (ChkOpciones.Checked)
                PnlOpcionGestion.Visible = true;
        }

        protected void DdlGestor_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunLimpiar();
        }
        protected void GrdvOrigen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                _dtbcodigos = (DataTable)ViewState["CodigosOPM"];

                if (e.Row.RowIndex >= 0)
                {
                    _chkselecc = (CheckBox)(e.Row.Cells[1].FindControl("ChkSelecc"));
                    _codigo = GrdvOrigen.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString();
                    _result = _dtbcodigos.Select("Codigo='" + _codigo + "'").FirstOrDefault();

                    if (_result != null) _chkselecc.Checked = true;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlTipoGestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCampania_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlAsignacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkSelecc_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkselected = (CheckBox)(_gvRow.Cells[1].FindControl("ChkSelecc"));
                _codigo = GrdvOrigen.DataKeys[_gvRow.RowIndex].Values["Codigo"].ToString();
                _dtbgestor = (DataTable)ViewState["GestorApoyo"];
                _result = _dtbgestor.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Selecc"] = _chkselected.Checked ? "SI" : "NO";
                _dtbgestor.AcceptChanges();
                ViewState["GestorApoyo"] = _dtbgestor;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkGestion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);
                DdlAccion.SelectedValue = "0";
                DdlTipoGestion.Enabled = false;
                DdlAccion.Enabled = false;
                ChkArbol.Checked = false;
                if (ChkGestion.Checked) DdlTipoGestion.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkArbol_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);
                DdlAccion.SelectedValue = "0";
                DdlAccion.Enabled = false;
                ChkGestion.Checked = false;
                DdlTipoGestion.SelectedValue = "-1";
                DdlTipoGestion.Enabled = false;
                if (ChkArbol.Checked) DdlAccion.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkFecha_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
                LblTotal.InnerText = "0";
                TxtFechaDesde.Enabled = false;
                TxtFechaHasta.Enabled = false;
                if (ChkFecha.Checked)
                {
                    TxtFechaDesde.Enabled = true;
                    TxtFechaHasta.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgPreview_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _sql = ""; _sql1 = "";
                TblLista.Visible = false;

                if (DdlEstrategia.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("No existe Estrategia Seleccionada..!", this, "W", "C");
                    return;
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    _sql1 = "";
                    _sql1 = "SELECT CodigoCLDE=CDE.CLDE_CODIGO,CodigoPERS=PER.PERS_CODIGO,";
                    _sql1 += "Gestorasignado=CDE.ctde_gestorasignado,Estado=1,FechaGestion=CONVERT(DATE,CDE.ctde_auxv3,121),";
                    _sql1 += "Cliente=PER.pers_nombrescompletos,Identificacion=PER.pers_numerodocumento,";
                    _sql1 += "Provincia=(SELECT PRV.prov_nombre FROM SoftCob_Provincia PRV (NOLOCK) WHERE PRV.PROV_CODIGO=PER.pers_provincia),";
                    _sql1 += "Ciudad=(SELECT CIU.ciud_nombre FROM SoftCob_Ciudad CIU (NOLOCK) WHERE CIU.CIUD_CODIGO=PER.pers_ciudad) ";
                    _sql1 += "FROM SoftCob_CUENTA_DEUDOR CDE (NOLOCK) INNER JOIN SoftCob_CLIENTE_DEUDOR CLI (NOLOCK) ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                    _sql1 += "INNER JOIN SoftCob_PERSONA PER (NOLOCK) ON CLI.PERS_CODIGO=PER.PERS_CODIGO ";
                    _sql1 += "WHERE CLI.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " AND CDE.ctde_estado=1 AND CLI.clde_estado=1 AND ";

                    if (RdbOpcionesApoyo.SelectedValue == "1")
                    {
                        if (DdlGestor.SelectedValue != "0")
                        {
                            _sql1 += "CDE.ctde_gestorasignado=" + DdlGestor.SelectedValue + " AND ";
                            
                        }
                        else _sql1 += "CDE.ctde_gestorasignado>0 AND ";
                    }
                    else
                    {
                        _sql1 += "CDE.ctde_gestorasignado IN(";
                    }

                    _sql1 = FunFormarSQL(_sql1, 0);

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql1, "", "", Session["Conectar"].ToString());

                    columnas = new[] { "Identificacion", "Cliente", "CodigoCLDE", "CodigoPERS", "Provincia", "Ciudad",
                        "Gestorasignado", "FechaGestion" };

                    view = new DataView(_dts.Tables[0]);
                    _dtb = view.ToTable(true, columnas);

                    _dtbpreview = _dtb;

                    if (ChkOpciones.Checked)
                    {
                        _sql1 = "";
                        _sql1 = "SELECT CodigoCLDE = GTE.gete_cldecodigo,Efectivo = CAST(CASE GTE.gete_efectivo WHEN 1 THEN 1 ELSE 0 END AS varchar),";
                        _sql1 += "FechaGestion = CONVERT(VARCHAR(10),GTE.gete_fechagestion,121) ";
                        _sql1 += "FROM SoftCob_GESTION_TELEFONICA GTE (NOLOCK) ";
                        _sql1 += "INNER JOIN SoftCob_CUENTA_DEUDOR CDE ON CDE.ctde_operacion=GTE.gete_operacion ";
                        _sql1 += "WHERE GTE.gete_cpcecodigo=" + DdlCatalogo.SelectedValue + " AND ";
                        _sql1 += "CONVERT(DATE,GTE.gete_fechagestion,101) BETWEEN ";
                        _sql1 += "CONVERT(DATE,'" + TxtFechaDesde.Text + "',101) AND CONVERT(DATE,'" + TxtFechaHasta.Text + "',101) AND ";
                        _sql1 += "GTE.gete_auxi3=0 AND CDE.ctde_estado=1 AND ";
                        if (ChkArbol.Checked) _sql1 += "GTE.gete_araccodigo=" + DdlAccion.SelectedValue + " AND ";
                    

                    _sql1 += "CDE.ctde_gestorasignado IN(";
                    _sql1 = FunFormarSQL(_sql1, 0);
                    _sql1 += "ORDER BY GTE.gete_cldecodigo";

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql1, "", "", Session["Conectar"].ToString());

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            _dtbgestion = _dts.Tables[0];

                            _dtbpreview = (DataTable)ViewState["DatosPreview"];
                            _dtbpreview.Clear();

                            _dtbgstsave = (DataTable)ViewState["DatosSave"];
                            _dtbgstsave.Clear();

                            _dtbgestor = (DataTable)ViewState["GestorApoyo"];
                            _resulgestor = _dtbgestor.Select("Selecc='SI'");

                            _contar = _resulgestor.Count();

                            foreach (DataRow _drfila in _dtb.Rows)
                            {
                                _resultado = _dtbgestion.Select("CodigoCLDE='" + _drfila["CodigoCLDE"].ToString() + "'");

                                _fechagestion = Convert.ToString(_resultado.AsEnumerable()
                                                        .Max(row => row["FechaGestion"]));

                                if (_resultado != null && _resultado.Length > 0)
                                {
                                    if (RdbOpcionesApoyo.SelectedValue == "2")
                                    {

                                        if (_dtbgestor.Rows.Count == 1)
                                        {
                                            _codgestor = int.Parse(_resulgestor[0]["Codigo"].ToString());
                                        }
                                        else
                                        {
                                            if (_contar == _otcontar) _otcontar = 0;
                                            _codgestor = int.Parse(_resulgestor[_otcontar]["Codigo"].ToString());
                                        }
                                        _otcontar++;
                                    }
                                    else _codgestor = int.Parse(DdlGestor.SelectedValue);

                                    if (ChkOpciones.Checked)
                                    {
                                        if (ChkGestion.Checked)
                                        {
                                            _efectivo = Convert.ToInt32(_resultado.AsEnumerable()
                                                                    .Max(row => row["Efectivo"]));

                                            if (DdlTipoGestion.SelectedValue == "0")
                                            {
                                                if (_efectivo == 0)
                                                {
                                                    _filagre = _dtbpreview.NewRow();
                                                    _filagre["Identificacion"] = _drfila["Identificacion"].ToString();
                                                    _filagre["Cliente"] = _drfila["Cliente"].ToString();
                                                    _filagre["Provincia"] = _drfila["Provincia"].ToString();
                                                    _filagre["Ciudad"] = _drfila["Ciudad"].ToString();
                                                    _filagre["FechaGestion"] = _fechagestion;
                                                    _dtbpreview.Rows.Add(_filagre);

                                                    _filnew = _dtbgstsave.NewRow();
                                                    _filnew["CodigoCLDE"] = _drfila["CodigoCLDE"].ToString();
                                                    _filnew["CodigoPERS"] = _drfila["CodigoPERS"].ToString();
                                                    _filnew["Gestorasignado"] = _codgestor;
                                                    _filnew["Estado"] = "1";
                                                    _filnew["auxv1"] = "";
                                                    _filnew["auxv2"] = "";
                                                    _filnew["auxv3"] = "";
                                                    _filnew["auxi1"] = "";
                                                    _filnew["auxi2"] = "";
                                                    _filnew["auxi3"] = "";
                                                    _dtbgstsave.Rows.Add(_filnew);
                                                }
                                            }
                                            else
                                            {
                                                if (_efectivo == 1)
                                                {
                                                    _filagre = _dtbpreview.NewRow();
                                                    _filagre["Identificacion"] = _drfila["Identificacion"].ToString();
                                                    _filagre["Cliente"] = _drfila["Cliente"].ToString();
                                                    _filagre["Provincia"] = _drfila["Provincia"].ToString();
                                                    _filagre["Ciudad"] = _drfila["Ciudad"].ToString();
                                                    _filagre["FechaGestion"] = _fechagestion;
                                                    _dtbpreview.Rows.Add(_filagre);

                                                    _filnew = _dtbgstsave.NewRow();
                                                    _filnew["CodigoCLDE"] = _drfila["CodigoCLDE"].ToString();
                                                    _filnew["CodigoPERS"] = _drfila["CodigoPERS"].ToString();
                                                    _filnew["Gestorasignado"] = _codgestor;
                                                    _filnew["Estado"] = "1";
                                                    _filnew["auxv1"] = "";
                                                    _filnew["auxv2"] = "";
                                                    _filnew["auxv3"] = "";
                                                    _filnew["auxi1"] = "";
                                                    _filnew["auxi2"] = "";
                                                    _filnew["auxi3"] = "";
                                                    _dtbgstsave.Rows.Add(_filnew);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _filagre = _dtbpreview.NewRow();
                                            _filagre["Identificacion"] = _drfila["Identificacion"].ToString();
                                            _filagre["Cliente"] = _drfila["Cliente"].ToString();
                                            _filagre["Provincia"] = _drfila["Provincia"].ToString();
                                            _filagre["Ciudad"] = _drfila["Ciudad"].ToString();
                                            _filagre["FechaGestion"] = _fechagestion;
                                            _dtbpreview.Rows.Add(_filagre);

                                            _filnew = _dtbgstsave.NewRow();
                                            _filnew["CodigoCLDE"] = _drfila["CodigoCLDE"].ToString();
                                            _filnew["CodigoPERS"] = _drfila["CodigoPERS"].ToString();
                                            _filnew["Gestorasignado"] = _codgestor;
                                            _filnew["Estado"] = "1";
                                            _filnew["auxv1"] = "";
                                            _filnew["auxv2"] = "";
                                            _filnew["auxv3"] = "";
                                            _filnew["auxi1"] = "";
                                            _filnew["auxi2"] = "";
                                            _filnew["auxi3"] = "";
                                            _dtbgstsave.Rows.Add(_filnew);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        _dtbgestor = (DataTable)ViewState["GestorApoyo"];

                        _resulgestor = _dtbgestor.Select("Selecc='SI'");
                        _contar = _resulgestor.Count();

                        _dtbgstsave = (DataTable)ViewState["DatosSave"];
                        _dtbgstsave.Clear();

                        if (_contar == 0) new FuncionesDAO().FunShowJSMessage("Debe Seleccionar al menos un gestor..!", this, "E", "C");
                        _contarx = 0;

                        foreach (DataRow _drfila in _dtb.Rows)
                        {
                            if (RdbOpcionesApoyo.SelectedValue == "1")
                            {
                                _codgestor = FunCodigoGestor();
                                _contarx++;
                            }
                            else _codgestor = int.Parse(DdlGestor.SelectedValue);

                            _filnew = _dtbgstsave.NewRow();
                            _filnew["CodigoCLDE"] = _drfila["CodigoCLDE"].ToString();
                            _filnew["CodigoPERS"] = _drfila["CodigoPERS"].ToString();
                            _filnew["Gestorasignado"] = _codgestor;
                            _filnew["Estado"] = "1";
                            _filnew["auxv1"] = "";
                            _filnew["auxv2"] = "";
                            _filnew["auxv3"] = "";
                            _filnew["auxi1"] = "";
                            _filnew["auxi2"] = "";
                            _filnew["auxi3"] = "";
                            _dtbgstsave.Rows.Add(_filnew);

                            if (RdbOpcionesApoyo.SelectedValue == "1")
                            {
                                if (_contar == _contarx)
                                {
                                    _dtbgestor = (DataTable)ViewState["GestorApoyo"];
                                    _resulgestor = _dtbgestor.Select("Selecc='ASIGNADO'");
                                    foreach (var _drgestor in _resulgestor)
                                    {
                                        _drgestor["Selecc"] = "SI";
                                        _dtbgestor.AcceptChanges();
                                    }
                                    ViewState["GestorApoyo"] = _dtbgestor;
                                    _contarx = 0;
                                }
                            }
                        }
                    }


                    if (_dtbpreview.Rows.Count > 0)
                    {
                        ViewState["Preview"] = _dtbpreview;

                        if (_dtbpreview.Rows.Count > 0) TblLista.Visible = true;

                        GrdvPreview.DataSource = _dtbpreview;
                        GrdvPreview.DataBind();
                        LblTotal.InnerText = _dtbpreview.Rows.Count.ToString();
                    }
                    else new FuncionesDAO().FunShowJSMessage("No Existen Datos Para Crear Lista..!", this, "E", "C");
                }

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void GrdvPreview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdvPreview.PageIndex = e.NewPageIndex;
            GrdvPreview.DataBind();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["Preview"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    _filename = "PreviewApy_" + DdlCedente.SelectedItem.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + _filename);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
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
                if (string.IsNullOrEmpty(TxtLista.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre de la Lista de Trabajo..!", this, "W", "C");
                    return;
                }

                if (DdlMarcado.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Marcado..!", this, "W", "C");
                    return;
                }

                if (ViewState["Nuevo"].ToString() == "0")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(94, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(DdlCatalogo.SelectedValue), 0, "", TxtLista.Text.Trim().ToUpper(),
                    TxtFechaInicio.Text.Trim(), Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Nombre de la Lista de Trabajo ya Existe..!", this, "E", "C");
                        _continuar = false;
                        return;
                    }

                    if (RdbOpcionesApoyo.SelectedValue == "2")
                    {
                        if (DdlGestor.SelectedValue == "0")
                        {
                            new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "C");
                            return;
                        }
                    }
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {

                    if (int.Parse(LblTotal.InnerText) > 0)
                    {
                        _dtbgstsave = (DataTable)ViewState["DatosSave"];
                        _codlistaarbol = int.Parse(ViewState["CodigoLista"].ToString());

                        _mensaje = new EstrategiaDAO().FunCrearListaTrabajo(_codlistaarbol, TxtLista.Text.Trim().ToUpper(),
                            TxtDescripcion.Text.Trim().ToUpper(), TxtFechaInicio.Text, TxtFechaFin.Text, int.Parse(DdlEstrategia.SelectedValue),
                            int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), ChkEstado.Checked,
                            DdlMarcado.SelectedValue, DdlCampania.SelectedValue, ChkGestion.Checked ? 1 : 0, DdlTipoGestion.SelectedValue,
                            ChkArbol.Checked ? 1 : 0, int.Parse(DdlAccion.SelectedValue), ChkFecha.Checked ? 1 : 0, TxtFechaDesde.Text.Trim(),
                            TxtFechaHasta.Text.Trim(), "0", DdlAsignacion.SelectedValue, DdlGestor.SelectedValue == "0" ? "1" : DdlGestor.SelectedValue,
                            int.Parse(LblTotal.InnerText), 2, 0, int.Parse(Session["usuCodigo"].ToString()),
                            Session["MachineName"].ToString(), _dtbgstsave, (DataTable)ViewState["Estrategia"],
                            "sp_NewListaTrabajo", Session["Conectar"].ToString());

                        if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
                        {
                            _sql1 =  FunCrearSQL();
                            _apoyogestor = _apoyogestor.Remove(_apoyogestor.Length - 1);

                            _dts = new ConsultaDatosDAO().FunConsultaDatos(148, int.Parse(_mensaje), 0, 0, "",
                                _apoyogestor, RdbOpcionesApoyo.SelectedValue, Session["Conectar"].ToString());
                            _mensaje = _dts.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    else
                    {
                        new FuncionesDAO().FunShowJSMessage("No existen datos para registrar..!", this, "E", "C");
                        return;
                    }

                    if (_mensaje == "OK") Response.Redirect("WFrm_ListaTrabajoAdminAP.aspx?MensajeRetornado=Guaradado Con Éxito", true);
                    else Lblerror.Text = _mensaje;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ListaTrabajoAdminAP.aspx", true);
        }

        #endregion
    }
}