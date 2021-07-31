namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteTelefonos : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        int _tiporep = 0, _tipomotivo = 0;
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
                    Lbltitulo.Text = "Reporte Acción Teléfonos";
                    FunCargarCombos(0);
                }
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
            switch (opcion)
            {
                case 0:
                    DdlGestor.DataSource = new ConsultaDatosDAO().FunConsultaDatos(136, 0, 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestor.DataTextField = "Descripcion";
                    DdlGestor.DataValueField = "Codigo";
                    DdlGestor.DataBind();
                    break;
                case 1:
                    PnlTelefonoEli.Visible = false;
                    PnlTelefonoMod.Visible = false;
                    GrdvTelefonoE.DataSource = null;
                    GrdvTelefonoE.DataBind();
                    GrdvTelefonoM.DataSource = null;
                    GrdvTelefonoM.DataBind();
                    ImgExportar.Visible = false;
                    break;
            }
        }

        #endregion

        #region Botones y Eventos
        protected void DdlBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
            TxtCriterio.Enabled = false;
            TxtCriterio.Text = "";
            if (DdlBuscar.SelectedValue != "T") TxtCriterio.Enabled = true;
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlTipoAccion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Acción..!", this, "W", "C");
                    return;
                }

                if (DdlTipoAccion.SelectedValue == "1")
                {
                    ViewState["ReporteTipo"] = "1";
                    _tiporep = 0;
                }

                if (DdlTipoAccion.SelectedValue == "2")
                {
                    ViewState["ReporteTipo"] = "2";
                    _tiporep = 1;
                    _tipomotivo = 7;
                }

                if (DdlTipoAccion.SelectedValue == "3")
                {
                    ViewState["ReporteTipo"] = "2";
                    _tiporep = 1;
                    _tipomotivo = 99;
                }

                _dts = new ConsultaDatosDAO().FunReporteAccionTelefono(_tiporep, int.Parse(DdlGestor.SelectedValue),
                    DdlBuscar.SelectedValue, TxtCriterio.Text.Trim(), ChkPorFecha.Checked ? 1 : 0, TxtFechaIni.Text.Trim(),
                    TxtFechaFin.Text.Trim(), _tipomotivo, "", "", "", 0, 0, 0, Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0) ImgExportar.Visible = true;

                if (_tiporep == 0)
                {
                    PnlTelefonoEli.Visible = true;
                    GrdvTelefonoE.DataSource = _dts;
                    GrdvTelefonoE.DataBind();
                    Session["GrdvDatosE"] = _dts.Tables[0];
                }

                if (_tiporep == 1)
                {
                    PnlTelefonoMod.Visible = true;
                    GrdvTelefonoM.DataSource = _dts;
                    GrdvTelefonoM.DataBind();
                    Session["GrdvDatosM"] = _dts.Tables[0];
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvTelefonoE_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdvTelefonoE.PageIndex = e.NewPageIndex;
            GrdvTelefonoE.DataBind();
        }

        protected void DdlTipoAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void DdlGestor_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void GrdvTelefonoM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdvTelefonoM.PageIndex = e.NewPageIndex;
            GrdvTelefonoM.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ViewState["ReporteTipo"].ToString() == "1")
                    _dtb = (DataTable)Session["GrdvDatosE"];

                if (ViewState["ReporteTipo"].ToString() == "2")
                    _dtb = (DataTable)Session["GrdvDatosM"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    _fileName = "Telefonos_" + DdlTipoAccion.SelectedItem.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }

        protected void ChkPorFecha_CheckedChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }
        #endregion
    }
}