namespace SoftCob.Views.Breanch
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class wFrm_ProcesarPagosBrench : Page
    {
        #region Variables
        ListItem itemC = new ListItem();
        ListItem itemG = new ListItem();
        DataSet dts = new DataSet();
        //string sql = "", estrategia = "", ordenar = "", fechaactual = "", mensaje = "";
        int contador = 0;
        string redirect = "", mensaje = "";
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
                    Response.Redirect("WFrm_GestionListaTrabajo.aspx?IdListaCabecera=" + Session["IdListaCabecera"].ToString(), true);
                    return;
                }

                ViewState["Anio"] = DateTime.Now.Year.ToString();
                ViewState["Mes"] = DateTime.Now.ToString("MM");
                ViewState["MesName"] = DateTime.Now.ToString("MMMM").ToUpper();
                TxtFechaProceso.Text = DateTime.Now.ToString("MM/dd/yyyy");
                FunCargarCombos(0);
                Lbltitulo.Text = "Generar Pagos para BRENCH";

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargaMantenimiento()
        {
            //try
            //{
            //    dtbBrenchDet.Clear();
            //    ViewState["BrenchDet"] = dtbBrenchDet;
            //    grdvBrenchDet.DataSource = dtbBrenchDet;
            //    grdvBrenchDet.DataBind();
            //    dtbBrench = (DataTable)ViewState["BrenchCab"];
            //    dts = new ConsultaDatosDTO().funConsultaDatos(116, int.Parse(ddlCedente.SelectedValue), int.Parse(ddlCatalogo.SelectedValue),
            //        int.Parse(ddlGestores.SelectedValue), "", "", "", Session["Conectar"].ToString());
            //    if (dts.Tables[0].Rows.Count > 0)
            //    {
            //        alert = "Gestor Tiene Abierto Presupuesto Año: " + dts.Tables[0].Rows[0]["Anio"].ToString() + " Mes: " +
            //            dts.Tables[0].Rows[0]["Mes"].ToString() + " Por $" + dts.Tables[0].Rows[0]["Exigible"].ToString();
            //        new FuncionesDAO().funShowJSMessage(alert, this);
            //        return;
            //    }
            //    if (dtbBrench.Rows.Count > 0)
            //    {
            //        dts = new ConsultaDatosDTO().funConsultaDatos(111, int.Parse(ddlCedente.SelectedValue), int.Parse(ddlCatalogo.SelectedValue),
            //            int.Parse(ddlGestores.SelectedValue), "", ViewState["Anio"].ToString(), ViewState["Mes"].ToString(), Session["Conectar"].ToString());
            //        if (dts.Tables[0].Rows.Count > 0)
            //        {
            //            ViewState["Procesado"] = "SI";
            //            ViewState["BrenchDet"] = dts.Tables[0];
            //            grdvBrenchDet.DataSource = dts;
            //            grdvBrenchDet.DataBind();
            //            btnGrabar.Enabled = false;
            //        }
            //        else
            //        {
            //            ViewState["Procesado"] = "NO";
            //            btnGrabar.Enabled = true;
            //            sql = "select Codigo = ROW_NUMBER() over(order by rango_dias),";
            //            sql += "Operaciones = count(*),Rango = rango_dias,";
            //            sql += "Monto = cast(convert(varchar(20),cast(sum(ctde_totaldeuda) as money),1) as varchar),";
            //            sql += "Exigible = cast(convert(varchar(20),cast(sum(ctde_valorexigible) as money),1) as varchar),";
            //            sql += "Porcentaje = '0.00',";
            //            sql += "Presupuesto = '0.00',";
            //            sql += "PorcentajeValor=0.00,PresupuestoValor=0.00,ExigibleValor = sum(ctde_valorexigible),MontoValor = sum(ctde_totaldeuda),";
            //            sql += "RangoInicial = 0,RangoFinal = 0,Orden = 0 from (select case";
            //            dtbBrench = (DataTable)ViewState["BrenchCab"];
            //            foreach (DataRow row in dtbBrench.Rows)
            //            {
            //                casos += " when dias_mora between " + row["RangoIni"].ToString() + " and " + row["RangoFin"].ToString() +
            //                    " then '" + row["Etiqueta"].ToString() + "'";
            //            }
            //            casos += " end as rango_dias,ctde_valorexigible,ctde_totaldeuda from (";
            //            casos += "select CD.ctde_totaldeuda, CD.ctde_valorexigible,CD.ctde_diasmora as dias_mora ";
            //            casos += "from SoftCob_CUENTA_DEUDOR CD INNER JOIN SoftCob_CLIENTE_DEUDOR CL ON CD.CLDE_CODIGO=CL.CLDE_CODIGO ";
            //            casos += "where CL.CPCE_CODIGO=" + ViewState["CodigoCPCE"].ToString() + " and CD.ctde_gestorasignado=";
            //            casos += ddlGestores.SelectedValue + " and CD.ctde_estado=1 and CL.clde_estado=1) res) tabla ";
            //            casos += "where isnull(rango_dias,'')!='' ";
            //            casos += "group by rango_dias order by rango_dias";
            //            sql = sql + casos;
            //            dts = new ConsultaDatosDTO().funConsultaDatos(15, 0, 0, 0, sql, "", "", Session["Conectar"].ToString());
            //            ViewState["BrenchDet"] = dts.Tables[0];
            //            grdvBrenchDet.DataSource = dts;
            //            grdvBrenchDet.DataBind();
            //        }
            //    }
            //    else new FuncionesDAO().funShowJSMessage("No existe Brench Creado para el Cedente..!", this);
            //}
            //catch (Exception ex)
            //{
            //    lblerror.Text = ex.ToString();
            //}
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
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();

                    dts = new ControllerDAO().FunGetConsultasCatalogo(12, "--Seleccione Gestor--", 
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestores.DataSource = dts;
                    DdlGestores.DataTextField = "Descripcion";
                    DdlGestores.DataValueField = "Codigo";
                    DdlGestores.DataBind();
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
                    ViewState["CodigoCEDE"] = DdlCedente.SelectedValue;
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
                    ViewState["CodigoCPCE"] = DdlCatalogo.SelectedValue;
                    FunCargarCombos(2);
                }
                else
                {
                    DdlCatalogo.Items.Clear();
                    itemC.Text = "--Seleccione Catálago/Producto--";
                    itemC.Value = "0";
                    DdlCatalogo.Items.Add(itemC);
                    ViewState["CodigoCPCE"] = "0";
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["CodigoCPCE"] = DdlCatalogo.SelectedValue;
            FunCargarCombos(1);
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlCatalogo.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catálogo..!", this);
                    return;
                }

                if (!new FuncionesDAO().IsDate(TxtFechaProceso.Text))
                {
                    new FuncionesDAO().FunShowJSMessage("Fecha Incorrecta..!", this);
                    return;
                }

                dts = new ConsultaDatosDAO().FunConsultaDatos(133, 0, int.Parse(ViewState["CodigoCEDE"].ToString()),
                    int.Parse(ViewState["CodigoCPCE"].ToString()), "", "", "", Session["Conectar"].ToString());

                if (dts.Tables[0].Rows.Count == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("No existen pagos registrados en la fecha..!", this);
                    return;
                }

                foreach (DataRow dr in dts.Tables[0].Rows)
                {
                    dts = new PagoCarteraDAO().FunGetPagoCartera(14, int.Parse(ViewState["CodigoCEDE"].ToString()), int.Parse(ViewState["CodigoCPCE"].ToString()),
                        "", dr["Operacion"].ToString(), "", dr["FechaPago"].ToString(), dr["ValorPago"].ToString().Replace(".", ","),
                        TxtFechaProceso.Text.Trim(), ViewState["MesName"].ToString(), "", "",
                        int.Parse(ViewState["Anio"].ToString()), int.Parse(ViewState["Mes"].ToString()), 0,
                        int.Parse(Session["usuCodigo"].ToString()), Session["MachineName"].ToString(), Session["Conectar"].ToString());
                    contador++;
                }

                if (contador == 0) mensaje = "No Existen Brench Creados..!";
                else mensaje = "Guardado con Exito..!";

                redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, mensaje);
                Response.Redirect(redirect, true);
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