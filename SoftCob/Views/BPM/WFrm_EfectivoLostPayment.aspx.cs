

namespace SoftCob.Views.BPM
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.IO;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class WFrm_EfectivoLostPayment : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        DataTable _dtbproyecc = new DataTable();
        ListItem _itemc = new ListItem();
        ListItem _itemg = new ListItem();
        int _codigobrmc = 0, _mes = 0, _year = 0, _codigoproy = 0, _codigo = 0;
        string _meslabel = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.ImgExportar);

                if (!IsPostBack)
                {
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                    Lbltitulo.Text = "EFECTIVIZAR PROYECCION";
                    FunCargarCombos(0);
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
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", ViewState["Conectar"].ToString());
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

                    if (_dts.Tables[0].Rows.Count == 0)
                    {
                        _itemc.Text = "--Seleccione Catálago/Producto--";
                        _itemc.Value = "0";
                        DdlCatalogo.Items.Add(_itemc);
                    }
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FunCargarCombos(2);
                FunCargarCombos(1);
                GrdvLost.DataSource = null;
                GrdvLost.DataBind();
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
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlCatalogo.SelectedValue.ToString() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Catalogo/Producto..!", this);
                    return;
                }

                if (DdlGestor.SelectedValue.ToString() == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Gestor..!", this);
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(227, int.Parse(DdlGestor.SelectedValue),
                    0, 0, "", "", "", ViewState["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    _codigobrmc = int.Parse(_dts.Tables[0].Rows[0]["CodigoBRMC"].ToString());

                    _dts = new ConsultaDatosDAO().FunConsultaDatos(228, _codigobrmc, 0, 0, "", "", "",
                        ViewState["Conectar"].ToString());

                    _meslabel = _dts.Tables[0].Rows[0]["MesLabel"].ToString();
                    _mes = int.Parse(_dts.Tables[0].Rows[0]["Mes"].ToString());
                    _year = int.Parse(_dts.Tables[0].Rows[0]["Anio"].ToString());

                    Lbltitulo.Text = "EFECTIVIZAR PROYECCION " + _meslabel + " " + _year.ToString();
                    DivPagos.Visible = false;

                    _dts = new PagoCarteraDAO().FunGetPagoCartera(23, int.Parse(DdlCedente.SelectedValue),
                        int.Parse(DdlCatalogo.SelectedValue), "", "", "", "", "", "PPR", "", "", "", _year, _mes,
                        int.Parse(DdlGestor.SelectedValue), 0, "", ViewState["Conectar"].ToString());

                    ViewState["PagosLost"] = _dts.Tables[0];
                    GrdvLost.DataSource = _dts.Tables[0];
                    GrdvLost.DataBind();

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        DivPagos.Visible = true;
                    }
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
                _dtb = (DataTable)ViewState["PagosLost"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    string FileName = "Reporte_Lost_" + DdlGestor.SelectedItem.ToString() + "-" +
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

        protected void ImgEfectivo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvLost.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvLost.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigoproy = int.Parse(GrdvLost.DataKeys[gvRow.RowIndex].Values["CodigoPROY"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(245, int.Parse(Session["usuCodigo"].ToString()),
                    _codigoproy, 0, "", "PGR", "", ViewState["Conectar"].ToString());

                _dts = new PagoCarteraDAO().FunGetPagoCartera(23, int.Parse(DdlCedente.SelectedValue),
                    int.Parse(DdlCatalogo.SelectedValue), "", "", "", "", "", "PPR", "", "", "", _year, _mes,
                    int.Parse(DdlGestor.SelectedValue), 0, "", ViewState["Conectar"].ToString());

                ViewState["PagosLost"] = _dts.Tables[0];
                GrdvLost.DataSource = _dts.Tables[0];
                GrdvLost.DataBind();

                if (_dts.Tables[0].Rows.Count == 0) DivPagos.Visible = false;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgGestiones_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                foreach (GridViewRow fr in GrdvLost.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }

                GrdvLost.Rows[_gvrow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
                _codigo = int.Parse(GrdvLost.DataKeys[_gvrow.RowIndex].Values["CodigoPERS"].ToString());

                ScriptManager.RegisterStartupScript(this, GetType(), "Visualizar", "javascript: var posicion_x; " +
                    "var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                    "window.open('../Breanch/WFrm_BrenchGestiones.aspx?CodigoCEDE=" + DdlCedente.SelectedValue +
                    "&CodigoCPCE=" + DdlCatalogo.SelectedValue + "&CodigoGEST=" + DdlGestor.SelectedValue +
                    "&CodigoPERS=" + _codigo + "',null,'left=' + posicion_x + " +
                    "', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, " +
                    "toolbar=no, location=no, menubar=no,titlebar=0');", true);
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
