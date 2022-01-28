namespace SoftCob.Views.BPM
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_TablaPagosConvenio : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbpagos = new DataTable();
        decimal _totalpago = 0.00M;
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
                    ViewState["CodigoCITA"] = Request["CodigoCITA"];
                    Lbltitulo.Text = "Tabla de Acuerdo << TABLA DE AMORTIZACION >>";

                    FunCargarMantenimiento();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(263, int.Parse(ViewState["CodigoCITA"].ToString()),
                    0, 0, "", "", "", ViewState["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    LblFechaAcuerdo.Text = _dts.Tables[0].Rows[0]["FechaAcuerdo"].ToString();
                    LblNumDocu.Text = _dts.Tables[0].Rows[0]["NumDocu"].ToString();
                    LblNombres.Text = _dts.Tables[0].Rows[0]["Nombres"].ToString();
                    LblValorCita.Text = _dts.Tables[0].Rows[0]["ValorCita"].ToString();
                    LblDescuento.Text = _dts.Tables[0].Rows[0]["Descuento"].ToString();
                    LblPago.Text = _dts.Tables[0].Rows[0]["Pago"].ToString();
                    LblTipoPago.Text = _dts.Tables[0].Rows[0]["TipoPago"].ToString();

                    GrdvPagos.DataSource = _dts.Tables[1];
                    GrdvPagos.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        #endregion

        #region Botones y Eventos
        protected void GrdvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _totalpago += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ValorPago"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = "TOTAL:";
                    e.Row.Cells[2].Text = _totalpago.ToString();
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                    e.Row.Font.Size = 11;
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
                _dtbpagos = (DataTable)ViewState["TablaPagos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtbpagos, "Datos");
                    string FileName = "Tabla_Amortizacion" + ViewState["NumDocumento"].ToString() + "-" +
                        DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }
        #endregion
    }
}