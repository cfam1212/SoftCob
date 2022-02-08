namespace SoftCob.Views.ReportesGerenciales
{
    using ControllerSoftCob;
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteGerencialV2 : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbefecges1 = new DataTable();
        DataTable _dtbefecges2 = new DataTable();
        DataTable _dtbpagoges1 = new DataTable();
        DataTable _dtbpagoges2 = new DataTable();
        DataTable _dtbgestor = new DataTable();
        ListItem _itemc = new ListItem();
        ListItem _itemg = new ListItem();
        DataRow _filagre, _result;
        string _codigo = "", _imagepath = "";
        int _row = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    _dtbgestor.Columns.Add("Codigo");
                    _dtbgestor.Columns.Add("Gestor");
                    ViewState["Gestores"] = _dtbgestor;
                    TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    Lbltitulo.Text = "Reporte Gerencial Efectvidiad Gestores << BUSINESS INTELLIGENCE >>";
                    ViewState["Boton"] = "0";

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 0, 0, 0, "", "LOGOFIRMA", "PATH LOGOS", Session["Conectar"].ToString());
                    if (_dts.Tables[0].Rows.Count > 0) ViewState["LogoReporte"] = _dts.Tables[0].Rows[0]["Valor"].ToString();
                    else ViewState["LogoReporte"] = "~/Images/LogoBBP.png";

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

        protected void DdlGestor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DdlGestor.SelectedValue != "0")
                {
                    _dtbgestor = (DataTable)ViewState["Gestores"];

                    if (GrdvGestor.Rows.Count < 2)
                    {
                        _filagre = _dtbgestor.NewRow();
                        _filagre["Codigo"] = DdlGestor.SelectedValue;
                        _filagre["Gestor"] = DdlGestor.SelectedItem.ToString();
                        _dtbgestor.Rows.Add(_filagre);
                        GrdvGestor.DataSource = _dtbgestor;
                        GrdvGestor.DataBind();
                    }
                    DdlGestor.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvGestor.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString();
                _dtbgestor = (DataTable)ViewState["Gestores"];
                _result = _dtbgestor.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                _dtbgestor.AcceptChanges();
                ViewState["Gestores"] = _dtbgestor;
                GrdvGestor.DataSource = _dtbgestor;
                GrdvGestor.DataBind();
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
                        new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this, "W", "C");
                        return;
                    }

                    if (!new FuncionesDAO().IsDate(TxtFechaFin.Text, "MM/dd/yyyy"))
                    {
                        new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this, "W", "C");
                        return;
                    }

                    if (DateTime.ParseExact(TxtFechaIni.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFechaFin.Text, 
                        "MM/dd/yyyy", CultureInfo.InvariantCulture))
                    {
                        new FuncionesDAO().FunShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this, "W", "C");
                        return;
                    }

                    _dtbgestor = (DataTable)ViewState["Gestores"];

                    if (_dtbgestor.Rows.Count < 2)
                    {
                        new FuncionesDAO().FunShowJSMessage("Debe Seleccionar 2 Gestores...!", this, "W", "C");
                        return;
                    }

                    BtnProcesar.Text = "Consultar";
                    ViewState["Boton"] = "1";
                    TblOpciones.Visible = false;
                    TblReporte.Visible = true;

                    RptViewDatos.LocalReport.DataSources.Clear();
                    RptViewDatos.LocalReport.EnableExternalImages = true;
                    _imagepath = new Uri(Server.MapPath(ViewState["LogoReporte"].ToString())).AbsoluteUri;

                    _row = 1;
                    foreach (DataRow _fila in _dtbgestor.Rows)
                    {
                        switch (_row)
                        {
                            case 1:
                                ViewState["Codigo1"] = _fila["Codigo"];
                                ViewState["Gestor1"] = _fila["Gestor"];
                                break;
                            case 2:
                                ViewState["Codigo2"] = _fila["Codigo"];
                                ViewState["Gestor2"] = _fila["Gestor"];
                                break;
                        }
                        _row++;
                    }

                    _dts = new ConsultaDatosDAO().FunRepGerencialG(6, int.Parse(DdlCedente.SelectedValue), 
                        int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), 
                        int.Parse(ViewState["Codigo1"].ToString()), "sp_RepGerencialV1", "", "", "", "", "", "", 1, 0, 0, 0, 0, 0, 
                        Session["Conectar"].ToString());

                    _dtbefecges1 = _dts.Tables[0];

                    _dts = new ConsultaDatosDAO().FunRepGerencialG(6, int.Parse(DdlCedente.SelectedValue), 
                        int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), 
                        int.Parse(ViewState["Codigo2"].ToString()), "sp_RepGerencialV1", "", "", "", "", "", "", 1, 0, 0, 0, 0, 0, 
                        Session["Conectar"].ToString());

                    _dtbefecges2 = _dts.Tables[0];

                    _dts = new ConsultaDatosDAO().FunRepGerencialG(8, int.Parse(DdlCedente.SelectedValue), 
                        int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), 
                        int.Parse(ViewState["Codigo1"].ToString()), "sp_RepGerencialV1", "", "", "", "", "", "", 1, 0, 0, 0, 0, 0, 
                        Session["Conectar"].ToString());

                    _dtbpagoges1 = _dts.Tables[0];

                    _dts = new ConsultaDatosDAO().FunRepGerencialG(8, int.Parse(DdlCedente.SelectedValue), 
                        int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), 
                        int.Parse(ViewState["Codigo2"].ToString()), "sp_RepGerencialV1", "", "", "", "", "", "", 1, 0, 0, 0, 0, 0, 
                        Session["Conectar"].ToString());

                    _dtbpagoges2 = _dts.Tables[0];

                    if (_dts.Tables[0].Rows.Count == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("No Existen Datos Para Mostrar..!", this, "W", "C");
                        return;
                    }

                    ReportParameter[] parameters = new ReportParameter[8];
                    parameters[0] = new ReportParameter("Cedente", DdlCedente.SelectedItem.ToString());
                    parameters[1] = new ReportParameter("Catalogo", DdlCatalogo.SelectedItem.ToString());
                    parameters[2] = new ReportParameter("FechaD", TxtFechaIni.Text.Trim());
                    parameters[3] = new ReportParameter("FechaH", TxtFechaFin.Text.Trim());
                    parameters[4] = new ReportParameter("FechaH", TxtFechaFin.Text.Trim());
                    parameters[5] = new ReportParameter("Operador1", ViewState["Gestor1"].ToString());
                    parameters[6] = new ReportParameter("Operador2", ViewState["Gestor2"].ToString());
                    parameters[7] = new ReportParameter("Logo", _imagepath);

                    RptViewDatos.LocalReport.ReportPath = Server.MapPath("../Reports/Rpt_GerencialV3.rdlc");
                    RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsOpe1Efec", _dtbefecges1));
                    RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsOpe2Efec", _dtbefecges2));
                    RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsOpe1Pago", _dtbpagoges1));
                    RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsOpe2Pago", _dtbpagoges2));

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

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}