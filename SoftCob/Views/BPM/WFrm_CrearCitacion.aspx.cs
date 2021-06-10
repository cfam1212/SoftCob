

namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Threading;

    public partial class WFrm_CrearCitacion : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbdatos = new DataTable();
        DataTable _dtbwhastapp = new DataTable();
        DataTable _dtbemail = new DataTable();
        DataTable _dtbterreno = new DataTable();
        decimal _totalExigible = 0.00M, _totalDeuda = 0.00M;
        string _correos = "", _codigo = "0", _observacion = "", _codigocomparar = "0";
        CheckBox _chkwhatsapp = new CheckBox();
        CheckBox _chkemail = new CheckBox();
        CheckBox _chkterreno = new CheckBox();
        ImageButton _imgcitacion = new ImageButton();
        TextBox _txtobserva = new TextBox();
        TextBox _observa;
        DataRow _resultado;
        DataRow[] _result;
        int _codigocita = 0;
        bool _lexiste = false;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");

                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
                Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
                TxtValor.Attributes.Add("onchange", "ValidarDecimales();");

                if (!IsPostBack)
                {
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    ViewState["CodigoPERS"] = Request["CodigoPERS"];
                    ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                    ViewState["CodigoCLDE"] = Request["CodigoCLDE"];
                    ViewState["Retornar"] = Request["Retornar"];
                    Lbltitulo.Text = "Nuevo Solicitud de Citacion";
                    PnlDatosDeudor.Height = 100;
                    PnlDatosGetion.Height = 120;
                    FunCargaMantenimiento();
                    FunCargarCombos(0);
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

                ViewState["NumeroDocumento"] = _dts.Tables[0].Rows[0]["Cedula"].ToString();
                GrdvDatosDeudor.DataSource = _dts;
                GrdvDatosDeudor.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(33, 0, 0, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    "", "", "", ViewState["Conectar"].ToString().ToString());

                GrdvDatosObligacion.DataSource = _dts;
                GrdvDatosObligacion.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:

                    DdlDireccion.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO CLIENTE",
                        "--Seleccione Tipo Direccion--", "S");
                    DdlDireccion.DataTextField = "Descripcion";
                    DdlDireccion.DataValueField = "Codigo";
                    DdlDireccion.DataBind();

                    DdlTipoMail.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO CLIENTE",
                        "--Seleccione Tipo Correo--", "S");
                    DdlTipoMail.DataTextField = "Descripcion";
                    DdlTipoMail.DataValueField = "Codigo";
                    DdlTipoMail.DataBind();

                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void GrdvDatosObligacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _totalExigible += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Exigible"));
                    _totalDeuda += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "MontoGSPBO"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[3].Text = "TOTAL:";
                    e.Row.Cells[4].Text = _totalDeuda.ToString();
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[5].Text = _totalExigible.ToString();
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

  

        protected void ChkWathaspp_CheckedChanged(object sender, EventArgs e)
        {
            TxtObservaWhatsapp.Text = "";
            TrWhast1.Visible = false;
            TrWhast2.Visible = false;
            TrWhast3.Visible = false;

            if (ChkWathaspp.Checked)
            {
                TrWhast1.Visible = true;
                TrWhast2.Visible = true;
                TrWhast3.Visible = true;

                _dts = new ConsultaDatosDAO().FunConsultaDatos(208, 0, int.Parse(ViewState["CodigoPERS"].ToString()), 0,
                    "", ViewState["NumeroDocumento"].ToString(), "", ViewState["Conectar"].ToString().ToString());

                ViewState["Whatsapp"] = _dts.Tables[0];
                GrdvCelulares.DataSource = _dts;
                GrdvCelulares.DataBind();
            }
        }

        protected void ChkCelular_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkwhatsapp = (CheckBox)(gvRow.Cells[3].FindControl("ChkCelular"));
                _txtobserva = (TextBox)(gvRow.Cells[2].FindControl("TxtObservaCelular"));
                _dtbwhastapp = (DataTable)ViewState["Whatsapp"];
                _codigo = GrdvCelulares.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                _resultado = _dtbwhastapp.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _resultado["Enviar"] = _chkwhatsapp.Checked ? "SI" : "NO";
                if (!_chkwhatsapp.Checked) _txtobserva.Text = "";

                _dtbwhastapp.AcceptChanges();
                ViewState["Whatsapp"] = _dtbwhastapp;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEmail_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DdlTipoMail.SelectedValue = "0";
                RdbEmail.SelectedValue = "PER";
                TxtEmail.Text = "";
                TxtObservaEmail.Text = "";
                TrEmail1.Visible = false;
                TrEmail2.Visible = false;
                TrEmail3.Visible = false;
                TrEmail4.Visible = false;
                TrEmail5.Visible = false;
                GrdvEmails.DataSource = null;
                GrdvEmails.DataBind();

                if (ChkEmail.Checked)
                {
                    TrEmail1.Visible = true;
                    TrEmail2.Visible = true;
                    TrEmail3.Visible = true;
                    TrEmail4.Visible = true;

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(234, 0, 0, 0, "TIPO CORREO", "CORREO",
                        ViewState["NumeroDocumento"].ToString(), ViewState["Conectar"].ToString().ToString());
                    GrdvEmails.DataSource = _dts;
                    GrdvEmails.DataBind();
                    ViewState["Emails"] = _dts.Tables[0];

                    if (_dts.Tables[0].Rows.Count > 0) TrEmail5.Visible = true;
                    else TrEmail5.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        //protected void ImgAgregarMail_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        if (DdlTipoMail.SelectedValue == "0")
        //        {
        //            new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Mail..!", this);
        //            return;
        //        }

        //        if (string.IsNullOrEmpty(TxtEmail.Text.Trim()))
        //        {
        //            new FuncionesDAO().FunShowJSMessage("Ingrese Email..!", this);
        //            return;
        //        }

        //        if (!new FuncionesDAO().Email_bien_escrito(TxtEmail.Text.Trim()))
        //        {
        //            new FuncionesDAO().FunShowJSMessage("Email Incorrecto..!", this);
        //            return;
        //        }

        //        _dtbdatos = (DataTable)ViewState["Emails"];

        //        if (ViewState["Emails"] != null)
        //        {
        //            _resultado = _dtbdatos.Select("Email='" + TxtEmail.Text.ToLower() + "'").FirstOrDefault();

        //            if (_resultado != null) _lexiste = true;
        //        }

        //        if (_lexiste)
        //        {
        //            new FuncionesDAO().FunShowJSMessage("Ya Existe Email Agregado..!", this);
        //            return;
        //        }

        //        TrEmail5.Visible = true;
        //        _dts = new ConsultaDatosDAO().FunInsertAdicionales(1, 0, 0, 0, "", ViewState["NumeroDocumento"].ToString(),
        //            "CORREO", DdlTipoMail.SelectedValue, RdbEmail.SelectedValue, TxtEmail.Text.Trim().ToLower(),
        //            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
        //            "", "", ViewState["Conectar"].ToString());

        //        _dts = new ConsultaDatosDAO().FunInsertAdicionales(6, 0, 0, 0, "", ViewState["NumeroDocumento"].ToString(),
        //            "CORREO", "TIPO CORREO", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
        //            "", "", "", "", "", "", "", "", "", "", "", "", ViewState["Conectar"].ToString());

        //        ViewState["Emails"] = _dts.Tables[0];
        //        GrdvEmails.DataSource = _dts.Tables[0];
        //        GrdvEmails.DataBind();

        //        DdlTipoMail.SelectedValue = "0";
        //        TxtEmail.Text = "";
        //        TxtObservaEmail.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        //protected void ChkSolEmail_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        //        _chkemail = (CheckBox)(gvRow.Cells[4].FindControl("ChkSolEmail"));
        //        _txtobserva = (TextBox)(gvRow.Cells[3].FindControl("TxtObservaCorreo"));

        //        _dtbemail = (DataTable)ViewState["Emails"];
        //        _codigo = GrdvEmails.DataKeys[gvRow.RowIndex].Values["CodigoMATD"].ToString();
        //        _resultado = _dtbemail.Select("CodigoMATD='" + _codigo + "'").FirstOrDefault();
        //        _resultado["Enviar"] = _chkemail.Checked ? "SI" : "NO";
        //        if (!_chkemail.Checked) _txtobserva.Text = "";

        //        _dtbemail.AcceptChanges();
        //        ViewState["Emails"] = _dtbemail;
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        //protected void ImgDelEmail_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
        //        _codigo = GrdvEmails.DataKeys[_gvrow.RowIndex].Values["CodigoMATD"].ToString();
        //        _correos = GrdvEmails.DataKeys[_gvrow.RowIndex].Values["Email"].ToString();

        //        _dtbemail = (DataTable)ViewState["Emails"];
        //        _resultado = _dtbemail.Select("CodigoMATD='" + _codigo + "'").FirstOrDefault();
        //        _resultado.Delete();
        //        _dtbdatos.AcceptChanges();
        //        GrdvEmails.DataSource = _dtbemail;
        //        GrdvEmails.DataBind();

        //        _dts = new ConsultaDatosDAO().FunInsertAdicionales(4, int.Parse(_codigo),
        //            0, 0, "", "", "", "", "", "", "", "", "", "", "", "", "",
        //            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
        //            ViewState["Conectar"].ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        protected void ChkTerreno_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DdlDireccion.SelectedValue = "0";
                RdbTerreno.SelectedValue = "DOM";
                TxtDireccion.Text = "";
                TxtReferencia.Text = "";
                TrTerreno1.Visible = false;
                TrTerreno2.Visible = false;
                TrTerreno3.Visible = false;
                TrTerreno4.Visible = false;
                TrTerreno5.Visible = false;
                TrTerreno6.Visible = false;
                GrdvTerreno.DataSource = null;
                GrdvTerreno.DataBind();

                if (ChkTerreno.Checked)
                {
                    TrTerreno1.Visible = true;
                    TrTerreno2.Visible = true;
                    TrTerreno3.Visible = true;
                    TrTerreno4.Visible = true;
                    TrTerreno5.Visible = true;
                    TrTerreno6.Visible = true;

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(234, 0, 0, 0, "TIPO DIRECCION", "DIRECCION",
                        ViewState["NumeroDocumento"].ToString(), ViewState["Conectar"].ToString().ToString());

                    GrdvTerreno.DataSource = _dts;
                    GrdvTerreno.DataBind();

                    ViewState["Terreno"] = _dts.Tables[0];

                    if (_dts.Tables[0].Rows.Count > 0) TrTerreno6.Visible = true;
                    else TrTerreno6.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        //protected void ImgAgregarTerreno_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        if (DdlDireccion.SelectedValue == "0")
        //        {
        //            new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this);
        //            return;
        //        }

        //        if (string.IsNullOrEmpty(TxtDireccion.Text.Trim()))
        //        {
        //            new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this);
        //            return;
        //        }

        //        _dtbterreno = (DataTable)ViewState["Terreno"];

        //        if (ViewState["Terreno"] != null)
        //        {
        //            _resultado = _dtbterreno.Select("Direccion='" + TxtDireccion.Text.ToUpper() + "'").FirstOrDefault();

        //            if (_resultado != null) _lexiste = true;
        //        }

        //        if (_lexiste)
        //        {
        //            new FuncionesDAO().FunShowJSMessage("Ya Existe Email Agregado..!", this);
        //            return;
        //        }

        //        TrTerreno6.Visible = true;

        //        _dts = new ConsultaDatosDAO().FunInsertAdicionales(2, 0, 0, 0, "", ViewState["NumeroDocumento"].ToString(),
        //            "DIRECCION", DdlDireccion.SelectedValue, RdbTerreno.SelectedValue, TxtDireccion.Text.Trim().ToUpper(),
        //            TxtReferencia.Text.Trim().ToUpper(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
        //            "", "", "", "", "", "", "", "", ViewState["Conectar"].ToString());

        //        _dts = new ConsultaDatosDAO().FunInsertAdicionales(6, 0, 0, 0, "", ViewState["NumeroDocumento"].ToString(),
        //            "DIRECCION", "TIPO DIRECCION", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
        //            "", "", "", "", "", "", "", "", "", "", "", "", ViewState["Conectar"].ToString());

        //        ViewState["Terreno"] = _dts.Tables[0];
        //        GrdvTerreno.DataSource = _dts;
        //        GrdvTerreno.DataBind();

        //        DdlDireccion.SelectedValue = "0";
        //        TxtDireccion.Text = "";
        //        TxtReferencia.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        protected void ChkSolTerreno_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkterreno = (CheckBox)(gvRow.Cells[5].FindControl("ChkSolTerreno"));
                _txtobserva = (TextBox)(gvRow.Cells[4].FindControl("TxtObservaCorreo"));

                _dtbterreno = (DataTable)ViewState["Terreno"];
                _codigo = GrdvTerreno.DataKeys[gvRow.RowIndex].Values["CodigoMATD"].ToString();
                _resultado = _dtbterreno.Select("CodigoMATD='" + _codigo + "'").FirstOrDefault();
                _resultado["Enviar"] = _chkterreno.Checked ? "SI" : "NO";
                if (!_chkterreno.Checked) _txtobserva.Text = "";

                _dtbterreno.AcceptChanges();
                ViewState["Terreno"] = _dtbterreno;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        //protected void ImgDelTerreno_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
        //        _codigo = GrdvTerreno.DataKeys[_gvrow.RowIndex].Values["CodigoMATD"].ToString();

        //        _dtbterreno = (DataTable)ViewState["Terreno"];
        //        _resultado = _dtbterreno.Select("CodigoMATD='" + _codigo + "'").FirstOrDefault();
        //        _resultado.Delete();
        //        _dtbterreno.AcceptChanges();
        //        GrdvTerreno.DataSource = _dtbterreno;
        //        GrdvTerreno.DataBind();

        //        _dts = new ConsultaDatosDAO().FunInsertAdicionales(4, int.Parse(_codigo),
        //            0, 0, "", "", "", "", "", "", "", "", "", "", "", "", "",
        //            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
        //            ViewState["Conectar"].ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtValor.Text.Trim()) || TxtValor.Text.Trim() == "0" || TxtValor.Text.Trim() == "0."
                    || TxtValor.Text.Trim() == "0.0" || TxtValor.Text.Trim() == ".")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Valor Citacion..!", this);
                    return;
                }

                if (!ChkWathaspp.Checked && !ChkEmail.Checked && !ChkTerreno.Checked)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Envio Citacion..!", this);
                    return;
                }

                if (ChkWathaspp.Checked)
                {
                    _dtbwhastapp = (DataTable)ViewState["Whatsapp"];
                    _result = _dtbwhastapp.Select("Enviar='SI'");

                    if (_result.Count() == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione al menos un numero de Celular...!", this);
                        return;
                    }

                    _dts = new ConsultaDatosDAO().FunNewSolictudCitacion(0, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "CSL",
                        TxtValor.Text.Trim(), int.Parse(Session["usuCodigo"].ToString()), TxtObservacion.Text.Trim().ToUpper(),
                        1, TxtObservaWhatsapp.Text.Trim().ToUpper(), 0, "", 0, "", "", "", "", "", "",
                        _result[0]["CodigoTipo"].ToString(), "CEL", "", "", "", "", "", "", 0, 0, 0, 0, 0,
                        int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        ViewState["Conectar"].ToString());

                    _codigocita = int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString());

                    foreach (DataRow _drfila in _result)
                    {
                        _codigo = _drfila["Codigo"].ToString();

                        foreach (GridViewRow i_row in GrdvCelulares.Rows)
                        {
                            _observa = (TextBox)GrdvCelulares.Rows[i_row.RowIndex].Cells[2].FindControl("TxtObservaCelular");
                            _observacion = _observa.Text.Trim().ToUpper();
                            _codigocomparar = GrdvCelulares.DataKeys[i_row.RowIndex]["Codigo"].ToString();

                            if (_codigo == _codigocomparar)
                            {
                                _dts = new ConsultaDatosDAO().FunNewSolictudCitacion(1, _codigocita, 0, 0, 0, "", "", 0, "",
                                    1, "", 0, "", 0, "", "Whatsapp", _drfila["Celular"].ToString(), "", "", "", "", "",
                                    _observacion, "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                                    Session["MachineName"].ToString(), ViewState["Conectar"].ToString());

                                break;
                            }
                        }
                    }
                }

                if (ChkEmail.Checked)
                {
                    _dtbemail = (DataTable)ViewState["Emails"];
                    _result = _dtbemail.Select("Enviar='SI'");

                    if (_result.Count() == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione al menos un Mail...!", this);
                        return;
                    }

                    _dts = new ConsultaDatosDAO().FunNewSolictudCitacion(0, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "CSL",
                        TxtValor.Text.Trim(), int.Parse(Session["usuCodigo"].ToString()), TxtObservacion.Text.Trim().ToUpper(),
                        ChkWathaspp.Checked ? 1 : 0, TxtObservaWhatsapp.Text.Trim().ToUpper(), 1,
                        TxtObservaEmail.Text.Trim().ToUpper(), 0, "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), ViewState["Conectar"].ToString());

                    _codigocita = int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString());

                    foreach (DataRow _drfila in _result)
                    {
                        _codigo = _drfila["CodigoMATD"].ToString();

                        foreach (GridViewRow i_row in GrdvEmails.Rows)
                        {
                            _observa = (TextBox)GrdvEmails.Rows[i_row.RowIndex].Cells[3].FindControl("TxtObservaCorreo");
                            _observacion = _observa.Text.Trim().ToUpper();
                            _codigocomparar = GrdvEmails.DataKeys[i_row.RowIndex]["CodigoMATD"].ToString();

                            if (_codigo == _codigocomparar)
                            {
                                _dts = new ConsultaDatosDAO().FunNewSolictudCitacion(1, _codigocita, 0, 0, 0, "", "", 0, "",
                                    1, "", 0, "", 0, "", "Email", "", _drfila["Email"].ToString(), "", "",
                                    _drfila["CodigoTIPO"].ToString(), _drfila["CodigoDEFI"].ToString(), _observacion, "", "",
                                    "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                                    Session["MachineName"].ToString(), ViewState["Conectar"].ToString());

                                break;
                            }
                        }
                    }
                }

                if (ChkTerreno.Checked)
                {
                    _dtbterreno = (DataTable)ViewState["Terreno"];
                    _result = _dtbterreno.Select("Enviar='SI'");

                    if (_result.Count() == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione al menos una Direccion...!", this);
                        return;
                    }

                    _dts = new ConsultaDatosDAO().FunNewSolictudCitacion(0, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "CSL",
                        TxtValor.Text.Trim(), int.Parse(Session["usuCodigo"].ToString()), TxtObservacion.Text.Trim().ToUpper(),
                        ChkWathaspp.Checked ? 1 : 0, "", ChkEmail.Checked ? 1 : 0, TxtObservaEmail.Text.Trim().ToUpper(),
                        1, TxtObservaTerreno.Text.Trim().ToUpper(), "", "", "", "", "", "", "",
                        "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), ViewState["Conectar"].ToString());

                    _codigocita = int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString());

                    foreach (DataRow _drfila in _result)
                    {
                        _codigo = _drfila["CodigoMATD"].ToString();

                        foreach (GridViewRow i_row in GrdvTerreno.Rows)
                        {
                            _observa = (TextBox)GrdvTerreno.Rows[i_row.RowIndex].Cells[4].FindControl("TxtObservaTerreno");
                            _observacion = _observa.Text.Trim().ToUpper();
                            _codigocomparar = GrdvTerreno.DataKeys[i_row.RowIndex]["CodigoMATD"].ToString();

                            if (_codigo == _codigocomparar)
                            {
                                _dts = new ConsultaDatosDAO().FunNewSolictudCitacion(1, _codigocita, 0, 0, 0, "", "", 0, "",
                                    1, "", 0, "", 0, "", "Terreno", "", "", _drfila["Direccion"].ToString(),
                                    _drfila["Referencia"].ToString(), _drfila["CodigoTIPO"].ToString(),
                                    _drfila["CodigoDEFI"].ToString(), _observacion, "", "", "", "", "", 0, 0, 0, 0, 0,
                                    int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                                    ViewState["Conectar"].ToString());

                                break;
                            }
                        }
                    }
                }

                switch (ViewState["Retornar"].ToString())
                {
                    case "0":
                        Response.Redirect("../Gestion/WFrm_ListaClientesAdmin.aspx?MensajeRetornado=Citacion Realizada..! ", true);
                        break;
                    case "1":
                        Response.Redirect("WFrm_SeguimientoCitacionAdmin.aspx?MensajeRetornado=Citacion Realizada..! ", true);
                        break;
                    case "2":
                        Response.Redirect("../Gestion/WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" +
                            Session["IdListaCabecera"].ToString(), true);
                        break;
                    case "3":
                        Response.Redirect("../ReportesManager/WFrm_ReporteGestorCedente.aspx?codigoCEDE=" +
                            ViewState["codigoCEDE"].ToString() + "&codigoCPCE=" +
                            ViewState["codigoCPCE"].ToString(), true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion

    }
}
