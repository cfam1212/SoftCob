namespace SoftCob.Views.ListaTrabajo
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevaListaTrabajo : Page
    {
        #region Variables
        ListItem _itemc = new ListItem();
        DataSet _dts = new DataSet();
        DataTable _dtbestrategia = new DataTable();
        DataView view;
        string _sql = "", _estrategia = "", _ordenar = "", _mensaje = "", _fechaactual = "";
        bool _validar = false, _continuar = true;
        int _codlistaarbol = 0, _pasadas = 0;
        DataTable _dtbgstsave = new DataTable();
        DataTable _dtbpreview = new DataTable();
        DataTable _dtb = new DataTable();
        DateTime _dtmfechainicio, _dtmfechafin, _dtmfechaactual;
        ListItem _asignacion = new ListItem();
        ListItem _campania = new ListItem();
        string[] columnas;
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
                    Response.Redirect("../Gestion/WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                ViewState["CodigoLista"] = Request["CodigoLista"];
                ViewState["Regresar"] = Request["Regresar"];
                ViewState["CodLista"] = null;
                ViewState["CodCatalogo"] = null;
                ViewState["CodMarcado"] = null;
                TxtFechaInicio.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                LblTotal.InnerText = "0";
                FunCargarCombos(0);
                FunCargarCombos(1);

                _dtbgstsave.Columns.Add("CodigoCLDE");
                _dtbgstsave.Columns.Add("CodigoPERS");
                _dtbgstsave.Columns.Add("Gestorasignado");
                _dtbgstsave.Columns.Add("Estado");
                _dtbgstsave.Columns.Add("Operacion");
                _dtbgstsave.Columns.Add("FechaGestion");
                _dtbgstsave.Columns.Add("auxv3");
                _dtbgstsave.Columns.Add("auxi1");
                _dtbgstsave.Columns.Add("auxi2");
                _dtbgstsave.Columns.Add("auxi3");
                ViewState["DatosSave"] = _dtbgstsave;

                _dtbpreview.Columns.Add("Identificacion");
                _dtbpreview.Columns.Add("Cliente");
                _dtbpreview.Columns.Add("Provincia");
                _dtbpreview.Columns.Add("Ciudad");
                _dtbpreview.Columns.Add("CodigoCLDE");
                _dtbpreview.Columns.Add("CodigoPERS");
                _dtbpreview.Columns.Add("FechaGestion");
                ViewState["DatosPreview"] = _dtbpreview;

                if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
                {
                    Lbltitulo.Text = "Nueva Lista de Trabajo";
                    ViewState["Nuevo"] = "0";
                }
                else
                {
                    ViewState["Nuevo"] = "1";
                    pnlDatosListas.Visible = true;
                    DdlEstrategia.Enabled = false;
                    DdlCedente.Enabled = false;
                    DdlCatalogo.Enabled = false;
                    FunCargarMantenimiento();
                    Lbltitulo.Text = "Editar Lista de Trabajo";
                    lblEstado.Visible = true;
                    ChkEstado.Visible = true;
                    ImgPreview.Enabled = false;
                    TxtFechaInicio.Enabled = false;
                    DdlAsignacion.Enabled = false;
                    DdlCampania.Enabled = false;
                    ChkGestor.Enabled = false;
                    DdlGestores.Enabled = false;
                    DdlGestorApoyo.Enabled = false;
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
                DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                DdlCatalogo.DataTextField = "CatalogoProducto";
                DdlCatalogo.DataValueField = "CodigoCatalogo";
                DdlCatalogo.DataBind();
                DdlCatalogo.SelectedValue = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                DdlMarcado.SelectedValue = _dts.Tables[0].Rows[0]["Marcado"].ToString();
                ChkEstado.Text = _dts.Tables[0].Rows[0]["Estado"].ToString();
                ChkEstado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                DdlGestores.SelectedValue = _dts.Tables[0].Rows[0]["Gestor"].ToString();

                if (DdlGestores.SelectedValue != "0") ChkGestor.Checked = true;

                DdlAsignacion.Items.Clear();
                _asignacion.Text = _dts.Tables[0].Rows[0]["Asignacion"].ToString();
                _asignacion.Value = _dts.Tables[0].Rows[0]["Asignacion"].ToString();
                DdlAsignacion.Items.Add(_asignacion);
                DdlGestorApoyo.SelectedValue = _dts.Tables[0].Rows[0]["GestorApoyo"].ToString();
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

                    DdlMarcado.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO MARCADO", 
                        "--Seleccione Tipo Marcado--", "S");
                    DdlMarcado.DataTextField = "Descripcion";
                    DdlMarcado.DataValueField = "Codigo";
                    DdlMarcado.DataBind();

                    break;
                case 1:
                    GrdvPreview.DataSource = null;
                    GrdvPreview.DataBind();
                    LblTotal.InnerText = "0";

                    DdlGestores.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", 
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestores.DataTextField = "Descripcion";
                    DdlGestores.DataValueField = "Codigo";
                    DdlGestores.DataBind();

                    DdlGestorApoyo.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", 
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestorApoyo.DataTextField = "Descripcion";
                    DdlGestorApoyo.DataValueField = "Codigo";
                    DdlGestorApoyo.DataBind();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(81, int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "",
                        Session["Conectar"].ToString());
                    DdlCatalogo.DataSource = _dts;
                    DdlCatalogo.DataTextField = "Descripcion";
                    DdlCatalogo.DataValueField = "Codigo";
                    DdlCatalogo.DataBind();

                    break;
                case 2:
                    DdlAsignacion.DataSource = new ConsultaDatosDAO().FunConsultaDatos(91, int.Parse(DdlCatalogo.SelectedValue), 0, 0, "",
                        "", "", Session["Conectar"].ToString());
                    DdlAsignacion.DataTextField = "Descripcion";
                    DdlAsignacion.DataValueField = "Codigo";
                    DdlAsignacion.DataBind();

                    DdlCampania.DataSource = new ConsultaDatosDAO().FunConsultaDatos(119, int.Parse(DdlCatalogo.SelectedValue),
                        0, 0, "", "", "", Session["Conectar"].ToString());
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

            if (DdlEstrategia.SelectedValue == "0")
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Estrategia..!", this, "W", "C");
                _validar = false;
            }

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

            //if (int.Parse(DdlCatalogo.SelectedValue) == 0)
            //{
            //    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo/Producto..!", this, "W", "C");
            //    _validar = false;
            //}

            if (ChkGestor.Checked)
            {
                if (DdlGestores.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "C");
                    _validar = false;
                }
            }

            if (!new FuncionesDAO().IsDate(TxtFechaInicio.Text))
            {
                new FuncionesDAO().FunShowJSMessage("Formato Fecha Incorrecta..!", this, "E", "C");
                _validar = false;
            }

            if (!new FuncionesDAO().IsDate(TxtFechaFin.Text))
            {
                new FuncionesDAO().FunShowJSMessage("Formato Fecha Incorrecta..!", this, "E", "C");
                _validar = false;
            }

            _fechaactual = DateTime.Now.ToString("MM/dd/yyyy");
            _dtmfechainicio = DateTime.ParseExact(TxtFechaInicio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfechafin = DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfechaactual = DateTime.ParseExact(_fechaactual, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            if (_dtmfechafin < _dtmfechainicio)
            {
                new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser mayor a Fecha Fin..!", this, "E", "C");
                _validar = false;
            }

            if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
            {
                if (_dtmfechainicio < _dtmfechaactual)
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser menor a la Fecha Actual..!", this, "E", "C");
                    _validar = false;
                }
            }
            return _validar;
        }

        private string FunFormarSQL(string nuevoSQL, int tipo)
        {
            try
            {
                _continuar = true;
                _estrategia = "";
                _ordenar = "";
                _pasadas = 0;
                _sql = nuevoSQL;
                _sql += "FROM SoftCob_CLIENTE_DEUDOR CL (NOLOCK) INNER JOIN SoftCob_CUENTA_DEUDOR CD (NOLOCK) ON CL.CLDE_CODIGO=CD.CLDE_CODIGO ";
                _sql += "INNER JOIN SoftCob_PERSONA PE (NOLOCK) ON CL.PERS_CODIGO=PE.PERS_CODIGO ";
                _sql += "INNER JOIN SoftCob_PROVINCIA PR (NOLOCK) ON PE.pers_provincia=PR.PROV_CODIGO ";
                _sql += "INNER JOIN SoftCob_CIUDAD CI (NOLOCK) ON PE.pers_ciudad=CI.CIUD_CODIGO WHERE ";
                _sql += "CL.clde_estado=1 AND CD.ctde_estado=1 AND ";
                if (DdlCatalogo.SelectedValue != "0")
                    _sql += "CL.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " AND ";

                if (ChkGestor.Checked)
                {
                    if (DdlGestores.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "C");
                        _continuar = false;
                    }
                    else
                        _sql += "CL.clde_estado=1 AND CD.ctde_gestorasignado=" + DdlGestores.SelectedValue + " AND CD.ctde_estado=1 AND ";
                }
                else _sql += "CL.clde_estado=1 AND CD.ctde_gestorasignado>0 AND CD.ctde_estado=1 AND ";

                if (DdlAsignacion.SelectedValue != "0") _sql += "CD.ctde_auxv2='" + DdlAsignacion.SelectedValue + "' AND ";

                if (DdlCampania.SelectedValue != "0") _sql += "CD.ctde_auxi4=" + DdlCampania.SelectedValue + " AND ";

                if (_continuar)
                {
                    _dtbestrategia = (DataTable)ViewState["Estrategia"];
                    foreach (DataRow row in _dtbestrategia.Rows)
                    {
                        if (row["Operacion"].ToString() != "BETWEEN")
                            _estrategia += row["Campo"].ToString() + " " + row["Operacion"].ToString() + " " + row["Valor"].ToString() + " AND ";
                        else
                        {
                            if (_pasadas == 0) _estrategia += row["Campo"].ToString() + " " + row["Operacion"].ToString() + " " + row["Valor"].ToString() + " AND ";
                            else _estrategia += row["Valor"].ToString() + " AND ";
                        }

                        if (tipo == 1)
                        {
                            if (!string.IsNullOrEmpty(row[4].ToString()))
                            {
                                _ordenar += row[1].ToString() + " " + row[4].ToString() + ",";
                            }
                        }
                        _pasadas++;
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
        #endregion

        #region Botones y Eventos
        protected void DdlEstrategia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);
                ViewState["Estrategia"] = null;
                ViewState["CodigoEstrategia"] = DdlEstrategia.SelectedValue;
                _dts = new ConsultaDatosDAO().FunConsultaDatos(22, int.Parse(DdlEstrategia.SelectedValue), 0, 0, "", "", "", 
                    Session["Conectar"].ToString());
                ViewState["Estrategia"] = _dts.Tables[0];
                GrdvEstrategia.DataSource = _dts;
                GrdvEstrategia.DataBind();
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
                //_dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));

                //if (_dts.Tables[0].Rows.Count > 0)
                //{
                //    ViewState["CodigoCedente"] = DdlCedente.SelectedValue;
                //    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                //    DdlCatalogo.DataTextField = "CatalogoProducto";
                //    DdlCatalogo.DataValueField = "CodigoCatalogo";
                //    DdlCatalogo.DataBind();
                //    ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
                //    FunCargarCombos(2);
                //}
                //else
                //{
                //    DdlCatalogo.Items.Clear();
                //    _itemc.Text = "--Seleccione Catálago/Producto--";
                //    _itemc.Value = "0";
                //    DdlCatalogo.Items.Add(_itemc);
                //}
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
                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    _dtbestrategia = (DataTable)ViewState["Estrategia"];
                    _sql = "SELECT Cliente=PE.pers_nombrescompletos,Identificacion=PE.pers_numerodocumento,";
                    _sql += "CodigoCLDE=CD.CLDE_CODIGO,CodigoPERS=PE.PERS_CODIGO,CodigoGEST=CD.ctde_gestorasignado,Estado=1,";
                    _sql += "Provincia=prov_nombre,Ciudad=ciud_nombre,Estado=1,FechaGestion=CONVERT(VARCHAR(10),CD.ctde_auxv3,101),";
                    _sql += "auxv1='',auxv2='',auxv3='',auxi1=0,auxi2=0,auxi3=0 ";
                    _sql = FunFormarSQL(_sql, 1);

                    if (!string.IsNullOrEmpty(_sql))
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "",
                            Session["Conectar"].ToString());

                        columnas = new[] { "CodigoCLDE", "CodigoPERS", "CodigoGEST", "Estado", "auxv1", "auxv2", "auxv3",
                            "auxi1", "auxi2", "auxi3" };
                        view = new DataView(_dts.Tables[0]);
                        _dtb = view.ToTable(true, columnas);

                        ViewState["DatosSave"] = _dtb;

                        columnas = new[] { "Identificacion", "Cliente", "Provincia", "Ciudad", "FechaGestion" };
                        view = new DataView(_dts.Tables[0]);
                        _dtb = view.ToTable(true, columnas);

                        GrdvPreview.DataSource = _dtb;
                        GrdvPreview.DataBind();
                        ViewState["Preview"] = _dtb;

                        if (_dtb.Rows.Count > 0) TblLista.Visible = true;

                        LblTotal.InnerText = _dtb.Rows.Count.ToString();
                    }
                    else new FuncionesDAO().FunShowJSMessage("No se puede formar la consulta..!", this, "E", "C");
                }
                else new FuncionesDAO().FunShowJSMessage("Seleccione Datos para preview / Campos o Datos en Grid..!", this, "W", "C");
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
                        int.Parse(DdlCatalogo.SelectedValue), 0, "", TxtLista.Text.Trim().ToUpper(), TxtFechaInicio.Text.Trim(), 
                        Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Nombre de la Lista de Trabajo ya Existe..!", this, "E", "C");
                        _continuar = false;
                        return;
                    }

                    if (ChkGestor.Checked)
                    {
                        if (DdlGestores.SelectedValue == "0")
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
                            TxtDescripcion.Text.Trim().ToUpper(), TxtFechaInicio.Text, TxtFechaFin.Text,
                            int.Parse(DdlEstrategia.SelectedValue), int.Parse(DdlCedente.SelectedValue),
                            int.Parse(DdlCatalogo.SelectedValue), ChkEstado.Checked, DdlMarcado.SelectedValue,
                            DdlCampania.SelectedValue, 0, "", 0, 0, 0, "", "", DdlGestores.SelectedValue,
                            DdlAsignacion.SelectedValue, DdlGestorApoyo.SelectedValue, int.Parse(LblTotal.InnerText),
                            0, int.Parse(DdlGestorApoyo.SelectedValue), int.Parse(Session["usuCodigo"].ToString()),
                            Session["MachineName"].ToString(), _dtbgstsave,
                            (DataTable)ViewState["Estrategia"], "sp_NewListaTrabajo", Session["Conectar"].ToString());

                        if (int.Parse(ViewState["CodigoLista"].ToString()) == 0) _mensaje = "OK";
                    }
                    else
                    {
                        new FuncionesDAO().FunShowJSMessage("No existen datos para registrar..!", this, "E", "C");
                        return;
                    }

                    if (_mensaje == "OK")
                    {
                        if (ViewState["Regresar"].ToString() == "L") Response.Redirect("WFrm_ListaTrabajoAdmin.aspx?MensajeRetornado=Guardado con Éxito", true);

                        if (ViewState["Regresar"].ToString() == "M") Response.Redirect("..\\ReportesManager\\WFrm_MonitoreoLstAdmin.aspx", true);
                    }
                    else Lblerror.Text = _mensaje;
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

        protected void GrdvPreview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdvPreview.PageIndex = e.NewPageIndex;
            GrdvPreview.DataBind();
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["Preview"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    string FileName = "Preview_" + DdlCedente.SelectedItem.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
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
        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
            //FunCargarCombos(1);
            FunCargarCombos(2);
            FunCargarCombos(3);
        }
        protected void ChkGestor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);

                if (ChkGestor.Checked) DdlGestores.Enabled = true;
                else
                {
                    DdlGestores.Enabled = false;
                    DdlGestores.SelectedValue = "0";
                }
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
        protected void DdlGestores_SelectedIndexChanged(object sender, EventArgs e)
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
        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            if (ViewState["Regresar"].ToString() == "L") Response.Redirect("WFrm_ListaTrabajoAdmin.aspx", true);
            if (ViewState["Regresar"].ToString() == "M") Response.Redirect("..\\ReportesManager\\WFrm_MonitoreoLstAdmin.aspx", true);
        }
        #endregion
    }
}