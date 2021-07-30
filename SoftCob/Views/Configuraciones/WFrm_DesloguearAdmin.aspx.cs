namespace SoftCob.Views.Configuraciones
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_DesloguearAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        int _codigo = 0;
        string _redirect = "", _fechalogueo = "", _horalogueo = "", _mensaje = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {

                Lbltitulo.Text = "Administrar Deslogueo Usuario";
                FunCargarMantenimiento();

               
                if (Request["MensajeRetornado"] != null)
                {
                    _mensaje = Request["MensajeRetornado"];
                    new FuncionesDAO().FunShowJSMessage(_mensaje, this, "S", "R");
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(67, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "", "",
                    Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = _dts;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                ViewState["grdvDatos"] = GrdvDatos.DataSource;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = int.Parse(GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _dts = new ConsultaDatosDAO().FunConsultaDatos(68, _codigo, 0, 0, "", "", "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    SoftCob_LOGUEO_TIEMPOS _loguintime = new SoftCob_LOGUEO_TIEMPOS();
                    {
                        _loguintime.USUA_CODIGO = _codigo;
                        _loguintime.empr_codigo = int.Parse(Session["CodigoEMPR"].ToString());
                        _loguintime.cpce_codigo = 0;
                        _loguintime.ltca_codigo = 0;
                        _loguintime.loti_tipologueo = "DL";
                        _fechalogueo = DateTime.Now.ToString("MM/dd/yyyy");
                        _loguintime.loti_fechalogueo = DateTime.ParseExact(_fechalogueo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        _horalogueo = DateTime.Now.ToString("HH:mm:ss");
                        _loguintime.loti_horalogueo = TimeSpan.Parse(_horalogueo);
                        _loguintime.loti_fechacompleta = DateTime.Now;
                        _loguintime.loti_auxv1 = "";
                        _loguintime.loti_auxv2 = "";
                        _loguintime.loti_auxv3 = "";
                        _loguintime.loti_auxv4 = "";
                        _loguintime.loti_auxi1 = 0;
                        _loguintime.loti_auxi2 = 0;
                        _loguintime.loti_auxi3 = 0;
                        _loguintime.loti_auxi4 = 0;
                        _loguintime.loti_auxd1 = DateTime.Now;
                        _loguintime.loti_auxd2 = DateTime.Now;
                        _loguintime.loti_auxd3 = DateTime.Now;
                        _loguintime.loti_auxd4 = DateTime.Now;
                        _loguintime.loti_fechacreacion = DateTime.Now;
                        _loguintime.loti_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                        _loguintime.loti_terminalcreacion = Session["MachineName"].ToString();
                    }

                    new ControllerDAO().FunCrearLogueoTiempos(_loguintime);
                    _redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Deslogueado con Exito..!");
                    Response.Redirect(_redirect, true);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgReset_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = int.Parse(GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
            _dts = new ConsultaDatosDAO().FunConsultaDatos(101, _codigo, 0, 0, "", new FuncionesDAO().FunEncripta("1234"), "",
                Session["Conectar"].ToString());
            _redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Password Reseteado con Exito..!");
            Response.Redirect(_redirect, true);
        }
        #endregion
    }
}