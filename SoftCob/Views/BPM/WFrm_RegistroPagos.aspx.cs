namespace SoftCob.Views.BPM
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_RegistroPagos : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtbpagos = new DataTable();
        TextBox _fechacuota = new TextBox();
        decimal _porcent = 0.00M, _pago = 0.00M, _exigible = 0.00M, _descuento = 0.00M, _abono = 0.00M,
            _saldo = 0.00M, _cuota = 0.00M, _total = 0.00M, _totalpago = 0.00M;
        int _contar = 0, _maxcodigo = 0, _numpagos = 0, _sumdias = 0, _pacocodigo = 0;
        string _fechaactual = "", _codigo = "", _ext = "", _nombre = "", _contentType = "", _ruta = "", _path = "",
            _totalname = "", _archivo = "", _mensaje = "";
        string[] _formatos;
        DateTime _dtmfechacuota, _dtmfecha;
        DataRow _fileagre, _result;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");

                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
                Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
                TxtPorcentaje.Attributes.Add("onchange", "ValidarDecimales();");
                TxtAbono.Attributes.Add("onchange", "ValidarDecimales1();");
                //Page.Form.Attributes.Add("enctype", "multipart/form-data");
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.ImgExportar);

                if (!IsPostBack)
                {
                    ViewState["CodigoCITA"] = Request["CodigoCITA"];
                    ViewState["CodigoPERS"] = Request["CodigoPERS"];
                    ViewState["CodigoCLDE"] = Request["CodigoCLDE"];
                    ViewState["CodigoGEST"] = Request["CodigoGEST"];
                    ViewState["NumDocumento"] = Request["NumDocumento"];
                    ViewState["Documento"] = Request["Documento"];
                    ViewState["Nombres"] = Request["Nombres"];
                    Lbltitulo.Text = "Registro Pagos << CALCULO TABLA DE AMORTIZACION >>";
                    TxtFechaPago.Text = DateTime.Now.ToString("yyyy-MM-dd");

                    _dtbpagos.Columns.Add("Codigo");
                    _dtbpagos.Columns.Add("FechaPago");
                    _dtbpagos.Columns.Add("TipoPago");
                    _dtbpagos.Columns.Add("ValorPago");
                    ViewState["TablaPagos"] = _dtbpagos;

                    ViewState["DiasPagoTotal"] = "10";
                    ViewState["DiasPagoDiferido"] = "30";
                    ViewState["MaxCodigo"] = "0";

                    FunCargarMantenimiento();
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
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(260, int.Parse(ViewState["CodigoCLDE"].ToString()),
                    int.Parse(ViewState["CodigoCITA"].ToString()), 0, "", "", "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    LblExigible.Text = _dts.Tables[0].Rows[0]["ValorCita"].ToString();
                    LblPago.Text = _dts.Tables[0].Rows[0]["ValorCita"].ToString();
                    ViewState["Exigible"] = _dts.Tables[0].Rows[0]["Exigible"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlPagos.DataSource = new ControllerDAO().FunGetParametroDetalle("VALIDACION PAGOS", "--Seleccione Opcion--", "S");
                    DdlPagos.DataTextField = "Descripcion";
                    DdlPagos.DataValueField = "Codigo";
                    DdlPagos.DataBind();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 1, 0, 0, "DIAS PAGO TOTAL", "VALIDACION PAGOS",
                        "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                        ViewState["DiasPagoTotal"] = _dts.Tables[0].Rows[0]["Valor"].ToString();

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(132, 1, 0, 0, "DIAS PAGO DIFERIDO", "VALIDACION PAGOS",
                        "", Session["Conectar"].ToString());

                    if (_dts.Tables[0].Rows.Count > 0)
                        ViewState["DiasPagoDiferido"] = _dts.Tables[0].Rows[0]["Valor"].ToString();

                    break;
            }
        }

        private string Funguardararchivo(HttpPostedFile file)
        {
            // Se carga la ruta física de la carpeta temp del sitio
            _ruta = Server.MapPath("~/citacion/" + ViewState["NumDocumento"].ToString());

            if (file.FileName.ToString().Length > 80)
            {
                new FuncionesDAO().FunShowJSMessage("Nombre del archivo debe contener hasta 80 caracteres..!", this, "W", "R");
                return _ruta = "";
            }

            // Si el directorio no existe, crearlo
            if (!Directory.Exists(_ruta))
                Directory.CreateDirectory(_ruta);

            _archivo = String.Format("{0}\\{1}", _ruta, file.FileName);

            // Verificar que el archivo no exista
            if (File.Exists(_archivo))
            {
                new FuncionesDAO().FunShowJSMessage(string.Format("Ya existe una imagen con nombre\"{0}\".", file.FileName), 
                    this, "W", "R");
                _ruta = "";
            }
            else file.SaveAs(_archivo);
            return _ruta;
        }
        #endregion

        #region Botones y Eventos
        protected void ImgVisto_Click(object sender, ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtPorcentaje.Text.Trim()) || TxtPorcentaje.Text == "0" || TxtPorcentaje.Text.Trim() == "0.00"
                || TxtPorcentaje.Text.Trim() == "0.0" || TxtPorcentaje.Text.Trim() == "." || TxtPorcentaje.Text.Trim() == "0.")
            {
                new FuncionesDAO().FunShowJSMessage("Valor Porcentaje en Cero o Incorrecto..!", this, "W", "R");
                return;
            }

            _porcent = decimal.Parse(TxtPorcentaje.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            _exigible = decimal.Parse(LblExigible.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            _descuento = Math.Round(_exigible * (_porcent / 100), 2);
            LblDescuento.Text = _descuento.ToString().Replace(",", ".");

            _pago = _exigible - _descuento;
            LblPago.Text = _pago.ToString().Replace(",", ".");

            RdbPagoTotal.Checked = false;
            RdbPagoDiferido.Checked = false;
            RdbSinAcuerdo.Checked = false;
            TxtAbono.Text = "0.00";
            TxtMeses.Text = "0";
            TrTiempo.Visible = false;
            TrTablaPagos.Visible = true;
            TrTiempo.Visible = false;
            TrFila1.Visible = false;
            TrFila2.Visible = false;
            TrArchivo.Visible = false;

            GrdvPagos.DataSource = null;
            GrdvPagos.DataBind();

        }

        protected void RdbPagoTotal_CheckedChanged(object sender, EventArgs e)
        {
            RdbPagoDiferido.Checked = false;
            RdbSinAcuerdo.Checked = false;
            RdbSinAcuerdo.Checked = false;
            TxtMeses.Text = "0";
            TxtAbono.Text = "0.00";
            TrTablaPagos.Visible = true;
            TrTiempo.Visible = false;
            TrFila1.Visible = false;
            TrFila2.Visible = false;
            TrArchivo.Visible = true;
            DdlPagos.Visible = true;
            LblMeses.Visible = false;
            TxtMeses.Visible = false;
            ImgModificar.Visible = false;
            DdlPagos.SelectedValue = "0";

            GrdvPagos.DataSource = null;
            GrdvPagos.DataBind();

            if (RdbPagoTotal.Checked) TrTiempo.Visible = true;
        }

        protected void DdlPagos_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtMeses.Text = "0";
            TxtAbono.Text = "0.00";
            _dtbpagos.Columns.Add("Codigo");
            _dtbpagos.Columns.Add("FechaPago");
            _dtbpagos.Columns.Add("TipoPago");
            _dtbpagos.Columns.Add("ValorPago");
            ViewState["TablaPagos"] = _dtbpagos;
            _dtbpagos.Clear();

            GrdvPagos.DataSource = null;
            GrdvPagos.DataBind();
        }

        protected void RdbPagoDiferido_CheckedChanged(object sender, EventArgs e)
        {
            RdbPagoTotal.Checked = false;
            RdbSinAcuerdo.Checked = false;
            TrTablaPagos.Visible = true;
            TrFila1.Visible = false;
            TrFila2.Visible = false;
            TrArchivo.Visible = true;
            TxtMeses.Text = "0";
            TxtAbono.Text = "0.00";
            //TxtPorcentaje.Text = "0";
            //LblDescuento.Text = "0.00";
            //LblPago.Text = LblExigible.Text;
            TxtAbono.Enabled = true;
            DdlPagos.Visible = false;
            DdlPagos.SelectedValue = "0";
            LblMeses.Visible = true;
            TxtMeses.Visible = true;
            ImgModificar.Visible = false;

            GrdvPagos.DataSource = null;
            GrdvPagos.DataBind();

            if (RdbPagoDiferido.Checked) TrTiempo.Visible = true;
        }

        protected void RdbSinAcuerdo_CheckedChanged(object sender, EventArgs e)
        {
            RdbPagoDiferido.Checked = false;
            RdbPagoTotal.Checked = false;
            TxtMeses.Text = "0";
            TxtAbono.Text = "0.00";
            TxtPorcentaje.Text = "0";
            LblDescuento.Text = "0.00";
            LblPago.Text = LblExigible.Text;
            TrTiempo.Visible = false;
            TrTablaPagos.Visible = false;
            TrArchivo.Visible = false;
            ImgModificar.Visible = false;

            _dtbpagos.Columns.Add("Codigo");
            _dtbpagos.Columns.Add("FechaPago");
            _dtbpagos.Columns.Add("TipoPago");
            _dtbpagos.Columns.Add("ValorPago");
            ViewState["TablaPagos"] = _dtbpagos;
            _dtbpagos.Clear();

            GrdvPagos.DataSource = null;
            GrdvPagos.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbpagos = (DataTable)ViewState["TablaPagos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtbpagos, "Datos");
                    string FileName = "Tabla_Amortizacion" + ViewState["NumDocumento"].ToString() + "-" +
                        DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgProcesar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _fechaactual = _dtmfecha.ToString("yyyy-MM-dd");

                TrFila1.Visible = false;
                TrFila2.Visible = false;
                TrArchivo.Visible = false;

                if (DateTime.ParseExact(TxtFechaPago.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture) <
                    DateTime.ParseExact(_fechaactual, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha Inicio no puede ser menor a la Fecha Actual..!", this, "W", "R");
                    return;
                }

                _pago = decimal.Parse(LblPago.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

                if (string.IsNullOrEmpty(TxtAbono.Text.Trim()) || TxtAbono.Text.Trim() == "." ||
                    TxtPorcentaje.Text.Trim() == "0.")
                {
                    new FuncionesDAO().FunShowJSMessage("Valor Abono Incorrecto..!", this, "W", "R");
                    TxtAbono.Text = "0.00";
                    return;
                }

                _abono = decimal.Parse(TxtAbono.Text.Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

                if (_abono > _pago)
                {
                    new FuncionesDAO().FunShowJSMessage("Valor del Abono Mayor al Valor Pago..!", this, "W", "R");
                    TxtAbono.Text = "0.00";
                    return;
                }

                if (_abono == _pago)
                {
                    new FuncionesDAO().FunShowJSMessage("Esta abonando el mismo valor del Pago..!", this, "W", "R");
                    return;
                }

                _dtbpagos = (DataTable)ViewState["TablaPagos"];
                _dtbpagos.Clear();
                GrdvPagos.DataSource = null;
                GrdvPagos.DataBind();

                if (_abono > 0)
                {
                    _fileagre = _dtbpagos.NewRow();
                    _fileagre["Codigo"] = _maxcodigo + 1;
                    _fileagre["FechaPago"] = TxtFechaPago.Text.Trim();
                    _fileagre["TipoPago"] = "Abono";
                    _fileagre["ValorPago"] = _abono.ToString();
                    _dtbpagos.Rows.Add(_fileagre);
                    _saldo = _pago - _abono;
                    ViewState["MaxCodigo"] = _maxcodigo + 1;
                }

                _saldo = _pago - _abono;

                ViewState["FechaCuota"] = TxtFechaPago.Text.Trim();

                if (RdbPagoTotal.Checked)
                {
                    if (DdlPagos.SelectedValue == "0")
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione Forma Pago Total..!", this, "W", "R");
                        return;
                    }

                    _numpagos = int.Parse(DdlPagos.SelectedValue);
                    _sumdias = int.Parse(ViewState["DiasPagoTotal"].ToString());

                }

                if (RdbPagoDiferido.Checked)
                {
                    if (string.IsNullOrEmpty(TxtMeses.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese Tiempo (Meses) Diferido..!", this, "W", "R");
                        return;
                    }

                    if (int.Parse(TxtMeses.Text.Trim()) >= 0 && int.Parse(TxtMeses.Text.Trim()) <= 1)
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese Tiempo (Meses) Diferido..!", this, "W", "R");
                        return;
                    }

                    _numpagos = int.Parse(TxtMeses.Text.Trim());
                    _sumdias = int.Parse(ViewState["DiasPagoDiferido"].ToString());

                }

                _cuota = Math.Round(_saldo / _numpagos, 2);
                _maxcodigo = int.Parse(ViewState["MaxCodigo"].ToString());

                for (int i = 0; i < _numpagos; i++)
                {
                    _contar++;
                    _maxcodigo++;

                    _dtmfechacuota = DateTime.ParseExact(ViewState["FechaCuota"].ToString(), "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                    if (_abono > 0 && _contar == 1)
                        _dtmfechacuota = _dtmfechacuota.AddDays(_sumdias);

                    ViewState["FechaCuota"] = _dtmfechacuota.ToString("yyyy-MM-dd");

                    if (_contar == _numpagos)
                    {
                        _cuota = _pago - (_total + _abono);
                    }

                    _total += _cuota;

                    _fileagre = _dtbpagos.NewRow();
                    _fileagre["Codigo"] = _maxcodigo;
                    _fileagre["FechaPago"] = ViewState["FechaCuota"].ToString();
                    _fileagre["TipoPago"] = "Cuota";
                    _fileagre["ValorPago"] = _cuota.ToString();
                    _dtbpagos.Rows.Add(_fileagre);

                    _dtmfechacuota = _dtmfechacuota.AddDays(_sumdias);
                    ViewState["FechaCuota"] = _dtmfechacuota.ToString("yyyy-MM-dd");
                }

                ViewState["TablaPagos"] = _dtbpagos;
                GrdvPagos.DataSource = _dtbpagos;
                GrdvPagos.DataBind();
                TrFila1.Visible = true;
                TrFila2.Visible = true;
                TrArchivo.Visible = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _totalpago += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ValorPago"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = "TOTAL:";
                    e.Row.Cells[2].Text = _totalpago.ToString();
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                    e.Row.Font.Size = 11;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvPagos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvPagos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = GrdvPagos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();

                _dtbpagos = (DataTable)ViewState["TablaPagos"];
                _result = _dtbpagos.Select("Codigo=" + _codigo).FirstOrDefault();

                ViewState["Codigo"] = _codigo;
                TxtFechaPago.Text = _result["FechaPago"].ToString();

                ImgModificar.Visible = true;


            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtbpagos = (DataTable)ViewState["TablaPagos"];

                _result = _dtbpagos.Select("FechaPago='" + TxtFechaPago.Text.Trim() + "'").FirstOrDefault();

                if (_result != null)
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha Cuota ya Existe..!", this, "W", "R");
                    return;
                }

                _result = _dtbpagos.Select("Codigo=" + ViewState["Codigo"].ToString()).FirstOrDefault();

                _result["FechaPago"] = TxtFechaPago.Text.Trim();
                _dtbpagos.AcceptChanges();
                ViewState["TablaPagos"] = _dtbpagos;
                GrdvPagos.DataSource = _dtbpagos;
                GrdvPagos.DataBind();
                ImgModificar.Visible = false;

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RdbPagoTotal.Checked && !RdbPagoDiferido.Checked && !RdbSinAcuerdo.Checked)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Opcion de Registro..!", this, "W", "R");
                    return;
                }

                if (RdbPagoTotal.Checked || RdbPagoDiferido.Checked)
                {
                    if (FileUpload1.HasFile)
                    {
                        _ext = FileUpload1.PostedFile.FileName;
                        _ext = _ext.Substring(_ext.LastIndexOf(".") + 1).ToLower();
                        _formatos = new string[] { "jpg", "jpeg", "bmp", "png", "gif", "pdf" };

                        if (Array.IndexOf(_formatos, _ext) < 0)
                        {
                            new FuncionesDAO().FunShowJSMessage("Formato de imagen inválido..!", this, "W", "R");
                            return;
                        }

                        _nombre = FileUpload1.FileName.Substring(0, FileUpload1.FileName.LastIndexOf("."));
                        _contentType = FileUpload1.PostedFile.ContentType;
                        _ruta = Funguardararchivo(FileUpload1.PostedFile);
                        _path = "~/citacion/" + ViewState["NumDocumento"].ToString();

                        _totalname = _path + " " + _nombre + " " + _ext;
                    }
                    else
                    {
                        new FuncionesDAO().FunShowJSMessage("Seleccione un archivo..!", this, "W", "R");
                        return;
                    }
                }

                if (RdbPagoTotal.Checked || RdbPagoDiferido.Checked)
                {
                    _dtbpagos = (DataTable)ViewState["TablaPagos"];

                    if (_dtbpagos.Rows.Count == 0)
                    {
                        new FuncionesDAO().FunShowJSMessage("Realice ProcesO para Generar Tabla de Pagos..!", this, "W", "R");
                        return;
                    }

                    _dts = new ConsultaDatosDAO().FunInsertRegistroPagos(0, 0, int.Parse(ViewState["CodigoCITA"].ToString()),
                        int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()),
                        ViewState["NumDocumento"].ToString(), ViewState["Exigible"].ToString(), LblDescuento.Text, TxtPorcentaje.Text.Trim(),
                        LblPago.Text, LblExigible.Text, RdbPagoTotal.Checked ? "TOTAL" : "DIFER", TxtFechaPago.Text.Trim(),
                        int.Parse(TxtMeses.Text.Trim()), TxtAbono.Text.Trim(), ViewState["Documento"].ToString(),
                        ViewState["Nombres"].ToString(), "", "", "", _path, _nombre, _ext, _contentType,
                        TxtObservacion.Text.Trim().ToUpper(), "", "", "", "", "", 0, 0, 0, 0, 0,
                        int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        Session["Conectar"].ToString());

                    _pacocodigo = int.Parse(_dts.Tables[0].Rows[0]["Codigo"].ToString());

                    foreach (DataRow _drfila in _dtbpagos.Rows)
                    {
                        _dts = new ConsultaDatosDAO().FunInsertRegistroPagos(1, _pacocodigo, int.Parse(ViewState["CodigoCITA"].ToString()),
                            int.Parse(ViewState["CodigoPERS"].ToString()), int.Parse(ViewState["CodigoCLDE"].ToString()),
                            ViewState["NumDocumento"].ToString(), LblExigible.Text, LblDescuento.Text, TxtPorcentaje.Text.Trim(),
                            LblPago.Text, "0", "", "", 0, "", "", "", _drfila["FechaPago"].ToString(),
                            _drfila["ValorPago"].ToString().Replace(",", "."), _drfila["TipoPago"].ToString(), "", "",
                            "", "", "", "", "", "", "", "", 0, 0, 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()),
                            Session["MachineName"].ToString(), Session["Conectar"].ToString());
                    }
                    _mensaje = "Convenio Registrado..!";
                }
                else
                {
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(244, int.Parse(ViewState["CodigoCITA"].ToString()),
                        int.Parse(ViewState["CodigoCLDE"].ToString()), 0, TxtObservacion.Text.Trim().ToUpper(), "CSV", "",
                        Session["Conectar"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(261, int.Parse(ViewState["CodigoCITA"].ToString()),
                        int.Parse(Session["usuCodigo"].ToString()), 0, TxtObservacion.Text.Trim().ToUpper(), "CSV",
                        Session["MachineName"].ToString(), Session["Conectar"].ToString());

                    _mensaje = "Cliente sin Convenio..!";
                }

                Response.Redirect("WFrm_RegistroCitacionAdmin.aspx?MensajeRetornado=" + _mensaje, true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_RegistroCitacionAdmin.aspx", true);
        }
        #endregion
    }
}