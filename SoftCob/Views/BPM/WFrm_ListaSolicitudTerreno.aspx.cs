namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using ClosedXML.Excel;
    using System.IO;

    public partial class WFrm_ListaSolicitudTerreno : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _codigo = "", _codigoclde = "", _codigopers = "", _codigocpce = "", _codigogest = "", _codigoesta = "";
        DataTable _dtb = new DataTable();
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
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    Lbltitulo.Text = "Lista Citaciones Solicitadas << VARIOS MEDIOS - TERRENO >>";
                    FunCargarMantenimiento();

                    if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page,
                        "::GSBPO GLOBAL SERVICES::", Request["MensajeRetornado"].ToString());
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(241, 1, 0, 0, "", "", "", ViewState["Conectar"].ToString());

                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
                ViewState["GrdvDatos"] = _dts.Tables[0];

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ImgExportar.Visible = true;
                    LblExportar.Visible = true;
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ImgCitacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCITA"].ToString();
                _codigocpce = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCPCE"].ToString();
                _codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
                _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();
                _codigogest = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoGEST"].ToString();

                Response.Redirect("WFrm_EstablecerFechaCitacion.aspx?CodigoCITA=" + _codigo + "&CodigoPERS=" + _codigopers +
                    "&CodigoCLDE=" + _codigoclde + "&CodigoCPCE=" + _codigocpce + "&CodigoGEST=" + _codigogest +
                    "&Retornar=1", true);
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
                _dtb = (DataTable)ViewState["GrdvDatos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    string FileName = "SolicitudCitaciones_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _codigoesta = GrdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoESTA"].ToString();

                    switch (_codigoesta)
                    {
                        case "REV":
                            e.Row.Cells[0].BackColor = System.Drawing.Color.Tomato;
                            break;
                    }
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
