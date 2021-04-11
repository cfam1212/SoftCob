namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReportesVarios : Page
    {
        #region Variables
        DataTable _dtb = new DataTable();
        DataSet _dts = new DataSet();
        string _fileName = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.ImgExportar);

                if (!IsPostBack)
                {
                    TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    Lbltitulo.Text = "Reportes Varios";
                }
                else GrdvDatos.DataSource = ViewState["GrdvDatos"];
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(136, 0, 0, 0, "", "", "", Session["Conectar"].ToString());
                        DdlGestor.DataSource = _dts;
                        DdlGestor.DataTextField = "Descripcion";
                        DdlGestor.DataValueField = "Codigo";
                        DdlGestor.DataBind();
                        break;
                    case 1:
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(166, 0, 0, 0, "", "", "", Session["Conectar"].ToString());
                        DdlGestor.DataSource = _dts;
                        DdlGestor.DataTextField = "Descripcion";
                        DdlGestor.DataValueField = "Codigo";
                        DdlGestor.DataBind();
                        break;
                    case 99:
                        PnlDatos.Visible = false;
                        GrdvDatos.DataSource = null;
                        GrdvDatos.DataBind();
                        LblExportar.Visible = false;
                        ImgExportar.Visible = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlTipoReporte.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Reporte..!", this);
                    return;
                }

                ImgExportar.Visible = false;
                LblExportar.Visible = false;
                switch (DdlTipoReporte.SelectedValue)
                {
                    case "1":
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(81, int.Parse(DdlGestor.SelectedValue), 0, 0, "", TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), Session["Conectar"].ToString());
                        break;
                    case "2":
                        if (DdlGestor.SelectedValue == "0")
                        {
                            new FuncionesDAO().FunShowJSMessage("Seleccione Evaluación..!", this);
                            return;
                        }

                        _dts = new ConsultaDatosDAO().FunConsultaDatos(167, int.Parse(DdlGestor.SelectedValue), 0, 0, "", TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), Session["Conectar"].ToString());
                        break;
                    case "3":
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(197, int.Parse(DdlGestor.SelectedValue), 0, 0, "", TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), Session["Conectar"].ToString());
                        break;
                    case "4":
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(204, 0, 0, 0, "", TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), Session["Conectar"].ToString());
                        break;
                }

                if (_dts.Tables[0].Rows.Count == 0)
                {
                    FunCargarCombos(99);
                    new FuncionesDAO().FunShowJSMessage("No Existen Datos..!", this);
                }
                else
                {
                    PnlDatos.Visible = true;
                    ImgExportar.Visible = true;
                    LblExportar.Visible = true;
                    ViewState["GrdvDatos"] = _dts.Tables[0];
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["GrdvDatos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    _fileName = "Reportes_Varios_" + DdlTipoReporte.SelectedItem.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + _fileName);
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

        protected void DdlTipoReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DdlGestor.SelectedValue = "0";
                FunCargarCombos(99);
                switch (DdlTipoReporte.SelectedValue)
                {
                    case "1":
                        LblTipoOPC.InnerText = "Por Gestor:";
                        FunCargarCombos(0);
                        break;
                    case "2":
                        LblTipoOPC.InnerText = "Evaluación:";
                        FunCargarCombos(1);
                        break;
                    case "3":
                        LblTipoOPC.InnerText = "Por Gestor:";
                        FunCargarCombos(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                LblExportar.Text = ex.ToString();
            }
        }

        protected void DdlGestor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(99);
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