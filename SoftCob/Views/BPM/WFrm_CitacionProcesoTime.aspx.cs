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
    public partial class WFrm_CitacionProcesoTime : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _codigo = "", _codigoclde = "", _codigopers = "", _terreno = "", _email = "", _whastapp = "", _mensaje = "";
        DataTable _dtb = new DataTable();
        Image _imgterreno = new Image();
        Image _imgemail = new Image();
        Image _imgwhastapp = new Image();
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
                    Lbltitulo.Text = "Citaciones en Proceso";
                    FunCargarMantenimiento();

                    //if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page,
                    //    "::GSBPO GLOBAL SERVICES::", Request["MensajeRetornado"].ToString());
                    if (Request["MensajeRetornado"] != null)
                    {
                        _mensaje = Request["MensajeRetornado"];
                        ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                            "'top-center'); alertify.success('" + _mensaje + "', 5, function(){console.log('dismissed');});", true);
                    }
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(247, 0, 0, 0, "", "", "", ViewState["Conectar"].ToString());
                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
                ViewState["GrdvDatos"] = _dts.Tables[0];

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(248, 0, 0, 0, "", "", "", ViewState["Conectar"].ToString());

                if (_dts.Tables[0].Rows[0]["Citas"].ToString() != "0")
                {
                    if (Request["MensajeRetornado"] == null)
                        SIFunBasicas.Basicas.PresentarMensaje(Page,
                            "::GSBPO GLOBAL SERVICES::", "Existe(n) " + _dts.Tables[0].Rows[0]["Citas"].ToString() +
                            " Cita(s) en Proceso con fecha(s) fuera de rango");
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
                _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();
                _codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
                Response.Redirect("WFrm_GenerarCitacion.aspx?Codigo=" + _codigo + "&CodigoPERS=" + _codigopers +
                    "&CodigoCLDE=" + _codigoclde, true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgCambiar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCITA"].ToString();

            _dts = new ConsultaDatosDAO().FunConsultaDatos(247, int.Parse(_codigo), 0, 0, "", "", "",
                ViewState["Conectar"].ToString());

            Response.Redirect(Request.Url.AbsolutePath, true);
        }
        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _terreno = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Terreno"].ToString();
                    _email = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Email"].ToString();
                    _whastapp = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Whastapp"].ToString();
                    _imgterreno = (Image)(e.Row.Cells[5].FindControl("ImgTerreno"));
                    _imgemail = (Image)(e.Row.Cells[6].FindControl("ImgEmail"));
                    _imgwhastapp = (Image)(e.Row.Cells[7].FindControl("ImgWhastapp"));

                    if(_terreno=="SI") _imgterreno.ImageUrl= "~/Botones/vistoverde.png";
                    if (_email == "SI") _imgemail.ImageUrl = "~/Botones/vistoverde.png";
                    if (_whastapp == "SI") _imgwhastapp.ImageUrl = "~/Botones/vistoverde.png";
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
                _dtb = (DataTable)ViewState["GrdvDatos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    string FileName = "CitacionesGeneradas_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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
    }
    #endregion
}