namespace SoftCob.Views.Mantenedor
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;

    public partial class WFrm_Menu : Page
    {
        #region Variables
        string _horalogueo = "", _fechalogueo = "";
        DataSet _dts = new DataSet();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!Page.IsPostBack)
            {
                try
                {
                    LblUsuario.Text = Session["usuNombres"].ToString();
                    _dts = new ControllerDAO().FunConsultarMenuPorUsuario(0, int.Parse(Session["usuCodigo"].ToString()),
                        int.Parse(Session["CodigoEMPR"].ToString()), "", "", 0, 0, Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0) new ControllerDAO().Crea_menuweb(_dts.Tables[0], ref Trvmenu);

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 0, 0, 0, "", "LOGOMN", "PATH LOGOS", Session["Conectar"].ToString());
                    if (_dts.Tables[0].Rows.Count > 0) ImgLogo.ImageUrl = _dts.Tables[0].Rows[0]["Valor"].ToString();
                    else ImgLogo.ImageUrl = "~/Images/LogoBBP.jpg";
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }
        #endregion

        #region Botones y Eventos
        protected void Trvmenu_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                if (Trvmenu.SelectedNode.NavigateUrl == "")
                {
                    Trvmenu.CollapseAll();
                    Trvmenu.SelectedNode.Expand();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void LnkCerrar_Click(object sender, EventArgs e)
        {
            if (Session["IN-CALL"].ToString() == "SI")
            {
                new FuncionesDAO().FunShowJSMessage("Se encuentra en Llamada, en cuanto termine la gestión podrá salir de la Lista de Trabajo..!", this, "E", "C");
                return;
            }

            SoftCob_USUARIO _usuario = new SoftCob_USUARIO();
            {
                _usuario.USUA_CODIGO = int.Parse(Session["usuCodigo"].ToString());
                _usuario.usua_statuslogin = false;
                _usuario.usua_terminallogin = "";
            }

            new ControllerDAO().FunUpdateLogueo(_usuario);
            SoftCob_LOGUEO_TIEMPOS _loguintime = new SoftCob_LOGUEO_TIEMPOS();
            _loguintime.USUA_CODIGO = int.Parse(Session["usuCodigo"].ToString());
            _loguintime.cpce_codigo = 0;
            _loguintime.ltca_codigo = 0;
            _loguintime.loti_tipologueo = "LF";
            _fechalogueo = DateTime.Now.ToString("MM/dd/yyyy");
            _loguintime.loti_fechalogueo = DateTime.ParseExact(_fechalogueo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _horalogueo = DateTime.Now.ToString("HH:mm:ss");
            _loguintime.loti_horalogueo = TimeSpan.Parse(_horalogueo);
            _loguintime.loti_fechacompleta = DateTime.Now;
            _loguintime.loti_auxv1 = "";
            _loguintime.loti_auxv2 = "";
            _loguintime.loti_auxv3 = "";
            _loguintime.loti_auxv4 = "";
            _loguintime.loti_auxv5 = "";
            _loguintime.loti_auxi1 = 0;
            _loguintime.loti_auxi2 = 0;
            _loguintime.loti_auxi3 = 0;
            _loguintime.loti_auxi4 = 0;
            _loguintime.loti_auxi5 = 0;
            _loguintime.loti_auxd1 = DateTime.Now;
            _loguintime.loti_auxd2 = DateTime.Now;
            _loguintime.loti_auxd3 = DateTime.Now;
            _loguintime.loti_auxd4 = DateTime.Now;
            _loguintime.loti_auxd5 = DateTime.Now;
            _loguintime.loti_fechacreacion = DateTime.Now;
            _loguintime.loti_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
            _loguintime.loti_terminalcreacion = Session["MachineName"].ToString();
            new ControllerDAO().FunCrearLogueoTiempos(_loguintime);
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "code", "salir();", true);
        }

        #endregion
    }
}