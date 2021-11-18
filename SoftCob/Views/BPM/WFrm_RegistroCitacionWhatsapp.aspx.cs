namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_RegistroCitacionWhatsapp : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        CheckBox _chkenviado = new CheckBox();
        DropDownList _ddlrepuesta = new DropDownList();
        ImageButton _imgeliminar = new ImageButton();
        string _ext = "", _path = "", _enviado = "", _celular = "", _redirect = "", _celularcomparar = "", _respuesta = "",
            _codigohcit = "";
        decimal _totalExigible = 0.00M, _totalDeuda = 0.00M;
        int _totalregistros = 0, _contar = 0;
        DataTable _dtbcitaciones = new DataTable();
        DataTable _tblagre = new DataTable();
        TextBox _txtfecha = new TextBox();
        DataRow _result;
        DataRow[] _resultado;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");

                Page.Form.Attributes.Add("enctype", "multipart/form-data");

                if (!IsPostBack)
                {
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    ViewState["CodigoCITA"] = Request["CodigoCITA"];
                    ViewState["CodigoPERS"] = Request["CodigoPERS"];
                    ViewState["CodigoCLDE"] = Request["CodigoCLDE"];
                    ViewState["NumDocumento"] = Request["NumDocumento"];

                    _dtbcitaciones.Columns.Add("CodigoCITA");
                    _dtbcitaciones.Columns.Add("Canal");
                    ViewState["Citaciones"] = _dtbcitaciones;

                    Lbltitulo.Text = "Establecer Fecha Citacion";
                    PnlDatosDeudor.Height = 100;
                    PnlDatosGetion.Height = 120;
                    PnlDatosGarante.Height = 120;
                    PnlCitaciones.Height = 230;
                    FunCargaMantenimiento();

                    if (Request["MensajeRetornado"] != null) new FuncionesDAO().FunShowJSMessage(Request["MensajeRetornado"], this,
                        "W", "C");
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(32, int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0,
                    "", "", "", ViewState["Conectar"].ToString().ToString());

                GrdvDatosDeudor.DataSource = _dts;
                GrdvDatosDeudor.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(33, 0, 0, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    "", "", "", ViewState["Conectar"].ToString());

                GrdvDatosObligacion.DataSource = _dts;
                GrdvDatosObligacion.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(45, 0, 0, 0, "", ViewState["NumDocumento"].ToString(), "",
                    ViewState["Conectar"].ToString().ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    TrGarantes.Visible = true;
                    GrdvDatosGarante.DataSource = _dts;
                    GrdvDatosGarante.DataBind();
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(246, 3, int.Parse(ViewState["CodigoCITA"].ToString()),
                    int.Parse(Session["usuCodigo"].ToString()), "", "Whatsapp", "", ViewState["Conectar"].ToString());

                ViewState["Citaciones"] = _dts.Tables[0];
                GrdvCitaciones.DataSource = _dts;
                GrdvCitaciones.DataBind();

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones
        protected void GrdvDatosObligacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _totalExigible += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Exigible"));
                    _totalDeuda += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ValorDeuda"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = "TOTAL:";
                    e.Row.Cells[3].Text = _totalDeuda.ToString();
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[4].Text = _totalExigible.ToString();
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;
                }
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(242, int.Parse(ViewState["CodigoCITA"].ToString()), 0,
                    0, "", "", "", ViewState["Conectar"].ToString());

                _path = _dts.Tables[0].Rows[0]["Ruta"].ToString();
                _ext = _dts.Tables[0].Rows[0]["Extension"].ToString();

                switch (_ext)
                {
                    case "png":
                    case "jpg":
                    case "jpeg":
                    case "bmp":
                    case "gif":
                        ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                            "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_DocumentosView.aspx?Path=" + _path +
                            "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=550px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
                        break;
                    case "pdf":
                        ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                            "posicion_y=(screen.height/2)-(600/2); window.open('WFrm_ViewPdf.aspx?Path=" + _path +
                            "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=980px, height=550px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEnviar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                _chkenviado = (CheckBox)(_gvRow.Cells[4].FindControl("ChkEnviar"));
                _celular = GrdvCitaciones.DataKeys[_gvRow.RowIndex].Values["Celular"].ToString();

                _dtb = (DataTable)ViewState["Citaciones"];
                _result = _dtb.Select("Celular='" + _celular + "'").FirstOrDefault();
                _result["Enviado"] = _chkenviado.Checked ? "SI" : "NO";
                _dtb.AcceptChanges();
                ViewState["Citaciones"] = _dtb;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvCitaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList _ddlrespuesta = (e.Row.FindControl("DdlRespuesta") as DropDownList);
                    _dts = new ControllerDAO().FunGetParametroDetalle("TIPO WHATSAPP", "--Seleccione Opcion--", "S");

                    _ddlrespuesta.DataSource = _dts;
                    _ddlrespuesta.DataTextField = "Descripcion";
                    _ddlrespuesta.DataValueField = "Codigo";
                    _ddlrespuesta.DataBind();
                }

                if (e.Row.RowIndex >= 0)
                {
                    _chkenviado = (CheckBox)(e.Row.Cells[3].FindControl("ChkEnviar"));
                    _ddlrepuesta = (DropDownList)(e.Row.Cells[4].FindControl("DdlRespuesta"));
                    _imgeliminar = (ImageButton)(e.Row.Cells[5].FindControl("ImgEliminar"));
                    _enviado = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["Enviado"].ToString();
                    _respuesta = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["Respuesta"].ToString();

                    if (_enviado == "SEND")
                    {
                        _chkenviado.Checked = true;
                        _chkenviado.Enabled = false;
                        _ddlrepuesta.Enabled = true;

                        _imgeliminar.ImageUrl = "~/Botones/eliminar.png";
                        _imgeliminar.Enabled = true;
                        _imgeliminar.Height = 15;
                    }
                    _ddlrepuesta.SelectedValue = _respuesta;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                _celular = GrdvCitaciones.DataKeys[_gvRow.RowIndex].Values["Celular"].ToString();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(256, int.Parse(ViewState["CodigoCITA"].ToString()),
                    int.Parse(Session["usuCodigo"].ToString()), int.Parse(Session["CedeCodigo"].ToString()), "",
                    ViewState["NumDocumento"].ToString(), _celular, ViewState["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(246, 3, int.Parse(ViewState["CodigoCITA"].ToString()),
                    int.Parse(Session["usuCodigo"].ToString()), "", "Whatsapp", "", ViewState["Conectar"].ToString());

                ViewState["Citaciones"] = _dts.Tables[0];
                GrdvCitaciones.DataSource = _dts;
                GrdvCitaciones.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["Citaciones"];
                _resultado = _dtb.Select("Enviado='SI'");

                _totalregistros = _dtb.Rows.Count;

                if (_resultado.Count() > 0)
                {
                    foreach (DataRow _drfila in _resultado)
                    {
                        _dts = new ConsultaDatosDAO().FunAgendaCitaciones(13, 0, 0, "", 0, "0", 0, "WHATSAPP ENVIADO",
                            "", "", "", "", _drfila["Celular"].ToString(), "", "", "", "", new byte[0], "", "", "", "",
                            "0", "0", 0, "", 0, "SEND", "Whatsapp", "WEV", "", "", "", "", "", "", "",
                            int.Parse(ViewState["CodigoCITA"].ToString()), 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                            Session["MachineName"].ToString(), ViewState["Conectar"].ToString());
                    }
                }

                _resultado = _dtb.Select("Enviado='SEND'");

                foreach (DataRow _drfila in _resultado)
                {
                    _celular = _drfila["Celular"].ToString();

                    foreach (GridViewRow i_row in GrdvCitaciones.Rows)
                    {
                        _ddlrepuesta = (DropDownList)GrdvCitaciones.Rows[i_row.RowIndex].Cells[5].FindControl("DdlRespuesta");
                        _celularcomparar = GrdvCitaciones.DataKeys[i_row.RowIndex]["Celular"].ToString();
                        _codigohcit = GrdvCitaciones.DataKeys[i_row.RowIndex]["CodigoHCIT"].ToString();

                        if (_ddlrepuesta.SelectedValue == "0")
                        {
                            //new FuncionesDAO().FunShowJSMessage("Seleccione Respuesta..!", this, "W", "C");
                            break;
                        }

                        if (_celular == _celularcomparar)
                        {
                            _dts = new ConsultaDatosDAO().FunAgendaCitaciones(14, int.Parse(ViewState["CodigoCLDE"].ToString()),
                                0, "", 0, "0", 0, "", "", "", "", "", _drfila["Celular"].ToString(), "", "", "", "",
                                new byte[0], "", "", "", "", "0", "0", 0, "", 0, _ddlrepuesta.SelectedValue, "Whatsapp",
                                "SEND", "", "", "", "", "", "", "", int.Parse(ViewState["CodigoCITA"].ToString()),
                                int.Parse(_codigohcit), 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                                Session["MachineName"].ToString(), ViewState["Conectar"].ToString());

                            _contar++;
                        }
                    }
                }

                if (_totalregistros == _contar)
                    Response.Redirect("WFrm_CitacionProcesoWhastapp.aspx?MensajeRetornado=Whatsapp Gestionados..!", true);
                else
                {
                    _redirect = string.Format("{0}?CodigoCITA={1}&CodigoPERS={2}&CodigoCLDE={3}&NumDocumento={4}" +
                        "&MensajeRetornado={5}", Request.Url.AbsolutePath, ViewState["CodigoCITA"].ToString(),
                        ViewState["CodigoPERS"].ToString(), ViewState["CodigoCLDE"].ToString(),
                        ViewState["NumDocumento"].ToString(), "Falta Responder Whatsapp");

                    Response.Redirect(_redirect);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_CitacionProcesoWhastapp.aspx", true);
        }
        #endregion

    }
}
