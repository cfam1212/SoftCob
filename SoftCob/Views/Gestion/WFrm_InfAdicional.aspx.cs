namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System.Configuration;
    using System;
    using System.Data;
    using System.Web.UI;
    
    public partial class WFrm_InfAdicional : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                ViewState["CodigoCEDE"] = Request["CodigoCEDE"];
                ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                ViewState["CodigoPERS"] = Request["CodigoPERS"];
                ViewState["Operacion"] = Request["Operacion"];
                Lbltitulo.Text = "Datos Adicionales";
                FunCargarMantenimiento(0);

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento(int v)
        {
            _dts = new ConsultaDatosDAO().FunDatosAdicionales(int.Parse(ViewState["CodigoCEDE"].ToString()),
                int.Parse(ViewState["CodigoCPCE"].ToString()), int.Parse(ViewState["CodigoPERS"].ToString()),
                ViewState["Operacion"].ToString(), "", "", "", 0, 0, 0, ViewState["Conectar"].ToString());

            GrdvDatos.DataSource = _dts;
            GrdvDatos.DataBind();
        }
        #endregion

        #region Botones y Eventos
        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }
        #endregion
    }
}