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

                    if (Session["LICENCIA"].ToString() == "SI")
                    {
                        if (int.Parse(Session["DiasLIC"].ToString()) < 5)
                        {
                            _mensaje = "Estimado Usuario, le quedan " + Session["DiasLIC"].ToString() + " Dia(s) ";
                            _mensaje += "EL SISTEMA QUEDARA INACTIVO CUANDO SE LLEGUE AL DIA 0, Comuniquese con su proveedor";

                            ScriptManager.RegisterStartupScript(this, GetType(), "pop",
                                "javascript: alertify.set('notifier','position', 'top-center'); alertify.error('" +
                                _mensaje + "', 100, function(){  console.log('dismissed'); });", true);

                            Session["LICENCIA"] = "NO";
                        }
                        else
                        {
                            _mensaje = "Estimado Usuario, le quedan " + Session["DiasLIC"].ToString() + " Dia(s) ";
                            _mensaje += "Para renovar la licencia, Comuniquese con su proveedor";

                            ScriptManager.RegisterStartupScript(this, GetType(), "pop",
                                "javascript: alertify.set('notifier','position', 'top-center'); alertify.warning('" +
                                _mensaje + "', 50, function(){  console.log('dismissed'); });", true);

                            Session["LICENCIA"] = "NO";
                        }
                    }

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 3, int.Parse(Session["usuCodigo"].ToString()), 0, "", "",
                        "", Session["Conectar"].ToString());

                    _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                    if (_contar > 0)
                    {
                        _mensaje = "TIENE " + _contar + " NOTIFICACION(ES) POR PROCESAR";
                        new FuncionesDAO().FunShowJSMessage(_mensaje, this, "W", "R");
                    }

                    if (Session["CrearParam"].ToString() == "SI")
                    {
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 0, 0, 0, "", "", "",
                            Session["Conectar"].ToString());

                        _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                        if (_contar > 0) _mensaje = "Tiene " + _contar + " SOLICITUD(ES) NOTIFICACION(ES) PENDIENTE(s)";
                        else
                        {
                            _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 1, 0, 0, "", "", "",
                                Session["Conectar"].ToString());

                            _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                            if (_contar > 0) _mensaje = "Tiene " + _contar + " NOTIFICACION(ES) EN PROCESO";
                            else
                            {
                                _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 4, 0, 0, "", "", "",
                                    Session["Conectar"].ToString());

                                _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                                if (_contar > 0) _mensaje = "Tiene " + _contar + " NOTIFICACION(ES) TERRENO GENERADAS POR GESTIONAR";
                                else
                                {
                                    _dts = new ConsultaDatosDAO().FunConsultaDatos(252, 5, 0, 0, "", "", "",
                                        Session["Conectar"].ToString());

                                    _contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                                    if (_contar > 0) _mensaje = "Tiene " + _contar + " NOTIFICACION(ES) MAIL GENERADAS POR GESTIONAR";
                                }
                            }
                        }

                        if (_contar > 0)
                            new FuncionesDAO().FunShowJSMessage(_mensaje, this, "W", "R");
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