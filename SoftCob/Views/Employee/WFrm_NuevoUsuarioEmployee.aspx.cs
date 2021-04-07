

namespace SoftCob.Views.Employee
{
    using ControllerSoftCob;
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

            DdlAsignarUsuario.DataSource = new CatalogosDTO().FunGetUsuarioSinAsignar();
            DdlAsignarUsuario.DataTextField = "Descripcion";
            DdlAsignarUsuario.DataValueField = "Codigo";
            DdlAsignarUsuario.DataBind();

            DdlDepartamento.DataSource = new CatalogosDTO().FunGetDepartamento();
            DdlDepartamento.DataTextField = "Descripcion";
            DdlDepartamento.DataValueField = "Codigo";
            DdlDepartamento.DataBind();

            DdlPerfil.DataSource = new CatalogosDTO().FunGetPerfil();
            DdlPerfil.DataTextField = "Descripcion";
            DdlPerfil.DataValueField = "Codigo";
            DdlPerfil.DataBind();

            DdlTipoUsuario.DataSource = new CatalogosDTO().FunGetParametroDetalle("TIPO USUARIOS", "--Seleccione Tipo--");
            DdlTipoUsuario.DataTextField = "Descripcion";
            DdlTipoUsuario.DataValueField = "Codigo";
            DdlTipoUsuario.DataBind();
        }

        private void FunCargarMantenimiento()
        {
            try
            {
                GSBPO_EMPLOYEE _employee = new GSBPO_EMPLOYEE();
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
                new FuncionesBAS().FunShowJSMessage("Seleccione Usuario o Cree uno nuevo..!", this);
                return;
            }

            if (DdlAsignarUsuario.SelectedValue == "0")
            {
                if (string.IsNullOrEmpty(TxtLogin.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Login para el Usuario..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtPassword.Text))
                {
                    new FuncionesBAS().FunShowJSMessage("Ingrese Password..!", this);
                    return;
                }

                if (DdlDepartamento.SelectedValue == "")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Departamento..!", this);
                    return;
                }

                if (DdlPerfil.SelectedValue == "")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Perfil..!", this);
                    return;
                }

                if (DdlTipoUsuario.SelectedValue == "")
                {
                    new FuncionesBAS().FunShowJSMessage("Seleccione Tipo Usuario..!", this);
                    return;
                }

                if (new UsuariosDTO().FunConsultaLogin(TxtLogin.Text.Trim()) != "")
                {
                    new FuncionesBAS().FunShowJSMessage("Login ya existe..!", this);
                    return;
                }
            }

            USUARIO _user = new USUARIO();
            _user.USU_CODIGO = int.Parse(DdlAsignarUsuario.SelectedValue);
            _user.empl_codigo = int.Parse(ViewState["CodigoEmployee"].ToString());

            if (_user.USU_CODIGO == 0)
            {
                _user.PER_CODIGO = int.Parse(DdlPerfil.SelectedValue);
                _user.usu_Nombres = ViewState["Nombre"].ToString();
                _user.usu_Apellidos = ViewState["Apellido"].ToString();
                _user.DEPA_CODIGO = int.Parse(DdlDepartamento.SelectedValue);
                _user.usu_TipoUsuario = DdlTipoUsuario.SelectedValue;
                _user.usu_Cedula = ViewState["Identificacion"].ToString();
                _user.usu_Login = TxtLogin.Text.Trim();
                _user.usu_Password = TxtPassword.Text.Trim();
                _user.usu_Estatus = true;
                _user.usu_Contador = 0;
                _user.usu_caducapass = false;
                _user.usu_fechacaduca = DateTime.Now;
                _user.usu_cambiarpass = false;
                _user.usu_statuslogin = false;
                _user.usu_permisosespeciales = false;
                _user.usu_terminallogin = "";
                _user.usu_FechaCreacion = DateTime.Now;
                _user.usu_UsuarioCreacion = int.Parse(Session["usuCodigo"].ToString());
                _user.usu_TerminalCreacion = Session["MachineName"].ToString();
                _user.usu_FUM = DateTime.Now;
                _user.usu_UUM = int.Parse(Session["usuCodigo"].ToString());
                _user.usu_TUM = Session["MachineName"].ToString();
                new UsuariosDTO().FunCrearUsuario(_user);
            }
            else new UsuariosDAO().FunEditarUsuarioEmployee(_user);

            Response.Redirect("WFrm_EmployeeAdmin.aspx?MensajeRetornado='Guardado con Éxito'");
        }
        #endregion
    }
}