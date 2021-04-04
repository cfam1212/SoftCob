namespace SoftCob.Views.Mantenedor
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    public partial class WFrm_Detalle : Page
    {
        #region Variables
        DataSet _dts = new DataSet(); 
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];

                try
                {
                    LblUsuario.Text = Session["usuNombres"].ToString();
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 0, 0, 0, "", "LOGODE", "PATH LOGOS", ViewState["Conectar"].ToString());
                    if (_dts.Tables[0].Rows.Count > 0) ImgLogo.ImageUrl = _dts.Tables[0].Rows[0]["Valor"].ToString();
                    if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                        Response.Redirect("~/Reload.html");
                }
                catch (Exception)
                {
                    Response.Redirect("~/Reload.html");
                }
            }
        } 
        #endregion
    }
}