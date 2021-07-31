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

    public partial class WFrm_NewDeudor : Page
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
        DataTable _dtbdirtitular = new DataTable();
        DataTable _dtbcorreo = new DataTable();
        DataTable _dtbcorreogarante = new DataTable();
        DataTable _dtbgarante = new DataTable();
        DataTable _dtbdirgarante = new DataTable();
        DataTable _tblbuscar = new DataTable();
        DataRow _result, _filagre;
        CheckBox _chkest = new CheckBox();
        ImageButton _imggestion = new ImageButton();
        string _estado = "", _codigocpce = "", _operacion = "", _nuevo = "", _codigo = "", _cedula = "", _mensaje = "", _response = "",
            _codigocede = "", _codigoclde = "";
        int _maxcodigo = 0, _perscodigo = 0;
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
            //TxtNumeroDocumento.Attributes.Add("onchange", "Validar_Cedula();");
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

                if (Request["MensajeRetornado"] != null)
                {
                    _mensaje = Request["MensajeRetornado"];
                    new FuncionesDAO().FunShowJSMessage(_mensaje, this, "S", "R");
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

                        DdlGenero.DataSource = new ControllerDAO().FunGetParametroDetalle("GENERO",
                            "--Seleccione Género--", "S");
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

                        DdlTipoTelefono.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO TELEFONO",
                            "--Seleccione Tipo--", "S");
                        DdlTipoTelefono.DataTextField = "Descripcion";
                        DdlTipoTelefono.DataValueField = "Codigo";
                        DdlTipoTelefono.DataBind();

                        DdlPropietario.DataSource = new ControllerDAO().FunGetParametroDetalle("PROPIEDAD TELEFONO",
                            "--Seleccione Propietario--", "S");
                        DdlPropietario.DataTextField = "Descripcion";
                        DdlPropietario.DataValueField = "Codigo";
                        DdlPropietario.DataBind();

                        DdlDirTitular.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO DIRECCION",
                            "--Seleccione Tipo--", "S");
                        DdlDirTitular.DataTextField = "Descripcion";
                        DdlDirTitular.DataValueField = "Codigo";
                        DdlDirTitular.DataBind();

                        DdlMailTitular.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO CORREO",
                            "--Seleccione Tipo--", "S");
                        DdlMailTitular.DataTextField = "Descripcion";
                        DdlMailTitular.DataValueField = "Codigo";
                        DdlMailTitular.DataBind();

                        DdlTipoGarante.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO CLIENTE",
                            "--Seleccione Tipo--", "S");
                        DdlTipoGarante.DataTextField = "Descripcion";
                        DdlTipoGarante.DataValueField = "Codigo";
                        DdlTipoGarante.DataBind();

                        DdlDirGarante.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO DIRECCION",
                            "--Seleccione Tipo--", "S");
                        DdlDirGarante.DataTextField = "Descripcion";
                        DdlDirGarante.DataValueField = "Codigo";
                        DdlDirGarante.DataBind();

                        DdlMailGarante.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO CORREO",
                            "--Seleccione Tipo--", "S");
                        DdlMailGarante.DataTextField = "Descripcion";
                        DdlMailGarante.DataValueField = "Codigo";
                        DdlMailGarante.DataBind();

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

                        DdlGestor.DataSource = new ConsultaDatosDAO().FunConsultaDatos(12, int.Parse(DdlCedente.SelectedValue), 1, 0,
                            "", "", "", Session["Conectar"].ToString());
                        DdlGestor.DataTextField = "Descripcion";
                        DdlGestor.DataValueField = "Codigo";
                        DdlGestor.DataBind();
                        break;
                    case 3:
                        TrActualizar.Visible = false;
                        BtnGrabar.Visible = false;
                        ViewState["CodigoPERS"] = "0";
                        DdlDirTitular.SelectedValue = "0";
                        TxtDirTitular.Text = "";
                        TxtRefTitular.Text = "";
                        DdlMailTitular.SelectedValue = "0";
                        TxtMailTitular.Text = "";
                        DdlCedente.SelectedValue = "0";
                        DdlProducto.Items.Clear();
                        _itemp.Text = "--Seleccione Producto--";
                        _itemp.Value = "0";
                        DdlProducto.Items.Add(_itemp);
                        TxtOperacion.Text = "";
                        DdlTipoOperacion.SelectedValue = "0";
                        TxtDiasMora.Text = "0";
                        TxtTotalDeuda.Text = "0.00";
                        TxtExigible.Text = "0.00";
                        DdlGestor.SelectedValue = "0";
                        DdlTipoTelefono.SelectedValue = "0";
                        DdlPropietario.SelectedValue = "0";
                        DdlPrefijo.Items.Clear();
                        _itemr.Text = "--Seleccione Prefijo--";
                        _itemr.Value = "0";
                        DdlPrefijo.Items.Add(_itemr);
                        TxtTelefono.Text = "";
                        TxtNomReferencia.Text = "";
                        TxtApeReferencia.Text = "";
                        TxtNomReferencia.Enabled = false;
                        TxtApeReferencia.Enabled = false;
                        DdlTipoGarante.SelectedValue = "0";
                        TxtCedulaGarante.Text = "";
                        TxtGarante.Text = "";
                        TxtApellidoGarante.Text = "";
                        TxtNumOperacion.Text = "";
                        DdlDirGarante.SelectedValue = "0";
                        TxtDirGarante.Text = "";
                        TxtRefGarante.Text = "";
                        ImgEditGarante.Enabled = false;
                        ImgModDirGarante.Enabled = false;
                        GrdvDirecGarante.DataSource = null;
                        GrdvDirecGarante.DataBind();
                        DdlMailGarante.SelectedValue = "0";
                        TxtMailGarante.Text = "";
                        ImgModMailGarante.Enabled = false;
                        GrdvMailGarante.DataSource = null;
                        GrdvMailGarante.DataBind();
                        ImgAddDirGarante.Enabled = false;
                        ImgAddMailGarante.Enabled = false;

                        TxtNombres.Text = "";
                        TxtApellidos.Text = "";
                        DdlGenero.SelectedValue = "0";
                        DdlProvincia.SelectedValue = "0";
                        DdlCiudad.SelectedValue = "0";
                        DdlEstCivil.SelectedValue = "0";
                        TxtFechaNacimiento.Text = DateTime.Now.ToString("MM/dd/yyyy");
                        ChkEstado.Checked = true;
                        ChkEstado.Text = "Activo";

                        _dts = new ConsultaDatosDAO().FunConsultaDatos(187, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(), "",
                            Session["Conectar"].ToString());

                        ViewState["DatosOperacion"] = _dts.Tables[1];
                        ViewState["DatosTelefonos"] = _dts.Tables[2];
                        ViewState["DireccionTitular"] = _dts.Tables[3];
                        ViewState["CorreoTitular"] = _dts.Tables[4];

                        GrdvDatos.DataSource = _dts.Tables[1];
                        GrdvDatos.DataBind();

                        GrdvTelefonos.DataSource = _dts.Tables[2];
                        GrdvTelefonos.DataBind();

                        GrdvDirecTitular.DataSource = _dts.Tables[3];
                        GrdvDirecTitular.DataBind();

                        GrdvMailTitular.DataSource = _dts.Tables[4];
                        GrdvMailTitular.DataBind();

                        ViewState["NumeroDocumento"] = TxtNumeroDocumento.Text.Trim();

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
                            ChkEstado.Text = _dts.Tables[0].Rows[0]["Estado"].ToString();
                            ChkEstado.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                            TrActualizar.Visible = true;
                        }
                        else BtnGrabar.Visible = true;

                        ImgAddDirTitular.Enabled = true;
                        ImgAddMailTitular.Enabled = true;
                        ImgAddOperacion.Enabled = true;
                        ImgAddTelefono.Enabled = true;
                        ImgAddGarante.Enabled = true;

                        _dts = new ConsultaDatosDAO().FunConsultaDatos(205, int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0, "",
                            TxtNumeroDocumento.Text.Trim(), "", Session["Conectar"].ToString());

                        ViewState["DatosGarante"] = _dts.Tables[0];

                        GrdvGarante.DataSource = _dts.Tables[0];
                        GrdvGarante.DataBind();

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
                    TxtNumeroDocumento.MaxLength = 10;
                }
                else
                {
                    TxtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.InvalidChars;
                    TxtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-*/{{}}[[]]\\'?¡¿+&";
                    TxtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                    TxtNumeroDocumento.MaxLength = 20;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void TxtNumeroDocumento_TextChanged(object sender, EventArgs e)
        {
            //if (DdlTipoDocumento.SelectedValue == "0")
            //{
            //    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Documento..!", this);
            //    return;
            //}

            //if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
            //{
            //    new FuncionesDAO().FunShowJSMessage("Ingrese Numero de Documento..!", this);
            //    return;
            //}

            //FunCargarCombos(3);
        }

        protected void DdlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void ImgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            if (DdlTipoDocumento.SelectedValue == "0")
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Documento..!", this, "W", "C");
                return;
            }

            if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
            {
                new FuncionesDAO().FunShowJSMessage("Ingrese Numero de Documento..!", this, "W", "C");
                return;
            }

            FunCargarCombos(3);
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked == true ? "Activo" : "Inactivo";
        }

        protected void ImgAddDirTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlDirTitular.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDirTitular.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this, "W", "C");
                    return;
                }

                if (ViewState["DireccionTitular"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["DireccionTitular"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoDIGT"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("Direccion='" + TxtDirTitular.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Direccion ya Existe..!", this, "E", "C");
                    return;
                }

                _dtbdirtitular = (DataTable)ViewState["DireccionTitular"];
                _filagre = _dtbdirtitular.NewRow();
                _filagre["CodigoDIGT"] = _maxcodigo + 1;
                _filagre["Tipo"] = DdlDirTitular.SelectedItem.ToString();
                _filagre["TipoCliente"] = "TIT";
                _filagre["Definicion"] = DdlDirTitular.SelectedValue;
                _filagre["Direccion"] = TxtDirTitular.Text.Trim().ToUpper();
                _filagre["Referencia"] = TxtRefTitular.Text.Trim().ToUpper();
                _filagre["Nuevo"] = "SI";

                _dtbdirtitular.Rows.Add(_filagre);
                ViewState["DireccionTitular"] = _dtbdirtitular;
                GrdvDirecTitular.DataSource = _dtbdirtitular;
                GrdvDirecTitular.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(0, 0, TxtNumeroDocumento.Text.Trim(),
                        "DIRECCION", "TIT", DdlDirTitular.SelectedValue, TxtDirTitular.Text.Trim().ToUpper(),
                        TxtRefTitular.Text.Trim().ToUpper(), "", "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());

                    ViewState["DireccionTitular"] = _dts.Tables[0];
                    GrdvDirecTitular.DataSource = _dts.Tables[0];
                    GrdvDirecTitular.DataBind();
                }

                DdlDirTitular.SelectedValue = "0";
                TxtDirTitular.Text = "";
                TxtRefTitular.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecDirTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvDirecTitular.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvDirecTitular.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = GrdvDirecTitular.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();
                _dtbdirtitular = (DataTable)ViewState["DireccionTitular"];
                _result = _dtbdirtitular.Select("CodigoDIGT='" + _codigo + "'").FirstOrDefault();
                ViewState["CodigoDIGT"] = _codigo;
                ViewState["DirecTitular"] = _result["Direccion"].ToString();
                DdlDirTitular.SelectedValue = _result["Definicion"].ToString();
                TxtDirTitular.Text = _result["Direccion"].ToString();
                TxtRefTitular.Text = _result["Referencia"].ToString();

                ImgAddDirTitular.Enabled = false;
                ImgModDirTitular.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModDirTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlDirTitular.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDirTitular.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this, "W", "C");
                    return;
                }

                _dtbdirtitular = (DataTable)ViewState["DireccionTitular"];

                if (ViewState["DirecTitular"].ToString() != TxtDirTitular.Text.Trim())
                {
                    _result = _dtbdirtitular.Select("Direccion='" + TxtDirTitular.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("Direccion ya Existe..!", this, "E", "C");
                        return;
                    }
                }

                _result = _dtbdirtitular.Select("CodigoDIGT='" + ViewState["CodigoDIGT"].ToString() + "'").FirstOrDefault();
                _result["Tipo"] = DdlDirTitular.SelectedItem.ToString();
                _result["Definicion"] = DdlDirTitular.SelectedValue;
                _result["Direccion"] = TxtDirTitular.Text.Trim().ToUpper();
                _result["Referencia"] = TxtRefTitular.Text.Trim().ToUpper();
                _dtbdirtitular.AcceptChanges();
                ViewState["DireccionTitular"] = _dtbdirtitular;
                GrdvDirecTitular.DataSource = _dtbdirtitular;
                GrdvDirecTitular.DataBind();

                ImgAddDirTitular.Enabled = true;
                ImgModDirTitular.Enabled = false;

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(1, int.Parse(ViewState["CodigoDIGT"].ToString()), "",
                        "DIRECCION", "TIT", DdlDirTitular.SelectedValue, TxtDirTitular.Text.Trim().ToUpper(),
                        TxtRefTitular.Text.Trim().ToUpper(), "", "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());
                }

                DdlDirTitular.SelectedValue = "0";
                TxtDirTitular.Text = "";
                TxtRefTitular.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliDirTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDirecTitular.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();
                _nuevo = GrdvDirecTitular.DataKeys[_gvrow.RowIndex].Values["Nuevo"].ToString();
                _dtbdirtitular = (DataTable)ViewState["DireccionTitular"];
                _result = _dtbdirtitular.Select("CodigoDIGT='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                _dtbdirtitular.AcceptChanges();
                ViewState["DireccionTitular"] = _dtbdirtitular;
                GrdvDirecTitular.DataSource = _dtbdirtitular;
                GrdvDirecTitular.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(2, int.Parse(_codigo), "",
                        "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());
                }

                DdlDirTitular.SelectedValue = "0";
                TxtDirTitular.Text = "";
                TxtRefTitular.Text = "";
                ImgAddDirTitular.Enabled = true;
                ImgModDirTitular.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddMailTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlMailTitular.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Email..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtMailTitular.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese E-Mail..!", this, "W", "C");
                    return;
                }

                if (!new FuncionesDAO().Email_bien_escrito(TxtMailTitular.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Email Incorrecto Titular..!", this, "E", "C");
                    return;
                }

                if (ViewState["CorreoTitular"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["CorreoTitular"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoDIGT"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("Email='" + TxtMailTitular.Text.Trim().ToLower() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Correo ya Existe..!", this, "E", "C");
                    return;
                }

                _dtbcorreo = (DataTable)ViewState["CorreoTitular"];
                _filagre = _dtbcorreo.NewRow();
                _filagre["CodigoDIGT"] = _maxcodigo + 1;
                _filagre["Tipo"] = DdlMailTitular.SelectedItem.ToString();
                _filagre["TipoCliente"] = "TIT";
                _filagre["Definicion"] = DdlMailTitular.SelectedValue;
                _filagre["Email"] = TxtMailTitular.Text.Trim().ToLower();
                _filagre["Nuevo"] = "SI";

                _dtbcorreo.Rows.Add(_filagre);
                ViewState["CorreoTitular"] = _dtbcorreo;
                GrdvMailTitular.DataSource = _dtbcorreo;
                GrdvMailTitular.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(3, 0, TxtNumeroDocumento.Text.Trim(),
                        "CORREO", "TIT", DdlMailTitular.SelectedValue, "","",
                        TxtMailTitular.Text.Trim().ToLower(), "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());

                    ViewState["CorreoTitular"] = _dts.Tables[0];
                    GrdvMailTitular.DataSource = _dts.Tables[0];
                    GrdvMailTitular.DataBind();
                }

                DdlMailTitular.SelectedValue = "0";
                TxtMailTitular.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecMailTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvDirecTitular.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvMailTitular.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = GrdvMailTitular.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();
                _dtbcorreo = (DataTable)ViewState["CorreoTitular"];
                _result = _dtbcorreo.Select("CodigoDIGT='" + _codigo + "'").FirstOrDefault();
                ViewState["CodigoDIGT"] = _codigo;
                ViewState["EMailTitular"] = _result["Email"].ToString();
                DdlMailTitular.SelectedValue = _result["Definicion"].ToString();
                TxtMailTitular.Text = _result["Email"].ToString();

                ImgAddMailTitular.Enabled = false;
                ImgModMailTitular.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModMailTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlMailTitular.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Email..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtMailTitular.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese E-Mail..!", this, "W", "C");
                    return;
                }

                if (!new FuncionesDAO().Email_bien_escrito(TxtMailTitular.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Email Incorrecto Titular..!", this, "E", "C");
                    return;
                }

                _dtbcorreo = (DataTable)ViewState["CorreoTitular"];

                if (ViewState["EMailTitular"].ToString() != TxtMailTitular.Text.Trim())
                {
                    _result = _dtbcorreo.Select("Email='" + TxtMailTitular.Text.Trim().ToLower() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("E-Mail ya Existe..!", this, "E", "C");
                        return;
                    }
                }

                _result = _dtbcorreo.Select("CodigoDIGT='" + ViewState["CodigoDIGT"].ToString() + "'").FirstOrDefault();
                _result["Tipo"] = DdlMailTitular.SelectedItem.ToString();
                _result["Definicion"] = DdlMailTitular.SelectedValue;
                _result["Email"] = TxtMailTitular.Text.Trim().ToLower();
                _dtbcorreo.AcceptChanges();
                ViewState["CorreoTitular"] = _dtbcorreo;
                GrdvMailTitular.DataSource = _dtbcorreo;
                GrdvMailTitular.DataBind();

                ImgAddMailTitular.Enabled = true;
                ImgModMailTitular.Enabled = false;

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(4, int.Parse(ViewState["CodigoDIGT"].ToString()), "",
                        "CORREO", "TIT", DdlMailTitular.SelectedValue, "", "", TxtMailTitular.Text.Trim().ToLower(), "", "", "", "",
                        0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                DdlMailTitular.SelectedValue = "0";
                TxtMailTitular.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliMailTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvMailTitular.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();
                _dtbcorreo = (DataTable)ViewState["CorreoTitular"];
                _result = _dtbcorreo.Select("CodigoMATD='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                _dtbcorreo.AcceptChanges();
                ViewState["CorreoTitular"] = _dtbcorreo;
                GrdvMailTitular.DataSource = _dtbcorreo;
                GrdvMailTitular.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(2, int.Parse(_codigo), "",
                        "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());
                }

                DdlMailTitular.SelectedValue = "0";
                TxtMailTitular.Text = "";
                ImgAddMailTitular.Enabled = true;
                ImgModMailTitular.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddOperacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Datos del Deudor..!", this, "W", "C");
                    return;
                }

                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this, "W", "C");
                    return;
                }

                if (DdlProducto.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Producto..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtOperacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Operacion..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDiasMora.Text.Trim()) || TxtDiasMora.Text.Trim() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Dias Mora..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtTotalDeuda.Text.Trim()) || TxtTotalDeuda.Text.Trim() == "0.00" ||
                    TxtTotalDeuda.Text.Trim() == "0" || TxtTotalDeuda.Text.Trim() == "0.0"
                    || TxtTotalDeuda.Text.Trim() == ".")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Total Deuda..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtExigible.Text.Trim()) || TxtExigible.Text.Trim() == "0.00" ||
                    TxtExigible.Text.Trim() == "0" || TxtExigible.Text.Trim() == "0.0"
                    || TxtExigible.Text.Trim() == ".")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Exigible..!", this, "W", "C");
                    return;
                }

                if (DdlGestor.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione un Gestor..!", this, "W", "C");
                    return;
                }

                if (ViewState["DatosOperacion"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["DatosOperacion"];
                    _result = _tblbuscar.Select("CodigoCPCE='" + DdlProducto.SelectedValue + "' and Operacion='" +
                        TxtOperacion.Text.Trim() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("No. de Obligacion ya Existe..!", this, "E", "C");
                    return;
                }

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

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(3, "", "", "", "", "", "", 0, 0, "", "",
                        int.Parse(DdlProducto.SelectedValue), TxtOperacion.Text.Trim(), TxtTotalDeuda.Text.Trim(),
                        TxtExigible.Text.Trim(), int.Parse(TxtDiasMora.Text.Trim()), int.Parse(DdlGestor.SelectedValue),
                        DdlTipoOperacion.SelectedValue, "", "", "", "", "", "", "", "", "", "",
                        int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), "", "", "",
                        int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0, Session["Conectar"].ToString());

                    ViewState["DatosOperacion"] = _dts.Tables[0];
                    GrdvDatos.DataSource = _dts.Tables[0];
                    GrdvDatos.DataBind();
                }

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

        protected void ImgModOperacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Datos del Deudor..!", this, "W", "C");
                    return;
                }

                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this, "W", "C");
                    return;
                }

                if (DdlProducto.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Producto..!", this, "W", "C");
                    return;
                }
                if (string.IsNullOrEmpty(TxtOperacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Operacion..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDiasMora.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Dias Mora..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtTotalDeuda.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Total Deuda..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtExigible.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Exigible..!", this, "W", "C");
                    return;
                }

                if (DdlTipoOperacion.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Operacion..!", this, "W", "C");
                    return;
                }

                if (ViewState["DatosOperacion"] != null)
                {
                    if (ViewState["Operacion"].ToString() != TxtOperacion.Text.Trim())
                    {
                        _tblbuscar = (DataTable)ViewState["DatosOperacion"];
                        _result = _tblbuscar.Select("CodigoCPCE='" + DdlCedente.SelectedValue + "' and Operacion='" +
                            TxtOperacion.Text.Trim() + "'").FirstOrDefault();

                        if (_result != null) _lexiste = true;
                    }
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("No. de Obligacion ya Existe..!", this, "E", "C");
                    return;
                }

                _totaldeuda = decimal.Parse(TxtTotalDeuda.Text.Trim(), CultureInfo.InvariantCulture);
                _exigible = decimal.Parse(TxtExigible.Text.Trim(), CultureInfo.InvariantCulture);

                _dtboperacion = (DataTable)ViewState["DatosOperacion"];

                _result = _dtboperacion.Select("CodigoCPCE='" + ViewState["CodigoCPCE"].ToString() + "' and Operacion='" +
                    ViewState["Operacion"].ToString() + "'").FirstOrDefault();
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

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(4, "", "", "", "", "", "", 0, 0, "", "",
                        int.Parse(DdlProducto.SelectedValue), TxtOperacion.Text.Trim(), TxtTotalDeuda.Text.Trim(),
                        TxtExigible.Text.Trim(), int.Parse(TxtDiasMora.Text.Trim()), int.Parse(DdlGestor.SelectedValue),
                        DdlTipoOperacion.SelectedValue == "T" ? "T" : ViewState["CodigoTIPO"].ToString(),
                        DdlTipoOperacion.SelectedValue == "C" ? "CREDITO" : "TARJETA",
                        "", "", "", "", "", "", "", "", "", int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), "", "", "", int.Parse(ViewState["CodigoCTDE"].ToString()),
                        int.Parse(DdlProducto.SelectedValue), 0, Session["Conectar"].ToString());
                }

                TxtOperacion.Text = "";
                TxtDiasMora.Text = "0";
                DdlTipoOperacion.SelectedValue = "0";
                TxtTotalDeuda.Text = "0.00";
                TxtExigible.Text = "0.00";
                DdlGestor.SelectedValue = "0";

                ImgAddOperacion.Enabled = true;
                ImgModOperacion.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSeleccOperacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvDatos.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvDatos.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoCTDE"].ToString();
                _codigocpce = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoCPCE"].ToString();
                _operacion = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Operacion"].ToString();
                _dtboperacion = (DataTable)ViewState["DatosOperacion"];
                _result = _dtboperacion.Select("CodigoCTDE='" + _codigo + "' and Operacion='" + _operacion + "'").FirstOrDefault();
                ViewState["CodigoCTDE"] = _codigo;
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

                ImgAddOperacion.Enabled = false;
                ImgModOperacion.Enabled = true;
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
                _codigo = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoCTDE"].ToString();
                _operacion = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Operacion"].ToString();
                _codigocpce = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoCPCE"].ToString();
                _dtboperacion = (DataTable)ViewState["DatosOperacion"];
                _result = _dtboperacion.Select("CodigoCPCE='" + _codigocpce + "' and Operacion='"
                    + _operacion + "'").FirstOrDefault();
                _result["Estado"] = _chkest.Checked ? "Activo" : "Inactivo";
                _dtboperacion.AcceptChanges();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(236, int.Parse(_codigo), 0, 0, "",
                    _chkest.Checked ? "Activo" : "Inactivo", "", Session["Conectar"].ToString());
                }

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliOperacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoCTDE"].ToString();
                _operacion = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Operacion"].ToString();
                _codigocpce = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["CodigoCPCE"].ToString();
                _dtboperacion = (DataTable)ViewState["DatosOperacion"];
                _result = _dtboperacion.Select("CodigoCPCE='" + _codigocpce + "' and Operacion='" +
                    _operacion + "'").FirstOrDefault();
                _result.Delete();
                _dtboperacion.AcceptChanges();
                ViewState["DatosOperacion"] = _dtboperacion;
                GrdvDatos.DataSource = _dtboperacion;
                GrdvDatos.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(237, int.Parse(_codigo), 0, 0, "",
                    "", "", Session["Conectar"].ToString());
                }

                ImgAddOperacion.Enabled = true;
                ImgModOperacion.Enabled = false;
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
                    _chkest = (CheckBox)(e.Row.Cells[7].FindControl("ChkEstOperacion"));
                    _imggestion = (ImageButton)(e.Row.Cells[9].FindControl("ImgGestionar"));
                    _estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    _nuevo = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Nuevo"].ToString();

                    if (_estado == "Activo") _chkest.Checked = true;

                    if (_nuevo == "NO")
                    {
                        _imggestion.ImageUrl = "~/Botones/procesarbg.png";
                        _imggestion.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(2);

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

        protected void ImgAddTelefono_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlTipoTelefono.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Telefono..!", this, "W", "C");
                    return;
                }

                if (DdlPropietario.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Propietario..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese numero de telefono..!", this, "W", "C");
                    return;
                }

                if (DdlTipoTelefono.SelectedValue == "CN")
                {
                    if (DdlPrefijo.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Prefijo..!", this, "W", "C");
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
                                .Max(row => int.Parse((string)row["CodigoTELE"]));
                        else _maxcodigo = 0;

                        _result = _tblbuscar.Select("Prefijo='" + DdlPrefijo.SelectedValue + "' and Telefono='" +
                            TxtTelefono.Text.Trim() + "'").FirstOrDefault();

                        if (_result != null) _lexiste = true;
                    }

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("No. de Telefono ya Existe..!", this, "E", "C");
                        return;
                    }

                    _dtbtelefonos = (DataTable)ViewState["DatosTelefonos"];
                    _filagre = _dtbtelefonos.NewRow();
                    _filagre["CodigoCEDE"] = DdlCedente.SelectedValue;
                    _filagre["CodigoTELE"] = _maxcodigo + 1;
                    _filagre["Telefono"] = TxtTelefono.Text.Trim();
                    _filagre["Tipo"] = DdlTipoTelefono.SelectedItem.ToString();
                    _filagre["CodTipo"] = DdlTipoTelefono.SelectedValue;
                    _filagre["Propietario"] = DdlPropietario.SelectedItem.ToString();
                    _filagre["CodPro"] = DdlPropietario.SelectedValue;
                    _filagre["Prefijo"] = DdlPrefijo.SelectedValue == "0" ? "" : DdlPrefijo.SelectedValue;
                    _filagre["Nombres"] = TxtNomReferencia.Text.Trim().ToUpper();
                    _filagre["Apellidos"] = TxtApeReferencia.Text.Trim().ToUpper();
                    _filagre["NomApe"] = TxtNomReferencia.Text.Trim().ToUpper() + " " +
                                        TxtApeReferencia.Text.Trim().ToUpper();
                    _filagre["Nuevo"] = "SI";
                    _dtbtelefonos.Rows.Add(_filagre);
                    ViewState["DatosTelefonos"] = _dtbtelefonos;
                    GrdvTelefonos.DataSource = _dtbtelefonos;
                    GrdvTelefonos.DataBind();

                    if (ViewState["CodigoPERS"].ToString() != "0")
                    {
                        _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(5, "", TxtTelefono.Text.Trim(), TxtNomReferencia.Text.Trim().ToUpper(),
                            TxtApeReferencia.Text.Trim().ToUpper(), "", "", 0, 0, "", "", 1, "", "0", "0", 0, 0, "", "",
                            DdlTipoTelefono.SelectedValue, DdlPrefijo.SelectedValue == "0" ? "" : DdlPrefijo.SelectedValue,
                            TxtTelefono.Text.Trim(), DdlPropietario.SelectedValue, "", "", "", "", "",
                            int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), "", "", "",
                            int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0, Session["Conectar"].ToString());

                        ViewState["DatosTelefonos"] = _dts.Tables[0];
                        GrdvTelefonos.DataSource = _dts.Tables[0];
                        GrdvTelefonos.DataBind();
                    }

                    DdlTipoTelefono.SelectedValue = "0";
                    DdlPropietario.SelectedValue = "0";
                    DdlPrefijo.SelectedValue = "0";
                    TxtTelefono.Text = "";
                    TxtNomReferencia.Text = "";
                    TxtApeReferencia.Text = "0";
                }
                else new FuncionesDAO().FunShowJSMessage("Telefono incorrecto..!", this, "E", "C");
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliTelefono_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvTelefonos.DataKeys[_gvrow.RowIndex].Values["CodigoTELE"].ToString();
                _dtbtelefonos = (DataTable)ViewState["DatosTelefonos"];
                _result = _dtbtelefonos.Select("CodigoTELE='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                _dtbtelefonos.AcceptChanges();
                GrdvTelefonos.DataSource = _dtbtelefonos;
                GrdvTelefonos.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(238, int.Parse(_codigo), 0, 0, "",
                    "", "", Session["Conectar"].ToString());
                }

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
                if (DdlTipoGarante.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtCedulaGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Cedula..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombres..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtNumOperacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Numero Operacion..!", this, "W", "C");
                    return;
                }

                if (ViewState["DatosGarante"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["DatosGarante"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoGART"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("Cedula='" + TxtCedulaGarante.Text.Trim() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("No. de Cedula ya Existe..!", this, "E", "C");
                    return;
                }

                _dtbdeudor = (DataTable)ViewState["DatosGarante"];
                _filagre = _dtbdeudor.NewRow();
                _filagre["CodigoGART"] = _maxcodigo + 1;
                _filagre["CodigoTipo"] = DdlTipoGarante.SelectedValue;
                _filagre["Tipo"] = DdlTipoGarante.SelectedItem.ToString();
                _filagre["Cedula"] = TxtCedulaGarante.Text.Trim();
                _filagre["Nombres"] = TxtGarante.Text.Trim().ToUpper();
                _filagre["Apellidos"] = TxtApellidoGarante.Text.Trim().ToUpper();
                _filagre["Garante"] = TxtGarante.Text.Trim().ToUpper() + "" + TxtApellidoGarante.Text.Trim().ToUpper();
                _filagre["Operacion"] = TxtNumOperacion.Text.Trim();
                _filagre["Nuevo"] = "SI";
                _dtbdeudor.Rows.Add(_filagre);
                ViewState["DatosGarante"] = _dtbdeudor;
                GrdvGarante.DataSource = _dtbdeudor;
                GrdvGarante.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(2, "", TxtCedulaGarante.Text.Trim(), TxtGarante.Text.Trim().ToUpper(),
                        TxtApellidoGarante.Text.Trim().ToUpper(), "", "", 0, 0, "", "", 0, TxtNumOperacion.Text.Trim(), "", "", 0, 0, "",
                        DdlTipoGarante.SelectedValue, "", "", "", "", "", "", "", "", "", int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), "", "", "", int.Parse(ViewState["CodigoPERS"].ToString()),
                        0, 0, Session["Conectar"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(205, int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0, "", "", "",
                        Session["Conectar"].ToString());

                    ViewState["DatosGarante"] = _dts.Tables[0];
                    GrdvGarante.DataSource = _dts.Tables[0];
                    GrdvGarante.DataBind();
                }

                DdlTipoGarante.SelectedValue = "0";
                TxtCedulaGarante.Text = "";
                TxtGarante.Text = "";
                TxtNumOperacion.Text = "";
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

                foreach (GridViewRow _fr in GrdvDatos.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvGarante.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = GrdvGarante.DataKeys[_gvrow.RowIndex].Values["CodigoGART"].ToString();                
                _dtbgarante = (DataTable)ViewState["DatosGarante"];

                _result = _dtbgarante.Select("CodigoGART='" + _codigo + "'").FirstOrDefault();

                ViewState["CodigoGART"] = _codigo;
                ViewState["CedulaGarante"] = _result["Cedula"].ToString();
                ViewState["TipoCliente"] = _result["CodigoTipo"].ToString();

                DdlTipoGarante.SelectedValue = _result["CodigoTipo"].ToString();
                TxtCedulaGarante.Text = _result["Cedula"].ToString();
                TxtGarante.Text = _result["Nombres"].ToString();
                TxtApellidoGarante.Text = _result["Apellidos"].ToString();
                TxtNumOperacion.Text = _result["Operacion"].ToString();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(208, 3, 0, 0, "", ViewState["CedulaGarante"].ToString(), "",
                    Session["Conectar"].ToString());

                ViewState["DireccionGarante"] = _dts.Tables[0];
                GrdvDirecGarante.DataSource = _dts;
                GrdvDirecGarante.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(208, 4, 0, 0, "", ViewState["CedulaGarante"].ToString(), "",
                    Session["Conectar"].ToString());

                ViewState["CorreoGarante"] = _dts.Tables[0];
                GrdvMailGarante.DataSource = _dts;
                GrdvMailGarante.DataBind();

                ImgAddGarante.Enabled = false;
                ImgEditGarante.Enabled = true;
                ImgAddDirGarante.Enabled = true;
                ImgAddMailGarante.Enabled = true;
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
                if (DdlTipoGarante.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtCedulaGarante
                    .Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Cedula..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombres..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtNumOperacion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Numero Operacion..!", this, "W", "C");
                    return;
                }

                if (ViewState["DatosGarante"] != null)
                {
                    if (ViewState["CedulaGarante"].ToString() != TxtCedulaGarante.Text.Trim())
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(107, 0, 0, 0, "", TxtCedulaGarante.Text.Trim(),
                            "", Session["Conectar"].ToString());

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            new FuncionesDAO().FunShowJSMessage("No. de Documento ya Existe..!", this, "E", "C");
                            TxtCedulaGarante.Text = ViewState["CedulaGarante"].ToString();
                            return;
                        }
                        //_tblbuscar = (DataTable)ViewState["DatosGarante"];
                        //_result = _tblbuscar.Select("Cedula='" + TxtCedulaGarante.Text.Trim() + "'").FirstOrDefault();

                        //if (_result != null) _lexiste = true;
                    }
                }
                //if (_lexiste)
                //{
                //    new FuncionesDAO().FunShowJSMessage("Garante ya Existe..!", this);
                //    return;
                //}


                _dtbgarante = (DataTable)ViewState["DatosGarante"];

                _result = _dtbgarante.Select("CodigoGART='" + ViewState["CodigoGART"].ToString() + "'").FirstOrDefault();
                _result["CodigoTipo"] = DdlTipoGarante.SelectedItem.ToString();
                _result["Tipo"] = DdlTipoGarante.SelectedItem.ToString();
                _result["Cedula"] = TxtCedulaGarante.Text.Trim();
                _result["Nombres"] = TxtGarante.Text.Trim().ToUpper();
                _result["Apellidos"] = TxtApellidoGarante.Text.Trim().ToUpper();
                _result["Garante"] = TxtGarante.Text.Trim().ToUpper() + "" + TxtApellidoGarante.Text.Trim().ToUpper();
                _result["Operacion"] = TxtNumOperacion.Text;
                _dtbgarante.AcceptChanges();

                ViewState["DatosGarante"] = _dtbgarante;
                GrdvGarante.DataSource = _dtbgarante;
                GrdvGarante.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunActualizarDatos(0, int.Parse(ViewState["CodigoGART"].ToString()),
                        TxtCedulaGarante.Text.Trim(), DdlTipoGarante.SelectedValue, TxtGarante.Text.Trim().ToUpper(),
                        TxtApellidoGarante.Text.Trim().ToUpper(), TxtNumOperacion.Text, ViewState["CedulaGarante"].ToString(), "", "", 0, 0, 0, 
                        Session["Conectar"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(205, int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0, "",
                        "", "", Session["Conectar"].ToString());

                    ViewState["DatosGarante"] = _dts.Tables[0];
                    GrdvGarante.DataSource = _dts;
                    GrdvGarante.DataBind();
                }

                DdlTipoGarante.SelectedValue = "0";
                TxtCedulaGarante.Text = "";
                TxtGarante.Text = "";
                TxtApellidoGarante.Text = "";
                TxtNumOperacion.Text = "";

                ImgAddGarante.Enabled = true;
                ImgEditGarante.Enabled = false;
                ImgAddDirGarante.Enabled = true;
                ImgAddMailGarante.Enabled = true;
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
                _codigo = GrdvGarante.DataKeys[_gvrow.RowIndex].Values["CodigoGART"].ToString();
                _dtbgarante = (DataTable)ViewState["DatosGarante"];
                _result = _dtbgarante.Select("CodigoGART='" + _codigo + "'").FirstOrDefault();
                _cedula = _result["Cedula"].ToString();
                _result.Delete();
                _dtbgarante.AcceptChanges();
                GrdvGarante.DataSource = _dtbgarante;
                GrdvGarante.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(239, int.Parse(_codigo), 0, 0, "", _cedula, "",
                        Session["Conectar"].ToString());
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(208, 3, 0, 0, "", ViewState["CedulaGarante"].ToString(), "",
                    Session["Conectar"].ToString());

                ViewState["DireccionGarante"] = _dts.Tables[0];
                GrdvDirecGarante.DataSource = _dts;
                GrdvDirecGarante.DataBind();

                _dts = new ConsultaDatosDAO().FunConsultaDatos(208, 4, 0, 0, "", ViewState["CedulaGarante"].ToString(), "",
                    Session["Conectar"].ToString());

                ViewState["CorreoGarante"] = _dts.Tables[0];
                GrdvMailGarante.DataSource = _dts;
                GrdvMailGarante.DataBind();

                DdlTipoGarante.SelectedValue = "0";
                TxtCedulaGarante.Text = "";
                TxtGarante.Text = "";
                TxtNumOperacion.Text = "";
                ImgAddGarante.Enabled = true;
                ImgEditGarante.Enabled = false;
                ImgAddDirGarante.Enabled = false;
                ImgAddMailGarante.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgGestionar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigocede = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCEDE"].ToString();
            _codigocpce = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCPCE"].ToString();
            _codigoclde = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();
            _perscodigo = int.Parse(GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString());
            _operacion = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Operacion"].ToString();

            Response.Redirect("../Gestion/WFrm_RegLlamadaEntrante.aspx?CodigoCEDE=" + _codigocede + "&CodigoCPCE=" + _codigocpce + "&CodigoCLDE="
                + _codigoclde + "&CodigoPERS=" + _perscodigo + "&NumeroDocumento=" + ViewState["NumeroDocumento"].ToString()
                + "&Operacion=" + _operacion + "&CodigoLTCA=0&CodigoUSU=0&Retornar=6", true);
        }

        protected void LnkActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["NumeroDocumento"].ToString() != TxtNumeroDocumento.Text.Trim())
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(175, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(), "",
                        Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("No. de Documento ya Existe..!", this, "E", "R");
                        return;
                    }
                }

                _dts = new ConsultaDatosDAO().FunUpdateDeudor(0, int.Parse(ViewState["CodigoPERS"].ToString()),
                    DdlTipoDocumento.SelectedValue, TxtNumeroDocumento.Text.Trim(), TxtNombres.Text.Trim().ToUpper(),
                    TxtApellidos.Text.Trim().ToUpper(), TxtFechaNacimiento.Text.Trim(), DdlGenero.SelectedValue, DdlEstCivil.SelectedValue,
                    int.Parse(DdlProvincia.SelectedValue), int.Parse(DdlCiudad.SelectedValue), "", "", "", "", "", "",
                    ChkEstado.Checked ? "Activo" : "Inactivo", ViewState["NumeroDocumento"].ToString(), 0, 0, 0,
                    int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());

                _response = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsoluteUri, "Grabado con Éxito");

                Response.Redirect(_response, true);

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddDirGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlDirGarante.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDirGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this, "W", "C");
                    return;
                }

                if (ViewState["DireccionGarante"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["DireccionGarante"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoDIGT"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("Direccion='" + TxtDirGarante.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Direccion ya Existe..!", this, "E", "C");
                    return;
                }

                _dtbdirgarante = (DataTable)ViewState["DireccionGarante"];
                _filagre = _dtbdirgarante.NewRow();
                _filagre["CodigoDIGT"] = _maxcodigo + 1;
                _filagre["Cedula"] = ViewState["CedulaGarante"].ToString();
                _filagre["Tipo"] = DdlDirGarante.SelectedItem.ToString();
                _filagre["TipoCliente"] = ViewState["TipoCliente"].ToString();
                _filagre["Definicion"] = DdlDirGarante.SelectedValue;
                _filagre["Direccion"] = TxtDirGarante.Text.Trim().ToUpper();
                _filagre["Referencia"] = TxtRefGarante.Text.Trim().ToUpper();
                _filagre["Nuevo"] = "SI";

                _dtbdirgarante.Rows.Add(_filagre);
                ViewState["DireccionGarante"] = _dtbdirgarante;
                GrdvDirecGarante.DataSource = _dtbdirgarante;
                GrdvDirecGarante.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(0, 0, ViewState["CedulaGarante"].ToString(),
                        "DIRECCION", ViewState["TipoCliente"].ToString(), DdlDirGarante.SelectedValue, TxtDirGarante.Text.Trim().ToUpper(),
                        TxtRefGarante.Text.Trim().ToUpper(), "", "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());

                    ViewState["DireccionGarante"] = _dts.Tables[0];
                    GrdvDirecGarante.DataSource = _dts;
                    GrdvDirecGarante.DataBind();
                }

                DdlDirGarante.SelectedValue = "0";
                TxtDirGarante.Text = "";
                TxtRefGarante.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecDirGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvDirecGarante.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvDirecGarante.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = GrdvDirecGarante.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();
                _dtbdirgarante = (DataTable)ViewState["DireccionGarante"];
                _result = _dtbdirgarante.Select("CodigoDIGT='" + _codigo + "'").FirstOrDefault();
                ViewState["CodigoDIGT"] = _codigo;
                ViewState["DirecGarante"] = _result["Direccion"].ToString();
                DdlDirGarante.SelectedValue = _result["Definicion"].ToString();
                TxtDirGarante.Text = _result["Direccion"].ToString();
                TxtRefGarante.Text = _result["Referencia"].ToString();

                ImgAddDirGarante.Enabled = false;
                ImgModDirGarante.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModDirGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlDirGarante.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDirGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this, "W", "C");
                    return;
                }

                _dtbdirgarante = (DataTable)ViewState["DireccionGarante"];

                if (ViewState["DirecGarante"].ToString() != TxtDirGarante.Text.Trim())
                {
                    _result = _dtbdirgarante.Select("Direccion='" + TxtDirGarante.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("Direccion ya Existe..!", this, "E", "C");
                        return;
                    }
                }

                _result = _dtbdirgarante.Select("CodigoDIGT='" + ViewState["CodigoDIGT"].ToString() + "'").FirstOrDefault();
                _result["Tipo"] = DdlDirGarante.SelectedItem.ToString();
                _result["Definicion"] = DdlDirGarante.SelectedValue;
                _result["Direccion"] = TxtDirGarante.Text.Trim().ToUpper();
                _result["Referencia"] = TxtRefGarante.Text.Trim().ToUpper();
                _dtbdirgarante.AcceptChanges();
                ViewState["DireccionGarante"] = _dtbdirgarante;
                GrdvDirecGarante.DataSource = _dtbdirgarante;
                GrdvDirecGarante.DataBind();

                ImgAddDirGarante.Enabled = true;
                ImgModDirGarante.Enabled = false;

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(1, int.Parse(ViewState["CodigoDIGT"].ToString()), "",
                        "DIRECCION", ViewState["TipoCliente"].ToString(), DdlDirGarante.SelectedValue, TxtDirGarante.Text.Trim().ToUpper(),
                        TxtRefGarante.Text.Trim().ToUpper(), "", "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());
                }

                DdlDirGarante.SelectedValue = "0";
                TxtDirGarante.Text = "";
                TxtRefGarante.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliDirGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDirecGarante.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();
                _dtbdirgarante = (DataTable)ViewState["DireccionGarante"];
                _result = _dtbdirgarante.Select("CodigoDIGT='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                _dtbdirgarante.AcceptChanges();
                ViewState["DireccionGarante"] = _dtbdirgarante;
                GrdvDirecGarante.DataSource = _dtbdirgarante;
                GrdvDirecGarante.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(2, int.Parse(_codigo), "",
                        "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());
                }

                DdlDirGarante.SelectedValue = "0";
                TxtDirGarante.Text = "";
                TxtRefGarante.Text = "";
                ImgAddDirGarante.Enabled = true;
                ImgModDirGarante.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAddMailGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlMailGarante.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Email..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtMailGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese E-Mail..!", this, "W", "C");
                    return;
                }

                if (!new FuncionesDAO().Email_bien_escrito(TxtMailGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Email Incorrecto Garante..!", this, "E", "C");
                    return;
                }

                if (ViewState["CorreoGarante"] != null)
                {
                    _tblbuscar = (DataTable)ViewState["CorreoGarante"];

                    if (_tblbuscar.Rows.Count > 0)
                        _maxcodigo = _tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["CodigoDIGT"]));
                    else _maxcodigo = 0;

                    _result = _tblbuscar.Select("Email='" + TxtMailGarante.Text.Trim().ToLower() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;
                }

                if (_lexiste)
                {
                    new FuncionesDAO().FunShowJSMessage("Correo ya Existe..!", this, "E", "C");
                    return;
                }

                _dtbcorreogarante = (DataTable)ViewState["CorreoGarante"];
                _filagre = _dtbcorreogarante.NewRow();
                _filagre["CodigoDIGT"] = _maxcodigo + 1;
                _filagre["Cedula"] = ViewState["CedulaGarante"].ToString();
                _filagre["Tipo"] = DdlMailGarante.SelectedItem.ToString();
                _filagre["TipoCliente"] = ViewState["TipoCliente"].ToString();
                _filagre["Definicion"] = DdlMailGarante.SelectedValue;
                _filagre["Email"] = TxtMailGarante.Text.Trim().ToLower();
                _filagre["Nuevo"] = "SI";

                _dtbcorreogarante.Rows.Add(_filagre);
                ViewState["CorreoGarante"] = _dtbcorreogarante;
                GrdvMailGarante.DataSource = _dtbcorreogarante;
                GrdvMailGarante.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(3, 0, ViewState["CedulaGarante"].ToString(),
                        "CORREO", ViewState["TipoCliente"].ToString(), DdlMailGarante.SelectedValue, "", "", TxtMailGarante.Text.Trim().ToLower(),
                        "", "", "", "", 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());

                    ViewState["CorreoGarante"] = _dts.Tables[0];
                    GrdvMailGarante.DataSource = _dts;
                    GrdvMailGarante.DataBind();
                }

                DdlMailGarante.SelectedValue = "0";
                TxtMailGarante.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecMailGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvMailGarante.Rows)
                {
                    _fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvMailGarante.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = GrdvMailGarante.DataKeys[_gvrow.RowIndex].Values["CodigoDIGT"].ToString();
                _dtbcorreogarante = (DataTable)ViewState["CorreoGarante"];
                _result = _dtbcorreogarante.Select("CodigoDIGT='" + _codigo + "'").FirstOrDefault();
                ViewState["CodigoDIGT"] = _codigo;
                ViewState["EMailGarante"] = _result["Email"].ToString();
                DdlMailGarante.SelectedValue = _result["Definicion"].ToString();
                TxtMailGarante.Text = _result["Email"].ToString();

                ImgAddMailGarante.Enabled = false;
                ImgModMailGarante.Enabled = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModMailGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlMailGarante.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Email..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtMailGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese E-Mail..!", this, "W", "C");
                    return;
                }

                if (!new FuncionesDAO().Email_bien_escrito(TxtMailGarante.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Email Incorrecto Titular..!", this, "E", "C");
                    return;
                }

                _dtbcorreogarante = (DataTable)ViewState["CorreoGarante"];

                if (ViewState["EMailGarante"].ToString() != TxtMailGarante.Text.Trim())
                {
                    _result = _dtbcorreogarante.Select("Email='" + TxtMailGarante.Text.Trim().ToLower() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("E-Mail ya Existe..!", this, "E", "C");
                        return;
                    }
                }

                _result = _dtbcorreogarante.Select("CodigoDIGT='" + ViewState["CodigoDIGT"].ToString() + "'").FirstOrDefault();
                _result["Tipo"] = DdlMailGarante.SelectedItem.ToString();
                _result["Definicion"] = DdlMailGarante.SelectedValue;
                _result["Email"] = TxtMailGarante.Text.Trim().ToLower();
                _dtbcorreogarante.AcceptChanges();
                ViewState["CorreoGarante"] = _dtbcorreogarante;
                GrdvMailGarante.DataSource = _dtbcorreogarante;
                GrdvMailGarante.DataBind();

                ImgAddMailGarante.Enabled = true;
                ImgModMailGarante.Enabled = false;

                if (ViewState["CodigoPERS"].ToString() != "0")
                {

                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(4, int.Parse(ViewState["CodigoDIGT"].ToString()), "",
                        "CORREO", "GAR", DdlMailGarante.SelectedValue, "", "", TxtMailGarante.Text.Trim().ToLower(), "", "", "", "",
                        0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());
                }

                DdlMailGarante.SelectedValue = "0";
                TxtMailGarante.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliMailGarante_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvMailGarante.DataKeys[_gvrow.RowIndex].Values["CodigoMATD"].ToString();
                _dtbcorreogarante = (DataTable)ViewState["CorreoGarante"];
                _result = _dtbcorreogarante.Select("CodigoMATD='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                _dtbcorreogarante.AcceptChanges();
                ViewState["CorreoGarante"] = _dtbcorreogarante;
                GrdvMailGarante.DataSource = _dtbcorreogarante;
                GrdvMailGarante.DataBind();

                if (ViewState["CodigoPERS"].ToString() != "0")
                {
                    _dts = new ConsultaDatosDAO().FunNewDireccionEmail(2, int.Parse(_codigo), "",
                        "", "", "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, "", Session["Conectar"].ToString());
                }

                DdlMailGarante.SelectedValue = "0";
                TxtMailGarante.Text = "";
                ImgAddMailGarante.Enabled = true;
                ImgModMailGarante.Enabled = false;
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
                    new FuncionesDAO().FunShowJSMessage("Ingrese No. Documento del Deudor..!", this, "W", "C");
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(175, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(),
                    "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Numero de Documento ya existe..!", this, "E", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtNombres.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombres del Deudor..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtApellidos.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Apellidos del Deudor..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtFechaNacimiento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Fecha de nacimiento..!", this, "W", "C");
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaNacimiento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha de nacimiento incorrecta..!", this, "E", "C");
                    return;
                }

                DateTime dtmfechanac = DateTime.ParseExact(String.Format("{0}", TxtFechaNacimiento.Text),
                     "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dtmfehaact = DateTime.ParseExact(String.Format("{0}", DateTime.Now.ToString("MM/dd/yyyy")),
                     "MM/dd/yyyy", CultureInfo.InvariantCulture);

                if (dtmfehaact <= dtmfechanac)
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha no puede ser menor a la actual..!", this, "E", "C");
                    return;
                }

                if (DdlGenero.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Genero..!", this, "W", "C");
                    return;
                }

                if (DdlProvincia.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Provincia..!", this, "W", "C");
                    return;
                }

                if (DdlCiudad.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Ciudad..!", this, "W", "C");
                    return;
                }

                if (DdlEstCivil.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Estado Civil..!", this, "W", "C");
                    return;
                }

                _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(0, DdlTipoDocumento.SelectedValue,
                    TxtNumeroDocumento.Text.Trim(), TxtNombres.Text.Trim().ToUpper(), TxtApellidos.Text.Trim().ToUpper(),
                    TxtFechaNacimiento.Text.Trim(), DdlGenero.SelectedValue, int.Parse(DdlProvincia.SelectedValue),
                    int.Parse(DdlCiudad.SelectedValue), DdlEstCivil.SelectedValue, "",
                    0, "", "0", "0", 0, 0, "", "", "", "", "", "", "", "", "", "", "", int.Parse(Session["usuCodigo"].ToString()),
                    Session["MachineName"].ToString(), "", "", "", int.Parse(ViewState["CodigoPERS"].ToString()), 0, 0,
                    Session["Conectar"].ToString());

                _perscodigo = int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString());

                //_dtbdirtitular = (DataTable)ViewState["DireccionTitular"];
                //foreach (DataRow _drfila in _dtbdirtitular.Rows)
                //{
                //    _dts = new ConsultaDatosDAO().FunInsertAdicionales(2, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(),
                //        "DIRECCION", "TIT", _drfila["Definicion"].ToString(), _drfila["Direccion"].ToString(),
                //        _drfila["Referencia"].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                //        "", "", "", "", "", "", "", "", "", "", Session["Conectar"].ToString());
                //}

                //_dtbcorreo = (DataTable)ViewState["CorreoTitular"];
                //foreach (DataRow _drfila in _dtbcorreo.Rows)
                //{
                //    _dts = new ConsultaDatosDAO().FunInsertAdicionales(1, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(),
                //        "CORREO", "TIT", _drfila["Definicion"].ToString(), _drfila["Email"].ToString(),
                //        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                //        "", "", Session["Conectar"].ToString());
                //}

                //_dtboperacion = (DataTable)ViewState["DatosOperacion"];
                //foreach (DataRow _drfila in _dtboperacion.Rows)
                //{
                //    _nuevo = "";
                //    if (_drfila["CodigoTIPO"].ToString() == "C") _nuevo = "CREDITO GSBPO";
                //    if (_drfila["CodigoTIPO"].ToString() == "T") _nuevo = "TARJETA GSBPO";
                //    _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(7, "", "", "", "", "", "", 0, 0, "", "",
                //        int.Parse(_drfila["CodigoCPCE"].ToString()), _drfila["Operacion"].ToString(),
                //        _drfila["TotalDeuda"].ToString(), _drfila["Exigible"].ToString(),
                //        int.Parse(_drfila["DiasMora"].ToString()), int.Parse(_drfila["Gestor"].ToString()),
                //        _drfila["CodigoTIPO"].ToString(), _nuevo,
                //        "", "", "", "", "", "", "", "", "", int.Parse(Session["usuCodigo"].ToString()),
                //        Session["MachineName"].ToString(), "", "", "", _perscodigo, 0, 0, Session["Conectar"].ToString());
                //}

                //_dtbtelefonos = (DataTable)ViewState["DatosTelefonos"];
                //foreach (DataRow _drfila in _dtbtelefonos.Rows)
                //{
                //    _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(8, "", "",
                //        _drfila["Nombres"].ToString(), _drfila["Apellidos"].ToString(), "", "", 0, 0,
                //        "", "", int.Parse(_drfila["CodigoCEDE"].ToString()), "", "0", "0", 0, 0, "", "",
                //        _drfila["CodTipo"].ToString(), _drfila["Prefijo"].ToString(), _drfila["Telefono"].ToString(),
                //        _drfila["CodPro"].ToString(), "", "", "", "", "", int.Parse(Session["usuCodigo"].ToString()),
                //        Session["MachineName"].ToString(), "", "", "", _perscodigo, 0, 0, Session["Conectar"].ToString());
                //}

                //_dtbdeudor = (DataTable)ViewState["DatosGarante"];
                //foreach (DataRow _drfila in _dtbdeudor.Rows)
                //{
                //    _dts = new ConsultaDatosDAO().FunCrearNuevoDeudor(2, "", TxtNumeroDocumento.Text.Trim(), "", "", "", "",
                //        0, 0, "", "", 0, _drfila["Operacion"].ToString(), "", "", 0, 0, "",
                //        _drfila["CodigoTipo"].ToString(), "", "", "", "", _drfila["Nombres"].ToString(), "", "", "", "",
                //        int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                //        TxtCedulaGarante.Text.Trim(), "", "", 0, 0, 0, Session["Conectar"].ToString());
                //}

                //_dtbdirgarante = (DataTable)ViewState["DireccionGarante"];
                //foreach (DataRow _drfila in _dtbdirgarante.Rows)
                //{
                //    _dts = new ConsultaDatosDAO().FunInsertAdicionales(2, 0, 0, 0, "", _drfila["Cedula"].ToString(),
                //        "DIRECCION", "GAR", _drfila["Definicion"].ToString(), _drfila["Direccion"].ToString(),
                //        _drfila["Referencia"].ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                //        "", "", "", "", "", "", "", "", "", "", Session["Conectar"].ToString());
                //}

                //_dtbcorreogarante = (DataTable)ViewState["CorreoGarante"];
                //foreach (DataRow _drfila in _dtbcorreogarante.Rows)
                //{
                //    _dts = new ConsultaDatosDAO().FunInsertAdicionales(1, 0, 0, 0, "", _drfila["Cedula"].ToString(),
                //        "CORREO", "GAR", _drfila["Definicion"].ToString(), _drfila["Email"].ToString(),
                //        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                //        "", "", Session["Conectar"].ToString());
                //}

                //_redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Registro Grabado con Exito..!");
                //Response.Redirect(_redirect);
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