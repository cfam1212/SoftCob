namespace SoftCob.Views.Breanch
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_BrenchEfectivoOn : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        ListItem _itemc = new ListItem();
        ListItem _itemg = new ListItem();
        int _tipo = 0, _codigo = 0, _gestor = 0;
        CheckBox _chkefec = new CheckBox();
        string _operacion = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Lbltitulo.Text = "Modificar Efectividad de Pago << ON DEMAND >> ";
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

                    _itemg.Text = "--Seleccione Gestor--";
                    _itemg.Value = "0";
                    DdlGestores.Items.Add(_itemg);

                    break;
                case 1:
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(12, int.Parse(DdlCedente.SelectedValue), 0, 0, "", "", "",
                        Session["Conectar"].ToString());
                    DdlGestores.DataSource = _dts;
                    DdlGestores.DataTextField = "Descripcion";
                    DdlGestores.DataValueField = "Codigo";
                    DdlGestores.DataBind();

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
            FunCargarCombos(1);
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlCedente.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente..!", this, "N", "C");
                    return;
                }

                if (DdlTipoDocumento.SelectedValue != "0")
                {
                    if (string.IsNullOrEmpty(TxtDocumento.Text.Trim()))
                    {
                        new FuncionesDAO().FunShowJSMessage("Ingrese No. de Documento..!", this, "N", "C");
                        return;
                    }
                }

                if (DdlGestores.SelectedValue == "0" && DdlTipoDocumento.SelectedValue == "0") _tipo = 0;
                if (DdlGestores.SelectedValue != "0" && DdlTipoDocumento.SelectedValue == "0") _tipo = 1;
                if (DdlGestores.SelectedValue != "0" && DdlTipoDocumento.SelectedValue == "1") _tipo = 2;
                if (DdlGestores.SelectedValue != "0" && DdlTipoDocumento.SelectedValue == "2") _tipo = 3;
                if (DdlGestores.SelectedValue == "0" && DdlTipoDocumento.SelectedValue == "1") _tipo = 4;
                if (DdlGestores.SelectedValue == "0" && DdlTipoDocumento.SelectedValue == "2") _tipo = 5;

                _dts = new ConsultaDatosDAO().FunConsultaDatos(221, int.Parse(DdlCatalogo.SelectedValue), _tipo,
                    int.Parse(DdlGestores.SelectedValue), "", TxtDocumento.Text.Trim(), "", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows.Count > 0) DivPagos.Visible = true;
                else DivPagos.Visible = false;

                GrdvPagos.DataSource = _dts;
                GrdvPagos.DataBind();

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEfectivo_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

            _chkefec = (CheckBox)(_gvrow.Cells[5].FindControl("ChkEfectivo"));
            _codigo = int.Parse(GrdvPagos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());
            _gestor = int.Parse(GrdvPagos.DataKeys[_gvrow.RowIndex].Values["CodigoGEST"].ToString());

            _dts = new ConsultaDatosDAO().FunConsultaDatos(222, _codigo, _chkefec.Checked ? 1 : 0, _gestor,
                "", TxtDocumento.Text.Trim(), "", Session["Conectar"].ToString());
        }

        protected void ImgGestiones_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

            _gestor = int.Parse(GrdvPagos.DataKeys[_gvrow.RowIndex].Values["CodigoGEST"].ToString());
            _operacion = GrdvPagos.DataKeys[_gvrow.RowIndex].Values["Operacion"].ToString();

            ScriptManager.RegisterStartupScript(this, GetType(), "Visualizar", "javascript: var posicion_x; " +
                "var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); " +
                "window.open('../Breanch/WFrm_BrenchGestiones.aspx?CodigoCEDE=" + DdlCedente.SelectedValue +
                "&CodigoCPCE=" + DdlCatalogo.SelectedValue + "&CodigoGEST=" + _gestor + "&Operacion=" + _operacion + "',null,'left=' + posicion_x + " +
                "', top=' + posicion_y + ', width=850px, height=450px, status=no,resizable= yes, scrollbars=yes, " +
                "toolbar=no, location=no, menubar=no,titlebar=0');", true);

        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Mantenedor/WFrm_Detalle.aspx", true);
        }
        #endregion
    }
}