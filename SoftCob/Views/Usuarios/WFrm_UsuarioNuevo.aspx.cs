namespace SoftCob.Views.Usuarios
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    public partial class WFrm_UsuarioNuevo : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");
                if (!IsPostBack)
                {
                    FunCargarCombos();
                    TxtFechaCaduca.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    ViewState["Login"] = "";
                    ViewState["CodigoUsuario"] = Request["Codigo"];
                    if (ViewState["CodigoUsuario"].ToString() == "0")
                    {
                        Lbltitulo.Text = "Agregar Nuevo Usuario";
                    }
                    else
                    {
                        Label7.Visible = true;
                        ChkEstado.Visible = true;
                        Lbltitulo.Text = "Editar Usuarios";
                        FunCargaMantenimiento(int.Parse(ViewState["CodigoUsuario"].ToString()));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        } 
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos()
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatosNew(10, int.Parse(Session["CodigoEMPR"].ToString()),
                "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, Session["Conectar"].ToString());
            DdlDepartamento.DataSource = _dts;
            DdlDepartamento.DataTextField = "Descripcion";
            DdlDepartamento.DataValueField = "Codigo";
            DdlDepartamento.DataBind();

            _dts = new ConsultaDatosDAO().FunConsultaDatosNew(5, int.Parse(Session["CodigoEMPR"].ToString()),
                "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, Session["Conectar"].ToString());
            DdlPerfil.DataSource = _dts;
            DdlPerfil.DataTextField = "Descripcion";
            DdlPerfil.DataValueField = "Codigo";
            DdlPerfil.DataBind();

            DdlTipoUsuario.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO USUARIOS", "--Seleccione Tipo--", "S");
            DdlTipoUsuario.DataTextField = "Descripcion";
            DdlTipoUsuario.DataValueField = "Codigo";
            DdlTipoUsuario.DataBind();
        }

        private void FunCargaMantenimiento(int _codigo)
        {
            SoftCob_USUARIO _user = new ControllerDAO().FunConsultarUsuarioPorCodigo(_codigo);
            TxtNombres.Text = _user.usua_nombres;
            TxtApellidos.Text = _user.usua_apellidos;
            TxtUser.Text = _user.usua_login;
            TxtPassword.Attributes.Add("Value", new FuncionesDAO().FunDesencripta(_user.usua_password));
            DdlDepartamento.SelectedValue = _user.DEPA_CODIGO.ToString();
            DdlPerfil.SelectedValue = _user.PERF_CODIGO.ToString();
            DdlTipoUsuario.SelectedValue = _user.usua_tipousuario.ToString();
            ChkCaduca.Checked = _user.usua_caducapass;
            ChkCaduca.Text = _user.usua_caducapass ? "Si" : "No";
            TxtFechaCaduca.Enabled = ChkCaduca.Checked;
            TxtFechaCaduca.Text = _user.usua_fechacaduca.ToString("MM/dd/yyyy");
            ChkCambiar.Checked = _user.usua_cambiarpass;
            ChkCambiar.Text = _user.usua_cambiarpass ? "Si" : "No";
            ChkPermisos.Checked = _user.usua_permisosespeciales;
            ChkPermisos.Text = _user.usua_permisosespeciales ? "Si" : "No";
            ChkEstado.Checked = _user.usua_estado;
            ChkEstado.Text = _user.usua_estado == true ? "Activo" : "Inactivo";
            ViewState["Login"] = TxtUser.Text;
        }
        #endregion

        #region Botones y Eventos
        protected void ChkCaduca_CheckedChanged(object sender, EventArgs e)
        {
            TxtFechaCaduca.Text = DateTime.Now.ToString("MM/dd/yyyy");
            ChkCaduca.Text = ChkCaduca.Checked == true ? "Si" : "No";
            TxtFechaCaduca.Enabled = ChkCaduca.Checked;
        }

        protected void ChkCambiar_CheckedChanged(object sender, EventArgs e)
        {
            ChkCambiar.Text = ChkCambiar.Checked == true ? "Si" : "No";
        }

        protected void ChkPermisos_CheckedChanged(object sender, EventArgs e)
        {
            ChkPermisos.Text = ChkPermisos.Checked == true ? "Si" : "No";
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked == true ? "Activo" : "Inactivo";
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtNombres.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Nombre del Usuario..!", this, "W","C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtApellidos.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Apellido del Usuario..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtUser.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Login del Usuario..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtPassword.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Pasword..!", this, "W", "C");
                    return;
                }

                if (DdlDepartamento.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Departamento..!", this, "W", "C");
                    return;
                }

                if (DdlPerfil.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Perfil..!", this, "W", "C");
                    return;
                }

                if (DdlTipoUsuario.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Usuario..!", this, "W", "C");
                    return;
                }

                if (ChkCaduca.Checked)
                {
                    if (!new FuncionesDAO().IsDate(TxtFechaCaduca.Text))
                    {
                        new FuncionesDAO().FunShowJSMessage("Fecha no válida..!", this, "E", "C");
                        return;
                    }
                }

                if (ViewState["Login"].ToString() != TxtUser.Text)
                {
                    if (!string.IsNullOrEmpty(new ControllerDAO().FunConsultaLogin(TxtUser.Text, int.Parse(Session["CodigoEMPR"].ToString()))))
                    {
                        new FuncionesDAO().FunShowJSMessage("Login ya existe..!", this, "E", "C");
                        return;
                    }
                }

                SoftCob_USUARIO _user = new SoftCob_USUARIO();
                {
                    _user.USUA_CODIGO = int.Parse(ViewState["CodigoUsuario"].ToString());
                    _user.PERF_CODIGO = int.Parse(DdlPerfil.SelectedValue);
                    _user.DEPA_CODIGO = int.Parse(DdlDepartamento.SelectedValue);
                    _user.empr_codigo = int.Parse(Session["CodigoEMPR"].ToString());
                    _user.empl_codigo = 0;
                    _user.usua_tipousuario = DdlTipoUsuario.SelectedValue;
                    _user.usua_nombres = TxtNombres.Text.ToUpper().Trim();
                    _user.usua_apellidos = TxtApellidos.Text.ToUpper().Trim();                                       
                    _user.usua_cedula = "";
                    _user.usua_login = TxtUser.Text;
                    _user.usua_password = new FuncionesDAO().FunEncripta(TxtPassword.Text);
                    _user.usua_estado = ChkEstado.Checked;
                    _user.usua_contador = 0;
                    _user.usua_caducapass = ChkCaduca.Checked;
                    _user.usua_fechacaduca = DateTime.ParseExact(TxtFechaCaduca.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    _user.usua_cambiarpass = ChkCambiar.Checked;
                    _user.usua_statuslogin = false;
                    _user.usua_permisosespeciales = ChkPermisos.Checked;
                    _user.usua_terminallogin = "";
                    _user.usua_logobienvenida = "";
                    _user.usua_logomenu = "";
                    _user.usua_mail = "";
                    _user.usua_celular = "";
                    _user.usua_foto = new byte[0];
                    _user.usua_imagepath = "";
                    _user.usua_imagefullpath = "";
                    _user.usua_auxv1 = "";
                    _user.usua_auxv2 = "";
                    _user.usua_auxv3 = "";
                    _user.usua_auxv4 = "";
                    _user.usua_auxv5 = "";
                    _user.usua_auxi1 = 0;
                    _user.usua_auxi2 = 0;
                    _user.usua_auxi3 = 0;
                    _user.usua_auxi4 = 0;
                    _user.usua_auxi5 = 0;
                    _user.usua_fechacreacion = DateTime.Now;
                    _user.usua_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _user.usua_terminalcreacion = Session["MachineName"].ToString();
                    _user.usua_fum = DateTime.Now;
                    _user.usua_uum = int.Parse(Session["usuCodigo"].ToString());
                    _user.usua_tum = Session["MachineName"].ToString();

                    if (_user.USUA_CODIGO == 0) new ControllerDAO().FunCrearUsuario(_user);
                    else
                    {
                        new ControllerDAO().FunEditarUsuario(_user);

                        _dts = new ConsultaDatosDAO().FunConsultaDatos(211, ChkEstado.Checked ? 1 : 0,
                            int.Parse(ViewState["CodigoUsuario"].ToString()), 0, "", "", "", Session["Conectar"].ToString());
                    }

                    Response.Redirect("WFrm_UsuarioAdmin.aspx?MensajeRetornado=Guardado con Éxito");
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_UsuarioAdmin.aspx", true);
        } 
        #endregion
    }
}