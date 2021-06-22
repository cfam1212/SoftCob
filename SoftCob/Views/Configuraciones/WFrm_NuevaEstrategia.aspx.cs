namespace SoftCob.Views.Configuraciones
{
    using AjaxControlToolkit;
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevaEstrategia : Page
    {

        #region Variables
        DataSet _dts = new DataSet();
        DataSet _dts1 = new DataSet();
        DataRow _filagre;
        DataTable _tblagre = new DataTable();
        DataTable _tblbuscar = new DataTable();
        DataTable _dtbcampos = new DataTable();
        ImageButton _imgsubir = new ImageButton();
        DataRow _result;
        CheckBox _chkestado = new CheckBox();
        ImageButton _imgborrar = new ImageButton();
        int _maxcodigo = 0, _prioridad = 0, _codigo = 0;
        bool _lexiste = false;
        string _mensaje = "", _textovalor = "", _estado = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            if (!IsPostBack)
            {
                if (Session["IN-CALL"].ToString() == "SI")
                {
                    new ElastixDAO().ElastixHangUp(Session["IPLocalAdress"].ToString(), 9999);
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                ViewState["CodigoEstrategia"] = Request["CodigoEstrategia"];
                ViewState["Codigo"] = "";
                FunCargarCombos(0);
                FunCargarCombos(1);
                FunCargarMantenimiento();

                if (int.Parse(ViewState["CodigoEstrategia"].ToString()) == 0) Lbltitulo.Text = "Nueva Estrategia";
                else Lbltitulo.Text = "Editar Estrategia";
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento()
        {
            try
            {
                _dts = new EstrategiaDAO().FunGetEstrategiaDetalle(int.Parse(ViewState["CodigoEstrategia"].ToString()));
                ViewState["EstrategiaDetalle"] = _dts.Tables[0];
                GrdvCampos.DataSource = _dts;
                GrdvCampos.DataBind();

                new FuncionesDAO().SetearGrid(GrdvCampos, _imgsubir, 4, _dts.Tables[0]);

                _dts = new ConsultaDatosDAO().FunConsultaDatos(21, int.Parse(ViewState["CodigoEstrategia"].ToString()), 0, 0, "", "", 
                    "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    TxtEstrategia.Text = _dts.Tables[0].Rows[0]["Estrategia"].ToString();
                    TxtDescripcion.Text = _dts.Tables[0].Rows[0]["Descripcion"].ToString();
                    lblEstado.Visible = true;
                    ChkEstadoCab.Visible = true;
                    ChkEstadoCab.Checked = _dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                    ChkEstadoCab.Text = _dts.Tables[0].Rows[0]["Estado"].ToString();
                    ViewState["fechacreacion"] = _dts.Tables[0].Rows[0]["Fechacreacion"].ToString();
                    ViewState["usucreacion"] = _dts.Tables[0].Rows[0]["Usuariocreacion"].ToString();
                    ViewState["terminalcreacion"] = _dts.Tables[0].Rows[0]["Terminalcreacion"].ToString();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlCampos.DataSource = new EstrategiaDAO().FunGetCamposComboEstrategia();
                    DdlCampos.DataTextField = "Descripcion";
                    DdlCampos.DataValueField = "Codigo";
                    DdlCampos.DataBind();
                    break;
                case 1:
                    List<KeyValuePair<string, string>> listOper = new List<KeyValuePair<string, string>>();
                    listOper.Add(new KeyValuePair<string, string>("", "--<Seleccion Operación>--"));
                    listOper.Add(new KeyValuePair<string, string>("=", "Igual (=)"));
                    listOper.Add(new KeyValuePair<string, string>(">", "Mayor que (>)"));
                    listOper.Add(new KeyValuePair<string, string>(">=", "Mayor Igual que (>=)"));
                    listOper.Add(new KeyValuePair<string, string>("<", "Menor que (<)"));
                    listOper.Add(new KeyValuePair<string, string>("<=", "Menor Igual que (<=)"));
                    listOper.Add(new KeyValuePair<string, string>("like", "Contiene Caracteres (Like)"));
                    listOper.Add(new KeyValuePair<string, string>("between", "Entre"));
                    DdlOperacion.DataSource = listOper;
                    DdlOperacion.DataTextField = "Value";
                    DdlOperacion.DataValueField = "Key";
                    DdlOperacion.DataBind();
                    break;
            }
        }

        private void FunLimpiarCampos()
        {
            DdlCampos.SelectedIndex = 0;
            DdlOperacion.SelectedIndex = 0;
            txtValor.Text = "";
            ChkOrdenar.Checked = false;
            RdbOrdenar.Items[0].Selected = true;
            RdbOrdenar.Enabled = false;
        }

        private void FunSetearCampos(int codigocampo)
        {
            try
            {
                txtValor.Text = "";
                _dts = new EstrategiaDAO().FunGetDatosCampos(codigocampo);
                ViewState["CodigoTabla"] = _dts.Tables[0].Rows[0]["CodigoTabla"].ToString();
                ViewState["Tipo"] = _dts.Tables[0].Rows[0]["Tipo"].ToString();
                txtValor.Attributes.Clear();

                switch (ViewState["Tipo"].ToString())
                {
                    case "date":
                    case "datetime":
                        CalendarExtender calExtender = new CalendarExtender();
                        calExtender.Format = "MM/dd/yyyy";
                        calExtender.TargetControlID = txtValor.ID;
                        PlaceTxt.Controls.Add(calExtender);
                        break;
                    case "int":
                    case "smallint":
                    case "tinyint":
                        FilteredTextBoxExtender filter = new FilteredTextBoxExtender();
                        filter.FilterType = FilterTypes.Numbers;
                        filter.TargetControlID = txtValor.ID;
                        PlaceTxt.Controls.Add(filter);
                        break;
                    case "decimal":
                    case "numeric":
                    case "float":
                    case "money":
                    case "real":
                        txtValor.Attributes.Add("onkeypress", "return NumeroDecimal(this.form.txtValor, event)");
                        txtValor.Attributes.Add("onchange", "ValidarDecimales();");
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private bool ValidarCondiciones()
        {
            if (DdlCampos.SelectedValue.ToString() == "" || DdlCampos.SelectedValue.ToString() == "0")
            {
                new FuncionesDAO().FunShowJSMessage("Debe seleccionar un Campo para la condición", this);
                return false;
            }

            if (DdlOperacion.SelectedValue.ToString() == "")
            {
                new FuncionesDAO().FunShowJSMessage("Debe seleccionar tipo de operación", this);
                return false;
            }

            if (txtValor.Text == "")
            {
                new FuncionesDAO().FunShowJSMessage("Debe ingresar un valor de comparación para la condición", this);
                return false;
            }

            _tblbuscar = (DataTable)ViewState["EstrategiaDetalle"];
            _result = _tblbuscar.Select("CodigoCampo='" + DdlCampos.SelectedValue.ToString() + "' and Operacion='" + DdlOperacion.SelectedItem.ToString() + "'").FirstOrDefault();
            _lexiste = _result != null ? true : false;

            if (_lexiste)
            {
                new FuncionesDAO().FunShowJSMessage("Ya existe un registro con esta condición", this);
                return false;
            }

            switch (ViewState["Tipo"].ToString())
            {
                case "numeric":
                case "double":
                case "decimal":
                case "int":
                    if (DdlOperacion.SelectedValue.ToString() == "like")
                    {
                        new FuncionesDAO().FunShowJSMessage("No puede utilizar la operacion like para datos numéricos", this);
                        return false;
                    }
                    break;
                case "date":
                case "smalldatetime":
                case "datetime":
                    if (DdlOperacion.SelectedValue.ToString() == "like")
                    {
                        new FuncionesDAO().FunShowJSMessage("No puede utilizar la operacion like para datos tipo fecha", this);
                        return false;
                    }
                    break;
            }
            return true;
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCampos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(DdlCampos.SelectedValue) > 0)
            {
                FunSetearCampos(int.Parse(DdlCampos.SelectedValue));
            }
        }

        protected void DdlOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(DdlCampos.SelectedValue) > 0)
            {
                FunSetearCampos(int.Parse(DdlCampos.SelectedValue));
            }
        }

        protected void ImgAddCampo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ValidarCondiciones())
                {
                    _textovalor = txtValor.Text;

                    switch (ViewState["Tipo"].ToString())
                    {
                        case "date":
                        case "datetime":
                        case "varchar":
                        case "char":
                            if (DdlOperacion.SelectedValue == "like") _textovalor = "'" + txtValor.Text + "%'";
                            else _textovalor = "'" + txtValor.Text + "'";
                            break;
                    }

                    _tblagre = (DataTable)ViewState["EstrategiaDetalle"];

                    if (_tblagre.Rows.Count > 0)
                    {
                        _maxcodigo = _tblagre.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                        _prioridad = _tblagre.AsEnumerable()
                            .Max(row => int.Parse((string)row["Prioridad"]));
                    }

                    _filagre = _tblagre.NewRow();
                    _filagre["Codigo"] = _maxcodigo + 1;
                    _filagre["CodigoCampo"] = DdlCampos.SelectedValue;
                    _filagre["Campo"] = DdlCampos.SelectedItem.ToString();
                    _filagre["Operacion"] = DdlOperacion.SelectedValue;
                    _filagre["Valor"] = _textovalor;

                    if (ChkOrdenar.Checked)
                    {
                        if (RdbOrdenar.Items[0].Selected) _filagre["Orden"] = "asc";
                        else _filagre["Orden"] = "desc";
                    }
                    else _filagre["Orden"] = "";

                    _filagre["Prioridad"] = _prioridad + 1;
                    _filagre["Estado"] = "Activo";
                    _filagre["auxv1"] = DdlOperacion.SelectedValue;
                    _filagre["auxv2"] = "";
                    _filagre["auxv3"] = "";
                    _filagre["auxi1"] = "0";
                    _filagre["auxi2"] = "0";
                    _filagre["auxi3"] = "0";
                    _tblagre.Rows.Add(_filagre);
                    _tblagre.DefaultView.Sort = "Prioridad";
                    ViewState["EstrategiaDetalle"] = _tblagre;
                    GrdvCampos.DataSource = _tblagre;
                    GrdvCampos.DataBind();
                    new FuncionesDAO().SetearGrid(GrdvCampos, _imgsubir, 4, _tblagre);
                    FunLimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkOrdenar_CheckedChanged(object sender, EventArgs e)
        {

            if (ChkOrdenar.Checked) RdbOrdenar.Enabled = true;
            else RdbOrdenar.Enabled = false;
        }

        protected void ImgSubir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _dtbcampos = (DataTable)ViewState["EstrategiaDetalle"];
                _codigo = int.Parse(GrdvCampos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _prioridad = int.Parse(GrdvCampos.DataKeys[_gvrow.RowIndex].Values["Prioridad"].ToString());

                _result = _dtbcampos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Prioridad"] = _prioridad - 1;
                _dtbcampos.AcceptChanges();

                _codigo = int.Parse(GrdvCampos.DataKeys[_gvrow.RowIndex - 1].Values["Codigo"].ToString());
                _prioridad = int.Parse(GrdvCampos.DataKeys[_gvrow.RowIndex - 1].Values["Prioridad"].ToString());
                _result = _dtbcampos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Prioridad"] = _prioridad + 1;
                _dtbcampos.AcceptChanges();

                _dtbcampos.DefaultView.Sort = "Prioridad ASC";
                _dtbcampos = _dtbcampos.DefaultView.ToTable();
                GrdvCampos.DataSource = _dtbcampos;
                GrdvCampos.DataBind();
                new FuncionesDAO().SetearGrid(GrdvCampos, _imgsubir, 4, _dtbcampos);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkestado = (CheckBox)(_gvrow.Cells[4].FindControl("chkEstado"));
                _dtbcampos = (DataTable)ViewState["EstrategiaDetalle"];
                _codigo = int.Parse(GrdvCampos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                _result = _dtbcampos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result["Estado"] = _chkestado.Checked ? "Activo" : "Inactivo";
                _dtbcampos.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgModCampo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ValidarCondiciones())
                {
                    _textovalor = txtValor.Text;

                    switch (ViewState["Tipo"].ToString())
                    {
                        case "date":
                        case "datetime":
                        case "varchar":
                        case "char":
                            if (DdlOperacion.SelectedValue == "like") _textovalor = "'" + txtValor.Text + "%'";
                            else _textovalor = "'" + txtValor.Text + "'";
                            break;
                    }

                    _tblbuscar = (DataTable)ViewState["EstrategiaDetalle"];
                    _result = _tblbuscar.Select("Codigo='" + ViewState["CodigoEst"].ToString() + "'").FirstOrDefault();
                    _result["CodigoCampo"] = DdlCampos.SelectedValue;
                    _result["Campo"] = DdlCampos.SelectedItem.ToString();
                    _result["Operacion"] = DdlOperacion.SelectedValue;
                    _result["Valor"] = _textovalor;

                    if (ChkOrdenar.Checked)
                    {
                        if (RdbOrdenar.Items[0].Selected) _result["Orden"] = "asc";
                        else _result["Orden"] = "desc";
                    }
                    else _result["Orden"] = "";

                    _result["Estado"] = ViewState["Estado"].ToString();
                    _result["auxv1"] = DdlOperacion.SelectedValue;
                    _result["auxv2"] = "";
                    _result["auxv3"] = "";
                    _result["auxi1"] = "0";
                    _result["auxi2"] = "0";
                    _result["auxi3"] = "0";
                    _tblbuscar.AcceptChanges();
                    _tblbuscar.DefaultView.Sort = "Prioridad";
                    ViewState["EstrategiaDetalle"] = _tblbuscar;
                    GrdvCampos.DataSource = _tblbuscar;
                    GrdvCampos.DataBind();
                    new FuncionesDAO().SetearGrid(GrdvCampos, _imgsubir, 4, _tblbuscar);
                    FunLimpiarCampos();
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
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow _fr in GrdvCampos.Rows)
                {
                    _fr.Cells[0].BackColor = Color.White;
                }

                GrdvCampos.Rows[_gvrow.RowIndex].Cells[0].BackColor = Color.Coral;
                _codigo = int.Parse(GrdvCampos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                ViewState["CodigoEst"] = _codigo;
                _dtbcampos = (DataTable)ViewState["EstrategiaDetalle"];
                _result = _dtbcampos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                ViewState["Estado"] = _result["Estado"].ToString();
                DdlCampos.SelectedValue = _result["CodigoCampo"].ToString();
                FunSetearCampos(int.Parse(DdlCampos.SelectedValue));
                DdlOperacion.SelectedValue = _result["Operacion"].ToString();
                txtValor.Text = _result["Valor"].ToString().Replace("'", "");

                if (!string.IsNullOrEmpty(_result["Orden"].ToString()))
                {
                    ChkOrdenar.Checked = true;
                    RdbOrdenar.Enabled = true;
                }

                if (_result["Orden"].ToString() == "asc") RdbOrdenar.Items[0].Selected = true;

                if (_result["Orden"].ToString() == "desc") RdbOrdenar.Items[1].Selected = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void ImgDelCampo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = int.Parse(GrdvCampos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
                new EstrategiaDAO().FunDelEstrategiaDet(int.Parse(ViewState["CodigoEstrategia"].ToString()), _codigo);
                _dtbcampos = (DataTable)ViewState["EstrategiaDetalle"];
                _result = _dtbcampos.Select("Codigo='" + _codigo + "'").FirstOrDefault();
                _result.Delete();
                _dtbcampos.AcceptChanges();
                _dtbcampos.DefaultView.Sort = "Prioridad ASC";
                GrdvCampos.DataSource = _dtbcampos;
                GrdvCampos.DataBind();
                new FuncionesDAO().SetearGrid(GrdvCampos, _imgsubir, 4, _dtbcampos);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.Message;
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtEstrategia.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese nombre de la Estrategia...!", this);
                    return;
                }

                if (string.IsNullOrEmpty(TxtDescripcion.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese descripción de la Estrategia...!", this);
                    return;
                }

                _dtbcampos = (DataTable)ViewState["EstrategiaDetalle"];

                if (_dtbcampos.Rows.Count > 0)
                {
                    SoftCob_ESTRATEGIA_CABECERA _estcab = new SoftCob_ESTRATEGIA_CABECERA();
                    {
                        _estcab.ESCA_CODIGO = int.Parse(ViewState["CodigoEstrategia"].ToString());
                        _estcab.esca_estrategia = TxtEstrategia.Text.Trim().ToUpper();
                        _estcab.esca_descripcion = TxtDescripcion.Text.Trim().ToUpper();
                        _estcab.esca_estado = ChkEstadoCab.Checked ? true : false;
                        _estcab.esca_auxv1 = "";
                        _estcab.esca_auxv2 = "";
                        _estcab.esca_auxv3 = "";
                        _estcab.esca_auxi1 = 0;
                        _estcab.esca_auxi2 = 0;
                        _estcab.esca_auxi3 = 0;
                        _estcab.esca_fum = DateTime.Now;
                        _estcab.esca_uum = int.Parse(Session["usuCodigo"].ToString());
                        _estcab.esca_tum = Session["MachineName"].ToString();
                    }

                    if (int.Parse(ViewState["CodigoEstrategia"].ToString()) == 0)
                    {
                        _estcab.esca_fechacreacion = DateTime.Now;
                        _estcab.esca_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                        _estcab.esca_terminalcreacion = Session["MachineName"].ToString();
                    }
                    else
                    {
                        _estcab.esca_fechacreacion = DateTime.ParseExact(ViewState["fechacreacion"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        _estcab.esca_usuariocreacion = int.Parse(ViewState["usucreacion"].ToString());
                        _estcab.esca_terminalcreacion = ViewState["terminalcreacion"].ToString();
                    }

                    _estcab.SoftCob_ESTRATEGIA_DETALLE = new List<SoftCob_ESTRATEGIA_DETALLE>();

                    List<SoftCob_ESTRATEGIA_DETALLE> _estdetalle = new List<SoftCob_ESTRATEGIA_DETALLE>();

                    foreach (DataRow _dr in _dtbcampos.Rows)
                    {
                        _estdetalle.Add(new SoftCob_ESTRATEGIA_DETALLE()
                        {
                            ESDE_CODIGO = new EstrategiaDAO().FunGetCodigoCabEstrategia(int.Parse(ViewState["CodigoEstrategia"].ToString()), int.Parse(_dr[0].ToString())),
                            ESCA_CODIGO = int.Parse(ViewState["CodigoEstrategia"].ToString()),
                            esde_caescodigo = int.Parse(_dr[1].ToString()),
                            esde_operacion = _dr[3].ToString(),
                            esde_valor = _dr[4].ToString(),
                            esde_orden = _dr[5].ToString(),
                            esde_prioridad = int.Parse(_dr[6].ToString()),
                            esde_estado = _dr[7].ToString() == "Activo" ? true : false,
                            esde_auxv1 = "",
                            esde_auxv2 = "",
                            esde_auxv3 = "",
                            esde_auxi1 = 0,
                            esde_auxi2 = 0,
                            esde_auxi3 = 0
                        });
                    }

                    _estcab.SoftCob_ESTRATEGIA_DETALLE = new List<SoftCob_ESTRATEGIA_DETALLE>();

                    foreach (SoftCob_ESTRATEGIA_DETALLE _detalle in _estdetalle)
                    {
                        _estcab.SoftCob_ESTRATEGIA_DETALLE.Add(_detalle);
                    }

                    if (_estcab.ESCA_CODIGO == 0) _mensaje = new EstrategiaDAO().FunCrearEstrategia(_estcab);
                    else _mensaje = new EstrategiaDAO().FunEditEstrategia(_estcab);

                    if (_mensaje == "") Response.Redirect("WFrm_EstrategiaAdmin.aspx?MensajeRetornado=Guardado con Éxito");
                    else Lblerror.Text = _mensaje;
                }
                else new FuncionesDAO().FunShowJSMessage("Ingrese al menos un registro para la Estrategia..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvCampos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkestado = (CheckBox)(e.Row.Cells[5].FindControl("chkEstado"));
                    _estado = GrdvCampos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();

                    if (_estado == "Activo") _chkestado.Checked = true;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstadoCab_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstadoCab.Text = ChkEstadoCab.Checked ? "Activo" : "Inactivo";
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_EstrategiaAdmin.aspx", true);
        }
        #endregion
    }
}