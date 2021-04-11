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
    public partial class WFrm_MonitorConsTimer : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        ListItem _itemc = new ListItem();
        Label _lblcalifica = new Label();
        string _filename = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            scriptManager.RegisterPostBackControl(ImgExportar);
            scriptManager.RegisterPostBackControl(ImgExportar1);

            if (!IsPostBack)
            {
                ViewState["Efectivas"] = "0";
                TxtFechaInicio.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Monitoreo Challenger - Tiempos - Gestión - Llamada";
                FunCargarCombos(0);
            }
            else
            {
                GrdvEfectivas.DataSource = ViewState["GrdvEfectivas"];
                GrdvNoEfectivas.DataSource = ViewState["GrdvNoEfectivas"];
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

                    break;
                case 1:
                    ViewState["Efectivas"] = "0";
                    GrdvEfectivas.DataSource = null;
                    GrdvEfectivas.DataBind();
                    GrdvNoEfectivas.DataSource = null;
                    GrdvNoEfectivas.DataBind();
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
            FunCargarCombos(1);
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

                if (DateTime.ParseExact(TxtFechaInicio.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(102, int.Parse(ViewState["codigoCPCE"].ToString()), 0, 0, "", TxtFechaInicio.Text, TxtFechaFin.Text, Session["Conectar"].ToString());
                ViewState["Efectivas"] = _dts.Tables[0].Rows.Count;
                GrdvEfectivas.DataSource = _dts.Tables[0];
                GrdvEfectivas.DataBind();

                ViewState["GrdvEfectivas"] = _dts.Tables[0];

                if (_dts.Tables[0].Rows.Count > 0) ImgExportar.Visible = true;

                GrdvNoEfectivas.DataSource = _dts.Tables[1];
                GrdvNoEfectivas.DataBind();

                if (_dts.Tables[1].Rows.Count > 0) ImgExportar1.Visible = true;

                ViewState["GrdvNoEfectivas"] = _dts.Tables[1];
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
                _dtb = (DataTable)ViewState["GrdvEfectivas"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    _filename = "MonitorConsTimerEfec_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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
                _dtb = (DataTable)ViewState["GrdvNoEfectivas"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    _filename = "MonitorConsTimerNoEfec_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void GrdvEfectivas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (int.Parse(ViewState["Efectivas"].ToString()) > 2)
                {
                    _lblcalifica = (Label)(e.Row.Cells[4].FindControl("lblCalifica"));

                    if (e.Row.RowIndex == 0)
                    {
                        _lblcalifica.Text = "EXECELENTE GESTION";
                        e.Row.Cells[0].BackColor = System.Drawing.Color.LawnGreen;
                        e.Row.Cells[4].BackColor = System.Drawing.Color.LawnGreen;
                    }

                    if (e.Row.RowIndex == int.Parse(ViewState["Efectivas"].ToString()) - 1)
                    {
                        _lblcalifica.Text = "PONER MAS EMPEÑO";
                        e.Row.Cells[0].BackColor = System.Drawing.Color.Red;
                        e.Row.Cells[4].BackColor = System.Drawing.Color.Red;
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
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}