namespace SoftCob.Views.ReportesGerenciales
{
    using ControllerSoftCob;
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Data;
    using System.Linq;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteGerencialV3 : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbresult = new DataTable();
        DataTable _dtbgestor = new DataTable();
        DataTable _dtbdato1 = new DataTable();
        DataTable _dtbdato2 = new DataTable();
        ListItem _itemc = new ListItem();
        ListItem _itemg = new ListItem();
        DataRow _filagre, _result;
        string _codigo = "", _imagepath = "", _report = "";
        int _row = 0, _opcion = 0, _opcionreport = 0, _codigoid = 0;
        DateTime _fechaini, _fechafin;
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
                    ViewState["Dato1"] = "0";
                    ViewState["Dato2"] = "0";
                    ViewState["Boton"] = "0";
                    TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    Lbltitulo.Text = "Reporte Gerencial Compromiso de Pago << BUSINESS INTELLIGENCE >>";

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
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString()); DdlGestor.DataTextField = "Descripcion";
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

        protected void RdbOpciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (RdbOpciones.SelectedValue)
            {
                case "1":
                    ChkPorYear.Enabled = false;
                    ChkPorMes.Enabled = false;
                    ChkPorYear.Checked = false;
                    ChkPorMes.Checked = false;
                    break;
                default:
                    ChkPorYear.Enabled = true;
                    ChkPorMes.Enabled = true;
                    break;
            }
        }

        protected void ChkPorGestor_CheckedChanged(object sender, EventArgs e)
        {
            DdlGestor.Enabled = false;
            DdlGestor.SelectedValue = "0";
            GrdvGestores.DataSource = null;
            GrdvGestores.DataBind();
            _dtbgestor = (DataTable)ViewState["Gestores"];
            _dtbgestor.Clear();
            ViewState["Gestores"] = _dtbgestor;
            if (ChkPorGestor.Checked)
            {
                DdlGestor.Enabled = true;
            }

        }
        protected void DdlGestor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DdlGestor.SelectedValue != "0")
                {
                    _dtbgestor = (DataTable)ViewState["Gestores"];

                    if (GrdvGestores.Rows.Count < 2)
                    {
                        _filagre = _dtbgestor.NewRow();
                        _filagre["Codigo"] = DdlGestor.SelectedValue;
                        _filagre["Gestor"] = DdlGestor.SelectedItem.ToString();
                        _dtbgestor.Rows.Add(_filagre);
                        GrdvGestores.DataSource = _dtbgestor;
                        GrdvGestores.DataBind();
                    }
                    //DdlGestor.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkComparativas_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void ChkPorYear_CheckedChanged(object sender, EventArgs e)
        {
            if (!ChkPorMes.Checked) ChkPorYear.Checked = true;
            else ChkPorMes.Checked = false;

        }

        protected void ChkPorMes_CheckedChanged(object sender, EventArgs e)
        {
            if (!ChkPorYear.Checked) ChkPorMes.Checked = true;
            else ChkPorYear.Checked = false;
        }

        protected void ImgDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvGestores.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString();
                _dtbgestor = (DataTable)ViewState["Gestores"];
                _result = _dtbgestor.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                _dtbgestor.AcceptChanges();
                ViewState["Gestores"] = _dtbgestor;
                GrdvGestores.DataSource = _dtbgestor;
                GrdvGestores.DataBind();
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
                        new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
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

                    if (DateTime.ParseExact(TxtFechaIni.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                    {
                        new FuncionesDAO().FunShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this, "W", "C");
                        return;
                    }

                    _imagepath = new Uri(Server.MapPath(ViewState["LogoReporte"].ToString())).AbsoluteUri;

                    if (RdbOpciones.SelectedValue == "0")
                    {
                        if (!ChkComparativas.Checked)
                        {
                            if (ChkPorGestor.Checked)
                            {
                                if (DdlGestor.SelectedValue == "0")
                                {
                                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "C");
                                    return;
                                }
                                else ViewState["Dato1"] = DdlGestor.SelectedItem.ToString();
                            }

                            if (!ChkPorGestor.Checked && ChkPorYear.Checked)
                            {
                                _opcion = 0;
                                _report = "../Reports/Rpt_GerencialV4.rdlc";
                            }

                            if (ChkPorGestor.Checked && ChkPorYear.Checked)
                            {
                                _opcion = 1;
                                _report = "../Reports/Rpt_GerencialV4.rdlc";
                            }

                            if (!ChkPorGestor.Checked && ChkPorMes.Checked)
                            {
                                _opcion = 2;
                                _report = "../Reports/Rpt_GerencialV5.rdlc";
                            }

                            if (ChkPorGestor.Checked && ChkPorMes.Checked)
                            {
                                _opcion = 3;
                                _report = "../Reports/Rpt_GerencialV5.rdlc";
                            }

                            _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue), 
                                int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), 
                                int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", _opcion, 0, 0, 0, 0, 0, 
                                Session["Conectar"].ToString());

                            _dtbresult = _dts.Tables[0];

                            _opcionreport = 0;
                        }
                        else
                        {
                            _fechaini = DateTime.ParseExact(TxtFechaIni.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                            _fechafin = DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                            ViewState["Dato1"] = _fechaini.Year;
                            ViewState["Dato2"] = _fechafin.Year;

                            if (!ChkPorGestor.Checked && ChkPorYear.Checked)
                            {
                                if (_fechaini.Year == _fechafin.Year)
                                {
                                    new FuncionesDAO().FunShowJSMessage("Imposible Comparar Años Iguales..!", this);
                                    return;
                                }

                                _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                    int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), "",
                                    int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", 4, 0,
                                    0, 0, 0, 0, Session["Conectar"].ToString());

                                _dtbdato1 = _dts.Tables[0];

                                _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                    int.Parse(DdlCatalogo.SelectedValue), TxtFechaFin.Text.Trim(), "",
                                    int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", 4, 0,
                                    0, 0, 0, 0, Session["Conectar"].ToString());

                                _dtbdato2 = _dts.Tables[0];

                                _opcionreport = 1;
                                _report = "../Reports/Rpt_GerencialV6.rdlc";

                            }

                            if (!ChkPorGestor.Checked && ChkPorMes.Checked)
                            {
                                if (_fechaini.Year == _fechafin.Year)
                                {
                                    new FuncionesDAO().FunShowJSMessage("Imposible Comparar Años Iguales..!", this, "W", "C");
                                    return;
                                }

                                _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                    int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(),
                                    int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", 5, 0,
                                    0, 0, 0, 0, Session["Conectar"].ToString());

                                _dtbdato1 = _dts.Tables[0];

                                _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                    int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(),
                                    int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", 6, 0,
                                    0, 0, 0, 0, Session["Conectar"].ToString());

                                _dtbdato2 = _dts.Tables[0];

                                _opcionreport = 1;
                                _report = "../Reports/Rpt_GerencialV7.rdlc";
                            }

                            if (ChkPorGestor.Checked)
                            {
                                _dtbgestor = (DataTable)ViewState["Gestores"];

                                if (_dtbgestor.Rows.Count < 2)
                                {
                                    new FuncionesDAO().FunShowJSMessage("Debe Seleccionar 2 Gestores..!", this, "W", "C");
                                    return;
                                }

                                _row = 1;
                                foreach (DataRow _fila in _dtbgestor.Rows)
                                {
                                    switch (_row)
                                    {
                                        case 1:
                                            ViewState["Codigo1"] = _fila["Codigo"];
                                            ViewState["Dato1"] = _fila["Gestor"];
                                            break;
                                        case 2:
                                            ViewState["Codigo2"] = _fila["Codigo"];
                                            ViewState["Dato2"] = _fila["Gestor"];
                                            break;
                                    }
                                    _row++;
                                }

                                if (ChkPorYear.Checked)
                                {
                                    if (_fechaini.Year == _fechafin.Year)
                                    {
                                        new FuncionesDAO().FunShowJSMessage("Imposible Comparar Años Iguales..!", this, "W", "C");
                                        return;
                                    }

                                    _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                        int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(),
                                        TxtFechaFin.Text.Trim(), int.Parse(ViewState["Codigo1"].ToString()),
                                        "sp_RepGerencialV1", "", "", "", "", "", "", 1, 0, 0, 0, 0, 0, ViewState
                                        ["Conectar"].ToString());

                                    _dtbdato1 = _dts.Tables[0];

                                    _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                        int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(),
                                        TxtFechaFin.Text.Trim(), int.Parse(ViewState["Codigo2"].ToString()),
                                        "sp_RepGerencialV1", "", "", "", "", "", "", 1, 0, 0, 0, 0, 0, ViewState
                                        ["Conectar"].ToString());

                                    _dtbdato2 = _dts.Tables[0];

                                    _opcionreport = 1;
                                    _report = "../Reports/Rpt_GerencialV6.rdlc";
                                }

                                if (ChkPorMes.Checked)
                                {
                                    _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                        int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(),
                                        TxtFechaFin.Text.Trim(), int.Parse(ViewState["Codigo1"].ToString()),
                                        "sp_RepGerencialV1", "", "", "", "", "", "", 7, 0, 0, 0, 0, 0, ViewState
                                        ["Conectar"].ToString());

                                    _dtbdato1 = _dts.Tables[0];

                                    _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                        int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(),
                                        TxtFechaFin.Text.Trim(), int.Parse(ViewState["Codigo2"].ToString()),
                                        "sp_RepGerencialV1", "", "", "", "", "", "", 7, 0, 0, 0, 0, 0, ViewState
                                        ["Conectar"].ToString());

                                    _dtbdato2 = _dts.Tables[0];

                                    _opcionreport = 1;
                                    _report = "../Reports/Rpt_GerencialV7.rdlc";
                                }
                            }
                        }
                    }

                    if (RdbOpciones.SelectedValue == "1")
                    {
                        if (!ChkComparativas.Checked)
                        {
                            if (ChkPorGestor.Checked)
                            {
                                if (DdlGestor.SelectedValue == "0")
                                {
                                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "C");
                                    return;
                                }
                                else ViewState["Dato1"] = DdlGestor.SelectedItem.ToString();
                            }

                            _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", DdlGestor.SelectedValue == "0" ? 8 : 9, 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                            _dtbresult = _dts.Tables[0];

                            _opcionreport = 2;
                            _report = "../Reports/Rpt_GerencialV8.rdlc";
                        }
                        else
                        {
                            _dtbgestor = (DataTable)ViewState["Gestores"];

                            if (_dtbgestor.Rows.Count < 2)
                            {
                                new FuncionesDAO().FunShowJSMessage("Debe Seleccionar 2 Gestores..!", this, "W", "C");
                                return;
                            }

                            _row = 1;
                            foreach (DataRow _fila in _dtbgestor.Rows)
                            {
                                switch (_row)
                                {
                                    case 1:
                                        ViewState["Codigo1"] = _fila["Codigo"];
                                        ViewState["Dato1"] = _fila["Gestor"];
                                        break;
                                    case 2:
                                        ViewState["Codigo2"] = _fila["Codigo"];
                                        ViewState["Dato2"] = _fila["Gestor"];
                                        break;
                                }
                                _row++;
                            }

                            _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                 int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(ViewState["Codigo1"].ToString()), "sp_RepGerencialV1", "", "", "", "", "", "", 9, 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                            _dtbdato1 = _dts.Tables[0];

                            _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue),
                                 int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(ViewState["Codigo2"].ToString()), "sp_RepGerencialV1", "", "", "", "", "", "", 9, 0, 0, 0, 0, 0, Session["Conectar"].ToString());

                            _dtbdato2 = _dts.Tables[0];

                            _opcionreport = 1;
                            _report = "../Reports/Rpt_GerencialV9.rdlc";

                        }
                    }

                    if (RdbOpciones.SelectedValue == "2")
                    {
                        if (!ChkComparativas.Checked)
                        {
                            if (ChkPorGestor.Checked)
                            {
                                if (DdlGestor.SelectedValue == "0")
                                {
                                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this, "W", "C");
                                    return;
                                }
                                else ViewState["Dato1"] = DdlGestor.SelectedItem.ToString();
                            }

                            if (ChkPorYear.Checked)
                            {
                                _dts = new ConsultaDatosDAO().FunRepGerencialG(14, int.Parse(DdlCedente.SelectedValue),
                                    int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(),
                                    int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", 0, 0, 0,
                                    0, 0, 0, Session["Conectar"].ToString());

                                _dtbresult = _dts.Tables[0];

                                _opcionreport = 3;
                                _report = "../Reports/Rpt_GerencialV10.rdlc";
                            }
                        }
                    }

                    BtnProcesar.Text = "Consultar";
                    ViewState["Boton"] = "1";
                    TblOpciones.Visible = false;
                    TblFiltros.Visible = false;
                    TblReporte.Visible = true;

                    RptViewDatos.LocalReport.DataSources.Clear();
                    RptViewDatos.LocalReport.EnableExternalImages = true;
                    ReportParameter[] parameters = new ReportParameter[8];
                    parameters[0] = new ReportParameter("Cedente", DdlCedente.SelectedItem.ToString());
                    parameters[1] = new ReportParameter("Catalogo", DdlCatalogo.SelectedItem.ToString());
                    parameters[2] = new ReportParameter("FechaD", TxtFechaIni.Text.Trim());
                    parameters[3] = new ReportParameter("FechaH", TxtFechaFin.Text.Trim());
                    parameters[4] = new ReportParameter("FechaH", TxtFechaFin.Text.Trim());
                    parameters[5] = new ReportParameter("Operador1", ViewState["Dato1"].ToString());
                    parameters[6] = new ReportParameter("Operador2", ViewState["Dato2"].ToString());
                    parameters[7] = new ReportParameter("Logo", _imagepath);

                    RptViewDatos.LocalReport.ReportPath = Server.MapPath(_report);

                    switch (_opcionreport)
                    {
                        case 0:
                            RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsAbonoYear", _dtbresult));
                            break;
                        case 1:
                            RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsDato1", _dtbdato1));
                            RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsDato2", _dtbdato2));
                            break;
                        case 2:
                            RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsCompPago", _dtbresult));
                            RptViewDatos.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubRerportEfecV8);
                            RptViewDatos.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportRespV8);
                            break;
                        case 3:
                            RptViewDatos.LocalReport.DataSources.Add(new ReportDataSource("DtsPagosReal", _dtbresult));
                            break;
                    }

                    RptViewDatos.LocalReport.SetParameters(parameters);
                    RptViewDatos.LocalReport.Refresh();
                }
                else
                {
                    ViewState["Boton"] = "0";
                    TblOpciones.Visible = true;
                    TblFiltros.Visible = true;
                    BtnProcesar.Text = "Procesar";
                    TblReporte.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void SubRerportEfecV8(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                _codigoid = int.Parse(e.Parameters["CodigoID"].Values[0].ToString());

                _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", DdlGestor.SelectedValue == "0" ? 10 : 11, _codigoid, 0, 0, 0, 0, Session["Conectar"].ToString());

                _dtbresult = _dts.Tables[0];

                ReportDataSource dts = new ReportDataSource("DtsEfectoV8", _dtbresult);
                e.DataSources.Add(dts);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void SubReportRespV8(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                _codigoid = int.Parse(e.Parameters["CodigoID"].Values[0].ToString());

                _dts = new ConsultaDatosDAO().FunRepGerencialG(13, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text.Trim(), TxtFechaFin.Text.Trim(), int.Parse(DdlGestor.SelectedValue), "sp_RepGerencialV1", "", "", "", "", "", "", DdlGestor.SelectedValue == "0" ? 12 : 13, _codigoid, 0, 0, 0, 0, Session["Conectar"].ToString());

                _dtbresult = _dts.Tables[0];

                ReportDataSource dts = new ReportDataSource("DtsRespV8", _dtbresult);
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