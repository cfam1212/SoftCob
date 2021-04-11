namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteCarteraGestor : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        string _identificacion = "", _operacion = "", _codigopers = "", _codigoclde = "", _filename;
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
                ViewState["codigoUSU"] = Request["codigoUSU"];
                SoftCob_USUARIO usuario = new ControllerDAO().FunGetUsuarioPorID(int.Parse(ViewState["codigoUSU"].ToString()));
                ViewState["Gestor"] = usuario.usua_nombres + "_" + usuario.usua_apellidos;
                Lbltitulo.Text = "Reporte Cartera Gestor " + usuario.usua_nombres + " " + usuario.usua_apellidos;
                FunCargarMantenimiento();
            }
        }
        #endregion

        #region Procedimientos Y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunGerReporteConsolidado(4, int.Parse(ViewState["codigoCEDE"].ToString()),
                    int.Parse(ViewState["codigoCPCE"].ToString()), int.Parse(ViewState["codigoUSU"].ToString()), "", "",
                    0, 0, Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    ViewState["GrdvDatos"] = _dts.Tables[0];
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
                _dts = new ConsultaDatosDAO().FunGerReporteConsolidado(4, int.Parse(ViewState["codigoCEDE"].ToString()), int.Parse(ViewState["codigoCPCE"].ToString()), int.Parse(ViewState["codigoUSU"].ToString()), "", "", 0, 0, Session["Conectar"].ToString());
                _dtb = (DataTable)ViewState["GrdvDatos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    _filename = "Consolidado_" + ViewState["Gestor"].ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void ImgGestionar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _identificacion = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Identificacion"].ToString();
                _operacion = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Operacion"].ToString();
                _codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
                _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();
                Response.Redirect("../Gestion/WFrm_RegLlamadaEntrante.aspx?CodigoCEDE=" + ViewState["codigoCEDE"].ToString() +
                    "&CodigoCPCE=" + ViewState["codigoCPCE"].ToString() + "&CodigoCLDE=" + _codigoclde + "&CodigoPERS=" + _codigopers + "&NumeroDocumento=" + _identificacion + "&Operacion=" + _operacion + "&CodigoUSU=" + ViewState["codigoUSU"].ToString() + "&CodigoLTCA=0" + "&Retornar=0", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgCitacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
                _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();
                Response.Redirect("../BPM/WFrm_NuevaCitacion.aspx?CodigoPERS=" + _codigopers + "&CodigoCLDE="
                    + _codigoclde + "&codigoCEDE=" + ViewState["codigoCEDE"].ToString() + "&codigoCPCE=" +
                    ViewState["codigoCPCE"].ToString() + "&codigoUSU=" + ViewState["codigoUSU"].ToString() +
                    "&Retornar=3", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ReporteGestorCedente.aspx?codigoCEDE=" + ViewState["codigoCEDE"].ToString() + "&codigoCPCE=" + ViewState["codigoCPCE"].ToString(), true);
        }
        #endregion
    }
}