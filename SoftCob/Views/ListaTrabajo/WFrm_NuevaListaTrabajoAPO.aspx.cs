namespace SoftCob.Views.ListaTrabajo
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Drawing;
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
        DataTable _dtbestrategia = new DataTable();
        DataTable _dtblistatrabajo = new DataTable();
        DataTable _dtbgestor = new DataTable();
        DataTable _dtbcodigos = new DataTable();
        DataTable _dtbgstsave = new DataTable();
        DataTable _dtbpreview = new DataTable();
        DataRow _result, _filagre;
        DataRow[] _resultado;
        string _sql = "", _estrategia = "", _ordenar = "", _fechaactual = "", _mensaje = "", _codigo = "",
            _sql1 = "", _gestor = "", _apoyo = "", _sql2 = "", _apoyogestor = "", _filename = "", _style = "";
        string[] _codigosopm;
        bool _validar = false, _continuar = true;
        int _tipogestion = 0, _codlistaarbol = 0, _congestor = 0, _operaciones = 0, _totalasignar = 0, _diferencia = 0,
            _codgestor = 0, _contar = 0, _otcontar = 0, _otcongestor = 0, _opcion = 0, _pasadas = 0;
        DateTime _dtmfechainicio, _dtmfechafin, _dtmfechaactual;
        CheckBox _chkselected = new CheckBox();
        ListItem _accion = new ListItem();
        ListItem _efecto = new ListItem();
        ListItem _respuesta = new ListItem();
        ListItem _contacto = new ListItem();
        ListItem _asignacion = new ListItem();
        ListItem _campania = new ListItem();
        ListItem _lista = new ListItem();
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
                if (Session["IN-CALL"].ToString() == "SI")
                {
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                _dtbcodigos.Columns.Add("Codigo");
                _dtbcodigos.Columns.Add("Descripcion");
                ViewState["CodigosOPM"] = _dtbcodigos;

                _dtbgstsave.Columns.Add("CodigoCLDE");
                _dtbgstsave.Columns.Add("codigoPERS");
                _dtbgstsave.Columns.Add("gestorasignado");
                _dtbgstsave.Columns.Add("estado");
                _dtbgstsave.Columns.Add("operacion");
                ViewState["DatosSave"] = _dtbgstsave;

                ViewState["CodigoLista"] = Request["CodigoLista"];
                ViewState["Preview"] = false;
                ViewState["CodigoCEDE"] = "0";
                ViewState["CodigoLTCA"] = "0";
                ViewState["CodigoCPCE"] = "0";
                ViewState["CodMarcado"] = "0";
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
                ViewState["CodigoCPCE"] = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                FunCargarCombos(2);
                DdlGestor.SelectedValue = _dts.Tables[0].Rows[0]["Gestor"].ToString();
                DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                DdlCatalogo.DataTextField = "CatalogoProducto";
                DdlCatalogo.DataValueField = "CodigoCatalogo";
                DdlCatalogo.DataBind();
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
                ViewState["Preview"] = true;
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
                    LblExportar.Visible = false;
                    ImgExportar.Visible = false;
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
            }
        }

        private bool FunValidarCampos()
        {
            _validar = true;

            if (string.IsNullOrEmpty(TxtLista.Text.Trim()))
            {
                new FuncionesDAO().FunShowJSMessage("Ingrese nombre de la Lista de Trabajo..!", this);
                _validar = false;
            }

            if (int.Parse(DdlCedente.SelectedValue) == 0)
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                _validar = false;
            }

            if (int.Parse(DdlCatalogo.SelectedValue) == 0)
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo/Producto..!", this);
                _validar = false;
            }

            if (!new FuncionesDAO().IsDate(TxtFechaInicio.Text))
            {
                new FuncionesDAO().FunShowJSMessage("Formato Fecha Incorrecta..!", this);
                _validar = false;
            }

            if (!new FuncionesDAO().IsDate(TxtFechaFin.Text))
            {
                new FuncionesDAO().FunShowJSMessage("Formato Fecha Incorrecta..!", this);
                _validar = false;
            }

            _fechaactual = DateTime.Now.ToString("MM/dd/yyyy");
            _dtmfechainicio = DateTime.ParseExact(TxtFechaInicio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfechafin = DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfechaactual = DateTime.ParseExact(_fechaactual, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            if (_dtmfechafin < _dtmfechainicio)
            {
                new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser mayor a Fecha Fin..!", this);
                _validar = false;
            }

            if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
            {
                if (_dtmfechainicio < _dtmfechaactual)
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser menor a la Fecha Actual..!", this);
                    _validar = false;
                }

                _dtbgestor = (DataTable)ViewState["GestorApoyo"];
                _result = _dtbgestor.Select("Selecc='SI'").FirstOrDefault();

                if (_result == null)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione al menos un Gestor..!", this);
                    _validar = false;
                }
            }

            if (RdbOpcionesApoyo.SelectedValue == "")
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Opción Gestor Apoyo", this);
                _validar = false;
            }
            return _validar;
        }

        private string FunCrearSQL(string sqlformado)
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

                sqlformado += _apoyo;
                sqlformado = sqlformado.Remove(sqlformado.Length - 1) + ")";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return sqlformado;
        }

        private string FunFormarSQL(string nuevoSQL, int tipo)
        {
            try
            {
                _continuar = true; _estrategia = ""; _ordenar = ""; _gestor = "";
                _pasadas = 0;
                _sql = nuevoSQL;
                _sql += "From SoftCob_CLIENTE_DEUDOR CL (nolock) INNER JOIN SoftCob_CUENTA_DEUDOR CD (nolock) ON CL.CLDE_CODIGO=CD.CLDE_CODIGO ";
                _sql += "INNER JOIN SoftCob_PERSONA PE (nolock) ON CL.PERS_CODIGO=PE.PERS_CODIGO ";
                _sql += "INNER JOIN SoftCob_PROVINCIA PR (nolock) ON PE.pers_provincia=PR.PROV_CODIGO ";
                _sql += "INNER JOIN SoftCob_CIUDAD CI (nolock) ON PE.pers_ciudad=CI.CIUD_CODIGO where ";
                _sql += "CL.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CL.clde_estado=1 and CD.ctde_estado=1 and ";
                _sql += "CD.ctde_gestorasignado in(";

                if (RdbOpcionesApoyo.SelectedValue == "1")
                {
                    _dtbgestor = (DataTable)ViewState["GestorApoyo"];
                    _resultado = _dtbgestor.Select("Selecc='SI'");

                    foreach (DataRow _row in _resultado)
                    {
                        _gestor += _row["Codigo"] + ",";
                    }
                }
                else _gestor = DdlGestor.SelectedValue + ",";

                _sql += _gestor;
                _sql = _sql.Remove(_sql.Length - 1) + ") and ";

                if (DdlAsignacion.SelectedValue != "0") _sql += "CD.ctde_auxv2='" + DdlAsignacion.SelectedValue + "' and ";

                if (DdlCampania.SelectedValue != "0") _sql += "CD.ctde_auxi4=" + DdlCampania.SelectedValue + " and ";

                if (_continuar)
                {
                    _dtbestrategia = (DataTable)ViewState["Estrategia"];

                    foreach (DataRow _row in _dtbestrategia.Rows)
                    {
                        if (_row["Operacion"].ToString() != "between")
                            _estrategia += _row["Campo"].ToString() + " " + _row["Operacion"].ToString() + " " + _row["Valor"].ToString() + " and ";
                        else
                        {
                            if (_pasadas == 0) _estrategia += _row["Campo"].ToString() + " " + _row["Operacion"].ToString() + " " + _row["Valor"].ToString() + " and ";
                            else _estrategia += _row["Valor"].ToString() + " and ";
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
                        if (!string.IsNullOrEmpty(_ordenar)) _ordenar = "Order by " + _ordenar.Remove(_ordenar.Length - 1);
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

        private void FunGenerarDatosSave()
        {
            try
            {
                _dtbgestor = (DataTable)ViewState["GestorApoyo"];
                _resultado = _dtbgestor.Select("Selecc='SI'");
                _congestor = _resultado.Count();
                _operaciones = int.Parse(LblTotal.InnerText);
                _dtbpreview = (DataTable)ViewState["CambiarPreview"];
                _dtbgstsave = (DataTable)ViewState["DatosSave"];
                _totalasignar = _operaciones / _congestor;
                _diferencia = _operaciones - (_totalasignar * _congestor);
                _codgestor = FunCodigoGestor();
                _otcongestor = 1;

                foreach (DataRow drfila in _dtbpreview.Rows)
                {
                    _contar++;
                    _otcontar++;
                    _filagre = _dtbgstsave.NewRow();
                    _filagre["CodigoCLDE"] = drfila["CodigoCLDE"].ToString();
                    _filagre["codigoPERS"] = drfila["codigoPERS"].ToString();
                    _filagre["gestorasignado"] = _codgestor;
                    _filagre["estado"] = 1;
                    _filagre["Operacion"] = drfila["Operacion"].ToString();
                    _dtbgstsave.Rows.Add(_filagre);

                    if (_contar == _totalasignar)
                    {
                        _codgestor = FunCodigoGestor();
                        _contar = 0;
                        _otcongestor++;

                        if (_congestor == _otcongestor)
                            _totalasignar = _totalasignar + _diferencia;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

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
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
                LblTotal.InnerText = "0";
                GrdvEstrategia.DataSource = null;
                GrdvEstrategia.DataBind();
                LblExportar.Visible = false;
                ImgExportar.Visible = false;
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
                _dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoCEDE"] = DdlCedente.SelectedValue;
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
                    ViewState["CodigoCPCE"] = DdlCatalogo.SelectedValue;
                    FunCargarCombos(2);
                }
                else
                {
                    DdlCatalogo.Items.Clear();
                    _itemc.Text = "--Seleccione Catálago/Producto--";
                    _itemc.Value = "0";
                    DdlCatalogo.Items.Add(_itemc);
                }
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
                ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
                FunCargarCombos(1);
                FunCargarCombos(2);
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
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
                LblTotal.InnerText = "0";
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
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
                LblTotal.InnerText = "0";
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
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
                LblTotal.InnerText = "0";
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
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
                LblTotal.InnerText = "0";
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
        protected void ChkArbol_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
                LblTotal.InnerText = "0";
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
                TblLista.Visible = false;
                _sql = ""; _sql1 = "";

                if (DdlEstrategia.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("No existe Estrategia Seleccionada..!", this);
                    return;
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    _tipogestion = 3;

                    if (ChkGestion.Checked) _tipogestion = 0;

                    if (ChkArbol.Checked) _tipogestion = 1;

                    if (!ChkGestion.Checked && !ChkArbol.Checked && ChkFecha.Checked) _tipogestion = 2;

                    _sql = "select distinct Cliente = PE.pers_nombrescompletos,Identificacion = PE.pers_numerodocumento,codigoCLDE = CD.CLDE_CODIGO,";
                    _sql += "Operacion = CD.ctde_operacion, DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible,";
                    _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre ";
                    _sql = FunFormarSQL(_sql, 1);

                    if (!string.IsNullOrEmpty(_sql))
                    {
                        _sql1 = "select CodigoCLDE = CDE.CLDE_CODIGO from SoftCob_CUENTA_DEUDOR CDE (nolock) ";
                        _sql1 += "INNER JOIN SoftCob_CLIENTE_DEUDOR CLI (nolock) ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                        _sql1 += "where CLI.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CDE.ctde_estado=1 and ";
                        _sql1 += "CDE.ctde_gestorasignado in(";
                        _sql1 = FunCrearSQL(_sql1);
                        _sql1 = _sql1 + " group by CDE.CLDE_CODIGO";

                        _sql2 = "select gete_cldecodigo,gete_araccodigo,gete_efectivo,";
                        _sql2 += "gete_fechagestion,gete_fechacreacion,gete_auxi1 ";
                        _sql2 += "from SoftCob_GESTION_TELEFONICA GTE (nolock) ";
                        _sql2 += "where gete_cedecodigo=" + DdlCedente.SelectedValue + " and gete_cpcecodigo=" + DdlCatalogo.SelectedValue + " and ";
                        _sql2 += "gete_auxi3=0 and gete_gestorasignado in(";
                        _sql2 = FunCrearSQL(_sql2);
                        _sql2 = _sql2 + " order by gete_fechacreacion asc";

                        _dtbgstsave = (DataTable)ViewState["DatosSave"];
                        _dts = new ListaTrabajoDAO().FunNewLstADE(0, _sql, int.Parse(ViewState["CodigoCEDE"].ToString()),
                            int.Parse(ViewState["CodigoCPCE"].ToString()), int.Parse(ViewState["CodigoLTCA"].ToString()),
                            _tipogestion, int.Parse(DdlTipoGestion.SelectedValue), int.Parse(DdlAccion.SelectedValue),
                            0, 0, 0, int.Parse(DdlGestor.SelectedValue), TxtFechaDesde.Text, TxtFechaHasta.Text,
                            _sql1, _sql2, "", "", ChkFecha.Checked ? 1 : 0, 0, 0, 0, _dtbgstsave,
                            Session["Conectar"].ToString());
                        Session["Preview"] = _dts;
                        ViewState["CambiarPreview"] = _dts.Tables[0];

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            TblLista.Visible = true;
                        }

                        GrdvPreview.DataSource = _dts.Tables[0];
                        GrdvPreview.DataBind();
                        LblTotal.InnerText = _dts.Tables[1].Rows[0]["Total"].ToString();
                    }
                    else new FuncionesDAO().FunShowJSMessage("No se puede formar la consulta..!", this);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                _filename = "PreviewApy_" + DdlCedente.SelectedItem.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + _filename);
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    GrdvPreview.AllowPaging = false;
                    GrdvPreview.DataSource = (DataSet)Session["Preview"];
                    GrdvPreview.DataBind();
                    GrdvPreview.HeaderRow.BackColor = Color.White;
                    foreach (GridViewRow row in GrdvPreview.Rows)
                    {
                        row.BackColor = Color.White;
                        row.Cells[1].Style.Add("mso-number-format", "\\@");
                        row.Cells[2].Style.Add("mso-number-format", "\\@");
                    }
                    GrdvPreview.RenderControl(hw);
                    _style = @"<style> .textmode { } </style>";
                    Response.Write(_style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
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
                _sql = ""; _sql1 = "";

                if (string.IsNullOrEmpty(TxtLista.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre de la Lista de Trabajo..!", this);
                    return;
                }

                if (DdlMarcado.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Marcado..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(94, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(ViewState["CodigoCPCE"].ToString()), 0, "", TxtLista.Text.Trim().ToUpper(),
                    TxtFechaInicio.Text.Trim(), Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Nombre de la Lista de Trabajo ya Existe..!", this);
                    _continuar = false;
                    return;
                }

                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor Apoyo..!", this);
                    return;
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    _tipogestion = 3;

                    if (ChkGestion.Checked) _tipogestion = 0;

                    if (ChkArbol.Checked) _tipogestion = 1;

                    if (!ChkGestion.Checked && !ChkArbol.Checked && ChkFecha.Checked) _tipogestion = 2;

                    if (int.Parse(LblTotal.InnerText) > 0)
                    {
                        if (int.Parse(ViewState["CodigoLista"].ToString()) > 0)
                        {
                            _sql = "Select top 0 codigoCLDE = CL.CLDE_CODIGO,codigoPERS = PE.PERS_CODIGO,codigoGEST = CD.ctde_gestorasignado,";
                            _sql += "Estado = 1,Operacion = CD.ctde_operacion,";
                            _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre,";
                            _sql += "DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible ";
                            _sql += "From SoftCob_CLIENTE_DEUDOR CL (nolock) INNER JOIN SoftCob_CUENTA_DEUDOR CD (nolock) ON CL.CLDE_CODIGO=CD.CLDE_CODIGO ";
                            _sql += "INNER JOIN SoftCob_PERSONA PE (nolock) ON CL.PERS_CODIGO=PE.PERS_CODIGO ";
                            _sql += "INNER JOIN SoftCob_Provincia PR (nolock) ON PE.pers_provincia=PR.PROV_CODIGO ";
                            _sql += "INNER JOIN SoftCob_Ciudad CI (nolock) ON PE.pers_ciudad=CI.CIUD_CODIGO where ";
                            _sql += "CL.CPCE_CODIGO=0 and CL.clde_estado=1 and CD.ctde_estado=1 and ";
                            _sql += "CD.ctde_gestorasignado=0";

                            _codlistaarbol = int.Parse(ViewState["CodigoLista"].ToString());

                            _sql1 = "select top 0 CodigoCLDE = CDE.CLDE_CODIGO from SoftCob_CUENTA_DEUDOR CDE (nolock) ";
                            _sql1 += "INNER JOIN SoftCob_CLIENTE_DEUDOR CLI (nolock) ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                            _sql1 += " where CLI.CPCE_CODIGO=0 and CDE.ctde_estado=1 and CDE.ctde_gestorasignado=0 ";

                            _dtbgstsave = (DataTable)ViewState["DatosSave"];
                        }
                        else
                        {
                            if (RdbOpcionesApoyo.SelectedValue == "1")
                            {
                                _sql = "Select distinct codigoCLDE = CL.CLDE_CODIGO,codigoPERS = PE.PERS_CODIGO,codigoGEST=" + DdlGestor.SelectedValue + ",";
                                _sql += "Estado = 1,Operacion = CD.ctde_operacion,";
                                _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre,";
                                _sql += "DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible ";
                                _sql = FunFormarSQL(_sql, 1);
                                _dtbgstsave = (DataTable)ViewState["DatosSave"];
                            }

                            _sql1 = "select CodigoCLDE = CDE.CLDE_CODIGO from SoftCob_CUENTA_DEUDOR CDE ";
                            _sql1 += "INNER JOIN SoftCob_CLIENTE_DEUDOR CLI ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                            _sql1 += "where CLI.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CDE.ctde_estado=1 and ";
                            _sql1 += "CDE.ctde_gestorasignado in(";
                            _sql1 = FunCrearSQL(_sql1);
                            _sql1 = _sql1 + " group by CDE.CLDE_CODIGO";

                            _sql2 = "select gete_cldecodigo,gete_araccodigo,gete_efectivo,";
                            _sql2 += "gete_fechagestion,gete_fechacreacion,gete_auxi1 ";
                            _sql2 += "from SoftCob_GESTION_TELEFONICA GTE (nolock) ";
                            _sql2 += "where gete_cedecodigo=" + DdlCedente.SelectedValue + " and gete_cpcecodigo=" + DdlCatalogo.SelectedValue + " and ";
                            _sql2 += "gete_auxi3=0 and gete_gestorasignado in(";
                            _sql2 = FunCrearSQL(_sql2);
                            _sql2 = _sql2 + " order by gete_fechacreacion asc";

                            if (RdbOpcionesApoyo.SelectedValue == "2") FunGenerarDatosSave();
                        }

                        _opcion = RdbOpcionesApoyo.SelectedValue == "1" ? 1 : 2;

                        _dts = new ListaTrabajoDAO().FunNewLstADE(_opcion, _sql, int.Parse(ViewState["CodigoCEDE"].ToString()),
                            int.Parse(ViewState["CodigoCPCE"].ToString()), int.Parse(ViewState["CodigoLTCA"].ToString()),
                            _tipogestion, int.Parse(DdlTipoGestion.SelectedValue), int.Parse(DdlAccion.SelectedValue),
                            0, 0, 0, int.Parse(DdlGestor.SelectedValue), TxtFechaDesde.Text, TxtFechaHasta.Text,
                            _sql1, _sql2, "", "", ChkFecha.Checked ? 1 : 0, 0, 0, 0, _dtbgstsave, Session["Conectar"].ToString());

                        _mensaje = new EstrategiaDAO().FunCrearListaTrabajo(_codlistaarbol, TxtLista.Text.Trim().ToUpper(),
                            TxtDescripcion.Text.Trim().ToUpper(), TxtFechaInicio.Text, TxtFechaFin.Text, int.Parse(DdlEstrategia.SelectedValue),
                            int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), ChkEstado.Checked,
                            DdlMarcado.SelectedValue, DdlCampania.SelectedValue, ChkGestion.Checked ? 1 : 0, DdlTipoGestion.SelectedValue,
                            ChkArbol.Checked ? 1 : 0, int.Parse(DdlAccion.SelectedValue), ChkFecha.Checked ? 1 : 0, TxtFechaDesde.Text.Trim(),
                            TxtFechaHasta.Text.Trim(), "0", DdlAsignacion.SelectedValue, DdlGestor.SelectedValue,
                            int.Parse(LblTotal.InnerText), 2, 0, int.Parse(Session["usuCodigo"].ToString()),
                            Session["MachineName"].ToString(), _dts.Tables[0], (DataTable)ViewState["Estrategia"],
                            "sp_NewListaTrabajo", Session["Conectar"].ToString());

                        if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
                        {
                            _apoyogestor = _apoyogestor.Remove(_apoyogestor.Length - 1);

                            _dts = new ConsultaDatosDAO().FunConsultaDatos(148, int.Parse(_mensaje), 0, 0, "",
                                _apoyogestor, RdbOpcionesApoyo.SelectedValue, Session["Conectar"].ToString());
                            _mensaje = _dts.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    else
                    {
                        new FuncionesDAO().FunShowJSMessage("No existen datos para registrar..!", this);
                        return;
                    }

                    if (_mensaje == "OK") Response.Redirect("WFrm_ListaTrabajoAdminAP.aspx?MensajeRetornado=Guardado con Éxito", true);
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