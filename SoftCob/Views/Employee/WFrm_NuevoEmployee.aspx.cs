﻿

namespace SoftCob.Views.Employee
{
    using ControllerSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Threading;
    public partial class WFrm_NuevoEmployee : Page
    {
        #region Variables
        string _mensaje = "";
        DataTable _dtbotrosestudios = new DataTable();
        DataTable _dtbidiomas = new DataTable();
        DataTable _dtbexperiencia = new DataTable();
        DataTable _dtbreflaborales = new DataTable();
        DataTable _dtbrefpersonales = new DataTable();
        DataTable _dtbbuscar = new DataTable();
        DataTable _dtbagregar = new DataTable();
        DataTable _dtb = new DataTable();
        DataSet _dts = new DataSet();
        bool _existe = false, _grabestudio = false;
        int _con = 0, _maxcodigo = 0, _codigo = 0;
        DataRow _resultado, _filagre;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            TxtIdentificacion.Attributes.Add("onchange", "Validar_Cedula();");

            if (!IsPostBack)
            {
                if (Session["IN-CALL"].ToString() == "SI")
                {
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                TxtFechaNacimiento.Text = DateTime.Now.ToString("MM/dd/yyyy");
                ViewState["Codigo"] = Request["Codigo"];
                ViewState["Tipo"] = Request["Tipo"];
                ViewState["CodigoEstu"] = "0";

                _dtbotrosestudios.Columns.Add("Codigo");
                _dtbotrosestudios.Columns.Add("Institucion");
                _dtbotrosestudios.Columns.Add("FechaDesde");
                _dtbotrosestudios.Columns.Add("FechaHasta");
                _dtbotrosestudios.Columns.Add("Titulo");
                ViewState["OtrosEstudios"] = _dtbotrosestudios;

                _dtbidiomas.Columns.Add("Codigo");
                _dtbidiomas.Columns.Add("Idioma");
                _dtbidiomas.Columns.Add("NivelH");
                _dtbidiomas.Columns.Add("NivelE");
                _dtbidiomas.Columns.Add("CodigoIdioma");
                ViewState["Idiomas"] = _dtbidiomas;

                _dtbexperiencia.Columns.Add("Codigo");
                _dtbexperiencia.Columns.Add("Empresa");
                _dtbexperiencia.Columns.Add("FecInicio");
                _dtbexperiencia.Columns.Add("FecFin");
                _dtbexperiencia.Columns.Add("Cargo");
                _dtbexperiencia.Columns.Add("Descripcion");
                _dtbexperiencia.Columns.Add("Motivo");
                _dtbexperiencia.Columns.Add("CodigoMotivo");
                ViewState["Experiencia"] = _dtbexperiencia;

                _dtbreflaborales.Columns.Add("Codigo");
                _dtbreflaborales.Columns.Add("Empresa");
                _dtbreflaborales.Columns.Add("Nombre");
                _dtbreflaborales.Columns.Add("Cargo");
                _dtbreflaborales.Columns.Add("Telefono");
                _dtbreflaborales.Columns.Add("Celular");
                _dtbreflaborales.Columns.Add("Email");
                ViewState["RefLaboral"] = _dtbreflaborales;

                _dtbrefpersonales.Columns.Add("Codigo");
                _dtbrefpersonales.Columns.Add("Nombre");
                _dtbrefpersonales.Columns.Add("Parentesco");
                _dtbrefpersonales.Columns.Add("Telefono");
                _dtbrefpersonales.Columns.Add("Celular");
                _dtbrefpersonales.Columns.Add("CodigoParen");
                ViewState["RefPersonal"] = _dtbrefpersonales;
                ViewState["identificacion"] = null;
                FunCargarCombos();

                if (ViewState["Tipo"].ToString() == "N") Lbltitulo.Text = "Nuevo Empleado";
                else
                {
                    Lbltitulo.Text = "Editar Empleado";
                    lblEstado.Visible = true;
                    ChkEstado.Visible = true;
                    FunCargarMantenimiento();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                GSBPO_EMPLOYEE _employee = new EmployeeDAO().FunGetEmployeePorCodigo(int.Parse(ViewState["Codigo"].ToString()));
                DdlTipoDocumento.SelectedValue = _employee.empl_tipodocumento;

                if (DdlTipoDocumento.SelectedValue == "C")
                {
                    txtIdentificacion_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.ValidChars;
                    txtIdentificacion_FilteredTextBoxExtender.InvalidChars = ".-";
                    txtIdentificacion_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                }
                else
                {
                    txtIdentificacion_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.InvalidChars;
                    txtIdentificacion_FilteredTextBoxExtender.InvalidChars = ".-*/{{}}[[]]\\";
                    txtIdentificacion_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                }

                TxtIdentificacion.Text = _employee.empl_identificacion;
                TxtNombres.Text = _employee.empl_nombres;
                TxtApellidos.Text = _employee.empl_apellidos;
                DdlGenero.SelectedValue = _employee.empl_genero;
                DdlEstadoCivil.SelectedValue = _employee.empl_estadocivil;
                DdlNacionalidad.SelectedValue = _employee.empl_nacionalidad;
                TxtFechaNacimiento.Text = _employee.empl_fechanacimiento.ToString("MM/dd/yyyy");
                DdlDepartamento.SelectedValue = _employee.DEPA_CODIGO.ToString();
                DdlTipoSangre.SelectedValue = _employee.emple_tiposangre;
                TxtDireccion.Text = _employee.empl_direccion;
                TxtTelefono1.Text = _employee.empl_telefonocasa1;
                TxtTelefono2.Text = _employee.empl_telefonocasa2;
                TxtCelular1.Text = _employee.empl_celular1;
                TxtCelular2.Text = _employee.empl_celular2;
                TxtEmail1.Text = _employee.empl_email1;
                TxtEmail2.Text = _employee.empl_email2;
                ChkEstado.Checked = _employee.empl_estado;
                ViewState["identificacion"] = TxtIdentificacion.Text;
                ViewState["usucreacion"] = _employee.empl_usuariocreacion;
                ViewState["fechacreacion"] = _employee.empl_fechacreacion;
                ViewState["terminalcreacion"] = _employee.empl_terminalcreacion;
                _dts = new EmployeeDTO().FunGetEstudios(int.Parse(ViewState["Codigo"].ToString()));

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoEstu"] = _dts.Tables[0].Rows[0][0].ToString();
                    TxtPrimaria.Text = _dts.Tables[0].Rows[0][1].ToString();
                    DdlFecIniPrimaria.SelectedValue = _dts.Tables[0].Rows[0][2].ToString();
                    DdlFecFinPrimaria.SelectedValue = _dts.Tables[0].Rows[0][3].ToString();
                    TxtSecundaria.Text = _dts.Tables[0].Rows[0][4].ToString();
                    DdlFecIniSecundaria.SelectedValue = _dts.Tables[0].Rows[0][5].ToString();
                    DdlFecFinSecundaria.SelectedValue = _dts.Tables[0].Rows[0][6].ToString();
                    TxtTituloS.Text = _dts.Tables[0].Rows[0][7].ToString();
                    TxtSuperior.Text = _dts.Tables[0].Rows[0][8].ToString();
                    DdlFecIniSuperior.SelectedValue = _dts.Tables[0].Rows[0][9].ToString();
                    DdlFecFinSuperior.SelectedValue = _dts.Tables[0].Rows[0][10].ToString();
                    TxtTituloR.Text = _dts.Tables[0].Rows[0][11].ToString();
                }

                _dts = new EmployeeDTO().FunGetOtrosEstudios(int.Parse(ViewState["Codigo"].ToString()));

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    GrdvOtrosEstudios.DataSource = _dts;
                    GrdvOtrosEstudios.DataBind();
                    ViewState["OtrosEstudios"] = _dts.Tables[0];
                }

                _dts = new EmployeeDTO().FunGetIdiomas(int.Parse(ViewState["Codigo"].ToString()));

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    GrdvIdiomas.DataSource = _dts;
                    GrdvIdiomas.DataBind();
                    ViewState["Idiomas"] = _dts.Tables[0];
                }

                _dts = new EmployeeDTO().FunGetExperiencia(int.Parse(ViewState["Codigo"].ToString()));

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    GrdvExperiencia.DataSource = _dts;
                    GrdvExperiencia.DataBind();
                    ViewState["Experiencia"] = _dts.Tables[0];
                }

                FunCargarComboEmpresa();

                _dts = new EmployeeDTO().FunGetRefLaboral(int.Parse(ViewState["Codigo"].ToString()));

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    GrdvRefLaboral.DataSource = _dts;
                    GrdvRefLaboral.DataBind();
                    ViewState["RefLaboral"] = _dts.Tables[0];
                }

                _dts = new EmployeeDTO().FunGetRefPersonal(int.Parse(ViewState["Codigo"].ToString()));

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    GrdvRefPersonal.DataSource = _dts;
                    GrdvRefPersonal.DataBind();
                    ViewState["RefPersonal"] = _dts.Tables[0];
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void FunCargarCombos()
        {
            DdlTipoDocumento.DataSource = new CatalogosDTO().FunGetParametroDetalleValor("TIPO DOCUMENTO", "--Seleccione Tipo--");
            DdlTipoDocumento.DataTextField = "Descripcion";
            DdlTipoDocumento.DataValueField = "Codigo";
            DdlTipoDocumento.DataBind();

            DdlGenero.DataSource = new CatalogosDTO().FunGetParametroDetalleValor("GENERO", "--Seleccione Género--");
            DdlGenero.DataTextField = "Descripcion";
            DdlGenero.DataValueField = "Codigo";
            DdlGenero.DataBind();

            DdlEstadoCivil.DataSource = new CatalogosDTO().FunGetParametroDetalleValor("ESTADO CIVIL", "--Seleccione Estado Civil--");
            DdlEstadoCivil.DataTextField = "Descripcion";
            DdlEstadoCivil.DataValueField = "Codigo";
            DdlEstadoCivil.DataBind();

            DdlNacionalidad.DataSource = new CatalogosDTO().FunGetParametroDetalleValor("NACIONALIDAD", "--Seleccione Nacionalidad--");
            DdlNacionalidad.DataTextField = "Descripcion";
            DdlNacionalidad.DataValueField = "Codigo";
            DdlNacionalidad.DataBind();

            DdlTipoSangre.DataSource = new CatalogosDTO().FunGetParametroDetalleValor("TIPO SANGRE", "--Seleccione Tipo Sangre--");
            DdlTipoSangre.DataTextField = "Descripcion";
            DdlTipoSangre.DataValueField = "Codigo";
            DdlTipoSangre.DataBind();

            DdlDepartamento.DataSource = new CatalogosDTO().FunGetDepartamento();
            DdlDepartamento.DataTextField = "Descripcion";
            DdlDepartamento.DataValueField = "Codigo";
            DdlDepartamento.DataBind();

            DdlIdiomas.DataSource = new CatalogosDTO().FunGetParametroDetalleValor("IDIOMAS", "--Seleccione Idioma--");
            DdlIdiomas.DataTextField = "Descripcion";
            DdlIdiomas.DataValueField = "Codigo";
            DdlIdiomas.DataBind();

            DdlMotivo.DataSource = new CatalogosDTO().FunGetParametroDetalleValor("MOTIVOS SALIDA", "--Seleccione Motivo--");
            DdlMotivo.DataTextField = "Descripcion";
            DdlMotivo.DataValueField = "Codigo";
            DdlMotivo.DataBind();

            DdlParentesco.DataSource = new CatalogosDTO().FunGetParametroDetalleValor("PARENTESCO", "--Seleccione Parentesco--");
            DdlParentesco.DataTextField = "Descripcion";
            DdlParentesco.DataValueField = "Codigo";
            DdlParentesco.DataBind();

            ListItem empresa = new ListItem();
            {
                empresa.Text = "--Seleccione Empresa--";
                empresa.Value = "0";
                DdlEmpresa.Items.Add(empresa);
            }

            new CatalogosDTO().FunLlenarDropValores(DdlFecIniPrimaria, 1970, DateTime.Now.Year);
            new CatalogosDTO().FunLlenarDropValores(DdlFecFinPrimaria, 1970, DateTime.Now.Year);
            new CatalogosDTO().FunLlenarDropValores(DdlFecIniSecundaria, 1970, DateTime.Now.Year);
            new CatalogosDTO().FunLlenarDropValores(DdlFecFinSecundaria, 1970, DateTime.Now.Year);
            new CatalogosDTO().FunLlenarDropValores(DdlFecIniSuperior, 1970, DateTime.Now.Year);
            new CatalogosDTO().FunLlenarDropValores(DdlFecFinSuperior, 1970, DateTime.Now.Year);
        }

        protected void FunCargarComboEmpresa()
        {
            DdlEmpresa.Items.Clear();

            if (ViewState["Experiencia"] != null)
            {
                _dtb = (DataTable)ViewState["Experiencia"];

                ListItem _empresa = new ListItem();
                {
                    _empresa.Text = "--Seleccione Empresa--";
                    _empresa.Value = "0";
                    DdlEmpresa.Items.Add(_empresa);
                }

                if (_dtb.Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        ListItem _nuevaempresa = new ListItem();
                        _con++;
                        _nuevaempresa.Text = _dr[1].ToString();
                        _nuevaempresa.Value = _dr[0].ToString();
                        _empresa = DdlEmpresa.Items.FindByText(_dr[1].ToString());

                        if (_empresa == null) DdlEmpresa.Items.Add(_nuevaempresa);
                    }
                }
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtIdentificacion.Text = "";

            if (DdlTipoDocumento.SelectedValue == "C")
            {
                txtIdentificacion_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.ValidChars;
                txtIdentificacion_FilteredTextBoxExtender.InvalidChars = ".-";
                txtIdentificacion_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
            }
            else
            {
                txtIdentificacion_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.InvalidChars;
                txtIdentificacion_FilteredTextBoxExtender.InvalidChars = ".-*/{{}}[[]]\\";
                txtIdentificacion_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Custom;
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtIdentificacion.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese No. de Documento..!", this);
                    return;
                }

                if (DdlTipoDocumento.SelectedItem.ToString() == "CEDULA")
                {
                    if (!new FuncionesDTO().CedulaBienEscrita(TxtIdentificacion.Text.Trim()))
                    {
                        new FuncionesBAS().FunShowJSMessage("No. de Cédula es incorrecto..!", this);
                        return;
                    }
                }

                if (string.IsNullOrEmpty(TxtNombres.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Nombre..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtApellidos.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Apellido..!", this);
                    return;
                }

                if (DdlGenero.SelectedValue == "0")
                {
                    new FuncionesBAS().FunShowJSMessage("Selecciones Género..!", this);
                    return;
                }

                if (DdlEstadoCivil.SelectedValue == "0")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Estado Civil..!", this);
                    return;
                }

                if (DdlNacionalidad.SelectedValue == "0")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Nacionalidad..!", this);
                    return;
                }

                if (DdlDepartamento.SelectedValue == "0")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Departamento..!", this);
                    return;
                }

                if (!string.IsNullOrEmpty(TxtTelefono1.Text))
                {
                    if (TxtTelefono1.Text.Trim().Length < 7 || TxtTelefono1.Text.Substring(0, 2) == "09")
                    {
                        new FuncionesBAS().FunShowJSMessage("Teléfono_1 ingresado es incorrecto..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtTelefono2.Text))
                {
                    if (TxtTelefono2.Text.Trim().Length < 7 || TxtTelefono2.Text.Substring(0, 2) == "09")
                    {
                        new FuncionesBAS().FunShowJSMessage("Teléfono_2 ingresado es incorrecto..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtCelular1.Text))
                {
                    if (TxtCelular1.Text.Trim().Length < 10 || TxtCelular1.Text.Trim().Substring(0, 2) != "09")
                    {
                        new FuncionesBAS().FunShowJSMessage("Celular_1 ingresado es incorrecto..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(TxtCelular2.Text))
                {
                    if (TxtCelular2.Text.Trim().Length < 10 || TxtCelular2.Text.Trim().Substring(0, 2) != "09")
                    {
                        new FuncionesBAS().FunShowJSMessage("Celular_2 ingresado es incorrecto..!", this);
                        return;
                    }
                }

                if (!new FuncionesDTO().IsDate(TxtFechaNacimiento.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Fecha de Nacimiento Incorrecta..! Formato(MM/dd/yyyy)", this);
                    return;
                }

                if (ViewState["identificacion"] != null)
                {
                    if (ViewState["identificacion"].ToString() != TxtIdentificacion.Text)
                    {
                        if (new EmployeeDTO().FunConsultarEmpleadoPorIdentificacion(TxtIdentificacion.Text.Trim()))
                        {
                            new FuncionesBAS().FunShowJSMessage("Empleado con Identificación ya existe..!", this);
                            return;
                        }
                    }
                }

                GSBPO_EMPLOYEE _employee = new GSBPO_EMPLOYEE();
                {
                    _employee.EMPL_CODIGO = int.Parse(ViewState["Codigo"].ToString());
                    _employee.DEPA_CODIGO = int.Parse(DdlDepartamento.SelectedValue);
                    _employee.empl_tipodocumento = DdlTipoDocumento.SelectedValue;
                    _employee.empl_identificacion = TxtIdentificacion.Text.Trim().ToUpper();
                    _employee.empl_nombres = TxtNombres.Text.ToUpper().Trim();
                    _employee.empl_apellidos = TxtApellidos.Text.Trim().ToUpper();
                    _employee.empl_fechanacimiento = DateTime.ParseExact(TxtFechaNacimiento.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    _employee.empl_nacionalidad = DdlNacionalidad.SelectedValue;
                    _employee.empl_genero = DdlGenero.SelectedValue;
                    _employee.empl_estadocivil = DdlEstadoCivil.SelectedValue;
                    _employee.emple_tiposangre = DdlTipoSangre.SelectedValue;
                    _employee.empl_direccion = TxtDireccion.Text.Trim().ToUpper();
                    _employee.empl_referencia = "";
                    _employee.empl_telefonocasa1 = TxtTelefono1.Text;
                    _employee.empl_telefonocasa2 = TxtTelefono2.Text;
                    _employee.empl_celular1 = TxtCelular1.Text;
                    _employee.empl_celular2 = TxtCelular2.Text;
                    _employee.empl_email1 = TxtEmail1.Text.Trim();
                    _employee.empl_email2 = TxtEmail2.Text.Trim();
                    _employee.empl_estado = ChkEstado.Checked;
                    _employee.empl_fum = DateTime.Now;
                    _employee.empl_uum = int.Parse(Session["usuCodigo"].ToString());
                    _employee.empl_tum = Session["MachineName"].ToString();

                    if (ViewState["Tipo"].ToString() == "N")
                    {
                        _employee.empl_fechacreacion = DateTime.Now;
                        _employee.empl_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                        _employee.empl_terminalcreacion = Session["MachineName"].ToString();
                    }
                    else
                    {
                        _employee.empl_fechacreacion = DateTime.Parse(ViewState["fechacreacion"].ToString());
                        _employee.empl_usuariocreacion = int.Parse(ViewState["usucreacion"].ToString());
                        _employee.empl_terminalcreacion = ViewState["terminalcreacion"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(TxtPrimaria.Text))
                {
                    if (DdlFecFinPrimaria.SelectedItem.ToString() == "--Seleccione--")
                    {
                        new FuncionesBAS().FunShowJSMessage("Ingrese Fecha Inicio..!", this);
                        return;
                    }

                    if (DdlFecIniPrimaria.SelectedItem.ToString() == "--Seleccione--")
                    {
                        new FuncionesBAS().FunShowJSMessage("Ingresa Fecha Fin..!", this);
                        return;
                    }

                    if (int.Parse(DdlFecIniPrimaria.SelectedValue) > int.Parse(DdlFecFinPrimaria.SelectedValue))
                    {
                        new FuncionesBAS().FunShowJSMessage("La Fecha Incio no puede ser menor a la Fecha Fin..!", this);
                        return;
                    }

                    _grabestudio = true;
                }

                if (!string.IsNullOrEmpty(TxtSecundaria.Text))
                {
                    if (DdlFecIniSecundaria.SelectedItem.ToString() == "--Seleccione--")
                    {
                        new FuncionesBAS().FunShowJSMessage("Ingrese Fecha Inicio..!", this);
                        return;
                    }

                    if (DdlFecFinSecundaria.SelectedItem.ToString() == "--Seleccione--")
                    {
                        new FuncionesBAS().FunShowJSMessage("Ingresa Fecha Fin..!", this);
                        return;
                    }

                    if (int.Parse(DdlFecIniSecundaria.SelectedValue) > int.Parse(DdlFecFinSecundaria.SelectedValue))
                    {
                        new FuncionesBAS().FunShowJSMessage("La Fecha Incio no puede ser menor a la Fecha Fin..!", this);
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtTituloS.Text))
                    {
                        new FuncionesBAS().FunShowJSMessage("Ingrese Título Obtenido..!", this);
                        return;
                    }

                    _grabestudio = true;
                }

                if (!string.IsNullOrEmpty(TxtSuperior.Text))
                {
                    if (DdlFecIniSuperior.SelectedItem.ToString() == "--Seleccione--")
                    {
                        new FuncionesBAS().FunShowJSMessage("Ingrese Fecha Inicio..!", this);
                        return;
                    }

                    if (DdlFecFinSuperior.SelectedItem.ToString() == "--Seleccione--")
                    {
                        new FuncionesBAS().FunShowJSMessage("Ingresa Fecha Fin..!", this);
                        return;
                    }

                    if (int.Parse(DdlFecIniSuperior.SelectedValue) > int.Parse(DdlFecFinSuperior.SelectedValue))
                    {
                        new FuncionesBAS().FunShowJSMessage("La Fecha Incio no puede ser menor a la Fecha Fin..!", this);
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtTituloR.Text))
                    {
                        new FuncionesBAS().FunShowJSMessage("Ingrese Título Obtenido..!", this);
                        return;
                    }

                    _grabestudio = true;

                }

                if (_grabestudio)
                {
                    GSBPO_ESTUDIOS _estudio = new GSBPO_ESTUDIOS();
                    {
                        _estudio.EMPL_CODIGO = int.Parse(ViewState["Codigo"].ToString());
                        _estudio.ESTU_CODIGO = int.Parse(ViewState["CodigoEstu"].ToString());
                        _estudio.estu_primaria = TxtPrimaria.Text.Trim().ToUpper();
                        _estudio.estu_apdesde = DdlFecIniPrimaria.SelectedValue == "--Seleccione--" ? "" : DdlFecIniPrimaria.SelectedValue;
                        _estudio.estu_aphasta = DdlFecFinPrimaria.SelectedValue == "--Seleccione--" ? "" : DdlFecFinPrimaria.SelectedValue;
                        _estudio.estu_secundaria = TxtSecundaria.Text.Trim().ToUpper();
                        _estudio.estu_asdesde = DdlFecIniSecundaria.SelectedValue == "--Seleccione--" ? "" : DdlFecIniSecundaria.SelectedValue;
                        _estudio.estu_ashasta = DdlFecFinSecundaria.SelectedValue == "--Seleccione--" ? "" : DdlFecFinSecundaria.SelectedValue;
                        _estudio.estu_titulosecundaria = TxtTituloS.Text.Trim().ToUpper();
                        _estudio.estu_superior = TxtSuperior.Text.Trim().ToUpper();
                        _estudio.estu_audesde = DdlFecIniSuperior.SelectedValue == "--Seleccione--" ? "" : DdlFecIniSuperior.SelectedValue;
                        _estudio.estu_auhasta = DdlFecFinSuperior.SelectedValue == "--Seleccione--" ? "" : DdlFecFinSuperior.SelectedValue;
                        _estudio.estu_titulosuperior = TxtTituloR.Text.Trim().ToUpper();
                    }

                    _employee.GSBPO_ESTUDIOS.Add(_estudio);
                }

                _employee.GSBPO_OTROSESTUDIOS = new List<GSBPO_OTROSESTUDIOS>();
                _dtb = (DataTable)ViewState["OtrosEstudios"];

                if (_dtb.Rows.Count > 0)
                {
                    List<GSBPO_OTROSESTUDIOS> _otrosestudios = new List<GSBPO_OTROSESTUDIOS>();

                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        if (!new EmployeeDTO().FunConsultarOtrosEstudios(_employee.EMPL_CODIGO, int.Parse(_dr[0].ToString())))
                        {
                            _otrosestudios.Add(new GSBPO_OTROSESTUDIOS()
                            {
                                EMPL_CODIGO = int.Parse(ViewState["Codigo"].ToString()),
                                otes_institucion = _dr[1].ToString(),
                                otes_oidesde = DateTime.ParseExact(_dr[2].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                otes_oihasta = DateTime.ParseExact(_dr[3].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                otes_tituloinstituto = _dr[4].ToString(),
                            });
                        }
                    }

                    _employee.GSBPO_OTROSESTUDIOS = new List<GSBPO_OTROSESTUDIOS>();

                    foreach (GSBPO_OTROSESTUDIOS _otros in _otrosestudios)
                    {
                        _employee.GSBPO_OTROSESTUDIOS.Add(_otros);
                    }
                }

                _dtb = (DataTable)ViewState["Idiomas"];
                _employee.GSBPO_IDIOMAS = new List<GSBPO_IDIOMAS>();

                if (_dtb.Rows.Count > 0)
                {
                    List<GSBPO_IDIOMAS> _idiomas = new List<GSBPO_IDIOMAS>();

                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        if (!new EmployeeDTO().FunConsultarIdiomas(_employee.EMPL_CODIGO, int.Parse(_dr[0].ToString())))
                        {
                            _idiomas.Add(new GSBPO_IDIOMAS()
                            {
                                EMPL_CODIGO = int.Parse(ViewState["Codigo"].ToString()),
                                idio_idioma = _dr[4].ToString(),
                                idio_nivelH = _dr[2].ToString(),
                                idio_nivelE = _dr[3].ToString()
                            });
                        }
                    }

                    _employee.GSBPO_IDIOMAS = new List<GSBPO_IDIOMAS>();

                    foreach (GSBPO_IDIOMAS _idio in _idiomas)
                    {
                        _employee.GSBPO_IDIOMAS.Add(_idio);
                    }
                }

                _dtb = (DataTable)ViewState["Experiencia"];
                _employee.GSBPO_EXPERIENCIA = new List<GSBPO_EXPERIENCIA>();

                if (_dtb.Rows.Count > 0)
                {
                    List<GSBPO_EXPERIENCIA> _experiencia = new List<GSBPO_EXPERIENCIA>();

                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        if (!new EmployeeDTO().FunConsultarExperiencia(_employee.EMPL_CODIGO, int.Parse(_dr[0].ToString())))
                        {
                            _experiencia.Add(new GSBPO_EXPERIENCIA()
                            {
                                EMPL_CODIGO = int.Parse(ViewState["Codigo"].ToString()),
                                expe_empresa = _dr[1].ToString(),
                                expe_desde = DateTime.ParseExact(_dr[2].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                expe_hasta = DateTime.ParseExact(_dr[3].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                expe_cargo = _dr[4].ToString(),
                                expe_descripcion = _dr[5].ToString(),
                                expe_motivo = _dr[7].ToString()
                            });
                        }
                    }

                    _employee.GSBPO_EXPERIENCIA = new List<GSBPO_EXPERIENCIA>();

                    foreach (GSBPO_EXPERIENCIA _expe in _experiencia)
                    {
                        _employee.GSBPO_EXPERIENCIA.Add(_expe);
                    }
                }

                _dtb = (DataTable)ViewState["RefLaboral"];
                _employee.GSBPO_REFERENCIASLABORALES = new List<GSBPO_REFERENCIASLABORALES>();

                if (_dtb.Rows.Count > 0)
                {
                    List<GSBPO_REFERENCIASLABORALES> _reflaboral = new List<GSBPO_REFERENCIASLABORALES>();

                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        if (!new EmployeeDTO().FunConsultarRefLaboral(_employee.EMPL_CODIGO, int.Parse(_dr[0].ToString())))
                        {
                            _reflaboral.Add(new GSBPO_REFERENCIASLABORALES()
                            {
                                EMPL_CODIGO = int.Parse(ViewState["Codigo"].ToString()),
                                rela_empresa = _dr[1].ToString(),
                                rela_nombre = _dr[2].ToString(),
                                rela_cargo = _dr[3].ToString(),
                                rela_telefono = _dr[4].ToString(),
                                rela_celular = _dr[5].ToString(),
                                rela_email = _dr[6].ToString()
                            });
                        }
                    }

                    _employee.GSBPO_REFERENCIASLABORALES = new List<GSBPO_REFERENCIASLABORALES>();

                    foreach (GSBPO_REFERENCIASLABORALES _refla in _reflaboral)
                    {
                        _employee.GSBPO_REFERENCIASLABORALES.Add(_refla);
                    }
                }

                _dtb = (DataTable)ViewState["RefPersonal"];

                _employee.GSBPO_REFERENCIASPERSONALES = new List<GSBPO_REFERENCIASPERSONALES>();

                if (_dtb.Rows.Count > 0)
                {
                    List<GSBPO_REFERENCIASPERSONALES> _refpersonal = new List<GSBPO_REFERENCIASPERSONALES>();

                    foreach (DataRow _dr in _dtb.Rows)
                    {
                        if (!new EmployeeDAO().FunConsultarRefPersonal(_employee.EMPL_CODIGO, int.Parse(_dr[0].ToString())))
                        {
                            _refpersonal.Add(new GSBPO_REFERENCIASPERSONALES()
                            {
                                EMPL_CODIGO = int.Parse(ViewState["Codigo"].ToString()),
                                repe_nomrefe = _dr[1].ToString(),
                                repe_parentesco = _dr[5].ToString(),
                                repe_telefono = _dr[3].ToString(),
                                repe_celular = _dr[4].ToString()
                            });
                        }
                    }

                    _employee.GSBPO_REFERENCIASPERSONALES = new List<GSBPO_REFERENCIASPERSONALES>();

                    foreach (GSBPO_REFERENCIASPERSONALES _refpe in _refpersonal)
                    {
                        _employee.GSBPO_REFERENCIASPERSONALES.Add(_refpe);
                    }
                }

                if (_employee.EMPL_CODIGO == 0) _mensaje = new EmployeeDAO().FunCrearEmployee(_employee);
                else _mensaje = new EmployeeDAO().FunEditEmployee(_employee);

                if (_mensaje == "") Response.Redirect("WFrm_EmployeeAdmin.aspx?MensajeRetornado='Guardado con Éxito'");
                else Lblerror.Text = _mensaje;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelOtrosEstudios_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                _codigo = int.Parse(GrdvOtrosEstudios.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                if (new EmployeeDAO().FunDelOtrosEstudios(_codigo) == "")
                {
                    _dtbotrosestudios = (DataTable)ViewState["OtrosEstudios"];
                    _resultado = _dtbotrosestudios.Select("Codigo=" + _codigo).FirstOrDefault();
                    _resultado.Delete();
                    _dtbotrosestudios.AcceptChanges();
                    ViewState["OtrosEstudios"] = _dtbotrosestudios;
                    GrdvOtrosEstudios.DataSource = _dtbotrosestudios;
                    GrdvOtrosEstudios.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAgregarIdiomas_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlIdiomas.SelectedValue == "")
                {
                    Lblerror.Text = "Seleccione Idioma..!";
                    return;
                }

                if (ViewState["Idiomas"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["Idiomas"];

                    if (_dtbbuscar.Rows.Count > 0)
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbbuscar.Select("Idioma='" + DdlIdiomas.SelectedItem.ToString() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesBAS().FunShowJSMessage("Ya existe Idioma ingresado..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["Idiomas"];

                _filagre = _dtbagregar.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Idioma"] = DdlIdiomas.SelectedItem.ToString();
                _filagre["NivelH"] = DdlNivelH.SelectedItem.ToString();
                _filagre["NivelE"] = DdlNivelE.SelectedItem.ToString();
                _filagre["CodigoIdioma"] = DdlIdiomas.SelectedValue;
                _dtbagregar.Rows.Add(_filagre);
                ViewState["Idiomas"] = _dtbagregar;
                GrdvIdiomas.DataSource = _dtbagregar;
                GrdvIdiomas.DataBind();
                DdlIdiomas.SelectedIndex = 0;
                DdlNivelH.SelectedIndex = 0;
                DdlNivelE.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelIdiomas_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = int.Parse(GrdvIdiomas.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                if (new EmployeeDAO().FunDelIdiomas(_codigo) == "")
                {
                    _dtbidiomas = (DataTable)ViewState["Idiomas"];
                    _resultado = _dtbidiomas.Select("Codigo=" + _codigo).FirstOrDefault();
                    _resultado.Delete();
                    ViewState["Idiomas"] = _dtbidiomas;
                    GrdvIdiomas.DataSource = _dtbidiomas;
                    GrdvIdiomas.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAgregarOtrosEstudios_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtInstitucion.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Institución Estudios..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtFecIniOtrosE.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Fecha de Inicio..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtFecFinOtrosE.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Fecha Fin..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtTituloOtrosEstudios.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Título Obtenido..!", this);
                    return;
                }

                if (!new FuncionesDTO().IsDate(TxtFecIniOtrosE.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (!new FuncionesDTO().IsDate(TxtFecFinOtrosE.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (DateTime.ParseExact(TxtFecIniOtrosE.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFecFinOtrosE.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new FuncionesBAS().FunShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha Fin..!", this);
                    return;
                }

                if (ViewState["OtrosEstudios"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["OtrosEstudios"];

                    if (_dtbbuscar.Rows.Count > 0)
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbbuscar.Select("Institucion='" + TxtInstitucion.Text.Trim().ToUpper() + "' and FechaDesde='"
                        + TxtFecIniOtrosE.Text + "' and FechaHasta='" + TxtFecFinOtrosE.Text + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesBAS().FunShowJSMessage("Ya existe estudio ingresado..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["OtrosEstudios"];
                _filagre = _dtbagregar.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Institucion"] = TxtInstitucion.Text.Trim().ToUpper();
                _filagre["FechaDesde"] = TxtFecIniOtrosE.Text;
                _filagre["FechaHasta"] = TxtFecFinOtrosE.Text;
                _filagre["Titulo"] = TxtTituloOtrosEstudios.Text.Trim().ToUpper();
                _dtbagregar.Rows.Add(_filagre);
                ViewState["OtrosEstudios"] = _dtbagregar;
                GrdvOtrosEstudios.DataSource = _dtbagregar;
                GrdvOtrosEstudios.DataBind();
                TxtInstitucion.Text = "";
                TxtFecIniOtrosE.Text = "";
                TxtFecFinOtrosE.Text = "";
                TxtTituloOtrosEstudios.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAgregarExperiencia_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtEmpresa.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Nombre de la Empresa..!", this);
                    return;
                }

                if (!new FuncionesDTO().IsDate(TxtFecIniEmpre.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (!new FuncionesDTO().IsDate(TxtFecFinEmpre.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (DateTime.ParseExact(TxtFecIniEmpre.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFecFinEmpre.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new FuncionesBAS().FunShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha Fin..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCargo.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Cargo que ocupó..!", this);
                    return;
                }

                if (DdlMotivo.SelectedValue == "")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Motivo de Salina..!", this);
                    return;
                }

                if (ViewState["Experiencia"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["Experiencia"];

                    if (_dtbbuscar.Rows.Count > 0)
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbbuscar.Select("Empresa='" + TxtEmpresa.Text.Trim().ToUpper() + "' and FecInicio='" +
                        TxtFecIniEmpre.Text + "' and FecFin='" + TxtFecFinEmpre.Text + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesBAS().FunShowJSMessage("Ya existe Empresa ingresada en el mismo rango de fecha..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["Experiencia"];
                _filagre = _dtbagregar.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Empresa"] = TxtEmpresa.Text.Trim().ToUpper();
                _filagre["FecInicio"] = TxtFecIniEmpre.Text;
                _filagre["FecFin"] = TxtFecFinEmpre.Text;
                _filagre["Cargo"] = TxtCargo.Text.Trim().ToUpper();
                _filagre["Descripcion"] = TxtDescripcion.Text.Trim().ToUpper();
                _filagre["Motivo"] = DdlMotivo.SelectedItem.ToString();
                _filagre["CodigoMotivo"] = DdlMotivo.SelectedValue;
                _dtbagregar.Rows.Add(_filagre);
                ViewState["Experiencia"] = _dtbagregar;
                GrdvExperiencia.DataSource = _dtbagregar;
                GrdvExperiencia.DataBind();
                TxtEmpresa.Text = "";
                TxtFecIniEmpre.Text = "";
                TxtFecFinEmpre.Text = "";
                TxtCargo.Text = "";
                TxtDescripcion.Text = "";
                DdlMotivo.SelectedIndex = 0;
                FunCargarComboEmpresa();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelExpeLaboral_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                _codigo = int.Parse(GrdvExperiencia.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                if (new EmployeeDAO().FunDelExperiencia(_codigo) == "")
                {
                    _dtbexperiencia = (DataTable)ViewState["Experiencia"];
                    _resultado = _dtbexperiencia.Select("Codigo=" + _codigo).FirstOrDefault();
                    _resultado.Delete();
                    _dtbexperiencia.AcceptChanges();
                    ViewState["Experiencia"] = _dtbexperiencia;
                    GrdvExperiencia.DataSource = _dtbexperiencia;
                    GrdvExperiencia.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAgregarRefLaboral_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlEmpresa.SelectedValue == "0")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Empresa..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtNombreRefe.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Nombre de Referencia..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtCargoRefe.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Cargo que ocupa..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtTelefonoRefe.Text.Trim() + TxtCelularRefe.Text.Trim()))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese al menos un número de teléfono..!", this);
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(TxtCelularRefe.Text.Trim()))
                    {
                        if (TxtCelularRefe.Text.Trim().Length < 10 || TxtCelularRefe.Text.Trim().Substring(0, 2) != "09")
                        {
                            new FuncionesBAS().FunShowJSMessage("Número de celular incorrecto..!", this);
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(TxtTelefonoRefe.Text.Trim()))
                    {
                        if (TxtTelefonoRefe.Text.Trim().Length < 7 || TxtTelefonoRefe.Text.Trim().Substring(0, 2) == "09")
                        {
                            new FuncionesBAS().FunShowJSMessage("Número de teléfono incorrecto..!", this);
                            return;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(TxtEmailRefe.Text.Trim()))
                {
                    if (!new FuncionesDTO().Email_bien_escrito(TxtEmailRefe.Text.Trim()))
                    {
                        new FuncionesBAS().FunShowJSMessage("Dirección email incorrecto..!", this);
                        return;
                    }
                }

                if (ViewState["RefLaboral"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["RefLaboral"];

                    if (_dtbbuscar.Rows.Count > 0)
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbbuscar.Select("Empresa='" + DdlEmpresa.SelectedItem.ToString() + "' and Nombre='" +
                        TxtNombreRefe.Text.Trim() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesBAS().FunShowJSMessage("Ya existe Referencia ingresada..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["RefLaboral"];
                _filagre = _dtbagregar.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Empresa"] = DdlEmpresa.SelectedItem.ToString();
                _filagre["Nombre"] = TxtNombreRefe.Text.Trim().ToUpper();
                _filagre["Cargo"] = TxtCargoRefe.Text.Trim().ToUpper();
                _filagre["Telefono"] = TxtTelefonoRefe.Text.Trim();
                _filagre["Celular"] = TxtCelularRefe.Text.Trim();
                _filagre["Email"] = TxtEmailRefe.Text.Trim();
                _dtbagregar.Rows.Add(_filagre);
                ViewState["RefLaboral"] = _dtbagregar;
                GrdvRefLaboral.DataSource = _dtbagregar;
                GrdvRefLaboral.DataBind();
                DdlEmpresa.SelectedIndex = 0;
                TxtNombreRefe.Text = "";
                TxtCargoRefe.Text = "";
                TxtTelefonoRefe.Text = "";
                TxtCelularRefe.Text = "";
                TxtEmailRefe.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelRefLaboral_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                _codigo = int.Parse(GrdvRefLaboral.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                if (new EmployeeDAO().FunDelRefLaboral(_codigo) == "")
                {
                    _dtbreflaborales = (DataTable)ViewState["RefLaboral"];
                    _resultado = _dtbreflaborales.Select("Codigo=" + _codigo).FirstOrDefault();
                    _resultado.Delete();
                    _dtbreflaborales.AcceptChanges();
                    ViewState["RefLaboral"] = _dtbreflaborales;
                    GrdvRefLaboral.DataSource = _dtbreflaborales;
                    GrdvRefLaboral.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAgregarRefPersonal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlParentesco.SelectedValue == "")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Parentesco..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtRefePersonal.Text.Trim()))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Nombre de Referencia..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtTelefonoRefPersonal.Text.Trim() + TxtCelularRefPersonal.Text.Trim()))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese al menos un número de teléfono..!", this);
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(TxtCelularRefPersonal.Text.Trim()))
                    {
                        if (TxtCelularRefPersonal.Text.Trim().Length < 10 || TxtCelularRefPersonal.Text.Trim().Substring(0, 2) != "09")
                        {
                            new FuncionesBAS().FunShowJSMessage("Número de celular incorrecto..!", this);
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(TxtTelefonoRefPersonal.Text.Trim()))
                    {
                        if (TxtTelefonoRefPersonal.Text.Trim().Length < 7 || TxtTelefonoRefPersonal.Text.Trim().Substring(0, 2) == "09")
                        {
                            new FuncionesBAS().FunShowJSMessage("Número de teléfono incorrecto..!", this);
                            return;
                        }
                    }
                }

                if (ViewState["RefPersonal"] != null)
                {
                    _dtbbuscar = (DataTable)ViewState["RefPersonal"];

                    if (_dtbbuscar.Rows.Count > 0)
                        _maxcodigo = _dtbbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                    else _maxcodigo = 0;

                    _resultado = _dtbbuscar.Select("Nombre='" + TxtRefePersonal.Text.Trim() + "' and Parentesco='" +
                        DdlParentesco.SelectedItem.ToString() + "'").FirstOrDefault();

                    if (_resultado != null) _existe = true;
                }

                if (_existe)
                {
                    new FuncionesBAS().FunShowJSMessage("Ya existe Referencia ingresada..!", this);
                    return;
                }

                _dtbagregar = (DataTable)ViewState["RefPersonal"];
                _filagre = _dtbagregar.NewRow();
                _filagre["Codigo"] = _maxcodigo + 1;
                _filagre["Nombre"] = TxtRefePersonal.Text.Trim().ToUpper();
                _filagre["Parentesco"] = DdlParentesco.SelectedItem.ToString();
                _filagre["Telefono"] = TxtTelefonoRefPersonal.Text.Trim();
                _filagre["Celular"] = TxtCelularRefPersonal.Text.Trim();
                _filagre["CodigoParen"] = DdlParentesco.SelectedValue;
                _dtbagregar.Rows.Add(_filagre);
                ViewState["RefPersonal"] = _dtbagregar;
                GrdvRefPersonal.DataSource = _dtbagregar;
                GrdvRefPersonal.DataBind();
                DdlParentesco.SelectedIndex = 0;
                TxtRefePersonal.Text = "";
                TxtTelefonoRefPersonal.Text = "";
                TxtCelularRefPersonal.Text = "";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelRefPersonal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                _codigo = int.Parse(GrdvRefPersonal.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                if (new EmployeeDAO().FunDelRefPersonal(_codigo) == "")
                {
                    _dtbrefpersonales = (DataTable)ViewState["RefPersonal"];
                    _resultado = _dtbrefpersonales.Select("Codigo=" + _codigo).FirstOrDefault();
                    _resultado.Delete();
                    _dtbrefpersonales.AcceptChanges();
                    ViewState["RefPersonal"] = _dtbrefpersonales;
                    GrdvRefPersonal.DataSource = _dtbrefpersonales;
                    GrdvRefPersonal.DataBind();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_EmployeeAdmin.aspx", true);
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        #endregion
    }
}