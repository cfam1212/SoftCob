namespace SoftCob.Views.Configuraciones
{
    using ControllerSoftCob;
    using System;
    using System.Web.UI;
    using System.Configuration;
    using System.Data;
    using System.Web.UI.WebControls;
    public partial class WFrm_ConfigurarAccionEfecto : Page
    {
        #region Varibales
        DataSet _dts = new DataSet();
        ListItem _itemc = new ListItem();
        CheckBox _chkselecc = new CheckBox();
        string _selecc = "";
        int _codigo = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Lbltitulo.Text = "Configurar Accion-Efecto (Generar Lista de Trabajo por Efecto)";
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
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

                    _itemc.Text = "--Seleccione Catálago/Producto--";
                    _itemc.Value = "0";
                    DdlCatalogo.Items.Add(_itemc);

                    break;
                case 1:
                    DdlCatalogo.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedente.SelectedValue));
                    DdlCatalogo.DataTextField = "CatalogoProducto";
                    DdlCatalogo.DataValueField = "CodigoCatalogo";
                    DdlCatalogo.DataBind();
                    FunCargarCombos(2);
                    break;
                case 2:
                    _dts = new ConsultaDatosDAO().FunConsultaDatos(146, int.Parse(DdlCatalogo.SelectedValue), 1, 0, "",
                        "", "", ViewState["Conectar"].ToString());
                    GrdvEfecto.DataSource = _dts;
                    GrdvEfecto.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCedente_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void DdlCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(2);
        }

        protected void ChkSelecc_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;

                _chkselecc = (CheckBox)(_gvrow.Cells[1].FindControl("ChkSelecc"));
                _codigo = int.Parse(GrdvEfecto.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString());

                _dts = new ConsultaDatosDAO().FunConsultaDatos(146, _codigo, 2, 0, "", _chkselecc.Checked ? "CP" : "", "",
                    ViewState["Conectar"].ToString());
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvEfecto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkselecc = (CheckBox)(e.Row.Cells[1].FindControl("ChkSelecc"));
                    _selecc = GrdvEfecto.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();

                    if (_selecc == "SI") _chkselecc.Checked = true;
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