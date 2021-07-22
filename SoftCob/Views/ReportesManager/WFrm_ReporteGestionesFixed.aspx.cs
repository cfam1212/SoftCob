namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteGestionesFixed : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
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
                    ViewState["CodigoCEDE"] = Request["CodigoCEDE"];
                    ViewState["Catalogo"] = Request["Catalogo"];
                    ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                    ViewState["FechaDesde"] = Request["FechaDesde"];
                    ViewState["FechaHasta"] = Request["FechaHasta"];
                    ViewState["BuscarPor"] = Request["BuscarPor"];
                    ViewState["Criterio"] = Request["Criterio"];
                    ViewState["Gestor"] = Request["Gestor"];
                    Lbltitulo.Text = "Reporte Gestiones << " + ViewState["Catalogo"].ToString() + " >>";
                    FunCargarMantenimiento();
                }
                else GrdvDatos.DataSource = Session["GrdvDatos"];
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos Y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunGetRerporteGestiones(0, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(ViewState["CodigoCPCE"].ToString()), ViewState["FechaDesde"].ToString(),
                    ViewState["FechaHasta"].ToString(), ViewState["BuscarPor"].ToString(), ViewState["Criterio"].ToString(),
                    "", "", int.Parse(ViewState["Gestor"].ToString()), 0, Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ImgExportar.Visible = true;
                    LblExportar.Visible = true;
                    ViewState["GrdvDatos"] = _dts.Tables[0];
                    GrdvDatos.DataSource = _dts.Tables[0];
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
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
                _dtb = (DataTable)ViewState["GrdvDatos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    string FileName = "Reporte_Gestiones_" + ViewState["Catalogo"].ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ReporteGestiones.aspx?CodigoCEDE=" + ViewState["CodigoCEDE"].ToString() +
                "&CodigoCPCE=" + ViewState["CodigoCPCE"].ToString() + "&FechaDesde=" + ViewState["FechaDesde"].ToString() +
                "&FechaHasta=" + ViewState["FechaHasta"].ToString() + "&BuscarPor=" + ViewState["BuscarPor"].ToString() +
                "&Criterio=" + ViewState["Criterio"].ToString() + "&Gestor=" + ViewState["Gestor"].ToString(), true);
        }

        protected void GrdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdvDatos.PageIndex = e.NewPageIndex;
            GrdvDatos.DataBind();
        }
        #endregion
    }
}