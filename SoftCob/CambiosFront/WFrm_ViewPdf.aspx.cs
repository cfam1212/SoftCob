namespace Pry_wGSBPO.GSBPO.BPM
{
    using System;
    using System.Web.UI;

    public partial class WFrm_ViewPdf : Page
    {
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
                string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"500px\">";
                embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                embed += "</object>";
                ltEmbed.Text = string.Format(embed, ResolveUrl(ViewState["Path"].ToString()));
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}