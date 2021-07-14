namespace SoftCob.Views.BPM
{
    using System;
    using System.Data;
    using System.Web.UI;
    public partial class WFrm_DocumentosView : Page
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
                    ViewState["Path"] = Request["Path"];
                    Lbltitulo.Text = "Visor de Documentos";
                    FunCargaMantenimiento();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            try
            {
                Imgdocumento.ImageUrl = ViewState["Path"].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}