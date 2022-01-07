namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_RegistroCitacionMail : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        CheckBox _chkenviado = new CheckBox();
        DropDownList _ddlrepuesta = new DropDownList();
        ImageButton _imgeliminar = new ImageButton();
        Object[] _objdatosmail = new Object[1];
        Object[] _objdatospie = new Object[5];
        string _ext = "", _path = "", _enviado = "", _email = "", _emailcomparar = "", _respuesta = "", _mensaje = "",
            _codigomatd = "", _codigohcit = "", _filepdf = "", _fileLogo = "", _fileTemplate = "", _mailsalterna = "", _subject = "",
            _cedula = "", _mails = "";
        decimal _totalExigible = 0.00M, _totalDeuda = 0.00M;
        int _totalregistros = 0, _contar = 0;
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
                    ViewState["CodigoCITA"] = Request["CodigoCITA"];
                    ViewState["CodigoPERS"] = Request["CodigoPERS"];
                    ViewState["CodigoCLDE"] = Request["CodigoCLDE"];
                    ViewState["NumDocumento"] = Request["NumDocumento"];

                    //_dtbcitaciones.Columns.Add("CodigoCITA");
                    //_dtbcitaciones.Columns.Add("Canal");
                    //ViewState["Citaciones"] = _dtbcitaciones;

                    Lbltitulo.Text = "Registro Citación Mail";
                    PnlDatosDeudor.Height = 100;
                    PnlDatosGetion.Height = 120;
                    PnlDatosGarante.Height = 120;
                    PnlCitaciones.Height = 230;

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 0, 0, 0, "", "LOGOMN", "PATH LOGOS", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        ViewState["_logo"] = _dts.Tables[0].Rows[0]["Valor"].ToString();
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 2, 0, 0, "", "SERVIDOR MAIL", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        ViewState["_smpt"] = _dts.Tables[0].Rows[0]["Valor"].ToString();
                        ViewState["_port"] = _dts.Tables[0].Rows[1]["Valor"].ToString();
                        ViewState["_enablessl"] = _dts.Tables[0].Rows[2]["Valor"].ToString();
                        ViewState["_username"] = _dts.Tables[0].Rows[3]["Valor"].ToString();
                        ViewState["_password"] = _dts.Tables[0].Rows[4]["Valor"].ToString();
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 2, 0, 0, "", "PIE MAIL", "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dts.Tables[0].Rows)
                        {
                            if (dr[0].ToString() == "PIE1") ViewState["Pie1"] = dr[1].ToString();
                            if (dr[0].ToString() == "PIE2") ViewState["Pie2"] = dr[1].ToString();
                            if (dr[0].ToString() == "PIE3") ViewState["Pie3"] = dr[1].ToString();
                            if (dr[0].ToString() == "PIE4") ViewState["Pie4"] = dr[1].ToString();
                        }
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(242, int.Parse(ViewState["CodigoCITA"].ToString()), 0,
                        0, "", "", "", Session["Conectar"].ToString());

                    ViewState["_path"] = _dts.Tables[0].Rows[0]["Ruta"].ToString();
                    ViewState["_ext"] = _dts.Tables[0].Rows[0]["Extension"].ToString();

                    FunCargaMantenimiento();

                    if (Request["MensajeRetornado"] != null) new FuncionesDAO().FunShowJSMessage(Request["MensajeRetornado"],
                        this, "W", "R");
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
                    "", "", "", Session["Conectar"].ToString().ToString());

                GrdvDatosDeudor.DataSource = _dts;
                GrdvDatosDeudor.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(33, 0, 0, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    "", "", "", Session["Conectar"].ToString());

                GrdvDatosObligacion.DataSource = _dts;
                GrdvDatosObligacion.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(45, 0, 0, 0, "", ViewState["NumDocumento"].ToString(), "",
                    Session["Conectar"].ToString().ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    TrGarantes.Visible = true;
                    GrdvDatosGarante.DataSource = _dts;
                    GrdvDatosGarante.DataBind();
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(246, 1, int.Parse(ViewState["CodigoCITA"].ToString()), 0,
                    "", "Email", "", Session["Conectar"].ToString());

                ViewState["Citaciones"] = _dts.Tables[0];
                GrdvCitaciones.DataSource = _dts;
                GrdvCitaciones.DataBind();

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected string FunMailsAternativos()
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 2, 0, 0, "", "MAILS ALTERNATIVOS", "",
                Session["Conectar"].ToString());

            foreach (DataRow _drfila in _dts.Tables[0].Rows)
            {
                _mailsalterna += _drfila["Valor"].ToString() + ",";
            }

            if (!string.IsNullOrEmpty(_mailsalterna))
                _mailsalterna = _mailsalterna.Remove(_mailsalterna.Length - 1);

            return _mailsalterna;

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
                    0, "", "", "", Session["Conectar"].ToString());

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
                _email = GrdvCitaciones.DataKeys[_gvRow.RowIndex].Values["Email"].ToString();

                _dtb = (DataTable)ViewState["Citaciones"];
                _result = _dtb.Select("Email='" + _email + "'").FirstOrDefault();
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
                    _dts = new ControllerDAO().FunGetParametroDetalle("TIPO MAIL", "--Seleccione Opcion--", "S");

                    _ddlrespuesta.DataSource = _dts;
                    _ddlrespuesta.DataTextField = "Descripcion";
                    _ddlrespuesta.DataValueField = "Codigo";
                    _ddlrespuesta.DataBind();
                }

                if (e.Row.RowIndex >= 0)
                {
                    _chkenviado = (CheckBox)(e.Row.Cells[4].FindControl("ChkEnviar"));
                    _ddlrepuesta = (DropDownList)(e.Row.Cells[5].FindControl("DdlRespuesta"));
                    _imgeliminar = (ImageButton)(e.Row.Cells[6].FindControl("ImgEliminar"));
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

                    if (!string.IsNullOrEmpty(_respuesta)) _ddlrepuesta.Enabled = false;
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

                _email = GrdvCitaciones.DataKeys[_gvRow.RowIndex].Values["Email"].ToString();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(255, int.Parse(ViewState["CodigoCITA"].ToString()),
                    int.Parse(Session["usuCodigo"].ToString()), 0, "", ViewState["NumDocumento"].ToString(), _email,
                    Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(246, 1, int.Parse(ViewState["CodigoCITA"].ToString()), 0,
                    "", "Email", "", Session["Conectar"].ToString());

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
                _resultado = _dtb.Select("Enviado='SI' AND CodigoDEFI='TRA'");

                _totalregistros = _dtb.Rows.Count;

                _filepdf = Server.MapPath(ViewState["_path"].ToString());
                _fileLogo = Server.MapPath(ViewState["_logo"].ToString());
                _fileTemplate = Server.MapPath("~/Template/HtmlMailSEND.html");

                if (ViewState["_smpt"] == null || ViewState["_port"] == null || ViewState["_enablessl"] == null ||
                    ViewState["_username"] == null || ViewState["_password"] == null)
                {
                    return;
                }

                _mailsalterna = FunMailsAternativos();

                _objdatospie[0] = ViewState["Pie1"].ToString();
                _objdatospie[1] = ViewState["Pie2"].ToString();
                _objdatospie[2] = ViewState["Pie3"].ToString();
                _objdatospie[3] = ViewState["Pie4"].ToString();

                if (_resultado.Count() > 0) //MAILS TRABAJA
                {
                    _objdatospie[4] = "Se Solicita muy cordialmente remitir la informacion adjunta a su colaborador";

                    _subject = "NOTIFICACION DE DEMANDA PARA SU COLABORADOR";

                    foreach (DataRow _drfila in _resultado)
                    {
                        _objdatosmail[0] = "<table border=\"1\" style=\"width: 100%\">" +
                            "<tr>" +
                                "<td style=\"width: 10%; text-align:center\">" +
                                    "<span style=\"font-weight:bold\">Tipo</span>" + "</td>" +
                                "<td style=\"width: 35%; text-align:center\">" +
                                    "<span style=\"font-weight:bold\">Nombres</span>" + "</td>" +
                                "<td style=\"width: 55%; text-align:center\">" +
                                    "<span style=\"font-weight:bold\">Observacion</span>" + "</td>" +
                            "</tr>";

                            //_dts = new ConsultaDatosDAO().FunAgendaCitaciones(11, 0, 0, "", 0, "0", 0, "MAIL ENVIADO",
                            //"", "", "", "", "", "", "", "", _drfila["Email"].ToString(), new byte[0], "", "", "", "",
                            //"0", "0", 0, "", 0, "SEND", "Email", "MEV", "", "", "", "", "", "", "",
                            //int.Parse(ViewState["CodigoCITA"].ToString()), 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                            //Session["MachineName"].ToString(), Session["Conectar"].ToString());

                        if (_drfila["CodigoTIPO"].ToString() != "FAM")
                        {
                            if (_drfila["CodigoTIPO"].ToString() == "TIT") _respuesta = "REMITIR A SU COLABORADOR QUIEN CONSTA COMO TITULAR DE LA OBLIGACION";

                            if (_drfila["CodigoTIPO"].ToString() == "GAR") _respuesta = "REMITIR A SU COLABORADOR QUIEN CONSTA COMO GARANTE DE LA OBLIGACION";

                            if (_drfila["CodigoTIPO"].ToString() == "CDU") _respuesta = "REMITIR A SU COLABORADOR QUIEN CONSTA COMO CODEUDOR DE LA OBLIGACION";

                            _objdatosmail[0] +=
                                "<tr>" +
                                    "<td>" + _drfila["TipoCliente"].ToString() + "</td>" +
                                    "<td>" + _drfila["Cliente"].ToString() + "</td>" +
                                    "<td>" + _respuesta + "</td>" +
                                "</tr></table>";

                            _mensaje = new FuncionesDAO().FunEnviarMail(_drfila["Email"].ToString(), _subject, _objdatosmail, _objdatospie,
                                _fileTemplate, ViewState["_smpt"].ToString(), int.Parse(ViewState["_port"].ToString()),
                                bool.Parse(ViewState["_enablessl"].ToString()), ViewState["_username"].ToString(),
                                ViewState["_password"].ToString(), _filepdf, _fileLogo, _mailsalterna, Session["Conectar"].ToString());
                        }
                    }
                }

                _resultado = _dtb.Select("Enviado='SI' AND CodigoDEFI='PER'");

                if (_resultado.Count() > 0) //MAILS PERSONAL
                {
                    _cedula = "";
                    _mails = "";
                    _respuesta = "";
                    _objdatosmail[0] = null;
                    _objdatospie[4] = "Por favor revise la información Adjunta";
                    _subject = "AVISO DE DEMANDA A: " + ViewState["Cliente"].ToString();

                    _objdatosmail[0] = "<table border=\"1\" style=\"width: 100%\">" +
                        "<tr>" +
                            "<td style=\"width: 10%; text-align:center\">" +
                                "<span style=\"font-weight:bold\">Tipo</span>" + "</td>" +
                            "<td style=\"width: 35%; text-align:center\">" +
                                "<span style=\"font-weight:bold\">Nombres</span>" + "</td>" +
                            "<td style=\"width: 55%; text-align:center\">" +
                                "<span style=\"font-weight:bold\">Observacion</span>" + "</td>" +
                        "</tr>";

                    foreach (DataRow _drfila in _resultado)
                    {
                        _dts = new ConsultaDatosDAO().FunAgendaCitaciones(11, 0, 0, "", 0, "0", 0, "MAIL ENVIADO",
                            "", "", "", "", "", "", "", "", _drfila["Email"].ToString(), new byte[0], "", "", "", "",
                            "0", "0", 0, "", 0, "SEND", "Email", "MEV", "", "", "", "", "", "", "",
                            int.Parse(ViewState["CodigoCITA"].ToString()), 0, 0, 0, 0,
                            int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                            ViewState["Conectar"].ToString());

                        if (_drfila["CodigoTIPO"].ToString() == "FAM") _respuesta = "POR FAVOR INFORMAR AL TITULAR SOBRE LA DEUDA";
                        if (_drfila["CodigoTIPO"].ToString() == "GAR") _respuesta = "CONSTA COMO GARANTE DEL TITULAR DE LA DEUDA";
                        if (_drfila["CodigoTIPO"].ToString() == "CDU") _respuesta = "CONSTA COMO CODEUDOR DEL TITULAR DE LA DEUDA";

                        if (_drfila["CodigoTIPO"].ToString() == "TIT")
                        {
                            if (_cedula != _drfila["Cedula"].ToString())
                            {
                                _objdatosmail[0] +=
                                "<tr>" +
                                    "<td>" + _drfila["TipoCliente"].ToString() + "</td>" +
                                    "<td>" + _drfila["Cliente"].ToString() + "</td>" +
                                    "<td>" + _respuesta + "</td>" +
                                "</tr>";
                            }
                        }
                        else
                        {
                            _objdatosmail[0] +=
                            "<tr>" +
                                "<td>" + _drfila["TipoCliente"].ToString() + "</td>" +
                                "<td>" + _drfila["Cliente"].ToString() + "</td>" +
                                "<td>" + _respuesta + "</td>" +
                            "</tr>";
                        }

                        _cedula = _drfila["Cedula"].ToString();
                        _mails = _mails + _drfila["Email"].ToString() + ",";
                    }

                    _objdatosmail[0] += "</table>";

                    _mails = _mails.Remove(_mails.Length - 1);

                    _mensaje = new FuncionesDAO().FunEnviarMail(_mails, _subject, _objdatosmail, _objdatospie,
                        _fileTemplate, ViewState["_smpt"].ToString(), int.Parse(ViewState["_port"].ToString()),
                        bool.Parse(ViewState["_enablessl"].ToString()), ViewState["_username"].ToString(),
                        ViewState["_password"].ToString(), _filepdf, _fileLogo, _mailsalterna,
                        Session["Conectar"].ToString());

                    _dts = new ConsultaDatosDAO().FunAgendaCitaciones(9, 0, 0, "", 0, "0", 0, "MAIL ENVIADO",
                        "", "", "", "", "", "", "", "", "", new byte[0], "", "", "", "", "0", "0", 0, "", 0,
                        "SEND", "Email", "MEV", "", "", "", "", "", "", "", int.Parse(ViewState["CodigoCITA"].ToString()),
                        0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                _resultado = _dtb.Select("Enviado='SEND'");

                foreach (DataRow _drfila in _resultado)
                {
                    _email = _drfila["Email"].ToString();

                    foreach (GridViewRow i_row in GrdvCitaciones.Rows)
                    {
                        _ddlrepuesta = (DropDownList)GrdvCitaciones.Rows[i_row.RowIndex].Cells[5].FindControl("DdlRespuesta");
                        _emailcomparar = GrdvCitaciones.DataKeys[i_row.RowIndex]["Email"].ToString();
                        _codigomatd = GrdvCitaciones.DataKeys[i_row.RowIndex]["CodigoMATD"].ToString();
                        _codigohcit = GrdvCitaciones.DataKeys[i_row.RowIndex]["CodigoHCIT"].ToString();

                        if (_email == _emailcomparar)
                        {
                            if(_ddlrepuesta.SelectedValue != "0")
                            {
                                _dts = new ConsultaDatosDAO().FunAgendaCitaciones(12, int.Parse(ViewState["CodigoCLDE"].ToString()),
                                    0, "", 0, "0", 0, "", "", "", "", "", "", "", "", "", _drfila["Email"].ToString(), new byte[0],
                                    "", "", "", "", "0", "0", 0, "", 0, _ddlrepuesta.SelectedValue, "Email",
                                    "", "", "", "", "", "", "", "", int.Parse(ViewState["CodigoCITA"].ToString()), 0,
                                    int.Parse(_codigomatd), int.Parse(_codigohcit), 0, int.Parse(Session["usuCodigo"].ToString()),
                                    Session["MachineName"].ToString(), Session["Conectar"].ToString());

                                _contar++;
                            }
                        }
                    }
                }

                Response.Redirect("WFrm_CitacionProcesoEmail.aspx?MensajeRetornado=Mails Gestionados..!");
                //if (_totalregistros == _contar)
                //    Response.Redirect("WFrm_CitacionProcesoEmail.aspx?MensajeRetornado=Mails Gestionados..!");
                //else
                //{
                //    _redirect = string.Format("{0}?CodigoCITA={1}&CodigoPERS={2}&CodigoCLDE={3}&NumDocumento={4}" +
                //        "&MensajeRetornado={5}", Request.Url.AbsolutePath, ViewState["CodigoCITA"].ToString(),
                //        ViewState["CodigoPERS"].ToString(), ViewState["CodigoCLDE"].ToString(),
                //        ViewState["NumDocumento"].ToString(), "Falta Responder Mails");

                //    Response.Redirect(_redirect);
                //}
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_CitacionProcesoEmail.aspx", true);
        }
        #endregion
    }
}
