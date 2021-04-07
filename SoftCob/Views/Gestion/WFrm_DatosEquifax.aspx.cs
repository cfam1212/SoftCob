namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    public partial class WFrm_DatosEquifax : Page
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
                    Lbltitulo.Text = "Consulta Datos << VARIOS >>";
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    ViewState["CodigoPERS"] = Request["CodigoPERS"];
                    ViewState["Operacion"] = Request["Operacion"];
                    FunCargarMantenimiento();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento()
        {
            _dts = new ConsultaDatosDAO().FunConsultaDatos(210, int.Parse(ViewState["CodigoPERS"].ToString()),
                int.Parse(Session["usuCodigo"].ToString()), 0, "", ViewState["Operacion"].ToString(), "",
                ViewState["Conectar"].ToString());

            if (_dts.Tables[0].Rows.Count > 0)
            {
                LblCodigo.Text = _dts.Tables[0].Rows[0]["Codigo"].ToString();
                LblFisico.Text = _dts.Tables[0].Rows[0]["Fisico"].ToString();
                LblProducto.Text = _dts.Tables[0].Rows[0]["Producto"].ToString();
                LblFecha.Text = _dts.Tables[0].Rows[0]["Fecha"].ToString();
            }
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