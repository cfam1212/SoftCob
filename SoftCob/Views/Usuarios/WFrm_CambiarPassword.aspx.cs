namespace SoftCob.Views.Usuarios
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Web.UI;
    public partial class WFrm_CambiarPassword : Page
    {
        #region Variables
        string _redirect = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Lbltitulo.Text = "Administrar Contraseñas";

                if (bool.Parse(Session["usuCambiarPass"].ToString()) == false)
                {
                    TxtPassAnterior.Enabled = false;
                    TxtNuevoPass.Enabled = false;
                    TxtConfirmarPass.Enabled = false;
                    BtnGrabar.Enabled = false;
                    Lblerror.Text = "Usuario no tiene permisos suficientes..!";
                }

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", 
                    Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtPassAnterior.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Contraseña Anterior..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtNuevoPass.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Contraseña Nueva..!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtConfirmarPass.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Confirmar Contraseña..!", this);
                    return;
                }

                SoftCob_USUARIO _user = new ControllerDAO().FunGetUsuarioPorID(int.Parse(Session["usuCodigo"].ToString()));

                if (new FuncionesDAO().FunDesencripta(_user.usua_password) != TxtPassAnterior.Text.Trim())
                {
                    new FuncionesDAO().FunShowJSMessage("Contraseña anterior incorrecta..!", this);
                    return;
                }

                if (TxtNuevoPass.Text.Trim() != TxtConfirmarPass.Text.Trim())
                {
                    new FuncionesDAO().FunShowJSMessage("Contraseñas no Coinciden..!", this);
                    return;
                }

                _user.usua_password = new FuncionesDAO().FunEncripta(TxtNuevoPass.Text.Trim());

                new ControllerDAO().FunChangePassword(_user);

                _redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Contraseña cambiada con Exito..!");
                Response.Redirect(_redirect, true);
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