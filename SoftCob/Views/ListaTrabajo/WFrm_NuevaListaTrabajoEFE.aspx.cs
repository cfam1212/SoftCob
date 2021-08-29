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
    public partial class WFrm_NuevaListaTrabajoEFE : Page
    {
        #region Variables
        ListItem _itemc = new ListItem();
        DataSet _dts = new DataSet();
        DataSet _dts1 = new DataSet();
        DataTable _dtbestrategia = new DataTable();
        DataTable _dtbefecto = new DataTable();
        DataTable _dtbcodigos = new DataTable();
        DataTable _dtbgstsave = new DataTable();
        DataTable _dtbpreview = new DataTable();
        DataTable _dtbgestion = new DataTable();
        DataTable _dtb = new DataTable();
        DataView view;
        DataRow _result, _filnew, _filagre;
        DataRow[] _resultado;
        string _sql = "", _estrategia = "", _ordenar = "", _fechaactual = "", _mensaje = "", _codigo = "",
            _sql1 = "", _efecto = "", _fechagestion = "";
        string[] _codigosopm, columnas;
        bool _validar = false, _continuar = true;
        int _codlistaarbol = 0, _pasadas = 0;
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

            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            scriptManager.RegisterPostBackControl(ImgExportar);

            if (!IsPostBack)
            {
                _dtbcodigos.Columns.Add("Codigo");
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
                    Lbltitulo.Text = "Nueva Lista de Trabajo << ARBOL EFECTO --COMPROMISOS DE PAGO-- >>";
                    ViewState["Nuevo"] = "0";
                }
                else
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(147, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());

                    _codigosopm = _dts.Tables[0].Rows[0]["Codigos"].ToString().Split(',');
                    _dtbcodigos = (DataTable)ViewState["CodigosOPM"];

                    foreach (var _datos in _codigosopm)
                    {
                        _result = _dtbcodigos.NewRow();
                        _result["Codigo"] = _datos;
                        _dtbcodigos.Rows.Add(_result);
                    }

                    ViewState["CodigosOPM"] = _dtbcodigos;
                    ViewState["Nuevo"] = "1";
                    PnlConfiguracion.Enabled = false;
                    PnlEfecto.Enabled = false;
                    PnlOpcionGestion.Enabled = false;
                    DdlEstrategia.Enabled = false;
                    DdlCedente.Enabled = false;
                    DdlCatalogo.Enabled = false;
                    FunCargarMantenimiento();
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
                ViewState["CodigoCPCE"] = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                FunCargarCombos(2);
                DdlGestorApoyo.SelectedValue = _dts.Tables[0].Rows[0]["GestorApoyo"].ToString();
                DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                DdlCatalogo.DataTextField = "CatalogoProducto";
                DdlCatalogo.DataValueField = "CodigoCatalogo";
                DdlCatalogo.DataBind();
                DdlCatalogo.SelectedValue = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                FunCargarCombos(2);
                DdlMarcado.SelectedValue = _dts.Tables[0].Rows[0]["Marcado"].ToString();
                DdlCampania.SelectedValue = _dts.Tables[0].Rows[0]["Campania"].ToString();
                ChkGestor.Checked = _dts.Tables[0].Rows[0]["Gestor"].ToString() != "0" ? true : false;
                DdlGestor.SelectedValue = _dts.Tables[0].Rows[0]["Gestor"].ToString();
                ChkFecha.Checked = _dts.Tables[0].Rows[0]["PorFecha"].ToString() == "1" ? true : false;
                TxtFechaDesde.Text = _dts.Tables[0].Rows[0]["FechaDesde"].ToString();
                TxtFechaHasta.Text = _dts.Tables[0].Rows[0]["FechaHasta"].ToString();
                ChkEstado.Text = _dts.Tables[0].Rows[0]["Estado"].ToString();
                ChkEstado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;

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
                    GrdvPreview.DataSource = null;
                    GrdvPreview.DataBind();
                    LblTotal.InnerText = "0";

                    DdlGestor.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", 
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestor.DataTextField = "Descripcion";
                    DdlGestor.DataValueField = "Codigo";
                    DdlGestor.DataBind();

                    DdlGestorApoyo.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", 
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestorApoyo.DataTextField = "Descripcion";
                    DdlGestorApoyo.DataValueField = "Codigo";
                    DdlGestorApoyo.DataBind();
                    break;
                case 2:
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

                    _dts1 = new ConsultaDatosDAO().FunConsultaDatos(146, int.Parse(DdlCatalogo.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    ViewState["ArbolEfecto"] = _dts1.Tables[0];
                    GrdvEfecto.DataSource = _dts1;
                    GrdvEfecto.DataBind();

                    if (_dts1.Tables[0].Rows.Count > 0) PnlEfecto.Visible = true;
                    else PnlEfecto.Visible = false;
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

            _dtmfecini = DateTime.ParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfecfin = DateTime.ParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            if (_dtmfechafin < _dtmfechainicio)
            {
                new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser mayor a Fecha Fin..!", this, "E", "C");
                _validar = false; ;
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
                    _validar = false; ;
                }

                _dtbestrategia = (DataTable)ViewState["ArbolEfecto"];
                _result = _dtbestrategia.Select("Selecc='SI'").FirstOrDefault();

                if (_result == null)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione al menos un Efecto..!", this, "W", "C");
                    _validar = false; ;
                }
            }

            return _validar;
        }
        private string FunCrearSQL(string sqlformado)
        {
            try
            {
                _efecto = "";
                _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                _resultado = _dtbefecto.Select("Selecc='SI'");

                foreach (DataRow _row in _resultado)
                {
                    _efecto += _row["Codigo"] + ",";
                }

                sqlformado += _efecto;
                //sqlformado = sqlformado.Remove(sqlformado.Length - 1) + ")" + " group by CDE.CLDE_CODIGO";
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
                _continuar = true; _estrategia = ""; _ordenar = "";
                _pasadas = 0;
                _sql1 = nuevoSQL;

                if (ChkGestor.Checked)
                {
                    if (DdlGestor.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "C");
                        _continuar = false;
                    }
                    else
                        _sql1 += "CLI.clde_estado=1 AND CDE.ctde_gestorasignado=" + DdlGestor.SelectedValue + " AND CDE.ctde_estado=1 AND ";
                }
                else _sql1 += "CLI.clde_estado=1 AND CDE.ctde_gestorasignado>0 AND CDE.ctde_estado=1 AND ";

                if (DdlAsignacion.SelectedValue != "0") _sql1 += "CDE.ctde_auxv2='" + DdlAsignacion.SelectedValue + "' AND ";

                if (DdlCampania.SelectedValue != "0") _sql1 += "CDE.ctde_auxi4=" + DdlCampania.SelectedValue + " AND ";

                if (DdlAsignacion.SelectedValue != "0") _sql1 += "CDE.ctde_auxv2='" + DdlAsignacion.SelectedValue + "' AND ";

                if (DdlCampania.SelectedValue != "0") _sql1 += "CDE.ctde_auxi4=" + DdlCampania.SelectedValue + " AND ";

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
                GrdvEstrategia.DataSource = null;
                GrdvEstrategia.DataBind();
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

        protected void ChkGestor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(3);

                if (ChkGestor.Checked) DdlGestor.Enabled = true;
                else
                {
                    DdlGestor.Enabled = false;
                    DdlGestor.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvEfecto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                _dtbcodigos = (DataTable)ViewState["CodigosOPM"];

                if (e.Row.RowIndex >= 0)
                {
                    _chkselecc = (CheckBox)(e.Row.Cells[1].FindControl("ChkSelecc"));
                    _codigo = GrdvEfecto.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString();
                    _result = _dtbcodigos.Select("Codigo='" + _codigo + "'").FirstOrDefault();

                    if (_result != null) _chkselecc.Checked = true;
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
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkselected = (CheckBox)(_gvrow.Cells[1].FindControl("ChkSelecc"));
                _codigo = GrdvEfecto.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString();
                _dtbefecto = (DataTable)ViewState["ArbolEfecto"];
                _result = _dtbefecto.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Selecc"] = _chkselected.Checked ? "SI" : "NO";
                _dtbefecto.AcceptChanges();
                ViewState["ArbolEfecto"] = _dtbefecto;
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
                    _sql1 += "Gestorasignado=CDE.ctde_gestorasignado,";
                    _sql1 += "Cliente=PER.pers_nombrescompletos,Identificacion=PER.pers_numerodocumento,";
                    _sql1 += "Provincia=(SELECT PRV.prov_nombre FROM SoftCob_Provincia PRV (NOLOCK) WHERE PRV.PROV_CODIGO=PER.pers_provincia),";
                    _sql1 += "Ciudad=(SELECT CIU.ciud_nombre FROM SoftCob_Ciudad CIU (NOLOCK) WHERE CIU.CIUD_CODIGO=PER.pers_ciudad) ";
                    _sql1 += "FROM SoftCob_CUENTA_DEUDOR CDE (NOLOCK) INNER JOIN SoftCob_CLIENTE_DEUDOR CLI (NOLOCK) ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                    _sql1 += "INNER JOIN SoftCob_PERSONA PER (NOLOCK) ON CLI.PERS_CODIGO=PER.PERS_CODIGO ";
                    _sql1 += "WHERE CLI.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " AND CDE.ctde_estado=1 AND CLI.clde_estado=1 AND ";

                    _sql1 = FunFormarSQL(_sql1, 0);

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql1, "", "",
                        Session["Conectar"].ToString());

                    columnas = new[] { "Identificacion", "Cliente", "CodigoCLDE", "CodigoPERS", "Provincia", "Ciudad",
                        "Gestorasignado" };

                    view = new DataView(_dts.Tables[0]);
                    _dtb = view.ToTable(true, columnas);

                    if (_dtb.Rows.Count == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("No Existen Datos para Crear Lista..!", this, "E", "C");
                        return;
                    }

                    _sql1 = "";
                    _sql1 = "SELECT DISTINCT CodigoCLDE=GTE.gete_cldecodigo,FechaGestion=CONVERT(VARCHAR(10),GTE.gete_fechagestion,121) ";
                    _sql1 += "FROM SoftCob_GESTION_TELEFONICA GTE (nolock) INNER JOIN SoftCob_CUENTA_DEUDOR CDE ON GTE.gete_cldecodigo=CDE.CLDE_CODIGO ";
                    _sql1 += "WHERE CONVERT(date,GTE.gete_fechagestion,101) BETWEEN CONVERT(DATE,'" + TxtFechaDesde.Text.Trim() + "',101) ";
                    _sql1 += "AND CONVERT(DATE,'" + TxtFechaHasta.Text.Trim() + "',101) AND ";
                    _sql1 += "GTE.gete_cedecodigo=" + DdlCedente.SelectedValue + " AND GTE.gete_cpcecodigo=" + DdlCatalogo.SelectedValue + " AND ";
                    _sql1 += "GTE.gete_auxi3=0 AND GTE.gete_arefcodigo IN(";
                    _sql1 = FunCrearSQL(_sql1);

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
                            _result = _dtbgestion.Select("CodigoCLDE='" + _drfila["CodigoCLDE"].ToString() + "'").
                                FirstOrDefault();

                            if (_result != null && _resultado.Length > 0)
                            {
                                _fechagestion = _result["FechaGestion"].ToString();

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

                    TblLista.Visible = true;
                    ViewState["Preview"] = _dtbpreview;

                    GrdvPreview.DataSource = _dtbpreview;
                    GrdvPreview.DataBind();
                    LblTotal.InnerText = _dtbpreview.Rows.Count.ToString();
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
                    string FileName = "PreviewApy_" + DdlCedente.SelectedItem.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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
                            int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), ChkEstado.Checked, DdlMarcado.SelectedValue, DdlCampania.SelectedValue,
                            0, "", 0, 0, 0, TxtFechaDesde.Text.Trim(), TxtFechaHasta.Text.Trim(), DdlGestor.SelectedValue,
                            DdlAsignacion.SelectedValue, DdlGestorApoyo.SelectedValue, int.Parse(LblTotal.InnerText),
                            3, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _dtbgstsave,
                            (DataTable)ViewState["Estrategia"], "sp_NewListaTrabajo", Session["Conectar"].ToString());

                        if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
                        {
                            _sql = FunCrearSQL("");
                            _efecto = _efecto.Remove(_efecto.Length - 1);
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(148, int.Parse(_mensaje), 0, 0, "",
                                _efecto, "", Session["Conectar"].ToString());
                            _mensaje = _dts.Tables[0].Rows[0][0].ToString();
                        }
                        if (_mensaje == "OK") Response.Redirect("WFrm_ListaTrabajoAdminEFE.aspx?MensajeRetornado=Guardado con Éxito", true);
                        else new FuncionesDAO().FunShowJSMessage(_mensaje, this);
                    }
                    else
                    {
                        new FuncionesDAO().FunShowJSMessage("No existen datos para registrar..!", this, "E", "C");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ListaTrabajoAdminEFE.aspx", true);
        }
        #endregion
    }
}