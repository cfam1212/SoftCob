namespace SoftCob.Views.Breanch
{
    using ControllerSoftCob;
    using ModeloSoftCob;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_NuevoBrenchMes : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        ListItem itemC = new ListItem();
        ListItem itemG = new ListItem();
        DataTable dtbBrench = new DataTable();
        DataTable dtbBrenchDet = new DataTable();
        string sql = "", casos = "", redirect = "", _mensaje = "";
        DataRow resultado, result;
        decimal tExigible = 0, tMonto = 0, tPorcentaje = 0, tPresupuesto = 0, vExigible = 0, vPorcentaje = 0, vPresupuesto = 0;
        bool continuar = false;
        int tOperaciones = 0;
        ImageButton ImgSelecc = new ImageButton();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-EC");
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            TxtPresupuesto.Attributes.Add("onchange", "ValidarDecimales();");

            if (!IsPostBack)
            {
                ViewState["Procesado"] = "NO";                
                FunCargarCombos(0);
                ViewState["Anio"] = DateTime.Now.Year.ToString();
                ViewState["MesName"] = DateTime.Now.ToString("MMMM").ToUpper();
                ViewState["Mes"] = DateTime.Now.ToString("MM");
                ViewState["CodigoBRMC"] = "0";
                Lbltitulo.Text = "Agregar Nuevo Brench Año: " + ViewState["Anio"].ToString() + " Mes: " + ViewState["MesName"].ToString();
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargaMantenimiento()
        {
            try
            {
                dtbBrenchDet.Clear();
                ViewState["CodigoBRMC"] = "0";
                ViewState["BrenchDet"] = dtbBrenchDet;
                GrdvBrenchDet.DataSource = dtbBrenchDet;
                GrdvBrenchDet.DataBind();
                dtbBrench = (DataTable)ViewState["BrenchCab"];

                dts = new ConsultaDatosDAO().FunConsultaDatos(116, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(DdlCatalogo.SelectedValue), int.Parse(DdlGestores.SelectedValue), "", "", "",
                    Session["Conectar"].ToString());

                if (dts.Tables[0].Rows.Count > 0) ViewState["CodigoBRMC"] = dts.Tables[0].Rows[0]["Codigo"].ToString();

                if (dtbBrench.Rows.Count > 0)
                {
                    sql += "SELECT CodigoAlter = ROW_NUMBER() OVER(ORDER BY rango_dias), Codigo = ISNULL((SELECT BMD.BRMD_CODIGO FROM SoftCob_BRENCHMESDET BMD ";
                    sql += "INNER JOIN SoftCob_BRENCHMESCAB BMC ON BMD.BRMC_CODIGO=BMC.BRMC_CODIGO ";
                    sql += "WHERE BMC.brmc_cedecodigo=" + DdlCedente.SelectedValue + " AND BMC.brmc_cpcecodigo=" + DdlCatalogo.SelectedValue + " AND BMC.brmc_presupuestoanio=" + ViewState["Anio"].ToString() + " AND ";
                    sql += "BMC.brmc_presupuestomes=" + ViewState["Mes"].ToString() + " and BMC.brmc_gestorasignado=" + DdlGestores.SelectedValue + " and BMD.brmd_brench=rango_dias),0),";
                    //sql += "ROW_NUMBER() OVER(ORDER BY rango_dias)),";
                    sql += "Operaciones = COUNT(*),Rango = rango_dias,";
                    sql += "Monto = CONVERT(VARCHAR(20),CAST(SUM(ctde_totaldeuda) AS money),1),";
                    sql += "Exigible = CONVERT(varchar(20),CAST(SUM(ctde_valorexigible) AS money),1),";
                    sql += "Porcentaje = ISNULL((SELECT BMD.brmd_porcentaje FROM SoftCob_BRENCHMESDET BMD ";
                    sql += "INNER JOIN SoftCob_BRENCHMESCAB BMC ON BMD.BRMC_CODIGO=BMC.BRMC_CODIGO ";
                    sql += "WHERE BMC.brmc_cedecodigo=" + DdlCedente.SelectedValue + " AND BMC.brmc_cpcecodigo=" + DdlCatalogo.SelectedValue + " AND BMC.brmc_presupuestoanio=" + ViewState["Anio"].ToString() + " AND ";
                    sql += "BMC.brmc_presupuestomes=" + ViewState["Mes"].ToString() + " AND BMC.brmc_gestorasignado=" + DdlGestores.SelectedValue + " AND BMD.brmd_brench=rango_dias),'0.00'),";
                    sql += "Presupuesto = ISNULL((SELECT CONVERT(VARCHAR(20),CAST(BMD.brmd_presupuesto AS money),1) FROM SoftCob_BRENCHMESDET BMD INNER JOIN SoftCob_BRENCHMESCAB BMC ON BMD.BRMC_CODIGO=BMC.BRMC_CODIGO ";
                    sql += "WHERE BMC.brmc_cedecodigo=" + DdlCedente.SelectedValue + " AND BMC.brmc_cpcecodigo=" + DdlCatalogo.SelectedValue + " AND ";
                    sql += "BMC.brmc_presupuestoanio=" + ViewState["Anio"].ToString() + " AND BMC.brmc_presupuestomes=" + ViewState["Mes"].ToString() + " AND ";
                    sql += "BMC.brmc_gestorasignado=" + DdlGestores.SelectedValue + " AND BMD.brmd_brench=rango_dias),'0.00'),";
                    sql += "PorcentajeValor=0.00,";
                    sql += "PresupuestoValor = ISNULL((SELECT BMD.brmd_presupuesto FROM SoftCob_BRENCHMESDET BMD INNER JOIN SoftCob_BRENCHMESCAB BMC ON BMD.BRMC_CODIGO=BMC.BRMC_CODIGO ";
                    sql += "WHERE BMC.brmc_cedecodigo=" + DdlCedente.SelectedValue + " AND BMC.brmc_cpcecodigo=" + DdlCatalogo.SelectedValue + " AND ";
                    sql += "BMC.brmc_presupuestoanio=" + ViewState["Anio"].ToString() + " AND BMC.brmc_presupuestomes=" + ViewState["Mes"].ToString() + " AND ";
                    sql += "BMC.brmc_gestorasignado=" + DdlGestores.SelectedValue + " AND BMD.brmd_brench=rango_dias),'0.00'),";
                    sql += "ExigibleValor = SUM(ctde_valorexigible),MontoValor = SUM(ctde_totaldeuda),";
                    sql += "RangoInicial = ISNULL((SELECT brde_rangoinicial FROM SoftCob_BRENCHDET WHERE brde_etiqueta=rango_dias " +
                        "AND brde_auxi1=" + ViewState["codigoCPCE"].ToString() + "),0),";
                    sql += "RangoFinal = ISNULL((SELECT brde_rangofinal FROM SoftCob_BRENCHDET WHERE brde_etiqueta=rango_dias " +
                        "AND brde_auxi1=" + ViewState["codigoCPCE"].ToString() + "),0),";
                    sql += "Orden = ISNULL((SELECT brde_orden FROM SoftCob_BRENCHDET WHERE brde_etiqueta=rango_dias " +
                        "AND brde_auxi1=" + ViewState["codigoCPCE"].ToString() + "),0) ";
                    sql += "FROM (SELECT CASE";
                    dtbBrench = (DataTable)ViewState["BrenchCab"];
                    foreach (DataRow row in dtbBrench.Rows)
                    {
                        casos += " WHEN dias_mora BETWEEN " + row["RangoIni"].ToString() + " AND " + row["RangoFin"].ToString() +
                            " THEN '" + row["Etiqueta"].ToString() + "'";
                    }
                    casos += " END AS rango_dias,ctde_valorexigible,ctde_totaldeuda FROM (";
                    casos += "SELECT CDE.ctde_totaldeuda, CDE.ctde_valorexigible,CDE.ctde_diasmora AS dias_mora ";
                    casos += "FROM SoftCob_CUENTA_DEUDOR CDE INNER JOIN SoftCob_CLIENTE_DEUDOR CLI ON CDE.CLDE_CODIGO=CLI.CLDE_CODIGO ";
                    casos += "WHERE CLI.CPCE_CODIGO=" + ViewState["codigoCPCE"].ToString() + " AND CDE.ctde_gestorasignado=";
                    casos += DdlGestores.SelectedValue + " AND CDE.ctde_estado=1 AND CLI.clde_estado=1) res) tabla ";
                    casos += "WHERE ISNULL(rango_dias,'')!='' ";
                    casos += "GROUP BY rango_dias ORDER BY rango_dias";
                    sql = sql + casos;
                    dts = new ConsultaDatosDAO().FunConsultaDatos(15, 0, 0, 0, sql, "", "", Session["Conectar"].ToString());
                    ViewState["BrenchDet"] = dts.Tables[0];
                    GrdvBrenchDet.DataSource = dts;
                    GrdvBrenchDet.DataBind();

                    //dts = new ConsultaDatosDTO().funConsultaDatos(111, int.Parse(ddlCedente.SelectedValue), int.Parse(ddlCatalogo.SelectedValue),
                    //    int.Parse(ddlGestores.SelectedValue), "", ViewState["Anio"].ToString(), ViewState["Mes"].ToString(), Session["Conectar"].ToString());
                    //if (dts.Tables[0].Rows.Count > 0)
                    //{
                    //    ViewState["Procesado"] = "SI";
                    //    ViewState["BrenchDet"] = dts.Tables[0];
                    //    grdvBrenchDet.DataSource = dts;
                    //    grdvBrenchDet.DataBind();
                    //    btnGrabar.Enabled = false;
                    //}
                    //else
                    //{
                    //    ViewState["Procesado"] = "NO";
                    //    btnGrabar.Enabled = true;
                    //    sql = "select Codigo = ROW_NUMBER() over(order by rango_dias),";
                    //    sql += "Operaciones = count(*),Rango = rango_dias,";
                    //    sql += "Monto = cast(convert(varchar(20),cast(sum(ctde_totaldeuda) as money),1) as varchar),";
                    //    sql += "Exigible = cast(convert(varchar(20),cast(sum(ctde_valorexigible) as money),1) as varchar),";
                    //    sql += "Porcentaje = '0.00',";
                    //    sql += "Presupuesto = '0.00',";
                    //    sql += "PorcentajeValor=0.00,PresupuestoValor=0.00,ExigibleValor = sum(ctde_valorexigible),MontoValor = sum(ctde_totaldeuda),";
                    //    sql += "RangoInicial = 0,RangoFinal = 0,Orden = 0 from (select case";
                    //    dtbBrench = (DataTable)ViewState["BrenchCab"];
                    //    foreach (DataRow row in dtbBrench.Rows)
                    //    {
                    //        casos += " when dias_mora between " + row["RangoIni"].ToString() + " and " + row["RangoFin"].ToString() +
                    //            " then '" + row["Etiqueta"].ToString() + "'";
                    //    }
                    //    casos += " end as rango_dias,ctde_valorexigible,ctde_totaldeuda from (";
                    //    casos += "select CD.ctde_totaldeuda, CD.ctde_valorexigible,CD.ctde_diasmora as dias_mora ";
                    //    casos += "from SoftCob_CUENTA_DEUDOR CD INNER JOIN SoftCob_CLIENTE_DEUDOR CL ON CD.CLDE_CODIGO=CL.CLDE_CODIGO ";
                    //    casos += "where CL.CPCE_CODIGO=" + ViewState["codigoCPCE"].ToString() + " and CD.ctde_gestorasignado=";
                    //    casos += ddlGestores.SelectedValue + " and CD.ctde_estado=1 and CL.clde_estado=1) res) tabla ";
                    //    casos += "where isnull(rango_dias,'')!='' ";
                    //    casos += "group by rango_dias order by rango_dias";
                    //    sql = sql + casos;
                    //    dts = new ConsultaDatosDTO().funConsultaDatos(15, 0, 0, 0, sql, "", "", Session["Conectar"].ToString());
                    //    ViewState["BrenchDet"] = dts.Tables[0];
                    //    grdvBrenchDet.DataSource = dts;
                    //    grdvBrenchDet.DataBind();
                    //}
                }
                else new FuncionesDAO().FunShowJSMessage("No existe Brench Creado para el Cedente..!", this);
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
                    DdlCedente.DataSource = new CedenteDAO().FunGetCedentes();
                    DdlCedente.DataTextField = "Descripcion";
                    DdlCedente.DataValueField = "Codigo";
                    DdlCedente.DataBind();
                    itemC.Text = "--Seleccione Catálago/Producto--";
                    itemC.Value = "0";
                    DdlCatalogo.Items.Add(itemC);
                    itemG.Text = "--Seleccione Gestor--";
                    itemG.Value = "0";
                    DdlGestores.Items.Add(itemG);
                    break;
                case 1:
                    dtbBrenchDet.Clear();
                    ViewState["BrenchDet"] = dtbBrench;
                    dtbBrench.Clear();
                    ViewState["BrenchCab"] = dtbBrench;
                    GrdvBrenchCab.DataSource = dtbBrench;
                    GrdvBrenchCab.DataBind();
                    GrdvBrenchDet.DataSource = dtbBrenchDet;
                    GrdvBrenchDet.DataBind();

                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();

                    dts = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", int.Parse(DdlCedente.SelectedValue), 
                        0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestores.DataSource = dts;
                    DdlGestores.DataTextField = "Descripcion";
                    DdlGestores.DataValueField = "Codigo";
                    DdlGestores.DataBind();
                    break;
                case 2:
                    dtbBrenchDet.Clear();
                    ViewState["BrenchDet"] = dtbBrench;
                    dtbBrench.Clear();
                    ViewState["BrenchCab"] = dtbBrench;
                    GrdvBrenchCab.DataSource = dtbBrench;
                    GrdvBrenchCab.DataBind();
                    GrdvBrenchDet.DataSource = dtbBrenchDet;
                    GrdvBrenchDet.DataBind();
                    dts = new CedenteDAO().FunGetBrenchDet(int.Parse(DdlCedente.SelectedValue), int.Parse(ViewState["codigoCPCE"].ToString()));
                    GrdvBrenchCab.DataSource = dts;
                    GrdvBrenchCab.DataBind();
                    ViewState["BrenchCab"] = dts.Tables[0];
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(1);
                dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                if (dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["codigoCEDE"] = DdlCedente.SelectedValue;
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
                    ViewState["codigoCPCE"] = DdlCatalogo.SelectedValue;
                    FunCargarCombos(2);
                }
                else
                {
                    DdlCatalogo.Items.Clear();
                    itemC.Text = "--Seleccione Catálago/Producto--";
                    itemC.Value = "0";
                    DdlCatalogo.Items.Add(itemC);
                    ViewState["codigoCPCE"] = "0";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["codigoCPCE"] = DdlCatalogo.SelectedValue;
            FunCargarCombos(2);
        }

        protected void DdlGestores_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargaMantenimiento();
        }

        protected void ImgModificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtPresupuesto.Text.Trim()) || TxtPresupuesto.Text.Trim() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese Presupuesto..!", this);
                    return;
                }

                //vPorcentaje = Math.Round(decimal.Parse(TxtPresupuesto.Text.Trim(),CultureInfo.InvariantCulture), 2);
                vExigible = Math.Round(decimal.Parse(ViewState["Exigible"].ToString()), 2);

                //vPresupuesto = Math.Round((vExigible * vPorcentaje) / 100, 2);

                vPresupuesto = decimal.Parse(TxtPresupuesto.Text.Trim(), CultureInfo.InvariantCulture);
                vPorcentaje = Math.Round((vPresupuesto / vExigible) * 100, 2);

                dtbBrenchDet = (DataTable)ViewState["BrenchDet"];
                resultado = dtbBrenchDet.Select("CodigoAlter='" + ViewState["Codigo"].ToString() + "'").FirstOrDefault();
                resultado["Porcentaje"] = vPorcentaje;
                resultado["Presupuesto"] = vPresupuesto.ToString("N2");
                resultado["PorcentajeValor"] = vPorcentaje;
                resultado["PresupuestoValor"] = vPresupuesto;
                dtbBrench = (DataTable)ViewState["BrenchCab"];
                result = dtbBrench.Select("Etiqueta='" + resultado["Rango"].ToString() + "'").FirstOrDefault();
                resultado["RangoInicial"] = result["RangoIni"].ToString();
                resultado["RangoFinal"] = result["RangoFin"].ToString();
                resultado["Orden"] = result["Orden"].ToString();
                dtbBrenchDet.AcceptChanges();
                ViewState["BrenchDet"] = dtbBrenchDet;
                GrdvBrenchDet.DataSource = dtbBrenchDet;
                GrdvBrenchDet.DataBind();
                //TxtPresupuesto.Text = "";
                TxtPresupuesto.Enabled = false;
                ImgModificar.Enabled = false;
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
                foreach (GridViewRow fr in GrdvBrenchDet.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvBrenchDet.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Beige;
                ViewState["Codigo"] = int.Parse(GrdvBrenchDet.DataKeys[gvRow.RowIndex].Values["CodigoAlter"].ToString());
                dtbBrench = (DataTable)ViewState["BrenchDet"];
                resultado = dtbBrench.Select("CodigoAlter='" + ViewState["Codigo"].ToString() + "'").FirstOrDefault();
                TxtPresupuesto.Text = resultado["Presupuesto"].ToString();
                ViewState["Exigible"] = resultado["ExigibleValor"].ToString();
                TxtPresupuesto.Enabled = true;
                ImgModificar.Enabled = true;
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
                dtbBrenchDet = (DataTable)ViewState["BrenchDet"];
                continuar = false;
                foreach (DataRow dr in dtbBrenchDet.Rows)
                {
                    if (dr["Presupuesto"].ToString() == "0.00")
                    {
                        new FuncionesDAO().FunShowJSMessage("Debe registrar todos los Presupuestos..!", this);
                        continuar = false;
                        break;
                    }
                    else continuar = true;
                }
                if (continuar)
                {
                    _mensaje = new ConsultaDatosDAO().FunProcesoBrenchGestor(1, 0, 0, int.Parse(DdlGestores.SelectedValue),
                        0, decimal.Parse(TxtPresupuesto.Text.Trim(), CultureInfo.InvariantCulture), "",
                        int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(),
                        "", "", "", "", "", int.Parse(DdlCatalogo.SelectedValue), int.Parse(ViewState["Anio"].ToString()),
                        int.Parse(ViewState["Mes"].ToString()), 0, 0, Session["Conectar"].ToString());

                    SoftCob_BRENCHMESCAB dato1 = new SoftCob_BRENCHMESCAB();
                    {
                        dato1.BRMC_CODIGO = int.Parse(ViewState["CodigoBRMC"].ToString());
                        dato1.brmc_cedecodigo = int.Parse(DdlCedente.SelectedValue);
                        dato1.brmc_cpcecodigo = int.Parse(DdlCatalogo.SelectedValue);
                        dato1.brmc_gestorasignado = int.Parse(DdlGestores.SelectedValue);
                        dato1.brmc_presupuestoanio = int.Parse(ViewState["Anio"].ToString());
                        dato1.brmc_presupuestomes = int.Parse(ViewState["Mes"].ToString());
                        dato1.brmc_presumeslabel = ViewState["MesName"].ToString();
                        dato1.brmc_totalmonto = decimal.Parse(ViewState["TotalMonto"].ToString());
                        dato1.brmc_totalexigible = decimal.Parse(ViewState["TotalExigible"].ToString());
                        dato1.brmc_presuporcentaje = decimal.Parse(ViewState["TotalPorcentaje"].ToString());
                        dato1.brmc_presupuestototal = decimal.Parse(ViewState["TotalPresupuesto"].ToString());
                        dato1.brmc_presupuestofecha = DateTime.Now;
                        dato1.brmc_presuporcencumplido = 0;
                        dato1.brmc_presutotalcumplido = 0;
                        dato1.brmc_presupuestogenerado = true;
                        dato1.brmc_fechacierre = DateTime.Now;
                        dato1.brmc_usuariocierre = 0;
                        dato1.brmc_terminalcierre = "";
                        dato1.brmc_auxv1 = "";
                        dato1.brmc_auxv2 = "";
                        dato1.brmc_auxv3 = "";
                        dato1.brmc_auxi1 = int.Parse(ViewState["TotalOperaciones"].ToString());
                        dato1.brmc_auxi2 = 0;
                        dato1.brmc_auxi3 = 0;
                        dato1.brmc_auxd1 = 0;
                        dato1.brmc_auxd2 = 0;
                        dato1.brmc_auxd3 = 0;
                        dato1.brmc_auxf1 = DateTime.Now;
                        dato1.brmc_auxf2 = DateTime.Now;
                        dato1.brmc_auxf3 = DateTime.Now;
                        dato1.brmc_fechacreacion = DateTime.Now;
                        dato1.brmc_usuariocreacion = int.Parse(Session["usuCodigo"].ToString());
                        dato1.brmc_terminalcreacion = Session["MachineName"].ToString();
                        dato1.brmc_fum = DateTime.Now;
                        dato1.brmc_uum = int.Parse(Session["usuCodigo"].ToString());
                        dato1.brmc_tum = Session["MachineName"].ToString();
                    }

                    List<SoftCob_BRENCHMESDET> dato2 = new List<SoftCob_BRENCHMESDET>();
                    foreach (DataRow dr in dtbBrenchDet.Rows)
                    {
                        dato2.Add(new SoftCob_BRENCHMESDET()
                        {
                            BRMD_CODIGO = int.Parse(dr["Codigo"].ToString()),
                            BRMC_CODIGO = int.Parse(ViewState["CodigoBRMC"].ToString()),
                            brmd_operaciones = int.Parse(dr["Operaciones"].ToString()),
                            brmd_montototal = decimal.Parse(dr["MontoValor"].ToString()),
                            brmd_totalexigible = decimal.Parse(dr["ExigibleValor"].ToString()),
                            brmd_porcentaje = decimal.Parse(dr["PorcentajeValor"].ToString()),
                            brmd_presupuesto = decimal.Parse(dr["PresupuestoValor"].ToString()),
                            brmd_brench = dr["Rango"].ToString(),
                            brmd_rangoinicial = int.Parse(dr["RangoInicial"].ToString()),
                            brmd_rangofinal = int.Parse(dr["RangoFinal"].ToString()),
                            brmd_orden = int.Parse(dr["Orden"].ToString()),
                            brmd_auxv1 = "",
                            brmd_auxv2 = "",
                            brmd_auxv3 = "",
                            brmd_auxi1 = 0,
                            brmd_auxi2 = 0,
                            brmd_auxi3 = 0
                        });
                    }
                    dato1.SoftCob_BRENCHMESDET = new List<SoftCob_BRENCHMESDET>();

                    foreach (SoftCob_BRENCHMESDET addDatos in dato2)
                    {
                        dato1.SoftCob_BRENCHMESDET.Add(addDatos);
                    }

                    if (ViewState["CodigoBRMC"].ToString() == "0") new CedenteDAO().FunCrearBrenchDet(dato1);
                    else new CedenteDAO().FunEditBrenchDet(dato1);

                    redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Exito..!");
                    Response.Redirect(redirect, true);
                }
                else new FuncionesDAO().FunShowJSMessage("No Existe Datos para el Brench..!", this);
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

        protected void GrdvBrenchDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    ImgSelecc = (ImageButton)(e.Row.Cells[5].FindControl("imgSelecc"));
                    if (ViewState["Procesado"].ToString() == "SI")
                    {
                        ImgSelecc.Enabled = false;
                        ImgSelecc.ImageUrl = "~/Botones/editargris.png";
                    }
                    else
                    {
                        ImgSelecc.Enabled = true;
                        ImgSelecc.ImageUrl = "~/Botones/selecc.png";
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    tMonto += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "MontoValor"));
                    tExigible += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ExigibleValor"));
                    //tPorcentaje += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Porcentaje"));
                    tPresupuesto += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PresupuestoValor"));
                    tOperaciones += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Operaciones"));
                    ViewState["TotalMonto"] = tMonto.ToString();
                    ViewState["TotalExigible"] = tExigible.ToString();
                    //ViewState["TotalPorcentaje"] = tPorcentaje.ToString();
                    ViewState["TotalOperaciones"] = tOperaciones.ToString();
                    ViewState["TotalPresupuesto"] = tPresupuesto.ToString();
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    tPorcentaje = Math.Round((tPresupuesto / tExigible) * 100, 2);
                    ViewState["TotalPorcentaje"] = tPorcentaje.ToString();
                    e.Row.Cells[0].Text = "Total:";
                    e.Row.Cells[1].Text = tOperaciones.ToString();
                    e.Row.Cells[2].Text = tMonto.ToString("c");
                    e.Row.Cells[3].Text = tExigible.ToString("c");
                    e.Row.Cells[4].Text = tPorcentaje.ToString();
                    e.Row.Cells[5].Text = tPresupuesto.ToString("c");
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}