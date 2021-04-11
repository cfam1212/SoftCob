namespace SoftCob.Views.Cedente
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_UpdateDeudor : Page
    {
        #region Variables
        ListItem _itemc = new ListItem();
        DataSet _dts = new DataSet();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            //TxtNumeroDocumento.Attributes.Add("onchange", "Validar_Cedula();");
            if (!IsPostBack)
            {
                TxtFechaNacimiento.Text = DateTime.Now.ToString("MM/dd/yyyy");
                ViewState["CodigoPERS"] = Request["CodigoPERS"];
                TxtFechaNacimiento.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Actualizar Cliente-Deudor";
                FunCargarCombos(0);
                FunCargarCombos(2);

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        DdlTipoDocumento.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO DOCUMENTO", 
                            "--Seleccione Tipo--", "S");
                        DdlTipoDocumento.DataTextField = "Descripcion";
                        DdlTipoDocumento.DataValueField = "Codigo";
                        DdlTipoDocumento.DataBind();

                        DdlGenero.DataSource = new ControllerDAO().FunGetParametroDetalle("GENERO", "--Seleccione Género--", "S");
                        DdlGenero.DataTextField = "Descripcion";
                        DdlGenero.DataValueField = "Codigo";
                        DdlGenero.DataBind();

                        DdlProvincia.DataSource = new ConsultaDatosDAO().FunConsultaDatos(186,
                            0, 0, 0, "", "", "", Session["Conectar"].ToString());
                        DdlProvincia.DataTextField = "Descripcion";
                        DdlProvincia.DataValueField = "Codigo";
                        DdlProvincia.DataBind();

                        _itemc.Text = "--Seleccione Ciudad--";
                        _itemc.Value = "0";
                        DdlCiudad.Items.Add(_itemc);

                        DdlEstCivil.DataSource = new ControllerDAO().FunGetParametroDetalle("ESTADO CIVIL", 
                            "--Seleccione Est. Civil--", "S");
                        DdlEstCivil.DataTextField = "Descripcion";
                        DdlEstCivil.DataValueField = "Codigo";
                        DdlEstCivil.DataBind();
                        break;
                    case 1:
                        DdlCiudad.DataSource = new ConsultaDatosDAO().FunConsultaDatos(186,
                            1, int.Parse(DdlProvincia.SelectedValue), 0, "", "", "", Session["Conectar"].ToString());
                        DdlCiudad.DataTextField = "Descripcion";
                        DdlCiudad.DataValueField = "Codigo";
                        DdlCiudad.DataBind();
                        break;
                    case 2:
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(32, int.Parse(ViewState["CodigoPERS"].ToString()), 0,
                            0, "", "", "", Session["Conectar"].ToString());

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            DdlTipoDocumento.SelectedValue = _dts.Tables[0].Rows[0]["Tipodocumento"].ToString();
                            TxtNumeroDocumento.Text = _dts.Tables[0].Rows[0]["Numdocumento"].ToString();
                            ViewState["Numdocumento"] = _dts.Tables[0].Rows[0]["Numdocumento"].ToString();
                            TxtNombres.Text = _dts.Tables[0].Rows[0]["Nombres"].ToString();
                            TxtApellidos.Text = _dts.Tables[0].Rows[0]["Apellidos"].ToString();
                            TxtFechaNacimiento.Text = _dts.Tables[0].Rows[0]["FechaNaci"].ToString();
                            DdlGenero.SelectedValue = _dts.Tables[0].Rows[0]["Genero"].ToString();
                            DdlProvincia.SelectedValue = _dts.Tables[0].Rows[0]["CodProv"].ToString();
                            FunCargarCombos(1);
                            DdlCiudad.SelectedValue = _dts.Tables[0].Rows[0]["CodCiudad"].ToString();
                            DdlEstCivil.SelectedValue = _dts.Tables[0].Rows[0]["EstCivil"].ToString();
                            Txtdirdomicilio.Text = _dts.Tables[0].Rows[0]["Dirdomicilio"].ToString();
                            Txtrefdomicilio.Text = _dts.Tables[0].Rows[0]["Refdomicilio"].ToString();
                            Txtdirtrabajo.Text = _dts.Tables[0].Rows[0]["Dirtrabajo"].ToString();
                            TxtReftrabajo.Text = _dts.Tables[0].Rows[0]["Reftrabajo"].ToString();
                            TxtCorreo.Text = _dts.Tables[0].Rows[0]["Correo"].ToString();
                            TxtMailEmpresa.Text = _dts.Tables[0].Rows[0]["CorreoTrab"].ToString();
                        }
                        break;
                    case 3:
                        if (ViewState["Numdocumento"].ToString() != TxtNumeroDocumento.Text.Trim())
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(175, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(),
                                "", Session["Conectar"].ToString());

                            if (_dts.Tables[0].Rows.Count > 0)
                            {
                                new FuncionesDAO().FunShowJSMessage("Numero de Documento ya existe..!", this);
                                TxtNumeroDocumento.Text = ViewState["Numdocumento"].ToString();
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtNumeroDocumento.Text = "";
                if (DdlTipoDocumento.SelectedValue == "C")
                {
                    TxtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.ValidChars;
                    TxtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-";
                    TxtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                }
                else
                {
                    TxtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.InvalidChars;
                    TxtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-*/{{}}[[]]\\'?¡¿+&";
                    TxtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void TxtNumeroDocumento_TextChanged(object sender, EventArgs e)
        {
            FunCargarCombos(3);
        }

        protected void DdlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlTipoDocumento.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Documento..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese No. Documento..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtNombres.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombres..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtApellidos.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Apellidos..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtFechaNacimiento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Fecha de nacimiento..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaNacimiento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha de nacimiento incorrecta..!", this);
                    return;
                }

                DateTime dtmfechanac = DateTime.ParseExact(String.Format("{0}", TxtFechaNacimiento.Text),
                     "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtmfehaact = DateTime.ParseExact(String.Format("{0}", DateTime.Now.ToString("MM/dd/yyyy")),
                     "MM/dd/yyyy", CultureInfo.InvariantCulture);

                if (dtmfehaact <= dtmfechanac)
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha no puede ser menor a la actual..!", this);
                    return;
                }

                if (DdlGenero.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Genero..!", this);
                    return;
                }

                if (DdlProvincia.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Provincia..!", this);
                    return;
                }

                if (DdlCiudad.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Ciudad..!", this);
                    return;
                }

                if (DdlEstCivil.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Estado Civil..!", this);
                    return;
                }

                if (!string.IsNullOrEmpty(TxtCorreo.Text.Trim()))
                {
                    if (!new FuncionesDAO().Email_bien_escrito(TxtCorreo.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Email incorrecto..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtMailEmpresa.Text.Trim()))
                {
                    if (!new FuncionesDAO().Email_bien_escrito(TxtMailEmpresa.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Email Empresa incorrecto..!", this);
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunUpdateDeudor(0, int.Parse(ViewState["CodigoPERS"].ToString()),
                    DdlTipoDocumento.SelectedValue, TxtNumeroDocumento.Text.Trim(),
                    TxtNombres.Text.Trim().ToUpper(), TxtApellidos.Text.Trim().ToUpper(), TxtFechaNacimiento.Text.Trim(),
                    DdlGenero.SelectedValue, DdlEstCivil.SelectedValue, int.Parse(DdlProvincia.SelectedValue),
                    int.Parse(DdlCiudad.SelectedValue), Txtdirdomicilio.Text.Trim().ToUpper(),
                    Txtrefdomicilio.Text.Trim().ToUpper(), Txtdirtrabajo.Text.Trim().ToUpper(),
                    TxtReftrabajo.Text.Trim().ToUpper(), TxtCorreo.Text.Trim(), TxtMailEmpresa.Text.Trim(), "", "", 0, 0, 0,
                    int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());

                ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }
        #endregion
    }
}