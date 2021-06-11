namespace SoftCob.Views.BPM
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Configuration;
    public partial class WFrm_DatosCanalCitacion : Page
    {

        #region Variables
        DataSet _dts = new DataSet();
        int _opcion = 0;
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
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    ViewState["CodigoCITA"] = Request["CodigoCITA"];
                    ViewState["Canal"] = Request["Canal"];
                    Lbltitulo.Text = "LISTA DATOS CANAL - <<" + ViewState["Canal"].ToString() + ">>";
                    FunCargarDatos();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        public void FunCargarDatos()
        {
            try
            {
                switch (ViewState["Canal"].ToString())
                {
                    case "Whatsapp":
                        _opcion = 0;
                        break;
                    case "Email":
                        _opcion = 1;
                        break;
                    case "Terreno":
                        _opcion = 2;
                        break;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(246, _opcion, int.Parse(ViewState["CodigoCITA"].ToString()), 0, "",
                    ViewState["Canal"].ToString(), "", ViewState["Conectar"].ToString());

                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}
