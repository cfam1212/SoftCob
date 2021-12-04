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

    public partial class WFrm_ListaSolicitudGestores : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _codigo = "", _codigoclde = "", _codigopers = "", _codigocpce = "", _codigogest = "", _codigoesta = "",
            _mensaje = "", _numerdocumento = "";
        DataTable _dtb = new DataTable();
        int _contar = 0;
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
                    Lbltitulo.Text = "Lista Citaciones Solicitadas << VARIOS MEDIOS >>";
                    FunCargarMantenimiento();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 1, 0, 0, "", "", "",
                        ViewState["Conectar"].ToString());

                    _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                    if (_contar > 0)
                    {
                        _mensaje = "Tiene " + _contar + " NOTIFICACION(ES) En PROCESO";
                    }
                    else
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 4, 0, 0, "", "", "",
                            Session["Conectar"].ToString());

                        _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                        if (_contar > 0)
                        {
                            _mensaje = "Tiene " + _contar + " NOTIFICACION(ES) TERRENO para GESTIONAR";
                        }
                        else
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 5, 0, 0, "", "", "",
                                Session["Conectar"].ToString());

                            _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                            if (_contar > 0)
                            {
                                _mensaje = "Tiene " + _contar + " NOTIFICACION(ES) EMAIL para GESTIONAR";
                            }
                            else
                            {
                                _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 2, 0, 0, "", "", "",
                                    Session["Conectar"].ToString());

                                _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                                if (_contar > 0)
                                {
                                    _mensaje = "Tiene " + _contar + " NOTIFICACION(ES) que no fueron PROCESADA(s)";
                                }
                            }
                        }
                    }

                    if (_contar > 0) new FuncionesDAO().FunShowJSMessage(_mensaje, this, "W", "L");

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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(241, 0, 0, 0, "", "CSL", "", ViewState["Conectar"].ToString());

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
                _numerdocumento = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Identificacion"].ToString();

                Response.Redirect("WFrm_RegistrarCitacionTime.aspx?CodigoCITA=" + _codigo + "&CodigoPERS=" + _codigopers +
                    "&CodigoCLDE=" + _codigoclde + "&NumDocumento=" + _numerdocumento + "&CodigoGEST=" + _codigogest +
                    "&Retornar=0", true);
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
                "CCF", "", Session["Conectar"].ToString());

            _dts = new ConsultaDatosDAO().FunConsultaDatos(261, int.Parse(_codigo), int.Parse(Session["usuCodigo"].ToString()), 0,
                "NOTIFICACION CAMBIADA DE FECHA", "CCF", Session["MachineName"].ToString(), Session["Conectar"].ToString());

            Response.Redirect(Request.Url.AbsolutePath, true);
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
                            e.Row.Cells[0].BackColor = System.Drawing.Color.GreenYellow;
                            break;
                    }
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
                    string FileName = "SolicitudNotificacion_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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
        #endregion
    }
}