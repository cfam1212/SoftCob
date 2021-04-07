namespace SoftCob.Views.Breanch
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    public partial class WFrm_BrenchGestiones : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["CodigoCEDE"] = Request["CodigoCEDE"];
                ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                ViewState["CodigoGEST"] = Request["CodigoGEST"];
                ViewState["Operacion"] = Request["Operacion"];
                Session["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                Lbltitulo.Text = "Gestiones Realizadas";
                FunCargarDatos();
            }
        }
        #endregion

        #region Procedimientos y Funciones 
        private void FunCargarDatos()
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(223, int.Parse(ViewState["CodigoCPCE"].ToString()),
                int.Parse(ViewState["CodigoGEST"].ToString()), 0, "", ViewState["Operacion"].ToString(), "",
                Session["Conectar"].ToString());

            GrdvGestiones.DataSource = _dts;
            GrdvGestiones.DataBind();
        }
        #endregion
    }
}