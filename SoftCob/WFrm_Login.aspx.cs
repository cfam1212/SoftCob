namespace SoftCob
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web.UI;

    public partial class WFrm_Login : Page
    {
        #region Variables
        string _horalogueo = "", _fechalogueo = "", _IP = "", _fechaactual = "", _fechapago = "";
        int _usucodigo = 0;
        DataSet _dts;
        DateTime _dtmfechaatual, _dtmfechapago;
        DataRow _resul;
        TimeSpan _dias;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Session["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    Session["Phone"] = ConfigurationManager.AppSettings["Phone"];
                    ViewState["Licencia"] = ConfigurationManager.AppSettings["Licencia"];
                    ViewState["SQL"] = ConfigurationManager.AppSettings["FormSQL"];                    
                    Session["usuLogin"] = "";
                    Session["usuCodigo"] = "";
                    Session["IPLocalAdress"] = "";
                    Session["IN-CALL"] = "NO";
                    Session["IPLocalAdress"] = "";
                    Session["LICENCIA"] = "NO";
                    Session["DiasLIC"] = "0";
                    //FunGetLic();
                    IPHostEntry NombreHost = Dns.GetHostEntry(Request.UserHostAddress);
                    Session["IPLocalAdress"] = NombreHost.AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                    _IP = NombreHost.HostName;
                    Session["MachineName"] = _IP.Substring(0, _IP.IndexOf("."));
                }
                catch
                {
                    FunGetTerminal();
                    Session["MachineName"] = Session["IPLocalAdress"].ToString();
                }
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnIngresar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["MachineName"] == null) FunGetTerminal();

                _usucodigo = new ControllerDAO().FunGetLogin(0, TxtUsuario.Text, TxtClave.Text);

                if (_usucodigo != 0 && _usucodigo != -1 && _usucodigo != -2)
                {
                    SoftCob_USUARIO _user = new ControllerDAO().FunGetUsuarioPorID(_usucodigo);
                    Session["CodigoEMPR"] = "0";
                    Session["usuCodigo"] = _usucodigo;
                    Session["usuPerfil"] = _user.PERF_CODIGO;
                    Session["usuNombres"] = _user.usua_nombres + " " + _user.usua_apellidos;
                    Session["usuSoloNombre"] = _user.usua_nombres;
                    Session["usuCambiarPass"] = _user.usua_cambiarpass;
                    ViewState["FechaCaduca"] = _user.usua_fechacaduca.ToString("MM/dd/yyyy");
                    Session["IN-CALL"] = "NO";
                    Session["PermisoEspecial"] = _user.usua_permisosespeciales ? "SI" : "NO";
                    Session["CedeCodigo"] = new ControllerDAO().FunGetGestor(_usucodigo);
                    Session["CrearParam"] = _user.SoftCob_PERFIL.perf_crearparametro == true ? "SI" : "NO";

                    //Verificar si el password no esta 
                    if (_user.usua_caducapass)
                    {
                        if (DateTime.ParseExact(ViewState["usuFechaCaduca"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture) <=
                            DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture))
                        {
                            Lblmensaje.Text = "Exipiró Acceso al Usuario, consulte con el Administrador..!";
                            return;
                        }
                    }
                    //Actualizar Status de logueo
                    SoftCob_USUARIO _usua = new SoftCob_USUARIO();
                    {
                        _usua.USUA_CODIGO = _usucodigo;
                        _usua.usua_statuslogin = true;
                        _usua.usua_terminallogin = Session["MachineName"].ToString();
                    }
                    new ControllerDAO().FunUpdateLogueo(_usua);
                    //Registrar Logueo inicial
                    SoftCob_LOGUEO_TIEMPOS _loguintime = new SoftCob_LOGUEO_TIEMPOS();
                    {
                        _loguintime.USUA_CODIGO = _usucodigo;
                        _loguintime.empr_codigo = 0;
                        _loguintime.cpce_codigo = 0;
                        _loguintime.ltca_codigo = 0;
                        _loguintime.loti_tipologueo = "LI";
                        _fechalogueo = DateTime.Now.ToString("MM/dd/yyyy");
                        _loguintime.loti_fechalogueo = DateTime.ParseExact(_fechalogueo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        _horalogueo = DateTime.Now.ToString("HH:mm:ss");
                        _loguintime.loti_horalogueo = TimeSpan.Parse(_horalogueo);
                        _loguintime.loti_fechacompleta = DateTime.Now;
                        _loguintime.loti_auxv1 = "";
                        _loguintime.loti_auxv2 = "";
                        _loguintime.loti_auxv3 = "";
                        _loguintime.loti_auxv4 = "";
                        _loguintime.loti_auxv5 = "";
                        _loguintime.loti_auxi1 = 0;
                        _loguintime.loti_auxi2 = 0;
                        _loguintime.loti_auxi3 = 0;
                        _loguintime.loti_auxi4 = 0;
                        _loguintime.loti_auxi5 = 0;
                        _loguintime.loti_auxd1 = DateTime.Now;
                        _loguintime.loti_auxd2 = DateTime.Now;
                        _loguintime.loti_auxd3 = DateTime.Now;
                        _loguintime.loti_auxd4 = DateTime.Now;
                        _loguintime.loti_auxd5 = DateTime.Now;
                        _loguintime.loti_fechacreacion = DateTime.Now;
                        _loguintime.loti_usuariocreacion = _usucodigo;
                        _loguintime.loti_terminalcreacion = Session["MachineName"].ToString();
                    }
                    new ControllerDAO().FunCrearLogueoTiempos(_loguintime);
                    Response.Redirect("~/Views/Mantenedor/WFrm_Principal.aspx", false);
                }
                else
                {
                    switch (_usucodigo)
                    {
                        case 0:
                            Lblmensaje.Text = "Usuario o Password incorrecto..!";
                            break;
                        case -1:
                            Lblmensaje.Text = "Usuario se encuentra logueado..!";
                            break;
                        case -2:
                            Lblmensaje.Text = "Usuario se encuentra logueado en esta estación..!";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        public void FunGetTerminal()
        {
            try
            {
                Session["IN-CALL"] = "NO";
                IPHostEntry NombreHost = Dns.GetHostEntry(Request.UserHostAddress);
                Session["IPLocalAdress"] = NombreHost.AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                string IP = NombreHost.HostName;
                Session["MachineName"] = IP.Substring(0, IP.IndexOf("."));
            }
            catch
            {
                Session["MachineName"] = Session["IPLocalAdress"].ToString();
            }
        }
        private void FunGetLic()
        {
            try
            {
                _fechaactual = DateTime.Now.ToString("yyyy-MM-dd");
                _dtmfechaatual = DateTime.ParseExact(_fechaactual, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (new FuncionesDAO().FunDesencripta(ViewState["Licencia"].ToString()) != "Permanente")
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0,
                        new FuncionesDAO().FunDesencripta(ViewState["SQL"].ToString()), "", "",
                        Session["Conectar"].ToString());

                    if (_dts == null || _dts.Tables[0].Rows.Count == 0)
                    {
                        TxtUsuario.Enabled = false;
                        TxtClave.Enabled = false;
                        BtnIngresar.Enabled = false;
                        Lblmensaje.Text = "INGRESE KEY VALIDA PARA ACTIVAR EL SISTEMA";
                    }
                    else
                    {
                        _resul = _dts.Tables[0].Select("Estado='" + "8NvnWyoj2ew=" + "'").FirstOrDefault();
                        _fechapago = new FuncionesDAO().FunDesencripta(_resul["Fecha"].ToString());

                        _dtmfechapago = DateTime.ParseExact(_fechapago, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        if (_dtmfechaatual > _dtmfechapago)
                        {
                            TxtUsuario.Enabled = false;
                            TxtClave.Enabled = false;
                            BtnIngresar.Enabled = false;
                            Lblmensaje.Text = "INGRESE KEY VALIDA PARA ACTIVAR EL SISTEMA";
                        }

                        if (_dtmfechapago >= _dtmfechaatual)
                        {
                            _dias = _dtmfechapago.Subtract(_dtmfechaatual);

                            if (_dias.Days <= 10)
                            {
                                Session["LICENCIA"] = "SI";
                                Session["DiasLIC"] = _dias.Days;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                TxtUsuario.Enabled = false;
                TxtClave.Enabled = false;
                BtnIngresar.Enabled = false;
                Lblmensaje.Text = "INGRESE KEY VALIDA PARA ACTIVAR EL SISTEMA";
            }
        }
        #endregion

    }
}