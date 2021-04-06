

namespace SoftCob.Views.Configuraciones
{
    using ControllerSoftCob;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    public partial class WFrm_HorariosAdmin : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        string _codigo = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                ViewState["Conectar"] = ConfigurationManager.AppSettings["SqlConn"];
                Lbltitulo.Text = "Administrar Horarios";
                FunCargarMantenimiento();
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, ":: SoftCob ::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargarMantenimiento()
        {
            try
            {
                _dts = new ConsultaDatosDAO().FunHorariobpm(0, 0, "", "", "", "", "", "", 0, "", "", "", 0, 0, 0,
                    int.Parse((Session["usuCodigo"].ToString())), Session["MachineName"].ToString(),
                    ViewState["Conectar"].ToString());

                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();

                if (_dts.Tables[0].Rows.Count > 0)
                {
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
            Response.Redirect("WFrm_NuevoHorario.aspx?Codigo=0", true);
        }

        protected void Btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow _gvrow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigo = GrdvDatos.DataKeys[_gvrow.RowIndex].Values["Codigo"].ToString();
            Response.Redirect("WFrm_NuevoHorario.aspx?Codigo=" + _codigo, true);
        }
        #endregion
    }
}