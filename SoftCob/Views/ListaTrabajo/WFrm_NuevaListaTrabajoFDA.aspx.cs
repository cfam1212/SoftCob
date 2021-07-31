namespace SoftCob.Views.ListaTrabajo
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevaListaTrabajoFDA : Page
    {
        #region Variables
        ListItem _itemc = new ListItem();
        DataSet _dts = new DataSet();
        DataSet _dts1 = new DataSet();
        DataTable _dtbestrategia = new DataTable();
        DataTable _dtblistatrabajo = new DataTable();
        DataTable _dtbgestoresgrupos = new DataTable();
        DataTable _dtbgstsave = new DataTable();
        DataTable _tblbuscar = new DataTable();
        DataTable _dtbcodigos = new DataTable();
        DataTable _dtb = new DataTable();
        DataRow _result;
        bool _lexiste = false;
        string _sql = "", _estrategia = "", _ordenar = "", _fechaactual = "", _mensaje = "",
            _sql1 = "", _grupo = "", _sql2 = "";
        string[] _codigosopm;
        bool _validar = false, _continuar = true;
        int _tipogestion = 0, _pasadas = 0, _codlistaarbol = 0;
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
                //if (Session["IN-CALL"].ToString() == "SI")
                //{
                //    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                //    return;
                //}
                _dtbcodigos.Columns.Add("Grupo");
                ViewState["Grupos"] = _dtbcodigos;
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
                LblTotal.InnerText = "0";
                FunCargarCombos(0);
                FunCargarCombos(1);

                if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
                {
                    Lbltitulo.Text = "Nueva Lista de Trabajo << FOCALIZADA POR Grupos-- >>";
                    ViewState["Nuevo"] = "0";
                }
                else
                {
                    ViewState["Nuevo"] = "1";
                    PnlConfiguracion.Enabled = false;
                    PnlGrupos.Visible = true;
                    PnlGrupos.Enabled = false;
                    PnlOpcionGestion.Enabled = false;
                    DdlEstrategia.Enabled = false;
                    DdlCedente.Enabled = false;
                    DdlCatalogo.Enabled = false;
                    FunCargarMantenimiento();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(147, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                    _codigosopm = _dts.Tables[0].Rows[0]["Codigos"].ToString().Split(',');
                    _dtbcodigos = (DataTable)ViewState["Grupos"];

                    foreach (var datos in _codigosopm)
                    {
                        _result = _dtbcodigos.NewRow();
                        _result["Grupo"] = datos;
                        _dtbcodigos.Rows.Add(_result);
                    }

                    ViewState["Grupos"] = _dtbcodigos;
                    GrdvGrupos.DataSource = _dtbcodigos;
                    GrdvGrupos.DataBind();
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
                DdlGestorApoyo.SelectedValue = _dts.Tables[0].Rows[0]["GestorApoyo"].ToString();
                DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                DdlCatalogo.DataTextField = "CatalogoProducto";
                DdlCatalogo.DataValueField = "CodigoCatalogo";
                DdlCatalogo.DataBind();
                DdlCatalogo.SelectedValue = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                FunCargarCombos(2);
                DdlMarcado.SelectedValue = _dts.Tables[0].Rows[0]["Marcado"].ToString();
                ChkGestor.Checked = _dts.Tables[0].Rows[0]["Gestor"].ToString() != "0" ? true : false;
                DdlGestor.SelectedValue = _dts.Tables[0].Rows[0]["Gestor"].ToString();
                ChkEstado.Text = _dts.Tables[0].Rows[0]["Estado"].ToString();
                ChkEstado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;

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

                    DdlMarcado.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO MARCADO", "--Seleccione Tipo Marcado--", "S");
                    DdlMarcado.DataTextField = "Descripcion";
                    DdlMarcado.DataValueField = "Codigo";
                    DdlMarcado.DataBind();

                    break;
                case 1:
                    GrdvGrupos.DataSource = null;
                    GrdvGrupos.DataBind();
                    _dtbcodigos = (DataTable)ViewState["Grupos"];
                    _dtbcodigos.Clear();
                    GrdvGestores.DataSource = null;
                    GrdvGestores.DataBind();
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

                    DdlGestorApoyo.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", 
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestorApoyo.DataTextField = "Descripcion";
                    DdlGestorApoyo.DataValueField = "Codigo";
                    DdlGestorApoyo.DataBind();
                    break;
            }
        }

        private bool FunValidarCampos()
        {
            _validar = true;

            if (string.IsNullOrEmpty(TxtLista.Text.Trim()))
            {
                new FuncionesDAO().FunShowJSMessage("Ingrese nombre de la _lista de Trabajo..!", this, "W", "C");
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

            if (_dtmfechafin < _dtmfechainicio)
            {
                new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser mayor a Fecha Fin..!", this, "E", "C");
                _validar = false; ;
            }

            if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
            {
                if (_dtmfechainicio < _dtmfechaactual)
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha Inicio no puede ser menor a la Fecha Actual..!", this, "E", "C");
                    _validar = false; ;
                }

                _dtbcodigos = (DataTable)ViewState["Grupos"];

                if (_dtbcodigos.Rows.Count == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Grupo de Focalizacón..!", this, "W", "C");
                    _validar = false; ;
                }
            }
            return _validar;
        }

        private string FunCrearSql(string _sqlformado)
        {
            try
            {
                _grupo = "";
                _dtbcodigos = (DataTable)ViewState["Grupos"];

                foreach (DataRow row in _dtbcodigos.Rows)
                {
                    _grupo += row["Grupo"] + ",";
                }

                _sqlformado += _grupo;
                _sqlformado = _sqlformado.Remove(_sqlformado.Length - 1) + ") and ";

                _dtbestrategia = (DataTable)ViewState["Estrategia"];

                foreach (DataRow row in _dtbestrategia.Rows)
                {
                    _estrategia += row["Campo"].ToString() + " " + row["Operacion"].ToString() + " " + row["Valor"].ToString() + " and ";
                }

                _sqlformado += _estrategia;
                _sqlformado = _sqlformado.Remove(_sqlformado.Length - 4);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _sqlformado;
        }

        private string FunFormarSql(string nuevosql, int tipo)
        {
            try
            {
                _continuar = true; _estrategia = ""; _ordenar = "";
                _pasadas = 0;

                _sql = nuevosql;
                _sql += "From SoftCob_CLIENTE_DEUDOR CL (nolock) INNER JOIN SoftCob_CUENTA_DEUDOR CD (nolock) ON CL.CLDE_CODIGO=CD.CLDE_CODIGO ";
                _sql += "INNER JOIN SoftCob_PERSONA PE (nolock) ON CL.PERS_CODIGO=PE.PERS_CODIGO ";
                _sql += "INNER JOIN SoftCob_PROVINCIA PR (nolock) ON PE.pers_provincia=PR.PROV_CODIGO ";
                _sql += "INNER JOIN SoftCob_CIUDAD CI (nolock) ON PE.pers_ciudad=CI.CIUD_CODIGO where ";
                _sql += "CL.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CL.clde_estado=1 and CD.ctde_estado=1 and ";

                if (ChkGestor.Checked)
                {
                    if (DdlGestor.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this);
                        _continuar = false;
                    }
                    else
                        _sql += "CL.clde_estado=1 and CD.ctde_gestorasignado=" + DdlGestor.SelectedValue + " and CD.ctde_auxi2 in(";
                }
                else _sql += "CD.ctde_gestorasignado>0 and CD.ctde_auxi2 in(";

                if (_continuar)
                {
                    _dtbcodigos = (DataTable)ViewState["Grupos"];

                    foreach (DataRow row in _dtbcodigos.Rows)
                    {
                        _grupo += row["Grupo"] + ",";
                    }

                    _sql += _grupo;
                    _sql = _sql.Remove(_sql.Length - 1) + ") and ";

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
        protected void ImgDelGrupo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
                LblTotal.InnerText = "0";
                _grupo = GrdvGrupos.DataKeys[_gvrow.RowIndex].Values["Grupo"].ToString();
                _dtbcodigos = (DataTable)ViewState["Grupos"];
                _result = _dtbcodigos.Select("Grupo='" + _grupo + "'").FirstOrDefault();
                _result.Delete();
                _dtbcodigos.AcceptChanges();

                GrdvGrupos.DataSource = _dtbcodigos;
                GrdvGrupos.DataBind();

                if (ViewState["GestoresGrupos"] != null)
                {
                    _dtbgestoresgrupos = (DataTable)ViewState["GestoresGrupos"];
                    _dtbgestoresgrupos.Clear();
                    GrdvGestores.DataSource = _dtbgestoresgrupos;
                    GrdvGestores.DataBind();
                }

                if (_dtbcodigos.Rows.Count == 0) PnlGrupos.Visible = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

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
                GrdvGrupos.DataSource = null;
                GrdvGrupos.DataBind();
                GrdvGestores.DataSource = null;
                GrdvGestores.DataBind();
                _dtbcodigos = (DataTable)ViewState["Grupos"];
                _dtbcodigos.Clear();
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

        protected void ImgAddGrupo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtGrupo.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Código del _grupo..!", this, "W", "C");
                    return;
                }

                if (ViewState["Grupos"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["Grupos"];
                    _result = _tblbuscar.Select("Grupo='" + TxtGrupo.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya Existe Grupo ingresado..!", this, "E", "C");
                    return;
                }
                //Buscar si el _grupo existe y esta activo
                _dts = new ConsultaDatosDAO().FunConsultaDatos(149, int.Parse(DdlCatalogo.SelectedValue), int.Parse(TxtGrupo.Text.Trim()), 0, "", "", "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("No existen datos en ese Grupo..!", this, "E", "C");
                    return;
                }

                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                GrdvGestores.DataSource = null;
                GrdvGestores.DataBind();
                PnlGrupos.Visible = true;
                _dtbcodigos = new DataTable();
                _dtbcodigos = (DataTable)ViewState["Grupos"];
                _result = _dtbcodigos.NewRow();
                _result["Grupo"] = TxtGrupo.Text;
                _dtbcodigos.Rows.Add(_result);
                ViewState["Grupos"] = _dtbcodigos;
                GrdvGrupos.DataSource = _dtbcodigos;
                GrdvGrupos.DataBind();
                TxtGrupo.Text = "";
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
                GrdvGrupos.DataSource = null;
                GrdvGrupos.DataBind();
                _dtbcodigos = (DataTable)ViewState["Grupos"];
                _dtbcodigos.Clear();
                GrdvGestores.DataSource = null;
                GrdvGestores.DataBind();
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

        protected void ImgPreview_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                TblLista.Visible = false;
                _sql = ""; _sql1 = "";

                if (DdlEstrategia.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("No existe _estrategia Seleccionada..!", this, "W", "C");
                    return;
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    _tipogestion = 5;

                    _sql = "select distinct Cliente = PE.pers_nombrescompletos,Identificacion = PE.pers_numerodocumento,codigoCLDE = CD.CLDE_CODIGO,";
                    _sql += "Operacion = CD.ctde_operacion, DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible,";
                    _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre ";
                    _sql = FunFormarSql(_sql, 1);

                    if (!string.IsNullOrEmpty(_sql))
                    {
                        _sql1 = "select CodigoCLDE = CDE.CLDE_CODIGO from SoftCob_CUENTA_DEUDOR CDE (nolock) ";
                        _sql1 += "INNER JOIN SoftCob_CLIENTE_DEUDOR CLI (nolock) ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                        _sql1 += " where CLI.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CDE.ctde_estado=1 and ";

                        if (ChkGestor.Checked)
                            _sql1 += "CDE.ctde_gestorasignado=" + DdlGestor.SelectedValue + " and CDE.ctde_auxi2 in(";
                        else _sql1 += "CDE.ctde_gestorasignado>0 and CDE.ctde_auxi2 in(";

                        _sql1 = FunCrearSql(_sql1);
                        _sql1 = _sql1 + " group by CDE.CLDE_CODIGO";

                        _sql2 = "select 0,0,0,getdate(),getdate(),0";

                        _dtbgstsave = (DataTable)ViewState["DatosSave"];

                        _dts = new ListaTrabajoDAO().FunNewLstADE(0, _sql, int.Parse(ViewState["CodigoCEDE"].ToString()),
                            int.Parse(ViewState["CodigoCPCE"].ToString()), int.Parse(ViewState["CodigoLTCA"].ToString()),
                            _tipogestion, int.Parse(DdlGestor.SelectedValue), 0, 0, 0, 0, int.Parse(DdlGestorApoyo.SelectedValue), "", "", _sql1, _sql2, "", "", 0, 0, 0, 0, _dtbgstsave, Session["Conectar"].ToString());

                        ViewState["Preview"] = _dts.Tables[0];

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            TblLista.Visible = true;
                        }

                        GrdvPreview.DataSource = _dts.Tables[0];
                        GrdvPreview.DataBind();
                        LblTotal.InnerText = _dts.Tables[1].Rows[0]["Total"].ToString();

                        _sql = "";
                        _sql = "select Codigo_grupo = CDE.ctde_auxi2,CodigoGESTOR = CDE.ctde_gestorasignado ";
                        _sql += "from SoftCob_CUENTA_DEUDOR CDE (nolock) INNER JOIN SoftCob_CLIENTE_DEUDOR CLI (nolock) ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                        _sql += "where CLI.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CDE.ctde_estado=1 and ";
                        _sql += "CDE.ctde_auxi2 in(";
                        _sql = FunCrearSql(_sql);
                        _sql += " and ctde_gestorasignado>0 order by CDE.ctde_auxi2,CDE.ctde_gestorasignado";
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(150, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

                        ViewState["GestoresGrupos"] = _dts.Tables[0];
                        GrdvGestores.DataSource = _dts;
                        GrdvGestores.DataBind();
                    }
                    else new FuncionesDAO().FunShowJSMessage("No se puede formar la consulta..!", this, "E", "C");
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
                //Response.Clear();
                //Response.Buffer = true;
                //string FileName = "PreviewApy_" + DdlCedente.SelectedItem.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.ms-excel";
                //using (StringWriter sw = new StringWriter())
                //{
                //    HtmlTextWriter hw = new HtmlTextWriter(sw);
                //    GrdvPreview.AllowPaging = false;
                //    GrdvPreview.DataSource = (DataSet)Session["Preview"];
                //    GrdvPreview.DataBind();
                //    GrdvPreview.HeaderRow.BackColor = Color.White;
                //    foreach (GridViewRow row in GrdvPreview.Rows)
                //    {
                //        row.BackColor = Color.White;
                //        row.Cells[1].Style.Add("mso-number-format", "\\@");
                //        row.Cells[2].Style.Add("mso-number-format", "\\@");
                //    }
                //    GrdvPreview.RenderControl(hw);
                //    string style = @"<style> .textmode { } </style>";
                //    Response.Write(style);
                //    Response.Output.Write(sw.ToString());
                //    Response.Flush();
                //    Response.End();
                //}

                _dtb = (DataTable)ViewState["Preview"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    string _filename = "PreviewApy_" + DdlCedente.SelectedItem.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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
                _sql = ""; _sql1 = "";

                if (string.IsNullOrEmpty(TxtLista.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre de la _lista de Trabajo..!", this, "W", "C");
                    return;
                }

                if (DdlMarcado.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Marcado..!", this, "W", "C");
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(94, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(ViewState["CodigoCPCE"].ToString()), 0, "", TxtLista.Text.Trim().ToUpper(),
                    TxtFechaInicio.Text.Trim(), Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Nombre de la _lista de Trabajo ya Existe..!", this, "E", "C");
                    _continuar = false;
                    return;
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    _tipogestion = 5;

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
                            _sql = FunFormarSql(_sql, 1);

                            _sql1 = "select CodigoCLDE = CDE.CLDE_CODIGO from SoftCob_CUENTA_DEUDOR CDE (nolock) ";
                            _sql1 += "INNER JOIN SoftCob_CLIENTE_DEUDOR CLI (nolock) ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                            _sql1 += " where CLI.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CDE.ctde_estado=1 and ";

                            if (ChkGestor.Checked)
                                _sql1 += "CDE.ctde_gestorasignado=" + DdlGestor.SelectedValue + " and CDE.ctde_auxi2 in(";
                            else _sql1 += "CDE.ctde_gestorasignado>0 and CDE.ctde_auxi2 in(";

                            //_sql1 = _sql1.Remove(_sql1.Length - 4);
                            _sql1 = FunCrearSql(_sql1);
                            _sql1 = _sql1 + " group by CDE.CLDE_CODIGO";

                            _sql2 = "select 0,0,0,getdate(),getdate(),0";
                        }

                        _dtbgstsave = (DataTable)ViewState["DatosSave"];

                        _dts = new ListaTrabajoDAO().FunNewLstADE(1, _sql, int.Parse(ViewState["CodigoCEDE"].ToString()),
                            int.Parse(ViewState["CodigoCPCE"].ToString()), int.Parse(ViewState["CodigoLTCA"].ToString()),
                            _tipogestion, int.Parse(DdlGestor.SelectedValue), 0,
                            0, 0, 0, int.Parse(DdlGestorApoyo.SelectedValue), "", "",
                            _sql1, _sql2, "", "", 0, 0, 0, 0, _dtbgstsave, Session["Conectar"].ToString());

                        _mensaje = new EstrategiaDAO().FunCrearListaTrabajo(_codlistaarbol, TxtLista.Text.Trim().ToUpper(),
                            TxtDescripcion.Text.Trim().ToUpper(), TxtFechaInicio.Text, TxtFechaFin.Text, int.Parse(DdlEstrategia.SelectedValue), int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), ChkEstado.Checked, DdlMarcado.SelectedValue, "", ChkGestor.Checked ? 1 : 0, "", 0, 0, 0, "", "", DdlGestor.SelectedValue, "", DdlGestorApoyo.SelectedValue, int.Parse(LblTotal.InnerText), 4, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _dts.Tables[0], (DataTable)ViewState["Estrategia"],
                            "sp_NewListaTrabajo", Session["Conectar"].ToString());

                        if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
                        {
                            _dtbgestoresgrupos = (DataTable)ViewState["GestoresGrupos"];

                            foreach (DataRow drfila in _dtbgestoresgrupos.Rows)
                            {
                                new ConsultaDatosDAO().FunConsultaDatos(152, int.Parse(_mensaje),
                                    int.Parse(drfila["Total"].ToString()), 0, drfila["Gestor"].ToString(),
                                    drfila["CodigoGrupo"].ToString(), "", Session["Conectar"].ToString());
                            }

                            _grupo = _grupo.Remove(_grupo.Length - 1);
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(148, int.Parse(_mensaje), 0, 0, "", _grupo, "",
                                Session["Conectar"].ToString());
                            _mensaje = _dts.Tables[0].Rows[0][0].ToString();
                        }
                    }
                    else
                    {
                        new FuncionesDAO().FunShowJSMessage("No existen datos para registrar..!", this, "E", "C");
                        return;
                    }

                    if (_mensaje == "OK") Response.Redirect("WFrm_ListaTrabajoAdminFDA.aspx?MensajeRetornado=Guardado con Éxito", true);
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
            Response.Redirect("WFrm_ListaTrabajoAdminFDA.aspx", true);
        }
        #endregion
    }
}