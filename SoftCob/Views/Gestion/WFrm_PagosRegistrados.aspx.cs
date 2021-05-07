namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Configuration;
    public partial class WFrm_PagosRegistrados : Page
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
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];

                    ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                    ViewState["Cedula"] = Request["Cedula"];
                    ViewState["Year"] = Request["Year"];
                    ViewState["Month"] = Request["Month"];
                    ViewState["Gestor"] = Request["Gestor"];
                    //Lbltitulo.Text = "Registros PAGOS REALIZADOS";
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
                _dts = new ConsultaDatosDAO().FunConsultaDatos(175, 0, 0, 0, "", ViewState["Cedula"].ToString(), "",
                    ViewState["Conectar"].ToString());

                Lbltitulo.Text = "Titular: << " + _dts.Tables[0].Rows[0]["Nombres"].ToString()
                    + " >> " + ViewState["Cedula"].ToString();

                _dts = new PagoCarteraDAO().FunGetPagoCartera(22, 0, int.Parse(ViewState["CodigoCPCE"].ToString()),
                     ViewState["Cedula"].ToString(), "", "", "", "", "", "", "", "",
                     int.Parse(ViewState["Year"].ToString()), int.Parse(ViewState["Month"].ToString()),
                     int.Parse(ViewState["Gestor"].ToString()), 0, "", ViewState["Conectar"].ToString());

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