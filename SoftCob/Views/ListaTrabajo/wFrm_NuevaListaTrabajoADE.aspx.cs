namespace SoftCob.Views.ListaTrabajo
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevaListaTrabajoADE : Page
    {
        #region Variables
        ListItem _itemc = new ListItem();
        DataSet _dts = new DataSet();
        DataTable _dtbestrategia = new DataTable();
        DataTable _dtbgstsave = new DataTable();
        DataTable _dtbpreview = new DataTable();
        DataTable _dtbgestion = new DataTable();
        DataTable _dtb = new DataTable();
        DataView view;
        string _estrategia = "", _ordenar = "", _fechaactual = "", _mensaje = "", _sql1 = "", _fechagestion = "", _filename = "";
        bool _validar = false, _continuar = true;
        int _codlistaarbol = 0, _pasadas = 0, _efectivo = 0;
        string[] columnas;
        DateTime _dtmfechainicio, _dtmfechafin, _dtmfechaactual, _dtmfecini, _dtmfecfin;
        CheckBox _chkselected = new CheckBox();
        ListItem _accion = new ListItem();
        ListItem _asignacion = new ListItem();
        ListItem _campania = new ListItem();
        DataRow _filagre, _filnew;
        DataRow[] _resultado;
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
                ViewState["CodigoLista"] = Request["CodigoLista"];
                ViewState["Regresar"] = Request["Regresar"];
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
                    Lbltitulo.Text = "Nueva Lista de Trabajo << POR ARBOL DE DECISION - ESTADO GESTION (EFECTIVA - NO EFECTIVA) >>";
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
                    pnlConfiguracion.Enabled = false;
                    pnlOpcionGestion.Enabled = false;
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(23, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", 
                    Session["Conectar"].ToString());
                TxtLista.Text = _dts.Tables[0].Rows[0]["ListaTrabajo"].ToString();
                TxtDescripcion.Text = _dts.Tables[0].Rows[0]["Descripcion"].ToString();
                TxtFechaInicio.Text = _dts.Tables[0].Rows[0]["FechaInicio"].ToString();
                TxtFechaFin.Text = _dts.Tables[0].Rows[0]["FechaFin"].ToString();
                DdlEstrategia.SelectedValue = _dts.Tables[0].Rows[0]["Codigoesca"].ToString();
                DdlCedente.SelectedValue = _dts.Tables[0].Rows[0]["Codigocedente"].ToString();
                FunCargarCombos(1);
                ViewState["CodigoCPCE"] = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                FunCargarCombos(2);
                DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                DdlCatalogo.DataTextField = "CatalogoProducto";
                DdlCatalogo.DataValueField = "CodigoCatalogo";
                DdlCatalogo.DataBind();
                DdlCatalogo.SelectedValue = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                DdlMarcado.SelectedValue = _dts.Tables[0].Rows[0]["Marcado"].ToString();
                DdlAsignacion.Items.Clear();
                _asignacion.Text = _dts.Tables[0].Rows[0]["Asignacion"].ToString();
                _asignacion.Value = _dts.Tables[0].Rows[0]["Asignacion"].ToString();
                DdlAsignacion.Items.Add(_asignacion);
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
                DdlGestores.SelectedValue = _dts.Tables[0].Rows[0]["Gestor"].ToString();
                if (DdlGestores.SelectedValue != "0") ChkGestor.Checked = true;
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

                    DdlMarcado.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO MARCADO", "--Seleccione Tipo Marcado--", "S");
                    DdlMarcado.DataTextField = "Descripcion";
                    DdlMarcado.DataValueField = "Codigo";
                    DdlMarcado.DataBind();

                    break;
                case 1:
                    GrdvListas.DataSource = null;
                    GrdvListas.DataBind();
                    GrdvPreview.DataSource = null;
                    GrdvPreview.DataBind();
                    LblTotal.InnerText = "0";

                    DdlAccion.Items.Clear();
                    _accion.Text = "--Seleccione Acción--";
                    _accion.Value = "0";
                    DdlAccion.Items.Add(_accion);

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

                    break;
                case 2:
                    DdlAccion.DataSource = new SpeechDAO().FunGetArbolNewAccion(int.Parse(ViewState["CodigoCPCE"].ToString()));
                    DdlAccion.DataTextField = "Descripcion";
                    DdlAccion.DataValueField = "Codigo";
                    DdlAccion.DataBind();

                    DdlAsignacion.DataSource = new ConsultaDatosDAO().FunConsultaDatos(91, int.Parse(ViewState["CodigoCPCE"].ToString()), 0, 0, "",
                        "", "", Session["Conectar"].ToString());
                    DdlAsignacion.DataTextField = "Descripcion";
                    DdlAsignacion.DataValueField = "Codigo";
                    DdlAsignacion.DataBind();

                    DdlCampania.DataSource = new ConsultaDatosDAO().FunConsultaDatos(119, int.Parse(ViewState["CodigoCPCE"].ToString()), 0, 0, "",
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

            _dtmfecini = DateTime.ParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfecfin = DateTime.ParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);


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
            }

            TimeSpan _diferencia = _dtmfecfin.Subtract(_dtmfecini);

            if (_diferencia.Days > 31)
            {
                new FuncionesDAO().FunShowJSMessage("Definir la consulta son en rangos de 30 a 31 dias..!", this);
                _validar = false;
            }

            if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
            {
                if (ChkGestor.Checked)
                {
                    if (DdlGestores.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Elija Gestor..!", this);
                        _validar = false; ;
                    }
                }

                if (ChkGestion.Checked == false && ChkArbol.Checked == false && ChkFecha.Checked == false)
                {
                    new FuncionesDAO().FunShowJSMessage("Elija de Opción Gestión (Gestión - Árbol)", this);
                    _validar = false; ;
                }

                if (ChkGestion.Checked)
                {
                    if (DdlTipoGestion.SelectedValue == "-1")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Gestión..!", this);
                        _validar = false; ;
                    }
                }

                if (ChkArbol.Checked)
                {
                    if (DdlAccion.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Opción de Árbol..!", this);
                        _validar = false; ;
                    }
                }

                if (ChkFecha.Checked)
                {
                    if (!new FuncionesDAO().IsDate(TxtFechaDesde.Text))
                    {
                        new FuncionesDAO().FunShowJSMessage("Formato Fecha Incorrecta..!", this);
                        _validar = false;
                    }
                    if (!new FuncionesDAO().IsDate(TxtFechaHasta.Text))
                    {
                        new FuncionesDAO().FunShowJSMessage("Formato Fecha Incorrecta..!", this);
                        _validar = false;
                    }
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
                _sql1 = nuevoSQL;

                if (DdlAsignacion.SelectedValue != "0") _sql1 += "CDE.ctde_auxv2='" + DdlAsignacion.SelectedValue + "' AND ";
                if (DdlCampania.SelectedValue != "0") _sql1 += "CDE.ctde_auxi4=" + DdlCampania.SelectedValue + " AND ";

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
                        _pasadas++;
                    }

                    if (tipo == 1)
                    {
                        if (!string.IsNullOrEmpty(_ordenar)) _ordenar = "ORDER BY " + _ordenar.Remove(_ordenar.Length - 1);
                    }

                    _sql1 += _estrategia;
                    _sql1 = _sql1.Remove(_sql1.Length - 4) + " " + _ordenar;
                }
                else _sql1 = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _sql1;
        }
        #endregion

        #region Botones y Eventos
        protected void DdlEstrategia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);
                ViewState["CodigoEstrategia"] = DdlEstrategia.SelectedValue;

                if (int.Parse(DdlEstrategia.SelectedValue) > 0)
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(22, int.Parse(DdlEstrategia.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
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
                FunCargarCombos(3);
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
                ViewState["CodigoCPCE"] = DdlCatalogo.SelectedValue;
                FunCargarCombos(1);
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

        protected void ChkGestor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                LblTotal.InnerText = "0";
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

        protected void ChkGestorSelec_CheckedChanged(object sender, EventArgs e)
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
                    _filename = "PreviewExt_" + DdlCedente.SelectedItem.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void ChkSelecc_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkselected = (CheckBox)(gvRow.Cells[4].FindControl("chkSelecc"));
                ViewState["Check"] = _chkselected.Checked;
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                LblTotal.InnerText = "0";
                foreach (GridViewRow fr in GrdvListas.Rows)
                {
                    _chkselected = (CheckBox)(gvRow.Cells[4].FindControl("chkSelecc"));
                    _chkselected.Checked = false;
                }
                _chkselected = (CheckBox)(gvRow.Cells[4].FindControl("chkSelecc"));
                _chkselected.Checked = (bool)ViewState["Check"];

                if (_chkselected.Checked)
                    ViewState["CodigoLTCA"] = GrdvListas.DataKeys[gvRow.RowIndex].Values["CodigoLSTA"].ToString();
                else ViewState["CodigoLTCA"] = "0";
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
                _sql1 = "";

                TblLista.Visible = false;

                if (DdlEstrategia.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("No existe Estrategia Seleccionada..!", this);
                    return;
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    _sql1 = "SELECT CodigoCLDE=CDE.CLDE_CODIGO,CodigoPERS=PER.PERS_CODIGO,";
                    _sql1 += "Gestorasignado=CDE.ctde_gestorasignado,Estado=1,FechaGestion=CONVERT(DATE,CDE.ctde_auxv3,121),";
                    _sql1 += "Cliente=PER.pers_nombrescompletos,Identificacion=PER.pers_numerodocumento,";
                    _sql1 += "Provincia=(SELECT PRV.prov_nombre FROM SoftCob_Provincia PRV (NOLOCK) WHERE PRV.PROV_CODIGO=PER.pers_provincia),";
                    _sql1 += "Ciudad=(SELECT CIU.ciud_nombre FROM SoftCob_Ciudad CIU (NOLOCK) WHERE CIU.CIUD_CODIGO=PER.pers_ciudad) ";
                    _sql1 += "FROM SoftCob_CUENTA_DEUDOR CDE (NOLOCK) INNER JOIN SoftCob_CLIENTE_DEUDOR CLI (NOLOCK) ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                    _sql1 += "INNER JOIN SoftCob_PERSONA PER (NOLOCK) ON CLI.PERS_CODIGO=PER.PERS_CODIGO ";
                    _sql1 += "WHERE CLI.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " AND CDE.ctde_estado=1 AND CLI.clde_estado=1 AND ";
                    if (ChkGestor.Checked) _sql1 += "CDE.ctde_gestorasignado=" + DdlGestores.SelectedValue + " AND ";
                    else _sql1 += "CDE.ctde_gestorasignado>0  AND ";

                    _sql1 = FunFormarSQL(_sql1, 0);

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql1, "", "", Session["Conectar"].ToString());

                    columnas = new[] { "Identificacion", "Cliente", "CodigoCLDE", "CodigoPERS", "Provincia", "Ciudad",
                        "Gestorasignado" };

                    view = new DataView(_dts.Tables[0]);
                    _dtb = view.ToTable(true, columnas);

                    _sql1 = "";
                    _sql1 = "SELECT CodigoCLDE = GTE.gete_cldecodigo,Efectivo = CAST(CASE GTE.gete_efectivo WHEN 1 THEN 1 ELSE 0 END AS varchar),";
                    _sql1 += "FechaGestion = CONVERT(VARCHAR(10),GTE.gete_fechagestion,121) ";
                    _sql1 += "FROM SoftCob_GESTION_TELEFONICA GTE (NOLOCK) ";
                    _sql1 += "INNER JOIN SoftCob_CUENTA_DEUDOR CDE ON CDE.ctde_operacion=GTE.gete_operacion ";
                    _sql1 += "WHERE GTE.gete_cpcecodigo=" + DdlCatalogo.SelectedValue + " AND ";
                    if (ChkGestorSelec.Checked)
                        _sql1 += "GTE.gete_gestorasignado=" + DdlGestores.SelectedValue + " AND ";
                    _sql1 += "CONVERT(DATE,GTE.gete_fechagestion,101) BETWEEN ";
                    _sql1 += "CONVERT(DATE,'" + TxtFechaDesde.Text + "',101) AND CONVERT(DATE,'" + TxtFechaHasta.Text + "',101) AND ";
                    _sql1 += "GTE.gete_auxi3=0 AND CDE.ctde_estado=1 ";
                    if (ChkArbol.Checked) _sql1 += "AND GTE.gete_araccodigo=" + DdlAccion.SelectedValue;
                    _sql1 += " ORDER BY GTE.gete_cldecodigo";

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql1, "", "",
                        Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        _dtbgestion = _dts.Tables[0];

                        _dtbpreview = (DataTable)ViewState["DatosPreview"];
                        _dtbpreview.Clear();

                        _dtbgstsave = (DataTable)ViewState["DatosSave"];
                        _dtbgstsave.Clear();

                        foreach (DataRow _drfila in _dtb.Rows)
                        {
                            _resultado = _dtbgestion.Select("CodigoCLDE='" + _drfila["CodigoCLDE"].ToString() + "'");

                            _fechagestion = Convert.ToString(_resultado.AsEnumerable()
                                                    .Max(row => row["FechaGestion"]));

                            if (_resultado != null && _resultado.Length > 0)
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
                                            _filnew["Gestorasignado"] = _drfila["Gestorasignado"].ToString();
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
                                            _filnew["Gestorasignado"] = _drfila["Gestorasignado"].ToString();
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
                                    _filnew["Gestorasignado"] = _drfila["Gestorasignado"].ToString();
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
                        
                        ViewState["Preview"] = _dtbpreview;

                        if(_dtbpreview.Rows.Count>0) TblLista.Visible = true;

                        GrdvPreview.DataSource = _dtbpreview;
                        GrdvPreview.DataBind();
                        LblTotal.InnerText = _dtbpreview.Rows.Count.ToString();
                    }
                    else new FuncionesDAO().FunShowJSMessage("No Existen Datos Para Crear Lista..!", this);
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
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre de la Lista de Trabajo..!", this);
                    return;
                }

                if (DdlMarcado.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Marcado..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(94, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(DdlCatalogo.SelectedValue), 0, "", TxtLista.Text.Trim().ToUpper(),
                    TxtFechaInicio.Text.Trim(), Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Nombre de la Lista de Trabajo ya Existe..!", this);
                    _continuar = false;
                    return;
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {

                    if (int.Parse(LblTotal.InnerText) > 0)
                    {
                        _dtbgstsave = (DataTable)ViewState["DatosSave"];
                        _codlistaarbol = int.Parse(ViewState["CodigoLista"].ToString());

                        _mensaje = new EstrategiaDAO().FunCrearListaTrabajo(_codlistaarbol, TxtLista.Text.Trim().ToUpper(),
                            TxtDescripcion.Text.Trim().ToUpper(), TxtFechaInicio.Text, TxtFechaFin.Text, int.Parse(DdlEstrategia.SelectedValue), int.Parse(DdlCedente.SelectedValue),
                            int.Parse(DdlCatalogo.SelectedValue), ChkEstado.Checked, DdlMarcado.SelectedValue, DdlCampania.SelectedValue,
                            ChkGestion.Checked ? 1 : 0, DdlTipoGestion.SelectedValue, ChkArbol.Checked ? 1 : 0, int.Parse(DdlAccion.SelectedValue),
                            ChkFecha.Checked ? 1 : 0, TxtFechaDesde.Text.Trim(), TxtFechaHasta.Text.Trim(), DdlGestores.SelectedValue,
                            DdlAsignacion.SelectedValue, DdlGestorApoyo.SelectedValue, int.Parse(LblTotal.InnerText),
                            1, int.Parse(DdlGestorApoyo.SelectedValue), int.Parse(Session["usuCodigo"].ToString()),
                            Session["MachineName"].ToString(), _dtbgstsave, (DataTable)ViewState["Estrategia"],
                            "sp_NewListaTrabajo", Session["Conectar"].ToString());

                        if (int.Parse(ViewState["CodigoLista"].ToString()) == 0) _mensaje = "OK";
                    }
                    else
                    {
                        new FuncionesDAO().FunShowJSMessage("No existen datos para registrar..!", this);
                        return;
                    }

                    if (_mensaje == "OK")
                    {
                        if (ViewState["Regresar"].ToString() == "L") Response.Redirect("WFrm_ListaTrabajoAdminADE.aspx?MensajeRetornado=Guardado con Éxito", true);
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

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            if (ViewState["Regresar"].ToString() == "L") Response.Redirect("WFrm_ListaTrabajoAdminADE.aspx", true);
            if (ViewState["Regresar"].ToString() == "M") Response.Redirect("..\\ReportesManager\\WFrm_MonitoreoLstAdmin.aspx", true);
        }
        #endregion
    }
}