namespace SoftCob.Views.CarteraPropia
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Threading;
    public partial class WFrm_PagoCartera : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        decimal _totalDeuda = 0.00M, _totalSaldo = 0.00M, _saldo = 0.00M, _pago = 0.00M, _totalPago = 0.00M,
                _valor = 0.00M;
        string _conexion = ConfigurationManager.AppSettings["SqlConn"];
        string _mensaje = "", _numdocumento = "", _operacion = "", _documento = "", _valorpago = "", _fechaactual = "",
            _rangodias = "";
        bool _concabecera = false, _continuar = true;
        ImageButton _imgrev = new ImageButton();
        ListItem _itemc = new ListItem();
        int _tipocliente = 0, _tipooperacion = 0, _efectivo = 0, _contar = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            TxtValor.Attributes.Add("onchange", "ValidarDecimales();");

            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                TxtFechaPago.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Pagos CARTERA";
                FunCascadaCombos(0);
                FunCascadaCombos(2);
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarDatos()
        {
            try
            {
                pnlPagos.Visible = true;
                lblTotalPago.InnerText = "";

                _dts = new PagoCarteraDAO().FunGetPagoCartera(4, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(ViewState["CodCatalogo"].ToString()), ViewState["identificacion"].ToString(),
                    ViewState["operacion"].ToString(), "", "", "0", "", "", "", "", 0, 0, 0, 0, "", _conexion);

                if (_dts != null)
                {
                    if (_dts.Tables[0].Rows.Count > 0) lblEstadoOperacion.InnerText = _dts.Tables[0].Rows[0]["Estado"].ToString();
                    else lblEstadoOperacion.InnerText = "ABONANDO";

                    lblEstado.InnerText = _dts.Tables[1].Rows[0]["Estado"].ToString();
                }

                _dts = new PagoCarteraDAO().FunGetPagoCartera(5, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(ViewState["CodCatalogo"].ToString()), ViewState["identificacion"].ToString(),
                    ViewState["operacion"].ToString(), "", "", "0", "", "", "", "", 0, 0, 0, 0, "", _conexion);

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    _dtb = _dts.Tables[0];
                    _totalPago = decimal.Parse(_dtb.Compute("Sum(SumPago)", "").ToString());
                    Session["totalPago"] = _totalPago;
                    lblTotalPago.InnerText = "$" + string.Format("{0:n}", _totalPago);
                }
                else Session["totalPago"] = "0";

                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();

                if (lblEstado.InnerText == "IMPAGA") ImgAgregar.Enabled = true;
                else ImgAgregar.Enabled = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void FunCargarDeudas()
        {
            _dts = new PagoCarteraDAO().FunGetPagoCartera(1, int.Parse(DdlCedente.SelectedValue),
                int.Parse(ViewState["CodCatalogo"].ToString()), lblIdentificacion.InnerText, "", "", "", "0", "", "", "", "",
                0, 0, 0, 0, "", _conexion);

            if (_dts != null)
            {
                ViewState["saldo"] = _dts.Tables[0].Rows[0]["SumSaldo"].ToString();

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    _dtb = _dts.Tables[0];
                    _totalDeuda = decimal.Parse(_dtb.Compute("Sum(SumDeuda)", "").ToString());
                    _totalSaldo = decimal.Parse(_dtb.Compute("Sum(SumSaldo)", "").ToString());
                    lblTotalDeuda.InnerText = "$" + string.Format("{0:n}", _totalDeuda);
                    lblTotalSaldo.InnerText = "$" + string.Format("{0:n}", _totalSaldo);
                }

                GrdvDeudas.DataSource = _dts;
                GrdvDeudas.DataBind();
            }
        }

        protected void FunLimpiarCampos()
        {
            DdlBuscarPor.SelectedIndex = 0;
            DdlTipoPago.SelectedIndex = 0;
            TxtBuscarPor.Text = "";
            TxtValor.Text = "";
            TxtDocumento.Text = "";
            TxtFechaPago.Text = DateTime.Now.ToString("MM/dd/yyyy");
            pnlPagos.Visible = false;
            ImgAgregar.Enabled = false;
        }

        protected void FunCascadaCombos(int tipo)
        {
            switch (tipo)
            {
                case 0:
                    DdlCedente.DataSource = new CedenteDAO().FunGetCedentes();
                    DdlCedente.DataTextField = "Descripcion";
                    DdlCedente.DataValueField = "Codigo";
                    DdlCedente.DataBind();

                    _itemc.Text = "--Seleccione Catálago/Producto--";
                    _itemc.Value = "0";
                    DdlCatalogo.Items.Add(_itemc);
                    break;
                case 1:
                    break;
                case 2:
                    DdlTipoPago.DataSource = new ControllerDAO().FunGetParametroDetalle("TIPO PAGO", "--Seleccione Tipo--", "I");
                    DdlTipoPago.DataTextField = "Descripcion";
                    DdlTipoPago.DataValueField = "Codigo";
                    DdlTipoPago.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnBuscar_Click(object sender, EventArgs e)
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
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo/Producto..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtBuscarPor.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Dato de consulta..!", this, "W", "C");
                    return;
                }

                if (DdlBuscarPor.SelectedValue == "I") _tipocliente = 0;
                else _tipocliente = 2;

                ImgAgregar.Enabled = false;
                Lblerror.Text = "";
                lblCliente.InnerText = "";
                lblIdentificacion.InnerText = "";
                lblEstado.InnerText = "";
                lblEstadoOperacion.InnerText = "";
                lblTotalDeuda.InnerText = "";
                lblTotalPago.InnerText = "";
                lblTotalSaldo.InnerText = "";
                GrdvDeudas.DataSource = null;
                GrdvDeudas.DataBind();
                GrdvDatos.DataSource = null;
                GrdvDatos.DataBind();

                ViewState["tipoCliente"] = _tipocliente;
                ViewState["tipoOperacion"] = _tipooperacion;

                _dts = new PagoCarteraDAO().FunGetPagoCartera(_tipocliente, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(ViewState["CodCatalogo"].ToString()), TxtBuscarPor.Text.Trim(), TxtBuscarPor.Text.Trim(),
                    "", "", "0", "", "", "", "", 0, 0, 0, 1, "", _conexion);

                if (_dts != null && _dts.Tables[0].Rows.Count > 0)
                {
                    lblCliente.InnerText = _dts.Tables[0].Rows[0]["Cliente"].ToString();
                    lblIdentificacion.InnerText = _dts.Tables[0].Rows[0]["Identificacion"].ToString();
                    FunCargarDeudas();
                }
                else new FuncionesDAO().FunShowJSMessage("No existen datos..!", this, "E", "C");
                FunLimpiarCampos();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!new FuncionesDAO().IsDate(TxtFechaPago.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this, "E", "C");
                    return;
                }

                DateTime dtime = DateTime.Now;
                _fechaactual = dtime.ToString("MM/dd/yyyy");

                if (DateTime.ParseExact(TxtFechaPago.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(_fechaactual, 
                    "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha de Pago no puede ser mayor a la Fecha actual..!", this, "E", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtValor.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese valor del Abono..!", this, "W", "C");
                    return;
                }

                if (string.IsNullOrEmpty(TxtDocumento.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese No. Documento..!", this, "W", "C");
                    return;
                }

                if (DdlTipoPago.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Pago..!", this, "W", "C");
                    return;
                }

                foreach (GridViewRow _row in GrdvDatos.Rows)
                {
                    _numdocumento = GrdvDatos.Rows[_row.RowIndex].Cells[3].Text.Trim();

                    if (TxtDocumento.Text.Trim() == _numdocumento)
                    {
                        _continuar = false;
                        break;
                    }
                }

                if (_continuar == false)
                {
                    new FuncionesDAO().FunShowJSMessage("No. de Documento ya está ingresado..!", this, "E", "C");
                    return;
                }

                decimal _valorPago = (decimal)Math.Round(decimal.Parse(TxtValor.Text.Replace(".", ",")), 2);

                _dts = new ConsultaDatosDAO().FunConsultaDatos(215, int.Parse(ViewState["CodCatalogo"].ToString()),
                    int.Parse(ViewState["CodigoGEST"].ToString()), int.Parse(ViewState["DiasMora"].ToString()),
                    "", "", "", _conexion);

                _rangodias = _dts.Tables[0].Rows[0]["DiasMora"].ToString();

                //_dts = new ConsultaDatosDAO().FunConsultaDatos(105, int.Parse(ViewState["CodCatalogo"].ToString()),
                //    int.Parse(ViewState["CodigoGEST"].ToString()), 0, "", TxtFechaPago.Text,
                //    ViewState["operacion"].ToString(), _conexion);

                //_contar = int.Parse(_dts.Tables[0].Rows[0]["Contar"].ToString());

                //if (_contar == 0)
                //{
                //    new FuncionesDAO().FunShowJSMessage("No Existe una confirmacion de pago, Solicite se realice la gestion " +
                //        "Para registrar el pago", this);
                //    return;
                //}

                _dts = new ConsultaDatosDAO().FunConsultaDatos(217, int.Parse(ViewState["CodCatalogo"].ToString()),
                    int.Parse(ViewState["CodigoGEST"].ToString()), 0, "", TxtFechaPago.Text,
                    ViewState["operacion"].ToString(), _conexion);

                _efectivo = int.Parse(_dts.Tables[0].Rows[0]["Efectivo"].ToString());

                SoftCob_PAGOSCARTERA pagocartera = new SoftCob_PAGOSCARTERA();
                {
                    pagocartera.pacp_cedecodigo = int.Parse(DdlCedente.SelectedValue);
                    pagocartera.pacp_cpcecodigo = int.Parse(ViewState["CodCatalogo"].ToString());
                    pagocartera.pacp_numerodocumento = lblIdentificacion.InnerText;
                    pagocartera.pacp_operacion = ViewState["operacion"].ToString();
                    pagocartera.pacp_documento = TxtDocumento.Text.Trim();
                    pagocartera.pacp_fechapago = DateTime.ParseExact(TxtFechaPago.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    pagocartera.pacp_valorpago = _valorPago;
                    pagocartera.pacp_gestor = int.Parse(ViewState["CodigoGEST"].ToString());
                    pagocartera.pacp_diasmora = int.Parse(ViewState["DiasMora"].ToString());
                    pagocartera.pacp_rangodias = _rangodias;
                    pagocartera.pacp_auxv1 = "";
                    pagocartera.pacp_auxv2 = "";
                    pagocartera.pacp_auxi1 = _efectivo;
                    pagocartera.pacp_auxi2 = 0;
                    pagocartera.pacp_fechacreacion = DateTime.Now;
                    pagocartera.pacp_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                    pagocartera.pacp_terminalcreacion = Session["MachineName"].ToString();
                }

                SoftCob_PAGOSCABECERA pagoCabecera = new SoftCob_PAGOSCABECERA();
                {
                    pagoCabecera.pcca_cedecodigo = int.Parse(DdlCedente.SelectedValue);
                    pagoCabecera.pcca_cpcecodigo = int.Parse(ViewState["CodCatalogo"].ToString());
                    pagoCabecera.pcca_numerodocumento = lblIdentificacion.InnerText;
                    pagoCabecera.pcca_operacion = ViewState["operacion"].ToString();
                    pagoCabecera.pcca_auxv1 = "";
                    pagoCabecera.pcca_auxv2 = "";
                    pagoCabecera.pcca_auxi1 = int.Parse(ViewState["CodigoGEST"].ToString());
                    pagoCabecera.pcca_auxi2 = 0;
                }

                _saldo = decimal.Parse(ViewState["saldo"].ToString());
                _pago = decimal.Parse(Session["totalPago"].ToString());
                _valor = Math.Round(decimal.Parse(TxtValor.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture), 2);
                pagocartera.pade_codigo = int.Parse(DdlTipoPago.SelectedValue);

                if ((_saldo - _valor) <= 0 || int.Parse(DdlTipoPago.SelectedValue) == 2 || int.Parse(DdlTipoPago.SelectedValue) == 3) //REGISTRO SI ES PAGO TOTAL
                {
                    pagoCabecera.pade_codigo = 2;
                    _dts = new PagoCarteraDAO().FunGetPagoCartera(6, int.Parse(DdlCedente.SelectedValue), int.Parse(ViewState["CodCatalogo"].ToString()), lblIdentificacion.InnerText, ViewState["operacion"].ToString(), TxtDocumento.Text.Trim(), TxtFechaPago.Text.Trim(), TxtValor.Text.Trim(), "", "", "", "", 0, 0, 0, int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _conexion);

                    if (_dts != null && _dts.Tables[0].Rows[0][0].ToString() == "OK") _concabecera = true;
                }
                else _concabecera = false;

                _dts = new PagoCarteraDAO().FunGetPagoCartera(7, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(ViewState["CodCatalogo"].ToString()), lblIdentificacion.InnerText,
                    ViewState["operacion"].ToString(), TxtDocumento.Text.Trim(), TxtFechaPago.Text.Trim(),
                    TxtValor.Text.Trim(), "", "", "", "", int.Parse(ViewState["CodigoGEST"].ToString()), _efectivo, 0,
                    int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _conexion);

                if (_dts != null && _dts.Tables[0].Rows[0][0].ToString() == "OK")
                {
                    if (_concabecera) _mensaje = new PagoCarteraDAO().FunInsertarPagoAbonoCab(pagocartera, pagoCabecera);
                    else _mensaje = new PagoCarteraDAO().FunInsertarPagoAbono(pagocartera);

                    if (_mensaje == "")
                    {
                        FunCargarDeudas();
                        FunCargarDatos();
                    }
                    else Lblerror.Text = _mensaje;
                }

                TxtFechaPago.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtDocumento.Text = "";
                TxtValor.Text = "";
                DdlTipoPago.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgReversar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _operacion = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Operacion"].ToString();
                _valorpago = GrdvDatos.DataKeys[gvRow.RowIndex].Values["SumPago"].ToString().Replace(",", ".");
                _documento = GrdvDatos.Rows[gvRow.RowIndex].Cells[3].Text;

                _dts = new PagoCarteraDAO().FunGetPagoCartera(8, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(ViewState["CodCatalogo"].ToString()), lblIdentificacion.InnerText, _operacion, _documento,
                    _fechaactual, _valorpago, "", "", "", "", int.Parse(ViewState["CodigoGEST"].ToString()), 0, 0,
                    int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), _conexion);

                if (_dts != null)
                {
                    if (_dts.Tables[0].Rows[0][0].ToString() == "OK")
                    {
                        FunCargarDatos();
                        FunCargarDeudas();
                    }
                    else new FuncionesDAO().FunShowJSMessage(_dts.Tables[0].Rows[0][0].ToString(), this);
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
                ViewState["identificacion"] = GrdvDeudas.DataKeys[gvRow.RowIndex].Values["Cedula"].ToString();
                ViewState["operacion"] = GrdvDeudas.Rows[gvRow.RowIndex].Cells[0].Text;
                ViewState["CodCatalogo"] = GrdvDeudas.DataKeys[gvRow.RowIndex].Values["CodigoCPCE"].ToString();
                ViewState["saldo"] = GrdvDeudas.DataKeys[gvRow.RowIndex].Values["SumSaldo"].ToString();
                ViewState["CodigoGEST"] = GrdvDeudas.DataKeys[gvRow.RowIndex].Values["CodigoGEST"].ToString();
                ViewState["DiasMora"] = GrdvDeudas.DataKeys[gvRow.RowIndex].Values["DiasMora"].ToString();
                FunCargarDatos();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunLimpiarCampos();
            _dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));

            if (_dts.Tables[0].Rows.Count > 0)
            {
                ViewState["CodigoCedente"] = DdlCedente.SelectedValue;
                DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                DdlCatalogo.DataTextField = "CatalogoProducto";
                DdlCatalogo.DataValueField = "CodigoCatalogo";
                DdlCatalogo.DataBind();
                DdlCatalogo.SelectedIndex = 0;
                ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
            }
            else
            {
                DdlCatalogo.Items.Clear();
                _itemc.Text = "--Seleccione Catálago/Producto--";
                _itemc.Value = "0";
                DdlCatalogo.Items.Add(_itemc);
            }
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}