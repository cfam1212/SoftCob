namespace SoftCob.Views.ReportesManager
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteOperaHistorico : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _sql = "", _filename = "", _style = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ImgExportar);
            if (!IsPostBack)
            {
                ViewState["Fecha"] = Request["Fecha"];
                ViewState["Cedente"] = Request["Cedente"];
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                FunCargarMantenimiento();
            }
            else GrdvDatos.DataSource = Session["GrdvDatos"];
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            _sql = "select Identificacion = hiop_identificacion,Operacion = hiop_operacion,Dias_Mora = hiop_diasmora,Exigible = ROUND(hiop_valorexigible,2),";
            _sql += "Total_Deuda = ROUND(hiop_totaldeuda,2) from ENTERPRISE_Cedentes..HISTORICO_" + ViewState["Cedente"].ToString();
            _sql += " where hiop_fechaproceso=convert(date,'" + ViewState["Fecha"].ToString() + "',103)";
            _dts = new ConsultaDatosDAO().FunGetRerporteGestiones(1, 0, 0, "", "", "", "", _sql, "", 0, 0, ViewState["Conectar"].ToString());
            Lbltitulo.Text = "Reporte Operaciones Historico ";
            GrdvDatos.DataSource = _dts;
            GrdvDatos.DataBind();

            if (_dts.Tables[0].Rows.Count > 0)
            {
                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
                Session["GrdvDatos"] = GrdvDatos.DataSource;
                //grdvDatos.UseAccessibleHeader = true;
                //grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            Session["GrdvDatos"] = GrdvDatos.DataSource;
        }
        #endregion

        #region Botones y Eventos
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Clear();
                GrdvDatos.AllowPaging = false;
                GrdvDatos.DataSource = (DataSet)Session["GrdvDatos"];
                GrdvDatos.DataBind();
                _filename = "Operaciones_Historico" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + _filename);
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                Response.Buffer = false;
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    foreach (GridViewRow row in GrdvDatos.Rows)
                    {
                        row.Cells[0].Style.Add("mso-number-format", "\\@");
                        row.Cells[1].Style.Add("mso-number-format", "\\@");
                    }
                    _style = @"<style> .textmode { } </style>";
                    Response.Write(_style);
                    GrdvDatos.RenderControl(hw);
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

        protected void GrdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdvDatos.PageIndex = e.NewPageIndex;
            GrdvDatos.DataBind();
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ReporteHistoricos.aspx", true);
        }

        #endregion
    }
}