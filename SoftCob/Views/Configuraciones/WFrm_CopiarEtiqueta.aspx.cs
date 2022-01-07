namespace SoftCob.Views.Configuraciones
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_CopiarEtiqueta : Page
    {
        #region Varibales
        DataSet _dts = new DataSet();
        ListItem _itemc = new ListItem();
        string _redirect = "", _mensaje = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Lbltitulo.Text = "Copiar Etiqueta Presupuesto";
                FunCargarCombos(0);

                if (Request["MensajeRetornado"] != null)
                {
                    _mensaje = Request["MensajeRetornado"];
                    new FuncionesDAO().FunShowJSMessage(_mensaje, this, "S", "C");
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    DdlCedenteO.DataSource = new CedenteDAO().FunGetCedentes();
                    DdlCedenteO.DataTextField = "Descripcion";
                    DdlCedenteO.DataValueField = "Codigo";
                    DdlCedenteO.DataBind();

                    _itemc.Text = "--Seleccione Catálago/Producto--";
                    _itemc.Value = "0";
                    DdlCatalogoO.Items.Add(_itemc);
                    DdlCatalogoD.Items.Add(_itemc);

                    DdlCedenteD.DataSource = new CedenteDAO().FunGetCedentes();
                    DdlCedenteD.DataTextField = "Descripcion";
                    DdlCedenteD.DataValueField = "Codigo";
                    DdlCedenteD.DataBind();
                    break;
                case 1:
                    DdlCatalogoO.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedenteO.SelectedValue));
                    DdlCatalogoO.DataTextField = "CatalogoProducto";
                    DdlCatalogoO.DataValueField = "CodigoCatalogo";
                    DdlCatalogoO.DataBind();
                    break;
                case 2:
                    DdlCatalogoD.DataSource = new CedenteDAO().FunGetCatalogoProducto(int.Parse(DdlCedenteD.SelectedValue));
                    DdlCatalogoD.DataTextField = "CatalogoProducto";
                    DdlCatalogoD.DataValueField = "CodigoCatalogo";
                    DdlCatalogoD.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void DdlCedenteO_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(1);
        }

        protected void DdlCedenteD_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCargarCombos(2);
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DdlCedenteD.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente Origen..!", this, "W", "C");
                    return;
                }

                if (DdlCedenteO.SelectedValue == "0")
                {
                    new FuncionesDAO().FunShowJSMessage("Seleccione Cedente Destino..!", this, "W", "C");
                    return;
                }

                if (DdlCatalogoO.SelectedValue == DdlCatalogoD.SelectedValue)
                {
                    new FuncionesDAO().FunShowJSMessage("No se puede Copiar Al mismo Catálogo/Producto..!", this, "E", "C");
                    return;
                }

                _dts = new ConsultaDatosDAO().FunConsultaDatos(259, int.Parse(DdlCatalogoO.SelectedValue),
                    int.Parse(DdlCedenteD.SelectedValue), int.Parse(DdlCatalogoD.SelectedValue), "",
                    Session["MachineName"].ToString(), "ETIQUETA", Session["Conectar"].ToString());

                if (_dts.Tables[0].Rows[0][0].ToString() == "NO")
                {
                    new FuncionesDAO().FunShowJSMessage("Cedente Origen NO tiene creado ETIQUETA", this, "E", "C");
                    return;
                }

                _redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Copiado con Éxito..!");
                Response.Redirect(_redirect, true);
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