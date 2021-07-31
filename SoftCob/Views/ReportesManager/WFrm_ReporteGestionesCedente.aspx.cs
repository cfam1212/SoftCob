namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteGestionesCedente : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        string _filename = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.ImgExportar);
                if (!IsPostBack)
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(52, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                    ViewState["CodigoCEDE"] = _dts.Tables[0].Rows[0]["Codigo"].ToString();
                    ViewState["CodigoCPCE"] = _dts.Tables[0].Rows[0]["CodigoCPCE"].ToString();
                    TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    Lbltitulo.Text = "Reporte Gestiones";
                    FunCargarCombos(0);
                }
                else GrdvDatos.DataSource = Session["GrdvDatos"];
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimiento y Funciones
        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlGestor.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", 
                        int.Parse(ViewState["CodigoCEDE"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestor.DataTextField = "Descripcion";
                    DdlGestor.DataValueField = "Codigo";
                    DdlGestor.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtBuscarPor.Text = "";
            TxtBuscarPor.Enabled = true;
            switch (DdlBuscar.SelectedValue)
            {
                case "0":
                    TxtBuscarPor.Enabled = false;
                    break;
            }
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!new FuncionesDAO().IsDate(TxtFechaIni.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this, "E", "C");
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaFin.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this, "E", "C");
                    return;
                }

                if (DateTime.ParseExact(TxtFechaIni.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this, "E", "C");
                    return;
                }

                if (DdlBuscar.SelectedItem.ToString() != "Todo" && string.IsNullOrEmpty(TxtBuscarPor.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese valor de Operación o Identificación..!", this, "W", "C");
                    return;
                }

                _dts = new ConsultaDatosDAO().FunGetRerporteGestiones(0, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(ViewState["CodigoCPCE"].ToString()), ViewState["FechaDesde"].ToString(),
                    ViewState["FechaHasta"].ToString(), ViewState["BuscarPor"].ToString(), ViewState["Criterio"].ToString(),
                    "", "", int.Parse(ViewState["Gestor"].ToString()), 0, Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ImgExportar.Visible = true;
                    LblExportar.Visible = true;
                    Session["GrdvDatos"] = _dts.Tables[0];
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                }
                else new FuncionesDAO().FunShowJSMessage("No Existen Datos para Mostrar..!", this, "E", "C");
            }
            catch (Exception)
            {
                Lblerror.Text = "Existe un error no interceptable.. Consulte con el Administrador del Sistema";
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
                _dtb = (DataTable)ViewState["GrdvDatos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    _filename = "Reporte_Gestiones_" + ViewState["Catalogo"].ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void GrdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdvDatos.PageIndex = e.NewPageIndex;
            GrdvDatos.DataBind();
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }

        #endregion
    }
}