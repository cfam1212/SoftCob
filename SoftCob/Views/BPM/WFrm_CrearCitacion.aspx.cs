namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_CrearCitacion : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbdatos = new DataTable();
        DataTable _dtbwhastapp = new DataTable();
        DataTable _dtbemail = new DataTable();
        DataTable _dtbterreno = new DataTable();
        DataTable _dtbcitaciones = new DataTable();
        DataTable _tblagre = new DataTable();
        DataRow _filagre;
        GridView _grdvCanales;
        decimal _totalExigible = 0.00M, _totalDeuda = 0.00M;
        string _correos = "", _codigo = "0", _observacion = "", _codigocomparar = "0", _tipocliente = "", _cedula = "", _sql = "",
            _whastapp = "", _email = "", _terreno = "", _canal = "", _codcita = "", _codigociud = "";
        CheckBox _chkwhatsapp = new CheckBox();
        CheckBox _chkemail = new CheckBox();
        CheckBox _chkterreno = new CheckBox();
        CheckBox _chksolicitar = new CheckBox();
        ImageButton _imgcitacion = new ImageButton();
        ImageButton _imglogo = new ImageButton();
        ImageButton _imgselecc = new ImageButton();
        ListItem _itemc = new ListItem();
        TextBox _txtobserva = new TextBox();
        TextBox _observa;
        DataRow _resultado;
        DataRow[] _result;
        int _codigocita = 0, _opcion = 0;
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
                    ViewState["CodigoPERS"] = Request["CodigoPERS"];
                    ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                    ViewState["CodigoCLDE"] = Request["CodigoCLDE"];
                    ViewState["NumDocumento"] = Request["NumDocumento"];
                    ViewState["Retornar"] = Request["Retornar"];

                    _dtbcitaciones.Columns.Add("CodigoCITA");
                    _dtbcitaciones.Columns.Add("Canal");
                    ViewState["Citaciones"] = _dtbcitaciones;

                    Lbltitulo.Text = "Nuevo Solicitud de Notificación";
                    PnlDatosDeudor.Height = 100;
                    PnlDatosGetion.Height = 120;
                    PnlDatosGarante.Height = 120;
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
                    "", "", "", Session["Conectar"].ToString());

                ViewState["NumeroDocumento"] = _dts.Tables[0].Rows[0]["Cedula"].ToString();
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

                _dts = new ConsultaDatosDAO().FunConsultaDatos(242, int.Parse(ViewState["CodigoPERS"].ToString()), 1, 0,
                    "", "", "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    TblHistorial.Visible = true;
                    _codcita = _dts.Tables[0].Rows[0]["CitaCodigo"].ToString();
                    _whastapp = _dts.Tables[0].Rows[0]["Whastapp"].ToString();
                    _email = _dts.Tables[0].Rows[0]["Email"].ToString();
                    _terreno = _dts.Tables[0].Rows[0]["Terreno"].ToString();

                    _tblagre = new DataTable();
                    _tblagre = (DataTable)ViewState["Citaciones"];

                    if (!string.IsNullOrEmpty(_whastapp))
                    {
                        _filagre = _tblagre.NewRow();
                        _filagre["CodigoCITA"] = _codcita;
                        _filagre["Canal"] = _whastapp;
                        _tblagre.Rows.Add(_filagre);
                    }

                    if (!string.IsNullOrEmpty(_email))
                    {
                        _filagre = _tblagre.NewRow();
                        _filagre["CodigoCITA"] = _codcita;
                        _filagre["Canal"] = _email;
                        _tblagre.Rows.Add(_filagre);
                    }

                    if (!string.IsNullOrEmpty(_terreno))
                    {
                        _filagre = _tblagre.NewRow();
                        _filagre["CodigoCITA"] = _codcita;
                        _filagre["Canal"] = _terreno;
                        _tblagre.Rows.Add(_filagre);
                    }

                    GrdvCitaciones.DataSource = _tblagre;
                    GrdvCitaciones.DataBind();
                }
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
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(207, 0, int.Parse(ViewState["CodigoPERS"].ToString()), 0, "",
                        ViewState["NumeroDocumento"].ToString(), "", Session["Conectar"].ToString());

                    DdlDireccion.DataSource = _dts;
                    DdlDireccion.DataTextField = "Descripcion";
                    DdlDireccion.DataValueField = "Codigo";
                    DdlDireccion.DataBind();

                    DdlTipoMail.DataSource = _dts;
                    DdlTipoMail.DataTextField = "Descripcion";
                    DdlTipoMail.DataValueField = "Codigo";
                    DdlTipoMail.DataBind();

                    DdlProvincia.DataSource = new ConsultaDatosDAO().FunConsultaDatos(186, 0, 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlProvincia.DataTextField = "Descripcion";
                    DdlProvincia.DataValueField = "Codigo";
                    DdlProvincia.DataBind();

                    _itemc.Text = "--Seleccione Ciudad--";
                    _itemc.Value = "0";
                    DdlCiudad.Items.Add(_itemc);

                    DdlSector.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO SECTOR", "--Seleccione Sector--", "S");
                    DdlSector.DataTextField = "Descripcion";
                    DdlSector.DataValueField = "Codigo";
                    DdlSector.DataBind();

                    break;
                case 1:
                    DdlCiudad.DataSource = new ConsultaDatosDAO().FunConsultaDatos(186,
                        1, int.Parse(DdlProvincia.SelectedValue), 0, "", "", "", Session["Conectar"].ToString());
                    DdlCiudad.DataTextField = "Descripcion";
                    DdlCiudad.DataValueField = "Codigo";
                    DdlCiudad.DataBind();
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
                    _totalDeuda += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ValorDeuda"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = "TOTAL:";
                    e.Row.Cells[3].Text = _totalDeuda.ToString();
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
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
        protected void ChkWathaspp_CheckedChanged(object sender, EventArgs e)
        {
            TxtObservaWhatsapp.Text = "";
            //TrWhast1.Visible = false;
            TrWhast2.Visible = false;
            TrWhast3.Visible = false;

            if (ChkWathaspp.Checked)
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(208, 0, int.Parse(ViewState["CodigoPERS"].ToString()), 0,
                    "", ViewState["NumeroDocumento"].ToString(), "", Session["Conectar"].ToString());

                ViewState["Whatsapp"] = _dts.Tables[0];
                GrdvCelulares.DataSource = _dts;
                GrdvCelulares.DataBind();

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    //TrWhast1.Visible = true;
                    TrWhast2.Visible = true;
                    TrWhast3.Visible = true;
                }
                else
                {
                    new FuncionesDAO().FunShowJSMessage("No Existen Celulares Efectivos..!", this, "W", "L");
                    ChkWathaspp.Checked = false;
                }
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
        protected void GrdvCitaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imglogo = (ImageButton)(e.Row.Cells[1].FindControl("ImgLogo"));
                    _canal = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["Canal"].ToString();

                    switch (_canal)
                    {
                        case "Whatsapp":
                            _imglogo.ImageUrl = "~/Botones/btnwhastapp.png";
                            break;
                        case "Email":
                            _imglogo.ImageUrl = "~/Botones/btnemailcitacion.png";
                            break;
                        case "Terreno":
                            _imglogo.ImageUrl = "~/Botones/casa.png";
                            break;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _canal = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["Canal"].ToString();
                    _codcita = GrdvCitaciones.DataKeys[e.Row.RowIndex].Values["CodigoCITA"].ToString();
                    _grdvCanales = e.Row.FindControl("GrdvCanales") as GridView;

                    switch (_canal)
                    {
                        case "Whatsapp":
                            _grdvCanales.Columns[1].Visible = false;
                            _grdvCanales.Columns[3].Visible = false;
                            _grdvCanales.Columns[4].Visible = false;
                            _grdvCanales.Columns[5].Visible = false;
                            _grdvCanales.Columns[6].Visible = false;
                            _grdvCanales.Columns[7].Visible = false;
                            _grdvCanales.Columns[9].Visible = false;
                            _opcion = 4;
                            break;
                        case "Email":
                            _grdvCanales.Columns[2].Visible = false;
                            _grdvCanales.Columns[4].Visible = false;
                            _grdvCanales.Columns[5].Visible = false;
                            _grdvCanales.Columns[6].Visible = false;
                            _grdvCanales.Columns[7].Visible = false;
                            _grdvCanales.Columns[9].Visible = false;
                            _opcion = 5;
                            break;
                        case "Terreno":
                            _grdvCanales.Columns[2].Visible = false;
                            _grdvCanales.Columns[3].Visible = false;
                            _opcion = 6;
                            break;
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(246, _opcion, int.Parse(_codcita), 0, "", _canal,
                        "VIS", ViewState["Conectar"].ToString());

                    if (_canal == "Terreno") ViewState["CitasTerreno"] = _dts.Tables[0];

                    _grdvCanales.DataSource = _dts;
                    _grdvCanales.DataBind();

                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
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
                //TrEmail3.Visible = false;
                TrEmail4.Visible = false;
                TrEmail5.Visible = false;
                GrdvEmails.DataSource = null;
                GrdvEmails.DataBind();

                if (ChkEmail.Checked)
                {
                    TrEmail1.Visible = true;
                    TrEmail2.Visible = true;
                    //TrEmail3.Visible = true;
                    TrEmail4.Visible = true;

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(234, 0, 0, 0, "TIPO CORREO", "CORREO",
                        ViewState["NumeroDocumento"].ToString(), Session["Conectar"].ToString().ToString());
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
        protected void ImgAgregarMail_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlTipoMail.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Mail..!", this, "W", "R");
                    return;
                }

                if (string.IsNullOrEmpty(TxtEmail.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Email..!", this, "W", "R");
                    return;
                }

                if (!new FuncionesDAO().Email_bien_escrito(TxtEmail.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Email Incorrecto..!", this, "W", "R");
                    return;
                }

                _dtbdatos = (DataTable)ViewState["Emails"];

                if (ViewState["Emails"] != null)
                {
                    _resultado = _dtbdatos.Select("Email='" + TxtEmail.Text.ToLower() + "'").FirstOrDefault();

                    if (_resultado != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya Existe Email Agregado..!", this, "W", "R");
                    return;
                }

                TrEmail5.Visible = true;

                if (DdlTipoMail.SelectedItem.ToString() == "TITULAR")
                {
                    _tipocliente = "TIT";
                    _cedula = ViewState["NumeroDocumento"].ToString();
                }
                else if (DdlTipoMail.SelectedItem.ToString() == "Familiar/Otros")
                {
                    if (RdbEmail.SelectedValue == "TRA")
                    {
                        new FuncionesDAO().FunShowJSMessage("Mail No Permitido..!", this, "W", "R");
                        return;
                    }
                    _tipocliente = "FAM";
                    _cedula = DdlTipoMail.SelectedValue;
                }
                else
                {
                    _sql = "SELECT TipoCliente=DER.dere_tiporeferencia,CedulaGAR=DER.dere_numdocumento FROM SoftCob_DEUDOR_REFERENCIAS DER (NOLOCK) ";
                    _sql += "WHERE DER.pers_codigo=" + ViewState["CodigoPERS"].ToString() + " AND DER.dere_numdocumento='" + DdlTipoMail.SelectedValue + "'";
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

                    _tipocliente = _dts.Tables[0].Rows[0]["TipoCliente"].ToString();
                    _cedula = _dts.Tables[0].Rows[0]["CedulaGAR"].ToString();
                }

                _dts = new ConsultaDatosDAO().FunInsertCorreoDireccion(0, ViewState["NumDocumento"].ToString(), _cedula, "CORREO", _tipocliente, RdbEmail.SelectedValue, "",
                    "", TxtEmail.Text.Trim().ToLower(), "", "", "", "", 0, 0, 0, 0,
                    int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(234, 0, 0, 0, "TIPO CORREO", "CORREO",
                    ViewState["NumDocumento"].ToString(), Session["Conectar"].ToString().ToString());

                ViewState["Emails"] = _dts.Tables[0];
                GrdvEmails.DataSource = _dts.Tables[0];
                GrdvEmails.DataBind();

                DdlTipoMail.SelectedValue = "0";
                TxtEmail.Text = "";
                TxtObservaEmail.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkSolEmail_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkemail = (CheckBox)(gvRow.Cells[4].FindControl("ChkSolEmail"));
                _txtobserva = (TextBox)(gvRow.Cells[3].FindControl("TxtObservaCorreo"));

                _dtbemail = (DataTable)ViewState["Emails"];
                _codigo = GrdvEmails.DataKeys[gvRow.RowIndex].Values["CodigoMATD"].ToString();
                _resultado = _dtbemail.Select("CodigoMATD='" + _codigo + "'").FirstOrDefault();
                _resultado["Enviar"] = _chkemail.Checked ? "SI" : "NO";
                if (!_chkemail.Checked) _txtobserva.Text = "";

                _dtbemail.AcceptChanges();
                ViewState["Emails"] = _dtbemail;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvEmails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chksolicitar = (CheckBox)(e.Row.Cells[4].FindControl("ChkSolEmail"));
                    _observacion = GrdvEmails.DataKeys[e.Row.RowIndex].Values["DirIncorrecta"].ToString();

                    switch (_observacion)
                    {
                        case "CLI":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Red;
                            _chksolicitar.Enabled = false;
                            break;
                        case "MRB":
                            e.Row.Cells[1].BackColor = System.Drawing.Color.Yellow;
                            _chksolicitar.Enabled = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvTerreno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chksolicitar = (CheckBox)(e.Row.Cells[5].FindControl("ChkSolTerreno"));
                    _imgselecc = (ImageButton)(e.Row.Cells[7].FindControl("ImgSelecc"));

                    _observacion = GrdvTerreno.DataKeys[e.Row.RowIndex].Values["DirIncorrecta"].ToString();
                    _codigociud = GrdvTerreno.DataKeys[e.Row.RowIndex].Values["CodigoCIUD"].ToString();

                    if (_observacion == "CMI")
                    {
                        e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
                        _chksolicitar.Enabled = false;
                        _imgselecc.ImageUrl = "~/Botones/seleccgris.png";
                        _imgselecc.Enabled = false;
                    }

                    if (_codigociud == "0") _chksolicitar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelEmail_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvEmails.DataKeys[_gvrow.RowIndex].Values["CodigoMATD"].ToString();
                _correos = GrdvEmails.DataKeys[_gvrow.RowIndex].Values["Email"].ToString();

                _dtbemail = (DataTable)ViewState["Emails"];
                _resultado = _dtbemail.Select("CodigoMATD='" + _codigo + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbdatos.AcceptChanges();
                GrdvEmails.DataSource = _dtbemail;
                GrdvEmails.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(208, 5, int.Parse(_codigo), 0, "", "", "", 
                    Session["Conectar"].ToString().ToString());
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

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
                //TrTerreno5.Visible = false;
                TrTerreno6.Visible = false;
                TrTerreno7.Visible = false;
                TrTerreno8.Visible = false;
                TrTerreno9.Visible = false;
                GrdvTerreno.DataSource = null;
                GrdvTerreno.DataBind();

                if (ChkTerreno.Checked)
                {
                    TrTerreno1.Visible = true;
                    TrTerreno2.Visible = true;
                    TrTerreno3.Visible = true;
                    TrTerreno4.Visible = true;
                    //TrTerreno5.Visible = true;
                    TrTerreno6.Visible = true;
                    TrTerreno7.Visible = true;
                    TrTerreno8.Visible = true;
                    TrTerreno9.Visible = true;

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(234, 0, 0, 0, "TIPO DIRECCION", "DIRECCION",
                        ViewState["NumDocumento"].ToString(), Session["Conectar"].ToString().ToString());

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

        protected void ImgAgregarTerreno_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlDireccion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this, "W", "R");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDireccion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this, "W", "R");
                    return;
                }

                if (DdlProvincia.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Provincia..!", this, "W", "R");
                    return;
                }

                if (DdlCiudad.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Ciudad..!", this, "W", "R");
                    return;
                }

                _dtbterreno = (DataTable)ViewState["Terreno"];

                if (ViewState["Terreno"] != null)
                {
                    _resultado = _dtbterreno.Select("Direccion='" + TxtDireccion.Text.ToUpper() + "'").FirstOrDefault();

                    if (_resultado != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Ya Existe Email Agregado..!", this, "W", "R");
                    return;
                }

                TrTerreno6.Visible = true;

                if (DdlDireccion.SelectedItem.ToString() == "TITULAR")
                {
                    _tipocliente = "TIT";
                    _cedula = ViewState["NumeroDocumento"].ToString();
                }
                else
                {
                    _sql = "SELECT TipoCliente=DER.dere_tiporeferencia,CedulaGAR=DER.dere_numdocumento FROM SoftCob_DEUDOR_REFERENCIAS DER (NOLOCK) ";
                    _sql += "WHERE DER.pers_codigo=" + ViewState["CodigoPERS"].ToString() + " AND DER.dere_numdocumento='" + DdlDireccion.SelectedValue + "'";
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

                    _tipocliente = _dts.Tables[0].Rows[0]["TipoCliente"].ToString();
                    _cedula = _dts.Tables[0].Rows[0]["CedulaGAR"].ToString();
                }

                _dts = new ConsultaDatosDAO().FunInsertCorreoDireccion(1, ViewState["NumDocumento"].ToString(), _cedula, "DIRECCION",
                    _tipocliente, RdbTerreno.SelectedValue, TxtDireccion.Text.Trim().ToUpper(), TxtReferencia.Text.Trim().ToUpper(), "",
                    DdlSector.SelectedValue, "", "", "", int.Parse(DdlProvincia.SelectedValue), int.Parse(DdlCiudad.SelectedValue), 0, 0, 
                    int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(234, 0, 0, 0, "TIPO DIRECCION", "DIRECCION",
                    ViewState["NumDocumento"].ToString(), Session["Conectar"].ToString().ToString());

                ViewState["Terreno"] = _dts.Tables[0];
                GrdvTerreno.DataSource = _dts;
                GrdvTerreno.DataBind();

                DdlDireccion.SelectedValue = "0";
                TxtDireccion.Text = "";
                TxtReferencia.Text = "";
                DdlSector.SelectedValue = "0";
                DdlProvincia.SelectedValue = "0";
                DdlCiudad.Items.Clear();
                _itemc.Text = "--Seleccione Ciudad--";
                _itemc.Value = "0";
                DdlCiudad.Items.Add(_itemc);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvTerreno.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvTerreno.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;

                _codigo = GrdvTerreno.DataKeys[_gvrow.RowIndex].Values["CodigoMATD"].ToString();
                ViewState["CodigoMATD"] = _codigo;

                _dtbterreno = (DataTable)ViewState["Terreno"];
                _resultado = _dtbterreno.Select("CodigoMATD='" + _codigo + "'").FirstOrDefault();

                ViewState["Direccion"] = _resultado["Direccion"].ToString();

                DdlDireccion.SelectedValue = _resultado["CodigoTIPO"].ToString() == "TIT" ? ViewState["NumDocumento"].ToString()
                    : _resultado["Documento"].ToString();

                RdbTerreno.SelectedValue = _resultado["CodigoDEFI"].ToString();
                TxtDireccion.Text = _resultado["Direccion"].ToString();
                TxtReferencia.Text = _resultado["Referencia"].ToString();
                DdlProvincia.SelectedValue = _resultado["CodigoPROV"].ToString();
                FunCargarCombos(1);
                DdlCiudad.SelectedValue = _resultado["CodigoCIUD"].ToString();
                DdlSector.SelectedValue = _resultado["CodigoSECT"].ToString();

                ImgAgregarTerreno.Enabled = false;
                ImgModificaTerreno.Enabled = true;

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ImgModificaTerreno_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlDireccion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this, "W", "R");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDireccion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this, "W", "R");
                    return;
                }

                if (DdlProvincia.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Provincia..!", this, "W", "R");
                    return;
                }

                if (DdlCiudad.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Ciudad..!", this, "W", "R");
                    return;
                }

                if (DdlDireccion.SelectedItem.ToString() == "TITULAR")
                {
                    _tipocliente = "TIT";
                    _cedula = ViewState["NumDocumento"].ToString();
                }
                else
                {
                    _sql = "SELECT TipoCliente=DER.dere_tiporeferencia,CedulaGAR=DER.dere_numdocumento FROM SoftCob_DEUDOR_REFERENCIAS DER (NOLOCK) ";
                    _sql += "WHERE DER.pers_codigo=" + ViewState["CodigoPERS"].ToString() + " AND DER.dere_numdocumento='" + DdlDireccion.SelectedValue + "'";
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, _sql, "", "", Session["Conectar"].ToString());

                    _tipocliente = _dts.Tables[0].Rows[0]["TipoCliente"].ToString();
                    _cedula = _dts.Tables[0].Rows[0]["CedulaGAR"].ToString();
                }

                _dts = new ConsultaDatosDAO().FunInsertCorreoDireccion(2, ViewState["NumDocumento"].ToString(), _cedula, "DIRECCION", _tipocliente, RdbTerreno.SelectedValue,
                    TxtDireccion.Text.Trim().ToUpper(), TxtReferencia.Text.Trim().ToUpper(), "", DdlSector.SelectedValue,
                    "", "", "", int.Parse(DdlProvincia.SelectedValue), int.Parse(DdlCiudad.SelectedValue), int.Parse(ViewState["CodigoMATD"].ToString()),
                    0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(234, 0, 0, 0, "TIPO DIRECCION", "DIRECCION",
                    ViewState["NumeroDocumento"].ToString(), Session["Conectar"].ToString().ToString());

                ViewState["Terreno"] = _dts.Tables[0];
                GrdvTerreno.DataSource = _dts;
                GrdvTerreno.DataBind();

                DdlDireccion.SelectedValue = "0";
                TxtDireccion.Text = "";
                TxtReferencia.Text = "";
                DdlProvincia.SelectedValue = "0";
                DdlCiudad.Items.Clear();
                _itemc.Text = "--Seleccione Ciudad--";
                _itemc.Value = "0";
                DdlCiudad.Items.Add(_itemc);
                DdlSector.SelectedValue = "0";

                ImgAgregarTerreno.Enabled = true;
                ImgModificaTerreno.Enabled = false;

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void ChkSolTerreno_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                _txtobserva = (TextBox)(gvRow.Cells[5].FindControl("TxtObservaTerreno"));
                _chkterreno = (CheckBox)(gvRow.Cells[6].FindControl("ChkSolTerreno"));
                
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

        protected void ImgDelTerreno_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvTerreno.DataKeys[_gvrow.RowIndex].Values["CodigoMATD"].ToString();

                _dtbterreno = (DataTable)ViewState["Terreno"];
                _resultado = _dtbterreno.Select("CodigoMATD='" + _codigo + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbterreno.AcceptChanges();
                GrdvTerreno.DataSource = _dtbterreno;
                GrdvTerreno.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(208, 5, int.Parse(_codigo), 0, "", "", "", 
                    Session["Conectar"].ToString().ToString());
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
                if (string.IsNullOrEmpty(TxtValor.Text.Trim()) || TxtValor.Text.Trim() == "0" || TxtValor.Text.Trim() == "0."
                    || TxtValor.Text.Trim() == "0.0" || TxtValor.Text.Trim() == ".")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Valor Citacion..!", this, "W", "L");
                    return;
                }

                if (!ChkWathaspp.Checked && !ChkEmail.Checked && !ChkTerreno.Checked)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Envio Citacion..!", this, "W", "L");
                    return;
                }

                if (ChkWathaspp.Checked)
                {
                    _dtbwhastapp = (DataTable)ViewState["Whatsapp"];
                    _result = _dtbwhastapp.Select("Enviar='SI'");

                    if (_result.Count() == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione al menos un numero de Celular...!", this, "W", "L");
                        return;
                    }
                }

                if (ChkEmail.Checked)
                {
                    _dtbemail = (DataTable)ViewState["Emails"];
                    _result = _dtbemail.Select("Enviar='SI'");

                    if (_result.Count() == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione al menos un Mail...!", this, "W", "L");
                        return;
                    }
                }

                if (ChkTerreno.Checked)
                {
                    _dtbterreno = (DataTable)ViewState["Terreno"];
                    _result = _dtbterreno.Select("Enviar='SI'");

                    if (_result.Count() == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione al menos una Direccion...!", this, "W", "L");
                        return;
                    }
                }

                if (ChkWathaspp.Checked)
                {
                    _dtbwhastapp = (DataTable)ViewState["Whatsapp"];
                    _result = _dtbwhastapp.Select("Enviar='SI'");

                    _dts = new ConsultaDatosDAO().FunNewSolictudCitacion(0, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "CSL",
                        TxtValor.Text.Trim(), int.Parse(Session["usuCodigo"].ToString()), TxtObservacion.Text.Trim().ToUpper(),
                        1, TxtObservaWhatsapp.Text.Trim().ToUpper(), 0, "", 0, "", "", "", "", "", "",
                        _result[0]["CodigoTipo"].ToString(), "CEL", "", "", "", "", "", "", 0, 0, 0, 0, 0,
                        int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());

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
                                    1, "", 0, "", 0, "", "Whatsapp", _drfila["Celular"].ToString(), "", "", "", _drfila["CodigoTipo"].ToString(), "",
                                    _observacion, "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                                    Session["MachineName"].ToString(), Session["Conectar"].ToString());

                                break;
                            }
                        }
                    }
                }

                if (ChkEmail.Checked)
                {
                    _dtbemail = (DataTable)ViewState["Emails"];
                    _result = _dtbemail.Select("Enviar='SI'");

                    _dts = new ConsultaDatosDAO().FunNewSolictudCitacion(0, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "CSL",
                        TxtValor.Text.Trim(), int.Parse(Session["usuCodigo"].ToString()), TxtObservacion.Text.Trim().ToUpper(),
                        ChkWathaspp.Checked ? 1 : 0, TxtObservaWhatsapp.Text.Trim().ToUpper(), 1,
                        TxtObservaEmail.Text.Trim().ToUpper(), 0, "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());

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
                                    _drfila["Documento"].ToString(), "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                                    Session["MachineName"].ToString(), Session["Conectar"].ToString());

                                break;
                            }
                        }
                    }
                }

                if (ChkTerreno.Checked)
                {
                    _dtbterreno = (DataTable)ViewState["Terreno"];
                    _result = _dtbterreno.Select("Enviar='SI'");

                    _dts = new ConsultaDatosDAO().FunNewSolictudCitacion(0, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()), "CSL",
                        TxtValor.Text.Trim(), int.Parse(Session["usuCodigo"].ToString()), TxtObservacion.Text.Trim().ToUpper(),
                        ChkWathaspp.Checked ? 1 : 0, TxtObservaWhatsapp.Text.Trim().ToUpper(), ChkEmail.Checked ? 1 : 0, TxtObservaEmail.Text.Trim().ToUpper(),
                        1, TxtObservaTerreno.Text.Trim().ToUpper(), "", "", "", "", "", "", "",
                        "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());

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
                                    _drfila["CodigoDEFI"].ToString(), _observacion, "",
                                    _drfila["CodigoSECT"].ToString(), _drfila["Documento"].ToString(), "", "",
                                    int.Parse(_codigocomparar), 0, int.Parse(_drfila["CodigoPROV"].ToString()), int.Parse(_drfila["CodigoCIUD"].ToString()),
                                    0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());

                                break;
                            }
                        }
                    }
                }

                switch (ViewState["Retornar"].ToString())
                {
                    case "0":
                        Response.Redirect("../Gestion/WFrm_ListaClientesAdmin.aspx?MensajeRetornado=Notificación Solicitada..! ", true);
                        break;
                    case "1":
                        Response.Redirect("WFrm_SeguimientoCitacionAdmin.aspx?MensajeRetornado=Notificación Solicitada..! ", true);
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
            switch (ViewState["Retornar"].ToString())
            {
                case "0":
                    Response.Redirect("../Gestion/WFrm_ListaClientesAdmin.aspx", true);
                    break;
                case "1":
                    Response.Redirect("WFrm_SeguimientoCitacionAdmin.aspx?", true);
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
        #endregion

    }
}
