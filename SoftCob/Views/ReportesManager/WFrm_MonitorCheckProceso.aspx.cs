namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_MonitorCheckProceso : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        string _estado = "", _codigoltca = "", _filename = "";
        ImageButton _imgselecc = new ImageButton();
        CheckBox _chkselecc = new CheckBox();
        DataRow _resultado;
        int _porgestionar = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
                scriptManager.RegisterPostBackControl(ImgExportar);

                if (!IsPostBack)
                {
                    ViewState["CodigoCEDE"] = Request["CodigoCEDE"];
                    ViewState["Catalogo"] = Request["Catalogo"];
                    ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                    ViewState["FechaDesde"] = Request["FechaDesde"];
                    ViewState["FechaHasta"] = Request["FechaHasta"];
                    ViewState["Tipo"] = Request["Tipo"];
                    ViewState["Gestor"] = Request["Gestor"];
                    ViewState["Consultar"] = "0";
                    ViewState["Estado"] = Request["Estado"];
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    Lbltitulo.Text = "Tablero de Control Monitoreo Listas << " + ViewState["Catalogo"].ToString() + " >>";
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
                _dts = new ConsultaDatosDAO().FunGetMonitoreoAdmin(int.Parse(ViewState["Tipo"].ToString()),
                    int.Parse(ViewState["CodigoCEDE"].ToString()), int.Parse(ViewState["CodigoCPCE"].ToString()),
                    0, 0, 0, 0, ViewState["FechaDesde"].ToString(), ViewState["FechaHasta"].ToString(),
                    int.Parse(ViewState["Gestor"].ToString()), "", "", "", int.Parse(ViewState["Estado"].ToString()), 0, 0,
                    ViewState["Conectar"].ToString());

                _dtb = _dts.Tables[0];

                foreach (DataRow drfila in _dts.Tables[0].Rows)
                {
                    _dts = new ConsultaDatosDAO().FunGetMonitoreoAdmin(5, int.Parse(ViewState["CodigoCEDE"].ToString()),
                        int.Parse(ViewState["CodigoCPCE"].ToString()), int.Parse(drfila["CodigoLista"].ToString()),
                        0, 0, 0, ViewState["FechaDesde"].ToString(), ViewState["FechaHasta"].ToString(),
                        int.Parse(drfila["CodigoGestor"].ToString()), "", "", "", 0, 0, 0,
                        ViewState["Conectar"].ToString());

                    _resultado = _dtb.Select("CodigoLista='" + drfila["CodigoLista"].ToString() + "' and CodigoGestor='" +
                        drfila["CodigoGestor"].ToString() + "'").FirstOrDefault();

                    _resultado["Operaciones"] = _dts.Tables[0].Rows[0]["Operaciones"].ToString();
                    _resultado["PorGestionar"] = _dts.Tables[0].Rows[0]["PorGestionar"].ToString();
                    _resultado["Efectivas"] = _dts.Tables[0].Rows[0]["Efectivas"].ToString();
                    _resultado["Estado"] = _dts.Tables[0].Rows[0]["Estado"].ToString();
                    _resultado["UltimaFecha"] = _dts.Tables[0].Rows[0]["UltimaFecha"].ToString();
                    _dtb.AcceptChanges();
                }

                GrdvDatos.DataSource = _dtb;
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunActualizarCheck(string activar)
        {

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
                    _filename = "Reporte_ListasTrabajo_" + ViewState["Catalogo"].ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            ViewState["Consultar"] = "1";
            Response.Redirect("WFrm_MonitoreoLstAdmin.aspx?CodigoCEDE=" + ViewState["CodigoCEDE"].ToString() +
                "&CodigoCPCE=" + ViewState["CodigoCPCE"].ToString() + "&FechaDesde=" + ViewState["FechaDesde"].ToString() +
                "&FechaHasta=" + ViewState["FechaHasta"].ToString() + "&Gestor=" + ViewState["Gestor"].ToString(), true);
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _porgestionar = int.Parse(GrdvDatos.DataKeys[e.Row.RowIndex].Values["PorGestionar"].ToString());
                    _estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    _codigoltca = GrdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoLista"].ToString();

                    if (_porgestionar == 0) e.Row.Cells[5].BackColor = Color.Red;

                    if (_porgestionar > 0 && _porgestionar < 50) e.Row.Cells[5].BackColor = Color.Coral;

                    if (_porgestionar > 50 && _porgestionar <= 100) e.Row.Cells[5].BackColor = Color.Silver;

                    if (_porgestionar > 100 && _porgestionar <= 500) e.Row.Cells[5].BackColor = Color.Beige;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void Chckchanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chckheader = (CheckBox)GrdvDatos.HeaderRow.FindControl("Chkall");

                foreach (GridViewRow row in GrdvDatos.Rows)
                {
                    _chkselecc = (CheckBox)row.FindControl("Chkselecc");

                    if (chckheader.Checked == true) _chkselecc.Checked = true;
                    else _chkselecc.Checked = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void Chkselecc_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion
    }
}