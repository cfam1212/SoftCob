namespace SoftCob.Views.ReportesManager
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_MonitoreoLstAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ListItem _itemc = new ListItem();
        ListItem _itemg = new ListItem();
        int _tipo = 0, _tipoc = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["CodigoCEDE"] = Request["CodigoCEDE"];
                ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                ViewState["FechaDesde"] = Request["FechaDesde"];
                ViewState["FechaHasta"] = Request["FechaHasta"];
                ViewState["Gestor"] = Request["Gestor"];
                TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Tablero de Control - Monitoreo de Listas de Trabajo";
                FunCargarCombos(0);
                if (ViewState["CodigoCEDE"] != null) FunCargarMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlCedente.DataSource = new CedenteDAO().FunGetCedentes();
                    DdlCedente.DataTextField = "Descripcion";
                    DdlCedente.DataValueField = "Codigo";
                    DdlCedente.DataBind();

                    DdlCatalogo.Items.Clear();
                    _itemc.Text = "--Seleccione Catálago/Producto--";
                    _itemc.Value = "0";
                    DdlCatalogo.Items.Add(_itemc);

                    _itemg.Text = "--Seleccione Gestor--";
                    _itemg.Value = "0";
                    DdlGestores.Items.Add(_itemg);

                    break;
                case 1:
                    _dts = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestores.DataSource = _dts;
                    DdlGestores.DataTextField = "Descripcion";
                    DdlGestores.DataValueField = "Codigo";
                    DdlGestores.DataBind();

                    //DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    //DdlCatalogo.DataTextField = "CatalogoProducto";
                    //DdlCatalogo.DataValueField = "CodigoCatalogo";
                    //DdlCatalogo.DataBind();
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
                DdlCatalogo.SelectedValue = ViewState["CodigoCPCE"].ToString();
                DdlGestores.SelectedValue = ViewState["Gestor"].ToString();
                TxtFechaIni.Text = ViewState["FechaDesde"].ToString();
                TxtFechaFin.Text = ViewState["FechaHasta"].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(1);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

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

                if (DdlGestores.SelectedValue == "0") _tipoc = 3;
                else _tipoc = 4;

                _dts = new ConsultaDatosDAO().FunGetMonitoreoAdmin(_tipoc, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(DdlCatalogo.SelectedValue), 0, 0, 0, 0, TxtFechaIni.Text, TxtFechaFin.Text,
                    int.Parse(DdlGestores.SelectedValue), "", "", "", int.Parse(DdlEstado.SelectedValue), 0, 0,
                    Session["Conectar"].ToString());

                if (DdlGestores.SelectedValue != "0") _tipo = 1;

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    if (DdlEstado.SelectedValue == "1" && ChkProceso.Checked)
                    {
                        Response.Redirect("WFrm_MonitorCheckProceso.aspx?CodigoCEDE=" + DdlCedente.SelectedValue +
                            "&Catalogo=" + DdlCatalogo.SelectedItem.ToString() + "&CodigoCPCE=" + DdlCatalogo.SelectedValue +
                            "&FechaDesde=" + TxtFechaIni.Text.Trim() + "&FechaHasta=" + TxtFechaFin.Text + "&Tipo=" + _tipo +
                            "&Gestor=" + DdlGestores.SelectedValue + "&Estado=" + DdlEstado.SelectedValue, true);
                    }
                    else
                    {
                        Response.Redirect("WFrm_MonitoreoLstAdmFixed.aspx?CodigoCEDE=" + DdlCedente.SelectedValue +
                            "&Catalogo=" + DdlCatalogo.SelectedItem.ToString() + "&CodigoCPCE=" + DdlCatalogo.SelectedValue +
                            "&FechaDesde=" + TxtFechaIni.Text.Trim() + "&FechaHasta=" + TxtFechaFin.Text + "&Tipo=" + _tipo +
                            "&Gestor=" + DdlGestores.SelectedValue + "&Estado=" + DdlEstado.SelectedValue, true);
                    }
                }
                else new FuncionesDAO().FunShowJSMessage("No Existe Datos Para Mostrar..!", this, "E", "C");
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
    }
}