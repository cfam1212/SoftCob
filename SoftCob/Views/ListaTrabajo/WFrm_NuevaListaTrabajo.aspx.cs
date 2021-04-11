namespace SoftCob.Views.ListaTrabajo
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Drawing;
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
        DataTable _dtblistatrabajo = new DataTable();
        string _sql = "", _estrategia = "", _ordenar = "", _mensaje = "", _fechaactual = "", _filename = "", _style = "";
        bool _validar = false, _continuar = true;
        int _codlistaarbol = 0, _pasadas = 0;
        DateTime _dtmfechainicio, _dtmfechafin, _dtmfechaactual;
        CheckBox _chkselected = new CheckBox();
        ListItem _accion = new ListItem();
        ListItem _efecto = new ListItem();
        ListItem _respuesta = new ListItem();
        ListItem _contacto = new ListItem();
        ListItem _lista = new ListItem();
        ListItem _asignacion = new ListItem();
        ListItem _campania = new ListItem();
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
                ViewState["Preview"] = false;
                ViewState["CodLista"] = null;
                ViewState["CodCatalogo"] = null;
                ViewState["CodMarcado"] = null;
                TxtFechaInicio.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                LblTotal.InnerText = "0";
                FunCargarCombos(0);
                FunCargarCombos(1);

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

                    DdlMarcado.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO MARCADO", 
                        "--Seleccione Tipo Marcado--", "S");
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
                    DdlAsignacion.DataSource = new ConsultaDatosDAO().FunConsultaDatos(91, int.Parse(ViewState["CodCatalogo"].ToString()), 0, 0, "",
                        "", "", Session["Conectar"].ToString());
                    DdlAsignacion.DataTextField = "Descripcion";
                    DdlAsignacion.DataValueField = "Codigo";
                    DdlAsignacion.DataBind();

                    DdlCampania.DataSource = new ConsultaDatosDAO().FunConsultaDatos(119, int.Parse(ViewState["CodCatalogo"].ToString()),
                        0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlCampania.DataTextField = "Descripcion";
                    DdlCampania.DataValueField = "Codigo";
                    DdlCampania.DataBind();
                    break;
            }
        }

        private bool FunValidarCampos()
        {
            _validar = true;

            if (DdlEstrategia.SelectedValue == "0")
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Estrategia..!", this);
                _validar = false;
            }

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

            if (ChkGestor.Checked)
            {
                if (DdlGestores.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this);
                    _validar = false;
                }
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
                _sql += "From SoftCob_CLIENTE_DEUDOR CL (nolock) INNER JOIN SoftCob_CUENTA_DEUDOR CD (nolock) ON CL.CLDE_CODIGO=CD.CLDE_CODIGO ";
                _sql += "INNER JOIN SoftCob_PERSONA PE (nolock) ON CL.PERS_CODIGO=PE.PERS_CODIGO ";
                _sql += "INNER JOIN SoftCob_PROVINCIA PR (nolock) ON PE.pers_provincia=PR.PROV_CODIGO ";
                _sql += "INNER JOIN SoftCob_CIUDAD CI (nolock) ON PE.pers_ciudad=CI.CIUD_CODIGO where ";
                _sql += "CL.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " and CL.clde_estado=1 and CD.ctde_estado=1 and ";

                if (ChkGestor.Checked)
                {
                    if (DdlGestores.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this);
                        _continuar = false;
                    }
                    else
                        _sql += "CL.clde_estado=1 and CD.ctde_gestorasignado=" + DdlGestores.SelectedValue + " and CD.ctde_estado=1 and ";
                }
                else _sql += "CL.clde_estado=1 and CD.ctde_gestorasignado>0 and CD.ctde_estado=1 and ";

                if (DdlAsignacion.SelectedValue != "0") _sql += "CD.ctde_auxv2='" + DdlAsignacion.SelectedValue + "' and ";

                if (DdlCampania.SelectedValue != "0") _sql += "CD.ctde_auxi4=" + DdlCampania.SelectedValue + " and ";

                if (_continuar)
                {
                    _dtbestrategia = (DataTable)ViewState["Estrategia"];
                    foreach (DataRow row in _dtbestrategia.Rows)
                    {
                        if (row["Operacion"].ToString() != "between")
                            _estrategia += row["Campo"].ToString() + " " + row["Operacion"].ToString() + " " + row["Valor"].ToString() + " and ";
                        else
                        {
                            if (_pasadas == 0) _estrategia += row["Campo"].ToString() + " " + row["Operacion"].ToString() + " " + row["Valor"].ToString() + " and ";
                            else _estrategia += row["Valor"].ToString() + " and ";
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
                ViewState["Estrategia"] = null;
                LblTotal.InnerText = "0";
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
                TblLista.Visible = false;
                FunCargarCombos(1);
                _dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoCedente"] = DdlCedente.SelectedValue;
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
                    ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
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

        protected void ImgPreview_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                TblLista.Visible = false;
                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    _dtbestrategia = (DataTable)ViewState["Estrategia"];
                    _sql = "select distinct Cliente = PE.pers_nombrescompletos,Identificacion = PE.pers_numerodocumento,";
                    _sql += "Operacion = CD.ctde_operacion, DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible,codigoCLDE = CD.CLDE_CODIGO,";
                    _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre ";
                    _sql = FunFormarSQL(_sql, 1);

                    if (!string.IsNullOrEmpty(_sql))
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());
                        GrdvPreview.DataSource = _dts;
                        GrdvPreview.DataBind();
                        Session["Preview"] = _dts;

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            TblLista.Visible = true;
                        }

                        _sql = "";
                        _sql = "select count(1) ";
                        _sql = FunFormarSQL(_sql, 0);
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());
                        LblTotal.InnerText = _dts.Tables[0].Rows[0][0].ToString();
                    }
                    else new FuncionesDAO().FunShowJSMessage("No se puede formar la consulta..!", this);
                }
                else new FuncionesDAO().FunShowJSMessage("Seleccione Datos para preview / Campos o Datos en Grid..!", this);
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

                if (ViewState["Nuevo"].ToString() == "0")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(94, int.Parse(ViewState["CodigoCedente"].ToString()), int.Parse(ViewState["CodCatalogo"].ToString()), 0, "",
                        TxtLista.Text.Trim().ToUpper(), TxtFechaInicio.Text.Trim(), Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Nombre de la Lista de Trabajo ya Existe..!", this);
                        _continuar = false;
                        return;
                    }

                    if (ChkGestor.Checked)
                    {
                        if (DdlGestores.SelectedValue == "0")
                        {
                            new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this);
                            return;
                        }
                    }
                }

                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    if (int.Parse(LblTotal.InnerText) > 0)
                    {
                        if (int.Parse(ViewState["CodigoLista"].ToString()) > 0)
                        {
                            _sql = "Select top 0 codigoCLDE = CL.CLDE_CODIGO,codigoPERS = PE.PERS_CODIGO,codigoGEST = CD.ctde_gestorasignado,";
                            _sql += "Estado = 1,Operacion = CD.ctde_operacion,auxv2 = '',auxv3 = '',auxi1 = 0,auxi2 = 0,auxi3 = 0,";
                            _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre,";
                            _sql += "DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible ";
                            _codlistaarbol = int.Parse(ViewState["CodigoLista"].ToString());
                        }
                        else
                        {
                            _codlistaarbol = 0;
                            if (DdlGestorApoyo.SelectedValue == "0")
                            {
                                _sql = "Select distinct codigoCLDE = CL.CLDE_CODIGO,codigoPERS = PE.PERS_CODIGO,codigoGEST = CD.ctde_gestorasignado,";
                                _sql += "Estado = 1,Operacion = CD.ctde_operacion,auxv2 = '',auxv3 = '',auxi1 = 0,auxi2 = 0,auxi3 = 0,";
                                _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre,";
                                _sql += "DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible ";
                            }
                            else
                            {
                                _sql = "Select distinct codigoCLDE = CL.CLDE_CODIGO,codigoPERS = PE.PERS_CODIGO,codigoGEST =" + DdlGestorApoyo.SelectedValue + ",";
                                _sql += "Estado = 1,Operacion = CD.ctde_operacion,auxv2 = '',auxv3 = '',auxi1 = 0,auxi2 = 0,auxi3 = 0,";
                                _sql += "EstadoCivil = pers_estadocivil,Genero = pers_genero,Provincia = prov_nombre,Ciudad = ciud_nombre,";
                                _sql += "DiasMora = CD.ctde_diasmora,Exigible = CD.ctde_valorexigible ";
                            }
                        }

                        _sql = FunFormarSQL(_sql, 1);
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());
                        string[] columnas = new[] { "codigoCLDE", "codigoPERS","codigoGEST", "Estado", "Operacion", "auxv2", "auxv3",
                            "auxi1","auxi2","auxi3"};
                        _dtblistatrabajo = _dts.Tables[0];
                        DataView view = new DataView(_dtblistatrabajo);
                        _dtblistatrabajo = view.ToTable(true, columnas);
                        _mensaje = new EstrategiaDAO().FunCrearListaTrabajo(_codlistaarbol, TxtLista.Text.Trim().ToUpper(),
                            TxtDescripcion.Text.Trim().ToUpper(), TxtFechaInicio.Text, TxtFechaFin.Text, int.Parse(DdlEstrategia.SelectedValue), int.Parse(DdlCedente.SelectedValue),
                            int.Parse(DdlCatalogo.SelectedValue), ChkEstado.Checked, DdlMarcado.SelectedValue, DdlCampania.SelectedValue,
                            0, "", 0, 0, 0, "", "", DdlGestores.SelectedValue, DdlAsignacion.SelectedValue, DdlGestorApoyo.SelectedValue,
                            int.Parse(LblTotal.InnerText),
                            0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _dtblistatrabajo,
                            (DataTable)ViewState["Estrategia"], "sp_NewListaTrabajo", Session["Conectar"].ToString());

                        if (int.Parse(ViewState["CodigoLista"].ToString()) == 0) _mensaje = "OK";
                    }
                    else
                    {
                        new FuncionesDAO().FunShowJSMessage("No existen datos para registrar..!", this);
                        return;
                    }

                    if (_mensaje == "OK")
                    {
                        if (ViewState["Regresar"].ToString() == "L") Response.Redirect("WFrm_ListaTrabajoAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);

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

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                _filename = "Preview_" + DdlCedente.SelectedItem.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
            FunCargarCombos(1);
            FunCargarCombos(2);
        }

        protected void ChkGestor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GrdvPreview.DataSource = null;
                GrdvPreview.DataBind();
                TblLista.Visible = false;
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

        protected void DdlGestores_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            if (ViewState["Regresar"].ToString() == "L") Response.Redirect("WFrm_ListaTrabajoAdmin.aspx", true);
            if (ViewState["Regresar"].ToString() == "M") Response.Redirect("..\\ReportesManager\\WFrm_MonitoreoLstAdmin.aspx", true);
        }
        #endregion
    }
}