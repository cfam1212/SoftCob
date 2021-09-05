namespace SoftCob.Views.ListaTrabajo
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevaListaOnDemand : Page
    {
        #region Variables
        ListItem _itemc = new ListItem();
        DataSet _dts = new DataSet();
        DataTable _dtbgstsave = new DataTable();
        DataTable _dtb = new DataTable();
        DataView view;
        string _sql = "", _mensaje = "", _fechaactual = "", _ext = "", _path = "",
            _filename = "", _path1 = "", _line = "", _datos = "";
        bool _validar = false, _continuar = true;
        int _codlistaarbol = 0;
        DateTime _dtmfechainicio, _dtmfechafin, _dtmfechaactual;       
        string[] columnas, _formatos;
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

                if (int.Parse(ViewState["CodigoLista"].ToString()) == 0)
                {
                    Lbltitulo.Text = "Nueva Lista de Trabajo << ON DEMAND >>";
                    ViewState["Nuevo"] = "0";
                }
                else
                {
                    ViewState["Nuevo"] = "1";
                    pnlDatosListas.Visible = true;
                    DdlCedente.Enabled = false;
                    DdlCatalogo.Enabled = false;
                    FunCargarMantenimiento();
                    Lbltitulo.Text = "Editar Lista de Trabajo << ON DEMAND >>";
                    lblEstado.Visible = true;
                    ChkEstado.Visible = true;
                    ImgPreview.Enabled = false;
                    TxtFechaInicio.Enabled = false;
                    DdlGestorApoyo.Enabled = false;
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
                FileUpload1.Enabled = false;
                _dts = new ConsultaDatosDAO().FunConsultaDatos(23, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                TxtLista.Text = _dts.Tables[0].Rows[0]["ListaTrabajo"].ToString();
                TxtDescripcion.Text = _dts.Tables[0].Rows[0]["Descripcion"].ToString();
                TxtFechaInicio.Text = _dts.Tables[0].Rows[0]["FechaInicio"].ToString();
                TxtFechaFin.Text = _dts.Tables[0].Rows[0]["FechaFin"].ToString();
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

                DdlGestorApoyo.SelectedValue = _dts.Tables[0].Rows[0]["GestorApoyo"].ToString();
                _dts = new ConsultaDatosDAO().FunConsultaDatos(26, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "",
                    "", "", Session["Conectar"].ToString());
                ImgPreview.ImageUrl = "~/Botones/Buscargris.png";
                ImgPreview.Enabled = false;
                LblPreview.Visible = false;
                pnlPreview.Visible = false;
                _dts = new ConsultaDatosDAO().FunConsultaDatos(25, int.Parse(ViewState["CodigoLista"].ToString()), 0, 0, "",
                    "", "", Session["Conectar"].ToString());
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
                    GrdvPreview.DataSource = null;
                    GrdvPreview.DataBind();
                    LblTotal.InnerText = "0";

                    DdlGestores.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--",
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestores.DataTextField = "Descripcion";
                    DdlGestores.DataValueField = "Codigo";
                    DdlGestores.DataBind();

                    DdlGestorApoyo.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor Apoyo--",
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestorApoyo.DataTextField = "Descripcion";
                    DdlGestorApoyo.DataValueField = "Codigo";
                    DdlGestorApoyo.DataBind();

                    break;
                case 2:
                    DdlGestorApoyo.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor Apoyo--",
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
                new FuncionesDAO().FunShowJSMessage("Ingrese nombre de la Lista de Trabajo..!", this, "W", "L");
                _validar = false;
            }

            if (int.Parse(DdlCedente.SelectedValue) == 0)
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this, "W", "L");
                _validar = false;
            }

            if (int.Parse(DdlCatalogo.SelectedValue) == 0)
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo/Producto..!", this, "W", "L");
                _validar = false;
            }

            if (ChkGestor.Checked)
            {
                if (DdlGestores.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "L");
                    _validar = false;
                }
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
        #endregion

        #region Botones y Eventos
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
                if (FileUpload1.HasFile)
                {
                    _filename = FileUpload1.PostedFile.FileName;
                    _ext = _filename.Substring(_filename.LastIndexOf(".") + 1).ToLower();
                    _formatos = new string[] { "txt" };

                    if (Array.IndexOf(_formatos, _ext) < 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Formato de imagen inválido..!", this, "E", "C");
                        return;
                    }

                    //_path = System.IO.Path.GetDirectoryName(FileUpload1.PostedFile.FileName);

                }
                else
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione un archivo..!", this, "E", "C");
                    return;
                }

                _path = Server.MapPath("~/");
                FileUpload1.SaveAs(_path + "/Archivos/" + FileUpload1.FileName);
                _path1 = _path + "Archivos\\" + FileUpload1.FileName;

                List<string> _operaciones = new List<string>();
                using (StreamReader reader = File.OpenText(_path1))
                {

                    while ((_line = reader.ReadLine()) != null)
                    {
                        _operaciones.Add(_line.Trim());
                    }
                }

                _datos = string.Join(",", _operaciones.Select(x => string.Format("'{0}'", x)));

                _sql = "";
                TblLista.Visible = false;
                _continuar = FunValidarCampos();

                if (_continuar)
                {
                    _sql = "";
                    _sql = "SELECT Cliente=PE.pers_nombrescompletos,Identificacion=PE.pers_numerodocumento,";
                    _sql += "CodigoCLDE=CD.CLDE_CODIGO,CodigoPERS=PE.PERS_CODIGO,CodigoGEST=CD.ctde_gestorasignado,Estado=1,";
                    _sql += "Provincia=prov_nombre,Ciudad=ciud_nombre,Estado=1,FechaGestion=CONVERT(VARCHAR(10),CD.ctde_auxv3,101),";
                    _sql += "auxv1='',auxv2='',auxv3='',auxi1=0,auxi2=0,auxi3=0 ";
                    _sql += "From SoftCob_CLIENTE_DEUDOR CL (NOLOCK) INNER JOIN SoftCob_CUENTA_DEUDOR CD (NOLOCK) ON CL.CLDE_CODIGO=CD.CLDE_CODIGO ";
                    _sql += "INNER JOIN SoftCob_PERSONA PE (NOLOCK) ON CL.PERS_CODIGO=PE.PERS_CODIGO ";
                    _sql += "INNER JOIN SoftCob_Provincia PR (NOLOCK) ON PE.pers_provincia=PR.PROV_CODIGO ";
                    _sql += "INNER JOIN SoftCob_Ciudad CI (NOLOCK) ON PE.pers_ciudad=CI.CIUD_CODIGO WHERE ";
                    _sql += "CL.CPCE_CODIGO=" + DdlCatalogo.SelectedValue + " AND CL.clde_estado=1 AND CD.ctde_estado=1 AND ";
                    _sql += "CD.ctde_operacion IN(";
                    _sql += _datos + ") AND ";

                    if (ChkGestor.Checked)
                    {
                        if (DdlGestores.SelectedValue == "0")
                        {
                            new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "L");
                            _continuar = false;
                        }
                        else
                            _sql += "CL.clde_estado=1 AND CD.ctde_gestorasignado=" + DdlGestores.SelectedValue + " AND CD.ctde_estado=1 AND ";
                    }
                    else _sql += "CL.clde_estado=1 AND CD.ctde_gestorasignado>0 AND CD.ctde_estado=1 AND ";

                    if (DdlGestorApoyo.SelectedValue != "0") _sql += "CD.ctde_auxv2='" + DdlGestorApoyo.SelectedValue + "' AND ";
                    //_sql = FunFormarSQL(_sql, 1);

                    _sql = _sql.Remove(_sql.Length - 4);

                    if (!string.IsNullOrEmpty(_sql))
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

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
                    else new FuncionesDAO().FunShowJSMessage("No se Puede Formar la Consulta..!", this, "W", "L"); 
                }
                else new FuncionesDAO().FunShowJSMessage("Seleccione Datos para preview / Campos o Datos en Grid..!", this, "W", "L");
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
        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtLista.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre de la Lista de Trabajo..!", this, "W", "L");
                    return;
                }

                if (DdlMarcado.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Marcado..!", this, "W", "L");
                    return;
                }

                if (ViewState["Nuevo"].ToString() == "0")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(94, int.Parse(DdlCedente.SelectedValue),
                        int.Parse(DdlCatalogo.SelectedValue), 0, "", TxtLista.Text.Trim().ToUpper(), TxtFechaInicio.Text.Trim(),
                        Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Nombre de la Lista de Trabajo ya Existe..!", this, "W", "L");
                        _continuar = false;
                        return;
                    }

                    if (ChkGestor.Checked)
                    {
                        if (DdlGestores.SelectedValue == "0")
                        {
                            new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "L");
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
                            0, int.Parse(DdlCedente.SelectedValue),
                            int.Parse(DdlCatalogo.SelectedValue), ChkEstado.Checked, DdlMarcado.SelectedValue,
                            FileUpload1.FileName, 0, "", 0, 0, 0, "", "", DdlGestores.SelectedValue,
                            DdlGestorApoyo.SelectedValue, DdlGestorApoyo.SelectedValue, int.Parse(LblTotal.InnerText),
                            5, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _dtbgstsave,
                            (DataTable)ViewState["Estrategia"], "sp_NewListaTrabajo", Session["Conectar"].ToString());

                        if (int.Parse(ViewState["CodigoLista"].ToString()) == 0) _mensaje = "OK";
                    }
                    else
                    {
                        new FuncionesDAO().FunShowJSMessage("No Existen Datos Para Registrar..!", this, "W", "L");
                        return;
                    }

                    if (_mensaje == "OK")
                    {
                        if (ViewState["Regresar"].ToString() == "L") Response.Redirect("WFrm_ListaTrabajoAdminDEMAN.aspx?MensajeRetornado=Guardado con Éxito", true);

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
            if (ViewState["Regresar"].ToString() == "L") Response.Redirect("WFrm_ListaTrabajoAdminDEMAN.aspx", true);
            if (ViewState["Regresar"].ToString() == "M") Response.Redirect("..\\ReportesManager\\WFrm_MonitoreoLstAdmin.aspx", true);
        }
        #endregion
    }
}