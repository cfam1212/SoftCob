

namespace SoftCob.Views.ListaTrabajo
{
   
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Drawing;
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
        DataTable _dtblistatrabajo = new DataTable();
        DataTable _dtbefecto = new DataTable();
        DataTable _dtbcodigos = new DataTable();
        DataTable _dtbgstsave = new DataTable();
        DataRow _result;
        DataRow[] _resultado;
        string _sql = "", _estrategia = "", _ordenar = "", _fechaactual = "", _mensaje = "", _codigo = "",
            _sql1 = "", _efecto = "", _sql2 = "";
        string[] _codigosopm;
        bool _validar = false, _continuar = true;
        int _tipogestion = 0, _codlistaarbol = 0, _pasadas = 0;
        DateTime _dtmfechainicio, _dtmfechafin, _dtmfechaactual;
        CheckBox _chkselected = new CheckBox();
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

            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            scriptManager.RegisterPostBackControl(ImgExportar);

            if (!IsPostBack)
            {
                if (Session["IN-CALL"].ToString() == "SI")
                {
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                _dtbcodigos.Columns.Add("Codigo");
                ViewState["CodigosOPM"] = _dtbcodigos;
                _dtbgstsave.Columns.Add("CodigoCLDE");
                _dtbgstsave.Columns.Add("codigoPERS");
                _dtbgstsave.Columns.Add("gestorasignado");
                _dtbgstsave.Columns.Add("estado");
                _dtbgstsave.Columns.Add("operacion");
                ViewState["DatosSave"] = _dtbgstsave;
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
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
                    Lbltitulo.Text = "Nueva Lista de Trabajo << ARBOL EFECTO --COMPROMISOS DE PAGO-- >>";
                    ViewState["Nuevo"] = "0";
                }
                else
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(147, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", ViewState["Conectar"].ToString());

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
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(23, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", ViewState["Conectar"].ToString());

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
                DdlCatalogo.DataSource = new CedenteDTO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
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

                _dts = new ConsultaDatosDAO().FunConsultaDatos(26, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", ViewState["Conectar"].ToString());

                GrdvEstrategia.DataSource = _dts;
                GrdvEstrategia.DataBind();
                ViewState["Estrategia"] = _dts.Tables[0];
                ViewState["Preview"] = true;
                ImgPreview.ImageUrl = "~/Botones/Buscargris.png";
                ImgPreview.Enabled = false;
                LblPreview.Visible = false;
                pnlPreview.Visible = false;

                _dts = new ConsultaDatosDAO().FunConsultaDatos(25, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", ViewState["Conectar"].ToString());

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
                    DdlEstrategia.DataSource = new CatalogosDTO().FunGetEstrategiaCab();
                    DdlEstrategia.DataTextField = "Descripcion";
                    DdlEstrategia.DataValueField = "Codigo";
                    DdlEstrategia.DataBind();

                    DdlCedente.DataSource = new CatalogosDTO().FunGetCedentes();
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

                    DdlMarcado.DataSource = new CatalogosDTO().FunGetParametroDetalleValor("TIPO MARCADO", "--Seleccione Tipo Marcado--");
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

                    DdlGestor.DataSource = new CatalogosDTO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", ViewState["Conectar"].ToString());
                    DdlGestor.DataTextField = "Descripcion";
                    DdlGestor.DataValueField = "Codigo";
                    DdlGestor.DataBind();

                    DdlGestorApoyo.DataSource = new CatalogosDTO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", ViewState["Conectar"].ToString());
                    DdlGestorApoyo.DataTextField = "Descripcion";
                    DdlGestorApoyo.DataValueField = "Codigo";
                    DdlGestorApoyo.DataBind();
                    break;
                case 2:
                    DdlAsignacion.DataSource = new ConsultaDatosDAO().FunConsultaDatos(91, int.Parse(DdlCatalogo.SelectedValue), 0, 0, "",
                        "", "", ViewState["Conectar"].ToString());
                    DdlAsignacion.DataTextField = "Descripcion";
                    DdlAsignacion.DataValueField = "Codigo";
                    DdlAsignacion.DataBind();

                    DdlCampania.DataSource = new ConsultaDatosDAO().FunConsultaDatos(119, int.Parse(DdlCatalogo.SelectedValue), 0, 0, "",
                        "", "", ViewState["Conectar"].ToString());
                    DdlCampania.DataTextField = "Descripcion";
                    DdlCampania.DataValueField = "Codigo";
                    DdlCampania.DataBind();

                    _dts1 = new ConsultaDatosDAO().FunConsultaDatos(146, int.Parse(DdlCatalogo.SelectedValue), 0, 0, "", "", "", ViewState["Conectar"].ToString());
                    ViewState["ArbolEfecto"] = _dts1.Tables[0];
                    GrdvEfecto.DataSource = _dts1;
                    GrdvEfecto.DataBind();

                    if (_dts1.Tables[0].Rows.Count > 0) PnlEfecto.Visible = true;
                    else PnlEfecto.Visible = false;
                    break;
            }
        }

        private bool FunValidarCampos()
        {
            _validar = true;

            if (string.IsNullOrEmpty(TxtLista.Text.Trim()))
            {
                new FuncionesBAS().FunShowJSMessage("Ingrese nombre de la Lista de Trabajo..!", this);
                _validar = false;
            }

            if (int.Parse(DdlCedente.SelectedValue) == 0)
            {
                new FuncionesBAS().FunShowJSMessage("Seleccione Cedente..!", this);
                _validar = false;
            }

            if (int.Parse(DdlCatalogo.SelectedValue) == 0)
            {
                new FuncionesBAS().FunShowJSMessage("Seleccione Catálogo/Producto..!", this);
                _validar = false;
            }

            if (!new FuncionesBAS().IsDate(TxtFechaInicio.Text))
            {
                new FuncionesBAS().FunShowJSMessage("Formato Fecha Incorrecta..!", this);
                _validar = false;
            }

            if (!new FuncionesBAS().IsDate(TxtFechaFin.Text))
            {
                new FuncionesBAS().FunShowJSMessage("Formato Fecha Incorrecta..!", this);
                _validar = false;
            }

            _fechaactual = DateTime.Now.ToString("MM/dd/yyyy");
            _dtmfechainicio = DateTime.ParseExact(TxtFechaInicio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfechafin = DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _dtmfechaactual = DateTime.ParseExact(_fechaactual, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            if (_dtmfechafin < _dtmfechainicio)
            {
                new FuncionesBAS().FunShowJSMessage("Fecha Inicio no puede ser mayor a Fecha Fin..!", this);
                _validar = false; ;
            }

            if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
            {
                if (_dtmfechainicio < _dtmfechaactual)
                {
                    new FuncionesBAS().FunShowJSMessage("Fecha Inicio no puede ser menor a la Fecha Actual..!", this);
                    _validar = false; ;
                }

                _dtbestrategia = (DataTable)ViewState["ArbolEfecto"];
                _result = _dtbestrategia.Select("Selecc='SI'").FirstOrDefault();

                if (_result == null)
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione al menos un Efecto..!", this);
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
                _sql = nuevoSQL;
                _sql += "From GSBPO_CLIENTE_DEUDOR CL (nolock) INNER JOIN GSBPO_CUENTA_DEUDOR CD (nolock) ON CL.CLDE_CODIGO=CD.CLDE_CODIGO ";
                _sql += "INNER JOIN GSBPO_PERSONA PE (nolock) ON CL.PERS_CODIGO=PE.PERS_CODIGO ";
                _sql += "INNER JOIN GSBPO_Provincia PR ON PE.pers_provincia=PR.PROV_COD ";
                _sql += "INNER JOIN GSBPO_Ciudad CI ON PE.pers_ciudad=CI.CIUD_COD where ";
                _sql += "CL.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CL.clde_estado=1 and CD.ctde_estado=1 and ";

                if (ChkGestor.Checked)
                {
                    if (DdlGestor.SelectedValue == "0")
                    {
                        new FuncionesBAS().FunShowJSMessage("Seleccione Gestor..!", this);
                        _continuar = false;
                    }
                    else
                        _sql += "CL.clde_estado=1 and CD.ctde_gestorasignado=" + DdlGestor.SelectedValue + " and CD.ctde_estado=1 and ";
                }
                else _sql += "CL.clde_estado=1 and CD.ctde_gestorasignado>0 and CD.ctde_estado=1 and ";

                if (DdlAsignacion.SelectedValue != "0") _sql += "CD.ctde_auxv2='" + DdlAsignacion.SelectedValue + "' and ";

                if (DdlCampania.SelectedValue != "0") _sql += "CD.ctde_auxi4=" + DdlCampania.SelectedValue + " and ";

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
                    _dts = new ConsultaDatosDTO().FunConsultaDatos(22, int.Parse(DdlEstrategia.SelectedValue), 0, 0, "", "", "", ViewState["Conectar"].ToString());
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
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
                LblTotal.InnerText = "0";

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
                _dts = new CedenteDTO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoCEDE"] = DdlCedente.SelectedValue;
                    DdlCatalogo.DataSource = new CedenteDTO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
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
                    new FuncionesBAS().FunShowJSMessage("No existe Estrategia Seleccionada..!", this);
                    return;
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    if (ChkFecha.Checked) _tipogestion = 4;
                    else _tipogestion = 5;

                    _sql = "select distinct Cliente = PE.pers_nombrescompletos,Identificacion = PE.pers_numerodocumento,codigoCLDE = CD.CLDE_CODIGO,";
                    _sql += "Operacion = CD.ctde_operacion, DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible,";
                    _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre ";
                    _sql = FunFormarSQL(_sql, 1);

                    if (!string.IsNullOrEmpty(_sql))
                    {
                        _sql1 = "select CodigoCLDE = CDE.CLDE_CODIGO from GSBPO_CUENTA_DEUDOR CDE ";
                        _sql1 += "INNER JOIN GSBPO_CLIENTE_DEUDOR CLI ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                        _sql1 += " where CLI.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CDE.ctde_estado=1 and ";

                        if (ChkGestor.Checked)
                            _sql1 += "CDE.ctde_gestorasignado=" + DdlGestor.SelectedValue + " and ";
                        else _sql1 += "CDE.ctde_gestorasignado>0 and ";

                        _sql1 = _sql1.Remove(_sql1.Length - 4);
                        _sql1 = _sql1 + " group by CDE.CLDE_CODIGO";

                        _sql2 = "select gete_cldecodigo,gete_arefcodigo,gete_efectivo,";
                        _sql2 += "gete_fechagestion,gete_fechacreacion,gete_auxi1 ";
                        _sql2 += "from GSBPO_GESTION_TELEFONICA GTE (nolock) INNER JOIN GSBPO_CUENTA_DEUDOR CDE ON GTE.gete_cldecodigo=CDE.CLDE_CODIGO ";
                        _sql2 += "where convert(date,GTE.gete_fechagestion,101) between CONVERT(date,'" + TxtFechaDesde.Text.Trim() + "',101) ";
                        _sql2 += "and CONVERT(date,'" + TxtFechaHasta.Text.Trim() + "',101) and ";
                        _sql2 += "gete_cedecodigo=" + DdlCedente.SelectedValue + " and gete_cpcecodigo=" + DdlCatalogo.SelectedValue + " and ";
                        _sql2 += "gete_auxi3=0 and gete_arefcodigo in(";
                        _sql2 = FunCrearSQL(_sql2);
                        _sql2 = _sql2 + " order by gete_fechacreacion desc";

                        _dtbgstsave = (DataTable)ViewState["DatosSave"];

                        _dts = new ListaTrabajoDAO().FunNewLstADE(0, _sql, int.Parse(ViewState["CodigoCEDE"].ToString()),
                            int.Parse(ViewState["CodigoCPCE"].ToString()), int.Parse(ViewState["CodigoLTCA"].ToString()),
                            _tipogestion, int.Parse(DdlGestor.SelectedValue), 0, 0, 0, 0, int.Parse(DdlGestorApoyo.SelectedValue), TxtFechaDesde.Text, TxtFechaHasta.Text, _sql1, _sql2, "", "", ChkFecha.Checked ? 1 : 0, 0, 0, 0, _dtbgstsave, ViewState["Conectar"].ToString());

                        Session["Preview"] = _dts;

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            TblLista.Visible = true;
                        }

                        GrdvPreview.DataSource = _dts.Tables[0];
                        GrdvPreview.DataBind();
                        LblTotal.InnerText = _dts.Tables[1].Rows[0]["Total"].ToString();
                    }
                    else new FuncionesBAS().FunShowJSMessage("No se puede formar la consulta..!", this);
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
                string FileName = "PreviewApy_" + DdlCedente.SelectedItem.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
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
                    string style = @"<style> .textmode { } </style>";
                    Response.Write(style);
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
                    new FuncionesBAS().FunShowJSMessage("Ingrese Nombre de la Lista de Trabajo..!", this);
                    return;
                }

                if (DdlMarcado.SelectedValue == "0")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Tipo de Marcado..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(94, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(ViewState["CodigoCPCE"].ToString()), 0, "", TxtLista.Text.Trim().ToUpper(),
                    TxtFechaInicio.Text.Trim(), ViewState["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    new FuncionesBAS().FunShowJSMessage("Nombre de la Lista de Trabajo ya Existe..!", this);
                    _continuar = false;
                    return;
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    _tipogestion = 4;

                    if (int.Parse(LblTotal.InnerText) > 0)
                    {
                        if (int.Parse(ViewState["CodigoLista"].ToString()) > 0)
                        {
                            _sql = "Select top 0 codigoCLDE = CL.CLDE_CODIGO,codigoPERS = PE.PERS_CODIGO,codigoGEST = CD.ctde_gestorasignado,";
                            _sql += "Estado = 1,Operacion = CD.ctde_operacion,";
                            _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre,";
                            _sql += "DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible ";
                            _sql += "From GSBPO_CLIENTE_DEUDOR CL (nolock) INNER JOIN GSBPO_CUENTA_DEUDOR CD (nolock) ON CL.CLDE_CODIGO=CD.CLDE_CODIGO ";
                            _sql += "INNER JOIN GSBPO_PERSONA PE (nolock) ON CL.PERS_CODIGO=PE.PERS_CODIGO ";
                            _sql += "INNER JOIN GSBPO_Provincia PR (nolock) ON PE.pers_provincia=PR.PROV_COD ";
                            _sql += "INNER JOIN GSBPO_Ciudad CI (nolock) ON PE.pers_ciudad=CI.CIUD_COD where ";
                            _sql += "CL.CPCE_CODIGO=0 and CL.clde_estado=1 and CD.ctde_estado=1 and ";
                            _sql += "CD.ctde_gestorasignado=0";

                            _codlistaarbol = int.Parse(ViewState["CodigoLista"].ToString());

                            _sql1 = "select top 0 CodigoCLDE = CDE.CLDE_CODIGO from GSBPO_CUENTA_DEUDOR CDE(nolock) ";
                            _sql1 += "INNER JOIN GSBPO_CLIENTE_DEUDOR CLI (nolock) ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                            _sql1 += " where CLI.CPCE_CODIGO=0 and CDE.ctde_estado=1 and CDE.ctde_gestorasignado=0 ";
                        }
                        else
                        {
                            if (DdlGestorApoyo.SelectedValue == "0")
                                _sql = "Select distinct codigoCLDE = CL.CLDE_CODIGO,codigoPERS = PE.PERS_CODIGO,codigoGEST = CD.ctde_gestorasignado,";
                            else
                                _sql = "Select distinct codigoCLDE = CL.CLDE_CODIGO,codigoPERS = PE.PERS_CODIGO,codigoGEST=" + DdlGestorApoyo.SelectedValue + ",";
                            _sql += "Estado = 1,Operacion = CD.ctde_operacion,";
                            _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre,";
                            _sql += "DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible ";
                            _sql = FunFormarSQL(_sql, 1);

                            _sql1 = "select CodigoCLDE = CDE.CLDE_CODIGO from GSBPO_CUENTA_DEUDOR CDE (nolock) ";
                            _sql1 += "INNER JOIN GSBPO_CLIENTE_DEUDOR CLI (nolock) ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                            _sql1 += " where CLI.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CDE.ctde_estado=1 and ";

                            if (ChkGestor.Checked)
                                _sql1 += "CDE.ctde_gestorasignado=" + DdlGestor.SelectedValue + " and ";
                            else _sql1 += "CDE.ctde_gestorasignado>0 and ";
                            _sql1 = _sql1.Remove(_sql1.Length - 4);
                            _sql1 = _sql1 + " group by CDE.CLDE_CODIGO";

                            _sql2 = "select gete_cldecodigo,gete_araccodigo,gete_efectivo,";
                            _sql2 += "gete_fechagestion,gete_fechacreacion,gete_auxi1 ";
                            _sql2 += "from GSBPO_GESTION_TELEFONICA GTE (nolock) INNER JOIN GSBPO_CUENTA_DEUDOR CDE (nolock) ON GTE.gete_cldecodigo=CDE.CLDE_CODIGO ";
                            _sql2 += "where convert(date,GTE.gete_fechagestion,101) between CONVERT(date,'" + TxtFechaDesde.Text.Trim() + "',101) ";
                            _sql2 += "and CONVERT(date,'" + TxtFechaHasta.Text.Trim() + "',101) and ";
                            _sql2 += "gete_cedecodigo=" + DdlCedente.SelectedValue + " and gete_cpcecodigo=" + DdlCatalogo.SelectedValue + " and ";
                            _sql2 += "gete_auxi3=0 and gete_arefcodigo in(";
                            _sql2 = FunCrearSQL(_sql2);
                            _sql2 = _sql2 + " order by gete_fechacreacion desc";
                        }

                        _dtbgstsave = (DataTable)ViewState["DatosSave"];

                        _dts = new ListaTrabajoDAO().FunNewLstADE(1, _sql, int.Parse(ViewState["CodigoCEDE"].ToString()),
                            int.Parse(ViewState["CodigoCPCE"].ToString()), int.Parse(ViewState["CodigoLTCA"].ToString()),
                            _tipogestion, int.Parse(DdlGestor.SelectedValue), 0, 0, 0, 0, int.Parse(DdlGestorApoyo.SelectedValue), TxtFechaDesde.Text, TxtFechaHasta.Text, _sql1, _sql2, "", "", ChkFecha.Checked ? 1 : 0, 0, 0, 0, _dtbgstsave, ViewState["Conectar"].ToString());

                        _mensaje = new EstrategiaDAO().FunCrearListaTrabajo(_codlistaarbol, TxtLista.Text.Trim().ToUpper(),
                            TxtDescripcion.Text.Trim().ToUpper(), TxtFechaInicio.Text, TxtFechaFin.Text, int.Parse(DdlEstrategia.SelectedValue), int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), ChkEstado.Checked, DdlMarcado.SelectedValue, DdlCampania.SelectedValue, ChkGestor.Checked ? 1 : 0, "",
                            0, 0, ChkFecha.Checked ? 1 : 0, TxtFechaDesde.Text.Trim(), TxtFechaHasta.Text.Trim(), DdlGestor.SelectedValue, DdlAsignacion.SelectedValue, DdlGestorApoyo.SelectedValue, int.Parse(LblTotal.InnerText), 3, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _dts.Tables[0], (DataTable)ViewState["Estrategia"],
                            "sp_NewListaTrabajo", ViewState["Conectar"].ToString());

                        if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
                        {
                            _efecto = _efecto.Remove(_efecto.Length - 1);
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(148, int.Parse(_mensaje), 0, 0, "", _efecto, "",
                                ViewState["Conectar"].ToString());
                            _mensaje = _dts.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    else
                    {
                        new FuncionesBAS().FunShowJSMessage("No existen datos para registrar..!", this);
                        return;
                    }

                    if (_mensaje == "OK") Response.Redirect("WFrm_ListaTrabajoAdminEFE.aspx?MensajeRetornado='Guardado con Éxito'", true);
                    else new FuncionesBAS().FunShowJSMessage(_mensaje, this);
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