namespace SoftCob.Views.ReportesGerenciales
{
    using ControllerSoftCob;
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteGerencialV1 : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbaccion = new DataTable();
        DataTable _dtbresult = new DataTable();
        DataTable _dtbefectivo = new DataTable();
        DataTable _dtbcomision = new DataTable();
        DataTable _dtbpago = new DataTable();
        DataTable _dtbgestion = new DataTable();
        ListItem _itemc = new ListItem();
        ListItem _itemg = new ListItem();
        string _imagepath = "";
        int _codigo = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    Lbltitulo.Text = "Reporte Gerencial POR ARBOL-GESTIONES << BUSISSNESS INTELLIGENCE >>";
                    ViewState["Boton"] = "0";
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
                    _dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
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
                DdlGestor.SelectedValue = ViewState["Gestor"].ToString();
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
                FunCargarCombos(2);
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
                TblReporte.Visible = false;

                if (ViewState["Boton"].ToString() == "0")
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

                    if (DdlTipo.SelectedValue == "0")
                    {
                        _dts = new ConsultaDatosDAO().FunRepGerencialG(DdlGestor.SelectedValue == "0" ? 0 : 1, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, Session["Conectar"].ToString());
                        _dtbaccion = _dts.Tables[0];
                    }

                    if (DdlTipo.SelectedValue == "1")
                    {
                        _dts = new ConsultaDatosDAO().FunRepGerencialG(6, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", DdlGestor.SelectedValue == "0" ? 0 : 1, 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                        _dtbefectivo = _dts.Tables[0];

                        _dts = new ConsultaDatosDAO().FunRepGerencialG(7, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", DdlGestor.SelectedValue == "0" ? 0 : 1, 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                        _dtbcomision = _dts.Tables[0];

                        _dts = new ConsultaDatosDAO().FunRepGerencialG(8, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", DdlGestor.SelectedValue == "0" ? 0 : 1, 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                        _dtbpago = _dts.Tables[0];

                        _dts = new ConsultaDatosDAO().FunRepGerencialG(9, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", DdlGestor.SelectedValue == "0" ? 0 : 1, 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                        _dtbgestion = _dts.Tables[0];
                    }

                    if (DdlTipo.SelectedValue == "2")
                    {
                        _dts = new ConsultaDatosDAO().FunRepGerencialG(10, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", DdlGestor.SelectedValue == "0" ? 0 : 1, 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                        _dtbaccion = _dts.Tables[0];
                    }

                    if (_dts.Tables[0].Rows.Count == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("No Existen Datos Para Mostrar..!", this, "E", "C");
                        return;
                    }

                    BtnProcesar.Text = "Consultar";
                    ViewState["Boton"] = "1";
                    TblOpciones.Visible = false;
                    TblReporte.Visible = true;

                    RptViewDatos.LocalReport.DataSources.Clear();
                    RptViewDatos.LocalReport.EnableExternalImages = true;
                    _imagepath = new Uri(Server.MapPath("~/Images/LogoGS1.png")).AbsoluteUri;

                    ReportParameter[] parameters = new ReportParameter[6];
                    parameters[0] = new ReportParameter("Cedente", DdlCedente.SelectedItem.ToString());
                    parameters[1] = new ReportParameter("Catalogo", DdlCatalogo.SelectedItem.ToString());
                    parameters[2] = new ReportParameter("FechaD", TxtFechaIni.Text.Trim());
                    parameters[3] = new ReportParameter("FechaH", TxtFechaFin.Text.Trim());
                    parameters[4] = new ReportParameter("Operador", DdlGestor.SelectedValue == "0" ? "TODOS" : DdlGestor.SelectedItem.ToString());
                    parameters[5] = new ReportParameter("Logo", _imagepath);

                    if (DdlTipo.SelectedValue == "0")
                    {
                        RptViewDatos.LocalReport.ReportPath = Server.MapPath("../Reports/Rpt_GerencialV1.rdlc");
                        RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsGerencialV1", _dtbaccion));
                        RptViewDatos.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(GerenEfectoSubRerport);
                        RptViewDatos.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(GerenRespuestaSubRerport);
                    }

                    if (DdlTipo.SelectedValue == "1")
                    {
                        RptViewDatos.LocalReport.ReportPath = Server.MapPath("../Reports/Rpt_GerencialV2.rdlc");
                        RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsEfectivo", _dtbefectivo));
                        //RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsComision", _dtbcomision));
                        RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsPago", _dtbpago));
                        RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsGestiones", _dtbgestion));
                    }

                    if (DdlTipo.SelectedValue == "2")
                    {
                        RptViewDatos.LocalReport.ReportPath = Server.MapPath("../Reports/Rpt_CompPagoGen.rdlc");
                        RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsCompPago", _dtbaccion));
                        RptViewDatos.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(CompEfecSubReport);
                        RptViewDatos.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(CompRepSubReport);
                    }

                    RptViewDatos.LocalReport.SetParameters(parameters);
                    RptViewDatos.LocalReport.Refresh();
                }
                else
                {
                    ViewState["Boton"] = "0";
                    TblOpciones.Visible = true;
                    BtnProcesar.Text = "Procesar";
                    TblReporte.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        void CompEfecSubReport(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                _codigo = int.Parse(e.Parameters["CodigoID"].Values[0].ToString());

                _dts = new ConsultaDatosDAO().FunRepGerencialG(11, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", DdlGestor.SelectedValue == "0" ? 0 : 1, _codigo, 0, 0, 0, 0, Session["Conectar"].ToString());

                _dtbresult = _dts.Tables[0];

                ReportDataSource dts = new ReportDataSource("DtsCompEfec", _dtbresult);
                e.DataSources.Add(dts);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        void CompRepSubReport(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                _codigo = int.Parse(e.Parameters["CodigoID"].Values[0].ToString());

                _dts = new ConsultaDatosDAO().FunRepGerencialG(12, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", DdlGestor.SelectedValue == "0" ? 0 : 1, _codigo, 0, 0, 0, 0, Session["Conectar"].ToString());

                _dtbresult = _dts.Tables[0];

                ReportDataSource dts = new ReportDataSource("DtsCompResp", _dtbresult);
                e.DataSources.Add(dts);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        void GerenEfectoSubRerport(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                _codigo = int.Parse(e.Parameters["CodigoID"].Values[0].ToString());

                _dts = new ConsultaDatosDAO().FunRepGerencialG(DdlGestor.SelectedValue == "0" ? 2 : 3, int.Parse(DdlCedente.SelectedValue), 
                    int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), 
                    "sp_RepGerencialV1", "", "", "", "", "", "", _codigo, 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                _dtbresult = _dts.Tables[0];

                ReportDataSource dts = new ReportDataSource("DtsEfecto", _dtbresult);
                e.DataSources.Add(dts);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        void GerenRespuestaSubRerport(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                _codigo = int.Parse(e.Parameters["CodigoID"].Values[0].ToString());

                _dts = new ConsultaDatosDAO().FunRepGerencialG(DdlGestor.SelectedValue == "0" ? 4 : 5, int.Parse(DdlCedente.SelectedValue), 
                    int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), 
                    "sp_RepGerencialV1", "", "", "", "", "", "", _codigo, 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                _dtbresult = _dts.Tables[0];

                ReportDataSource dts = new ReportDataSource("DtsRespuesta", _dtbresult);
                e.DataSources.Add(dts);
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