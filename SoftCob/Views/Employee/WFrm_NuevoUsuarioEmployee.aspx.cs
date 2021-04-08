namespace SoftCob.Views.Employee
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Web.UI;
    public partial class WFrm_NuevoUsuarioEmployee : Page
    {
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                if (Session["IN-CALL"].ToString() == "SI")
                {
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                ViewState["CodigoEmployee"] = Request["CodigoEmployee"];
                ViewState["CodigoUsuario"] = Request["CodigoUsuario"];
                FunCargarCombos();
                FunCargarMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos()
        {

            DdlAsignarUsuario.DataSource = new ControllerDAO().FunGetUsuarioSinAsignar();
            DdlAsignarUsuario.DataTextField = "Descripcion";
            DdlAsignarUsuario.DataValueField = "Codigo";
            DdlAsignarUsuario.DataBind();

            DdlDepartamento.DataSource = new ControllerDAO().FunGetDepartamento();
            DdlDepartamento.DataTextField = "Descripcion";
            DdlDepartamento.DataValueField = "Codigo";
            DdlDepartamento.DataBind();

            DdlPerfil.DataSource = new ControllerDAO().FunGetPerfil();
            DdlPerfil.DataTextField = "Descripcion";
            DdlPerfil.DataValueField = "Codigo";
            DdlPerfil.DataBind();

            DdlTipoUsuario.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO USUARIOS", "--Seleccione Tipo--", "S");
            DdlTipoUsuario.DataTextField = "Descripcion";
            DdlTipoUsuario.DataValueField = "Codigo";
            DdlTipoUsuario.DataBind();
        }

        private void FunCargarMantenimiento()
        {
            try
            {
                SoftCob_EMPLOYEE _employee = new SoftCob_EMPLOYEE();
                _employee = new EmployeeDAO().FunGetEmployeePorCodigo(int.Parse(ViewState["CodigoEmployee"].ToString()));
                Lbltitulo.Text = "Asignar Usuario a: " + _employee.empl_nombres + " " + _employee.empl_apellidos;
                TxtLogin.Text = _employee.empl_identificacion;
                ViewState["Identificacion"] = _employee.empl_identificacion;
                ViewState["Nombre"] = _employee.empl_nombres;
                ViewState["Apellido"] = _employee.empl_apellidos;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_EmployeeAdmin.aspx", true);
        }
        protected void DdlAsignarUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lblerror.Text = "";
            TxtLogin.Text = "";
            lbllogin.Visible = false;
            lblpassword.Visible = false;
            TxtLogin.Visible = false;
            TxtPassword.Visible = false;
            lbldepartamento.Visible = false;
            lblperfil.Visible = false;
            lbltipousuario.Visible = false;
            DdlDepartamento.Visible = false;
            DdlPerfil.Visible = false;
            DdlTipoUsuario.Visible = false;
            switch (DdlAsignarUsuario.SelectedValue)
            {
                case "0":
                    TxtLogin.Text = ViewState["Identificacion"].ToString();
                    lbllogin.Visible = true;
                    lblpassword.Visible = true;
                    TxtLogin.Visible = true;
                    TxtPassword.Visible = true;
                    lbldepartamento.Visible = true;
                    lblperfil.Visible = true;
                    lbltipousuario.Visible = true;
                    DdlDepartamento.Visible = true;
                    DdlPerfil.Visible = true;
                    DdlTipoUsuario.Visible = true;
                    break;
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (DdlAsignarUsuario.SelectedValue == "-1")
            {
                new FuncionesDAO().FunShowJSMessage("Seleccione Usuario o Cree uno nuevo..!", this);
                return;
            }

            if (DdlAsignarUsuario.SelectedValue == "0")
            {
                if (string.IsNullOrEmpty(TxtLogin.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Login para el Usuario..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtPassword.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Password..!", this);
                    return;
                }

                if (DdlDepartamento.SelectedValue == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Departamento..!", this);
                    return;
                }

                if (DdlPerfil.SelectedValue == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Perfil..!", this);
                    return;
                }

                if (DdlTipoUsuario.SelectedValue == "")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo Usuario..!", this);
                    return;
                }

                if (new ControllerDAO().FunConsultaLogin(TxtLogin.Text.Trim()) != "")
                {
                    new FuncionesDAO().FunShowJSMessage("Login ya existe..!", this);
                    return;
                }
            }

            SoftCob_USUARIO _user = new SoftCob_USUARIO();
            {
                _user.USUA_CODIGO = int.Parse(DdlAsignarUsuario.SelectedValue);
                _user.empl_codigo = int.Parse(ViewState["CodigoEmployee"].ToString());

                if (_user.USUA_CODIGO == 0)
                {
                    _user.PERF_CODIGO = int.Parse(DdlPerfil.SelectedValue);
                    _user.usua_nombres = ViewState["Nombre"].ToString();
                    _user.usua_apellidos = ViewState["Apellido"].ToString();
                    _user.DEPA_CODIGO = int.Parse(DdlDepartamento.SelectedValue);
                    _user.usua_tipousuario = DdlTipoUsuario.SelectedValue;
                    _user.usua_cedula = ViewState["Identificacion"].ToString();
                    _user.usua_login = TxtLogin.Text.Trim();
                    _user.usua_password = TxtPassword.Text.Trim();
                    _user.usua_estado = true;
                    _user.usua_contador = 0;
                    _user.usua_caducapass = false;
                    _user.usua_fechacaduca = DateTime.Now;
                    _user.usua_cambiarpass = false;
                    _user.usua_statuslogin = false;
                    _user.usua_permisosespeciales = false;
                    _user.usua_terminallogin = "";
                    _user.usua_fechacreacion = DateTime.Now;
                    _user.usua_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _user.usua_terminalcreacion = Session["MachineName"].ToString();
                    _user.usua_fum = DateTime.Now;
                    _user.usua_uum = int.Parse(Session["usuCodigo"].ToString());
                    _user.usua_tum = Session["MachineName"].ToString();
                    new ControllerDAO().FunCrearUsuario(_user);
                }
                else new ControllerDAO().FunEditarUsuarioEmployee(_user);
            }

            Response.Redirect("WFrm_EmployeeAdmin.aspx?MensajeRetornado=Guardado con Éxito");
        }
        #endregion
    }
}