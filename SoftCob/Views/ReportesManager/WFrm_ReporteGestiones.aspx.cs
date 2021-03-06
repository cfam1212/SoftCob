namespace SoftCob.Views.ReportesManager
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteGestiones : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ListItem _itemc = new ListItem();
        ListItem _itemg = new ListItem();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ViewState["CodigoCEDE"] = Request["CodigoCEDE"];
                    ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                    ViewState["FechaDesde"] = Request["FechaDesde"];
                    ViewState["FechaHasta"] = Request["FechaHasta"];
                    ViewState["BuscarPor"] = Request["BuscarPor"];
                    ViewState["Criterio"] = Request["Criterio"];
                    ViewState["Gestor"] = Request["Gestor"];
                    TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    Lbltitulo.Text = "Reporte Gestiones";
                    FunCargarCombos(0);
                    if (ViewState["CodigoCEDE"] != null) FunCargarMantenimiento();
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

                if (!new FuncionesDAO().IsDate(TxtFechaIni.Text, "MM/dd/yyyy"))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this, "E", "C");
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaFin.Text, "MM/dd/yyyy"))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this, "E", "C");
                    return;
                }
                if (DateTime.ParseExact(TxtFechaIni.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this, "E", "C");
                    return;
                }

                if (DdlBuscar.SelectedItem.ToString() != "Todo" && string.IsNullOrEmpty(TxtBuscarPor.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese valor de Operación o Identificación..!", this, "W", "C");
                    return;
                }

                _dts = new ConsultaDatosDAO().FunGetRerporteGestiones(3, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text, TxtFechaFin.Text, DdlBuscar.SelectedValue, TxtBuscarPor.Text.Trim(), "", "", int.Parse(DdlGestor.SelectedValue),
                    0, Session["Conectar"].ToString());

                if (int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString()) > 0)
                {
                    Response.Redirect("WFrm_ReporteGestionesFixed.aspx?CodigoCEDE=" + DdlCedente.SelectedValue +
                        "&Catalogo=" + DdlCatalogo.SelectedItem.ToString() + "&CodigoCPCE=" + DdlCatalogo.SelectedValue +
                        "&FechaDesde=" + TxtFechaIni.Text.Trim() + "&FechaHasta=" + TxtFechaFin.Text.Trim() +
                        "&BuscarPor=" + DdlBuscar.SelectedValue + "&Criterio=" + TxtBuscarPor.Text.Trim() +
                        "&Gestor=" + DdlGestor.SelectedValue, true);
                }
                else new FuncionesDAO().FunShowJSMessage("No Existen Datos para Mostrar..!", this, "E", "C");
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtBuscarPor.Text = "";
            TxtBuscarPor.Enabled = true;
            switch (DdlBuscar.SelectedValue)
            {
                case "0":
                    TxtBuscarPor.Enabled = false;
                    break;
            }
        }

        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(1);
                FunCargarCombos(2);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }

        #endregion

        #region Procedimiento y Funciones
        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlCedente.DataSource = new CedenteDAO().FunGetCedentes();
                    DdlCedente.DataTextField = "Descripcion";
                    DdlCedente.DataValueField = "Codigo";
                    DdlCedente.DataBind();

                    _itemc.Text = "--Seleccione Catálago/Producto--";
                    _itemc.Value = "0";
                    DdlCatalogo.Items.Add(_itemc);

                    _itemg.Text = "--Seleccione Gestor--";
                    _itemg.Value = "0";
                    DdlGestor.Items.Add(_itemg);
                    break;
                case 1:
                    DdlGestor.Items.Clear();
                    _itemg.Text = "--Seleccione Gestor--";
                    _itemg.Value = "0";
                    DdlGestor.Items.Add(_itemg);
                    DdlGestor.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", 
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestor.DataTextField = "Descripcion";
                    DdlGestor.DataValueField = "Codigo";
                    DdlGestor.DataBind();
                    break;
                case 2:
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(81, int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "",
                        Session["Conectar"].ToString());
                    DdlCatalogo.DataSource = _dts;
                    DdlCatalogo.DataTextField = "Descripcion";
                    DdlCatalogo.DataValueField = "Codigo";
                    DdlCatalogo.DataBind();
                    break;
            }
        }

        protected void FunCargarMantenimiento()
        {
            try
            {
                DdlCedente.SelectedValue = ViewState["CodigoCEDE"].ToString();
                FunCargarCombos(1);
                FunCargarCombos(2);
                DdlCatalogo.SelectedValue = ViewState["CodigoCPCE"].ToString();
                TxtFechaIni.Text = ViewState["FechaDesde"].ToString();
                TxtFechaFin.Text = ViewState["FechaHasta"].ToString();
                DdlBuscar.SelectedValue = ViewState["BuscarPor"].ToString();
                if (DdlBuscar.SelectedValue != "0")
                {
                    TxtBuscarPor.Enabled = true;
                    TxtBuscarPor.Text = ViewState["Criterio"].ToString();
                }
                DdlGestor.SelectedValue = ViewState["Gestor"].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}