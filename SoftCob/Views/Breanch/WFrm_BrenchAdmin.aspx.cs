namespace SoftCob.Views.Breanch
{
    using ControllerSoftCob;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_BrenchAdmin : Page
    {
        #region Variables
        string strCodigo = "", _mensaje = "";
        CheckBox chkEstado = new CheckBox();
        DataSet dts = new DataSet();

        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                Lbltitulo.Text = "Administrar Brench";
                FunCargarMantenimiento();
                
                if (Request["MensajeRetornado"] != null)
                {
                    _mensaje = Request["MensajeRetornado"];
                    new FuncionesDAO().FunShowJSMessage(_mensaje, this, "S", "R");
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                dts = new ListaTrabajoDAO().FunGetBrenchAdmin();

                if (dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = dts;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("WFrm_NuevoBrench.aspx?CodigoBrench=0", true);
        }
        protected void Btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                strCodigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                Response.Redirect("WFrm_NuevoBrench.aspx?CodigoBrench=" + strCodigo, true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}