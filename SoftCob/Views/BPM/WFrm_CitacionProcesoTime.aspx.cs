namespace SoftCob.Views.BPM
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_CitacionProcesoTime : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _codigo = "", _codigoclde = "", _codigopers = "", _terreno = "", _email = "", _whastapp = "", _mensaje = "",
            _codigogest = "", _numdocumento = "";
        int _contar = 0;
        DataTable _dtb = new DataTable();
        Image _imgterreno = new Image();
        Image _imgemail = new Image();
        Image _imgwhastapp = new Image();
        ImageButton _imgcambiar = new ImageButton();
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
                    Lbltitulo.Text = "Notificaciones en Proceso";
                    FunCargarMantenimiento();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 2, 0, 0, "", "", "", Session["Conectar"].ToString());

                    _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                    if (_contar > 0)
                    {
                        _mensaje = "Tiene " + _contar + " NOTIFIACIONES(ES) NO PROCESADA(s)";
                        new FuncionesDAO().FunShowJSMessage(_mensaje, this, "W", "C");
                    }

                    if (Request["MensajeRetornado"] != null)
                    {
                        _mensaje = Request["MensajeRetornado"];
                        new FuncionesDAO().FunShowJSMessage(_mensaje, this, "S", "R");
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(247, 0, 0, 0, "", "CPR", "", Session["Conectar"].ToString());
                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
                ViewState["GrdvDatos"] = _dts.Tables[0];

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ImgExportar.Visible = true;
                    lblExportar.Visible = true;
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(248, 0, 0, 0, "", "", "", Session["Conectar"].ToString());

                _contar = int.Parse(_dts.Tables[0].Rows[0]["Citas"].ToString());

                if (_contar > 0)
                {
                    _mensaje = "Existe(n) " + _contar + _dts.Tables[0].Rows[0]["Citas"].ToString();
                    _mensaje += "Notificacion(s) en Proceso con fecha(s) fuera de rango";
                    new FuncionesDAO().FunShowJSMessage(_mensaje, this, "W", "C");
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
                _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();
                _codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
                _codigogest = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoGEST"].ToString();
                _numdocumento = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Identificacion"].ToString();

                Response.Redirect("WFrm_RegistrarCitacionTime.aspx?CodigoCITA=" + _codigo + "&CodigoPERS=" + _codigopers +
                    "&CodigoCLDE=" + _codigoclde + "&CodigoGEST=" + _codigogest + "&NumDocumento=" + _numdocumento +
                    "&Retornar=1", true);
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
                    _terreno = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Terreno"].ToString();
                    _email = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Email"].ToString();
                    _whastapp = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Whastapp"].ToString();
                    _imgterreno = (Image)(e.Row.Cells[5].FindControl("ImgTerreno"));
                    _imgemail = (Image)(e.Row.Cells[6].FindControl("ImgEmail"));
                    _imgwhastapp = (Image)(e.Row.Cells[7].FindControl("ImgWhastapp"));
                    _imgcambiar = (ImageButton)(e.Row.Cells[8].FindControl("ImgCambiar"));

                    if (_terreno == "SI")
                    {
                        _imgterreno.ImageUrl = "~/Botones/vistoverde.png";
                        _imgcambiar.ImageUrl = "~/Botones/btnnotepad.png";
                        _imgcambiar.Enabled = true;
                    }
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
                    string FileName = "Notificacones_Generadas_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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
        protected void ImgCambiar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCITA"].ToString();
            _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();

            _dts = new ConsultaDatosDAO().FunConsultaDatos(257, int.Parse(_codigo), int.Parse(_codigoclde), 0, "",
                "CGE", "", ViewState["Conectar"].ToString());

            Response.Redirect(Request.Url.AbsolutePath, true);
        }
    }
    #endregion
}