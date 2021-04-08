namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Threading;
    using System.Web.UI;
    public partial class WFrm_SeleccionListaTrabajo : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        int _idlista = 0, _idgestor = 0;
        string _horalogueo = "", _fechalogueo = "", _servidor = "", _cliente = "", _strcti = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                ViewState["Automatico"] = ConfigurationManager.AppSettings["AutomatiCALL"].ToString();
                if (Session["IN-CALL"].ToString() == "SI")
                {
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                    Response.Redirect("wFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }
                Lbltitulo.Text = "Lista de Trabajos Activas";
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                FunCargarMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(44, 0, 0, 0, "", "", "", ViewState["Conectar"].ToString());
                _dts = new ConsultaDatosDAO().FunConsultaDatos(137, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "", "", ViewState["Conectar"].ToString());
                _dts = new ControllerDAO().FunGetConsultasCatalogo(28, "--Seleccione Lista--", int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "", "", ViewState["Conectar"].ToString());
                DdlListaTrabajo.DataSource = _dts;
                DdlListaTrabajo.DataTextField = "Descripcion";
                DdlListaTrabajo.DataValueField = "Codigo";
                DdlListaTrabajo.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void ElegirLista()
        {
            try
            {
                _idlista = int.Parse(this.DdlListaTrabajo.SelectedValue.ToString());
                ViewState["idListaActiva"] = _idlista;
                _idgestor = int.Parse(Session["usuCodigo"].ToString());
                _dts = new ConsultaDatosDAO().FunConsultaDatos(23, _idlista, 0, 0, "", "", "", ViewState["Conectar"].ToString());
                ViewState["CodigoCedente"] = _dts.Tables[0].Rows[0]["Codigocedente"].ToString();
                ViewState["CodigoCatalago"] = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                FunGrabarListaActiva(_idlista, _idgestor);
                FunStartPhone();
            }
            catch (Exception ex)
            {
                FunShowException(ex);
            }
        }

        private void FunStartPhone()
        {
            _servidor = "";
            _cliente = "ELASTIX";

            if (_cliente == "ELASTIX") _servidor = ConfigurationManager.AppSettings["PathCTIElastix"].ToString();

            _strcti = string.Format(@"SmartPhone({0},{1})", "'" + _cliente + "'", "'" + _servidor + "'");
            FunExecuteJsCode(_strcti);
        }

        private void FunExecuteJsCode(string texto)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "script1", texto, true);
        }

        private void FunShowException(Exception ex)
        {
            FunExecuteJsCode(" $(\"#divException\").attr(\"style\", \"visibility:visible\");");
            Lblerror.Text = ex.ToString();
            //this.litMensajeEx.Text = ex.ToString();
        }

        private void FunGrabarListaActiva(int idLista, int idGestor)
        {
            try
            {
                new ConsultaDatosDAO().FunConsultaDatos(27, idLista, idGestor, 0, "", "", "", ViewState["Conectar"].ToString());
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void WaitToPhoneClient()
        {
            string phoneCliente = "ELASTIX";

            while (true)
            {

                string estado = GetState(phoneCliente);
                if (estado == "READY")
                {
                    break;
                }

                Thread.Sleep(100);
            }
        }

        private string GetState(string cliente)
        {
            Thread.Sleep(500);
            if (cliente == "ELASTIX") return "READY";
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnRedirect_Click(object sender, EventArgs e)
        {
            Thread.Sleep(100);
            WaitToPhoneClient();
            Response.Redirect("wFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + ViewState["idListaActiva"].ToString(), true);
        }

        protected void DdlListaTrabajo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DdlListaTrabajo.SelectedValue != "0")
                {
                    _idlista = int.Parse(this.DdlListaTrabajo.SelectedValue.ToString());
                    ViewState["idListaActiva"] = _idlista;
                    _idgestor = int.Parse(Session["usuCodigo"].ToString());
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(23, _idlista, 0, 0, "", "", "", ViewState["Conectar"].ToString());
                    ViewState["CodigoCedente"] = _dts.Tables[0].Rows[0]["Codigocedente"].ToString();
                    ViewState["CodigoCatalago"] = _dts.Tables[0].Rows[0]["Codigocatalogo"].ToString();
                    FunGrabarListaActiva(_idlista, _idgestor);

                    SoftCob_LOGUEO_TIEMPOS loguintime = new SoftCob_LOGUEO_TIEMPOS();
                    {
                        loguintime.USUA_CODIGO = int.Parse(Session["usuCodigo"].ToString());
                        loguintime.empr_codigo = int.Parse(Session["CodigoEMPR"].ToString());
                        loguintime.cpce_codigo = int.Parse(ViewState["CodigoCatalago"].ToString());
                        loguintime.ltca_codigo = int.Parse(ViewState["idListaActiva"].ToString());
                        loguintime.loti_tipologueo = "IG";
                        _fechalogueo = DateTime.Now.ToString("MM/dd/yyyy");
                        loguintime.loti_fechalogueo = DateTime.ParseExact(_fechalogueo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        _horalogueo = DateTime.Now.ToString("HH:mm:ss");
                        loguintime.loti_horalogueo = TimeSpan.Parse(_horalogueo);
                        loguintime.loti_fechacompleta = DateTime.Now;
                        loguintime.loti_auxv1 = "";
                        loguintime.loti_auxv2 = "";
                        loguintime.loti_auxv3 = "";
                        loguintime.loti_auxv4 = "";
                        loguintime.loti_auxi1 = 0;
                        loguintime.loti_auxi2 = 0;
                        loguintime.loti_auxi3 = 0;
                        loguintime.loti_auxi4 = 0;
                        loguintime.loti_auxd1 = DateTime.Now;
                        loguintime.loti_auxd2 = DateTime.Now;
                        loguintime.loti_auxd3 = DateTime.Now;
                        loguintime.loti_auxd4 = DateTime.Now;
                        loguintime.loti_fechacreacion = DateTime.Now;
                        loguintime.loti_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                        loguintime.loti_terminalcreacion = Session["MachineName"].ToString();
                    }
                    new ControllerDAO().FunCrearLogueoTiempos(loguintime);

                    if (ViewState["Automatico"].ToString() == "SI") FunStartPhone();
                    else
                        Response.Redirect("wFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + ViewState["idListaActiva"].ToString(), true);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}