namespace SoftCob.Views.Menu
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Web.UI;
    public partial class WFrm_MenuNuevo : Page
    {
        #region Variables
        int _codigo = 0;
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
                    Lbltitulo.Text = "Agregar Nuevo Menú";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtNombreMenu.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese nombre del Menú..!", this, "W", "C");
                    return;
                }

                if (new ControllerDAO().FunConsultaMenu(TxtNombreMenu.Text.Trim(), int.Parse(Session["CodigoEMPR"].ToString())) > 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Nombre del Menú ya Existe..!", this, "E", "C");
                    return;
                }

                SoftCob_MENU _menu = new SoftCob_MENU();
                {
                    _menu.empr_codigo = int.Parse(Session["CodigoEMPR"].ToString());
                    _menu.menu_descripcion = TxtNombreMenu.Text.Trim();
                    _menu.menu_nivel = 0;
                    _menu.menu_estado = true;
                    _menu.menu_orden = new ControllerDAO().FunGetOrdenMenu(int.Parse(Session["CodigoEMPR"].ToString()));
                    _menu.menu_auxi1 = 0;
                    _menu.menu_auxi2 = 0;
                    _menu.menu_auxi3 = 0;
                    _menu.menu_auxv1 = "";
                    _menu.menu_auxv2 = "";
                    _menu.menu_auxv3 = "";
                    _menu.menu_fechacreacion = DateTime.Now;
                    _menu.menu_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    _menu.menu_terminalcreacion = Session["MachineName"].ToString();
                    _menu.menu_fum = DateTime.Now;
                    _menu.menu_tum = Session["MachineName"].ToString();
                    _menu.menu_uum = int.Parse(Session["usuCodigo"].ToString());
                }

                _codigo = new ControllerDAO().FunCrearMenu(_menu);

                if (_codigo > 0)
                {
                    Response.Redirect("WFrm_MenuEdit.aspx?Codigo=" + _codigo, true);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_MenuAdmin.aspx", true);
        }
        #endregion

    }
}