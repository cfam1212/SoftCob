namespace SoftCob.Views.ReportesManager
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteRegistroPagos : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ListItem _itemc = new ListItem();
        TimeSpan _turnotarde, _turnonoche, _tiempoactual, _diferencia;
        int _minutoslatencia = 0;
        string _validar = "";
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
                    Lbltitulo.Text = "Reporte Registro Pagos Cartera";
                    _tiempoactual = DateTime.Now.TimeOfDay;
                    _dts = new ControllerDAO().FunGetDatosParametroDet("HORARIOS SALIDA");

                    if (_dts.Tables[0].Rows.Count == 0) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);

                    if (_dts.Tables[0].Rows[0]["Prametro"].ToString() == "VALIDAR") _validar = _dts.Tables[0].Rows[0]["ValorV"].ToString();

                    if (_dts.Tables[0].Rows[0]["Prametro"].ToString() == "SALIDA TARDE") _turnotarde = TimeSpan.Parse(_dts.Tables[0].Rows[0]["ValorV"].ToString());

                    if (_dts.Tables[0].Rows[1]["Prametro"].ToString() == "SALIDA NOCHE") _turnonoche = TimeSpan.Parse(_dts.Tables[0].Rows[1]["ValorV"].ToString());

                    if (_dts.Tables[0].Rows[2]["Prametro"].ToString() == "MINUTOS LATENCIA") _minutoslatencia = int.Parse(_dts.Tables[0].Rows[2]["ValorI"].ToString());

                    if (Session["IN-CALL"].ToString() == "SI") Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);

                    if (_validar == "SI")
                    {
                        _diferencia = _turnotarde - _tiempoactual;

                        if (_diferencia.Hours > 0) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);

                        if (_diferencia.Hours == 0 && _diferencia.Minutes > _minutoslatencia) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);

                        if (_diferencia.Hours < 0)
                        {
                            _diferencia = _turnonoche - _tiempoactual;

                            if (_diferencia.Hours > 0) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);

                            if (_diferencia.Hours == 0 && _diferencia.Minutes > _minutoslatencia) Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
                        }
                    }

                    TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    FunCargarCombos(0);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(140, int.Parse(Session["usuCodigo"].ToString()), 0, 0, "", "", "", Session["Conectar"].ToString());
                        DdlCedente.DataSource = _dts;
                        DdlCedente.DataTextField = "Descripcion";
                        DdlCedente.DataValueField = "Codigo";
                        DdlCedente.DataBind();
                        DdlCedente.SelectedIndex = 1;
                        FunCargarCombos(1);
                        _itemc.Text = "--Seleccione Catálago/Producto--";
                        _itemc.Value = "0";
                        DdlCatalogo.Items.Add(_itemc);
                        break;
                    case 1:
                        DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                        DdlCatalogo.DataTextField = "CatalogoProducto";
                        DdlCatalogo.DataValueField = "CodigoCatalogo";
                        DdlCatalogo.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this, "W", "C");
                    return;
                }

                if (DdlCatalogo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo..!", this, "W", "C");
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaIni.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaFin.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this, "E", "C");
                    return;
                }

                if (DateTime.ParseExact(TxtFechaIni.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this, "E", "C");
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(139, int.Parse(DdlCatalogo.SelectedValue), int.Parse(Session["usuCodigo"].ToString()), 0, "", TxtFechaIni.Text, TxtFechaFin.Text, Session["Conectar"].ToString());

                if (int.Parse(_dts.Tables[0].Rows[0]["Total"].ToString()) > 0)
                {
                    Response.Redirect("WFrm_ReporteRegPagosFixed.aspx?CodigoCPCE=" + DdlCatalogo.SelectedValue +
                        "&FechaDesde=" + TxtFechaIni.Text.Trim() + "&FechaHasta=" + TxtFechaFin.Text.Trim(), true);
                }
                else new FuncionesDAO().FunShowJSMessage("No Existen Datos para Mostrar..!", this, "E", "C");
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }
        #endregion
    }
}