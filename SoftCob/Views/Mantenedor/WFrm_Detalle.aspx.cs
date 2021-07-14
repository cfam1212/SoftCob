namespace SoftCob.Views.Mantenedor
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    public partial class WFrm_Detalle : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        int _contar = 0;
        string _mensaje = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                try
                {
                    LblUsuario.Text = Session["usuNombres"].ToString();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 0, 0, 0, "", "LOGODE", "PATH LOGOS", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0) ImgLogo.ImageUrl = _dts.Tables[0].Rows[0]["Valor"].ToString();

                    if (Session["CrearParam"].ToString() == "SI")
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 0, 0, 0, "", "", "",
                            Session["Conectar"].ToString());

                        _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                        if (_contar > 0)
                        {
                            _mensaje = "Tiene " + _contar + " SOLICITUD CITACION(ES) Pendiente(s)";
                            new FuncionesDAO().FunShowJSMessage(_mensaje, this, "W", "R");
                        }
                        else
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 1, 0, 0, "", "", "",
                                Session["Conectar"].ToString());

                            _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                            if (_contar > 0)
                            {
                                _mensaje = "Tiene " + _contar + " CITACION(ES) En PROCESO";
                                new FuncionesDAO().FunShowJSMessage(_mensaje, this, "N", "R");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Redirect("~/Reload.html");
                }
            }
        } 
        #endregion
    }
}