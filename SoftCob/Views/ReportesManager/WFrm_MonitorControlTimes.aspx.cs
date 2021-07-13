namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_MonitorControlTimes : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        ListItem _accion = new ListItem();
        ListItem _efecto = new ListItem();
        ListItem _respuesta = new ListItem();
        ListItem _contacto = new ListItem();
        ListItem _itemc = new ListItem();
        string _filename = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ImgExportar);
            scriptManager.RegisterPostBackControl(this.ImgExportar1);
            scriptManager.RegisterPostBackControl(this.ImgExportar2);
            if (!IsPostBack)
            {
                TxtFechaInicio.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Monitoreo - Tiempos - Gestión - Llamada";
                FunCargarCombos(0);
            }
            else
            {
                GrdvEfectivas.DataSource = ViewState["GrdvEfectivas"];
                GrdvMaxLlamada.DataSource = ViewState["GrdvMaximo"];
                GrdvLogueos.DataSource = ViewState["GrdvLogueo"];
            }
        }
        #endregion

        #region Procedimientos y Funciones
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

                    _dts = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", int.Parse(DdlCedente.SelectedValue),
                        0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestores.DataSource = _dts;
                    DdlGestores.DataTextField = "Descripcion";
                    DdlGestores.DataValueField = "Codigo";
                    DdlGestores.DataBind();
                    break;
                case 1:
                    GrdvEfectivas.DataSource = null;
                    GrdvEfectivas.DataBind();
                    GrdvMaxLlamada.DataSource = null;
                    GrdvMaxLlamada.DataBind();
                    GrdvLogueos.DataSource = null;
                    GrdvLogueos.DataBind();

                    ImgExportar.Visible = false;
                    LblExportar.Visible = false;
                    ImgExportar1.Visible = false;
                    LblExportar1.Visible = false;
                    ImgExportar2.Visible = false;
                    LblExportar2.Visible = false;

                    _dts = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestores.DataSource = _dts;
                    DdlGestores.DataTextField = "Descripcion";
                    DdlGestores.DataValueField = "Codigo";
                    DdlGestores.DataBind();
                    break;
                case 2:
                    GrdvEfectivas.DataSource = null;
                    GrdvEfectivas.DataBind();
                    GrdvMaxLlamada.DataSource = null;
                    GrdvMaxLlamada.DataBind();
                    GrdvLogueos.DataSource = null;
                    GrdvLogueos.DataBind();

                    ImgExportar.Visible = false;
                    LblExportar.Visible = false;
                    ImgExportar1.Visible = false;
                    LblExportar1.Visible = false;
                    ImgExportar2.Visible = false;
                    LblExportar2.Visible = false;
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(1);

                _dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["codigoCEDE"] = DdlCedente.SelectedValue;
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
                    ViewState["codigoCPCE"] = DdlCatalogo.SelectedValue;
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
            ViewState["codigoCPCE"] = DdlCatalogo.SelectedValue;
            FunCargarCombos(2);
        }
        protected void DdlGestores_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(2);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["GrdvEfectivas"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    _filename = "MonitorEfectivas_" + DdlGestores.SelectedItem.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void ImgExportar1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["GrdvMaximo"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    _filename = "MonitorMaxGestion_" + DdlGestores.SelectedItem.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void ImgExportar2_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["GrdvLogueo"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    _filename = "MonitorLogueo_" + DdlGestores.SelectedItem.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlCatalogo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo..!", this);
                    return;
                }

                if (DdlGestores.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaInicio.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaFin.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (DateTime.ParseExact(TxtFechaInicio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > 
                    DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(99, int.Parse(DdlGestores.SelectedValue), 0, 0, "", 
                    TxtFechaInicio.Text, TxtFechaFin.Text, Session["Conectar"].ToString());

                GrdvLogueos.DataSource = _dts;
                GrdvLogueos.DataBind();
                ViewState["GrdvLogueo"] = _dts.Tables[0];

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ImgExportar.Visible = true;
                    LblExportar.Visible = true;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(100, int.Parse(DdlGestores.SelectedValue), 0, 0, "", 
                    TxtFechaInicio.Text, TxtFechaFin.Text, Session["Conectar"].ToString());

                GrdvEfectivas.DataSource = _dts.Tables[0];
                GrdvEfectivas.DataBind();
                ViewState["GrdvEfectivas"] = _dts.Tables[0];

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ImgExportar1.Visible = true;
                    LblExportar1.Visible = true;
                }

                GrdvMaxLlamada.DataSource = _dts.Tables[1];
                GrdvMaxLlamada.DataBind();

                if (_dts.Tables[1].Rows.Count > 0)
                {
                    ImgExportar2.Visible = true;
                    LblExportar2.Visible = true;
                }

                ViewState["GrdvMaximo"] = _dts.Tables[1];
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}