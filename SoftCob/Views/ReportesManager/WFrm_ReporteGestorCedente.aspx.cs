namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteGestorCedente : Page
    {
        #region Variables
        int _totalOperaciones = 0;
        decimal _totalSaldos = 0.00M;
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        string _codigo = "", _filename = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ImgExportar);
            if (!IsPostBack)
            {
                ViewState["codigoCEDE"] = Request["codigoCEDE"];
                ViewState["codigoCPCE"] = Request["codigoCPCE"];
                _dts = new ConsultaDatosDAO().FunConsultaDatos(58, int.Parse(ViewState["codigoCPCE"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                Lbltitulo.Text = "Reporte Catálogo/Producto: " + _dts.Tables[0].Rows[0]["Descripcion"].ToString();
                ViewState["Cedente"] = _dts.Tables[0].Rows[0]["Descripcion"].ToString();
                FunCargarMantenimiento();
            }
        }
        #endregion

        #region Procedimientos yFunciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunGerReporteConsolidado(2, int.Parse(ViewState["codigoCEDE"].ToString()), int.Parse(ViewState["codigoCPCE"].ToString()), 0, "", "", 0, 0, Session["Conectar"].ToString());
                if (_dts.Tables[0].Rows.Count > 0)
                {
                    _dtb = _dts.Tables[0];
                    _totalOperaciones = int.Parse(_dtb.Compute("Sum(Operaciones)", "").ToString());
                    _totalSaldos = decimal.Parse(_dtb.Compute("Sum(SumSaldo)", "").ToString());
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    ViewState["grdvDatos"] = GrdvDatos.DataSource;
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                    lblOperaciones.InnerText = _totalOperaciones.ToString("##,###.##");
                    lblSaldos.InnerText = "$" + string.Format("{0:n}", _totalSaldos);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void Btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["codigoUSU"].ToString();
                Response.Redirect("WFrm_ReporteCarteraGestor.aspx?codigoCEDE=" + ViewState["codigoCEDE"].ToString() +
                    "&codigoCPCE=" + ViewState["codigoCPCE"].ToString() + "&codigoUSU=" + _codigo);
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
                _dts = new ConsultaDatosDAO().FunGerReporteConsolidado(3, int.Parse(ViewState["codigoCEDE"].ToString()), 
                    int.Parse(ViewState["codigoCPCE"].ToString()), 0, "", "", 0, 0, Session["Conectar"].ToString());
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dts.Tables[0], "Datos");
                    _filename = "Consolidado_" + ViewState["Cedente"].ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + 
                        ".xlsx";
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

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ReporteConsolidado.aspx", true);
        }
        #endregion
    }
}