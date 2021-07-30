namespace SoftCob.Views.Cedente
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_UpdateDeudor : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ListItem _itemc = new ListItem();        
        DataTable _dtbdirtitular = new DataTable();
        DataTable _dtbcorreo = new DataTable();
        DataTable _tblbuscar = new DataTable();
        DataRow _result, _filagre;
        //CheckBox _chkest = new CheckBox();
        string _codigo = "", _nuevo = "", _mensaje = "";
        int _maxcodigo = 0;
        bool _lexiste = false;
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
                ViewState["NumDocumento"] = Request["NumDocumento                                                                                                                                                                                                                                 "];
                TxtFechaNacimiento.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Actualizar Cliente-Deudor";
                FunCargarCombos(0);
                FunCargarCombos(2);

                
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
                            TxtNumeroDocumento.Text = _dts.Tables[0].Rows[0]["Cedula"].ToString();
                            ViewState["Numdocumento"] = TxtNumeroDocumento.Text;
                            TxtNombres.Text = _dts.Tables[0].Rows[0]["Nombres"].ToString();
                            TxtApellidos.Text = _dts.Tables[0].Rows[0]["Apellidos"].ToString();
                            TxtFechaNacimiento.Text = _dts.Tables[0].Rows[0]["FechaNaci"].ToString();
                            DdlGenero.SelectedValue = _dts.Tables[0].Rows[0]["Genero"].ToString();
                            DdlProvincia.SelectedValue = _dts.Tables[0].Rows[0]["CodProv"].ToString();
                            FunCargarCombos(1);
                            DdlCiudad.SelectedValue = _dts.Tables[0].Rows[0]["CodCiudad"].ToString();
                            DdlEstCivil.SelectedValue = _dts.Tables[0].Rows[0]["EstCivil"].ToString();

                            _dts = new ConsultaDatosDAO().FunConsultaDatos(187, 0, 0, 0, "", TxtNumeroDocumento.Text.Trim(), "",
                                Session["Conectar"].ToString());

                            ViewState["DireccionTitular"] = _dts.Tables[3];
                            ViewState["CorreoTitular"] = _dts.Tables[4];

                            GrdvDirecTitular.DataSource = _dts.Tables[3];
                            GrdvDirecTitular.DataBind();

                            GrdvMailTitular.DataSource = _dts.Tables[4];
                            GrdvMailTitular.DataBind();
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

        protected void ImgAddDirTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlDirTitular.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this, "N", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDirTitular.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this, "N", "C");
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
                    new FuncionesDAO().FunShowJSMessage("Direccion ya Existe..!", this, "W", "C");
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
        protected void ImgModDirTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlDirTitular.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Direccion..!", this, "N", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDirTitular.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Direccion..!", this, "N", "C");
                    return;
                }

                _dtbdirtitular = (DataTable)ViewState["DireccionTitular"];

                if (ViewState["DirecTitular"].ToString() != TxtDirTitular.Text.Trim())
                {
                    _result = _dtbdirtitular.Select("Direccion='" + TxtDirTitular.Text.Trim().ToUpper() + "'").FirstOrDefault();

                    if (_result != null) _lexiste = true;

                    if (_lexiste)
                    {
                        new FuncionesDAO().FunShowJSMessage("Direccion ya Existe..!", this, "W", "C");
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
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Email..!", this, "N", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtMailTitular.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese E-Mail..!", this, "N", "C");
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
                    new FuncionesDAO().FunShowJSMessage("Correo ya Existe..!", this, "W", "C");
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
                        "CORREO", "TIT", DdlMailTitular.SelectedValue, "", "",
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
        protected void ImgModMailTitular_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlMailTitular.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Email..!", this, "N", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtMailTitular.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese E-Mail..!", this, "N", "C");
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
                        new FuncionesDAO().FunShowJSMessage("E-Mail ya Existe..!", this, "W", "C");
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
        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlTipoDocumento.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Documento..!", this, "N", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtNumeroDocumento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese No. Documento..!", this, "N", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtNombres.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombres..!", this, "N", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtApellidos.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Apellidos..!", this, "N", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtFechaNacimiento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Fecha de nacimiento..!", this, "N", "C");
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
                    new FuncionesDAO().FunShowJSMessage("La Fecha no puede ser menor a la actual..!", this, "W", "C");
                    return;
                }

                if (DdlGenero.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Genero..!", this, "N", "C");
                    return;
                }

                if (DdlProvincia.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Provincia..!", this, "N", "C");
                    return;
                }

                if (DdlCiudad.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Ciudad..!", this, "N", "C");
                    return;
                }

                if (DdlEstCivil.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Estado Civil..!", this, "N", "C");
                    return;
                }

                _dts = new ConsultaDatosDAO().FunUpdateDeudor(0, int.Parse(ViewState["CodigoPERS"].ToString()),
                    DdlTipoDocumento.SelectedValue, TxtNumeroDocumento.Text.Trim(),
                    TxtNombres.Text.Trim().ToUpper(), TxtApellidos.Text.Trim().ToUpper(), TxtFechaNacimiento.Text.Trim(),
                    DdlGenero.SelectedValue, DdlEstCivil.SelectedValue, int.Parse(DdlProvincia.SelectedValue),
                    int.Parse(DdlCiudad.SelectedValue), "", "", "", "", "", "", "", "", 0, 0, 0,
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