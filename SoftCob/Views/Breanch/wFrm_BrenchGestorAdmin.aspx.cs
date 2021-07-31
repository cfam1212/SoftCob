
namespace SoftCob.Views.Breanch
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class wFrm_BrenchGestorAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        DataSet dtsX = new DataSet();
        DataTable dtbBrench = new DataTable();
        DataTable dtbPagos = new DataTable();
        DataTable _dtb = new DataTable();
        ListItem itemC = new ListItem();
        CheckBox _chkcierre = new CheckBox();
        ImageButton ImgVer = new ImageButton();
        int codigoGestor = 0, codigoBRMC = 0;
        DataRow resultado;
        DataRow[] resul;
        string mensaje = "", redirect = "", _fecha = "", _mensaje = "";
        decimal _totalexigible = 0.00M, _totalpagos = 0.00M, _porcumplido = 0.00M, _valpresupuesto = 0.00M;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");

            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ImgExportar);

            if (!IsPostBack)
            {
                TxtFechaPago.Text = DateTime.Now.ToString("MM/dd/yyyy");
                ViewState["Procesado"] = "NO";
                FunCargarCombos(0);
                ViewState["Anio"] = DateTime.Now.Year.ToString();
                ViewState["MesName"] = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture).ToUpper();
                ViewState["Mes"] = DateTime.Now.ToString("MM");
                Lbltitulo.Text = "Generar Brench Año: " + ViewState["Anio"].ToString() + " Mes: " + ViewState["MesName"].ToString();
                if (Request["MensajeRetornado"] != null)
                {
                    _mensaje = Request["MensajeRetornado"];
                    new FuncionesDAO().FunShowJSMessage(_mensaje, this, "S", "R");
                }
             
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargaMantenimiento()
        {
            try
            {
                pnlBrenchMes.Visible = false;
                ImgExportar.Visible = false;
                LblExportar.Visible = false;

                dts = new ConsultaDatosDAO().FunConsultaDatos(115, int.Parse(DdlCedente.SelectedValue), int.Parse(DdlCatalogo.SelectedValue),
                    0, "", "", "", Session["Conectar"].ToString());
                ViewState["BrenchGestor"] = dts.Tables[0];

                GrdvBrenchGestor.DataSource = dts;
                GrdvBrenchGestor.DataBind();

                if (dts.Tables[0].Rows.Count > 0)
                {
                    pnlBrenchMes.Visible = true;
                    ImgExportar.Visible = true;
                    LblExportar.Visible = true;
                }

                pnlDetalleBrench.Visible = false;
                GrdvBrenchRango.DataSource = null;
                GrdvBrenchRango.DataBind();
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
                    break;
                case 1:
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
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
                }
                else
                {
                    DdlCatalogo.Items.Clear();
                    itemC.Text = "--Seleccione Catálago/Producto--";
                    itemC.Value = "0";
                    DdlCatalogo.Items.Add(itemC);
                    ViewState["codigoCPCE"] = "0";
                }
                FunCargaMantenimiento();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargaMantenimiento();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void ImgExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _dtb = (DataTable)ViewState["BrenchGestor"];

                string[] columnas = new[] { "Gestor", "ExigibleValor","Porcentaje", "PresupuestoValor", "Fecha",
                    "PorCumplido", "Pagos"};

                //dtbPagos = _dtb;
                DataView view = new DataView(_dtb);
                dtbPagos = view.ToTable(true, columnas);

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtbPagos, "Datos");
                    string FileName = "Brench_" + DdlCatalogo.SelectedItem.ToString() + "_" +
                        ViewState["MesName"].ToString() + "_" + ViewState["Anio"].ToString() +
                        "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

        protected void GrdvBrenchRango_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowIndex >= 0)
                //{
                //    _fecha = GrdvBrenchGestor.DataKeys[e.Row.RowIndex].Values["Fecha"].ToString();
                //    _chkcierre = (CheckBox)(e.Row.Cells[9].FindControl("ChkCierre"));

                //    if (_fecha == "NUEVO") _chkcierre.Enabled = false;
                //}

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _valpresupuesto += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ValorPresupuesto"));
                    _totalpagos += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ValorCumplido"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (_valpresupuesto > 0)
                    {
                        _porcumplido = Math.Round((_totalpagos / _valpresupuesto) * 100, 2);
                    }

                    e.Row.Cells[0].Text = "TOTAL:";
                    e.Row.Cells[1].Text = "$ " + _valpresupuesto.ToString("N2");
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[3].Text = "$ " + _totalpagos.ToString("N2");
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[4].Text = _porcumplido.ToString("N2");
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvBrenchGestor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _fecha = GrdvBrenchGestor.DataKeys[e.Row.RowIndex].Values["Fecha"].ToString();
                    _chkcierre = (CheckBox)(e.Row.Cells[9].FindControl("ChkCierre"));

                    if (_fecha == "NUEVO") _chkcierre.Enabled = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    _totalexigible += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PresupuestoValor"));
                    _totalpagos += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Pagos"));
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (_totalexigible > 0)
                    {
                        _porcumplido = Math.Round((_totalpagos / _totalexigible) * 100, 2);
                    }

                    e.Row.Cells[2].Text = "TOTAL:";
                    e.Row.Cells[3].Text = "$ " + _totalexigible.ToString("N2");
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[6].Text = "$ " + _totalpagos.ToString("N2");
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;

                    e.Row.Cells[7].Text = _porcumplido.ToString("N2");
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Font.Bold = true;
                    e.Row.Font.Size = 8;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgVer_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                ImgVer = (ImageButton)(gvRow.Cells[9].FindControl("imgVer"));

                foreach (GridViewRow fr in GrdvBrenchGestor.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvBrenchGestor.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Turquoise;
                codigoGestor = int.Parse(GrdvBrenchGestor.DataKeys[gvRow.RowIndex].Values["CodigoGestor"].ToString());
                codigoBRMC = int.Parse(GrdvBrenchGestor.DataKeys[gvRow.RowIndex].Values["CodigoBRMC"].ToString());
                dtsX = new ConsultaDatosDAO().FunConsultaDatos(118, codigoBRMC, int.Parse(DdlCatalogo.SelectedValue), 0,
                    "", "", "", Session["Conectar"].ToString());
                pnlDetalleBrench.Visible = true;
                GrdvBrenchRango.DataSource = dtsX;
                GrdvBrenchRango.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkCierre_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _chkcierre = (CheckBox)(gvRow.Cells[9].FindControl("ChkCierre"));

                if (_chkcierre.Checked) GrdvBrenchGestor.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Beige;
                else GrdvBrenchGestor.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.White;

                codigoGestor = int.Parse(GrdvBrenchGestor.DataKeys[gvRow.RowIndex].Values["CodigoGestor"].ToString());
                dtbBrench = (DataTable)ViewState["BrenchGestor"];
                resultado = dtbBrench.Select("CodigoGestor='" + codigoGestor + "'").FirstOrDefault();
                resultado["Cierre"] = _chkcierre.Checked ? "SI" : "NO";
                dtbBrench.AcceptChanges();
                ViewState["BrenchGestor"] = dtbBrench;
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
                dtbBrench = (DataTable)ViewState["BrenchGestor"];
                resul = dtbBrench.Select("Cierre='" + "SI" + "'");

                if (resul.Count() == 0)
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione al Menos un Gestor para el Proceso..!", this, "W", "C");
                    return;
                }

                foreach (DataRow _drfila in resul)
                {
                    mensaje = new ConsultaDatosDAO().FunProcesoBrenchGestor(0, int.Parse(_drfila["CodigoBRMC"].ToString()),
                        0, int.Parse(_drfila["CodigoGestor"].ToString()), decimal.Parse(_drfila["PorCumplido"].ToString()),
                        decimal.Parse(_drfila["Pagos"].ToString()), TxtFechaPago.Text.Trim(), int.Parse(Session["usuCodigo"].ToString()),
                        Session["MachineName"].ToString(), "", "", "", "", "", int.Parse(DdlCatalogo.SelectedValue),
                        int.Parse(_drfila["Anio"].ToString()), int.Parse(_drfila["Mes"].ToString()),
                        int.Parse(_drfila["CodigoGestor"].ToString()), 0, Session["Conectar"].ToString());
                }

                redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Procesado Con Exito..!");
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