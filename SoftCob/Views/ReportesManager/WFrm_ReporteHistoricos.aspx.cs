namespace SoftCob.Views.ReportesManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteHistoricos : Page
    {
        #region Variables
        int _intgridview = 0, _tipo = 0;
        string _sql = "", _fecha = "";
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        ListItem _itemc = new ListItem();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ImgExportar);
            if (!IsPostBack)
            {

                TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Lbltitulo.Text = "Reporte Historico";
                FunCargarCombos(0);
            }
            else GrdvDatos.DataSource = Session["grdvDatos"];
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
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                _tipo = 0;
                pnlHistorico1.Visible = false;
                pnlHistorico2.Visible = false;
                ImgExportar.Visible = false;
                lblExportar.Visible = false;

                if (DdlCedente.SelectedItem.ToString() == "--Seleccione Cedente--")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaIni.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaFin.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("No es una fecha válida..!", this);
                    return;
                }

                if (DateTime.ParseExact(TxtFechaIni.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(TxtFechaFin.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new FuncionesDAO().FunShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this);
                    return;
                }

                if (DdlBuscar.SelectedValue != "0" && string.IsNullOrEmpty(TxtBuscarPor.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese valor de Operación o Identificación..!", this);
                    return;
                }

                if (DdlTipoReporte.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Tipo de Reporte..!", this);
                    return;
                }

                ViewState["Cedente"] = DdlCedente.SelectedItem.ToString().Replace(" ", "") + "_" + DdlCedente.SelectedValue;
                switch (DdlTipoReporte.SelectedValue)
                {
                    case "C":
                        _sql = "select FechaProceso = convert(date,HC.hiop_fechaproceso,103),Identificacion = HC.hiop_identificacion,";
                        _sql += "Operacion = HC.hiop_operacion,Exigible = HC.hiop_valorexigible,DiasMora = HC.hiop_diasmora " +
                            "from ENTERPRISE_Cedentes..HISTORICO_" + ViewState["Cedente"].ToString() + " HC (nolock) ";
                        _sql += "where HC.hiop_fechaproceso between convert(date,'" + TxtFechaIni.Text + "',101) and convert(date,'" + TxtFechaFin.Text + "',101) and ";

                        if (DdlBuscar.SelectedValue == "I") _sql += "HC.hiop_identificacion='" + TxtBuscarPor.Text.Trim() + "' and ";

                        if (DdlBuscar.SelectedValue == "O") _sql += "HC.hiop_operacion='" + TxtBuscarPor.Text.Trim() + "' and ";

                        _sql = _sql.Remove(_sql.Length - 4) + "order by HC.hiop_fechaproceso";
                        _tipo = 1;
                        _intgridview = 1;
                        break;
                    case "S":
                        _sql = "select * from (select hiop_identificacion 'Identificación',hiop_operacion 'Operación',DAY(hiop_fechaproceso) AS DedID,hiop_valorexigible ";
                        _sql += "from ENTERPRISE_Cedentes..HISTORICO_" + ViewState["Cedente"].ToString() + " (nolock) where hiop_fechaproceso between CONVERT(date,'" + TxtFechaIni.Text + "',101) ";

                        if (DdlBuscar.SelectedValue == "I") _sql += "and CONVERT(date,'" + TxtFechaFin.Text + "',101) and hiop_identificacion='" + TxtBuscarPor.Text + "') deds ";

                        if (DdlBuscar.SelectedValue == "O") _sql += "and CONVERT(date,'" + TxtFechaFin.Text + "',101) and hiop_operacion='" + TxtBuscarPor.Text + "') deds ";

                        if (DdlBuscar.SelectedValue == "0") _sql += "and CONVERT(date,'" + TxtFechaFin.Text + "',101)) deds ";

                        _sql += "PIVOT (MAX(hiop_valorexigible)FOR DedID IN([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],";
                        _sql += "[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) descs";
                        _tipo = 1;
                        _intgridview = 1;
                        break;
                    case "O":
                        _sql = "select Fecha = convert(date,hiop_fechaproceso,103),Operaciones = CT.operaciones,Saldo = '$' + cast(convert(varchar(20),cast(CT.saldos as money),1) as varchar)";
                        _sql += "from (select count(*) operaciones,SUM(hiop_valorexigible) saldos,hiop_fechaproceso " +
                            "from ENTERPRISE_Cedentes..HISTORICO_" + ViewState["Cedente"].ToString() + " (nolock) ";
                        _sql += " group by hiop_fechaproceso)as CT where CT.hiop_fechaproceso between CONVERT(date,'" + TxtFechaIni.Text + "',101) and ";
                        _sql += "CONVERT(date,'" + TxtFechaFin.Text + "',101) order by CT.hiop_fechaproceso ";
                        _tipo = 1;
                        _intgridview = 2;
                        break;
                    case "G":
                        _sql = "select distinct Accion_Respuesta = (select XC.arac_descripcion from SoftCob_ACCION XC (nolock) where XC.ARAC_CODIGO=GT.gete_araccodigo), ";
                        _sql += "Total = count(*) from SoftCob_GESTION_TELEFONICA GT (nolock) where GT.gete_cpcecodigo=" + DdlCatalogo.SelectedValue + " and ";
                        _sql += "GT.gete_fechagestion between CONVERT(date,'" + TxtFechaIni.Text + "',101) and CONVERT(date,'" + TxtFechaFin.Text + "',101) ";

                        if (DdlBuscar.SelectedValue == "I") _sql += "GT.gete_numerodocumento='" + TxtBuscarPor.Text + "' ";

                        if (DdlBuscar.SelectedValue == "O") _sql += "GT.gete_operacion='" + TxtBuscarPor.Text + "' ";

                        _sql += "GROUP BY GT.gete_araccodigo";
                        _tipo = 1;
                        _intgridview = 1;
                        break;
                    case "R":
                        _sql = "select Cedula = PC.pacp_numerodocumento,Operacion = PC.pacp_operacion,Nombre = PE.pers_nombrescompletos,";
                        _sql += "Documento = PC.pacp_documento,TipoPago = (select PD.pade_nombre from SoftCob_PARAMETRO_DETALLE PD (nolock) where PD.pade_valorI=PC.pade_codigo and ";
                        _sql += "PD.PARA_CODIGO in(select PARA_CODIGO from SoftCob_PARAMETRO_CABECERA (nolock) where para_nombre='TIPO PAGO')),";
                        _sql += "Valor = cast(round(PC.pacp_valorpago,2) as decimal(12,2)),FechaPago = CONVERT(varchar(10),PC.pacp_fechapago,121),";
                        _sql += "Gestor= (select usu_Nombres+' '+usu_Apellidos from USUARIO (nolock) where USU_CODIGO in";
                        _sql += "(select ctde_gestorasignado from SoftCob_CUENTA_DEUDOR (nolock) where ctde_operacion=PC.pacp_operacion)),";
                        _sql += "FechaRegistro = CONVERT(varchar(10),PC.pacp_fechacreacion,121),";
                        _sql += "Usuario = (select US.usu_Nombres+' '+US.usu_Apellidos from USUARIO US (nolock) where US.USU_CODIGO=PC.pacp_usuariocreacion) ";
                        _sql += "from SoftCob_PAGOSCARTERA PC INNER JOIN SoftCob_PERSONA PE (nolock) ON PC.pacp_numerodocumento=PE.pers_numerodocumento ";
                        _sql += "where PC.pacp_cpcecodigo=" + DdlCatalogo.SelectedValue + " and PC.pacp_fechapago between CONVERT(date,'" + TxtFechaIni.Text + "',101) and ";
                        _sql += "CONVERT(date,'" + TxtFechaFin.Text + "',101) ";

                        if (DdlBuscar.SelectedValue == "I") _sql += "and PC.pacp_numerodocumento='" + TxtBuscarPor.Text + "' ";

                        if (DdlBuscar.SelectedValue == "O") _sql += "and PC.pacp_operacion='" + TxtBuscarPor.Text + "' ";

                        _sql += "order by PC.pacp_fechacreacion";
                        _tipo = 1;
                        _intgridview = 1;
                        break;
                    case "E":
                        _sql = "select * from (select day(gete_fechagestion) as DedID,count(1) as total ";
                        _sql += "from SoftCob_GESTION_TELEFONICA (nolock) where gete_cpcecodigo=" + DdlCatalogo.SelectedValue + "and  gete_fechagestion between CONVERT(date,'" + TxtFechaIni.Text + "',101) ";

                        if (DdlBuscar.SelectedValue == "I") _sql += "and CONVERT(date,'" + TxtFechaFin.Text + "',101) and gete_numerodocumento='" + TxtBuscarPor.Text + "' group by gete_fechagestion) deds ";

                        if (DdlBuscar.SelectedValue == "O") _sql += "and CONVERT(date,'" + TxtFechaFin.Text + "',101) and gete_operacion='" + TxtBuscarPor.Text + "' group by gete_fechagestion) deds ";

                        if (DdlBuscar.SelectedValue == "0") _sql += "and CONVERT(date,'" + TxtFechaFin.Text + "',101) group by gete_fechagestion) deds ";

                        _sql += "PIVOT (MAX(total)FOR DedID IN([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],";
                        _sql += "[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) descs";
                        _tipo = 1;
                        _intgridview = 1;
                        break;
                    case "F":
                        _sql = "";
                        _tipo = 1;
                        _intgridview = 1;
                        _dts = new ConsultaDatosDAO().FunConsultaDatos(226, int.Parse(DdlCatalogo.SelectedValue), 0, 0,
                            "", "", "", Session["Conectar"].ToString());
                        break;
                }

                if (!string.IsNullOrEmpty(_sql))
                {
                    _dts = new ConsultaDatosDAO().FunGetRerporteGestiones(_tipo, int.Parse(DdlCedente.SelectedValue),
                        int.Parse(DdlCatalogo.SelectedValue), TxtFechaIni.Text, TxtFechaFin.Text,
                        DdlBuscar.SelectedItem.ToString(), TxtBuscarPor.Text.Trim(), _sql, "", 0, 0,
                        Session["Conectar"].ToString());
                }

                if (_dts != null)
                {
                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        ImgExportar.Visible = true;
                        lblExportar.Visible = true;

                        if (_intgridview == 1)
                        {
                            GrdvDatos.DataSource = _dts;
                            GrdvDatos.DataBind();
                            pnlHistorico1.Visible = true;
                            ViewState["GrdvDatos"] = _dts.Tables[0];
                        }
                        else
                        {
                            GrdvDatos1.DataSource = _dts;
                            GrdvDatos1.DataBind();
                            pnlHistorico2.Visible = true;
                            ViewState["GrdvDatos"] = _dts.Tables[0];
                        }
                    }
                    if (_dts.Tables[0].Rows.Count > 65535)
                    {
                        Lblerror.Text = "El reporte contiene mas de 65536 registros, por favor para exportar filtre otro rango de fechas";
                    }
                }
                else
                {
                    Lblerror.Text = "Existe un error en los datos..!";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["GrdvDatos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    string FileName = "Historico_" + DdlCedente.SelectedItem.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void GrdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdvDatos.PageIndex = e.NewPageIndex;
            GrdvDatos.DataBind();
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

        protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _fecha = GrdvDatos1.Rows[gvRow.RowIndex].Cells[1].Text;
            Response.Redirect("WFrm_ReporteOperaHistorico.aspx?Fecha=" + _fecha + "&Cedente=" + ViewState["Cedente"].ToString());
        }

        protected void GrdvDatos1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdvDatos1.PageIndex = e.NewPageIndex;
            GrdvDatos1.DataBind();
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
        }

        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoCedente"] = DdlCedente.SelectedValue;
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
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