namespace SoftCob.Views.Cedente
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevoDeudor : Page
    {
        #region Variables
        ListItem _itemc = new ListItem();
        ListItem _itemp = new ListItem();
        ListItem _itemg = new ListItem();
        ListItem _itemr = new ListItem();
        DataSet _dts = new DataSet();
        DataTable _dtboperacion = new DataTable();
        DataTable _dtbtelefonos = new DataTable();
        DataTable _dtbdeudor = new DataTable();
        DataTable _tblbuscar = new DataTable();
        DataRow _result, _filagre;
        DataRow[] _resultado;
        CheckBox _chkest = new CheckBox();
        ImageButton _imgeliminar = new ImageButton();
        string _estado = "", _codigocpce = "", _operacion = "", _nuevo = "", _codigo = "", _redirect = "", _mensaje = "";
        int _maxcodigo = 0;
        decimal _totaldeuda, _exigible;
        bool _lexiste = false, _validar = true;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            TxtNumeroDocumento.Attributes.Add("onchange", "Validar_Cedula();");
            TxtTotalDeuda.Attributes.Add("onchange", "ValidarDecimales();");
            TxtExigible.Attributes.Add("onchange", "ValidarDecimales1();");

            if (!IsPostBack)
            {
                TxtFechaNacimiento.Text = DateTime.Now.ToString("MM/dd/yyyy");
                ViewState["CodigoPERS"] = "0";
                ViewState["NumeroDocumento"] = "";
                FunCargarCombos(0);
                TxtFechaNacimiento.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Cliente-Deudor << AGREGAR - ACTUALIZAR >>";

                //if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
                if (Request["MensajeRetornado"] != null)
                {
                    _mensaje = Request["MensajeRetornado"];
                    ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:alertify.set('notifier','position', " +
                        "'top-center'); alertify.success('" + _mensaje + "', 5, function(){console.log('dismissed');});", true);
                }
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

                        DdlCedente.DataSource = new CedenteDAO().FunGetCedentes();
                        DdlCedente.DataTextField = "Descripcion";
                        DdlCedente.DataValueField = "Codigo";
                        DdlCedente.DataBind();

                        _itemp.Text = "--Seleccione Producto--";
                        _itemp.Value = "0";
                        DdlProducto.Items.Add(_itemp);

                        DdlTipoOperacion.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO OPERACION", 
                            "--Seleccione Tipo--", "S");
                        DdlTipoOperacion.DataTextField = "Descripcion";
                        DdlTipoOperacion.DataValueField = "Codigo";
                        DdlTipoOperacion.DataBind();

                        _itemg.Text = "--Seleccione Gestor--";
                        _itemg.Value = "0";
                        DdlGestor.Items.Add(_itemg);

                        _itemr.Text = "--Seleccione Prefijo--";
                        _itemr.Value = "0";
                        DdlPrefijo.Items.Add(_itemr);

                        DdlTipoTelefono.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO TELEFONO", "--Seleccione Tipo--",
                            "S");
                        DdlTipoTelefono.DataTextField = "Descripcion";
                        DdlTipoTelefono.DataValueField = "Codigo";
                        DdlTipoTelefono.DataBind();

                        DdlPropietario.DataSource = new ControllerDAO().FunGetParametroDetalle("PROPIEDAD TELEFONO", 
                            "--Seleccione Propietario--", "S");
                        DdlPropietario.DataTextField = "Descripcion";
                        DdlPropietario.DataValueField = "Codigo";
                        DdlPropietario.DataBind();

                        _dts = new ConsultaDatosDAO().FunConsultaDatos(187,
                            0, 0, 0, "", TxtNumeroDocumento.Text.Trim(), "", Session["Conectar"].ToString());
                        ViewState["DatosOperacion"] = _dts.Tables[1];
                        ViewState["DatosTelefonos"] = _dts.Tables[2];

                        break;
                    case 1:
                        DdlCiudad.DataSource = new ConsultaDatosDAO().FunConsultaDatos(186,
                            1, int.Parse(DdlProvincia.SelectedValue), 0, "", "", "", Session["Conectar"].ToString());
                        DdlCiudad.DataTextField = "Descripcion";
                        DdlCiudad.DataValueField = "Codigo";
                        DdlCiudad.DataBind();
                        break;
                    case 2:
                        DdlProducto.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                        DdlProducto.DataTextField = "CatalogoProducto";
                        DdlProducto.DataValueField = "CodigoCatalogo";
                        DdlProducto.DataBind();

                        DdlGestor.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--",
                            int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                        DdlGestor.DataTextField = "Descripcion";
                        DdlGestor.DataValueField = "Codigo";
                        DdlGestor.DataBind();
                        break;
                    case 3:
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(205, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(), "", Session["Conectar"].ToString());
                        ViewState["DatosGarante"] = _dts.Tables[0];
                        GrdvDeudor.DataSource = _dts;
                        GrdvDeudor.DataBind();

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            TrDeudor.Visible = true;
                        }

                        _dts = new ConsultaDatosDAO().FunConsultaDatos(187, int.Parse(DdlCedente.SelectedValue), 0, 0, "", TxtNumeroDocumento.Text.Trim(), "",
                            Session["Conectar"].ToString());
                        ViewState["DatosOperacion"] = _dts.Tables[1];
                        ViewState["DatosTelefonos"] = _dts.Tables[2];
                        ViewState["NumeroDocumento"] = TxtNumeroDocumento.Text.Trim();

                        TrDatos.Visible = false;
                        TrTelefonos.Visible = false;

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            ViewState["CodigoPERS"] = _dts.Tables[0].Rows[0]["CodigoPERS"].ToString();
                            TxtNombres.Text = _dts.Tables[0].Rows[0]["Nombres"].ToString();
                            TxtApellidos.Text = _dts.Tables[0].Rows[0]["Apellidos"].ToString();
                            TxtFechaNacimiento.Text = _dts.Tables[0].Rows[0]["FechaNaci"].ToString();
                            DdlGenero.SelectedValue = _dts.Tables[0].Rows[0]["Genero"].ToString();
                            DdlProvincia.SelectedValue = _dts.Tables[0].Rows[0]["Provincia"].ToString();
                            FunCargarCombos(1);
                            DdlCiudad.SelectedValue = _dts.Tables[0].Rows[0]["Ciudad"].ToString();
                            DdlEstCivil.SelectedValue = _dts.Tables[0].Rows[0]["EstCivil"].ToString();
                            ChkEstado.Visible = true;
                            LblEstado.Visible = true;
                            ChkEstado.Text = _dts.Tables[0].Rows[0]["Estado"].ToString();
                            ChkEstado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                            TxtDirDomicilio.Text = _dts.Tables[0].Rows[0]["DirDomi"].ToString();
                            TxtRefDomicilio.Text = _dts.Tables[0].Rows[0]["RefDomi"].ToString();
                            TxtDirTrabajo.Text = _dts.Tables[0].Rows[0]["DirTrabajo"].ToString();
                            TxtRefTrabajo.Text = _dts.Tables[0].Rows[0]["RefTrabajo"].ToString();
                            TxtMailPersonal.Text = _dts.Tables[0].Rows[0]["CorreoPersonal"].ToString();
                            TxtMailEmpresa.Text = _dts.Tables[0].Rows[0]["CorreoEmpresa"].ToString();
                        }

                        if (_dts.Tables[1].Rows.Count > 0)
                        {
                            TrDatos.Visible = true;
                            GrdvDatos.DataSource = _dts.Tables[1];
                            GrdvDatos.DataBind();
                        }

                        if (_dts.Tables[2].Rows.Count > 0)
                        {
                            TrTelefonos.Visible = true;
                            GrdvTelefonos.DataSource = _dts.Tables[2];
                            GrdvTelefonos.DataBind();
                        }
                        break;
                    case 4:
                        DdlPrefijo.DataSource = new ControllerDAO().FunGetParametroDetalle("PREFIJOS", "--Seleccione Prefijo--", "S");
                        DdlPrefijo.DataTextField = "Descripcion";
                        DdlPrefijo.DataValueField = "Codigo";
                        DdlPrefijo.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private bool FunValidarTelefonos()
        {
            try
            {
                if (DdlTipoTelefono.SelectedValue == "CL")
                {
                    if (TxtTelefono.Text.Trim().Length != 10) _validar = false;

                    if (TxtTelefono.Text.Trim().Length == 10)
                    {
                        if (TxtTelefono.Text.Trim().Substring(0, 2) != "09") _validar = false;
                    }
                }

                if (DdlTipoTelefono.SelectedValue == "CN")
                {
                    if (TxtTelefono.Text.Trim().Length != 7) _validar = false;

                    if (TxtTelefono.Text.Trim().Length == 7)
                    {
                        if (TxtTelefono.Text.Trim().Substring(0, 2) == "09") _validar = false;

                        if (TxtTelefono.Text.Trim().Substring(0, 1) == "0") _validar = false;

                        if (TxtTelefono.Text.Trim().Substring(0, 1) == "1") _validar = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
            return _validar;
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
            //FunCargarCombos(3);
        }

        protected void DdlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked == true ? "Activo" : "Inactivo";
        }

        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(2);
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkest = (CheckBox)(e.Row.Cells[7].FindControl("ChkEstOperacion"));
                    _imgeliminar = (ImageButton)(e.Row.Cells[8].FindControl("ImgEliOpera"));
                    _estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    _nuevo = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Nuevo"].ToString();

                    if (_estado == "Activo") _chkest.Checked = true;

                    if (_nuevo == "SI")
                    {
                        _imgeliminar.ImageUrl = "~/Botones/eliminar.png";
                        _imgeliminar.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Datos del Deudor..!", this);
                    return;
                }

                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlProducto.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Producto..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtOperacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Operacion..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtDiasMora.Text.Trim()) || TxtDiasMora.Text.Trim() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Dias Mora..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtTotalDeuda.Text.Trim()) || TxtTotalDeuda.Text.Trim() == "0.00" ||
                    TxtTotalDeuda.Text.Trim() == "0" || TxtTotalDeuda.Text.Trim() == "0.0"
                    || TxtTotalDeuda.Text.Trim() == ".")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Total Deuda..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtExigible.Text.Trim()) || TxtExigible.Text.Trim() == "0.00" ||
                    TxtExigible.Text.Trim() == "0" || TxtExigible.Text.Trim() == "0.0"
                    || TxtExigible.Text.Trim() == ".")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Exigible..!", this);
                    return;
                }

                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione un Gestor..!", this);
                    return;
                }

                if (ViewState["DatosOperacion"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["DatosOperacion"];
                    _result = _tblbuscar.Select("CodigoCPCE='" + DdlProducto.SelectedValue + "' and Operacion='" + TxtOperacion.Text.Trim() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("No. de Obligacion ya Existe..!", this);
                    return;
                }

                TrDatos.Visible = true;
                _dtboperacion = (DataTable)ViewState["DatosOperacion"];
                _filagre = _dtboperacion.NewRow();
                _filagre["Producto"] = DdlProducto.SelectedItem.ToString();
                _filagre["Operacion"] = TxtOperacion.Text.Trim();
                _filagre["DiasMora"] = TxtDiasMora.Text.Trim();
                _filagre["TotalDeuda"] = TxtTotalDeuda.Text.Trim().Replace(".", ",");
                _filagre["Exigible"] = TxtExigible.Text.Trim().Replace(".", ",");
                _filagre["Tipo"] = DdlTipoOperacion.SelectedValue == "0" ? "" : DdlTipoOperacion.SelectedItem.ToString();
                _filagre["CodigoCEDE"] = DdlCedente.SelectedValue;
                _filagre["CodigoCPCE"] = DdlProducto.SelectedValue;
                _filagre["Estado"] = "Activo";
                _filagre["CodigoTIPO"] = DdlTipoOperacion.SelectedValue;
                _filagre["Gestor"] = DdlGestor.SelectedValue;
                _filagre["Nuevo"] = "SI";
                _dtboperacion.Rows.Add(_filagre);
                ViewState["DatosOperacion"] = _dtboperacion;
                GrdvDatos.DataSource = _dtboperacion;
                GrdvDatos.DataBind();

                TxtOperacion.Text = "";
                TxtDiasMora.Text = "0";
                DdlTipoOperacion.SelectedValue = "0";
                TxtTotalDeuda.Text = "0.00";
                TxtExigible.Text = "0.00";
                DdlGestor.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgMod_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Datos del Deudor..!", this);
                    return;
                }

                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlProducto.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Producto..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(TxtOperacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Operacion..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtDiasMora.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Dias Mora..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtTotalDeuda.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Total Deuda..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtExigible.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Exigible..!", this);
                    return;
                }

                if (DdlTipoOperacion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Operacion..!", this);
                    return;
                }

                if (ViewState["DatosOperacion"] != null)
                {
                    if (ViewState["Operacion"].ToString() != TxtOperacion.Text.Trim())
                    {
                        _tblbuscar = (DataTable)ViewState["DatosOperacion"];
                        _result = _tblbuscar.Select("CodigoCPCE='" + DdlCedente.SelectedValue + "' and Operacion='" +
                            ViewState["Operacion"] + "'").FirstOrDefault();

                        if (_result != null) _lexiste = true;
                    }
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("No. de Obligacion ya Existe..!", this);
                    return;
                }

                _totaldeuda = decimal.Parse(TxtTotalDeuda.Text.Trim(), CultureInfo.InvariantCulture);
                _exigible = decimal.Parse(TxtExigible.Text.Trim(), CultureInfo.InvariantCulture);

                _dtboperacion = (DataTable)ViewState["DatosOperacion"];

                _result = _dtboperacion.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() + "' and Operacion='" +
                    ViewState["Operacion"] + "'").FirstOrDefault();
                _result["Producto"] = DdlProducto.SelectedItem.ToString();
                _result["Operacion"] = TxtOperacion.Text.Trim();
                _result["DiasMora"] = TxtDiasMora.Text.Trim();
                _result["TotalDeuda"] = _totaldeuda;
                _result["Exigible"] = _exigible;
                _result["Tipo"] = DdlTipoOperacion.SelectedValue == "C" ? "CREDITO" : "TARJETA";
                _result["CodigoTIPO"] = DdlTipoOperacion.SelectedValue == "T" ? "T" : ViewState["CodigoTIPO"].ToString();
                _result["CodigoCEDE"] = DdlCedente.SelectedValue;
                _result["CodigoCPCE"] = DdlProducto.SelectedValue;
                _result["Estado"] = ViewState["Estado"].ToString();
                _result["Gestor"] = DdlGestor.SelectedValue;
                _dtboperacion.AcceptChanges();
                ViewState["DatosOperacion"] = _dtboperacion;
                GrdvDatos.DataSource = _dtboperacion;
                GrdvDatos.DataBind();
                TxtOperacion.Text = "";
                TxtDiasMora.Text = "0";
                DdlTipoOperacion.SelectedValue = "0";
                TxtTotalDeuda.Text = "0.00";
                TxtExigible.Text = "0.00";
                DdlGestor.SelectedValue = "0";
                ImgMod.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvDatos.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvDatos.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigocpce = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoCPCE"].ToString();
                _operacion = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Operacion"].ToString();
                _dtboperacion = (DataTable)ViewState["DatosOperacion"];
                _result = _dtboperacion.Select("CodigoCPCE='" + _codigocpce + "' and Operacion='" + _operacion + "'").FirstOrDefault();
                ViewState["CodigoCPCE"] = _codigocpce;
                ViewState["Operacion"] = _operacion;
                ViewState["CodigoTIPO"] = _result["CodigoTIPO"].ToString();
                ViewState["Tipo"] = _result["Tipo"].ToString();
                DdlCedente.SelectedValue = _result["CodigoCEDE"].ToString();
                FunCargarCombos(2);
                DdlProducto.SelectedValue = _result["CodigoCPCE"].ToString();
                TxtOperacion.Text = _result["Operacion"].ToString();

                if (_result["CodigoTIPO"].ToString() != "T") DdlTipoOperacion.SelectedValue = "C";
                else DdlTipoOperacion.SelectedValue = "T";

                TxtDiasMora.Text = _result["DiasMora"].ToString();
                TxtTotalDeuda.Text = _result["TotalDeuda"].ToString().Replace(",", ".");
                TxtExigible.Text = _result["Exigible"].ToString().Replace(",", ".");
                DdlGestor.SelectedValue = _result["Gestor"].ToString();
                ViewState["Estado"] = _result["Estado"].ToString();
                ImgMod.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlTipoTelefono_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DdlPrefijo.Items.Clear();
                _itemr.Text = "--Seleccione Prefijo--";
                _itemr.Value = "0";
                DdlPrefijo.Items.Add(_itemr);

                if (DdlTipoTelefono.SelectedValue == "CN") FunCargarCombos(4);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlTipo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCedula.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Cedula..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombres..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtNumOperacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Numero Operacion..!", this);
                    return;
                }

                if (!string.IsNullOrEmpty(TxtMailPerGar.Text.Trim()))
                {
                    if (!new FuncionesDAO().Email_bien_escrito(TxtMailPerGar.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Email Incorrecto..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtMailEmpGar.Text.Trim()))
                {
                    if (!new FuncionesDAO().Email_bien_escrito(TxtMailEmpGar.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Email Personal Incorrecto..!", this);
                        return;
                    }
                }

                if (ViewState["DatosGarante"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["DatosGarante"];
                    _result = _tblbuscar.Select("Cedula='" + TxtCedula.Text.Trim() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("No. de Cedula ya Existe..!", this);
                    return;
                }

                TrDeudor.Visible = true;
                _dtbdeudor = (DataTable)ViewState["DatosGarante"];
                _filagre = _dtbdeudor.NewRow();
                _filagre["Tipo"] = DdlTipo.SelectedItem.ToString();
                _filagre["Cedula"] = TxtCedula.Text.Trim();
                _filagre["Nombres"] = TxtGarante.Text.Trim().ToUpper();
                _filagre["Operacion"] = TxtNumOperacion.Text.Trim();
                _filagre["DirDom"] = TxtDomiGarante.Text.Trim().ToUpper();
                _filagre["RefDom"] = TxtRefGarante.Text.Trim().ToUpper();
                _filagre["DirTrabajo"] = TxtTrabGarante.Text.Trim().ToUpper();
                _filagre["RefTrabajo"] = TxtRefTrabajoGar.Text.Trim().ToUpper();
                _filagre["CorreoPersonal"] = TxtMailPerGar.Text.Trim();
                _filagre["CorreoTrabajo"] = TxtMailEmpGar.Text.Trim();
                _filagre["Nuevo"] = "SI";
                _dtbdeudor.Rows.Add(_filagre);
                ViewState["DatosGarante"] = _dtbdeudor;
                GrdvDeudor.DataSource = _dtbdeudor;
                GrdvDeudor.DataBind();

                DdlTipo.SelectedValue = "0";
                TxtCedula.Text = "";
                TxtGarante.Text = "";
                TxtNumOperacion.Text = "";
                TxtDomiGarante.Text = "";
                TxtRefGarante.Text = "";
                TxtTrabGarante.Text = "";
                TxtRefTrabajoGar.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEditGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlTipo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCedula.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Cedula..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombres..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtNumOperacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Numero Operacion..!", this);
                    return;
                }

                if (!string.IsNullOrEmpty(TxtMailPerGar.Text.Trim()))
                {
                    if (!new FuncionesDAO().Email_bien_escrito(TxtMailPerGar.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Email Personal Incorrecto..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtMailEmpGar.Text.Trim()))
                {
                    if (!new FuncionesDAO().Email_bien_escrito(TxtMailEmpGar.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Email Empresa Incorrecto..!", this);
                        return;
                    }
                }

                _tblbuscar = (DataTable)ViewState["DatosGarante"];

                if (ViewState["Cedula"].ToString() != TxtCedula.Text.Trim())
                {
                    _result = _tblbuscar.Select("Cedula='" + TxtCedula.Text.Trim() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("No. de Cedula ya Existe..!", this);
                    return;
                }

                _result = _tblbuscar.Select("Cedula='" + ViewState["Cedula"].ToString() + "'").FirstOrDefault();
                _result["Tipo"] = DdlTipo.SelectedItem.ToString();
                _result["Cedula"] = TxtCedula.Text.Trim();
                _result["Nombres"] = TxtGarante.Text.Trim().ToUpper();
                _result["Operacion"] = TxtNumOperacion.Text.Trim();
                _result["DirDom"] = TxtDomiGarante.Text.Trim().ToUpper();
                _result["RefDom"] = TxtRefGarante.Text.Trim().ToUpper();
                _result["DirTrabajo"] = TxtTrabGarante.Text.Trim().ToUpper();
                _result["RefTrabajo"] = TxtRefTrabajoGar.Text.Trim().ToUpper();
                _result["CorreoPersonal"] = TxtMailPerGar.Text.Trim();
                _result["CorreoTrabajo"] = TxtMailEmpGar.Text.Trim();
                _tblbuscar.AcceptChanges();
                ViewState["DatosGarante"] = _tblbuscar;
                GrdvDeudor.DataSource = _tblbuscar;
                GrdvDeudor.DataBind();

                DdlTipo.SelectedValue = "0";
                TxtCedula.Text = "";
                TxtGarante.Text = "";
                TxtNumOperacion.Text = "";
                TxtDomiGarante.Text = "";
                TxtRefGarante.Text = "";
                TxtTrabGarante.Text = "";
                TxtRefTrabajoGar.Text = "";
                TxtMailPerGar.Text = "";
                TxtMailEmpGar.Text = "";

                ImgAddGarante.Enabled = true;
                ImgEditGarante.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDeudor.DataKeys[_gvrow.RowIndex].Values["Cedula"].ToString();
                _dtbdeudor = (DataTable)ViewState["DatosGarante"];
                _result = _dtbdeudor.Select("Cedula='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                ViewState["DatosGarante"] = _dtbdeudor;
                GrdvDeudor.DataSource = _dtbdeudor;
                GrdvDeudor.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvDeudor.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvDeudor.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = GrdvDeudor.DataKeys[_gvrow.RowIndex].Values["Cedula"].ToString();
                _dtbdeudor = (DataTable)ViewState["DatosGarante"];
                _result = _dtbdeudor.Select("Cedula='" + _codigo + "'").FirstOrDefault();
                ViewState["Cedula"] = _codigo;
                DdlTipo.SelectedValue = _result["Tipo"].ToString();
                TxtCedula.Text = _result["Cedula"].ToString();
                TxtGarante.Text = _result["Nombres"].ToString();
                TxtNumOperacion.Text = _result["Operacion"].ToString();
                TxtDomiGarante.Text = _result["DirDom"].ToString();
                TxtRefGarante.Text = _result["RefDom"].ToString();
                TxtTrabGarante.Text = _result["DirTrabajo"].ToString();
                TxtRefTrabajoGar.Text = _result["RefTrabajo"].ToString();
                TxtMailPerGar.Text = _result["CorreoPersonal"].ToString();
                TxtMailEmpGar.Text = _result["CorreoTrabajo"].ToString();

                ImgAddGarante.Enabled = false;
                ImgEditGarante.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliOpera_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _operacion = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Operacion"].ToString();
                _dtboperacion = (DataTable)ViewState["DatosOperacion"];
                _result = _dtboperacion.Select("Operacion='" + _operacion + "'").FirstOrDefault();
                _result.Delete();
                ViewState["DatosOperacion"] = _dtboperacion;
                GrdvDatos.DataSource = _dtboperacion;
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            FunCargarCombos(3);
        }

        protected void GrdvDeudor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgeliminar = (ImageButton)(e.Row.Cells[5].FindControl("ImgEliGarante"));
                    _nuevo = GrdvDeudor.DataKeys[e.Row.RowIndex].Values["Nuevo"].ToString();

                    if (_nuevo == "SI")
                    {
                        _imgeliminar.ImageUrl = "~/Botones/eliminar.png";
                        _imgeliminar.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlPropietario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtNomReferencia.Text = "";
                TxtApeReferencia.Text = "";
                TxtNomReferencia.Enabled = false;
                TxtApeReferencia.Enabled = false;

                if (DdlPropietario.SelectedValue != "DE" && DdlPropietario.SelectedValue != "0")
                {
                    TxtNomReferencia.Enabled = true;
                    TxtApeReferencia.Enabled = true;
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
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString();
                _dtbtelefonos = (DataTable)ViewState["DatosTelefonos"];
                _result = _dtbtelefonos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                _dtbtelefonos.AcceptChanges();
                GrdvTelefonos.DataSource = _dtbtelefonos;
                GrdvTelefonos.DataBind();

                if (_dtbtelefonos.Rows.Count == 0) TrTelefonos.Visible = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvTelefonos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _imgeliminar = (ImageButton)(e.Row.Cells[5].FindControl("ImgEliminar"));
                    _nuevo = GrdvTelefonos.DataKeys[e.Row.RowIndex].Values["Nuevo"].ToString();

                    if (_nuevo == "SI")
                    {
                        _imgeliminar.ImageUrl = "~/Botones/eliminar.png";
                        _imgeliminar.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddTel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlTipoTelefono.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Telefono..!", this);
                    return;
                }

                if (DdlPropietario.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Propietario..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese numero de telefono..!", this);
                    return;
                }

                if (DdlTipoTelefono.SelectedValue == "CN")
                {
                    if (DdlPrefijo.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Prefijo..!", this);
                        return;
                    }
                }

                if (FunValidarTelefonos())
                {
                    if (ViewState["DatosTelefonos"] != null)
                    {
                        _tblbuscar = (DataTable)ViewState["DatosTelefonos"];

                        if (_tblbuscar.Rows.Count > 0)
                            _maxcodigo = _tblbuscar.AsEnumerable()
                                .Max(row => int.Parse((string)row["Codigo"]));
                        else _maxcodigo = 0;

                        _result = _tblbuscar.Select("CodigoCEDE='" + DdlCedente.SelectedValue + "' and Prefijo='" + DdlPrefijo.SelectedValue + "' and Telefono='" + TxtTelefono.Text.Trim() + "'").FirstOrDefault();

                        if (_result != null) _lexiste = true;
                    }

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("No. de Telefono ya Existe..!", this);
                        return;
                    }

                    TrTelefonos.Visible = true;
                    _dtbtelefonos = (DataTable)ViewState["DatosTelefonos"];
                    _filagre = _dtbtelefonos.NewRow();
                    _filagre["CodigoCEDE"] = DdlCedente.SelectedValue;
                    _filagre["Codigo"] = _maxcodigo + 1;
                    _filagre["Telefono"] = TxtTelefono.Text.Trim();
                    _filagre["Tipo"] = DdlTipoTelefono.SelectedItem.ToString();
                    _filagre["CodTipo"] = DdlTipoTelefono.SelectedValue;
                    _filagre["Propietario"] = DdlPropietario.SelectedItem.ToString();
                    _filagre["CodPro"] = DdlPropietario.SelectedValue;
                    _filagre["Prefijo"] = DdlPrefijo.SelectedValue == "0" ? "" : DdlPrefijo.SelectedValue;
                    _filagre["Nombres"] = TxtNomReferencia.Text.Trim().ToUpper();
                    _filagre["Apellidos"] = TxtApeReferencia.Text.Trim().ToUpper();
                    _filagre["NomApe"] = TxtNomReferencia.Text.Trim().ToUpper() + " " + TxtApeReferencia.Text.Trim().ToUpper();
                    _filagre["Nuevo"] = "SI";
                    _dtbtelefonos.Rows.Add(_filagre);
                    ViewState["DatosTelefonos"] = _dtbtelefonos;
                    GrdvTelefonos.DataSource = _dtbtelefonos;
                    GrdvTelefonos.DataBind();
                    DdlTipoTelefono.SelectedValue = "0";
                    DdlPropietario.SelectedValue = "0";
                    TxtTelefono.Text = "";
                    TxtNomReferencia.Text = "";
                    TxtApeReferencia.Text = "0";
                }
                else new FuncionesDAO().FunShowJSMessage("Telefono incorrecto..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstOperacion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkest = (CheckBox)(_gvrow.Cells[7].FindControl("ChkEstOperacion"));
                _operacion = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Operacion"].ToString();
                _codigocpce = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoCPCE"].ToString();
                _dtboperacion = (DataTable)ViewState["DatosOperacion"];

                _result = _dtboperacion.Select("CodigoCPCE='" + _codigocpce + "' and Operacion='" + _operacion + "'").FirstOrDefault();
                _result["Estado"] = _chkest.Checked ? "Activo" : "Inactivo";
                _dtboperacion.AcceptChanges();
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
                if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese No. Documento del Deudor..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtNombres.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombres del Deudor..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtApellidos.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Apellidos del Deudor..!", this);
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

                if (!string.IsNullOrEmpty(TxtMailPersonal.Text.Trim()))
                {
                    if (!new FuncionesDAO().Email_bien_escrito(TxtMailPersonal.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Email Personal Incorrecto..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtMailEmpresa.Text.Trim()))
                {
                    if (!new FuncionesDAO().Email_bien_escrito(TxtMailEmpresa.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Email Empresa Incorrecto..!", this);
                        return;
                    }
                }

                _dtboperacion = (DataTable)ViewState["DatosOperacion"];
                _dtbtelefonos = (DataTable)ViewState["DatosTelefonos"];
                _dtbdeudor = (DataTable)ViewState["DatosGarante"];

                if (_dtboperacion.Rows.Count == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese al menos una operacion..!", this);
                    return;
                }

                if (ViewState["NumeroDocumento"].ToString() != TxtNumeroDocumento.Text.Trim())
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(175, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(),
                        "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Numero de Documento ya existe..!", this);
                        return;
                    }
                }

                if (_dtboperacion != null)
                {
                    foreach (DataRow _drfila in _dtboperacion.Rows)
                    {
                        _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(0, DdlTipoDocumento.SelectedValue,
                            TxtNumeroDocumento.Text.Trim(), TxtNombres.Text.Trim().ToUpper(), TxtApellidos.Text.Trim().ToUpper(),
                            TxtFechaNacimiento.Text.Trim(), DdlGenero.SelectedValue, int.Parse(DdlProvincia.SelectedValue),
                            int.Parse(DdlCiudad.SelectedValue), DdlEstCivil.SelectedValue, _drfila["Estado"].ToString(),
                            int.Parse(_drfila["CodigoCPCE"].ToString()), _drfila["Operacion"].ToString(),
                           _drfila["TotalDeuda"].ToString(), _drfila["Exigible"].ToString(), int.Parse(_drfila["DiasMora"].ToString()),
                           int.Parse(_drfila["Gestor"].ToString()),
                            _drfila["CodigoTIPO"].ToString(), _drfila["Tipo"].ToString(), "", "", "", "", "",
                            TxtDirDomicilio.Text.Trim().ToUpper(), TxtRefDomicilio.Text.Trim().ToUpper(),
                            TxtDirTrabajo.Text.Trim().ToUpper(), TxtRefTrabajo.Text.Trim().ToUpper(),
                            int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                            ChkEstado.Checked ? "Activo" : "Inactivo", TxtMailPersonal.Text.Trim().ToLower(), TxtMailEmpresa.Text.Trim().ToLower(),
                            int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0, Session["Conectar"].ToString());
                    }
                }

                if (_dtbtelefonos != null)
                {
                    _resultado = _dtbtelefonos.Select("Nuevo='SI'");

                    foreach (DataRow _drfila in _resultado)
                    {
                        _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(1, "", TxtNumeroDocumento.Text.Trim(),
                            _drfila["Nombres"].ToString(), _drfila["Apellidos"].ToString(), "", "", 0, 0, "", "",
                            int.Parse(_drfila["CodigoCEDE"].ToString()), "", "0", "0", 0, 0, "", "", _drfila["CodTipo"].ToString(),
                            _drfila["Prefijo"].ToString(), _drfila["Telefono"].ToString(), _drfila["CodPro"].ToString(), "", "", "", "", "",
                            int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                            "", "", "", 0, 0, 0, Session["Conectar"].ToString());
                    }
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(206, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(), "",
                    Session["Conectar"].ToString());

                if (_dtbdeudor != null)
                {
                    foreach (DataRow _drfila in _dtbdeudor.Rows)
                    {
                        _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(2, "", TxtNumeroDocumento.Text.Trim(),
                            "", "", "", "", 0, 0, "", "", 0, _drfila["Operacion"].ToString(), "0", "0", 0, 0,
                            "", _drfila["Tipo"].ToString(), "", "", "", "", _drfila["Nombres"].ToString(), _drfila["DirDom"].ToString(),
                            _drfila["RefDom"].ToString(), _drfila["DirTrabajo"].ToString(), _drfila["RefTrabajo"].ToString(),
                            int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                            _drfila["Cedula"].ToString(), _drfila["CorreoPersonal"].ToString(), _drfila["CorreoTrabajo"].ToString(),
                            0, 0, 0, Session["Conectar"].ToString());
                    }
                }

                _redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Registro Grabado con Exito..!");
                Response.Redirect(_redirect);
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