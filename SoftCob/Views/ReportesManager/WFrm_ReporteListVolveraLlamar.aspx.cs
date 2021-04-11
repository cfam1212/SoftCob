namespace SoftCob.Views.ReportesManager
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ReporteListVolveraLlamar : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        ListItem _itemc = new ListItem();
        ListItem _itemg = new ListItem();
        string _tipo = "0";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TxtFechaIni.Text = DateTime.Now.ToString("MM/dd/yyyy");
                TxtFechaFin.Text = DateTime.Now.ToString("MM/dd/yyyy");
                LblTitulo.Text = "Reporte Seguimiento << VOLVER A LLAMAR >> ";
                FunCargarCombos(0);
            }
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

                    _itemg.Text = "--Todos--";
                    _itemg.Value = "0";
                    DdlGestor.Items.Add(_itemg);

                    break;
                case 1:
                    DdlGestor.Items.Clear();
                    _itemg.Text = "--Todos--";
                    _itemg.Value = "0";
                    DdlGestor.Items.Add(_itemg);
                    DdlGestor.DataSource = new ControllerDAO().FunGetConsultasCatalogo(12, "--Todos--", 
                        int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "", Session["Conectar"].ToString());
                    DdlGestor.DataTextField = "Descripcion";
                    DdlGestor.DataValueField = "Codigo";
                    DdlGestor.DataBind();
                    break;
                case 2:
                    _dts = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataSource = _dts;
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
                FunCargarCombos(1);
                FunCargarCombos(2);
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
            }
        }

        protected void ChkFecha_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkFecha.Checked) ChkFecha.Text = "Fecha Llamar";
            else ChkFecha.Text = "Fecha Registro";
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ChkFecha.Checked && DdlGestor.SelectedValue == "0") _tipo = "0";
                if (!ChkFecha.Checked && DdlGestor.SelectedValue != "0") _tipo = "1";
                if (ChkFecha.Checked && DdlGestor.SelectedValue == "0") _tipo = "2";
                if (ChkFecha.Checked && DdlGestor.SelectedValue != "0") _tipo = "3";

                Response.Redirect("WFrm_ListLlamarFixed.aspx?CodigoCEDE=" + DdlCedente.SelectedValue + "&CodigoCEDE=" + DdlCedente.SelectedValue + "&CodigoCPCE=" + DdlCatalogo.SelectedValue + "&FechaDesde=" + TxtFechaIni.Text.Trim() + "&FechaHasta=" + TxtFechaFin.Text + "&Gestor=" + DdlGestor.SelectedValue + "&Tipo=" + _tipo, true);
            }
            catch (Exception ex)
            {
                LblError.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}