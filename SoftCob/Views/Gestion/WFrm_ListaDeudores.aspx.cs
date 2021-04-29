namespace SoftCob.Views.Gestion
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Configuration;
    using System.Web.UI.WebControls;
    public partial class WFrm_ListaDeudores : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");

                if (!IsPostBack)
                {
                    ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];

                    ViewState["CodigoCPCE"] = Request["CodigoCPCE"];
                    ViewState["CodigoGEST"] = Request["CodigoGEST"];

                    Lbltitulo.Text = "Lista Clientes-Deudor";
                    FunCargarDatos();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        public void FunCargarDatos()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunConsultaDatos(233, int.Parse(ViewState["CodigoCPCE"].ToString()),
                    int.Parse(ViewState["CodigoGEST"].ToString()), 0, "", "", "", ViewState["Conectar"].ToString());

                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

                Session["CedulaProyecc"] = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Cedula"].ToString();
                Session["ClienteProyecc"] = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Cliente"].ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "pop", "CloseWindow();", true);

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }
        #endregion
    }
}