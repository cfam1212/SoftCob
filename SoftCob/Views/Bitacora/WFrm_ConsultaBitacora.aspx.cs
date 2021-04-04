namespace SoftCob.Views.Bitacora
{
    using System;
    using System.Web.UI;
    public partial class WFrm_ConsultaBitacora : Page
    {
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Consulta General << BITACORA DE REGISTROS >>";
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_ConsultaBitacoraAdmin.aspx?FechaDesde=" + TxtFechaIni.Text.Trim() + "&FechaHasta=" +
                TxtFechaFin.Text.Trim(), true);
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}