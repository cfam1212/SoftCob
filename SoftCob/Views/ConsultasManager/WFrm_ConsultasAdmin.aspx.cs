namespace SoftCob.Views.ConsultasManager
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_ConsultasAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        string codigoCEDE = "", codigoCPCE = "", codigoCLDE = "", codigoPERS = "", buscaIde = "", buscaPer = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Lbltitulo.Text = "Consultas - Acciones Gestiones";
            }

            else GrdvDatos.DataSource = ViewState["GrdvDatos"];

        }
        #endregion

        #region Botones y Eventos
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtCriterio.Text.Trim()))
                {
                    new FuncionesDAO().FunShowJSMessage("Ingrese criterio de búsqueda..!", this, "N", "C");
                    return;
                }

                if (DdlBuscarPor.SelectedValue == "C")
                {
                    buscaPer = TxtCriterio.Text.Trim();
                    buscaIde = "";
                }
                else
                {
                    buscaPer = "";
                    buscaIde = TxtCriterio.Text.Trim();
                }

                if (ChkArbol.Checked)
                {
                    if (TxtCriterio.Text.Trim().Length < 10)
                    {
                        new FuncionesDAO().FunShowJSMessage("Dato tiene menos de 10 digitos..", this, "W", "C");
                        return;
                    }

                    Session["CedulaCookie"] = TxtCriterio.Text.Trim();
                    ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); " +
                        "posicion_y=(screen.height/2)-(600/2); window.open('../Gestion/WFrm_ArbolRecursivo.aspx?Cedula=" + TxtCriterio.Text.Trim() +
                        "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=950px, height=650px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
                }
                else
                {
                    dts = new ConsultaDatosDAO().FunConsultaDatos(95, 0, 0, 0, "", buscaIde, buscaPer, Session["Conectar"].ToString());
                    GrdvDatos.DataSource = dts;
                    GrdvDatos.DataBind();
                    ViewState["GrdvDatos"] = dts;
                    if (dts.Tables[0].Rows.Count > 0) DivDatos.Visible = true;
                    else DivDatos.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            codigoCEDE = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCEDE"].ToString();
            codigoCPCE = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCPCE"].ToString();
            codigoCLDE = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCLDE"].ToString();
            codigoPERS = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
            Response.Redirect("WFrm_AccionConsulta.aspx?codigoCEDE=" + codigoCEDE + "&codigoCPCE=" + codigoCPCE +
                "&codigoCLDE=" + codigoCLDE + "&codigoPERS=" + codigoPERS, true);
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