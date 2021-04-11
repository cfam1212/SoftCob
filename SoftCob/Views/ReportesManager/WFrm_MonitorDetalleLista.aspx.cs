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
    public partial class WFrm_MonitorDetalleLista : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ImgExportar);
            if (!IsPostBack)
            {
                ViewState["codigoLTCA"] = Request["codigoLTCA"];
                ViewState["codigoGestor"] = Request["codigoGestor"];
                ViewState["codigoCEDE"] = Request["codigoCEDE"];
                ViewState["codigoCPCE"] = Request["codigoCPCE"];
                ViewState["Catalogo"] = Request["Catalogo"];
                ViewState["FechaDesde"] = Request["FechaDesde"];
                ViewState["FechaHasta"] = Request["FechaHasta"];
                ViewState["Tipo"] = Request["Tipo"];
                ViewState["Gestor"] = Request["Gestor"];
                ViewState["Estado"] = Request["Estado"];
                SoftCob_USUARIO usuario = new ControllerDAO().FunGetUsuarioPorID(int.Parse(ViewState["codigoGestor"].ToString()));
                ViewState["NameGestor"] = usuario.usua_nombres + "_" + usuario.usua_apellidos;
                _dts = new ConsultaDatosDAO().FunConsultaDatos(23, int.Parse(ViewState["codigoLTCA"].ToString()), 0, 0, "", "", "", 
                    Session["Conectar"].ToString());
                Lbltitulo.Text = "Reporte Monitoreo Lista " + _dts.Tables[0].Rows[0]["ListaTrabajo"].ToString() + " - " + 
                    usuario.usua_nombres + " " + usuario.usua_apellidos;
                FunCargarMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunGetMonitoreoAdmin(2, int.Parse(ViewState["codigoCEDE"].ToString()),
                    int.Parse(ViewState["codigoCPCE"].ToString()), int.Parse(ViewState["codigoLTCA"].ToString()),
                    0, 0, 0, "", "", int.Parse(ViewState["codigoGestor"].ToString()), "", "", "", 1, 0, 0,
                    Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    ViewState["grdvDatos"] = GrdvDatos.DataSource;
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
                _dts = new ConsultaDatosDAO().FunGetMonitoreoAdmin(2, int.Parse(ViewState["codigoCEDE"].ToString()),
                    int.Parse(ViewState["codigoCPCE"].ToString()), int.Parse(ViewState["codigoLTCA"].ToString()),
                    0, 0, 0, "", "", int.Parse(ViewState["codigoGestor"].ToString()), "", "", "", 1, 0, 0,
                    Session["Conectar"].ToString());

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dts.Tables[0], "Datos");
                    string FileName = "Monitoreo_" + ViewState["NameGestor"].ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_MonitoreoLstAdmFixed.aspx?CodigoCEDE=" + ViewState["codigoCEDE"].ToString() + "&Catalogo=" +
                ViewState["Catalogo"].ToString() + "&CodigoCPCE=" + ViewState["codigoCPCE"].ToString() + "&FechaDesde=" +
                ViewState["FechaDesde"].ToString() + "&FechaHasta=" + ViewState["FechaHasta"].ToString() + "&Tipo=" +
                ViewState["Tipo"].ToString() + "&Gestor=" + ViewState["Gestor"].ToString() + "&Estado=" +
                ViewState["Estado"].ToString());
        }
        #endregion
    }
}