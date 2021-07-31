namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_MonitoreoLstAdmFixed : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        string _estado = "", _codigoltca = "", _codigogestor = "", _filename = "";
        ImageButton _imgselecc = new ImageButton();
        DataRow _resultado;
        int _porgestionar = 0;
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
                    ViewState["Tipo"] = Request["Tipo"];
                    ViewState["Gestor"] = Request["Gestor"];
                    ViewState["Consultar"] = "0";
                    ViewState["Estado"] = Request["Estado"];
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
                    Session["Conectar"].ToString());

                _dtb = _dts.Tables[0];
                ViewState["GrdvDatos"] = _dtb;

                foreach (DataRow drfila in _dtb.Rows)
                {
                    _dts = new ConsultaDatosDAO().FunGetMonitoreoAdmin(5, int.Parse(ViewState["CodigoCEDE"].ToString()),
                        int.Parse(ViewState["CodigoCPCE"].ToString()), int.Parse(drfila["CodigoLista"].ToString()),
                        0, 0, 0, ViewState["FechaDesde"].ToString(), ViewState["FechaHasta"].ToString(),
                        int.Parse(drfila["CodigoGestor"].ToString()), "", "", "", 0, 0, 0,
                        Session["Conectar"].ToString());

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
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
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

        protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigoltca = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoLista"].ToString();
                _codigogestor = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoGestor"].ToString();

                Response.Redirect("WFrm_MonitorDetalleLista.aspx?codigoLTCA=" + _codigoltca + "&codigoGestor=" + _codigogestor +
                    "&codigoCEDE=" + ViewState["CodigoCEDE"].ToString() + "&codigoCPCE=" + ViewState["CodigoCPCE"].ToString() +
                    "&Catalogo=" + ViewState["Catalogo"].ToString() + "&FechaDesde=" + ViewState["FechaDesde"].ToString() +
                    "&FechaHasta=" + ViewState["FechaHasta"].ToString() + "&Tipo=" + ViewState["Tipo"].ToString() + "&Gestor=" +
                    ViewState["Gestor"].ToString() + "&Estado=" + ViewState["Estado"].ToString(), false);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgselecc = (ImageButton)(e.Row.Cells[10].FindControl("ImgSelecc"));
                    _porgestionar = int.Parse(GrdvDatos.DataKeys[e.Row.RowIndex].Values["PorGestionar"].ToString());
                    _estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    _codigoltca = GrdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoLista"].ToString();

                    if (_porgestionar == 0)
                    {
                        e.Row.Cells[5].BackColor = Color.Red;
                        _imgselecc.ImageUrl = "~/Botones/Buscargris.png";
                        _imgselecc.Enabled = false;
                    }
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
        #endregion
    }
}