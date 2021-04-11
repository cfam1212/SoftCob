namespace SoftCob.Views.ConsultasManager
{
    using ClosedXML.Excel;
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ConsultaAccion : Page
    {
        #region Variable
        DataSet _dts = new DataSet();
        ListItem _itemc = new ListItem();
        int _opcion = 0;
        DataTable _dtb = new DataTable();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ImgExportar);

            if (!IsPostBack)
            {
                Lbltitulo.Text = "Consulta de Accion-Gestión";
                FunCargarCombos(0);
            }
            else GrdvDatos.DataSource = Session["grdvDatos"];
        }
        #endregion

        #region Funciones y Procedimientos
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
                _dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoCedente"] = DdlCedente.SelectedValue;
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
                    ViewState["CodCatalogo"] = DdlCatalogo.SelectedValue;
                    FunCargarCombos(2);
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

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ImgExportar.Visible = false;
                LblExportar.Visible = false;

                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this);
                    return;
                }

                if (DdlBuscarPor.SelectedItem.ToString() != "Todo")
                {
                    if (string.IsNullOrEmpty(TxtBuscarPor.Text))
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese dato de busqueda..!", this);
                        return;
                    }
                }

                GrdvDatos.DataSource = null;
                GrdvDatos.DataBind();

                if (DdlAccion.SelectedValue == "0" && DdlBuscarPor.SelectedValue == "0") _opcion = 128;

                if (DdlAccion.SelectedValue != "0" && DdlBuscarPor.SelectedValue == "0") _opcion = 129;

                if (DdlAccion.SelectedValue == "0" && DdlBuscarPor.SelectedValue != "0") _opcion = 130;

                if (DdlAccion.SelectedValue != "0" && DdlBuscarPor.SelectedValue != "0") _opcion = 131;

                _dts = new ConsultaDatosDAO().FunConsultaDatos(_opcion, int.Parse(DdlCatalogo.SelectedValue), 0, 0, "", DdlAccion.SelectedItem.ToString(), TxtBuscarPor.Text.Trim(), Session["Conectar"].ToString());

                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
                Session["grdvDatos"] = _dts.Tables[0];

                if (_dts.Tables[0].Rows.Count > 0)
                {
                    ImgExportar.Visible = true;
                    LblExportar.Visible = true;
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
                _dtb = (DataTable)Session["grdvDatos"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dtb, "Datos");
                    string FileName = "ConsultaAccion_" + DdlCedente.SelectedItem.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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
        protected void DdlBuscarPor_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtBuscarPor.Text = "";
            TxtBuscarPor.Enabled = true;
            switch (DdlBuscarPor.SelectedValue)
            {
                case "0":
                    TxtBuscarPor.Enabled = false;
                    break;
            }
        }

        protected void GrdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdvDatos.PageIndex = e.NewPageIndex;
            GrdvDatos.DataBind();
        }
        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }

        #endregion
    }
}